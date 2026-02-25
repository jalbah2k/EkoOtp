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

public class CategoryController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "category" };
    }

    // GET api/<controller>/5
    public string Get(int id, int lang)
    {
        return id.ToString();
    }

    // POST api/<controller>
    public JToken[] Post([FromBody] dynamic values)
    {
        return GetFilters(values);
    }

    private JToken[] GetFilters([FromBody] dynamic values)
    {
        string userid = values["u"];
        try { int.Parse(userid); }
        catch { return null; }

        string id = values["lib"];
        try { int.Parse(id); }
        catch { return null; }


        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("res.Category_api", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@userid", userid);
            dapt.SelectCommand.Parameters.AddWithValue("@libid", id);

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0 && ds.Tables[1].Rows.Count == 1)
            {
                JToken[] json = new JToken[dt.Rows.Count];
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    json[i++] = JObject.Parse("{ \"id\" : \"" + dr["id"].ToString() + "\", \"name\" : \"" + dr["name"].ToString() + "\"" + 
                                                ", \"libseo\" : \"" + ds.Tables[1].Rows[0]["seo"].ToString().ToLower() + "\"}");

                }
                return json;
            }
        }
        return new JToken[0];
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
