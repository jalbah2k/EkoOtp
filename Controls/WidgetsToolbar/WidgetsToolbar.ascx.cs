using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Configuration;
using System.Reflection;

public partial class Controls_WidgetsToolbar_WidgetsToolbar : System.Web.UI.UserControl
{
    #region Properties

    private string SEO
    {
        get
        {
            return CMSHelper.GetSeo();
        }
    }

    private int LoggedInID
    {
        get
        {
            int _LoggedInID = 0;
            if (Session["LoggedInID"] != null)
                int.TryParse(Session["LoggedInID"].ToString(), out _LoggedInID);
            else
            {
                string url = "/Login.aspx?r=" + SEO;
                //Response.Redirect(url);
            }

            return _LoggedInID;
        }
    }

    private string Language
    {
        get
        {
            //return (Session["Language"] != null ? Session["Language"].ToString() : "1");
            //return CMSHelper.GetCleanQueryString("lang", "1");
            return CMSHelper.GetLanguageNumber();
        }
    }

    #endregion Properties

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoggedInID"] != null && Permissions.Get(int.Parse(Session["LoggedInID"].ToString()), int.Parse(Session["PageID"].ToString())) > 1)
        {
            PagesSection pageSection = new PagesSection();
            pageSection.ValidateRequest = false;
            pageSection.EnableEventValidation = false;
            //ModifiyValidation(false);
        
        
            if (!IsPostBack)
            {
                BindWidgets();
            }
        }
    }

    /*private void ModifiyValidation(bool validate)
    {

        var pagesSection = System.Configuration.ConfigurationManager.GetSection("system.web/pages") as PagesSection;
        var httpRuntime = System.Configuration.ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;

        if (pagesSection != null && httpRuntime != null && pagesSection.ValidateRequest != validate)
        {
            var fi = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            fi.SetValue(pagesSection, false);
            fi.SetValue(httpRuntime, false);
            pagesSection.ValidateRequest = validate;
            pagesSection.EnableEventValidation = validate;
            //httpRuntime.RequestValidationMode = new Version(validate ? "4.0" : "2.0");
            fi.SetValue(pagesSection, true);
            fi.SetValue(httpRuntime, true);
        }
    }*/

    private void BindWidgets()
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("ZoneContentList", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@MYLANG", Language);
            dapt.SelectCommand.Parameters.AddWithValue("@MYUSERID", LoggedInID);
            dapt.SelectCommand.Parameters.AddWithValue("@IncludeMenu", true);
            dapt.Fill(ds);
        }

        DataView dv1 = ds.Tables[0].DefaultView;
        dv1.RowFilter = "control = 'content'";

        repContent.DataSource = dv1;
        repContent.DataBind();

        dv1.RowFilter = "control = 'banners'";

        repBannerGallery.DataSource = dv1;
        repBannerGallery.DataBind();

        dv1.RowFilter = "control = 'PhotoNanogallery'";  //"control = 'photos'";

        repPhotoGallery.DataSource = dv1;
        repPhotoGallery.DataBind();
        //pnlPhotoGallery.Visible = dv1.Count > 0;

        dv1.RowFilter = "control = 'documents'";

        repDocuments.DataSource = dv1;
        repDocuments.DataBind();
        //pnlDocuments.Visible = dv1.Count > 0;

        dv1.RowFilter = "control = 'eform'";

        repeForm.DataSource = dv1;
        repeForm.DataBind();
        pnleForm.Visible = dv1.Count > 0;

        dv1.RowFilter = "control = 'popupmessages'";

        repPopupMessages.DataSource = dv1;
        repPopupMessages.DataBind();
        //pnlPopupMessages.Visible = dv1.Count > 0;

        dv1.RowFilter = "control = 'Menu' and orientation = 'Horizontal'";
        int MenuHCount = dv1.Count;

        repMenuH.DataSource = dv1;
        repMenuH.DataBind();

        dv1.RowFilter = "control = 'Menu' and orientation = 'Vertical'";

        repMenuV.DataSource = dv1;
        repMenuV.DataBind();
        pnlMenu.Visible = MenuHCount > 0 || dv1.Count > 0;

        /*foreach (ListItem li in ddlContentControl.Items)
        {
            if (li.Text.EndsWith("9"))
                li.Text = li.Text.TrimEnd('9');
        }*/

        dv1.RowFilter = "control = 'BreakingNews'";

        repNews.DataSource = dv1;
        repNews.DataBind();

        dv1.RowFilter = "control = 'ContentRow'";
        dv1.Sort = "name";

        repTemplate.DataSource = dv1;
        repTemplate.DataBind();
    }

    protected void WidgetItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            //string name = Regex.Replace(drv["name"].ToString().Replace("PopupMessage - ", ""), drv["control"].ToString() + " - ", "", RegexOptions.IgnoreCase);
            string name = drv["name"].ToString();

            string control = drv["control"].ToString();
            if (control.ToLower() != "content")
            {
                int pos = 0;
                if ((pos = name.IndexOf(" - ")) > 0)
                    name = name.Substring(pos + 3);

                if (control.ToLower() == "menu")
                {
                    control += drv["orientation"].ToString() == "Horizontal" ? "h" : "v";
                }
            }

            Literal litItem = (Literal)e.Item.FindControl("litItem");
            litItem.Text = string.Format("<li id=\"{2}_{1}\" class=\"WidgetItem\"><img src=\"/images/lemonaid/menuicons/widgets_18x18.png\" class=\"widget_handle_icon\" alt=\"{2}\" align=\"middle\" />&nbsp;{0}</li>\n", name, drv["id"].ToString(), control);

        }
    }
}