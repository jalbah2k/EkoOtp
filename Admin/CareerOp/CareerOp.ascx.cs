using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using CuteWebUI;
using Image = System.Web.UI.WebControls.Image;

public partial class Admin_Banner : System.Web.UI.UserControl
{
    public Admin_Banner()
    {

    }

    protected void Page_Load(object sender, EventArgs e)
    {


        ////litName.Text = ds.Tables[0].Rows[0]["name"].ToString();

        binddocumentgroups();

        //GV_Main_RowEditing();

        /*
        if (Session["azgroup"] != null)
        {
            string physDir = HttpContext.Current.Request.MapPath("/Photos/" + Session["docgroup"].ToString());

            if (!Directory.Exists(physDir))
            {
                Directory.CreateDirectory(physDir);
            }

            //upfold.Text = "Photos/" + Session["docgroup"].ToString();
            // ((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = "Photos/" + Session["docgroup"].ToString();
            //  ((ThumbGeneratorAdapter)FileUploader1.Adapters[1]).FolderName = "Photos/" + Session["docgroup"].ToString();
            // ((DocProc.Godd2)FileUploader1.Adapters[2]).groupid = Session["docgroup"].ToString();
            //Response.Write(":" + Session["docgroup"].ToString() + ":");
        }*/

        //Response.Write("test:" + FileUploader1.JsFunc_Init + ":test");

        // bindcontacts();

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

    public void addnewlist(object o, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm;


        sqlComm = new SqlCommand("declare @temp int insert into resourcelists2(name,groupid) values(@name,@groupid) set @temp=@@IDENTITY insert into content(name,control,param,language) values('Resources - '+@name,'resources',@temp,1)", sqlConn);
        sqlComm.Parameters.AddWithValue("@name", Textbox3.Text);
        sqlComm.Parameters.AddWithValue("@groupid", ddlGroupsA.SelectedValue);

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        //binddocumentgroups();
        backtolists(o, e);
    }

    public void addnewgroup(object o, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm;


        sqlComm = new SqlCommand("declare @temp int insert into resourcegroups2(name,listid) values(@name,@groupid) set @temp=@@IDENTITY", sqlConn);
        sqlComm.Parameters.AddWithValue("@name", txtName.Text);
        sqlComm.Parameters.AddWithValue("@groupid", Session["azlist"].ToString());

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        //binddocumentgroups();


        
        backtolist(o, e);
    }

    private void binddocumentgroups()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select *,groups.name as groupname from resourcelists2,groups where resourcelists2.groupid=groups.id ", sqlConn);
        //dapt.SelectCommand.Parameters.AddWithValue("@ID", Session["LoggedInID"].ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        GV_Main.DataSource = ds.Tables[0];
        GV_Main.DataBind();
/*
        if (ds.Tables[0].Rows.Count < 1)
        {
            list.Visible = false;
            nolist.Visible = true;
        }
        else
        {
            list.Visible = true;
            nolist.Visible = false;
        }*/
    }

    private void binddocumentgroups2()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from resourcegroups2 where listid=@id order by name", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@ID", Session["azlist"].ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        GV_Main2.DataSource = ds.Tables[0];
        GV_Main2.DataBind();

        
        if (ds.Tables[0].Rows.Count < 1)
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
        if (Session["azgroup"] != null)
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlDataAdapter dapt = new SqlDataAdapter("select * from resources2 where resourcegroup=@ID", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@ID", Session["azgroup"].ToString());
            DataSet ds = new DataSet();
            dapt.Fill(ds);
            this.GV_Docs.DataSource = ds.Tables[0];
            GV_Docs.DataBind();

            if (ds.Tables[0].Rows.Count < 1)
            {
                doctbl.Visible = false;
                nopic.Visible = true;
            }
            else
            {
                doctbl.Visible = true;
                nopic.Visible = false;
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

    protected void GV_Docs_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < this.GV_Docs.Rows.Count; i++)
        {

            //for delete button
            ImageButton lb;
            lb = (ImageButton)this.GV_Docs.Rows[i].Cells[3].FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this link?');");

        }


    }

    protected void GV_Docs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GV_Docs.PageIndex = e.NewPageIndex;
        binddocuments();
    }

    protected void GV_Docs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Int32 id = Convert.ToInt32(this.GV_Docs.DataKeys[e.RowIndex].Value.ToString());

        //Delete
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("delete from resources2 where id=@ID", sqlConn);
        sqlComm.Parameters.AddWithValue("@ID", id.ToString());
        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        binddocuments();

    }

    public void addnewlink(object o, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from languages select pages.name, '/' + pages.seo as url from pages,Languages l where l.id=pages.language order by pages.name", sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        //ddlLanguage.DataSource = ds.Tables[0];
        //ddlLanguage.DataTextField = "name";
       // ddlLanguage.DataValueField = "id";
        //ddlLanguage.DataBind();

        ddlPages.DataSource = ds.Tables[1];
        ddlPages.DataTextField = "name";
        ddlPages.DataValueField = "url";
        ddlPages.DataBind();

        ddlPages.Items.Insert(0, new ListItem("Select a page", "0"));

        Textbox1.Text = "";
        Textbox2.Text = "";
        pnlView.Visible = false;
        pnlAddDoc.Visible = true;
    }
    
    protected void GV_Docs_RowEditing(object sender, GridViewEditEventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlView.Visible = false;
        pnlEditDoc.Visible = true;
        //uploader.Visible = true;

        //ViewState["action"] = "edit";

        Int32 id = Convert.ToInt32(this.GV_Docs.DataKeys[e.NewEditIndex].Value.ToString());
        Session["hb"] = id;



        //((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = "/uploads/documents/" + id.ToString();

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from resources2 where id=@ID", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@ID", id.ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        txtDocName.Text = ds.Tables[0].Rows[0]["name"].ToString();
        //txtCH.Text = ds.Tables[0].Rows[0]["captionheader"].ToString();
        //txtC.Text = ds.Tables[0].Rows[0]["caption"].ToString();
        txtUrl.Text = ds.Tables[0].Rows[0]["url"].ToString();
        txtDescr.Text = ds.Tables[0].Rows[0]["description"].ToString();
        ddlTarget.ClearSelection();
        ddlTarget.Items.FindByValue(ds.Tables[0].Rows[0]["target"].ToString()).Selected = true;
        //ddlTarget.ClearSelection();
        //ddlTarget.Items.FindByValue(ds.Tables[0].Rows[0]["target"].ToString()).Selected = true;

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


        sqlComm = new SqlCommand("update resources2 set name=@name,url=@url,[description]=@description,target=@target where id=@ID", sqlConn);
        sqlComm.Parameters.AddWithValue("@name", txtDocName.Text.Replace("'", "&#39;"));
        sqlComm.Parameters.AddWithValue("@ID", Session["hb"].ToString());
       // sqlComm.Parameters.AddWithValue("@captionheader", txtCH.Text);
        //sqlComm.Parameters.AddWithValue("@caption", txtC.Text.Replace("'", "&#39;"));
        sqlComm.Parameters.AddWithValue("@url", txtUrl.Text);
        sqlComm.Parameters.AddWithValue("@description", txtDescr.Text);
        sqlComm.Parameters.AddWithValue("@target", ddlTarget.SelectedValue);
       // sqlComm.Parameters.AddWithValue("@target", ddlTarget.SelectedValue);
        //sqlComm.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        binddocuments();
        backtodoclist(o, e);
    }

    public void savenewlink(object o, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm;

        if (fileid.Text == "")
        {


            sqlComm = new SqlCommand("insert into resources2(resourcegroup,name,url,[description],target) values(@cat,@name,@url,@description,@target)", sqlConn);
            sqlComm.Parameters.AddWithValue("@name", Textbox1.Text.Replace("'", "&#39;"));
            sqlComm.Parameters.AddWithValue("@cat", Session["azgroup"].ToString());
            //sqlComm.Parameters.AddWithValue("@caption", txtC.Text.Replace("'", "&#39;"));
			sqlComm.Parameters.AddWithValue("@url", (Textbox2.Text.Trim() == "" ? (ddlPages.SelectedValue == "0" ? "/" : ddlPages.SelectedValue) : Textbox2.Text));
            sqlComm.Parameters.AddWithValue("@description", txtDescrAdd.Text);
            // sqlComm.Parameters.AddWithValue("@target", ddlTarget.SelectedValue);
            //sqlComm.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);

            sqlComm.Parameters.AddWithValue("@target", ddlTargetAdd.SelectedValue);

            
        }
        else
        {

            sqlComm = new SqlCommand("insert into resources2(resourcegroup,name,url,[description],target) values(@cat,(select name from files where id=@id),@url,@description,@target)", sqlConn);
            //sqlComm.Parameters.AddWithValue("@name", Textbox1.Text.Replace("'", "&#39;"));
            sqlComm.Parameters.AddWithValue("@cat", Session["azgroup"].ToString());
            //sqlComm.Parameters.AddWithValue("@caption", txtC.Text.Replace("'", "&#39;"));
            sqlComm.Parameters.AddWithValue("@id", fileid.Text);
            sqlComm.Parameters.AddWithValue("@url", "/files/" + fileid.Text);
            sqlComm.Parameters.AddWithValue("@description", txtDescrAdd.Text);
            sqlComm.Parameters.AddWithValue("@target", ddlTargetAdd.SelectedValue);
            // sqlComm.Parameters.AddWithValue("@target", ddlTarget.SelectedValue);
            //sqlComm.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);

        }



        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();
        binddocuments();
        backtodoclist(o, e);

        txtDescrAdd.Text = "";
        txtDescr.Text = "";
        txtName.Text = "";
    }

    protected void GV_Main_DataBound(object sender, EventArgs e)
    {
        for (int i = 0; i < this.GV_Main.Rows.Count; i++)
        {

            //for delete button
            ImageButton lb;
            lb = (ImageButton)this.GV_Main.Rows[i].Cells[3].FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this list?');");

        }


    }

    protected void GV_Main_DataBound2(object sender, EventArgs e)
    {
        for (int i = 0; i < this.GV_Main.Rows.Count; i++)
        {

            //for delete button
            ImageButton lb;
            lb = (ImageButton)this.GV_Main.Rows[i].Cells[3].FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this group?');");

        }


    }

    protected void GV_Main_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GV_Main.PageIndex = e.NewPageIndex;
        //binddocumentgroups();
    }

    protected void GV_Main_PageIndexChanging2(object sender, GridViewPageEventArgs e)
    {
        this.GV_Main2.PageIndex = e.NewPageIndex;
        binddocumentgroups2();
    }


    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.RowIndex].Value.ToString());

        //Delete
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("delete from resourcelists2 where id=@ID delete from content where control='resources' and param=@ID", sqlConn);
        sqlComm.Parameters.AddWithValue("@ID", id.ToString());
        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        binddocumentgroups();

    }

    protected void GV_Main_RowDeleting2(object sender, GridViewDeleteEventArgs e)
    {
        Int32 id = Convert.ToInt32(this.GV_Main2.DataKeys[e.RowIndex].Value.ToString());

        //Delete
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("delete from resourcegroups2 where id=@ID", sqlConn);
        sqlComm.Parameters.AddWithValue("@ID", id.ToString());
        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        binddocumentgroups2();

    }

    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Session["gve"] = e;
        GV_Main_RowEditing();
    }

    protected void GV_Main_RowEditing2(object sender, GridViewEditEventArgs e)
    {
        Session["gve"] = e;
        GV_Main_RowEditing2();
    }

    protected void GV_Main_RowEditing()
    {
        GridViewEditEventArgs e = (GridViewEditEventArgs)Session["gve"];

        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlListGroups.Visible = true;
        pnlView.Visible = false;
        //uploader.Visible = true;

        //ViewState["action"] = "edit";

        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.NewEditIndex].Value.ToString());
        Session["azlist"] = id;
        //Session["azgroup"] = id;

        //((DocProc.Godd)FileUploader1.Adapters[1]).groupid = "6";




        // ((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = "/uploads/documents/" + id.ToString();

        //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        //SqlDataAdapter dapt = new SqlDataAdapter("select id,name,filename from documents where groupid=@ID", sqlConn);
        //dapt.SelectCommand.Parameters.AddWithValue("@ID", id.ToString());
        //DataSet ds = new DataSet();
        //dapt.Fill(ds);

        binddocumentgroups2();
        //binddocuments();

        //lblFrom.Text = ds.Tables[0].Rows[0]["username"].ToString();
        // lblSubject.Text = ds.Tables[0].Rows[0]["subject"].ToString();
        //litMessage.Text = ds.Tables[0].Rows[0]["message"].ToString();

        //Session["mailfrom"] = ds.Tables[0].Rows[0]["fromuser"].ToString();
    }

    protected void GV_Main_RowEditing2()
    {
        GridViewEditEventArgs e = (GridViewEditEventArgs)Session["gve"];

        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlListGroups.Visible = false;
        pnlView.Visible = true;
        //uploader.Visible = true;

        //ViewState["action"] = "edit";

        Int32 id = Convert.ToInt32(this.GV_Main2.DataKeys[e.NewEditIndex].Value.ToString());
        //Session["azlist"] = id;
        Session["azgroup"] = id;

        //((DocProc.Godd)FileUploader1.Adapters[1]).groupid = "6";




        // ((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = "/uploads/documents/" + id.ToString();

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select id,name from resourcegroups2 where id=@ID", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@ID", id.ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        
        txtGroupNameEdit.Text = ds.Tables[0].Rows[0]["name"].ToString();

        //binddocumentgroups2();
        binddocuments();

        //lblFrom.Text = ds.Tables[0].Rows[0]["username"].ToString();
        // lblSubject.Text = ds.Tables[0].Rows[0]["subject"].ToString();
        //litMessage.Text = ds.Tables[0].Rows[0]["message"].ToString();

        //Session["mailfrom"] = ds.Tables[0].Rows[0]["fromuser"].ToString();
    }

	public void SaveGroupName(object o, EventArgs e)
	{
		SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
		SqlCommand command = new SqlCommand("update resourcegroups2 set name=@name where id=@id",sqlConn);
		command.Parameters.AddWithValue("@id",Session["azgroup"].ToString());
		command.Parameters.AddWithValue("@name",txtGroupNameEdit.Text);
		
		sqlConn.Open();
		command.ExecuteNonQuery();
		sqlConn.Close();
	}

    public void clear()
    {
        txtName.Text = "";
        txtDescr.Text = "";
        txtDescrAdd.Text = "";
        //ddlGroup.ClearSelection();
       // ddlLanguage.ClearSelection();
    }

    public void backtolists(object sender, EventArgs e)
    {
        pnlMakeList.Visible = false;
        pnlListGroups.Visible = false;
        pnlList.Visible = true;
        pnlSend.Visible = false;
        pnlView.Visible = false;
        pnlEditDoc.Visible = false;

        //uploader.Visible = false;

        binddocumentgroups();
    }

    public void backtolist(object sender, EventArgs e)
    {
        pnlListGroups.Visible = true;
        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlView.Visible = false;
        pnlEditDoc.Visible = false;

        //uploader.Visible = false;

        binddocumentgroups2();
    }

    public void backtodoclist(object sender, EventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlAddDoc.Visible = false;
        pnlView.Visible = true;
        pnlEditDoc.Visible = false;
        //uploader.Visible = false;

        binddocuments();
    }

    public void newlist(object s, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from groups order by name select pages.name, '/' + pages.seo as url from pages,Languages l where l.id=pages.language order by pages.name", sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        //ddlLanguage.DataSource = ds.Tables[0];
        // ddlLanguage.DataTextField = "name";
        //ddlLanguage.DataValueField = "id";
        //ddlLanguage.DataBind();

        ddlGroupsA.DataSource = ds.Tables[0];
        ddlGroupsA.DataTextField = "name";
        ddlGroupsA.DataValueField = "id";
        ddlGroupsA.DataBind();

        ddlPages.DataSource = ds.Tables[1];
        ddlPages.DataTextField = "name";
        ddlPages.DataValueField = "url";
        ddlPages.DataBind();

        ddlPages.Items.Insert(0, new ListItem("Select a page", "0"));

        pnlMakeList.Visible = true;
        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlView.Visible = false;
        pnlEditDoc.Visible = false;
        //uploader.Visible = false;
        clear();
    }

    public void newgroup(object s, EventArgs e)
    {
        //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        //SqlDataAdapter dapt = new SqlDataAdapter("select * from groups order by name select pages.name, '/' + substring(l.name,0,3) + '/' + pages.seo as url from pages,Languages l where l.id=pages.language order by pages.name", sqlConn);
        //DataSet ds = new DataSet();
        //dapt.Fill(ds);

        //ddlLanguage.DataSource = ds.Tables[0];
       // ddlLanguage.DataTextField = "name";
        //ddlLanguage.DataValueField = "id";
        //ddlLanguage.DataBind();

        //ddlGroupsA.DataSource = ds.Tables[0];
       // ddlGroupsA.DataTextField = "name";
       // ddlGroupsA.DataValueField = "id";
       // ddlGroupsA.DataBind();

       // ddlPages.DataSource = ds.Tables[1];
       // ddlPages.DataTextField = "name";
       // ddlPages.DataValueField = "url";
       // ddlPages.DataBind();

       // ddlPages.Items.Insert(0, new ListItem("Select a page", "0"));

        pnlListGroups.Visible = false;
        pnlList.Visible = false;
        pnlSend.Visible = true;
        pnlView.Visible = false;
        pnlEditDoc.Visible = false;
        //uploader.Visible = false;
        clear();
    }

    public void fileup(object o, UploaderEventArgs e)
    {
        string bigfile = "C:\\websites\\hrrh\\banners\\" + Session["docgroup"].ToString() + "\\" + e.FileName;
        System.IO.File.Delete(bigfile);

        e.MoveTo(bigfile);

        int intNewWidth;
        int intNewHeight;
        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(bigfile);
        ImageFormat fmtImageFormat = imgInput.RawFormat;
        int intOldWidth = imgInput.Width;
        int intOldHeight = imgInput.Height;

        int intMaxSide;
        double dblCoef;

        /*
        intNewHeight = intOldHeight;
        intNewWidth = intOldWidth;

		
        if (intOldWidth > 544)
        {
            dblCoef = intOldWidth / (double)544;
            intNewWidth = Convert.ToInt32(intOldWidth / dblCoef);
            intNewHeight = Convert.ToInt32(intOldHeight / dblCoef);
        }

        if (intOldHeight > 408)
        {
            dblCoef = intOldHeight / (double)408;
            intNewWidth = Convert.ToInt32(intOldWidth / dblCoef);
            intNewHeight = Convert.ToInt32(intOldHeight / dblCoef);
        }
        */

        intNewWidth = 150;
        intNewHeight = 69;

        string newfilename = "thumb_" + e.FileName;
        string newfile = "C:\\websites\\hrrh\\banners\\" + Session["docgroup"].ToString() + "\\" + newfilename;
        System.IO.File.Delete(newfile);
        Bitmap bmpResized = new Bitmap(imgInput, intNewWidth, intNewHeight);
        bmpResized.Save(newfile, fmtImageFormat);
        imgInput.Dispose();
        bmpResized.Dispose();



        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("insert into banners(name,filename,groupid,mime,url,target) values(@name,@filename,@groupid,@mime,'#','_self')", sqlConn);
        sqlComm.Parameters.AddWithValue("@name", e.FileName.Split('.')[0]);
        sqlComm.Parameters.AddWithValue("@groupid", HttpContext.Current.Session["docgroup"].ToString());
        sqlComm.Parameters.AddWithValue("@filename", e.FileName);
        sqlComm.Parameters.AddWithValue("@mime", System.IO.Path.GetExtension(e.FileName));
        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();
    }


    public void ResizeFromStream(string ImageSavePath, int MaxSideSize, Stream Buffer)
    {
        int intNewWidth;
        int intNewHeight;
        System.Drawing.Image imgInput = System.Drawing.Image.FromStream(Buffer);

        //Determine image format 
        ImageFormat fmtImageFormat = imgInput.RawFormat;

        //get image original width and height 
        int intOldWidth = imgInput.Width;
        int intOldHeight = imgInput.Height;

        //determine if landscape or portrait 
        int intMaxSide;

        if (intOldWidth >= intOldHeight)
        {
            intMaxSide = intOldWidth;
        }
        else
        {
            intMaxSide = intOldHeight;
        }


        if (intMaxSide > MaxSideSize)
        {
            //set new width and height 
            double dblCoef = MaxSideSize / (double)intMaxSide;
            intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
            intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
        }
        else
        {
            intNewWidth = intOldWidth;
            intNewHeight = intOldHeight;
        }
        //create new bitmap 
        Bitmap bmpResized = new Bitmap(imgInput, intNewWidth, intNewHeight);

        //save bitmap to disk 
        bmpResized.Save(ImageSavePath, fmtImageFormat);

        //release used resources 
        imgInput.Dispose();
        bmpResized.Dispose();
        Buffer.Close();
    }





}
