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

        string subcatid = values["sub"];
        try { int.Parse(subcatid); }
        catch { subcatid = ""; }

        string searchterm = values["search"];

        string save = "0";
        try { save = values["save"]; }
        catch{ }

        if (libid == "" && searchterm == "")
        {
            JToken[] json = new JToken[1];
            json[0] = JObject.Parse("{ \"items\" : \"" + "" + "\", \"header\" : \"" + "" + "\"}");

            return json;
        }

        return Populate(userid, libid, catid, subcatid, searchterm, 1, save);
    }

    private JToken[] Populate(string userid, string libid, string catid, string subcatid, string searchterm, int lang, string save)
    {
        DataSet ds = new DataSet();
        string html = "";
        string header = "";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            ResourceSearch res = new ResourceSearch("Resources_Search_New", CommandType.StoredProcedure, userid, lang);
            res.LibraryId = libid;
            res.CategoryId = catid;
            res.SubCategoryId = subcatid;
            res.Keywords = searchterm;
            res.Save = save;

            SqlDataAdapter dapt = res.Build();
            dapt.SelectCommand.Connection = conn;

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            Res_ItemTemplate _item = new Res_ItemTemplate();

            foreach (DataRow dr in dt.Rows)
            {
                html += _item.GetContent(dr, ConfigurationManager.AppSettings.Get("Resources.Page.Details"));
            }

            if (searchterm != "")
            {
                int records = dt.Rows.Count;
                string libName = "";

                if (ds.Tables[1].Rows.Count > 0)
                {
                    libName = ds.Tables[1].Rows[0]["name"].ToString();
                }
                    
                header = res.GetHeaderResult(records, searchterm, libName, ConfigurationManager.AppSettings["Resources.Page"] + "?search_term=" + searchterm);
            }
            else if (ds.Tables[1].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[1].Rows[0];
                header = String.Format("<h1>{0}</h1><p>{1}</p>", dr["name"].ToString(), dr["description"].ToString());
            }
        }

        html = html.Replace("\"", "“");
        JToken[] json = new JToken[1];
        json[0] = JObject.Parse("{ \"items\" : \"" + html + "\", \"header\" : \"" + header + "\"}");

        return json;
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
