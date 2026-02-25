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

public partial class Admin_Controls_Pie_LibrariesPartial : System.Web.UI.UserControl
{
    public string ResourceID
    {
        set { ViewState["resourceid"] = value; }
        get
        {
            string ret = "";
            if (ViewState["resourceid"] != null)
                ret = ViewState["resourceid"].ToString();
            return ret;
        }
    }

    public int ResourcesGroupId
    {
        set { ViewState["ResourcesGroupId"] = value; }
        get { return ViewState["ResourcesGroupId"] != null ? int.Parse(ViewState["ResourcesGroupId"].ToString()) : 0; }
    }

    public DataTable dtCategories
    {
        set { ViewState["dtCategories"] = value; }
        get { return (DataTable)ViewState["dtCategories"]; }
    }

    public DataTable dtSubCategories
    {
        set { ViewState["dtSubCategories"] = value; }
        get { return (DataTable)ViewState["dtSubCategories"]; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    private void Clear()
    {
        ResourceID = "";
        ViewState["ResourcesGroupId"] = null;
        DataTable dt = new DataTable();
        repLibraries.DataSource = dt;
        repLibraries.DataBind();

        dtCategories = null;
        dtSubCategories = null;

        this.Visible = false;
    }
    public void Populate(string resid)
    {
        Clear();
        ResourceID = resid;
        Populate();

    }

    public void Populate(int libid)
    {
        Clear();
        ResourcesGroupId = libid;
        Populate();

    }

    private void Populate()
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("ResourcesGroupsCateg_Select_New", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            if(ResourceID != "")
                dapt.SelectCommand.Parameters.AddWithValue("@resid", ResourceID);

            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];
            dtCategories = ds.Tables[1];
            dtSubCategories = ds.Tables[2];

            if (dt.Rows.Count > 0)
            {
                this.Visible = true;
                repLibraries.DataSource = dt;
                repLibraries.DataBind();
            }

        }
    }
    protected void repLibraries_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("plLibrary");

            int cat = 0;
            int.TryParse(rw["categ_qty"].ToString(), out cat);

            CheckBox cb = new CheckBox();
            cb.ID = "fld_lib_" + rw["id"].ToString();
            cb.TextAlign = TextAlign.Right;
            cb.Text = rw["name"].ToString() + " (" + cat.ToString() + ")";

            cb.CssClass = "rb-enhanced rb-res-lib";


            if (Convert.ToBoolean(rw["BelongsToLibray"]) ||                             //Editing resource
               ResourcesGroupId == int.Parse(rw["id"].ToString())                       //Adding resource           
               )
            {
                cb.Checked = true;
            }

            HtmlGenericControl divRow = new HtmlGenericControl("div");
            if (cb.Checked && cat > 0)
            {
                divRow.Attributes.Add("class", "minus categ");
            }
            else if(cat > 0)
                divRow.Attributes.Add("class", "plus categ");

            if (cat > 0)
            {
                string parentid = library_id = rw["id"].ToString();
                Repeater repCategories = (Repeater)e.Item.FindControl("repCategories");
                BindCategories(repCategories, dtCategories, "ResourcesGroupId = " + parentid);
            }

      
            HtmlGenericControl divCb = new HtmlGenericControl("div");
            divCb.Attributes.Add("class", "div-cb");
            divCb.Controls.Add(cb);


            divRow.Controls.Add(divCb);
            ph.Controls.Add(divRow);

        }
    }

    string library_id;
    private void BindCategories(Repeater repeater, DataTable dt, string rowfilter)
    {
        DataView dv = dt.DefaultView;
        dv.RowFilter = rowfilter;
        dv.Sort = "priority";
        repeater.DataSource = dv;
        repeater.DataBind();
    }

    protected void repCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("plCategory");

            int cat = 0;
            int.TryParse(rw["subcateg_qty"].ToString(), out cat);

            CheckBox cb = new CheckBox();
            cb.ID = "fld_cat_" + library_id + "_" + rw["ResourceTypeId"].ToString();
            cb.TextAlign = TextAlign.Right;
            cb.Text = rw["categ"].ToString();

            if(cat > 0)
                cb.Text += " (" + cat.ToString() + ")";

            cb.CssClass = "rb-enhanced rb-res-cat";


            if (Convert.ToBoolean(rw["BelongsToCateg"]) || Convert.ToBoolean(rw["BelongsToSubcateg"]))
            {
                cb.Checked = true;
            }

            HtmlGenericControl divRow = new HtmlGenericControl("div");
            if (cb.Checked && cat > 0)
            {
                divRow.Attributes.Add("class", "minus categ");
            }
            else if (cat > 0)
                divRow.Attributes.Add("class", "plus categ");

            if (cat > 0)
            {
                string parentid = rw["id"].ToString();
                Repeater repCategories = (Repeater)e.Item.FindControl("repSubcategories");
                BindCategories(repCategories, dtSubCategories, "id = " + parentid + " and ResourcesGroupId = " + library_id);
            }


            HtmlGenericControl divCb = new HtmlGenericControl("div");
            divCb.Attributes.Add("class", "div-cb");
            divCb.Controls.Add(cb);


            divRow.Controls.Add(divCb);
            ph.Controls.Add(divRow);


        }
    }

    protected void repSubcategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView rw = (DataRowView)e.Item.DataItem;
            PlaceHolder ph = (PlaceHolder)e.Item.FindControl("plSubCategory");

            CheckBox cb = new CheckBox();
            cb.ID = "fld_subcat_" + library_id + "_" + rw["subtypeid"].ToString();
            cb.TextAlign = TextAlign.Right;
            cb.Text = rw["subcateg"].ToString();

            cb.CssClass = "rb-enhanced rb-res-subcat";

            if (Convert.ToBoolean(rw["BelongsToSubcateg"]))
            {
                cb.Checked = true;
            }

            HtmlGenericControl divRow = new HtmlGenericControl("div");
 
            HtmlGenericControl divCb = new HtmlGenericControl("div");
            divCb.Attributes.Add("class", "div-cb");
            divCb.Controls.Add(cb);


            divRow.Controls.Add(divCb);
            ph.Controls.Add(divRow);


        }
    }
}