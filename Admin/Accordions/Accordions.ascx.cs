using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuteEditor;

public partial class Admin_Accordions_Accordions : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindGroups();
        }
              
        BindEditor(Editor1);
        BindEditor(Editor2);
        BindEditor(Editor3);
    }

    int Group
    {
        set { ViewState["Group"] = value; }
        get { return Convert.ToInt32(ViewState["Group"]); }
    }

    int SubGroup
    {
        set { ViewState["SubGroup"] = value; }
        get { return Convert.ToInt32(ViewState["SubGroup"]); }
    }
    int Item
    {
        set { ViewState["Item"] = value; }
        get { return Convert.ToInt32(ViewState["Item"]); }
    }
    int SubItem
    {
        set { ViewState["SubItem"] = value; }
        get { return Convert.ToInt32(ViewState["SubItem"]); }
    }

    string special_users = "1";
    private void BindGroups()
    {
        Group = 0;
        DataSet ds = new DataSet();
        string sqlstr = " select * from accordion.groups select * from languages select * from groups where id in (select group_id from Users_Groups_Access where (user_id = @userid and access_level>1) or @userid in (" + special_users + "))";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());
        ds = getDataset(sq, true);


        this.GV_Main.DataSource = ds.Tables[0];
        this.GV_Main.DataBind();

        ddlLanguage.DataSource = ds.Tables[1];
        ddlLanguage.DataTextField = "name";
        ddlLanguage.DataValueField = "id";
        ddlLanguage.DataBind();

        ddlGroup.DataSource = ds.Tables[2];
        ddlGroup.DataTextField = "name";
        ddlGroup.DataValueField = "id";
        ddlGroup.DataBind();
        ddlGroup.SelectedValue = "1";       //Common group
    }

    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Group = Convert.ToInt32(GV_Main.DataKeys[e.NewEditIndex].Values[0]);
        SwitchView(pnlSubGroup);
        BindSubgroups(Group);

    }

    private void SwitchView(Panel pnl)
    {
        pnlGroups.Visible = false;
        pnlSubGroup.Visible = false;
        pnlEdit_Subgrp.Visible = false;
        pnlList_Items.Visible = false;
        pnlEdit_Item.Visible = false;
        pnlEditGrp.Visible = false;
        pnlList_SubItems.Visible = false;
        pnlEdit_SubItem.Visible = false;

        pnl.Visible = true;

    }
    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName == "EditGroup")
        {
            txtNameGrp.Text = "";

            SwitchView(pnlEditGrp);
            Group = Convert.ToInt32(e.CommandArgument.ToString());

            DataTable dt = new DataTable();
            string sqlstr = @" select * from accordion.groups where id=@id";
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@id", Group);

            dt = getTable(sq, true);

            if (dt.Rows.Count > 0)
                txtNameGrp.Text = dt.Rows[0]["name"].ToString();
        }
    }

    private void ClearFields()
    {
        txtNameGrp.Text = "";
        tbName.Text = "";
        tbTitle.Text = "";
        tbTitleFR.Text = "";
        ddlLanguage.ClearSelection();

    }


    protected void bttn_Cancel_Ggroup_Click(object sender, EventArgs e)
    {
        SwitchView(pnlGroups );
        BindGroups();
    }

    #region SubGroup
    private void BindSubgroups(int group)
    {
        DataTable dt = new DataTable();
        string sqlstr = @" select sg.*, g.name as groupname, l.name as langname, isnull(t.qty, 0) as qty, gr.name as CMSGroup 
                            from accordion.subgroups sg inner join accordion.groups g on g.id=sg.group_id inner join languages l on sg.language=l.id 
                            left outer join (select count(id) qty, subgroup_id from accordion.items group by subgroup_id) t on sg.id = t.subgroup_id 
                            left outer join Groups gr on gr.id = sg.CMSGroupId 
                            where group_id=@id and (sg.CMSGroupId in (select group_id from Users_Groups_Access where user_id = @userid ) or @userid in (" + special_users + "))" +
                            "order by sg.name ";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@id", group);
        sq.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());

        dt = getTable(sq, true);

        if (dt.Rows.Count > 0)
            litTtile_Group.Text = dt.Rows[0]["groupname"].ToString();

        this.GridView2.DataSource = dt;
        this.GridView2.DataBind();
    }
    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((DataRowView)e.Row.DataItem != null)
        {
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("LB_Delete");
            DataRowView dr = (DataRowView)e.Row.DataItem;
            if (dr["qty"].ToString() == "0")
                LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this element?');");
            else
            {
                LB_Delete.Enabled = false;
                LB_Delete.Attributes.Add("style", "opacity:0.3");
            }

        }
    }

    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        RemoveRecord("delete from accordion.SubGroups where id=@id delete from content where control='accordion' and param=@id", (int)GridView2.DataKeys[e.RowIndex].Value);
        BindSubgroups(Group);

    }

    protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
    {
        SubGroup = Convert.ToInt32(GridView2.DataKeys[e.NewEditIndex].Values[0]);
        SwitchView(pnlList_Items);
        BindItems(SubGroup);
    }


    protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditGrp")
        {
            SubGroup = Convert.ToInt32(e.CommandArgument);
            SwitchView(pnlEdit_Subgrp);

            DataTable dt = new DataTable();
            string sqlstr = @" select * from accordion.subgroups where id=@id";
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@id", SubGroup);

            dt = getTable(sq, true);

            if (dt.Rows.Count > 0)
            {
                tbName.Text = dt.Rows[0]["name"].ToString();
                tbTitle.Text = dt.Rows[0]["title"].ToString();
                tbTitleFR.Text = dt.Rows[0]["titlefr"].ToString();
                
                ddlLanguage.SelectedValue = dt.Rows[0]["language"].ToString();
                ddlGroup.SelectedValue = dt.Rows[0]["CMSGroupId"].ToString();
                rblColumns.SelectedValue = dt.Rows[0]["columns"].ToString();
            }

        }
    }

    protected void bttn_Cancel_Subgroup_Click(object sender, EventArgs e)
    {
        ClearFields();
        SwitchView(pnlSubGroup);
    }

    protected void bttn_Save_Subgroup_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parms = new List<SqlParameter>();
        string sql = "";
        if (SubGroup == 0)
        {
            //Insert
            sql = @"insert into accordion.subgroups (group_id, language, name, title, titlefr, CMSGroupId, columns) values(@gid, @lang, @name, @title, @titlefr, @cmsgrp, @columns)  
                    insert into content(name,control,param,language) values('Accordion - ' + @name,'Accordion',SCOPE_IDENTITY(),@lang)";
            parms.Add(new SqlParameter("@gid", Group));
        }
        else
        {
            //Update
            sql = @"update accordion.subgroups set language=@lang, name=@name, title=@title, titlefr=@titlefr, CMSGroupId=@cmsgrp, columns=@columns where id=@id
                    update content set name='Accordion - ' + @name where control='Accordion' and param=@id ";
            parms.Add(new SqlParameter("@id", SubGroup));
        }

        parms.Add(new SqlParameter("@name", tbName.Text));
        parms.Add(new SqlParameter("@title", tbTitle.Text));
        parms.Add(new SqlParameter("@titlefr", tbTitleFR.Text));
        parms.Add(new SqlParameter("@lang", ddlLanguage.SelectedValue));
        parms.Add(new SqlParameter("@cmsgrp", ddlGroup.SelectedValue));
        parms.Add(new SqlParameter("@columns", rblColumns.SelectedValue));


        ProcessRecord(sql, parms, CommandType.Text);

        ClearFields();
        SwitchView(pnlSubGroup);
        BindSubgroups(Group);
    }

    protected void btAddSubgroup_Click(object sender, EventArgs e)
    {
        ClearFields();
        SubGroup = 0;
        SwitchView(pnlEdit_Subgrp);

    }
    #endregion

    #region Items
    protected void BindItems(int subgrp)
    {
        DataTable dt = new DataTable();
        string sqlstr = @" select i.*, sg.name as subgroupname, l.name as langname from accordion.items i inner join accordion.subgroups sg on i.subgroup_id = sg.id  inner join languages l on sg.language=l.id where subgroup_id = @id order by priority";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@id", subgrp);

        dt = getTable(sq, true);

        if (dt.Rows.Count > 0)
            litTtile_SubGroup.Text = dt.Rows[0]["subgroupname"].ToString();

        this.GridViewItems.DataSource = dt;
        this.GridViewItems.DataBind();
    }

    protected void GridViewItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((DataRowView)e.Row.DataItem != null)
        {
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("LB_Delete");
            DataRowView dr = (DataRowView)e.Row.DataItem;
            LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this element?');");            
        }
    }

    protected void GridViewItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        RemoveRecord("delete from accordion.items where id=@id and id not in (select item_id from Accordion.SubItems)", (int)GridViewItems.DataKeys[e.RowIndex].Value);
        BindItems(SubGroup);
    }

    protected void GridViewItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditItem")
        {
            Item = Convert.ToInt32(e.CommandArgument);
            SwitchView(pnlEdit_Item);

            DataTable dt = new DataTable();
            string sqlstr = @" select * from accordion.items where id=@id";
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@id", Item);

            dt = getTable(sq, true);

            if (dt.Rows.Count > 0)
            {
                txtTitle.Text = dt.Rows[0]["title"].ToString();
                Editor1.Text = dt.Rows[0]["content"].ToString();
                Editor3.Text = dt.Rows[0]["footer"].ToString();
            }

        }
        if (e.CommandName == "EditItems")
        {
            Item = Convert.ToInt32(e.CommandArgument);
            SwitchView(pnlList_SubItems);

            BindSubItems(Item);
        }

            
        if (e.CommandName == "Up")
        {
            SwapPriority("AccordionSwapPriority", Convert.ToInt32(e.CommandArgument), 1);
            BindItems(SubGroup);
        }
        else if (e.CommandName == "Down")
        {
            SwapPriority("AccordionSwapPriority", Convert.ToInt32(e.CommandArgument), 0);
            BindItems(SubGroup);
        }
    }

    private bool SwapPriority(string sql, int item, int _dir)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", item);
        cmd.Parameters.AddWithValue("@direction", _dir);
        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return ret > 0;
    }
    protected void btAddItem_Click(object sender, EventArgs e)
    {
        ClearFields_Item();
        Item = 0;
        SwitchView(pnlEdit_Item);
    }

    protected void btnBackItems_Click(object sender, EventArgs e)
    {
        SwitchView(pnlSubGroup);
        BindSubgroups(Group);
    }

    protected void btnBack_item_Click(object sender, EventArgs e)
    {
        ClearFields_Item();
        SwitchView(pnlList_Items);
    }

    protected void btnSave_item_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parms = new List<SqlParameter>();
        string sql = "";
        if (Item == 0)
        {
            //Insert
            sql = "declare @id int insert into accordion.items (subgroup_id, Title, Content, priority, footer) values(@gid, @Title, @Content, 100000, @footer) select @id=SCOPE_IDENTITY() update accordion.items set priority=id where id=@id";
            parms.Add(new SqlParameter("@gid", SubGroup));
        }
        else
        {
            //Update
            sql = "update accordion.items set  Title=@Title, Content=@Content, footer=@footer where id=@id";
            parms.Add(new SqlParameter("@id", Item));
        }
        parms.Add(new SqlParameter("@Title", txtTitle.Text));
        parms.Add(new SqlParameter("@Content", Editor1.Text));
        parms.Add(new SqlParameter("@footer", Editor3.Text));

        ProcessRecord(sql, parms, CommandType.Text);

        ClearFields_Item();
        SwitchView(pnlList_Items);
        BindItems(SubGroup);
    }

    protected void ClearFields_Item()
    {
        Editor1.Text = "";
        Editor3.Text = "";
        txtTitle.Text = "";
    }

    private void BindEditor(Editor editor)
    {
        //string TemplateItemList = "Paste,PasteText,CssClass,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,InsertDocument,YouTube,|,RemoveFormat,CleanCode";
        //string TemplateItemList = "FormatBlock,Paste,PasteText,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,InsertDocument,YouTube,|,RemoveFormat,CleanCode";
        string TemplateItemList = "Find,FormatBlock,Images,Paste,PasteText,|,Undo,Redo,|,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,Indent,Outdent,|,InsertLink,InsertImage,InsertFlash,InsertMedia,YouTube,|,RemoveFormat,CleanCode";
        editor.TemplateItemList = TemplateItemList;
        editor.ResizeMode = CuteEditor.EditorResizeMode.None;
        editor.Height = 200;

        editor.EmptyAlternateText = EmptyAlternateText.ForceAdd;
        editor.EnableStripScriptTags = false;
        editor.EnableStripIframeTags = false;
        editor.BreakElement = BreakElement.P;

        editor.EditorWysiwygModeCss = string.Format(ConfigurationManager.AppSettings.Get("EditorCss") + "?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        editor.PreviewModeCss = string.Format(ConfigurationManager.AppSettings.Get("EditorCss") + "?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        editor.EditorBodyStyle = "background-color:#FFFFFF; background-image:none; width:auto; height:auto;";

        CuteEditor.ToolControl ronster = Editor1.ToolControls["Images"];
        if (ronster != null)
        {

            DataTable dt = new DataTable();
            string sqlstr = "ZoneControlList";
            SqlCommand sq = new SqlCommand(sqlstr);
           // sq.Parameters.AddWithValue("@control", "TableWidget,photos");
            sq.Parameters.AddWithValue("@control", "Video");
            sq.Parameters.AddWithValue("@MYUSERID", Session["LoggedInId"].ToString());

            dt = getTable(sq);

            if (dt.Rows.Count > 0)
            {
                CuteEditor.RichDropDownList dd = (CuteEditor.RichDropDownList)ronster.Control;
                CuteEditor.RichListItem rich = dd.Items[0];
                dd.Items.Clear();
                rich.Text = "Template";
                rich.Html = "Template";
                dd.Items.Add(rich);
                dd.Items.Add("Callout Box", "", "<div class=\"callout-box\"><h3>This is the callout headline</h3><p>Quisque egestas fermentum mi, id placerat lacus sodales quis. Suspendisse potenti. Ut gravida pellentesque leo, et viverra risus iaculis vel. Aliquam erat volutpat. </p></div>");
                dd.Items.Add("Callout Box - Check", "", "<div class=\"callout-box check\"><h3>This is the callout headline</h3><p>Quisque egestas fermentum mi, id placerat lacus sodales quis. Suspendisse potenti. Ut gravida pellentesque leo, et viverra risus iaculis vel. Aliquam erat volutpat. </p></div>");
                dd.Items.Add("Callout Box - Bell", "", "<div class=\"callout-box bell\"><h3>This is the callout headline</h3><p>Quisque egestas fermentum mi, id placerat lacus sodales quis. Suspendisse potenti. Ut gravida pellentesque leo, et viverra risus iaculis vel. Aliquam erat volutpat. </p></div>");
                dd.Items.Add("Unordered List in Two Columns", "", "<ul class='two-columns'><li>one</li><li>two</li><li>three</li><li>four</li></ul>");
                dd.Items.Add("Button", "", "<div><span class=\"button\">This is a button!</span></div>");


                foreach (DataRow dr in dt.Rows)
                {
                    string title = dr["name"].ToString();
                    int npos = title.IndexOf("-");
                    if (npos > 0)
                        title = title.Substring(npos + 1).Trim();

                    dd.Items.Add("<img src=\"/images/icons/types/txt.png\"/><span>" + dr["name"].ToString() + "</span>", "",
                                    String.Format("&lt;widget id='{0}' class='{1}' title='{2}' &sol;&gt;",
                                                    dr["id"].ToString(),
                                                    dr["control"].ToString(),
                                                    title
                                                    )
                                );

                    //other variant for the format could be : <widget id='10159' class='control-quicklinks'>Quicklinks: Test</widget>
                }
            }
        }

            
        CuteEditor.ToolControl toolctrl = editor.ToolControls["FormatBlock"];
        if (toolctrl != null)
        {
            CuteEditor.RichDropDownList dropdown = (CuteEditor.RichDropDownList)toolctrl.Control;
            //the first item is the caption     
            CuteEditor.RichListItem richitem = dropdown.Items[0];
            //clear the items from configuration files     
            dropdown.Items.Clear();
            //add the caption    
            dropdown.Items.Add(richitem);
            //add text and value     
            dropdown.Items.Add(String.Format("<span style=\"{0}\">Bodytext</span>", ConfigurationManager.AppSettings.Get("EditorDropdownBody")), "Bodytext", "<p>");
            dropdown.Items.Add(String.Format("<span style=\"{0}\">Header</span>", ConfigurationManager.AppSettings.Get("EditorDropdownH1")), "Header", "<h1>");
            dropdown.Items.Add(String.Format("<span style=\"{0}\">Title</span>", ConfigurationManager.AppSettings.Get("EditorDropdownH2")), "Title", "<h2>");
            dropdown.Items.Add(String.Format("<span style=\"{0}\">Subtitle</span>", ConfigurationManager.AppSettings.Get("EditorDropdownH3")), "Subtitle", "<h3>");

            //dropdown.Items.Add("DIV", "<div>");
        }

        
    }
    #endregion
    protected void btnCancelGrp_Click(object sender, EventArgs e)
    {
        SwitchView(pnlGroups);

        BindGroups();
    }

    protected void btnSaveGrp_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parms = new List<SqlParameter>();
        string sql = "";

        {
            //Update
            sql = "update accordion.groups set name=@name where id=@id";
            parms.Add(new SqlParameter("@id", Group));
        }
        parms.Add(new SqlParameter("@name", txtNameGrp.Text));

        ProcessRecord(sql, parms, CommandType.Text);

        ClearFields();
        SwitchView(pnlGroups);
        BindGroups();
    }

    #region SubItems

    protected void btnBackToItems_Click(object sender, EventArgs e)
    {
        ClearFields_SubItem();
        SwitchView(pnlList_Items);
    }

    protected void btnAddSubItem_Click(object sender, EventArgs e)
    {
        ClearFields_SubItem();
        SubItem = 0;
        SwitchView(pnlEdit_SubItem);

    }
    protected void BindSubItems(int item)
    {
        string sqlstr = @" select * from Accordion.SubItems where item_id=@id order by priority 
                            select * from Accordion.Items where id=@id ";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@id", item);

        DataSet ds = getDataset(sq, true);
        DataTable dt = ds.Tables[0];

        if (ds.Tables[1].Rows.Count > 0)
            litSubItem_Title.Text = ds.Tables[1].Rows[0]["Title"].ToString();

        this.GridViewSubItems.DataSource = dt;
        this.GridViewSubItems.DataBind();
    }
    protected void GridViewSubItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((DataRowView)e.Row.DataItem != null)
        {
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("LB_Delete");
            DataRowView dr = (DataRowView)e.Row.DataItem;
            LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this element?');");
        }
    }

    protected void GridViewSubItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        RemoveRecord("delete from accordion.SubItems where id=@id", (int)GridViewSubItems.DataKeys[e.RowIndex].Value);
        BindSubItems(Item);
    }

    protected void GridViewSubItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditItem")
        {
            SubItem = Convert.ToInt32(e.CommandArgument);
            SwitchView(pnlEdit_SubItem);

            DataTable dt = new DataTable();
            string sqlstr = @" select * from accordion.subitems where id=@id";
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@id", SubItem);

            dt = getTable(sq, true);

            if (dt.Rows.Count > 0)
            {
                txtSubTitle.Text = dt.Rows[0]["title"].ToString();
                Editor2.Text = dt.Rows[0]["content"].ToString();
            }

        }

        if (e.CommandName == "Up")
        {
            SwapPriority("AccordionSub_SwapPriority", Convert.ToInt32(e.CommandArgument), 1);
            BindSubItems(Item);
        }
        else if (e.CommandName == "Down")
        {
            SwapPriority("AccordionSub_SwapPriority", Convert.ToInt32(e.CommandArgument), 0);
            BindSubItems(Item);
        }
    }

    protected void btnBack_subitem_Click(object sender, EventArgs e)
    {
        ClearFields_SubItem();
        SwitchView(pnlList_SubItems);
    }

    private void ClearFields_SubItem()
    {
        txtSubTitle.Text = "";
        Editor2.Text = "";
    }

    protected void btnSave_subitem_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parms = new List<SqlParameter>();
        string sql = "";
        if (SubItem == 0)
        {
            //Insert
            sql = "declare @id int insert into accordion.SubItems (item_id, Title, Content, Priority) values(@itemid, @Title, @Content, 100000) select @id=SCOPE_IDENTITY() update accordion.SubItems set priority=id where id=@id";
            parms.Add(new SqlParameter("@itemid", Item));
        }
        else
        {
            //Update
            sql = "update accordion.SubItems set  Title=@Title, Content=@Content where id=@id";
            parms.Add(new SqlParameter("@id", SubItem));
        }
        parms.Add(new SqlParameter("@Title", txtSubTitle.Text));
        parms.Add(new SqlParameter("@Content", Editor2.Text));

        ProcessRecord(sql, parms, CommandType.Text);

        ClearFields_SubItem();
        SwitchView(pnlList_SubItems);
        BindSubItems(Item);
    }
    #endregion


    #region dal
    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");

    //   protected string sqlUpdate = "";

    private DataTable getTable(string cmd)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd, _connection);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.Fill(dt);
        return dt;
    }
    private DataTable getTable(SqlCommand cmd)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    private DataTable getTable(SqlCommand cmd, bool a)
    {
        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    private DataSet getDataset(SqlCommand cmd, bool a)
    {
        DataSet dt = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    private bool ProcessRecord(string sql, string fld, string _value, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = type;
        cmd.Parameters.AddWithValue(fld, _value);
        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return (ret > 0);
    }

    private bool ProcessRecord(string sql, List<SqlParameter> parms, CommandType type)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_connection));
        cmd.CommandType = type;

        foreach (SqlParameter p in parms)
            cmd.Parameters.Add(p);

        cmd.Connection.Open();
        int ret = cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        return (ret > 0);
    }

    private bool RemoveRecord(string sql, int rcrd)
    {
        return ProcessRecord(sql, "@id", rcrd.ToString(), CommandType.Text);
    }
    #endregion

}