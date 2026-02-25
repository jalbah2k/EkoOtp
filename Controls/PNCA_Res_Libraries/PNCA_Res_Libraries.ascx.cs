using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Routing;

public partial class PNCA_Res_Libraries : System.Web.UI.UserControl
{
    public string Parameter;
    public PNCA_Res_Libraries() {}
    public PNCA_Res_Libraries(string p) { Parameter = p; }

    protected string _seo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        _seo = this.Page.RouteData.Values["seo"].ToString().ToLower();

        if (
            //!IsPostBack 
            //&& 
            this.Page.RouteData.Values["id"] != null)
        {         
            string seo = "";
            if ((seo = this.Page.RouteData.Values["id"].ToString()) != "")
            {
                PopulateCat(seo);
                return;
            }

            PopulateLib();
        }
    }

    #region Library
    private void PopulateLib()
    {

        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("res.Library_sel", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count == 1)
            {
                DataRow rw = ds.Tables[1].Rows[0];
                if (rw["qty"].ToString() != "0")
                {
                    Literal litContent = new Literal();
                    string s = "<a href='{3}'><div class='{2}'><h2>{0}</h2><br><h4>Favourites</h4><br>{1}</div></a>";
                    s = String.Format(s, "My Resources", "view resources (" + rw["qty"].ToString() + ")", "myresources", "/myfavourites");

                    litContent.Text = s;
                    plMy.Controls.Add(litContent);
                }
            }

            DataView dv = dt.DefaultView;
            dv.RowFilter = "lib_type=1";
            repeaterLibrary.DataSource = dv;
            repeaterLibrary.DataBind();

            dv.RowFilter = "lib_type=2";
            repeaterLibrary_PNCA.DataSource = dv;
            repeaterLibrary_PNCA.DataBind();


            if (dt.Rows.Count > 0)
                EKO_Breadcrumbs1.Content = "Resources> <h1>Select a Library:</h1>";

            if (repeaterLibrary.Items.Count > 0)
                litEKOTitle.Text = "<h2>EKO resources</h2>";

            if (repeaterLibrary_PNCA.Items.Count > 0)
                litPNCATitle.Text = "<h2>PNCA resources</h2><a name='pnca' style='position:relative; top:-50px; visibility:hidden; opactity:0; height:0; width:0; overflow:hidden;'></a>";

        }

        pnlLibrary.Visible = true;
    }

    protected void repeaterLibrary_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("plContent");

            Literal litContent = new Literal();
            string s = "<div class='{2}'><h2>{0}</h2><span class='viewLink'>{1} {4}</span></div>";

            if (!String.IsNullOrEmpty(rw["User_id"].ToString()))
            {
                s = "<a href='{3}'>" + s + "</a>";
                s = String.Format(s,
                    rw["name"].ToString(),
                    "view categories (" + rw["qty"].ToString() + ")",
                    "unlock",
                    "/" + _seo + "/" + rw["seo"].ToString().ToLower(),
                    "");
            }
            else
                s = String.Format(s, rw["name"].ToString(), "Limited member access", "lock", "", "<div class='infoToggle'></div><div class='infoPopup'>Your account does not have sufficient permissions to access these resources</div>");

            litContent.Text = s;

            ph.Controls.Add(litContent);

        }
    }
    #endregion

    #region Category
    private void PopulateCat(string library)
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("res.Category_sel", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@lib", library);
            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());
            dapt.SelectCommand.Parameters.AddWithValue("@show", DBNull.Value);
            dapt.SelectCommand.Parameters.AddWithValue("@status", DBNull.Value);

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataView dv = dt.DefaultView;
            dv.RowFilter = "parentid=0 and qty>0";

            if (dt.Rows.Count > 0)
            {
                EKO_Breadcrumbs1.Content = String.Format("<a href='/{0}'>Resources</a>> {1} <h1> Select a Category:</h1>", _seo, dt.Rows[0]["library"].ToString());

                repeaterCategory.DataSource = dv;
                repeaterCategory.DataBind();

                this.Page.Title += " - " + dt.Rows[0]["library"].ToString();

                pnlCategory.Visible = true;


            }
            else
                Response.Redirect("/" + _seo);

        }

    }

    protected void repeaterCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("plContent");

            Literal litContent = new Literal();
            string s = "<div class='{2}'><h2>{0}</h2><span class='viewLink'>{1}</span></div>";

            if (rw["qty"].ToString() != "0")
            {
                // e.g.: https://www.empoweredkidsontario.ca/resources?library=new_OAP&Category=EKO_branding&subcategory=unset&search_term=styleguide
                string url = "/" + ConfigurationManager.AppSettings["Resources.Page"] + "?" + String.Format("library={0}&category={1}", rw["libseo"].ToString(), rw["catseo"].ToString());

                string qty = Convert.ToString(Convert.ToInt32(rw["qty"]));

                s = String.Format("<a href='{0}'>", url.ToLower()) + s + "</a>";
                s = String.Format(s,
                    rw["name"].ToString(),
                    "view resources (" + qty + ")",
                    "unlock");

            }
            else
            {
                s = String.Format(s, 
                    rw["name"].ToString(), 
                    "resources (0)", 
                    "lock");
            }

            litContent.Text = s;

            ph.Controls.Add(litContent);

        }
    } 
    #endregion
}