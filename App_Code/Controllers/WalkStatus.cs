using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

public class WalkStatusController : ApiController
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

        string status = value["status"];
        try { int.Parse(status); }
        catch { return; }

        //update DB
        SqlCommand cmd = new SqlCommand("update eko.Members set EkoWalkStatus=@status, EkoWalkStep = (case when @status = 3 then '' else EkoWalkStep end) where userid=@id; select @@ROWCOUNT", new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")));
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@status", status);
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
