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

public class CitiesPNCAController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "city" };
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

        int regid = 0;
        string region = values["reg"];
        int.TryParse(region, out regid);

        int cityid = 0;
        string city = values["city"];
        int.TryParse(city, out cityid); 
        

        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("pnca.City_api", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

            if (servid > 0)
                dapt.SelectCommand.Parameters.AddWithValue("@serviceid", servid);

            if (regid > 0)
                dapt.SelectCommand.Parameters.AddWithValue("@regionid", regid);

            if (cityid > 0)
                dapt.SelectCommand.Parameters.AddWithValue("@cityid", cityid);

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];

            if (dt.Rows.Count > 0 && ds.Tables[1].Rows.Count == 1)
            {
                JToken[] json = new JToken[dt.Rows.Count];
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    string stemp = "{ \"id\" : \"" + dr["id"].ToString() + "\", \"name\" : \"" + dr["name"].ToString() + "\"" + ", \"type\" : \"city\"";

                    if (ds.Tables[1].Rows.Count == 1)
                    {
                        if (dt1.Rows[0]["ServSeo"].ToString() != "")
                            stemp += ", \"service\" : \"" + dt1.Rows[0]["ServSeo"].ToString().ToLower() + "\"";

                        if (dt1.Rows[0]["RegSeo"].ToString() != "")
                            stemp += ", \"region\" : \"" + dt1.Rows[0]["RegSeo"].ToString().ToLower() + "\"";

                        if (dt1.Rows[0]["CitySeo"].ToString() != "")
                            stemp += ", \"city\" : \"" + dt1.Rows[0]["CitySeo"].ToString().ToLower() + "\"";
                    }

                    stemp += "}";

                    json[i++] = JObject.Parse(stemp);

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
