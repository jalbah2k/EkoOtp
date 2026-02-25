using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using CuteWebUI;

public partial class Admin_Dash_Dash : System.Web.UI.UserControl
{
    private int AdminRightsAccessLevel = Convert.ToInt32(ConfigurationManager.AppSettings["AdminRightsAccessLevel"]);

    private int LoggedInID
    {
        get
        {
            int _LoggedInID = 0;
            if (Session["LoggedInID"] != null)
                int.TryParse(Session["LoggedInID"].ToString(), out _LoggedInID);
            else
            {
                string url = "/login";
                if (Request.QueryString["c"] != null)
                    url += "?c=" + Request.QueryString["c"].ToString();
                Response.Redirect(url);
            }

            return _LoggedInID;
        }
    }
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

    public void PageSizeChange(object o, EventArgs e)
    {
        GV_Main.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        GV_Main.PageIndex = 0;

        binddocumentgroups();
    }
    public void pager_Command(object sender, CommandEventArgs e)
	{
		int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
		pager1.CurrentIndex = currnetPageIndx;
		GV_Main.PageIndex = currnetPageIndx - 1;

		binddocumentgroups();
	}

    protected void Page_Load(object sender, EventArgs e)
    {
        litMessage.Text = "";

        if (!IsPostBack)
        {
            sortExp = "name"; // default sorted column
            sortOrder = "asc";    // default sort order

            if ((bool)Session["Multilingual"])
            {
                trLanguage.Visible = true;
            }

            SqlDataAdapter dapt = new SqlDataAdapter("select id,name from groups where id in (select group_id from users_groups_access where user_id=" + Session["LoggedInID"].ToString() + " and access_level>1) order by name", ConfigurationManager.AppSettings["CMServer"]);
            /*string sqlstr = " select id, name from groups ";
            if (LoggedInID > 1)
                sqlstr += " where id in (select Group_id from Users_Groups_Access where User_id = @LoggedInId and Access_Level >= @Access_Level) ";
            sqlstr += " order by name ";
            SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, ConfigurationManager.AppSettings["CMServer"]);
            dapt.SelectCommand.Parameters.AddWithValue("@LoggedInID", LoggedInID);
            dapt.SelectCommand.Parameters.AddWithValue("@Access_Level", AdminRightsAccessLevel);*/
            DataSet ds = new DataSet();
            dapt.Fill(ds);
            ddlGroup.DataSource = ds.Tables[0];
            ddlGroup.DataBind();

            ////litName.Text = ds.Tables[0].Rows[0]["name"].ToString();
            binddocumentgroups();
        }

        if (Session["docgroup"] != null)
        {
            string physDir = HttpContext.Current.Request.MapPath("/Data/Documents/" + Session["docgroup"].ToString());

            if (!Directory.Exists(physDir))
            {
                Directory.CreateDirectory(physDir);
            }

            // ((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = "Documents/" + Session["docgroup"].ToString();
            // ((DocProc.Godd)FileUploader1.Adapters[1]).groupid = Session["docgroup"].ToString();
        }

        //Response.Write("test:" + FileUploader1.JsFunc_Init + ":test");

        // bindcontacts();

    }

    public void filter(object o, EventArgs e)
    {
        binddocumentgroups();
    }

    private void bindcontacts()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from groups order by name select id,username + ' (' + name + ')' as username from users order by username", sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        //ddlToGroup.Items.Clear();
        // ddlToUser.Items.Clear();

        //ddlToGroup.DataSource = ds.Tables[0];
        //ddlToGroup.DataBind();
        //ddlToUser.DataSource = ds.Tables[1];
        // ddlToUser.DataBind();

        // ddlToGroup.Items.Insert(0,new ListItem("SINGLE USER", "-1"));
        // ddlToGroup.Items.Insert(0,new ListItem("EVERYONE", "0"));


    }

    public void updateddls(object o, EventArgs e)
    {
        //  if (ddlToGroup.SelectedValue == "-1")
        //  {
        //      ddlToUser.Visible = true;
        //  }
        //  else
        //  {
        //      ddlToUser.Visible = false;
        //   }

    }

    public void addnewgroup(object o, EventArgs e)
    {
        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlCommand sqlComm;

            //string sql = "insert into documentgroups(name,listname,language,groupid,sortbyname) values(@name,@listname,@language,@groupid,@sort) insert into content(name,control,param,language) values(@cname,'documents',@@IDENTITY,@language)";
            string sql = "declare @gid int insert into documentgroups(name,listname,language,groupid,sortbyname) values(@name,@listname,@language,@groupid,@sort) set @gid=SCOPE_IDENTITY() insert into content(name,control,param,language) values(@cname,'documents',@gid,@language) select @gid";
            ////string sqlEdit = "update documentgroups set name=@name,listname=@listname,language=@language,groupid=@groupid,sortbyname=@sort where id=@id update Content set language=@language,name=@cname where control='documents' and param=@id";
            //string sqlEdit = "update documentgroups set name=@name,listname=@listname,language=@language,groupid=@groupid,sortbyname=@sort where id=@id update Content set language=@language,name=@cname where control='documents' and param=@id select @id";
            string sqlEdit = "update documentgroups set name=@name,listname=@listname,language=@language,groupid=@groupid,sortbyname=0 where id=@id update Content set language=@language,name=@cname where control='documents' and param=@id select @id";
            if (Session["editlist"] != null)
            {
                sql = sqlEdit;
            }

            sqlComm = new SqlCommand(sql, sqlConn);
            sqlComm.Parameters.AddWithValue("@name", txtName.Text);
            sqlComm.Parameters.AddWithValue("@listname", txtListName.Text);
            sqlComm.Parameters.AddWithValue("@groupid", ddlGroup.SelectedValue);
            sqlComm.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);
            sqlComm.Parameters.AddWithValue("@cname", "Documents - " + txtName.Text);
            sqlComm.Parameters.AddWithValue("@sort", rblSort.SelectedValue);

            if (Session["editlist"] != null)
            {
                sqlComm.Parameters.AddWithValue("@id", Session["docgroup"].ToString());
                Session["editlist"] = null;
            }

            sqlConn.Open();
            //sqlComm.ExecuteNonQuery();
            string id = sqlComm.ExecuteScalar().ToString();

            DataTable dt = new DataTable();
            SqlDataAdapter dapt = new SqlDataAdapter("select name, private from Groups where id=@id", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@id", ddlGroup.SelectedValue);
            dapt.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                string docPath = HttpContext.Current.Request.MapPath("/Data/Documents/" + id);

                if ((bool)dt.Rows[0]["private"])
                {
                    if (!Directory.Exists(docPath))
                        Directory.CreateDirectory(docPath);

                    if (!File.Exists(docPath + @"/web.config"))
                        CMSHelper.CreatePrivateConfigFile(dt.Rows[0]["name"].ToString(), docPath, ddlGroup.SelectedValue);      // dt.Rows[0]["id"].ToString());
                }
                else
                {
                    if (File.Exists(docPath + @"/web.config"))
                        File.Delete(docPath + @"/web.config");
                }
            }

            sqlConn.Close();
        }

        //binddocumentgroups();
        backtolist(o, e);
    }

    private void binddocumentgroups()
    {
        binddocumentgroups(sortExp, sortOrder);
    }
    private void binddocumentgroups(string sortExp, string sortDir)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        ////SqlDataAdapter dapt = new SqlDataAdapter("select dg.id,dg.name,dg.listname,g.name as 'group',l.name as 'language' from documentgroups dg,groups g,languages l where g.id=dg.groupid and l.id=dg.language and (dg.name like '%" + txtFilter.Text + "%' or dg.listname like '%" + txtFilter.Text + "%')", sqlConn);
        //SqlDataAdapter dapt = new SqlDataAdapter("select dg.id,dg.name,dg.listname,g.name as 'group',l.name as 'language' from documentgroups dg inner join groups g on dg.groupid=g.id inner join languages l on dg.language=l.id where (@filter is null or dg.name like '%' + @filter + '%' or dg.listname like '%' + @filter + '%') and (@lang is null or dg.language=@lang)", sqlConn);
        
        SqlDataAdapter dapt = new SqlDataAdapter(
            "select dg.id,dg.name,dg.listname,g.name as 'group',l.name as 'language' " +
            "from documentgroups dg inner join groups g on dg.groupid=g.id inner join languages l on dg.language=l.id " +
            "where (@filter is null or dg.name like '%' + @filter + '%' or dg.listname like '%' + @filter + '%') and (@lang is null or dg.language=@lang) " +
            "and dg.groupid in (select item from dbo.fSplitString(@grps, ',')) ", sqlConn);

        string stemp = "";
        foreach (ListItem li in ddlGroup.Items)
        {
            if (stemp != "")
                stemp += ",";

            stemp += li.Value;
        }

        dapt.SelectCommand.Parameters.AddWithValue("@grps", stemp);

        if (txtFilter.Text.Trim().Length > 0)
            dapt.SelectCommand.Parameters.AddWithValue("@filter", txtFilter.Text.Trim());
        else
            dapt.SelectCommand.Parameters.AddWithValue("@filter", DBNull.Value);
        if (!(bool)Session["Multilingual"])
            dapt.SelectCommand.Parameters.AddWithValue("@lang", 1);
        else
            dapt.SelectCommand.Parameters.AddWithValue("@lang", DBNull.Value);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        DataView DV = ds.Tables[0].DefaultView;
        if (sortExp != string.Empty)
        {
            DV.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }
        GV_Main.DataSource = DV;
        GV_Main.DataBind();

        pager1.ItemCount = DV.Count;
        pager1.Visible = DV.Count > GV_Main.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Main, DV.Count);

        if (DV.Count < 1)
        {
            list.Visible = false;
            nolist.Visible = true;
        }
        else
        {
            list.Visible = true;
            nolist.Visible = false;
        }
    }

    private void binddocuments()
    {
        if (Session["docgroup"] != null)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlDataAdapter dapt = new SqlDataAdapter("select * from documents where groupid=@ID order by priority", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@ID", Session["docgroup"].ToString());
            DataSet ds = new DataSet();
            dapt.Fill(ds);
            GV_Docs.DataSource = ds.Tables[0];
            GV_Docs.DataBind();

            if (ds.Tables[0].Rows.Count < 1)
            {
                doctbl.Visible = false;
                nodoc.Visible = true;
            }
            else
            {
                doctbl.Visible = true;
                nodoc.Visible = false;
            }
        }

    }

    public void refresh(object o, UploaderEventArgs[] e)
    {
        binddocuments();
    }

    public void refresh(object o, EventArgs e)
    {
        binddocuments();
    }

    protected void GV_Docs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drw = (DataRowView)e.Row.DataItem;

            //for delete button
            ImageButton lb;
            lb = (ImageButton)e.Row.FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this document?');");


            Literal litHfId = (Literal)e.Row.FindControl("litHfId");
            litHfId.Text = String.Format("<input type='hidden' class='HfId' value='{0}' />", drw["id"].ToString());
        }
    }

    protected void GV_Docs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Int32 id = Convert.ToInt32(this.GV_Docs.DataKeys[e.RowIndex].Value.ToString());

        //Delete
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("declare @groupid int; declare @filename nvarchar(1000) " +
            "select @groupid=groupid, @filename=filename from documents where id=@ID " +
            "delete from documents where id=@ID " +
            "exec Report.ContentLogModifications 'documents', @groupid, @userid; " +
            "select cast(@groupid as varchar(20)) + '/' + @filename ", sqlConn);
        sqlComm.Parameters.AddWithValue("@ID", id.ToString());
        sqlComm.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());

        sqlConn.Open();
        object filename = sqlComm.ExecuteScalar();
        sqlConn.Close();

        string docPath = HttpContext.Current.Request.MapPath("/Data/Documents/" + filename.ToString());
        try { File.Delete(docPath); }
        catch (Exception ex) { litMessage.Text = ex.Message; }

        binddocuments();

    }

    protected void GV_Docs_RowEditing(object sender, GridViewEditEventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlView.Visible = false;
        pnlEditDoc.Visible = true;


        //ViewState["action"] = "edit";

        Int32 id = Convert.ToInt32(this.GV_Docs.DataKeys[e.NewEditIndex].Value.ToString());
        Session["doc"] = id;



        //((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = "/uploads/documents/" + id.ToString();

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select id,name from documents where id=@ID", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@ID", id.ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        txtDocName.Text = ds.Tables[0].Rows[0]["name"].ToString();

        //txtTitle.Text = ds.Tables[1].Rows[0]["listname"].ToString();

        //binddocuments();

        //lblFrom.Text = ds.Tables[0].Rows[0]["username"].ToString();
        // lblSubject.Text = ds.Tables[0].Rows[0]["subject"].ToString();
        //litMessage.Text = ds.Tables[0].Rows[0]["message"].ToString();

        //Session["mailfrom"] = ds.Tables[0].Rows[0]["fromuser"].ToString();
    }

    public void savedocname(object o, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm;

        //sqlComm = new SqlCommand("update documents set name=@name where id=@ID update Content set language=@language, name=@name where control='documents' and param=@ID", sqlConn);
        sqlComm = new SqlCommand("update documents set name=@name where id=@ID", sqlConn);
        sqlComm.Parameters.AddWithValue("@name", txtDocName.Text);
        sqlComm.Parameters.AddWithValue("@ID", Session["doc"].ToString());
        //sqlComm.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        backtodoclist(o, e);
    }

    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Language column
        e.Row.Cells[3].Visible = (bool)Session["Multilingual"];

        #region Add sorted class to headers
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExp);
            e.Row.Cells[colIndex].CssClass = sortOrder == "asc" ? "sortasc" : "sortdesc";
        }
        #endregion Add sorted class to headers

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("LB_Delete");
            LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure to delete this document list?')");
        }
    }
    protected void GV_Main_Sorting(object sender, GridViewSortEventArgs e)
    {
        sortExp = e.SortExpression;

        if (sortOrder == "desc")
            sortOrder = "asc";
        else
            sortOrder = "desc";
        binddocumentgroups(e.SortExpression, sortOrder);
    }
    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.RowIndex].Value.ToString());

        string docPath = HttpContext.Current.Request.MapPath("/Data/Documents/" + id.ToString());
        try { Directory.Delete (docPath, true); }
        catch (Exception ex) { litMessage.Text = ex.Message; return; }



        //Delete
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("delete from documentgroups where id=@ID delete from content where param=@ID and control='documents'", sqlConn);
        sqlComm.Parameters.AddWithValue("@ID", id.ToString());
        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        binddocumentgroups();
    }
    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = true;
        pnlView.Visible = false;
        pnlEditDoc.Visible = false;

       // ltDocument.Text = "Edit Document List";


        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.NewEditIndex].Value.ToString());
        Session["docgroup"] = id;

        //((DocProc.Godd)FileUploader1.Adapters[1]).groupid = "6";
        // ((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = "/uploads/documents/" + id.ToString();

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from documentgroups where id=" + Session["docgroup"].ToString(), sqlConn);
        //dapt.SelectCommand.Parameters.AddWithValue("@ID", id.ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        clear();
        Session["editlist"] = "edit";
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow rw = ds.Tables[0].Rows[0];
            txtName.Text = rw["Name"].ToString();
            txtListName.Text = rw["ListName"].ToString();
            ListItem li = ddlGroup.Items.FindByValue(rw["groupid"].ToString());
            if (li != null)
                li.Selected = true;
            li = ddlLanguage.Items.FindByValue(rw["language"].ToString());
            if (li != null)
                li.Selected = true;
            //rblSort.Items.FindByValue(rw["sortbyname"].ToString());
            //if (li != null)
            //    li.Selected = true;
        }
    }

    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "View")
        {
            pnlList.Visible = false;
            pnlSend.Visible = false;
            pnlView.Visible = true;

            Int32 id = Convert.ToInt32(e.CommandArgument);
            Session["docgroup"] = id;

            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlDataAdapter dapt = new SqlDataAdapter("select listname from documentgroups where id=" + Session["docgroup"].ToString(), sqlConn);

            DataSet ds = new DataSet();
            dapt.Fill(ds);
            txtTitle.Text = ds.Tables[0].Rows[0][0].ToString();

            binddocuments();
        }

    }

    public void clear()
    {
        txtName.Text = "";
        txtListName.Text = "";
        ddlGroup.ClearSelection();
        ddlLanguage.ClearSelection();
        Session.Remove("editlist");
    }

    public void savetitle(object s, EventArgs e)
    {
        //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        //SqlCommand sqlComm = new SqlCommand("update documentgroups set listname='" + txtTitle.Text + "' where id=" + Session["docgroup"].ToString(),sqlConn);

        //sqlConn.Open();
        //sqlComm.ExecuteNonQuery();
        //sqlConn.Close();

        backtolist(s, e);
    }

    public void backtolist(object sender, EventArgs e)
    {
        pnlList.Visible = true;
        pnlSend.Visible = false;
        pnlView.Visible = false;
        pnlEditDoc.Visible = false;

        GV_Main.EditIndex = -1;
        binddocumentgroups();
    }

    public void backtodoclist(object sender, EventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlView.Visible = true;
        pnlEditDoc.Visible = false;

        GV_Docs.EditIndex = -1;
        binddocuments();
    }

    public void newgroup(object s, EventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = true;
        pnlView.Visible = false;
        pnlEditDoc.Visible = false;
        //ltDocument.Text = "New Document List";

        clear();
    }

    public void fileup(object o, UploaderEventArgs e)
    {
        string _filename = e.FileName.Replace("&amp;", "and").Replace("&", "and").Replace("%", "").Replace("*", "").Replace(":", "").Replace(",", "").Replace("#", "No.");
        e.CopyTo(Request.MapPath("/Data/Documents/") + Session["docgroup"].ToString() + "\\" + _filename);

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("insert into documents(name,filename,groupid,mime,priority) values(@name,@filename,@groupid,@mime,(select isnull(max(priority), 0)+1 from documents)) exec Report.ContentLogModifications 'documents', @groupid, @userid ", sqlConn);
        sqlComm.Parameters.AddWithValue("@name", e.FileName.Split('.')[0]);
        sqlComm.Parameters.AddWithValue("@groupid", HttpContext.Current.Session["docgroup"].ToString());
        sqlComm.Parameters.AddWithValue("@filename", _filename);
        sqlComm.Parameters.AddWithValue("@mime", System.IO.Path.GetExtension(e.FileName));
        sqlComm.Parameters.AddWithValue("@userid", Session["LoggedInID"].ToString());

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();
    }


    protected void GV_Docs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Up")
        {
            string grp = Session["docgroup"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlCommand sqlComm = new SqlCommand("declare @high int declare @low int select @high =priority from Documents where id=@ID and groupid= '" + grp + "' select @low =Max( priority) from Documents where priority<@high and groupid= '" + grp + "'  if @low is not null begin update documents set priority=@high where groupid= '" + grp + "' and id = (select top 1 id from Documents where priority=@low and groupid= '" + grp + "') update Documents set priority=@low where id=@ID and groupid= '" + grp + "' end", sqlConn);
            sqlComm.Parameters.AddWithValue("@ID", e.CommandArgument);
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
            binddocuments();
        }
        else if (e.CommandName == "Down")
        {
            string grp = Session["docgroup"].ToString();
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlCommand sqlComm = new SqlCommand("declare @high int declare @low int select @low = priority from Documents where id=@ID and groupid= '" + grp + "' select @high = Min( priority) from Documents where priority>@low and groupid= '" + grp + "' if @high is not null begin update documents set priority=@low where groupid= '" + grp + "' and id = (select top 1 id from Documents where priority=@high and groupid= '" + grp + "') update Documents set priority=@high where id=@ID and groupid= '" + grp + "' end", sqlConn);
            sqlComm.Parameters.AddWithValue("@ID", e.CommandArgument);
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
            binddocuments();
        }
    }
}
