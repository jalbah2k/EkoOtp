using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

public class LikeNewsController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<controller>
    public void Post([FromBody]dynamic value)
    {
        string id = value["id"];
        try { int.Parse(id); }
        catch { return; }

        //update DB
        SqlCommand cmd = new SqlCommand("NewsRoomLikeIt_Add", new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Connection.Open();
        string ret = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
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
