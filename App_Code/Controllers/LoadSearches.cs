using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Http;

public class LoadSearchesController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public string Get(int offset, int records, int category)
    {

        StringBuilder sb = new StringBuilder("");
        string template = @"<div>{3} - <a href='/resources?{0}' target='{2}'>{1}</a></div>";

        DataTable dt = new DataTable();
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["dbResources"]))
        {
            SqlDataAdapter da = new SqlDataAdapter("MyLastSearches", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@userid", category);
            da.SelectCommand.Parameters.AddWithValue("@top", records * (offset + 1));
            //da.SelectCommand.Parameters.AddWithValue("@resorces_only", resources);

            da.Fill(dt);
        }


        for (int i = 0; i < records; i++)
        {
            try
            {
                DataRow rw = dt.Rows[(records * offset) + i];
                string s = String.Format(template,
                    rw["querystring"].ToString(),
                    rw["parameters"].ToString(),
                    rw["target"].ToString(),
                    Convert.ToDateTime(rw["timestamp"]).ToString("MMMM dd, yyyy"));

                sb.Append(s);
            }
            catch { break; }
        }

        return sb.ToString();
    }

    // POST api/<controller>
    public void Post([FromBody]dynamic value)
    {
       
    }


    // PUT api/<controller>/5
    public void Put(string name, [FromBody]string value)
    {
    }

    // DELETE api/<controller>/5
    public void Delete(int id)
    {
    }

}
