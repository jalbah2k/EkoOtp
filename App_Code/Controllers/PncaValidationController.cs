using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

public class PncaValidationController : ApiController
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
    public string Post([FromBody]dynamic value)
    {
        string ret = "";

        string id = value["id"];
        try { int.Parse(id); }
        catch { return "error"; }

        string lib = value["lib"];
        try { int.Parse(id); }
        catch { return "error"; }

        if( id != lib)
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select * from pnca.Organizations where OrganizationId=@orgid", ConfigurationManager.AppSettings["CMServer"]);
            DataTable dt = new DataTable();
            dapt.SelectCommand.Parameters.AddWithValue("@orgid", lib);
            dapt.Fill(dt);

            if (dt.Rows.Count > 0)
                ret = "This Organization name is already assigned to another member";
        }       

        return ret;
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
