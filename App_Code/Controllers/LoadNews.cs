using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web.Http;

public class LoadNewsController : ApiController
{
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<controller>/5
    public string Get(int offset, int records, int publish, int category = 0)
    {


        string template = @"<a href='/{0}' class='{5}' title='{2}' target='{7}' {6}>
                          <div class='div_table news_description'>
                            <div class='div_cell image_news'>
                                <img src='/Controls/Newsroom/ThumbNail.ashx?PictureID={1}&amp;maxsz=80' alt='{3}'{8}/>
                            </div>
                            <div class='div_cell home-news-desc'>
                            <h3>{4}</h3><br>
                            <h4>{2}</h4>
                        </div>                      
                    </div>
                </a>";

        DataTable dt = new DataTable();
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlDataAdapter da = new SqlDataAdapter("BreakingNews", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@Lang", 1);
            if (category != 0)
                da.SelectCommand.Parameters.AddWithValue("@categ", category);
            else
                da.SelectCommand.Parameters.AddWithValue("@categ", DBNull.Value);

            da.SelectCommand.Parameters.AddWithValue("@publish", publish);
            da.SelectCommand.Parameters.AddWithValue("@top", records * (offset + 1));

            if (category > 0)
            {
                da.SelectCommand.Parameters.AddWithValue("@category", category);

            }

            da.Fill(dt);
        }

        StringBuilder sb = new StringBuilder("");
        string NewsroomFilesPath = "/data/NewsroomFiles/";

        for (int i = 0; i < records; i++)
        {
            try
            {
                DataRow dr = dt.Rows[(records * offset) + i];

                #region Photo
                //string photo = "/images/base/newsplaceholder.jpg";
                //if (dr["MIMEType"].ToString() != "")
                //{  
                //    // photo = "/Controls/Newsroom/ThumbNail.ashx?PictureID=" + dr["id"].ToString() + "&maxsz=125";
                    
                //}
                #endregion

                #region Link
                string url;
                string target = "_self";
                string filename = "";
                string myclass = "three jnewssc";
                string prefix = ""; // CMSHelper.GetLanguagePrefix();
                string ltdate = DateTime.Parse(Convert.ToDateTime(dr["NewsDate"].ToString(), CultureInfo.InvariantCulture).ToString(), CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);

                if (dr["type"].ToString() == "0")
                {
                    url = prefix + (publish == 3 ? "membernews?newsid=" : "newsroom?newsid=") + dr["linkid"].ToString();

                }
                else if (dr["type"].ToString() == "1")
                {
                    myclass += " newsitem read_more open_new_tab";
                    filename = "filename=" +  NewsroomFilesPath + dr["filename"].ToString();
                    url = "#";
                }
                else
                {
                    if (dr["seo"].ToString() != "")
                    {
                        url = "/" + dr["seo"].ToString();

                    }
                    else
                    {
                        // theLink.Attributes.Add("onclick", "window.open('" + dr["ExternalURL"].ToString() + "', null, 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, resizable'); return false;");
                        url = dr["ExternalURL"].ToString();
                        target = "_blank";
                    }
                }
                #endregion


                string s = String.Format(template,
                    url,
                    dr["id"].ToString(),
                    dr["Title"].ToString(),
                    dr["PhotoAltText"].ToString(),
                    ltdate,
                    myclass,
                    filename,
                    target,
                    dr["MIMEType"].ToString() == "" ? " style='display:none;'" : "");

                //    photo,
                //    dr["PhotoAltText"].ToString(),
                //    ((DateTime)dr["NewsDate"]).ToString("MMMM dd, yyyy"),
                //    dr["Title"].ToString(),
                //    dr["DetailsShort"].ToString() != "" ? "<p>" + dr["DetailsShort"].ToString() + "</p>" : "",
                //    url,
                //    target);



                //@"<a href='/{0}?newsid={1} class='{5}' title='{2}' target='{7}' {6}>
                //          <div class='div_table news_description'>
                //            <div class='div_cell image_news'>
                //                <img src='/Controls/Newsroom/ThumbNail.ashx?PictureID={1}&amp;maxsz=80' alt='{3}'>
                //            </div>
                //            <div class='div_cell home-news-desc>
                //            <h3>{4}</h3><br>
                //            <h4>{2}</h4>"

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
