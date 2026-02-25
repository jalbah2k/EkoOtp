using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public class BannerCaptionController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public string Get(int id, int lang)
    {
        int priority = lang + 1;
        string myvalue = "";
        //SqlDataAdapter dapt = new SqlDataAdapter("select ROW_NUMBER() OVER (ORDER BY bannerpriority) AS id, caption, BannerLink from Banners where Gallery=@id", ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select ROW_NUMBER() OVER (ORDER BY bannerpriority) AS id, title, body, BannerLink, ButtonText, ButtonLink, target from Banners where Gallery=@id", ConfigurationManager.AppSettings["CMServer"]);
        DataTable dt = new DataTable();
        dapt.SelectCommand.Parameters.AddWithValue("@id", id);
        dapt.Fill(dt);
        DataRow[] drs = dt.Select("id=" + priority.ToString());

        if (drs.Length > 0)
        {
            //////myvalue = drs[0]["caption"].ToString();
            ////if (drs[0]["BannerLink"].ToString() != "")
            ////{
            ////    //myvalue = String.Format("<span class=\"banner-h1\"><a href=\"{2}\">{0}</a></span><span class=\"banner-p\"><a href=\"{2}\">{1}</a></span>",
            ////    //    drs[0]["title"].ToString(),
            ////    //    drs[0]["body"].ToString(),
            ////    //    drs[0]["BannerLink"].ToString()
            ////    //    );

            ////    if (drs[0]["caption"].ToString() != "")
            ////        myvalue = String.Format("<a href=\"{1}\">{0}</a>", drs[0]["caption"].ToString(), drs[0]["BannerLink"].ToString());

            ////    //if (drs[0]["body"].ToString() != "")
            ////    //    myvalue += String.Format("<span class=\"banner-p\"><a href=\"{1}\">{0}</a></span>", drs[0]["body"].ToString(), drs[0]["BannerLink"].ToString());
            ////}
            ////else
            ////{
            ////   //myvalue = String.Format("<span class=\"banner-h1\">{0}</span><span class=\"banner-p\">{1}</span>", drs[0]["title"].ToString(), drs[0]["body"].ToString());

            ////    if (drs[0]["caption"].ToString() != "")
            ////        myvalue = String.Format("{0}", drs[0]["caption"].ToString());

            ////    //if (drs[0]["body"].ToString() != "")
            ////    //    myvalue += String.Format("<span class=\"banner-p\">{0}</span>", drs[0]["body"].ToString());
            ////}

            DataRow dr = drs[0];

            if (dr["Title"] != DBNull.Value && dr["Title"].ToString().Length > 0)
            {
                myvalue = string.Format("<h2>{0}</h2>", dr["Title"].ToString());
            }

            if (dr["Body"] != DBNull.Value && dr["Body"].ToString().Length > 0)
            {
                myvalue += string.Format("<p>{0}</p>", dr["Body"].ToString());
            }

            if (dr["ButtonText"] != DBNull.Value && dr["ButtonText"].ToString().Length > 0)
            {
                myvalue += string.Format("<a href='{0}' target='{2}'>{1}</a>",
                    dr["ButtonLink"].ToString(),
                    dr["ButtonText"].ToString(),
                    Convert.ToBoolean(dr["target"]) ? "_blank" : "_self"
                    );
            }


            myvalue += string.Format("<div class=\"container ms-layer ms-caption\"><div class=\"banCapBox\">{0}</div></div>", myvalue);
        }
        return myvalue;
    }

    // POST api/<controller>
    public void Post([FromBody]string value)
    {
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
