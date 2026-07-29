// =====================================================================
// SemanticSearch.cs  --  drop into App_Code
//
// STEP 1: semantic layer for the RESOURCES search section only.
//
// Targets .NET Framework 4.x / C# 5 (Web Site project). No external
// NuGet packages: System.Web.Extensions (JavaScriptSerializer) +
// HttpWebRequest. All calls are SYNCHRONOUS by design so they are safe
// to call from the existing synchronous SearchResults control without
// async-over-sync deadlock risk.
//
// Provider is configurable (OpenAI or Azure OpenAI) via appSettings so
// you are not locked in before the data-residency question is settled.
//
// Permission trimming is NOT done here. The caller passes in the set of
// resource IDs the user is allowed to see (Resources_Search_New already
// computes this as its @mytable result set), and semantic ranking is
// restricted to that set.
// =====================================================================

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace EkoSearch
{
    // ==================================================================
    // Configuration helper -- single place that reads appSettings.
    // ==================================================================
    public static class SearchConfig
    {
        public static string Provider           // "openai" | "azure"
        { get { return (ConfigurationManager.AppSettings["Search.Embedding.Provider"] ?? "openai").ToLower(); } }

        public static string ApiKey
        { get { return ConfigurationManager.AppSettings["Search.Embedding.ApiKey"]; } }

        public static string Model              // OpenAI model name
        { get { return ConfigurationManager.AppSettings["Search.Embedding.Model"] ?? "text-embedding-3-small"; } }

        public static int Dimensions
        {
            get
            {
                int d;
                return int.TryParse(ConfigurationManager.AppSettings["Search.Embedding.Dimensions"], out d) ? d : 1536;
            }
        }

        // For Azure: full endpoint incl. deployment + api-version, e.g.
        // https://myres.openai.azure.com/openai/deployments/embed/embeddings?api-version=2023-05-15
        public static string AzureEndpoint
        { get { return ConfigurationManager.AppSettings["Search.Embedding.AzureEndpoint"]; } }

        public static string ResourcesConnName
        { get { return ConfigurationManager.AppSettings["Search.Resources.ConnectionName"] ?? "dbResources"; } }

        public static int ResourcesPublishedStatus
        {
            get
            {
                int s;
                return int.TryParse(ConfigurationManager.AppSettings["Search.Resources.PublishedStatus"], out s) ? s : 1;
            }
        }

        public static int ResourcesLanguageId
        {
            get
            {
                int l;
                return int.TryParse(ConfigurationManager.AppSettings["Search.Resources.LanguageId"], out l) ? l : 1;
            }
        }

        // Which section "types" get semantic search. Step 1: "resources".
        public static bool SemanticEnabledFor(string type)
        {
            string csv = ConfigurationManager.AppSettings["Search.SemanticTypes"] ?? "resources";
            foreach (string t in csv.Split(','))
                if (t.Trim().Equals(type, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        public static bool ShowSemanticBadge
        { get { return string.Equals(ConfigurationManager.AppSettings["Search.ShowSemanticBadge"], "true", StringComparison.OrdinalIgnoreCase); } }

        public static float MinScore
        {
            get
            {
                float m;
                return float.TryParse(ConfigurationManager.AppSettings["Search.Embedding.MinScore"], out m) ? m : 0.25f;
            }
        }

        public static string ConnString(string name)
        { return ConfigurationManager.ConnectionStrings[name].ConnectionString; }
    }

    // ==================================================================
    // Provenance of a fused result.
    // ==================================================================
    public enum HitSource { KeywordOnly, SemanticOnly, Both }

    public sealed class FusedHit
    {
        public int ResourceId;
        public float Score;
        public HitSource Source;
    }

    public sealed class SemanticHit
    {
        public int ResourceId;
        public float Score;
    }

    // ==================================================================
    // Synchronous embedding client (OpenAI or Azure).
    // ==================================================================
    public static class EmbeddingClient
    {
        public static float[] EmbedSync(string text)
        {
            List<float[]> r = EmbedBatchSync(new List<string> { text });
            return r.Count > 0 ? r[0] : new float[0];
        }

        public static List<float[]> EmbedBatchSync(List<string> texts)
        {
            bool azure = SearchConfig.Provider == "azure";

            string url = azure ? SearchConfig.AzureEndpoint
                               : "https://api.openai.com/v1/embeddings";

            var ser = new JavaScriptSerializer();
            ser.MaxJsonLength = int.MaxValue;

            // OpenAI needs "model"; Azure infers it from the deployment URL.
            Dictionary<string, object> payload;
            if (azure)
                payload = new Dictionary<string, object> { { "input", texts } };
            else
                payload = new Dictionary<string, object> { { "model", SearchConfig.Model }, { "input", texts } };

            byte[] body = Encoding.UTF8.GetBytes(ser.Serialize(payload));

            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json";
            req.Timeout = 15000;
            if (azure)
                req.Headers.Add("api-key", SearchConfig.ApiKey);
            else
                req.Headers.Add("Authorization", "Bearer " + SearchConfig.ApiKey);

            using (Stream rs = req.GetRequestStream())
                rs.Write(body, 0, body.Length);

            string json;
            try
            {
                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var sr = new StreamReader(resp.GetResponseStream()))
                    json = sr.ReadToEnd();
            }
            catch (WebException wex)
            {
                string detail = "";
                if (wex.Response != null)
                    using (var sr = new StreamReader(wex.Response.GetResponseStream()))
                        detail = sr.ReadToEnd();
                throw new InvalidOperationException("Embedding API error: " + wex.Message + " | " + detail, wex);
            }

            var parsed = ser.Deserialize<EmbeddingResponse>(json);
            return parsed.data
                .OrderBy(d => d.index)
                .Select(d => d.embedding.Select(v => (float)v).ToArray())
                .ToList();
        }

        public class EmbeddingResponse { public List<EmbeddingItem> data; }
        public class EmbeddingItem { public int index; public List<double> embedding; }
    }

    // ==================================================================
    // Vector math + packing.
    // ==================================================================
    public static class VectorMath
    {
        public static byte[] Pack(float[] v)
        {
            var b = new byte[v.Length * 4];
            Buffer.BlockCopy(v, 0, b, 0, b.Length);
            return b;
        }

        public static float[] Unpack(byte[] b)
        {
            var v = new float[b.Length / 4];
            Buffer.BlockCopy(b, 0, v, 0, b.Length);
            return v;
        }

        public static void NormalizeInPlace(float[] v)
        {
            double sum = 0;
            for (int i = 0; i < v.Length; i++) sum += (double)v[i] * v[i];
            float n = (float)Math.Sqrt(sum);
            if (n == 0f) return;
            for (int i = 0; i < v.Length; i++) v[i] /= n;
        }

        public static float Dot(float[] a, float[] b)
        {
            float s = 0f;
            int len = Math.Min(a.Length, b.Length);
            for (int i = 0; i < len; i++) s += a[i] * b[i];
            return s;
        }

        public static string Sha256Hex(string text)
        {
            using (var sha = SHA256.Create())
            {
                byte[] h = sha.ComputeHash(Encoding.Unicode.GetBytes(text ?? ""));
                var sb = new StringBuilder(64);
                for (int i = 0; i < h.Length; i++) sb.Append(h[i].ToString("X2"));
                return sb.ToString();
            }
        }
    }

    // ==================================================================
    // Indexer -- (re)embeds changed resources, removes stale ones.
    // Synchronous; run from the admin page (can take minutes on first run).
    // ==================================================================
    public static class ResourceEmbeddingIndexer
    {
        public class IndexResult { public int Embedded; public int Removed; public string Error; }

        public static IndexResult RunAll()
        {
            var result = new IndexResult();
            string conn = SearchConfig.ConnString(SearchConfig.ResourcesConnName);
            int status = SearchConfig.ResourcesPublishedStatus;
            int lang = SearchConfig.ResourcesLanguageId;

            try
            {
                // 1) Remove stale (unpublished/hidden) embeddings.
                var stale = new List<int>();
                using (var cn = new SqlConnection(conn))
                using (var cmd = new SqlCommand("dbo.Resources_GetStaleEmbeddings", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PublishedStatus", status);
                    cmd.Parameters.AddWithValue("@LanguageId", lang);
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                        while (rd.Read()) stale.Add(rd.GetInt32(0));
                }
                foreach (int id in stale)
                {
                    using (var cn = new SqlConnection(conn))
                    using (var cmd = new SqlCommand("dbo.Resources_DeleteEmbedding", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ResourceId", id);
                        cn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    result.Removed++;
                }

                // 2) Pull resources needing embedding.
                var pending = new List<KeyValuePair<int, string>>();
                using (var cn = new SqlConnection(conn))
                using (var cmd = new SqlCommand("dbo.Resources_GetNeedingEmbedding", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PublishedStatus", status);
                    cmd.Parameters.AddWithValue("@LanguageId", lang);
                    cmd.CommandTimeout = 120;
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                        while (rd.Read())
                            pending.Add(new KeyValuePair<int, string>(rd.GetInt32(0), rd.GetString(1)));
                }

                // 3) Embed in batches and save.
                const int batch = 50;
                for (int off = 0; off < pending.Count; off += batch)
                {
                    var slice = pending.Skip(off).Take(batch).ToList();
                    var vectors = EmbeddingClient.EmbedBatchSync(slice.Select(p => p.Value).ToList());

                    using (var cn = new SqlConnection(conn))
                    {
                        cn.Open();
                        for (int i = 0; i < slice.Count; i++)
                        {
                            using (var cmd = new SqlCommand("dbo.Resources_SaveEmbedding", cn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@ResourceId", slice[i].Key);
                                cmd.Parameters.AddWithValue("@Model", SearchConfig.Model);
                                cmd.Parameters.AddWithValue("@Dimensions", SearchConfig.Dimensions);
                                cmd.Parameters.AddWithValue("@Vector", VectorMath.Pack(vectors[i]));
                                cmd.Parameters.AddWithValue("@SourceHash", VectorMath.Sha256Hex(slice[i].Value));
                                cmd.ExecuteNonQuery();
                            }
                            result.Embedded++;
                        }
                    }
                }

                ResourceSemanticSearch.InvalidateCache();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }

    // ==================================================================
    // Search service -- in-memory vector cache + ranking + RRF fusion.
    // ==================================================================
    public static class ResourceSemanticSearch
    {
        private static volatile ConcurrentDictionary<int, float[]> _cache;
        private static readonly object Lock = new object();

        public static void InvalidateCache() { _cache = null; }

        private static ConcurrentDictionary<int, float[]> Cache()
        {
            var c = _cache;
            if (c != null) return c;
            lock (Lock)
            {
                if (_cache != null) return _cache;
                var loaded = new ConcurrentDictionary<int, float[]>();
                string conn = SearchConfig.ConnString(SearchConfig.ResourcesConnName);
                using (var cn = new SqlConnection(conn))
                using (var cmd = new SqlCommand("dbo.Resources_GetAllEmbeddings", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120;
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            int id = rd.GetInt32(0);
                            byte[] bytes = (byte[])rd["Vector"];
                            float[] v = VectorMath.Unpack(bytes);
                            VectorMath.NormalizeInPlace(v);
                            loaded[id] = v;
                        }
                    }
                }
                _cache = loaded;
                return loaded;
            }
        }

        /// <summary>
        /// Rank resources semantically, RESTRICTED to permittedIds (the
        /// user's accessible + filtered set from Resources_Search_New).
        /// Returns empty on any API failure so the caller falls back to
        /// keyword-only.
        /// </summary>
        public static List<SemanticHit> Search(string query, ICollection<int> permittedIds, int topN)
        {
            var hits = new List<SemanticHit>();
            if (string.IsNullOrEmpty(query) || permittedIds == null || permittedIds.Count == 0)
                return hits;

            float[] q;
            try { q = EmbeddingClient.EmbedSync(query); }
            catch { return hits; }
            VectorMath.NormalizeInPlace(q);

            var cache = Cache();
            float min = SearchConfig.MinScore;

            foreach (int id in permittedIds)
            {
                float[] v;
                if (!cache.TryGetValue(id, out v)) continue;   // not yet embedded
                float score = VectorMath.Dot(q, v);
                if (score >= min)
                    hits.Add(new SemanticHit { ResourceId = id, Score = score });
            }

            return hits.OrderByDescending(h => h.Score).Take(topN).ToList();
        }

        /// <summary>
        /// Reciprocal Rank Fusion of keyword + semantic rankings, with
        /// provenance per result. Rank-based, so the two score scales
        /// don't need to be compatible.
        /// </summary>
        public static List<FusedHit> Fuse(List<int> keywordRankedIds, List<SemanticHit> semantic, int topN)
        {
            const float k = 60f;
            var score = new Dictionary<int, float>();
            var inKeyword = new HashSet<int>();
            var inSemantic = new HashSet<int>();

            for (int rank = 0; rank < keywordRankedIds.Count; rank++)
            {
                int id = keywordRankedIds[rank];
                inKeyword.Add(id);
                float add = 1f / (k + rank + 1);
                score[id] = score.ContainsKey(id) ? score[id] + add : add;
            }
            for (int rank = 0; rank < semantic.Count; rank++)
            {
                int id = semantic[rank].ResourceId;
                inSemantic.Add(id);
                float add = 1f / (k + rank + 1);
                score[id] = score.ContainsKey(id) ? score[id] + add : add;
            }

            return score
                .OrderByDescending(p => p.Value)
                .Take(topN)
                .Select(p => new FusedHit
                {
                    ResourceId = p.Key,
                    Score = p.Value,
                    Source = (inKeyword.Contains(p.Key) && inSemantic.Contains(p.Key)) ? HitSource.Both
                           : inSemantic.Contains(p.Key) ? HitSource.SemanticOnly
                           : HitSource.KeywordOnly
                })
                .ToList();
        }
    }
}
