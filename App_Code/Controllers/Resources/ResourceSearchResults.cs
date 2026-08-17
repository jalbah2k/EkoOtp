using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

public static class ResourceSearchResults
{
    public static JToken[] Empty()
    {
        return Json("", "");
    }

    public static JToken[] Json(string items, string header)
    {
        JObject obj = new JObject();
        obj["items"] = items ?? "";
        obj["header"] = header ?? "";
        JToken[] json = new JToken[1];
        json[0] = obj;
        return json;
    }

    public static JToken[] Run(string userid, string libid, string catid, string formatid, string audienceid, string searchterm, string save)
    {
        DataSet ds = new DataSet();
        string html = "";
        string header = "";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            ResourceSearch res = new ResourceSearch("Resources_Search_New", CommandType.StoredProcedure, userid, 1);
            res.LibraryId = libid ?? "";
            res.CategoryId = catid ?? "";
            res.FormatId = formatid ?? "";
            res.AudienceId = audienceid ?? "";
            res.Keywords = searchterm ?? "";
            res.Save = save ?? "0";

            SqlDataAdapter dapt = res.Build();
            dapt.SelectCommand.Connection = conn;
            dapt.Fill(ds);

            DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
            Res_ItemTemplate item = new Res_ItemTemplate();

            foreach (DataRow dr in dt.Rows)
                html += item.GetContent(dr, ConfigurationManager.AppSettings.Get("Resources.Page.Details"));

            if (!String.IsNullOrEmpty(searchterm))
            {
                string libName = "";
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    libName = ds.Tables[1].Rows[0]["name"].ToString();

                header = res.GetHeaderResult(dt.Rows.Count, searchterm, libName,
                    ConfigurationManager.AppSettings["Resources.Page"] + "?search_term=" + Uri.EscapeDataString(searchterm));
            }
            else if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[1].Rows[0];
                header = String.Format("<h1>{0}</h1><p>{1}</p>", dr["name"].ToString(), dr["description"].ToString());
            }
        }

        return Json(html, header);
    }

    public static string ReadId(dynamic values, string key)
    {
        try
        {
            string val = values[key] == null ? "" : values[key].ToString();
            if (String.IsNullOrEmpty(val) || val == "null")
                return "";
            int.Parse(val);
            return val;
        }
        catch
        {
            return "";
        }
    }

    public static string ReadText(dynamic values, string key)
    {
        try
        {
            string val = values[key] == null ? "" : values[key].ToString();
            if (val == "null")
                return "";
            return val.Trim();
        }
        catch
        {
            return "";
        }
    }
}
