using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuteEditor;

public partial class Admin_MemberDirectory_ListAndDetails : System.Web.UI.UserControl
{
    #region Properties
    protected string MyId
    {
        get
        {
            return ViewState["MyId"] != null ? ViewState["MyId"].ToString() : "0";
        }
        set
        {
            ViewState["MyId"] = value;
        }
    }
	
	 protected string OrgId
    {
        get
        {
            return ViewState["OrgId"] != null ? ViewState["OrgId"].ToString() : "0";
        }
        set
        {
            ViewState["OrgId"] = value;
        }
    }

    protected string imgPath { set; get; }
    protected string sortExp
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
    protected string sortOrder
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
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.MaintainScrollPositionOnPostBack = true;

        imgPath = ConfigurationManager.AppSettings["Organizations.PNCA.Logo.Path"];        

        if (!IsPostBack)
        {
            BindDDLs();
            BindData();
        }
    }

    private void BindDDLs()
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select * from pnca.regions order by name; " +
                "select * from dbo.Organizations where id != @id order by name", conn);
            dapt.SelectCommand.Parameters.Add(new SqlParameter("id", ConfigurationManager.AppSettings.Get("Organizations.CMEY.ID")));
            dapt.SelectCommand.CommandType = CommandType.Text;
            dapt.Fill(ds);

            ddlRegionFilter.DataSource = ds.Tables[0];
            ddlRegionFilter.DataBind();
            ddlRegionFilter.Items.Insert(0, "");

            ddlRegion.DataSource = ds.Tables[0];
            ddlRegion.DataBind();
            ddlRegion.Items.Insert(0, "");
		
            ddlOrganization.DataSource = ds.Tables[1];
            ddlOrganization.DataBind();
            ddlOrganization.Items.Insert(0, "");

		}
    }

    #region Grid Panel
    private void BindData(string SortExpresion, string SortOrder)
    {
        sortExp = SortExpresion;
        sortOrder = SortOrder;
        BindData();

    }
    private void BindData()
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("pnca.Organizations_Search", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

            string searchTerm = txtFilter.Text.Replace("&amp;", "&");

            if (searchTerm.Length > 0)
            {
                // --------- LCID ------------

                searchTerm = RemoveNoiseWords(searchTerm, LCIDs[0]);

                FullTextSearch.FullTextSearch myFTS = new FullTextSearch.FullTextSearch(searchTerm);


                //if (searchTerm.Length > 0)
                if (!searchTerm.Contains("hospital"))
                {
                    dapt.SelectCommand.Parameters.Add(new SqlParameter("@keywords", myFTS.NormalForm));
                    dapt.SelectCommand.Parameters.Add(new SqlParameter("@LCID", LCIDs[0]));
                }
            }

            dapt.Fill(dt);
        }

        DataView DV = dt.DefaultView;
        if (!(sortExp == string.Empty))
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortOrder);
        }

        if (ddlRegionFilter.SelectedValue != "")
            DV.RowFilter = "RegionId=" + ddlRegionFilter.SelectedValue;


        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

        pager1.ItemCount = DV.Count;
        pager1.Visible = dt.Rows.Count > GV_Main.PageSize;

    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindData();
    }


    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFilter.Text = "";
        ddlRegionFilter.ClearSelection();
        BindData();
    }
    protected void bttn_Add_Click(object sender, EventArgs e)
    {
        ClearFields();
        ddlRegion.Enabled = true;
        SetView("pnlDetails");
        btnDone.Visible = btnDone2.Visible = false;

    }

    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton lb;
            lb = (ImageButton)e.Row.FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this Organization?');");

            DataRowView dr = (DataRowView)e.Row.DataItem;

            CheckBox cbxActive = (CheckBox)e.Row.FindControl("cbxActive");
            cbxActive.Checked = Convert.ToBoolean(dr["active"]);
        }
    }

    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName.ToLower())
        {
            case "delete":
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
                {
                    SqlCommand command = new SqlCommand("pnca.Organizations_Del", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", e.CommandArgument.ToString());

                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();                  
                }
                BindData();
                break;

            case "editing":
                ClearFields();
                MyId = e.CommandArgument.ToString();
                PopulateDetails(MyId);
                break;

            case "services":
                MyId = e.CommandArgument.ToString();
                PopulateServices(MyId);
                break;

            default:
                break;
        }
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

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GV_Main.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        this.GV_Main.PageIndex = 0;
        BindData();
    }

    protected void pager1_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pager1.CurrentIndex = currnetPageIndx;
        GV_Main.PageIndex = currnetPageIndx - 1;

        BindData();
    }
    #endregion

    #region Details Panel
    private void PopulateDetails(string id)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("pnca.Organizations_Sel", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.Add(new SqlParameter("@id", id));

            DataSet ds = new DataSet();
            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            OrgId = "0";
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                OrgId = dr["OrganizationId"].ToString() != "" ? dr["OrganizationId"].ToString() : "0";
                
                tbEmail.Text = dr["Email"].ToString();
                tbUrl.Text = dr["Website"].ToString();
                tbAdminContact.Text = dr["AdminContact"].ToString();

               // tbTitle.Text = dr["Name"].ToString();
                ddlOrganization.SelectedValue = dr["OrganizationId"].ToString();
                litTitle.Text = dr["Name"].ToString();

                ddlRegion.SelectedValue = dr["RegionId"].ToString();

                cbActive.Checked = Convert.ToBoolean(dr["active"]);

                if (lnkDeleteLogo.Visible = imgLogo.Visible = dr["Logo"].ToString() != "")
                {
                    imgLogo.ImageUrl = imgPath + dr["id"].ToString() + "/" + dr["Logo"].ToString();
                    imgLogo.AlternateText = dr["AltTextLogo"].ToString();
                }


                btnDone.Visible = btnDone2.Visible = true;

                ddlRegion.Enabled = ds.Tables[1].Rows.Count == 0;

                SetView("pnlDetails");
            }           
        }

    }

    private void ClearFields()
    {
        MyId = "0";
        OrgId = "0";


        //tbTitle.Text = "";
        tbAdminContact.Text = "";
        tbEmail.Text = "";
        ddlOrganization.ClearSelection();
        litTitle.Text = "";
        tbUrl.Text = "";
        ddlRegion.ClearSelection();
        imgLogo.ImageUrl = "";
        imgLogo.AlternateText = "";

        imgLogo.Visible = lnkDeleteLogo.Visible = false;
        cbActive.Checked = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sql = "";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            List<SqlParameter> prms = new List<SqlParameter>();
            if (MyId == "0")
            {
                //Insert
                sql = "pnca.Organizations_Ins";
            }
            else
            {
                //Edit
                sql = "pnca.Organizations_Upd";
                prms.Add(new SqlParameter("@id", MyId));
            }

            //Set SQL Parameters:
            #region SQL Parameters
            prms.Add(new SqlParameter("@orgid", ddlOrganization.SelectedValue));
            prms.Add(new SqlParameter("@name", ddlOrganization.SelectedItem.Text));
 
            prms.Add(new SqlParameter("@AdminContact", tbAdminContact.Text));
            prms.Add(new SqlParameter("@Email", tbEmail.Text));
            prms.Add(new SqlParameter("@website", tbUrl.Text));
            prms.Add(new SqlParameter("@RegionId", ddlRegion.SelectedValue));
            prms.Add(new SqlParameter("@active", cbActive.Checked ? 1 : 0));

            #endregion


            //Execute SP

            object ret = ExecuteQuery(sql, prms.ToArray(), conn, CommandType.StoredProcedure);
            MyId = ret.ToString();
           

            //Upload files:
            #region Upload Files
            sql = "";
            string fileName = "";
            List<SqlParameter> parmsImg = new List<SqlParameter>();

            if (fuLogo.HasFile)
            {
                fileName = SaveFile(fuLogo);
                sql = "update pnca.Organizations set Logo=@Logo, AltTextLogo=Name + '’s Logo'";
                parmsImg.Add(new SqlParameter("@Logo", fileName));

            }            

            if (sql.Length > 0)
            {
                sql += " where id=@id";
                parmsImg.Add(new SqlParameter("@id", MyId));

                ExecuteNoQuery(sql, parmsImg.ToArray(), conn, CommandType.Text);
            }
            #endregion

            if (sql == "pnca.Organizations_Ins")
            {
                ClearFields();
                SetView("pnlList");
                BindData();
            }
            else
            {
                //Reload the form:
                PopulateDetails(MyId);
            }
        }
    }

    private string SaveFile(FileUpload fu)
    {
        string dir = GetDirectory();
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string filename = fu.FileName;
        string file = dir + "\\" + filename;
        if (File.Exists(file))
            File.Delete(file);

        fu.SaveAs(file);

        return filename;
    }

    #region Delete
    string sqlDelFile = "declare @filename nvarchar(500) update pnca.Organizations set @filename={0}, {0}='' where id=@id select @filename";
    protected void lnkDeleteLogo_Click(object sender, EventArgs e)
    {
        string sql = String.Format(sqlDelFile, "Logo");
        DeleteFile(sql);
    }

    protected void lnkDeleteImage_Click(object sender, EventArgs e)
    {
        string sql = String.Format(sqlDelFile, "Image");
        DeleteFile(sql);
    }

    private void DeleteFile(string sql)
    {
        List<SqlParameter> parmsImg = new List<SqlParameter>();
        parmsImg.Add(new SqlParameter("@id", MyId));
        object obj = ExecuteQuery(sql, parmsImg.ToArray(), new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")), CommandType.Text);

        string filename = obj.ToString();
        if (!String.IsNullOrEmpty(filename))
        {
            File.Delete(GetDirectory() + filename);
        }

        PopulateDetails(MyId);
    }

    private string GetDirectory()
    {
        return Server.MapPath(String.Format("{0}{1}", imgPath, MyId));
    }
    #endregion

    protected void btnBackToList_Click(object sender, EventArgs e)
    {
        ClearFields();
        SetView("pnlList");
    }

    protected void btnDone_Click(object sender, EventArgs e)
    {

        ClearFields();
        SetView("pnlList");
        BindData();
    }
    #endregion

    #region Set View
    protected string CurrentView
    {
        set { ViewState["currentview"] = value; }
        get { return ViewState["currentview"] == null ? pnlList.ID : ViewState["currentview"].ToString(); }
    }

    private void SetView(string view)
    {
        CurrentView = view;
        SetView();
    }

    private void SetView()
    {
        IsViewable(pnlList);
        IsViewable(pnlDetails);
        IsViewable(pnlServices);

    }

    private bool IsViewable(Panel pnl)
    {
        return pnl.Visible = pnl.ID == CurrentView;
    }

    #endregion View

    #region Auxiliar

    string[] LCIDs = { "1033", "1036" };  //1033 - English USA; 3084 - French France;
    private string RemoveNoiseWords(string searchTerm, string LCID)
    {
        //Do nothing if there is any quotations " or '
        string strReturn = "";

        if (searchTerm.IndexOf("\"") == -1 && searchTerm.IndexOf("'") == -1)
        {

            SqlParameter[] param;
            param = new SqlParameter[] {
                new SqlParameter("@keywords", searchTerm),
                new SqlParameter("@lang", LCID)
            };

            DataTable tbKeywords = getTable("RemoveNoiseWords", param);
            foreach (DataRow dr in tbKeywords.Rows)
            {
                strReturn += dr["item"] + " ";
            }
        }
        else
        {
            strReturn = searchTerm;
        }
        return strReturn.Trim();
    }

    private string con = ConfigurationManager.AppSettings.Get("CMServer");
    private DataTable getTable(string cmd, SqlParameter[] param)
    {
        SqlDataAdapter da = new SqlDataAdapter(cmd, con);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Parameters.AddRange(param);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public object ExecuteQuery(string sql, SqlParameter[] prms, SqlConnection conn, CommandType cmdType)
    {
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.CommandType = cmdType;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        object ret = cmd.ExecuteScalar();
        cmd.Connection.Close();
        return ret;
    }
    public void ExecuteNoQuery(string sql, SqlParameter[] prms, SqlConnection conn, CommandType cmdType)
    {
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.CommandType = cmdType;
        cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
    }
    #endregion

    DataTable dtCities = new DataTable();
    DataTable dtServices = new DataTable();

    protected void PopulateServices(string id)
    {
        litScript.Text = "";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("pnca.OrganizationServices_Sel", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.Add(new SqlParameter("@id", id));

            DataSet ds = new DataSet();
            dapt.Fill(ds);

            if (ds.Tables[3].Rows.Count > 0)
                litOrganizationName.Text = ds.Tables[3].Rows[0]["Name"].ToString();

            DataTable dt = ds.Tables[0];

            dtCities = ds.Tables[1];
            dtServices = ds.Tables[2];

            GV_Services.DataSource = dt;
            GV_Services.DataBind();

            SetView("pnlServices");
        }
    }

    protected void ddlRegionFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void GV_Services_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            DataRow[] drs = dtServices.Select("OrganizationId=" + MyId + "and ServiceId=" + drv["id"].ToString(), "City ASC");
            CheckBox cbxActive = (CheckBox)e.Row.FindControl("cbxActive");
            cbxActive.Checked = drs.Count() > 0;

            Literal litService = (Literal)e.Row.FindControl("litService");
            litService.Text = drv["name"].ToString();

            CheckBoxList cblCities = (CheckBoxList)e.Row.FindControl("cblCities");
            cblCities.Attributes.Add("MyServiceId", drv["id"].ToString());

            drs = dtCities.Select("ServiceId=" + drv["id"].ToString(), "City ASC");
            foreach(DataRow dr in drs)
            {
                string id = dr["id"].ToString();
                
                ListItem li = new ListItem(dr["City"].ToString(), id);

                DataRow[] dd = dtServices.Select("id=" + id);
                if (dd.Length == 1)
                    li.Selected = true;

                li.Attributes.Add("class", "cb-enhanced");
                cblCities.Items.Add(li);
            }
        }
    }

    protected void cblCities_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Response.Write(((CheckBoxList)sender).SelectedValue);
        string sql =@"delete from pnca.OrganizationServices 
                    where OrganizationId=@orgid 
                        and ServiceCityId in (select id from pnca.Services_City where ServiceId = @serviceid) ; ";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            CheckBoxList mycblist = ((CheckBoxList)sender);
            List<SqlParameter> prms = new List<SqlParameter>();
            
            //Set SQL Parameters:
            #region SQL Parameters
            prms.Add(new SqlParameter("@orgid", MyId));
            prms.Add(new SqlParameter("@serviceid", mycblist.Attributes["MyServiceId"]));       //prms.Add(new SqlParameter("@servicecityid", mycblist.SelectedValue));

            #endregion

            foreach(ListItem li in mycblist.Items)
            { 
                if(li.Selected)
                    sql += " insert into pnca.OrganizationServices (OrganizationId, ServiceCityId) select id, " + li.Value + " from pnca.Organizations where id=@orgid and " + li.Value +
                                                        " not in (select ServiceCityId from pnca.OrganizationServices where OrganizationId = @orgid)";
            }

            ExecuteNoQuery(sql, prms.ToArray(), conn, CommandType.Text);

            PopulateServices(MyId);

            string myscript = "$(document).ready(function () { ($(\"[myserviceid = " + mycblist.Attributes["MyServiceId"] + "]\").parent().siblings($('img'))).click();});";
            litScript.Text = "<script>" + myscript + "</script>";
        }
    }
}