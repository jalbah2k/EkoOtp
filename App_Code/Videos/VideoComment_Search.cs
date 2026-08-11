using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for VideoComments
/// </summary>
public class VideoComment_Search
{
    private FullTextSearch.FullTextSearch myFTS;
    private string[] LCIDs = { "1033", "1036", "1034" };  //1033 - English USA; 3084 - French France; 1034: Spanish Spain

    private SqlDataAdapter dapt;

    public int VideoId { set; get; }
    public int ParentId { set; get; }
    public string Keywords { set; get; }
    public int Language { set; get; }
    public VideoComment_Search()
    {
        Language = 1;
        VideoId = 1;
        ParentId = 0;
    }
    public VideoComment_Search(int videoid, string CommandText, CommandType commType, int lang)
    {
        Language = lang;
        VideoId = videoid;
        dapt = new SqlDataAdapter(new SqlCommand(CommandText));
        dapt.SelectCommand.CommandType = commType;
        dapt.SelectCommand.Parameters.AddWithValue("@LCID", LCIDs[lang - 1]);
        dapt.SelectCommand.Parameters.AddWithValue("@LanguageId", Language);

    }

    public SqlDataAdapter Build()
    {
        if (Keywords != null && Keywords != "")
        {
            string searchTerm = Keywords.Trim();

            searchTerm = FTSAux.RemoveNoiseWords(searchTerm, LCIDs[Language - 1]);
            myFTS = new FullTextSearch.FullTextSearch(searchTerm);

            dapt.SelectCommand.Parameters.Add(new SqlParameter("@keywords", myFTS.NormalForm));
            dapt.SelectCommand.Parameters.Add(new SqlParameter("@searchTerm", searchTerm));

        }

        dapt.SelectCommand.Parameters.Add(new SqlParameter("@videoid", VideoId));
        dapt.SelectCommand.Parameters.Add(new SqlParameter("@parentid", ParentId));

        return dapt;
    }

    public string GetResults( bool is_admin = true, int parentid = 0)
    {
        string res = "";
        ParentId = parentid;

        SqlDataAdapter da = Build();
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            da.SelectCommand.Connection = conn;
            da.Fill(ds);
        }

        int len = 100;

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string id = dr["id"].ToString();
            string username = dr["username"].ToString();
            char letter = username.Substring(0, 1).ToUpper()[0];
            string header = "<span class='sp-letter' style='background-color:{0}'>{1}</span>&nbsp;&nbsp;<span class='sp-header'>{2}&nbsp;&#149;&nbsp;{3}</span>";
            header = String.Format(header, GenerateHexColorFromLetter(letter), letter.ToString(), username, GetTimeStasmp(dr));


            string temp = dr["Comment"].ToString();
            if (temp.Length > len)
                temp = temp.Remove(len) + String.Format("<a href='#rm_{0}' class='read-more' title='read more'>...</a>", id);

            string s = "<div class='div-comment'>" + header + "<div><p>" + temp + "</p></div>";

            s += "<div class='div-sp-link'>";
            int qty = Convert.ToInt32(dr["qty"]);
            if (qty > 0)
            {
                string replies = "<div><a href='#sp_{0}' class='sp-link' videoid='{2}' title='view replies' >{1}</a></div>";
                if (is_admin)
                    replies = replies.Replace("sp-link", "sp-link-admin");
                replies = String.Format(replies, id, qty.ToString() + (qty == 1 ? " reply" : " replies") + " ↓", VideoId);
                s += replies;
            }
            else
                s += "<div></div>";

            if (!is_admin && Convert.ToBoolean(ds.Tables[0].Rows[0]["AllowComments"]))
                s += String.Format("<div><a href='#addcomment_{0}' class='icon_comment' videoid='{1}' title='add reply'><img src='/images/icons/comment.png' alt='comment icon' /></a></div>", id, VideoId);
            else
                s += "<div></div>";

            if (is_admin)
                s += String.Format("<div><a href='#bin_{0}' class='del-comment' title='Delete comment' parentid='{1}'><img src='/images/lemonaid/buttonsNew/ex.png' alt='delete comment' /></a></div>", id, parentid);

            s += "</div></div>";

            res += s;
        }

        return res;
    }

    public string GetHeader( bool is_admin = true, int parentid = 0)
    {
        string res = "";

        int qty = 0;
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("video.Get_CommentQty", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@id", VideoId);

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
                qty = Convert.ToInt32(dt.Rows[0]["total"]);
        }

        string span = String.Format("<span class='sp-total-comments'><span class='sp-total-comments-qty'>{0}</span> {1}</span>",
                        qty > 0 ? qty.ToString() : "",
                        qty > 0 ? (qty > 1 ? "comments" : "comment") : ""
                        );

        if (!is_admin && Convert.ToBoolean( ds.Tables[1].Rows[0]["AllowComments"]))
        {
            res = String.Format(@"<div class='div-sp-comment'>
                                                <div>{2}</div>
                                                <div><input type='button' value='Add comment' id='btnAddComment_{0}' class='button' videoid='{0}' parentid='{1}' /></div>
                                                <div></div>
                                                </div><br>",
                                VideoId,
                                parentid,
                                span
                                );
        }
        else
        {
            res = String.Format(@"<div class='div-sp-comment'>
                                                <div>{0}</div>
                                                <div></div>
                                                <div></div>
                                                </div><br>",
                                span
                                );
        }

        return res;
    }
    public static string GetTimeStasmp(DataRow dr)
    {
        string timestamp = "";
        int deltaminutes = Convert.ToInt32(dr["deltaminutes"]);
        if (deltaminutes < 2)
            timestamp = "1 minute ago";
        else if (deltaminutes < 60)
            timestamp = deltaminutes.ToString() + " minutes ago";
        else
        {   //more than 60 min

            int deltahours = Convert.ToInt32(dr["deltahours"]);
            if (deltahours < 2)
                timestamp = "1 hour ago";
            else if (deltahours < 23)
                timestamp = deltahours.ToString() + " hours ago";
            else
            {
                // more than 24 hours
                int deltadays = Convert.ToInt32(dr["deltadays"]);
                if (deltadays < 2)
                    timestamp = "1 day ago";
                else if (deltadays < 8)
                    timestamp = deltadays.ToString() + " days ago";
                else
                {   //more than 7 days
                    int deltaweeks = Convert.ToInt32(dr["deltaweeks"]);
                    if (deltaweeks < 2)
                        timestamp = "1 week ago";
                    else if (deltaweeks < 5)
                        timestamp = deltaweeks.ToString() + " weeks ago";
                    else
                    {
                        int deltamonths = Convert.ToInt32(dr["deltamonths"]);
                        if (deltamonths < 2)
                            timestamp = "1 month ago";
                        else if (deltaweeks < 5)
                            timestamp = deltamonths.ToString() + " months ago";
                    }
                }
            }
        }

        return timestamp;
    }

    public static string GenerateHexColorFromLetter(char letter)
    {
        return ColorTranslator.ToHtml(GetColorFromLetter(letter));
    }
    private static Color GetColorFromLetter(char letter)
    {
        // Convert the letter to its ASCII value
        int asciiValue = (int)letter;

        // Use the ASCII value to generate RGB values
        int red = (asciiValue * 3) % 256;
        int green = (asciiValue * 5) % 256;
        int blue = (asciiValue * 7) % 256;

        // Create and return the color
        return Color.FromArgb(red, green, blue);
    }
}
  