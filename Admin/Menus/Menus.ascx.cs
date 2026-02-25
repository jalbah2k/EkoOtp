using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Admin_Menus_Menus : System.Web.UI.UserControl
{
    private string selectednode;
    StringBuilder sb;
    int m_accesslevel;

    [Serializable()]
    public class MinisiteMenusFR
    {
        public string id;
        public string name;
        public MinisiteMenusFR(string _id, string _name)
        {
            id = _id;
            name = _name;
        }
    }

    public Dictionary<string, MinisiteMenusFR> dicMinisiteMenusFR
    {
        get
        {
            return ViewState["dicMinisiteMenusFR"] != null ? (Dictionary<string, MinisiteMenusFR>)ViewState["dicMinisiteMenusFR"] : new Dictionary<string, MinisiteMenusFR>();
        }
        set
        {
            ViewState["dicMinisiteMenusFR"] = value;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        sb = new StringBuilder(60);
        if (!IsPostBack)
        {
            if (!CMSHelper.isMultilingual)
            {
                ff1.Visible = false;
                trTitleFR.Visible = false;
                frow1.Visible = false;
                trNewMenuFrenchName.Visible = false;

                //SeoPrefixEN = "/";
            }
            //else
            //{
            //    SeoPrefixEN = "/en/";
            //}

            if (Request.QueryString["m"] != null)
            {
                selectednode = Request.QueryString["m"];
                try
                {
                    int.Parse(selectednode);
                }
                catch
                {
                    btn_EditMenu.Visible = true;
                }
                ViewState["selectednode"] = selectednode;
            }
            else
                ViewState["selectednode"] = null;


            ShowHideControls();

            try
            {
                InitControls();
                int.Parse(selectednode);
                FillData(selectednode);
            }
            catch { }

        }
        if (ViewState["selectednode"] != null)
        {
            //Postback            
            selectednode = ViewState["selectednode"].ToString();
        }


    }

    #region Button Events
    protected void btn_NewMenu_Click(object sender, ImageClickEventArgs e)
    {
        pnlNewMenu.Visible = true;

        txtMenuName_EN.Text = "";
        txtMenuName_FR.Text = "";
        ddlOrientation.ClearSelection();
        ddlGroup.ClearSelection();
        ddlPage.ClearSelection();
        try
        {
            ddlGroup.Items.FindByValue("1").Selected = true;
        }
        catch { }

        ibSaveNewMenu.CommandName = "add";

        oTreeMenuCollection1.LoadMenus();
    }
    protected void btn_EditMenu_Click(object sender, ImageClickEventArgs e)
    {
        int id = 0;
        if (ViewState["selectednode"] != null && ViewState["selectednode"].ToString().StartsWith("menuid_"))
        {
            int.TryParse(ViewState["selectednode"].ToString().Replace("menuid_", ""), out id);
        }

        if (id > 0)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
            {
                SqlDataAdapter dapt = new SqlDataAdapter("select * from menus where linkid=@id", conn);
                dapt.SelectCommand.Parameters.AddWithValue("@id", id);
                dapt.Fill(dt);
            }

            if (dt.Rows.Count > 0)
            {
                pnlNewMenu.Visible = true;

                txtMenuName_EN.Text = dt.Rows[0]["name"].ToString();
                if (CMSHelper.isMultilingual && dt.Rows.Count > 1)
                    txtMenuName_FR.Text = dt.Rows[1]["name"].ToString();
                ddlOrientation.ClearSelection();
                try
                {
                    ddlOrientation.SelectedValue = dt.Rows[0]["orientation"].ToString();
                }
                catch { }
                ddlGroup.ClearSelection();
                try
                {
                    ddlGroup.SelectedValue = dt.Rows[0]["groupid"].ToString();
                }
                catch
                {
                    ddlGroup.Items.FindByValue("1").Selected = true;
                }

                ddlPage.ClearSelection();
                try
                {
                    string seo = dt.Rows[0]["landingpage"].ToString();
                    if (CMSHelper.isMultilingual || CMSHelper.AlwaysWithEN)
                    {
                        if (seo.Contains("/en/") || seo.Contains("/fr/"))
                            seo = seo.Remove(0, 4);
                    }
                    else
                    {
                        if (seo.Contains("/"))
                            seo = seo.Remove(0, 1);
                    }
                    DataTable dt2 = new DataTable();
                    SqlDataAdapter dapt2 = new SqlDataAdapter(" select id from pages where seo=@seo", new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")));
                    dapt2.SelectCommand.Parameters.AddWithValue("@seo", seo);
                    dapt2.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                        ddlPage.SelectedValue = dt2.Rows[0][0].ToString();
                }
                catch { }

                ibSaveNewMenu.CommandName = "save";
                ibSaveNewMenu.CommandArgument = id.ToString();
            }
        }

        oTreeMenuCollection1.LoadMenus();
    }
    public void ibSaveNewMenu_Click(object o, CommandEventArgs e)
    {
        pnlNewMenu.Visible = false;

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlCommand command = new SqlCommand();

            if (e.CommandName == "save")
            {
                command = new SqlCommand("Menu_Update", conn);
                command.Parameters.AddWithValue("@id", e.CommandArgument.ToString());
            }
            else
            {
                command = new SqlCommand("Menu_Insert", conn);
            }

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name_en", txtMenuName_EN.Text.Trim());
            if (CMSHelper.isMultilingual)
                command.Parameters.AddWithValue("@name_fr", txtMenuName_FR.Text.Trim());
            command.Parameters.AddWithValue("@orientation", ddlOrientation.SelectedItem.Text);
            if (ddlOrientation.SelectedItem.Text.ToLower() == "horizontal")
            {
                command.Parameters.AddWithValue("@menucss", "menu");
                command.Parameters.AddWithValue("@spacerheight", 0);
            }
            else
            {
                command.Parameters.AddWithValue("@menucss", "minimenu");
                command.Parameters.AddWithValue("@spacerheight", 1);
            }

            if(ddlPage.SelectedValue != "")
            {
                command.Parameters.AddWithValue("@pageid", ddlPage.SelectedValue);
                command.Parameters.AddWithValue("@english_prefix", "/");
            }

            command.Parameters.AddWithValue("@groupid", ddlGroup.SelectedValue);

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }

        this.oTreeMenuCollection1.LoadMenus();
    }
    protected void ibCancelNewMenu_Click(object sender, EventArgs e)
    {
        pnlNewMenu.Visible = false;

        this.oTreeMenuCollection1.LoadMenus();
    }

    protected void btn_Addnew_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["isadd"] = "true";    //?
        ViewState["action"] = "add";    //?

        tbl_Navigation.Visible = true;
        tbl_Step3.Visible = true;

        oTreeMenuCollection1.LoadMenus();
        InitControls();
        Reset_fields();

    }
    protected void btn_Del_Click(object sender, ImageClickEventArgs e)
    {
        try { int.Parse(selectednode); }
        catch { return; }
        
        string id = selectednode;

        //Validation - Check child record
        DataTable dt1 = new DataTable();
        dt1 = mGet_Menuitems_ByParentid(Convert.ToInt32(id));

        if (dt1.Rows.Count != 0)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "er", "if(confirm('You are about to delete menu items which contain sub-items. Click OK to continue'))deleteNodes(\"" + id + "\");", true);
            this.oTreeMenuCollection1.LoadMenus();
            return;
        }
        //end validation

        mDelete_Menu(Convert.ToInt32(id));

        ViewState["selectednode"] = null;
        this.oTreeMenuCollection1.LoadMenus();
        HideEditControls();

        ViewState["isadd"] = "false";
        ViewState["action"] = null;
    }
    protected void btn_Up_Click(object sender, ImageClickEventArgs e)
    {
        //UP
        //validation
         if (string.IsNullOrEmpty(selectednode))
        {
            return;
        }

         try { int.Parse(selectednode); }
         catch { return; }

         DecreasePriority(selectednode);

         this.oTreeMenuCollection1.LoadMenus();
    }
    
    protected void btn_Down_Click(object sender, ImageClickEventArgs e)
    {
        //DOWN
        //validation
        if (string.IsNullOrEmpty(selectednode))
        {
            return;
        }

        try { int.Parse(selectednode); }
        catch { return; }

        IncreasePriority(selectednode);
        this.oTreeMenuCollection1.LoadMenus();
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        if (ViewState["action"] != null)
        {
           
            if (ViewState["action"].ToString() == "edit")   //UPDATE
            { 
                #region UPDATE
                int menuitemid = Convert.ToInt32(selectednode);

                DataTable dt3 = new DataTable();

                //english       
                if (this.DDL_Pages.SelectedIndex == 0)
                {
                    mEdit_Menu(menuitemid.ToString(), "", this.txt_Menu_EN.Text, this.txt_Title_EN.Text.Trim(), this.txt_URL.Text, this.DDL_Target.SelectedValue.ToString(), this.RB_Visible.SelectedValue.ToString(), this.RB_Enabled.SelectedValue.ToString(), this.ddlLinkedMinisite.SelectedValue);

                }
                else
                {
                    //for url (using pageid & seo)
                    dt3 = mGet_One_Page(Convert.ToInt32(this.DDL_Pages.SelectedValue.ToString()));
                   //////////////////// string mURL_en = "/en/" + dt3.Rows[0]["seo"].ToString();
                    string mURL_en = CMSHelper.SeoPrefixEN + dt3.Rows[0]["seo"].ToString();

                    mEdit_Menu(menuitemid.ToString(), this.DDL_Pages.SelectedValue.ToString(), this.txt_Menu_EN.Text, this.txt_Title_EN.Text.Trim(), mURL_en, this.DDL_Target.SelectedValue.ToString(), this.RB_Visible.SelectedValue.ToString(), this.RB_Enabled.SelectedValue.ToString(), this.ddlLinkedMinisite.SelectedValue);
                }

                int menuitemid_fr = 0;
                if (CMSHelper.isMultilingual)
                {
                    if (rblMultiLang.SelectedValue == "1")
                    {
                        //french

                        DataTable dt4 = new DataTable();
                        dt4 = mGet_One_Menuitems_Linkid_Deeper(menuitemid,"2");

                        if (dt4.Rows.Count > 0)
                        {

                            menuitemid_fr = Convert.ToInt32(dt4.Rows[0]["id"].ToString());

                            if (this.DDL_Pages.SelectedIndex == 0)
                            {
                                mEdit_Menu(menuitemid_fr.ToString(), "", this.txt_Menu_FR.Text, this.txt_Title_FR.Text.Trim(), this.txt_URL_fr.Text, this.DDL_Target.SelectedValue.ToString(), this.RB_Visible.SelectedValue.ToString(), this.RB_Enabled.SelectedValue.ToString(), dicMinisiteMenusFR[this.ddlLinkedMinisite.SelectedValue].id);
                            }
                            else
                            {
                                dt3 = mGet_One_Page_Linkid(Convert.ToInt32(this.DDL_Pages.SelectedValue.ToString()));
                                string mURL_fr;
                                if (dt3.Rows.Count > 0)
                                {
                                    mURL_fr = "/fr/" + dt3.Rows[0]["seo"].ToString();
                                }
                                else
                                {
                                    mURL_fr = this.txt_URL_fr.Text;
                                }

                                DataTable dt7 = new DataTable();
                                dt7 = mGet_One_Page_Linkid(Convert.ToInt32(this.DDL_Pages.SelectedValue.ToString()));
                                string pageid = "";
                                if (dt7.Rows.Count > 0)
                                {
                                    pageid = dt7.Rows[0]["id"].ToString();
                                }
                                mEdit_Menu(menuitemid_fr.ToString(), pageid, this.txt_Menu_FR.Text, this.txt_Title_FR.Text.Trim(), mURL_fr, this.DDL_Target.SelectedValue.ToString(), this.RB_Visible.SelectedValue.ToString(), this.RB_Enabled.SelectedValue.ToString(), dicMinisiteMenusFR[this.ddlLinkedMinisite.SelectedValue].id);
                            }
                        }
                        else
                        {
                            int id_fr = 0;
                            InsertMenuItemFR(ref dt3, menuitemid, ref id_fr);
                        }

                    }
                    else
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
                        {
                            SqlCommand cmd = new SqlCommand("delete from menuitems where linkid=@id and language=2", con);
                            cmd.Parameters.AddWithValue("@id", menuitemid);


                            con.Open();
                            Convert.ToInt32(cmd.ExecuteNonQuery());
                            con.Close();
                        }
                    }
                }

                sb = sb.Remove(0, sb.Length);
                sb.Append(" Delete from menu_group ");
                sb.Append(" where menuitem_id=@id ");
                if (CMSHelper.isMultilingual && menuitemid_fr != 0 && rblMultiLang.SelectedValue == "1")
                    sb.Append(" or menuitem_id=@idfr ");

                sb.Append(" insert into menu_group(menuitem_id,group_id) values(@id,@grp) ");
                if (CMSHelper.isMultilingual && menuitemid_fr != 0 && rblMultiLang.SelectedValue == "1")
                    sb.Append(" insert into menu_group(menuitem_id,group_id) values(@idfr,@grp) ");

                string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
                string commandString = sb.ToString();


                using (SqlConnection connection = new SqlConnection(strConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(commandString, connection);
                    cmd.Parameters.AddWithValue("@id", menuitemid.ToString());

                    if (CMSHelper.isMultilingual && rblMultiLang.SelectedValue == "1")
                        cmd.Parameters.AddWithValue("@idfr", menuitemid_fr.ToString());

                    cmd.Parameters.AddWithValue("@grp", DDL_Groups.SelectedValue);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                #endregion
            } 
            
            else if (ViewState["action"].ToString() == "add")
            { 
                #region INSERT
                string id = getIdFromMenuItems(selectednode);

                string menuid = "";
                menuid = getMenuId(id);

                if (selectednode.Contains("menuid_"))
                    id = "0";

                DataTable dt3 = new DataTable();

                //english       
                int id_en;
                if (this.DDL_Pages.SelectedIndex == 0)
                {
                    id_en = mAdd_Menu(menuid, id, "", "", this.txt_Menu_EN.Text, this.txt_Title_EN.Text.Trim(), this.txt_URL.Text, this.DDL_Target.SelectedValue.ToString(), this.RB_Visible.SelectedValue.ToString(), this.RB_Enabled.SelectedValue.ToString(), "", this.ddlLinkedMinisite.SelectedValue, "1");
                }
                else
                {
                    //for url (using pageid & seo)

                    dt3 = mGet_One_Page(Convert.ToInt32(this.DDL_Pages.SelectedValue.ToString()));
                    //string mURL_en = "/en/" + dt3.Rows[0]["seo"].ToString();
                    string mURL_en = CMSHelper.SeoPrefixEN + dt3.Rows[0]["seo"].ToString();

                    id_en = mAdd_Menu(menuid, id, this.DDL_Pages.SelectedValue.ToString(), "", this.txt_Menu_EN.Text, this.txt_Title_EN.Text.Trim(), mURL_en, this.DDL_Target.SelectedValue.ToString(), this.RB_Visible.SelectedValue.ToString(), this.RB_Enabled.SelectedValue.ToString(), "", this.ddlLinkedMinisite.SelectedValue, "1");
                }


                #region FRENCH
                int id_fr = 0;
                if (CMSHelper.isMultilingual 
                    && rblMultiLang.SelectedValue == "1"
                    )
                {                    
                    //for french
                    InsertMenuItemFR(ref dt3, id_en, ref id_fr);
                } 
                #endregion

                sb = sb.Remove(0, sb.Length);
                sb.Append(" insert into menu_group(menuitem_id,group_id) values(@id,@grp) ");

                if (CMSHelper.isMultilingual && id_fr != 0 && rblMultiLang.SelectedValue == "1")
                    sb.Append(" insert into menu_group(menuitem_id,group_id) values(@idfr,@grp) ");

                string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
                string commandString = sb.ToString();


                using (SqlConnection connection = new SqlConnection(strConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(commandString, connection);
                    cmd.Parameters.AddWithValue("@id", id_en.ToString());

                    if (CMSHelper.isMultilingual && id_fr != 0 && rblMultiLang.SelectedValue == "1")
                        cmd.Parameters.AddWithValue("@idfr", id_fr.ToString());

                    cmd.Parameters.AddWithValue("@grp", DDL_Groups.SelectedValue);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                #endregion
            }            
        }
                
        btn_Addnew.Visible = ddlLinkedMinisite.SelectedValue == "";
        Reset_fields();
        this.tbl_Navigation.Visible = false;
        this.tbl_Step3.Visible = false;
        ViewState["isadd"] = "false";
        this.oTreeMenuCollection1.LoadMenus();
    }

    private void InsertMenuItemFR(ref DataTable dt3, int id_en, ref int id_fr)
    {
        string mParentid_fr, menuid_fr;

        //get french parentid & Menuid\

        DataTable dt4 = new DataTable();
        dt4 = mGet_One_Menuitems_Linkid(id_en, "2");

        if (dt4.Rows.Count > 0)
        {
            mParentid_fr = dt4.Rows[0]["parentid_fr"].ToString();
            menuid_fr = dt4.Rows[0]["menuid_fr"].ToString();

            string mPriority = getPriority_ById(id_en.ToString());

            if (this.DDL_Pages.SelectedIndex == 0)
            {
                id_fr = mAdd_Menu(menuid_fr, mParentid_fr, "", mPriority, this.txt_Menu_FR.Text, this.txt_Title_FR.Text.Trim(), this.txt_URL_fr.Text, this.DDL_Target.SelectedValue.ToString(), this.RB_Visible.SelectedValue.ToString(), this.RB_Enabled.SelectedValue.ToString(), id_en.ToString(), dicMinisiteMenusFR[this.ddlLinkedMinisite.SelectedValue].id, "2");
            }
            else
            {
                dt3 = mGet_One_Page_Linkid(Convert.ToInt32(this.DDL_Pages.SelectedValue.ToString()));
                string mURL_fr = "/fr/" + dt3.Rows[0]["seo"].ToString();
                DataTable dt7 = new DataTable();
                dt7 = mGet_One_Page_Linkid(Convert.ToInt32(this.DDL_Pages.SelectedValue.ToString()));
                id_fr = mAdd_Menu(menuid_fr, mParentid_fr, dt7.Rows[0]["id"].ToString(), mPriority, this.txt_Menu_FR.Text, this.txt_Title_FR.Text.Trim(), mURL_fr, this.DDL_Target.SelectedValue.ToString(), this.RB_Visible.SelectedValue.ToString(), this.RB_Enabled.SelectedValue.ToString(), id_en.ToString(), dicMinisiteMenusFR[this.ddlLinkedMinisite.SelectedValue].id, "2");
            }

        }

    }
    #endregion

    #region DAL
    private DataTable mGet_Pages_English()
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from pages where language = @lang_id order by name";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@lang_id", "1");
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    private DataTable mGet_MenuItems_ById(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
       // string commandString = "select mi.*,isnull (ShowLock, 0) ShowLock from menuitems mi inner join menus m on mi.menuid = m.id LEFT OUTER JOIN MenuItemShowLock k on mi.id = k.MenuItemId where mi.linkid = @mID order by language";
        string commandString = @"select mi.*,isnull (ShowLock, 0) ShowLock 
                    ,isnull(t.id, 0) OnlyMobile
                    from menuitems mi inner join menus m on mi.menuid = m.id 
                    LEFT OUTER JOIN MenuItemShowLock k on mi.id = k.MenuItemId 
                    LEFT OUTER JOIN MenuItems_MobileItems t on mi.id = t.id
                    where mi.linkid = @mID order by language";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);

            cmd.Parameters.AddWithValue("@mID", mID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    private DataTable mGet_One_Page_SEO(string mSEO)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where seo= @mSEO";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mSEO", mSEO);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];

    }

    private DataTable mGet_One_Page(int mID)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where id = @mID";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mID", mID);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }

    private void mEdit_Menu(string id, string pageid, string text, string tooltip, string navigateurl, string target, string visible, string enabled, string linkedmenuid)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" update MenuItems ");
        sb.Append(" Set text = @text, ");
        sb.Append(" tooltip = @tooltip, ");
        sb.Append(" pageid = @pageid, ");
        sb.Append(" navigateurl = @navigateurl, ");
        sb.Append(" target = @target, ");
        sb.Append(" visible = @enabled, ");                 //@visible, ");
        sb.Append(" enabled = @enabled, ");
        sb.Append(" linkedmenuid = @linkedmenuid");
       

        sb.Append(" where id = @id; delete from MenuItemShowLock where MenuItemId = @id; ");

        if (rbShowLock.SelectedValue == "1")
            sb.Append(" insert into MenuItemShowLock (MenuItemId) values(@id) ");

        sb.Append(" delete from MenuItems_MobileItems where id=@id");
        if (cbOnlyMobile.Checked)
            sb.Append(" insert into MenuItems_MobileItems (id) values(@id) ");


        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();
     
        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Parameters.AddWithValue("@tooltip", tooltip);
            cmd.Parameters.AddWithValue("@navigateurl", navigateurl);
            cmd.Parameters.AddWithValue("@target", target);
            cmd.Parameters.AddWithValue("@visible", enabled);       // visible);
            cmd.Parameters.AddWithValue("@enabled", enabled);
            cmd.Parameters.AddWithValue("@pageid", pageid);
            cmd.Parameters.AddWithValue("@id", id);
            if (string.IsNullOrEmpty(linkedmenuid))
                cmd.Parameters.AddWithValue("@linkedmenuid", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@linkedmenuid", linkedmenuid);
            
            connection.Open();
            cmd.ExecuteNonQuery();
        }
      
    }

    private DataTable mGet_Menuitems_ByParentid(int mParentid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = " select * from MenuItems where parentid = @mParentid";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mParentid", mParentid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    private void mDelete_Menu(int mID)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(" Delete from MenuItems ");
        sb.Append(" where linkid = @id ");

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@id", mID);

            connection.Open();
            cmd.ExecuteNonQuery();
        }


    }

    private DataTable mGet_One_Page_Linkid(int mLinkid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from Pages where linkid = @mLinkid and id <> @mLinkid";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mLinkid", mLinkid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }

    private DataTable mGet_One_Menuitems_Linkid_Deeper(int mLinkid, string lang)
    {
        //Only for update

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
//        string commandString = "select * from MenuItems where linkid = @mLinkid and id <> @mLinkid";
        string commandString = "SELECT     vw.parentid_fr, vw.menuid_fr, mi.* FROM  vw_GetMenuId_ParentId_Language AS vw INNER JOIN MenuItems AS mi ON vw.menuid_fr = mi.menuid WHERE (vw.id_en = @mLinkid) AND (vw.language_fr1 = @lang) AND (vw.language_fr1 = ISNULL(vw.language_fr2, vw.language_fr1)) and mi.linkid = @mLinkid ";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mLinkid", mLinkid);
            cmd.Parameters.AddWithValue("@lang", lang);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];

    }

    private DataTable mGet_One_Menuitems_Linkid(int mLinkid, string lang)
    {
        //Only for insert
//            string vw_GetMenuId_ParentId_Language = @"SELECT     ISNULL(mi_fr.id, 0) AS parentid_fr, m_fr.id AS menuid_fr, mi_en.id AS id_en, m_fr.language AS language_fr1,                                                        m_fr2.language AS language_fr2
//                                                    FROM         dbo.Menus AS m_fr2 RIGHT OUTER JOIN
//                                                                          dbo.MenuItems AS mi_en INNER JOIN
//                                                                          dbo.Menus AS m_en INNER JOIN
//                                                                          dbo.Menus AS m_fr ON m_en.linkid = m_fr.linkid ON mi_en.menuid = m_en.id LEFT OUTER JOIN
//                                                                          dbo.MenuItems AS mi_fr ON mi_en.parentid = mi_fr.linkid ON m_fr2.id = mi_fr.menuid
//                                                    WHERE     (m_en.language = 1)";

        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select parentid_fr, menuid_fr from vw_GetMenuId_ParentId_Language where id_en = @mLinkid and language_fr1 = @lang and language_fr1 = ISNULL(language_fr2, language_fr1)";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mLinkid", mLinkid);
            cmd.Parameters.AddWithValue("@lang", lang);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];



    }

    private int mAdd_Menu(string menuid, string parentid, string pageid, string priority, string text, string tooltip, string navigateurl, string target, string visible, string enabled, string linkid, string linkedmenuid, string lang)
    {
        sb = sb.Remove(0, sb.Length);
        sb.Append(@" IF @priority = '' BEGIN SET @priority = NULL END ");
        sb.Append("IF @priority IS NULL BEGIN SELECT top 1 @priority = priority + 1 from MenuItems where parentid = @parentid and menuid = @menuid order by priority desc END ");
        sb.Append("IF @priority IS NULL BEGIN SELECT @priority = 1 END ");
        sb.Append("insert into MenuItems  (menuid, parentid, pageid, priority, text, tooltip, navigateurl, target, visible, enabled, linkid, linkedmenuid,language) ");
        sb.Append(@" values   (@menuid, @parentid, @pageid, @priority,   @text, @tooltip, @navigateurl, @target, @visible, @enabled, @linkid, @linkedmenuid, @lang)");
              
        sb.Append(" DECLARE @newid int ");
        sb.Append(" SELECT @newid = SCOPE_IDENTITY() ");
        if(linkid == "")
            sb.Append(" UPDATE MenuItems SET linkid = @newid WHERE id = @newid ");
                
        sb.Append(" SELECT @newid");

        
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = sb.ToString();


        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@menuid", menuid);

            if (parentid == "" || parentid == null || parentid == "NULL")
            {
                cmd.Parameters.AddWithValue("@parentid", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@parentid", parentid);
            }

            cmd.Parameters.AddWithValue("@pageid", pageid);
            cmd.Parameters.AddWithValue("@priority", priority);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Parameters.AddWithValue("@tooltip", tooltip);
            cmd.Parameters.AddWithValue("@navigateurl", navigateurl.Replace("&", ""));
            cmd.Parameters.AddWithValue("@target", target);
            cmd.Parameters.AddWithValue("@visible", enabled);       // visible);
            cmd.Parameters.AddWithValue("@enabled", enabled);
            cmd.Parameters.AddWithValue("@linkid", linkid);
            if (string.IsNullOrEmpty(linkedmenuid))
                cmd.Parameters.AddWithValue("@linkedmenuid", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@linkedmenuid", linkedmenuid);

            cmd.Parameters.AddWithValue("@lang", lang);

            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            return i;

        }

    }

    private string getIdFromMenuItems(string id)
    {
        if (id.Contains("menuid_"))
        {
            string menuid = id.Substring(id.IndexOf("_") + 1);
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlDataAdapter dapt = new SqlDataAdapter("select id from MenuItems where menuid = @menuid", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@menuid", menuid);
            DataTable dt = new DataTable();
            dapt.Fill(dt);

            if (dt.Rows.Count > 0)
                id = dt.Rows[0]["id"].ToString();

        }

        return id;

    }

    private string getMenuId(string id)
    {
        string menuid = "";
        if (!(id.Contains("menuid_")))
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlDataAdapter dapt = new SqlDataAdapter("select menuid from MenuItems where id = @id", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@id", id);
            DataTable dt = new DataTable();
            dapt.Fill(dt);

            if (dt.Rows.Count > 0)
                menuid = dt.Rows[0]["menuid"].ToString();
            else
                menuid = "0";
        }
        else
        {
            menuid = id.Substring(id.IndexOf("_") + 1);
        }

        int.Parse(menuid);
        return menuid;

    }

    private string getMenuId(string id, string lang)
    {
        string menuid = "";
        if (!(id.Contains("menuid_")))
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlDataAdapter dapt = new SqlDataAdapter("select m.id from Menus m inner join MenuItems mi on m.linkid = mi.menuid where mi.id = @id and m.language=@lang", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@id", id);
            dapt.SelectCommand.Parameters.AddWithValue("@lang", lang);
            DataTable dt = new DataTable();
            dapt.Fill(dt);

            if (dt.Rows.Count > 0)
                menuid = dt.Rows[0]["id"].ToString();
            else
                menuid = "0";
        }
        else
        {
            menuid = id.Substring(id.IndexOf("_") + 1);
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlDataAdapter dapt = new SqlDataAdapter("select m.id from Menus m where m.linkid = @id and m.language=@lang", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@id", id);
            dapt.SelectCommand.Parameters.AddWithValue("@lang", lang);
            DataTable dt = new DataTable();
            dapt.Fill(dt);

            if (dt.Rows.Count > 0)
                menuid = dt.Rows[0]["id"].ToString();
            else
                menuid = "0";
        }

        int.Parse(menuid);
        return menuid;

    }

    private string getPriority_ById(string id)
    {
        string priority = "0";

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select priority from MenuItems where id = @id", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@id", id);
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            priority = dt.Rows[0]["priority"].ToString();
        }

        return priority;
    }

    private DataTable mGet_MenuItems_ByMenuid_Parentid_NULL(string mMenuid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from menuitems where  menuid  = @mMenuid and parentid=0 order by priority desc";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mMenuid", mMenuid);
         
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }  

    private DataTable mGet_MenuItems_ByMenuid_Parentid(string mMenuid, string mParent_id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from menuitems where  menuid  = @mMenuid and parentid = @mParent_id order by priority desc";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mMenuid", mMenuid);
            cmd.Parameters.AddWithValue("@mParent_id", mParent_id);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        return ds.Tables[0];
    }

    private void DecreasePriority(string id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand("Priority_Decrease", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    private void IncreasePriority(string id)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand("Priority_Increase", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    #endregion

    #region MyFunctions
    
    #region Initialization
    private void FillData(string id)
    {
       
        Reset_fields();
        try
        {
            int.Parse(selectednode);
        }
        catch { return; }

        ViewState["isadd"] = "false";
        ViewState["action"] = "edit";
        
        DataTable dt = new DataTable();  
        dt = mGet_MenuItems_ById(Convert.ToInt32(id));

        ddlLinkedMinisite.ClearSelection();
        try
        {
            ddlLinkedMinisite.SelectedValue = dt.Rows[0]["linkedmenuid"].ToString();
        }
        catch { }

        btn_Addnew.Visible = dt.Rows[0]["linkedmenuid"] == DBNull.Value;
        
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select top 1 * from menu_group where menuitem_id=" + id, sqlConn);

        DataSet ds = new DataSet();
        dapt.Fill(ds);

        if (Session["LoggedInId"].ToString() == "1")
        {
            DDL_Groups.ClearSelection();

            if (ds.Tables[0].Rows.Count > 0)
            {
                DDL_Groups.Items.FindByValue(ds.Tables[0].Rows[0]["group_id"].ToString()).Selected = true;
            }
            else
            {
                DDL_Groups.Items.FindByValue("-1").Selected = true;
            }
        }


        this.txt_Menu_EN.Text = dt.Rows[0]["text"].ToString();
        this.txt_Title_EN.Text = dt.Rows[0]["tooltip"].ToString();

        if (dt.Rows[0]["pageid"].ToString() != "0")
        {
            string url = dt.Rows[0]["navigateurl"].ToString();
            string seo = url;
            if (CMSHelper.isMultilingual || CMSHelper.AlwaysWithEN)
            {
                if (seo.Contains("/en/") || seo.Contains("/fr/"))
                    seo = seo.Remove(0, 4);
            }
            else {
                if (seo.Contains("/"))
                    seo = seo.Remove(0, 1);
            }
            DataTable dt4 = new DataTable();
            //dt4 = mGet_One_Page_SEO(url);
            dt4 = mGet_One_Page_SEO(seo);

            if (dt4.Rows.Count > 0)
            {
                string mID = dt4.Rows[0]["id"].ToString();

                this.txt_URL.Text = "";

                /*if (DDL_Pages.Items.Contains(DDL_Pages.Items.FindByValue(mID)) == true)
                {
                    this.DDL_Pages.SelectedValue = mID;
                }
                else
                {
                    this.DDL_Pages.SelectedIndex = 0;
                }*/
                DDL_Pages.ClearSelection();
                try
                {
                    this.DDL_Pages.SelectedValue = mID;
                }
                catch
                {
                    this.DDL_Pages.SelectedIndex = 0;
                }
            }

            txt_URL.Text = url;

            if (CMSHelper.isMultilingual && dt.Rows.Count == 2)
                txt_URL_fr.Text = dt.Rows[1]["navigateurl"].ToString();
        }
        else
        {
            this.txt_URL.Text = dt.Rows[0]["navigateurl"].ToString();
            if (CMSHelper.isMultilingual && dt.Rows.Count == 2)
                txt_URL_fr.Text = dt.Rows[1]["navigateurl"].ToString();
            this.DDL_Pages.SelectedIndex = 0;
        }


        this.DDL_Target.SelectedValue = dt.Rows[0]["target"].ToString();


        if (dt.Rows[0]["visible"].ToString().ToLower() == "true")
        {
            this.RB_Visible.SelectedIndex = 0;
        }
        else
        {
            this.RB_Visible.SelectedIndex = 1;
        }


        if (dt.Rows[0]["enabled"].ToString().ToLower() == "true")
        {
            this.RB_Enabled.SelectedIndex = 0;
        }
        else
        {
            this.RB_Enabled.SelectedIndex = 1;
        }

        if (CMSHelper.isMultilingual && dt.Rows.Count == 2)
        {
            this.txt_Menu_FR.Text = dt.Rows[1]["text"].ToString();
            this.txt_Title_FR.Text = dt.Rows[1]["tooltip"].ToString();
            rblMultiLang.SelectedValue = "1";
        }
        else
            rblMultiLang.SelectedValue = "0";


        if (Convert.ToBoolean(dt.Rows[0]["ShowLock"]))
        {
            this.rbShowLock.SelectedIndex = 0;
        }
        else
        {
            this.rbShowLock.SelectedIndex = 1;
        }

        cbOnlyMobile.Checked = Convert.ToBoolean(dt.Rows[0]["OnlyMobile"]);
  
    }

    //enum access_level { Deny, Viewer, Editor, Publisher, Admin };
    private void ShowHideControls()
    {
        
        if (string.IsNullOrEmpty(selectednode))
        {
            HideEditControls();
            
        }
        else
        {
            
            try
            {
                int.Parse(selectednode);                 
            }
            catch
            {
                btn_Addnew.Visible = (Session["LoggedInId"].ToString() == "1");
                btn_Del.Visible = false;
                btn_Down.Visible = false;
                btn_Up.Visible = false;

                tbl_Navigation.Visible = false;
                tbl_Step3.Visible = false;
                return;
            }

            btn_Del.Attributes.Add("OnClick", "return confirm('Are you sure to delete this menu item?');");

            getAccesLevel(selectednode);

            tbl_Step3.Visible = m_accesslevel > 0;
            
            tbl_Navigation.Visible = m_accesslevel > 1 || Session["LoggedInId"].ToString() == "1";
            btn_Addnew.Visible = m_accesslevel > 1 || Session["LoggedInId"].ToString() == "1";
            btn_Del.Visible = m_accesslevel > 1 || Session["LoggedInId"].ToString() == "1";
            btn_Down.Visible = m_accesslevel > 1 || Session["LoggedInId"].ToString() == "1";
            btn_Up.Visible = m_accesslevel > 1 || Session["LoggedInId"].ToString() == "1";


        }
    }

    private void HideEditControls()
    {
        btn_Addnew.Visible = false;
        btn_Del.Visible = false;
        btn_Down.Visible = false;
        btn_Up.Visible = false;

        tbl_Navigation.Visible = false;
        tbl_Step3.Visible = false;
    }    

    private void getAccesLevel(string id)
    {
        
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select Access_Level from Users_Groups_Access where user_id = @userid and (group_id = (select group_id from Menu_Group where menuitem_id=@id) or @userid=1)", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@userid",  Session["LoggedInId"].ToString());
        dapt.SelectCommand.Parameters.AddWithValue("@id", id);
        DataTable dt = new DataTable();
        
        dapt.Fill(dt);
        
        if (dt.Rows.Count > 0)
            m_accesslevel = int.Parse(dt.Rows[0]["Access_Level"].ToString());
        else
            m_accesslevel = 0;

   }

    private void InitControls()
    {
        Load_DDLPages();
        Load_DDLGroups();
        Load_DDLMinisites();

        DDL_Groups.Items.Add(new ListItem("NONE", "-1"));
        DDL_Groups.Items.FindByValue("-1").Attributes.Add("color", "red");

        DDL_Groups.Items.Insert(0, new ListItem("select one", ""));


        //  if (Session["LoggedInId"].ToString() == "1")
        {
            grprow.Visible = true;
        }
        //else
        //{
        //    grprow.Visible = false;
        //}
    } 
    #endregion

    void Load_DDLPages()
    {

        DataTable dt = new DataTable();
        dt = mGet_Pages_English();
        int i = dt.Rows.Count;
        this.DDL_Pages.DataSource = dt;
        this.DDL_Pages.DataTextField = "name";
        this.DDL_Pages.DataValueField = "id";
        this.DDL_Pages.DataBind();

        this.DDL_Pages.Items.Insert(0, new ListItem(" ", "none"));

        this.ddlPage.DataSource = dt;
        this.ddlPage.DataTextField = "name";
        this.ddlPage.DataValueField = "id";
        this.ddlPage.DataBind();

        this.ddlPage.Items.Insert(0, new ListItem("", "none"));
    }

    void Load_DDLGroups()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from Groups where id in (select group_id from users_groups_access where user_id=" + Session["LoggedInId"].ToString() + " and access_level>1) or 1=" 
            + Session["LoggedInId"].ToString() + " order by name", sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        DDL_Groups.DataSource = ds.Tables[0];
        DDL_Groups.DataBind();

        ddlGroup.DataSource = ds.Tables[0];
        ddlGroup.DataValueField = "id";
        ddlGroup.DataTextField = "name";
        ddlGroup.DataBind();
        try
        {
            ddlGroup.Items.FindByValue("1").Selected = true;
        }
        catch { }
    }

    void Load_DDLMinisites()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select id, name, linkid, language from Menus where orientation='Vertical' and menucss='minimenu' order by name", sqlConn);
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        DataView dv = dt.DefaultView;
        dv.RowFilter = "language = 1";
        
        ddlLinkedMinisite.DataSource = dv;
        ddlLinkedMinisite.DataValueField = "id";
        ddlLinkedMinisite.DataTextField = "name";
        ddlLinkedMinisite.DataBind();
        ddlLinkedMinisite.Items.Insert(0, new ListItem(""));

        if (CMSHelper.isMultilingual)
        {
            dv.RowFilter = "language = 2";

            if (dv.Count > 0)
            {
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                dicMinisiteMenusFR = new Dictionary<string, MinisiteMenusFR>();
                dicMinisiteMenusFR.Add("", new MinisiteMenusFR("", ""));
                foreach (DataRow dr in dv.ToTable().Rows)
                {
                    dicMinisiteMenusFR.Add(dr["linkid"].ToString(), new MinisiteMenusFR(dr["id"].ToString(), dr["name"].ToString()));
                }
                hfMinisitesFR.Value = serialize.Serialize(dicMinisiteMenusFR);
            }
        }
    }

    void Reset_fields()
    {
        this.txt_Menu_EN.Text = "";
        this.txt_Menu_FR.Text = "";
        this.txt_Title_EN.Text = "";
        this.txt_Title_FR.Text = "";
        this.txt_URL.Text = "";
        this.txt_URL_fr.Text = "";
        this.DDL_Pages.SelectedIndex = 0;
        this.ddlLinkedMinisite.ClearSelection();

        //  this.RB_Enabled.ClearSelection();
        this.rbShowLock.SelectedValue = "0";            //.ClearSelection();

        this.txt_Menu_FR.Text = "";
        rblMultiLang.SelectedValue = "0";

        cbOnlyMobile.Checked = false;
    }

    #endregion

    private bool multi_change = false;
    protected void rblMultiLang_SelectedIndexChanged(object sender, EventArgs e)
    {
        multi_change = true;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (rblMultiLang.SelectedValue == "0")
        {
            ff1.Visible = false;
            trTitleFR.Visible = false;
            frow1.Visible = false;
            trNewMenuFrenchName.Visible = false;
        }
        else
        {
            ff1.Visible = true;
            trTitleFR.Visible = true;
            frow1.Visible = true;
            trNewMenuFrenchName.Visible = true;
        }
 
        if(multi_change)
            oTreeMenuCollection1.LoadMenus();
    }
}