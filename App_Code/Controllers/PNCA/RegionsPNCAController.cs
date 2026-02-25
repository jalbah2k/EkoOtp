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

public class RegionsPNCAController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "region" };
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

        int servid = 0;
        string service = values["serv"];
        int.TryParse(service, out servid);


        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("pnca.Region_api", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

            if (servid > 0)
                dapt.SelectCommand.Parameters.AddWithValue("@serviceid", servid);

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            JToken[] json = new JToken[dt.Rows.Count + ds.Tables[2].Rows.Count];

           int i = 0;
           if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string stemp = "{ \"id\" : \"" + dr["id"].ToString() + "\", \"name\" : \"" + dr["name"].ToString() + "\"" + ", \"type\" : \"region\"";

                    stemp += ", \"service\" : \"";
                    if (ds.Tables[1].Rows.Count == 1)
                        stemp +=  ds.Tables[1].Rows[0]["ServSeo"].ToString().ToLower();

                    stemp += "\"}";

                    json[i++] = JObject.Parse(stemp);
                }
            }
            
            if (ds.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    string stemp = "{ \"id\" : \"" + dr["id"].ToString() + "\", \"name\" : \"" + dr["name"].ToString() + "\"" + ", \"type\" : \"city\"";

                    stemp += ", \"service\" : \"";
                    if (ds.Tables[1].Rows.Count == 1)
                        stemp += ds.Tables[1].Rows[0]["ServSeo"].ToString().ToLower();

                    stemp += "\"}";

                    json[i++] = JObject.Parse(stemp);

                }
            }

            return json;

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
