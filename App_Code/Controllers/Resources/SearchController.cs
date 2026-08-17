using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Newtonsoft.Json.Linq;

public class SearchController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "search" };
    }

    // GET api/<controller>/5
    public string Get(int id, int lang)
    {
        return id.ToString();
    }

    // POST api/<controller>
    public JToken[] Post([FromBody] dynamic values)
    {
        return GetResult(values);
    }

    private JToken[] GetResult([FromBody] dynamic values)
    {
        string userid = values["u"];
        try { int.Parse(userid); }
        catch { return null; }

        string libid = values["lib"];
        try { int.Parse(libid); }
        catch { libid = ""; }

        string catid = values["cat"];
        try { int.Parse(catid); }
        catch { catid = ""; }

        string formatid = values["format"];
        try { int.Parse(formatid); }
        catch { formatid = ""; }

        string audienceid = values["audience"];
        try { int.Parse(audienceid); }
        catch { audienceid = ""; }

        string searchterm = values["search"] == null ? "" : values["search"].ToString();
        if (searchterm == "null")
            searchterm = "";

        string save = "0";
        try { save = values["save"]; }
        catch{ }

        if (libid == "" && searchterm == "" && formatid == "" && audienceid == "" && catid == "")
            return SearchJson("", "");

        return Populate(userid, libid, catid, formatid, audienceid, searchterm, 1, save);
    }

    private static JToken[] SearchJson(string items, string header)
    {
        JObject obj = new JObject();
        obj["items"] = items ?? "";
        obj["header"] = header ?? "";
        JToken[] json = new JToken[1];
        json[0] = obj;
        return json;
    }

    private JToken[] Populate(string userid, string libid, string catid, string formatid, string audienceid, string searchterm, int lang, string save)
    {
        DataSet ds = new DataSet();
        string html = "";
        string header = "";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            ResourceSearch res = new ResourceSearch("Resources_Search_New", CommandType.StoredProcedure, userid, lang);
            res.LibraryId = libid;
            res.CategoryId = catid;
            res.FormatId = formatid;
            res.AudienceId = audienceid;
            res.Keywords = searchterm ?? "";
            res.Save = save;

            SqlDataAdapter dapt = res.Build();
            dapt.SelectCommand.Connection = conn;

            dapt.Fill(ds);
            DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();

            Res_ItemTemplate _item = new Res_ItemTemplate();

            foreach (DataRow dr in dt.Rows)
            {
                html += _item.GetContent(dr, ConfigurationManager.AppSettings.Get("Resources.Page.Details"));
            }

            if (!String.IsNullOrEmpty(searchterm))
            {
                int records = dt.Rows.Count;
                string libName = "";

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    libName = ds.Tables[1].Rows[0]["name"].ToString();

                header = res.GetHeaderResult(records, searchterm, libName, ConfigurationManager.AppSettings["Resources.Page"] + "?search_term=" + searchterm);
            }
            else if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[1].Rows[0];
                header = String.Format("<h1>{0}</h1><p>{1}</p>", dr["name"].ToString(), dr["description"].ToString());
            }
        }

        return SearchJson(html, header);
    }

    // PUT api/<controller>/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/<controller>/5
    public void Delete(int id)
    {
    }
}
