//#define IMG_TAG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class QuickLinks : System.Web.UI.UserControl
{
    public string parameter;
    #region DAL

    StringBuilder sb;

	public QuickLinks()
	{
		
	}

	public QuickLinks(string s)
	{
        parameter = s;

    }

    public DataTable mGet_All_QuickLinks(string lang_id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from QuickLinks where lang_id = @lang_id and active=1 and groupid=@id order by priority";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", parameter);
            cmd.Parameters.AddWithValue("@lang_id", lang_id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

  
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        sb = new StringBuilder(60);
        
        if (!IsPostBack)
        {
            //to delete
            //Session["language"] = "1";


            Load_DDLQuickLinks(Session["Language"].ToString());

            //to be delete
            //Session["user_id"] = "-1";
        }
    }

    #region My_Functions


    void Load_DDLQuickLinks(string lang_id)
    {

        litStyles.Text = "<style>";

        DataTable dt = new DataTable();
        dt = mGet_All_QuickLinks(lang_id);
        Repeater1.DataSource = dt;
        Repeater1.DataBind();

        litStyles.Text += "</style>";

        if (dt.Rows.Count == 0)
            litDummy.Text = "&nbsp;";

        //this.DDL_QuickLinks.DataSource = dt;
        //this.DDL_QuickLinks.DataTextField = "name";
        //this.DDL_QuickLinks.DataValueField = "url";
        //this.DDL_QuickLinks.DataBind();

            //if (Session["Language"].ToString() == "1")
            //{
            //	this.DDL_QuickLinks.Items.Insert(0, new ListItem("I would like to", "none"));
            //}
            //else
            //{
            //	this.DDL_QuickLinks.Items.Insert(0, new ListItem("Comment puis-je t'aider?", "none"));
            //}
    }

    #endregion

    protected void DDL_QuickLinks_SelectedIndexChanged(object sender, EventArgs e)
    {
        //String mURL = this.DDL_QuickLinks.SelectedValue.ToString();
        //if (mURL != "none")
        //{
        //    Response.Redirect(mURL);
        //}
    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.DataItem != null)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            Literal litLink = (Literal)e.Item.FindControl("litLink");

            string QLImagesPath = "/data/QuickLinks/" + parameter + "/";

#if IMG_TAG
            litLink.Text = String.Format("<a href='{0}' target='{2}' title='{1} link' class='qLinkIcon'>{3}<span>{1}</span></a>", 
                dr["url"].ToString(), 
                dr["name"].ToString(),
                dr["target"].ToString(), 
                
                (!String.IsNullOrEmpty(dr["image"].ToString()) ? String.Format("<img src='{0}' alt='{1}' class='qlk-icon'>", QLImagesPath + dr["image"].ToString(), dr["name"].ToString() + " icon") : "")
                +
                (!String.IsNullOrEmpty(dr["image2"].ToString()) ? String.Format("<img src='{0}' alt='{1}' class='qlk-hover'>", QLImagesPath + dr["image2"].ToString(), dr["name"].ToString() + " hover") : "")
                 
                );
            
#else
            string myid = "quickLinkIcon_" + dr["id"].ToString();
            litStyles.Text += "#" + myid + ":before {" + String.Format("background: url('{0}{1}') no-repeat center left!important;background-size: contain!important;", 
                QLImagesPath, dr["image"].ToString()) + "}" + Environment.NewLine;
            litStyles.Text += "#" + myid + ":hover:before {" + String.Format("background: url('{0}{1}') no-repeat center left!important;background-size: contain!important;",
                QLImagesPath, dr["image2"].ToString()) + "}" + Environment.NewLine;

            string s = "<a href='{0}' target='{2}' title='{3}' class='qLinkIcon' id='{4}'>{1}{5}</a>";
            string spanvalue = "";

            if (dr["url"].ToString().ToLower().Contains("membership/mymessages"))
            {
                string strConnectionString = ConfigurationManager.ConnectionStrings["yafnet"].ToString();
                string commandString = @"select count(*) as qty from yaf_UserPMessage where (flags & 1) = 0 and UserID in 
                                            (select UserID from yaf_User where ProviderUserKey in 
                                            (select yaf_userid COLLATE SQL_Latin1_General_CP1_CI_AS from EKO.dbo.Users where id=@userid))";
                DataTable dt = new DataTable();

                using (SqlConnection connection = new SqlConnection(strConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(commandString, connection);
                    cmd.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        int qty = Convert.ToInt32(dt.Rows[0][0]);
                        if (qty > 0)
                        {
                            s = "<a href='{0}' target='{2}' title='{3}' class='qLinkIcon' id='{4}'>{1} <span>{5}</span></a>";
                            spanvalue = qty.ToString();
                        }
                    }
                }

            }
            else if (dr["url"].ToString().ToLower().Contains("/empowerbci"))
            {
                s = "<a href='{0}' target='{2}' title='{3}' class='qLinkIcon' id='{4}'>{1}</a>";

                litLink.Text = String.Format(s,
                    dr["url"].ToString(),
                    "<img src='/images/Empower BCI Identifier-Simplified.png' alt='' />",
                    dr["target"].ToString(),
                    "",
                    myid
                );

                return;
            }

            litLink.Text = String.Format(s,
                dr["url"].ToString().Replace("/logout.aspx", "javascript: LogOutClick()").Replace("/logout", "javascript: LogOutClick()"),
                dr["name"].ToString(),
                dr["target"].ToString(),
                dr["name"].ToString() + " icon",
                myid,
                spanvalue
                );
#endif
        }
    }
}
