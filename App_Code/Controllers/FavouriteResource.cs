using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

public class FavouriteResourceController : ApiController
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
    public bool Post([FromBody]dynamic value)
    {
        bool ret = false;

        string id = value["id"];
        try { int.Parse(id); }
        catch { return ret; }

        string ln = value["ln"];
        int len = 0;
        try { len = int.Parse(ln); }
        catch { return ret; }

        string resid = id.Substring(0, len);
        string userid = id.Substring(len);
        //update DB
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@ResourceId", resid));
            parms.Add(new SqlParameter("@userid", userid));

            ret = (bool)MyDAL.ExecuteQuery(@"update ResourcesUsers_Link set Favourite = case when Favourite = 1 then 0 else 1 end where ResourceId=@ResourceId and UserId=@userid;
                                            select Favourite from ResourcesUsers_Link where ResourceId=@ResourceId and UserId=@userid",
                    parms.ToArray(),
                    CommandType.Text,
                    conn);

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
