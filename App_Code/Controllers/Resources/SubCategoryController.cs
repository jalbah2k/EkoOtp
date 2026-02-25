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

public class SubCategoryController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "sub" };
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

        string libid = values["lib"];
        try { int.Parse(libid); }
        catch { return null; }

        string catid = values["cat"];
        try { int.Parse(catid); }
        catch { return null; }

        int subid = 0;
        string sub = values["sub"];
        int.TryParse(sub, out subid); 
        

        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("res.SubCategory_api", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@userid", userid);
            dapt.SelectCommand.Parameters.AddWithValue("@libid", libid);
            dapt.SelectCommand.Parameters.AddWithValue("@catid", catid);

            if (subid > 0)
                dapt.SelectCommand.Parameters.AddWithValue("@subcatid", subid);

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];

            if (dt.Rows.Count > 0 && dt1.Rows.Count == 1)
            {
                JToken[] json = new JToken[dt.Rows.Count];
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    json[i++] = JObject.Parse("{ \"id\" : \"" + dr["id"].ToString() + "\", \"name\" : \"" + dr["name"].ToString() + "\"" +
                                                ", \"libseo\" : \"" + dt1.Rows[0]["libseo"].ToString().ToLower() + "\"" +
                                                ", \"catseo\" : \"" + dt1.Rows[0]["catseo"].ToString().ToLower() + "\"" +
                                                ", \"subseo\" : \"" + dt1.Rows[0]["subseo"].ToString().ToLower() + "\"}");

                }
                return json;
            }
            else if(dt1.Rows.Count == 1)
            {
                JToken[] json = new JToken[dt1.Rows.Count];

                json[0] = JObject.Parse("{ \"id\" : \"" + "0" + "\", \"name\" : \"" + "" + "\"" +
                                                ", \"libseo\" : \"" + dt1.Rows[0]["libseo"].ToString().ToLower() + "\"" +
                                                ", \"catseo\" : \"" + dt1.Rows[0]["catseo"].ToString().ToLower() + "\"" +
                                                ", \"subseo\" : \"" + dt1.Rows[0]["subseo"].ToString().ToLower() + "\"}");

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
