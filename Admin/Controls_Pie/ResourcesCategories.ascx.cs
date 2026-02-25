//#define SUBSUB_CATEGORY
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Admin_Controls_Pie_ResourcesCategories : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["sortOrder"] = "desc";

            BindDropDowns();
            Bind();
        }
    }

    private void BindDropDowns()
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter(@" select * from Languages
                                                        select * from eko.dbo.Groups where id in (select Group_id from eko.dbo.Users_Groups_Access where User_id=@userid and access_level>1) or @userid=1 order by name "
                                    , conn);
            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());
            dapt.Fill(ds);


#if MULTI_LANGUAGE
            DataTable dt = ds.Tables[1];

            ddlLanguage.DataSource = dt;
            ddlLanguage.DataValueField = "id";
            ddlLanguage.DataTextField = "name";
            ddlLanguage.DataBind();
            ddlLanguage.Items.Insert(0, new ListItem("Select One", ""));
#endif

            ddlGroup.DataSource = ds.Tables[1];
            ddlGroup.DataValueField = "id";
            ddlGroup.DataTextField = "name";
            ddlGroup.DataBind();

        }
    }

    private void Bind()
    {
        if (ViewState["sortExp"] != null)
        {
            Bind(ViewState["sortExp"].ToString(), ViewState["sortOrder"].ToString());
        }
        else
        {
            Bind("", "");
        }
    }

    private void Bind(string sortExp, string sortDir)
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("ResourceTypes_Select_New", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

            if (txtFilterName.Text != "")
                dapt.SelectCommand.Parameters.AddWithValue("@name", txtFilterName.Text.Trim());

            string stemp = "";
            foreach (ListItem li in ddlGroup.Items)
            {
                if (stemp != "")
                    stemp += ",";

                stemp += li.Value;
            }

            dapt.SelectCommand.Parameters.AddWithValue("@groups", stemp);

            dapt.Fill(dt);
        }

        if (dt.Rows.Count > 0)
        {
            DataView myDataView = new DataView();
            myDataView = dt.DefaultView;

            if (sortExp != string.Empty)
            {
                myDataView.Sort = string.Format("{0} {1}", sortExp, sortDir);
            }

            myDataView.RowFilter = "parentid=0";

            gvMain.DataSource = myDataView;
            gvMain.DataBind();

            pager1.ItemCount = myDataView.Count;
            if (gvMain.PageSize >= myDataView.Count)
            {
                pager1.Visible = false;
            }
            else
            {
                pager1.Visible = true;
            }

            gridarea.Visible = true;
            noMain.Visible = false;

            litPagerShowing.Text = CMSHelper.GetPagerInfo(gvMain, myDataView.Count);

        }
        else
        {
            gridarea.Visible = false;
            noMain.Visible = true;
        }
    }

    public void export(object s, EventArgs e)
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("ResourceTypes_Select", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;

            if (txtFilterName.Text != "")
                dapt.SelectCommand.Parameters.AddWithValue("@name", txtFilterName.Text.Trim());

            dapt.Fill(dt);
        }

        //if (dt.Rows.Count > 0)
        //{
        DataView myDataView = new DataView();
        myDataView = dt.DefaultView;

        if (ViewState["sortExp"] != null && !string.IsNullOrEmpty(ViewState["sortExp"].ToString()))
        {
            myDataView.Sort = string.Format("{0} {1}", ViewState["sortExp"].ToString(), ViewState["sortOrder"].ToString());
        }

        //gvMain.DataSource = myDataView;

        //}

        string attachment = "attachment; filename=ResourceCategories.xls";

        Response.ClearContent();

        Response.AddHeader("content-disposition", attachment);

        Response.ContentType = "application/vnd.ms-excel";

        string tab = "";

        foreach (DataColumn dc in myDataView.ToTable().Columns)
        {
            Response.Write(tab + dc.ColumnName);

            tab = "\t";
        }

        Response.Write("\n");


        int i;

        foreach (DataRow dr in dt.Rows)
        {
            tab = "";

            for (i = 0; i < dt.Columns.Count; i++)
            {
                Response.Write(tab + "\"" + dr[i].ToString().Replace("\"", "“") + "\"");

                tab = "\t";
            }

            Response.Write("\n");
        }

        Response.End();

    }

    public void rowcommand(object o, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "deleteamenitiestype":
                DeleteRow(e.CommandArgument.ToString());
                break;
            case "editamenitiestype":
                Edit(e.CommandArgument.ToString());
                break;
            default:
                break;
        }
    }

    public void DeleteRow(string id)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlCommand command = new SqlCommand("ResourceTypes_Delete", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }

        Bind();
    }

    private string CatId
    {
        set { ViewState["CatId"] = value; }
        get { return ViewState["CatId"].ToString(); }
    }
    private string SubId
    {
        set { ViewState["SubId"] = value; }
        get { return ViewState["SubId"].ToString(); }
    }

    private void Edit(string id)
    {
        pnlEdit.Visible = true;
        pnlList.Visible = false;

        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("ResourceTypes_Select", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@id", id);
            dapt.Fill(dt);
        }

        txtName.Text = dt.Rows[0]["name"].ToString();
        txtInternalName.Text = dt.Rows[0]["internal_name"].ToString();

        tbDesc.Text = dt.Rows[0]["description"].ToString();

        rblLibraryTypes.SelectedValue = dt.Rows[0]["categ_type"].ToString();
        rblLibraryTypes.Enabled = false;


#if MULTI_LANGUAGE
        ddlLanguage.ClearSelection();
        try
        {
            ddlLanguage.SelectedValue = dt.Rows[0]["language"].ToString();
        }
        catch { }
#endif

        CatId = id;
        BindSub(id);

        ibSave.CommandName = "save";
        ibSave.CommandArgument = id;


#if SUBSUB_CATEGORY
        imgAddSubSub.Visible = false;
        pnlSubSub.Visible = false;
        lblSubcategory.Text = ""; 
#endif
    }

    private void BindSub(string id)
    {
        pnlSub.Visible = true;
#if SUBSUB_CATEGORY
        pnlSubDetails.Visible = false;
#endif
        DataTable dt2 = new DataTable();
        this.ddlSubcategory.Items.Clear();
        txtSubCateg.Text = "";

        

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select p.*, t.qty from ResourceTypes p left outer join (select parentid, count(id) as qty from ResourceTypes group by parentid) t on p.id=t.parentid  where p.parentid=@id order by name", conn);
            dapt.SelectCommand.CommandType = CommandType.Text;
            dapt.SelectCommand.Parameters.AddWithValue("@id", id);
            dapt.Fill(dt2);

           // if (dt2.Rows.Count > 0)
            {
                ddlSubcategory.DataSource = dt2;
                ddlSubcategory.DataBind();

                foreach (ListItem li in ddlSubcategory.Items)
                    li.Selected = true;

                litErrorSub.Text = "";
                gvSubCateg.DataSource = dt2;
                gvSubCateg.DataBind();

#if SUBSUB_CATEGORY
                pnlSubDetails.Visible = true;

            }

            gvSubCateg.DataSource = dt2; 
            gvSubCateg.DataBind();
#else
            }
#endif
        }
    }

    public void Add(object o, EventArgs e)
    {
        pnlEdit.Visible = true;
        pnlList.Visible = false;
        ibSave.CommandName = "add";

        pnlSub.Visible = false;

        txtName.Text = "";
        tbDesc.Text = "";
        txtInternalName.Text = "";

        rblLibraryTypes.ClearSelection();
        rblLibraryTypes.Enabled = true;

#if MULTI_LANGUAGE
        ddlLanguage.ClearSelection();
#endif
    }

    protected void SAVE(object sender, EventArgs e)
    {
        pnlEdit.Visible = false;
        pnlList.Visible = true;

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlCommand command = new SqlCommand();

            if (ibSave.CommandName == "save")
            {
                command = new SqlCommand("ResourceTypes_Update", conn);
                command.Parameters.AddWithValue("@id", ibSave.CommandArgument.ToString());
            }
            else
            {
                command = new SqlCommand("ResourceTypes_Insert", conn);
                command.Parameters.AddWithValue("@type", rblLibraryTypes.SelectedValue);

            }

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", txtName.Text.Trim());
            command.Parameters.AddWithValue("@internal_name", txtInternalName.Text.Trim());

#if MULTI_LANGUAGE
            command.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);
#else
            command.Parameters.AddWithValue("@language", 1);

#endif
            command.Parameters.AddWithValue("@description", tbDesc.Text.Trim());


            //string items = "";
            ////foreach (ListItem li in ddlSubcategory.Items)
            ////{
            ////    if (li.Selected)
            ////        items += li.Text + ",";
            ////}

            //for (int i = 0; i < ddlSubcategory.Items.Count; i++)
            //{
            //    if (ddlSubcategory.Items[i].Selected)
            //        items += ddlSubcategory.Items[i].Text + ";";
            //}
            //if (items.Length > 0)
            //{
            //    int npos = items.LastIndexOf(";");
            //    if (npos > 0)
            //        items.Remove(npos);
            //}

            //if (items.Length > 0)
            //    command.Parameters.AddWithValue("@SubCategories", items);

            //ddlSubcategory.Items.Clear();

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }

        Bind();
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {
            ImageButton ImageButton2 = (ImageButton)e.Row.FindControl("ImageButton2");
            ImageButton2.Attributes.Add("onclick", "return confirm('Are you sure want to delete this item?')");

        }
    }
    protected void ibCancel_Click(object sender, EventArgs e)
    {
        pnlEdit.Visible = false;
        pnlList.Visible = true;

        Bind();
    }

    public string sortOrder
    {
        get
        {
            if (ViewState["sortOrder"].ToString() == "desc")
            {
                ViewState["sortOrder"] = "asc";
            }
            else
            {
                ViewState["sortOrder"] = "desc";
            }

            return ViewState["sortOrder"].ToString();
        }
        set
        {
            ViewState["sortOrder"] = value;
        }
    }

    public void dosort(object o, GridViewSortEventArgs e)
    {
        ViewState["sortExp"] = e.SortExpression;
        Bind(e.SortExpression, sortOrder);
    }

    public void PageSizeChange(object o, EventArgs e)
    {
        gvMain.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        gvMain.PageIndex = 0;

        Bind();
    }

    public void pager_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pager1.CurrentIndex = currnetPageIndx;
        gvMain.PageIndex = currnetPageIndx - 1;

        Bind();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        Bind();
    }
    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFilterName.Text = "";

        Bind();
    }

    protected void imgAddSubbl_Click(object sender, EventArgs e)
    {
        if (txtSubCateg.Text != "")
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
            {
                SqlCommand command = new SqlCommand();

                string id = ibSave.CommandArgument;
                command = new SqlCommand("insert into ResourceTypes (name, internal_name, Language, parentid, categ_type) values(@name, @name, @language, @id, @type) ", conn);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@name", txtSubCateg.Text.Trim());
#if MULTI_LANGUAGE
                command.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);
#else
                command.Parameters.AddWithValue("@language", 1);

#endif
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@type", rblLibraryTypes.SelectedValue);


                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                BindSub(id);
            }
            txtSubCateg.Text = "";
        }
    }

    protected void gvSubCateg_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {
            ImageButton ImageButton2 = (ImageButton)e.Row.FindControl("ImageButton2");
            ImageButton2.Attributes.Add("onclick", "return confirm('Are you sure want to delete this item?')");

        }
    }

    public void rowcommand_sub(object o, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "deleteamenitiestype":
                DeleteRowSub(e.CommandArgument.ToString());
                break;
            case "editamenitiestype":
                EditSub(e.CommandArgument.ToString());
                break;
            default:
                break;
        }
    }

    public void DeleteRowSub(string id)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlCommand command = new SqlCommand("ResourceTypes_Delete", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }

        BindSub(CatId);
    }

    private void EditSub(string id)
    {

        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("ResourceTypes_Select", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.AddWithValue("@id", id);
            dapt.Fill(dt);
        }


        SubId = id;
#if SUBSUB_CATEGORY
        lblSubcategory.Text = dt.Rows[0]["name"].ToString();
        BindSubsub(id);

        imgAddSubSub.Visible = true;
        pnlSubSub.Visible = true;
#endif
    }

#if SUBSUB_CATEGORY
    private void BindSubsub(string id)
    {
        DataTable dt2 = new DataTable();

        btnSaveSubsub.Visible = false;
        ddlSubsubcategory.Items.Clear();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select * from ResourceTypes where parentid=@id order by name", conn);
            dapt.SelectCommand.CommandType = CommandType.Text;
            dapt.SelectCommand.Parameters.AddWithValue("@id", id);
            dapt.Fill(dt2);

            if (dt2.Rows.Count > 0)
            {
                ddlSubsubcategory.DataSource = dt2;
                ddlSubsubcategory.DataBind();

                foreach (ListItem li in ddlSubsubcategory.Items)
                    li.Selected = true;

                btnSaveSubsub.Visible = true;
                imgAddSubSub.Visible = true;

            }

        }
    }


    protected void imgAddSubSub_Click(object sender, EventArgs e)
    {
        if (txtSubsubCateg.Text != "")
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
            {
                SqlCommand command = new SqlCommand();

                string id = SubId;
                command = new SqlCommand("insert into ResourceTypes (name, Language, parentid) values(@name, @language, @id) ", conn);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@name", txtSubsubCateg.Text.Trim());
                command.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);
                command.Parameters.AddWithValue("@id", id);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                BindSub(CatId);
                BindSubsub(id);
            }
            txtSubsubCateg.Text = "";
        }

    }
    protected void btnSaveSubsub_Click(object sender, EventArgs e)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            SqlCommand command = new SqlCommand();


            command = new SqlCommand("delete from ResourceTypes where parentid = @id and id not in (select item from dbo.fSplitString(@SubCategories, ';'))", conn);
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@id", SubId);


            string items = "";


            for (int i = 0; i < ddlSubsubcategory.Items.Count; i++)
            {
                if (ddlSubsubcategory.Items[i].Selected)
                    items += ddlSubsubcategory.Items[i].Value + ";";
            }
            if (items.Length > 0)
            {
                int npos = items.LastIndexOf(";");
                if (npos > 0)
                    items.Remove(npos);
            }

            if (items.Length > 0)
                command.Parameters.AddWithValue("@SubCategories", items);
            else
                command.CommandText = "delete from ResourceTypes where parentid = @id ";

            ddlSubsubcategory.Items.Clear();

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }

        lblSubcategory.Text = "";
        BindSub(CatId);
        BindSubsub(SubId);
        pnlSubSub.Visible = false;
    }
#endif

    protected void gvSubCateg_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {
            DataRowView drw = (DataRowView)e.Row.DataItem;
            TextBox txtSubName = (TextBox)e.Row.FindControl("txtSubName");
            txtSubName.Text = drw["name"].ToString();
            TextBox txtSubInternalName = (TextBox)e.Row.FindControl("txtSubInternalName");
            txtSubInternalName.Text = drw["internal_name"].ToString();


            ImageButton ImageButton2 = (ImageButton)e.Row.FindControl("ImageButton2");
            ImageButton2.Attributes.Add("onclick", "return confirm('Are you sure want to delete this item?')");

        }
    }

    protected void gvSubCateg_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        switch (e.CommandName.ToLower())
        {
           
            case "deletesubcat":
                int id = Convert.ToInt32(e.CommandArgument);
                SqlCommand delcommand = new SqlCommand();
                delcommand.CommandText = "ResourceTypes_Delete";
                delcommand.CommandType = CommandType.StoredProcedure;
                delcommand.Parameters.AddWithValue("@id", id);

                if(ExcuteCommand(delcommand))
                    BindSub(CatId);

                break;

        }
    }

    protected bool ExcuteCommand(SqlCommand command)
    {
        bool bret = false;
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("dbResources")))
        {
            try
            {
                command.Connection = conn;

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                bret = true;
            }
            catch (Exception ex)
            {
                litErrorSub.Text = "<br>" + ex.Message + "<br>";
            }
        }

        return bret;
    }

    protected void UpdateSub(object sender, ImageClickEventArgs e)
    {
        GridViewRow row = (sender as ImageButton).NamingContainer as GridViewRow;


       //TextBox txtInternalName = (TextBox)row.FindControl("txtSubInternalName");
        TextBox txtSubName = (TextBox)row.FindControl("txtSubInternalName");
        HiddenField txtID = (HiddenField)row.FindControl("txtID");


        string sql = @"update ResourceTypes
                                set	name = ISNULL(@name, name)
	                            ,internal_name = ISNULL(@internal_name, internal_name)
                                where id = @id";
        

        SqlCommand updcommand = new SqlCommand();
        updcommand.CommandText = sql;
        updcommand.CommandType = CommandType.Text;
        updcommand.Parameters.AddWithValue("@id", txtID.Value);
        updcommand.Parameters.AddWithValue("@name", txtSubName.Text);
        updcommand.Parameters.AddWithValue("@internal_name", txtSubName.Text);

        if(ExcuteCommand(updcommand))
            BindSub(CatId);
    }
}