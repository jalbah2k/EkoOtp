using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Resurces
/// </summary>


public delegate void ReloadResourcesCategories_Admin();
public delegate void ReloadResourcesLibraries_Admin();

public delegate void AfterFiltersLoaded();

public class ResourceSearch
{
    private FullTextSearch.FullTextSearch myFTS;
    private string[] LCIDs = { "1033", "1036", "1034" };  //1033 - English USA; 3084 - French France; 1034: Spanish Spain

    private SqlDataAdapter dapt;

    public string LibraryId { set; get; }
    public string CategoryId { set; get; }
    public string SubCategoryId { set; get; }
    public string Keywords { set; get; }
    public int Language { set; get; }
    public string Save { set; get; }

    public ResourceSearch() { }
    public ResourceSearch(string CommandText, CommandType commType, string userid, int lang) 
    {
        Language = lang;
        dapt = new SqlDataAdapter(new SqlCommand(CommandText));
        dapt.SelectCommand.CommandType = commType;
        dapt.SelectCommand.Parameters.AddWithValue("@userid", userid);
        dapt.SelectCommand.Parameters.AddWithValue("@LCID", LCIDs[lang - 1]);
        //dapt.SelectCommand.Parameters.AddWithValue("@Status", 1);
        //dapt.SelectCommand.Parameters.AddWithValue("@Show", 1);
        dapt.SelectCommand.Parameters.AddWithValue("@LanguageId", Language);

    }

    public ResourceSearch(string CommandText, CommandType commType, string userid, int lang, bool status, bool show)
    {
        Language = lang;
        dapt = new SqlDataAdapter(new SqlCommand(CommandText));
        dapt.SelectCommand.CommandType = commType;
        dapt.SelectCommand.Parameters.AddWithValue("@userid", userid);
        dapt.SelectCommand.Parameters.AddWithValue("@LCID", LCIDs[lang - 1]);
        dapt.SelectCommand.Parameters.AddWithValue("@Status", status ? 1 : 0);
        dapt.SelectCommand.Parameters.AddWithValue("@Show", show ? 1 : 0);
        dapt.SelectCommand.Parameters.AddWithValue("@LanguageId", Language);

    }

    public SqlDataAdapter Build()
    {
        if (!String.IsNullOrEmpty(LibraryId))
        {
            dapt.SelectCommand.Parameters.AddWithValue("@ResourcesGroupId", LibraryId);

            if (!String.IsNullOrEmpty(CategoryId))
            {
                dapt.SelectCommand.Parameters.AddWithValue("@CategID", CategoryId);

                if (!String.IsNullOrEmpty(SubCategoryId))
                    dapt.SelectCommand.Parameters.AddWithValue("@SubCategID", SubCategoryId);
            }
        }

        if (Keywords != "")
        {
            string searchTerm = Keywords.Trim();

            searchTerm = FTSAux.RemoveNoiseWords(searchTerm, LCIDs[Language - 1]);
            myFTS = new FullTextSearch.FullTextSearch(searchTerm);

            dapt.SelectCommand.Parameters.Add(new SqlParameter("@keywords", myFTS.NormalForm));
            dapt.SelectCommand.Parameters.Add(new SqlParameter("@searchTerm", searchTerm));

        }

        if ((!String.IsNullOrEmpty(CategoryId) || Keywords != "") && Save == "1")
            dapt.SelectCommand.Parameters.Add(new SqlParameter("@save", 1));

        return dapt;
    }

    public SqlDataAdapter BuildFavourite()
    {
      
        dapt.SelectCommand.Parameters.AddWithValue("@Favourite", 1);

        return dapt;
    }

    public string GetHeaderResult(int records, string searc_term, string library, string seo, string category = "", string subcategory = "")
    {
        string temp = "<p>There {0} <strong>{1} {2}</strong> found based on your search term <strong>'{3}'</strong>";
        temp = String.Format(temp, (records > 1 ? "are" : "is"), records.ToString(), (records > 1 ? "resources" : "resource"), searc_term);

        //if (subcategory  != "")
        //    temp += " from within the <strong>" + subcategory + "</strong> sub category";
        //else if (category != "")
        //    temp += " from within the <strong>" + category + "</strong> category";
        //else if (library != "")

        if (library != "")
        {
            temp += " from within the <strong>" + library + "</strong> library.</p>";
            temp += String.Format("<p>To clear your filters and view all available resources matching your search term <a href='{0}'>click here</a>.</p>", seo);
        }
        else
            temp += ".</p>";

        return temp;
    }

    public void DownloadFile(string ResourceID, string UserId)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("Pie.Document_select", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@ResourceId", ResourceID);

            DataTable dt = new DataTable();
            dapt.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@ResourceId", ResourceID));
                parms.Add(new SqlParameter("@userid", UserId ));
                parms.Add(new SqlParameter("@action", 1));              //1: means Download

                string sqlcomm = @"insert into ResourcesDownloaded(ResourceId, UserId, Action) values(@ResourceId, @userid, @action) 
                                    
                                    if not exists (select id from ResourcesUsers_Link where ResourceId = @ResourceId and userid=@userid)
                                        insert into ResourcesUsers_Link (ResourceId, UserId) 
                                        select @ResourceId, @userid";
                
                MyDAL.ExecuteNonQuery(sqlcomm, parms.ToArray(), CommandType.Text, conn);

                DataRow rw = dt.Rows[0];

                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

                HttpContext.Current.Response.ContentType = rw["MIMEType"].ToString();            // "application/octet-stream";

                if (!rw["MIMEType"].ToString().ToLower().Contains("video"))
                {
                    Byte[] bytes = (Byte[])rw["content"];
                    HttpContext.Current.Response.BinaryWrite(bytes);
                }
                else
                {
                    //Byte[] bytes = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["Resources.Video.Path"] + 
                    //                                            rw["Path"].ToString().Replace("/Resources", "") + rw["FileName"].ToString()));

                    Byte[] bytes = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(rw["Path"].ToString() + rw["FileName"].ToString()));

                    HttpContext.Current.Response.BinaryWrite(bytes);
                }

                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + rw["FileName"].ToString().Replace(",", " "));
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();

            }
        }
    }
}
