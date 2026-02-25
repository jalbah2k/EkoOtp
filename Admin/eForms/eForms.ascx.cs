using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;



public partial class Admin_eForms_eForms : System.Web.UI.UserControl
{
    string _group = "Common";
    string _language = "English";
    string _sql = "select id,name from groups where id in (select group_id from users_groups_access where user_id={0} and access_level>1) order by name";

    public string sortExp
    {
        get
        {
            return ViewState["sortExp"] != null ? ViewState["sortExp"].ToString() : "";
        }
        set
        {
            ViewState["sortExp"] = value;
        }
    }
    public string sortOrder
    {
        get
        {
            return ViewState["sortOrder"] != null ? ViewState["sortOrder"].ToString() : "desc";
        }
        set
        {
            ViewState["sortOrder"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            sortExp = "Name"; // default sorted column
            sortOrder = "asc";    // default sort order

            if ((bool)Session["Multilingual"])
            {
                trLanguage.Visible = true;
                trLanguage2.Visible = true;
            }

            string _groups = String.Format(_sql, Session["LoggedInID"]);
            SqlDataAdapter dapt = new SqlDataAdapter(_groups, ConfigurationManager.AppSettings["CMServer"]);
            DataSet ds = new DataSet();
            dapt.Fill(ds);
            ddlGroup.DataSource = ds.Tables[0];
            ddlGroup.DataBind();

            ListItem li = ddlGroup.Items.FindByText(_group);
            if (li != null)
                li.Selected = true;

            DataTable dt = eFormDAL.getTable("BASE_Languages_Get");

            ddlLanguage.DataSource = dt;
            ddlLanguage.DataTextField = "name";
            ddlLanguage.DataValueField = "id";
            ddlLanguage.DataBind();
            li = ddlLanguage.Items.FindByText(_language);
            if (li != null)
            {
                ddlLanguage.ClearSelection();
                li.Selected = true;
            }

            ddlLanguageF.DataSource = dt;
            ddlLanguageF.DataTextField = "name";
            ddlLanguageF.DataValueField = "id";
            ddlLanguageF.DataBind();
            li = ddlLanguageF.Items.FindByText(_language);
            if (li != null)
            {
                ddlLanguageF.ClearSelection();
                li.Selected = true;
            }

            if (Request["id"] != null)
            {
                Session["ViewSubmission"] = Request["id"];
                Response.Redirect(Request.Url.AbsoluteUri.Replace("&id="+ Request["id"],""));
            }
            else if (Session["ViewSubmission"] != null)
            {
                SwitchView("Submission");
                string sql = string.Format("select b.* from eFormSubmissions a join eForms b on a.formid=b.id where a.id={0}", Session["ViewSubmission"]);
                SqlDataAdapter da = new SqlDataAdapter(sql, eFormDAL._connection);
                DataTable dt1 = new DataTable();
                da.Fill(dt1);
                if (dt.Rows.Count > 0)
                {
                    DataRow rw = dt1.Rows[0];
                    Hashtable he1 = new Hashtable();
                    he1.Add("formid", rw["id"].ToString());
                    he1.Add("name", rw["name"].ToString());
                    he1.Add("title", rw["title"].ToString());
                    he1.Add("language", rw["language"].ToString());
                    he1.Add("submissionid", Session["ViewSubmission"].ToString());
                    this.eSubmission.InitME(he1);
                }

                
                Session.Remove("ViewSubmission");
            }
            else
                SwitchView("View");

        }
        BindData();
    }

    public void PageSizeChange(object o, EventArgs e)
    {
        GV_Main.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        GV_Main.PageIndex = 0;

        BindData();
    }
    public void pager_Command(object sender, CommandEventArgs e)
	{
		int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
		pager1.CurrentIndex = currnetPageIndx;
		GV_Main.PageIndex = currnetPageIndx - 1;

		BindData();
	}

    private void BindData()
    {
        BindData(sortExp, sortOrder);
    }
    private void BindData(string sortExp, string sortDir)
    {
        DataTable dt = new DataTable();
        //        Session["LoggedInID"].ToString()
        //SqlParameter[] prms = { new SqlParameter("@uid", Session["LoggedInID"].ToString()) };
        List<SqlParameter> prms = new List<SqlParameter>();
        prms.Add(new SqlParameter("@uid", Session["LoggedInID"].ToString()));
        if (!(bool)Session["Multilingual"])
            prms.Add(new SqlParameter("@lang", 1));
        dt = eFormDAL.getTable(sqlSelect, prms);

        DataView DV = dt.DefaultView;
        if (sortExp != string.Empty)
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }
        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

        pager1.ItemCount = DV.Count;
        pager1.Visible = DV.Count > GV_Main.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Main, DV.Count);

        pnlGriedView.Visible = DV.Count > 0;
        pnlListEmpty.Visible = DV.Count == 0;
    }

    private void SwitchView()
    {
        string view = CurrentView;
        pnleFormsList.Visible = view == "View";
        btAdd_eForms.Visible = view == "View";
        pnlAddeForm.Visible = view == "Add";
        pnlEditeForm.Visible = view == "Edit";
        pnleForm.Visible = view == "Content";
        pnleFormSubmission.Visible = view == "Submission";
    }
    private void SwitchView(string view)
    {
        CurrentView = view;
        SwitchView();
    }

    protected void btCopy_Click(object sender, EventArgs e)
    {
        string formID = ((ImageButton)sender).CommandArgument;
        SqlParameter[] prms = { 
            new SqlParameter("@id", formID)};
        eFormDAL.ProcessRecord(sqlCopy, prms);
        BindData();
    }

    #region dal
    protected string sqlSelect = "eFormsSelect";
    protected string sqlDelete = "eFormDelete";
    protected string sqlUpdate = "eFormUpdate";
    protected string sqlInsert = "eFormCreate";
    protected string sqlCopy = "eFormCopy";
    #endregion dal

    #region Grid_Gallery

    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Language column
        e.Row.Cells[1].Visible = (bool)Session["Multilingual"];

        #region Add sorted class to headers
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExp);
            e.Row.Cells[colIndex].CssClass = sortOrder == "asc" ? "sortasc" : "sortdesc";
        }
        #endregion Add sorted class to headers

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ibDelete = (ImageButton)e.Row.FindControl("lbDelete");
            //ImageButton ibSubmission = (ImageButton)e.Row.FindControl("lbSubmissions");
            if (ibDelete != null)
                ibDelete.Attributes.Add("OnClick", "return confirm('Are you sure to delete this Form?');");
            //if (ibSubmission != null)
            //    ibSubmission.CommandArgument = e.Row.DataItemIndex.ToString();
        }
    }

    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Select":
                eFormID = e.CommandArgument.ToString();
                break;
            case "Submissions":
                //test = e.CommandArgument.ToString();
                SwitchView("Submission");
                this.eSubmission.InitME(eFormHash(GetRowID(e.CommandArgument.ToString())));
                break;
            case "Content":
                //test = GetRowID(e.CommandArgument.ToString()).ToString();
                ibPreview.OnClientClick = "window.open('/admin/eforms/preview.aspx?previewid=" + e.CommandArgument.ToString() + "', 'preview', 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, resizable'); return false;";
                SwitchView("Content");
                Session["eForm"] = eFormHash(GetRowID(e.CommandArgument.ToString()));
                this.eFormFields.InitME();
                break;
        }
    }

    private int GetRowID(string _id)
    {
        int id = -1;
        for (int i = 0; i < GV_Main.Rows.Count; i++)
        {
            if (_id == GV_Main.DataKeys[i].Value.ToString())
            {
                id = i;
                break;
            }
        }
        return id;
    }

    protected string test;
    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        eFormID = GV_Main.DataKeys[e.NewEditIndex].Value.ToString();
        PopulateFormFields();
        SwitchView("Edit");
    }

    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        eFormDAL.RemoveRecord(sqlDelete, GV_Main.DataKeys[e.RowIndex].Value.ToString());
        this.BindData();
    }

    private void PopulateFormFields()
    {
        SqlParameter[] prms = { new SqlParameter("@id", eFormID) };
        DataTable dtf = eFormDAL.getTable("eFormsSelect", prms);

        DataView dv = new DataView(eFormDAL.getTable("GroupsSelect"));
        dv.Sort = "name";
        ddlGroupF.DataSource = dv;
        ddlGroupF.DataBind();

        ListItem li = ddlGroupF.Items.FindByValue(dtf.Rows[0]["groupid"].ToString());
        if (li != null)
            li.Selected = true;

        ListItem li2 = this.ddlLanguageF.Items.FindByValue(dtf.Rows[0]["language"].ToString());
        if (li2 != null)
        {
            ddlLanguageF.ClearSelection();
            li2.Selected = true;
        }

        tbEmailF.Text = dtf.Rows[0]["email"].ToString();
        bool _email = false;
        bool.TryParse(dtf.Rows[0]["sendemail"].ToString(), out _email);
        cbEmailF.Checked = _email;

        bool _captcha = false;
        bool.TryParse(dtf.Rows[0]["captcha"].ToString(), out _captcha);
        cbCaptchaF.Checked = _captcha;

        bool _popup = false;
        bool.TryParse(dtf.Rows[0]["popup"].ToString(), out _popup);
        cbPopupF.Checked = _popup;



        tbNameF.Text = dtf.Rows[0]["name"].ToString();
        tbTitleF.Text = dtf.Rows[0]["title"].ToString();
    }

    private Hashtable eFormHash(int index)
    {
        Hashtable he = new Hashtable();
        he.Add("formid", GV_Main.DataKeys[index].Value.ToString());
        he.Add("name", GV_Main.DataKeys[index].Values[1].ToString());
        he.Add("title", GV_Main.DataKeys[index].Values[1].ToString());
        he.Add("language", GV_Main.DataKeys[index].Values[4].ToString());
        return he;
    }

    protected void GV_Main_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExp = e.SortExpression;

        if (sortOrder == "desc")
            sortOrder = "asc";
        else
            sortOrder = "desc";
        BindData(e.SortExpression, sortOrder);
    }
    #endregion Grid_Gallery

    protected void btAdd_eForms_Click(object sender, EventArgs e)
    {
        SwitchView("Add");
    }

    protected void imgSubmit_Click(object sender, EventArgs e)
    {
        SwitchView("View");
        AddeForm();
        CleaneForm();
        BindData();
    }

    private void AddeForm()
    {
        SqlParameter[] prms = { 
            new SqlParameter("@Name", tbeFormName.Text.Trim()),
            new SqlParameter("@Title", tbeFormTitle.Text.Trim()),
            new SqlParameter("@email", tbEmail.Text.Trim()),
            new SqlParameter("@sendemail",cbEmail.Checked),
            new SqlParameter("@language", ddlLanguage.SelectedValue),
            new SqlParameter("@group", ddlGroup.SelectedValue),
            new SqlParameter("@captcha", cbCaptcha.Checked),
            new SqlParameter("@popup", cbPopup.Checked)
                              };
        eFormID = eFormDAL.ProcessRecord(sqlInsert, prms);
    }

    private void CleaneForm()
    {
        ListItem li = ddlGroup.Items.FindByText(_group);
        if (li != null)
            li.Selected = true;
        li = ddlLanguage.Items.FindByText(_language);
        if (li != null)
        {
            ddlLanguage.ClearSelection();
            li.Selected = true;
        }
        tbeFormName.Text = "";
        tbeFormTitle.Text = "";
        tbEmail.Text = "";
        cbEmail.Checked = false;
        cbCaptcha.Checked = false;
        cbPopup.Checked = false;
    }

    private void UpdateForm()
    {
        SqlParameter[] prms = { 
            new SqlParameter("@id", eFormID),
            new SqlParameter("@Name", tbNameF.Text.Trim()),
            new SqlParameter("@Title", tbTitleF.Text.Trim()),
            new SqlParameter("@email", tbEmailF.Text.Trim()),
            new SqlParameter("@sendemail",cbEmailF.Checked),
            new SqlParameter("@language", ddlLanguageF.SelectedValue),
            new SqlParameter("@groupid", ddlGroupF.SelectedValue),
            new SqlParameter("@captcha", cbCaptchaF.Checked),
            new SqlParameter("@popup", cbPopupF.Checked)
                              };
        eFormID = eFormDAL.ProcessRecord(sqlUpdate, prms);
    }


    protected void imgBack_Click(object sender, EventArgs e)
    {
        SwitchView("View");
        BindData();
    }

    protected void imgSubmitF_Click(object sender, EventArgs e)
    {
        SwitchView("View");
        this.GV_Main.EditIndex = -1;
        UpdateForm();
        BindData();
    }

    protected void imgBackF_Click(object sender, EventArgs e)
    {
        SwitchView("View");
        this.GV_Main.EditIndex = -1;
        BindData();
    }


    protected string eFormID
    {
        set { ViewState["eFormID"] = value; }
        get { return (string)ViewState["eFormID"]; }
    }

    protected string CurrentView
    {
        set { Session["CurrentView"] = value; }
        get { return (Session["CurrentView"] == null ? "View" : Session["CurrentView"].ToString()); }
    }
}
