using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json.Linq;

public class KeywordSearchController : ApiController
{
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "keywordsearch" };
    }

    public JToken[] Post([FromBody] dynamic values)
    {
        string userid = ResourceSearchResults.ReadId(values, "u");
        if (userid == "")
            return ResourceSearchResults.Empty();

        string searchterm = ResourceSearchResults.ReadText(values, "search");
        if (searchterm == "")
            return ResourceSearchResults.Empty();

        string save = ResourceSearchResults.ReadText(values, "save");
        if (save == "")
            save = "1";

        return ResourceSearchResults.Run(userid, "", "", "", "", searchterm, save);
    }
}
