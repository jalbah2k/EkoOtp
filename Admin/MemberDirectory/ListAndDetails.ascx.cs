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
            return ViewState["MyID"] != null ? ViewState["MyID"].ToString() : "0";
        }
        set
        {
            ViewState["MyID"] = value;
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
    public string OrganizationType { set; get; }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        imgPath = ConfigurationManager.AppSettings["Organizations.Logo.Path"];
        BindEditor(tbMission);
        BindEditor(tbAboutUs);
        BindEditor(tbOurStory);
        BindEditor(tbServices);
        BindEditor(tbOurTeam);
        BindEditor(tbAchievements);
        BindEditor(tbOurPartners);
        BindEditor(tbLearnMoreAboutUs);
        

        trFeaturedFilter.Visible = trFeature.Visible = OrganizationType == "1";
        if (!IsPostBack)
        {
            BindDDLs();
            BindData();
        }
    }

    private void BindDDLs()
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select * from dbo.Organizations where id != @id order by name", conn);
            dapt.SelectCommand.Parameters.Add(new SqlParameter("id", ConfigurationManager.AppSettings.Get("Organizations.CMEY.ID")));
            dapt.SelectCommand.CommandType = CommandType.Text;
            dapt.Fill(dt);

            ddlOrganization.DataSource = dt;
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
            SqlDataAdapter dapt = new SqlDataAdapter("eko.Organizations_Search", conn);
            dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
            dapt.SelectCommand.Parameters.Add(new SqlParameter("@type", OrganizationType));

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

        if (cbxFeatureFilter.Checked)
            DV.RowFilter = "Featured = 1";
        else
            DV.RowFilter = "";

        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

        pager1.ItemCount = DV.Count;
        pager1.Visible = dt.Rows.Count > GV_Main.PageSize;

        this.GV_Main.Columns[2].Visible = OrganizationType == "1";
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void cbxFeatureFilter_CheckedChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void btnClearFilter_Click(object sender, EventArgs e)
    {
        txtFilter.Text = "";
        cbxFeatureFilter.Checked = false;
        BindData();
    }
    protected void bttn_Add_Click(object sender, EventArgs e)
    {
        ClearFields();
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


            CheckBox cbxFeature = (CheckBox)e.Row.FindControl("cbxFeature");
            cbxFeature.Checked = Convert.ToBoolean(dr["Featured"]);

            CheckBox cbxActive = (CheckBox)e.Row.FindControl("cbxActive");
            cbxActive.Checked = Convert.ToBoolean(dr["active"]);

            CheckBox cbxImage = (CheckBox)e.Row.FindControl("cbxImage");
            cbxImage.Checked = dr["Image"].ToString() != "" ? true : false;
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
                    SqlCommand command = new SqlCommand("eko.Organizations_Del", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", e.CommandArgument.ToString());

                    conn.Open();
                    object salida = command.ExecuteScalar();
                    conn.Close();

                    if ((int)salida != 0)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(GetType(), "warning", "alert('This organization is associated with at least one member. Instead of deleting it, you can disable it in the edit panel.');", true);
                    }
                }
                BindData();
                break;

            case "editing":
                ClearFields();
                MyId = e.CommandArgument.ToString();
                PopulateDetails(MyId);
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
            SqlDataAdapter dapt = new SqlDataAdapter("eko.OrganizationsExt_Sel", conn);
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
                
                tbAddress.Text = dr["Address"].ToString();
                tbCity.Text = dr["City"].ToString();
                tbEmail.Text = dr["Email"].ToString();
                tbFax.Text = dr["Fax"].ToString();
                tbPhone.Text = dr["PhoneNumber"].ToString();
                tbPostal.Text = dr["PostalCode"].ToString();

               // tbTitle.Text = dr["Name"].ToString();
                ddlOrganization.SelectedValue = dr["OrganizationId"].ToString();
                litTitle.Text = dr["Name"].ToString();
				
                tbTollFree.Text = dr["Toll_free"].ToString();
                tbUrl.Text = dr["URL"].ToString();
                tbSeo.Text = dr["SEO"].ToString();


                tbFacebook.Text = dr["Facebook"].ToString();
                tbInstagram.Text = dr["Instagram"].ToString();
                tbTwitter.Text = dr["Twitter"].ToString();
                tbLinkedIn.Text = dr["LinkedIn"].ToString();
                tbYouTube.Text = dr["YouTube"].ToString();


                #region Editor
                tbMission.Text = dr["MissionStatement"].ToString();
                tbAboutUs.Text = dr["AboutUs"].ToString();
                tbOurStory.Text = dr["OurStory"].ToString();
                tbServices.Text = dr["Services_ClientDemographics"].ToString();
                tbOurTeam.Text = dr["OurPeople_OurTeam"].ToString();
                tbAchievements.Text = dr["Achievements_AwardsAndRecognition"].ToString();
                tbOurPartners.Text = dr["OurPartners"].ToString();
                tbLearnMoreAboutUs.Text = dr["LearnMoreAboutUs"].ToString();
                
                #endregion



                if (lnkDeleteLogo.Visible = imgLogo.Visible = dr["Logo"].ToString() != "")
                {
                    imgLogo.ImageUrl = imgPath + dr["id"].ToString() + "/" + dr["Logo"].ToString();
                    imgLogo.AlternateText = tbAltText_Logo.Text = dr["AltTextLogo"].ToString();
                }


                if (lnkDeleteImage.Visible = imgImage.Visible = dr["Image"].ToString() != "")
                {
                    imgImage.ImageUrl = imgPath + dr["id"].ToString() + "/" + dr["Image"].ToString();
                    imgImage.AlternateText = tbAltText_Img.Text = dr["AltTextImg"].ToString();
                }

                try { ddlBackgroundPosition.SelectedValue = dr["BackgroundPosition"].ToString(); } catch { }
                try { ddlBackgroundPosition2.SelectedValue = dr["BackgroundPosition_Horizontal"].ToString(); } catch { }

                cbFeatured.Checked = Convert.ToBoolean(dr["Featured"]);
                cbActive.Checked = Convert.ToBoolean(dr["active"]);

                btnDone.Visible = btnDone2.Visible = true;

                SetView("pnlDetails");
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[1].Rows[0];
                txtLoaction1.Text = dr["Description"].ToString();
                txtTitle1.Text = dr["Title"].ToString();

                if (ds.Tables[1].Rows.Count > 1)
                {
                    GetTextBoxLocations();
                    string scr = "$(document).ready(function () {";

                    for (int i = 1; i < ds.Tables[1].Rows.Count; i++)
                    {
                        dr = ds.Tables[1].Rows[i];
                        MyTextBoxLocations[i].Text = dr["Description"].ToString();
                        MyTextBoxTitles[i].Text = dr["Title"].ToString();

                        if (!String.IsNullOrEmpty(dr["Description"].ToString())) 
                            scr += "$(\".trLocation[value = '" + i + "']\").show();";
                    }
                    scr += "});";

                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "locations", scr, true);

                }
            }
        }

    }

    TextBox[] MyTextBoxTitles = new TextBox[5];
    TextBox[] MyTextBoxLocations = new TextBox[5];
    private void GetTextBoxLocations()
    {
        int i = 0;
        bool istitle = true;
        foreach(Control ct in plDetails.Controls)
        {
            try 
            { 
                TextBox tb = (TextBox)ct;

                if (istitle)
                {
                    MyTextBoxTitles[i] = tb;
                    istitle = false;
                }
                else
                {
                    MyTextBoxLocations[i] = tb;
                    istitle = true;
                    i++;
                }

                if (i >= 5)
                    break;
            }
            catch { }
        }
    }
    protected void btnSeo_Click(object sender, EventArgs e)
    {
        Regex regex = new Regex("[^a-zA-Z0-9]");
        //tbSeo.Text = regex.Replace(tbTitle.Text, "").ToLower();
        tbSeo.Text = regex.Replace(ddlOrganization.SelectedItem.Text, "").ToLower();

    }
    private void ClearFields()
    {
        MyId = "0";
        OrgId = "0";

        tbAboutUs.Text = "";
        tbAchievements.Text = "";
        tbAddress.Text = "";
        tbCity.Text = "";
        tbEmail.Text = "";
        tbFax.Text = "";
        tbMission.Text = "";
        tbOurPartners.Text = "";
        tbLearnMoreAboutUs.Text = "";
        tbOurStory.Text = "";
        tbOurTeam.Text = "";
        tbPhone.Text = "";
        tbPostal.Text = "";
        tbServices.Text = "";
        //tbTitle.Text = "";
        ddlOrganization.ClearSelection();
        litTitle.Text = "";
        tbTollFree.Text = "";
        tbUrl.Text = "";
        imgImage.ImageUrl = "";
        imgImage.AlternateText = "";
        imgLogo.ImageUrl = "";
        imgLogo.AlternateText = "";
        tbAltText_Img.Text = "";
        tbAltText_Logo.Text = "";
        tbSeo.Text = "";

        tbFacebook.Text = "";
        tbInstagram.Text = "";
        tbTwitter.Text = "";
        tbLinkedIn.Text = "";
        tbYouTube.Text = "";

        txtLoaction1.Text = "";
        txtLoaction2.Text = "";
        txtLoaction3.Text = "";
        txtLoaction4.Text = "";
        txtLoaction5.Text = "";

        imgLogo.Visible = lnkDeleteLogo.Visible = false;
        imgImage.Visible = lnkDeleteImage.Visible = false;

        cbFeatured.Checked = false;
        cbActive.Checked = false;

        ddlBackgroundPosition.ClearSelection();
        ddlBackgroundPosition2.ClearSelection();
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
                sql = "eko.Organizations_Ins";
            }
            else
            {
                //Edit
                sql = "eko.Organizations_Upd";
                prms.Add(new SqlParameter("@id", MyId));
            }

            //Set SQL Parameters:
            #region SQL Parameters
            prms.Add(new SqlParameter("@orgid", ddlOrganization.SelectedValue));
            prms.Add(new SqlParameter("@name", ddlOrganization.SelectedItem.Text));
            prms.Add(new SqlParameter("@seo", tbSeo.Text.ToLower()));
            prms.Add(new SqlParameter("@Address", tbAddress.Text));
            prms.Add(new SqlParameter("@City", tbCity.Text));
            prms.Add(new SqlParameter("@PostalCode", tbPostal.Text));
            prms.Add(new SqlParameter("@PhoneNumber", tbPhone.Text));
            prms.Add(new SqlParameter("@Fax", tbFax.Text));
            prms.Add(new SqlParameter("@Toll_free", tbTollFree.Text));
            prms.Add(new SqlParameter("@Email", tbEmail.Text));
            prms.Add(new SqlParameter("@URL", tbUrl.Text));
            prms.Add(new SqlParameter("@Mission", tbMission.Text));
            prms.Add(new SqlParameter("@AboutUs", tbAboutUs.Text));
            prms.Add(new SqlParameter("@OurStory", tbOurStory.Text));
            prms.Add(new SqlParameter("@Services", tbServices.Text));
            prms.Add(new SqlParameter("@OurTeam", tbOurTeam.Text));
            prms.Add(new SqlParameter("@Achievements", tbAchievements.Text));
            prms.Add(new SqlParameter("@OurPartners", tbOurPartners.Text));
            prms.Add(new SqlParameter("@AltTextLogo", tbAltText_Logo.Text));
            prms.Add(new SqlParameter("@AltTextImg", tbAltText_Img.Text));
            prms.Add(new SqlParameter("@Featured", cbFeatured.Checked ? 1 : 0));
            prms.Add(new SqlParameter("@active", cbActive.Checked ? 1 : 0));
            prms.Add(new SqlParameter("@bannerpos", ddlBackgroundPosition.SelectedValue));
            prms.Add(new SqlParameter("@bannerpos_hor", ddlBackgroundPosition2.SelectedValue));

            prms.Add(new SqlParameter("@Facebook", tbFacebook.Text));
            prms.Add(new SqlParameter("@Instagram", tbInstagram.Text));
            prms.Add(new SqlParameter("@Twitter", tbTwitter.Text));
            prms.Add(new SqlParameter("@LinkedIn", tbLinkedIn.Text));
            prms.Add(new SqlParameter("@YouTube", tbYouTube.Text));

            prms.Add(new SqlParameter("@LearnMoreAboutUs", tbLearnMoreAboutUs.Text));

            if (txtLoaction1.Text != "")
            {
                prms.Add(new SqlParameter("@Location1", txtLoaction1.Text));
                prms.Add(new SqlParameter("@Title1", txtTitle1.Text));
            }
            if (txtLoaction2.Text != "")
            {
                prms.Add(new SqlParameter("@Location2", txtLoaction2.Text));
                prms.Add(new SqlParameter("@Title2", txtTitle2.Text));
            }
            if (txtLoaction3.Text != "")
            {
                prms.Add(new SqlParameter("@Location3", txtLoaction3.Text));
                prms.Add(new SqlParameter("@Title3", txtTitle3.Text));
            }
            if (txtLoaction4.Text != "")
            {
                prms.Add(new SqlParameter("@Location4", txtLoaction4.Text));
                prms.Add(new SqlParameter("@Title4", txtTitle4.Text));
            }
            if (txtLoaction5.Text != "")
            {
                prms.Add(new SqlParameter("@Location5", txtLoaction5.Text));
                prms.Add(new SqlParameter("@Title5", txtTitle5.Text));
            }

            prms.Add(new SqlParameter("@type", OrganizationType));

            #endregion


            //Execute SP
            try
            {
                object ret = ExecuteQuery(sql, prms.ToArray(), conn, CommandType.StoredProcedure);
                MyId = ret.ToString();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key row in object") && ex.Message.Contains("IX_Organizations_1"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "warning_seo", "alert(\"This SEO is already in use. Please enter a different value or click 'Sugest me'.\");", true);
                }
                else
                    throw new Exception(ex.Message);
            }

            //Upload files:
            #region Upload Files
            sql = "";
            string fileName = "";
            List<SqlParameter> parmsImg = new List<SqlParameter>();

            if (fuLogo.HasFile)
            {
                fileName = SaveFile(fuLogo);
                sql = "update eko.Organizations set Logo=@Logo ";
                parmsImg.Add(new SqlParameter("@Logo", fileName));

            }

            if (fuImage.HasFile)
            {
                fileName = SaveFile(fuImage);

                if (sql.Length == 0)
                    sql = "update eko.Organizations set Image=@Image ";
                else
                    sql += ", Image=@image ";

                parmsImg.Add(new SqlParameter("@image", fileName));

            }

            if (sql.Length > 0)
            {
                sql += " where id=@id";
                parmsImg.Add(new SqlParameter("@id", MyId));

                ExecuteNoQuery(sql, parmsImg.ToArray(), conn, CommandType.Text);
            }
            #endregion

            if (sql == "eko.Organizations_Ins")
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
    string sqlDelFile = "declare @filename nvarchar(500) update eko.Organizations set @filename={0}, {0}='' where id=@id select @filename";
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

    private void BindEditor(Editor editor)
    {
        //string TemplateItemList = "FormatBlock,Paste,PasteText,|,Undo,Redo,|,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,Indent,Outdent,|,InsertLink,|,RemoveFormat,CleanCode";
        string TemplateItemList = "FormatBlock,Paste,PasteText,|,Undo,Redo,|,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,Indent,Outdent,|,InsertLink,|,CleanCode";
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
}