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

public class SearchPNCAController : ApiController
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

        string servid = values["serv"];
        try { int.Parse(servid); }
        catch { servid = ""; }

        string regid = values["reg"];
        try { int.Parse(regid); }
        catch { regid = ""; }

        string cityid = values["city"];
        try { int.Parse(cityid); }
        catch { cityid = ""; }

        string searchterm = values["search"];

        //if (servid == "" && searchterm == "")
        //{
        //    JToken[] json = new JToken[1];
        //    json[0] = JObject.Parse("{ \"items\" : \"" + "" + "\", \"header\" : \"" + "" + "\"}");

        //    return json;
        //}

        return Populate(servid, regid, cityid, searchterm, 1);
    }

    private JToken[] Populate( string servid, string regid, string cityid, string searchterm, int lang)
    {
        DataSet ds = new DataSet();
        string html = "";
        string header = "";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            PNCA_DirectorySearch dir = new PNCA_DirectorySearch("pnca.Organizations_PublicSearch", CommandType.StoredProcedure, lang);
            dir.ServiceId = servid;
            dir.RegionId = regid;
            dir.CityId = cityid;
            dir.Keywords = searchterm;

            SqlDataAdapter dapt = dir.Build();
            dapt.SelectCommand.Connection = conn;

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            PNCA_ItemTemplate _item = new PNCA_ItemTemplate();

            foreach (DataRow dr in dt.Rows)
            {
                html += _item.GetContent(dr);
            }

            //if (searchterm != "")
            //{
            //    int records = dt.Rows.Count;
            //    string libName = "";

            //    if (ds.Tables[1].Rows.Count > 0)
            //    {
            //        libName = ds.Tables[1].Rows[0]["name"].ToString();
            //    }
                    
            //    header = dir.GetHeaderResult(records, searchterm, libName, ConfigurationManager.AppSettings["Resources.Page"] + "?search_term=" + searchterm);
            //}
            //else if (ds.Tables[1].Rows.Count > 0)
            //{
            //    DataRow dr = ds.Tables[1].Rows[0];
            //    header = String.Format("<h1>{0}</h1><p>{1}</p>", dr["name"].ToString(), dr["description"].ToString());
            //}
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
