using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json.Linq;

public class SearchController : ApiController
{
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "search" };
    }

    public string Get(int id, int lang)
    {
        return id.ToString();
    }

    public JToken[] Post([FromBody] dynamic values)
    {
        string userid = ResourceSearchResults.ReadId(values, "u");
        if (userid == "")
            return ResourceSearchResults.Empty();

        string libid = ResourceSearchResults.ReadId(values, "lib");
        string catid = ResourceSearchResults.ReadId(values, "cat");
        string formatid = ResourceSearchResults.ReadId(values, "format");
        string audienceid = ResourceSearchResults.ReadId(values, "audience");
        string searchterm = ResourceSearchResults.ReadText(values, "search");
        string save = ResourceSearchResults.ReadText(values, "save");
        if (save == "")
            save = "0";

        if (libid == "" && formatid == "" && audienceid == "" && catid == "" && searchterm == "")
            return ResourceSearchResults.Empty();

        if (libid == "" && formatid == "" && audienceid == "" && catid == "")
            return ResourceSearchResults.Run(userid, "", "", "", "", searchterm, save);

        return ResourceSearchResults.Run(userid, libid, catid, formatid, audienceid, searchterm, save);
    }

    public void Put(int id, [FromBody] string value)
    {
    }

    public void Delete(int id)
    {
    }
}
