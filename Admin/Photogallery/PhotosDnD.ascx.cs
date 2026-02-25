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
using System.Xml;

public partial class Admin_PhotosDnD : System.Web.UI.UserControl
{
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
        if (!IsPostBack)
        {
            sortExp = "name"; // default sorted column
            sortOrder = "asc";    // default sort order

            if ((bool)Session["Multilingual"])
            {
                trLanguage.Visible = true;
                trLanguage2.Visible = true;
            }

            SqlDataAdapter dapt = new SqlDataAdapter("select id,name from groups where id in (select group_id from users_groups_access where user_id=" + Session["LoggedInID"].ToString() + " and access_level>1) order by name select * from languages", ConfigurationManager.AppSettings["CMServer"]);
            DataSet ds = new DataSet();
            dapt.Fill(ds);
            ddlGroup.DataSource = ds.Tables[0];
            ddlGroup.DataBind();

            ddlLanguage.DataSource = ds.Tables[1];
            ddlLanguage.DataTextField = "name";
            ddlLanguage.DataValueField = "id";
            ddlLanguage.DataBind();

            ddlLanguage2.DataSource = ds.Tables[1];
            ddlLanguage2.DataTextField = "name";
            ddlLanguage2.DataValueField = "id";
            ddlLanguage2.DataBind();

            ////litName.Text = ds.Tables[0].Rows[0]["name"].ToString();

            binddocumentgroups();

            if (Session["docgroup"] != null)
            {
                string physDir = HttpContext.Current.Request.MapPath("~/data/photos/" + Session["docgroup"].ToString());
                string physThumbDir = HttpContext.Current.Request.MapPath("~/data/photos/" + Session["docgroup"].ToString()) + "/thumbs";

                if (!Directory.Exists(physDir))
                {
                    Directory.CreateDirectory(physDir);
                }

                if (!Directory.Exists(physThumbDir))
                {
                    Directory.CreateDirectory(physThumbDir);
                }
			    //upfold.Text = "Photos/" + Session["docgroup"].ToString();
               // ((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = "Photos/" + Session["docgroup"].ToString();
              //  ((ThumbGeneratorAdapter)FileUploader1.Adapters[1]).FolderName = "Photos/" + Session["docgroup"].ToString();
               // ((DocProc.Godd2)FileUploader1.Adapters[2]).groupid = Session["docgroup"].ToString();
                //Response.Write(":" + Session["docgroup"].ToString() + ":");
            }

            //Response.Write("test:" + FileUploader1.JsFunc_Init + ":test");
        
           // bindcontacts();
        }
    }

    private void bindcontacts()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from groups where id in (select group_id from users_groups_access where user_id=" + Session["LoggedInID"].ToString() + " and access_level>1) order by name select id,username + ' (' + name + ')' as username from users order by username", sqlConn);
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
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm;


        //sqlComm = new SqlCommand("insert into photogroups(name,title,language,groupid) values(@name,@title,@language,@groupid) insert into content(name,control,param,language) values(@cname,'photos',@@IDENTITY,@language)", sqlConn);
        sqlComm = new SqlCommand("insert into photogroups(name,title,language,groupid) values(@name,@title,@language,@groupid) insert into content(name,control,param,language) values(@cname,'PhotoNanogallery',@@IDENTITY,@language)", sqlConn);
        sqlComm.Parameters.AddWithValue("@name", txtName.Text);
        sqlComm.Parameters.AddWithValue("@title", txtTitle.Text);
        sqlComm.Parameters.AddWithValue("@groupid", ddlGroup.SelectedValue);
        sqlComm.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);
        sqlComm.Parameters.AddWithValue("@cname", "Photos - " + txtName.Text);

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

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
        //SqlDataAdapter dapt = new SqlDataAdapter("select dg.id,dg.name,g.name as 'group',l.name as language from photogroups dg,groups g,languages l where l.id=dg.language and g.id=dg.groupid and g.id in (select group_id from users_groups_access where user_id=" + Session["LoggedInID"].ToString() + " and access_level>1)", sqlConn);
        SqlDataAdapter dapt = new SqlDataAdapter("select dg.id,dg.name,g.name as 'group',l.name as language from photogroups dg inner join groups g on dg.groupid=g.id inner join languages l on dg.language=l.id where g.id in (select group_id from users_groups_access where user_id=" + Session["LoggedInID"].ToString() + " and access_level>1) and (@lang is null or dg.Language=@lang)", sqlConn);
        //dapt.SelectCommand.Parameters.AddWithValue("@ID", Session["LoggedInID"].ToString());
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
            SqlDataAdapter dapt = new SqlDataAdapter("select * from photos where groupid=@ID order by priority select * from PhotoGroups where id=@ID", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@ID", Session["docgroup"].ToString());
            DataSet ds = new DataSet();
            dapt.Fill(ds);
            GV_Docs.DataSource = ds.Tables[0];
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

            DataRow dr = ds.Tables[1].Rows[0];
            txtName2.Text = dr["name"].ToString();
            txtTitle2.Text = dr["title"].ToString();
            ddlLanguage2.ClearSelection();
            try { ddlLanguage2.SelectedValue = dr["language"].ToString(); }
            catch { }
            ddlMode.SelectedValue = dr["galleryStyle"].ToString();
            if(dr["changeImageOnHover"].ToString() != "")
                cbHover.Checked = Convert.ToBoolean(dr["changeImageOnHover"].ToString());

            if (dr["flickr"].ToString() != "")
            {
                if (cbFlickr.Checked = Convert.ToBoolean(dr["flickr"].ToString()))
                {
                    txtFlickrUserName.Text = dr["FlickrUserName"].ToString();
                    txtflickrSetId.Text = dr["FlickrSetId"].ToString();
                }
            }
            else
            {
                cbFlickr.Checked = false;
                txtFlickrUserName.Text = "";
                txtflickrSetId.Text = "";
            }


            pnlImages.Visible = !cbFlickr.Checked;
            ImageOverButton7.Visible = pnlFlickr.Visible = cbFlickr.Checked;
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
            DataRowView drv = (DataRowView)e.Row.DataItem;

            Literal litFName = (Literal)e.Row.FindControl("litFName");
            litFName.Text = drv["filename"].ToString().Replace("&amp;", "and").Replace("&", " ");

            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("LB_Delete");
            LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure to delete this photo?')");

            Literal litHfId = (Literal)e.Row.FindControl("litHfId");
            litHfId.Text = String.Format("<input type='hidden' class='HfId' value='{0}' />", drv["id"].ToString());
        }
    }
    protected void GV_Docs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Int32 id = Convert.ToInt32(this.GV_Docs.DataKeys[e.RowIndex].Value.ToString());

        //Delete
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("delete from photos where id=@ID", sqlConn);
        sqlComm.Parameters.AddWithValue("@ID", id.ToString());
        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        binddocuments();

        WriteXMLFile();

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
        Session["doc"] = id;

        

        //((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = "/uploads/documents/" + id.ToString();

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select id,name,captionheader,caption from photos where id=@ID", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@ID", id.ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);
        txtDocName.Text = ds.Tables[0].Rows[0]["name"].ToString();
        txtCH.Text = ds.Tables[0].Rows[0]["captionheader"].ToString();
        txtC.Text = ds.Tables[0].Rows[0]["caption"].ToString();

        //binddocuments();

        //lblFrom.Text = ds.Tables[0].Rows[0]["username"].ToString();
        // lblSubject.Text = ds.Tables[0].Rows[0]["subject"].ToString();
        //litMessage.Text = ds.Tables[0].Rows[0]["message"].ToString();

        //Session["mailfrom"] = ds.Tables[0].Rows[0]["fromuser"].ToString();

        WriteXMLFile();
    }

    public void savedocname(object o, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm;


        sqlComm = new SqlCommand("update photos set name=@name, caption=@caption, captionheader=@captionheader where id=@ID", sqlConn);
        sqlComm.Parameters.AddWithValue("@name", txtDocName.Text);
        sqlComm.Parameters.AddWithValue("@ID", Session["doc"].ToString());
        sqlComm.Parameters.AddWithValue("@captionheader", txtCH.Text);
        sqlComm.Parameters.AddWithValue("@caption", txtC.Text);
        //sqlComm.Parameters.AddWithValue("@language", ddlLanguage.SelectedValue);

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        binddocuments();
        backtodoclist(o, e);
    }

    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // Language column
        e.Row.Cells[2].Visible = (bool)Session["Multilingual"];

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
            LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure to delete this photo gallery?')");
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

        //Delete
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        //SqlCommand sqlComm = new SqlCommand("delete from photogroups where id=@ID delete from content where param=@ID and control='photos'", sqlConn);
         SqlCommand sqlComm = new SqlCommand("delete from photogroups where id=@ID delete from content where param=@ID and control='PhotoNanogallery'", sqlConn);
       sqlComm.Parameters.AddWithValue("@ID", id.ToString());
        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        binddocumentgroups();
    }
    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlView.Visible = true;
    	//uploader.Visible = true;

        //ViewState["action"] = "edit";

        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.NewEditIndex].Value.ToString());
        Session["docgroup"] = id;


        string physDir = HttpContext.Current.Request.MapPath("~/data/photos/" + Session["docgroup"].ToString());
        string physThumbDir = HttpContext.Current.Request.MapPath("~/data/photos/" + Session["docgroup"].ToString()) + "/thumbs";

        if (!Directory.Exists(physDir))
        {
            Directory.CreateDirectory(physDir);
        }

        if (!Directory.Exists(physThumbDir))
        {
            Directory.CreateDirectory(physThumbDir);
        }

        //((DocProc.Godd)FileUploader1.Adapters[1]).groupid = "6";
        

       

       // ((FileSaverAdapter)FileUploader1.Adapters[0]).FolderName = "/uploads/documents/" + id.ToString();

        //SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        //SqlDataAdapter dapt = new SqlDataAdapter("select id,name,filename from documents where groupid=@ID", sqlConn);
        //dapt.SelectCommand.Parameters.AddWithValue("@ID", id.ToString());
        //DataSet ds = new DataSet();
        //dapt.Fill(ds);
        binddocuments();

        //lblFrom.Text = ds.Tables[0].Rows[0]["username"].ToString();
       // lblSubject.Text = ds.Tables[0].Rows[0]["subject"].ToString();
        //litMessage.Text = ds.Tables[0].Rows[0]["message"].ToString();

        //Session["mailfrom"] = ds.Tables[0].Rows[0]["fromuser"].ToString();
    }

    

    public void clear()
    {
        txtName.Text = "";
        txtTitle.Text = "";
        ddlGroup.ClearSelection();
        ddlLanguage.ClearSelection();
    }

    public void backtolist(object sender, EventArgs e)
    {
        pnlList.Visible = true;
        pnlSend.Visible = false;
        pnlView.Visible = false;
        pnlEditDoc.Visible = false;

        //uploader.Visible = false;

        this.GV_Main.EditIndex = -1;
        binddocumentgroups();
    }

    public void backtodoclist(object sender, EventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlView.Visible = true;
        pnlEditDoc.Visible=false;
        //uploader.Visible = false;

        this.GV_Docs.EditIndex = -1;
        binddocuments();
    }

    public void newgroup(object s, EventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = true;
        pnlView.Visible = false;
        pnlEditDoc.Visible = false;
		//uploader.Visible = false;
        clear();
    }

	public void fileup(object o, UploaderEventArgs e)
	{
         string bigfile = Server.MapPath("~/data/photos/") + Session["docgroup"].ToString() + "\\" + e.FileName.Replace("&amp;", "and").Replace("&", " ");

        if (File.Exists(bigfile))
        {
            try { File.Delete(bigfile); }
            catch { }
        }

		e.MoveTo(bigfile);

        string newfilename;
        string newfilename2;
        ResizeFromFile(e, bigfile, out newfilename, out newfilename2, 1024, "");
        ResizeFromFile(e, bigfile, out newfilename, out newfilename2, 500, 333, "thumbs\\");

        System.IO.File.Delete(bigfile);

		SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("declare @priority int select @priority=isnull(max(priority),0) from vPhotos where groupid = @groupid  insert into photos(name,filename,groupid,mime,priority) values(@name,@filename,@groupid,@mime,@priority+1)", sqlConn);
		sqlComm.Parameters.AddWithValue("@name", newfilename.Split('.')[0].Replace("%20", " "));
		sqlComm.Parameters.AddWithValue("@groupid", HttpContext.Current.Session["docgroup"].ToString());
        sqlComm.Parameters.AddWithValue("@filename", newfilename2);
		sqlComm.Parameters.AddWithValue("@mime", System.IO.Path.GetExtension(newfilename));
		sqlConn.Open();
		sqlComm.ExecuteNonQuery();
		sqlConn.Close();

        WriteXMLFile();
	}

    private void ResizeFromFile(UploaderEventArgs e, string bigfile, out string newfilename, out string newfilename2, int MaxSideSize, string thumbsFolder)
    {
        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(bigfile);
        ImageFormat fmtImageFormat = imgInput.RawFormat;

        FixOrientation(imgInput);

        #region Calculate new size
        int intNewWidth;
        int intNewHeight;

        //get image original width and height 
        int intOldWidth = imgInput.Width;
        int intOldHeight = imgInput.Height;

        //determine if landscape or portrait 
        int intMaxSide;

        if (thumbsFolder == "")
        {
            if (intOldWidth >= intOldHeight)
            {
                intMaxSide = intOldWidth;
            }
            else
            {
                intMaxSide = intOldHeight;
            }
        }
        else
        {
            if (intOldWidth >= intOldHeight)
            {
                intMaxSide = intOldHeight;
            }
            else
            {
                intMaxSide = intOldWidth;
            }
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
        #endregion

        newfilename = "resized_" + e.FileName;
        newfilename2 = newfilename.Replace("%20", "-");

        string newfile = Server.MapPath("~/data/photos/") + Session["docgroup"].ToString() + "\\" + thumbsFolder + newfilename2;
        if (File.Exists(newfile))
        {
            try { File.Delete(newfile); }
            catch { }
        }

        Bitmap bmpResized = new Bitmap(imgInput, intNewWidth, intNewHeight);
        bmpResized.Save(newfile, fmtImageFormat);
        imgInput.Dispose();
        bmpResized.Dispose();
    }

    private void ResizeFromFile(UploaderEventArgs e, string bigfile, out string newfilename, out string newfilename2, int maxWidth, int maxHeight, string thumbsFolder)
    {
        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(bigfile);
        ImageFormat fmtImageFormat = imgInput.RawFormat;

        FixOrientation(imgInput);

        #region Calculate new size
        int intNewWidth;
        int intNewHeight;
        int intOldWidth = imgInput.Width;
        int intOldHeight = imgInput.Height;

        //      int intMaxSide;
        double dblCoef;

        intNewHeight = intOldHeight;
        intNewWidth = intOldWidth;

        if (intNewWidth > maxWidth)     //485 original value for maxWidth
        {
            dblCoef = intNewWidth / (double)maxWidth;
            intNewWidth = Convert.ToInt32(intNewWidth / dblCoef);
            intNewHeight = Convert.ToInt32(intNewHeight / dblCoef);
        }

        if (intNewHeight > maxHeight)   //364 original value for maxHeight
        {
            dblCoef = intNewHeight / (double)maxHeight;
            intNewWidth = Convert.ToInt32(intNewWidth / dblCoef);
            intNewHeight = Convert.ToInt32(intNewHeight / dblCoef);
        }

        if (intNewWidth > maxWidth)
        {
            dblCoef = intNewWidth / (double)maxWidth;
            intNewWidth = Convert.ToInt32(intNewWidth / dblCoef);
            intNewHeight = Convert.ToInt32(intNewHeight / dblCoef);
        }


        newfilename = "resized_" + e.FileName;
        newfilename2 = newfilename.Replace("%20", "-");
        #endregion

        string newfile = Server.MapPath("~/data/photos/") + Session["docgroup"].ToString() + "\\" + thumbsFolder + newfilename2;
        if (File.Exists(newfile))
        {
            try { File.Delete(newfile); }
            catch { }
        }

        Bitmap bmpResized = new Bitmap(imgInput, intNewWidth, intNewHeight);
        bmpResized.Save(newfile, fmtImageFormat);
        imgInput.Dispose();
        bmpResized.Dispose();
    }

 
    private static void FixOrientation(System.Drawing.Image imgInput)
    {
        if (Array.IndexOf(imgInput.PropertyIdList, 274) > -1)
        {
            var orientation = (int)imgInput.GetPropertyItem(274).Value[0];
            switch (orientation)
            {
                case 1:
                    // No rotation required.
                    break;
                case 2:
                    imgInput.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case 3:
                    imgInput.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 4:
                    imgInput.RotateFlip(RotateFlipType.Rotate180FlipX);
                    break;
                case 5:
                    imgInput.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case 6:
                    imgInput.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 7:
                    imgInput.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;
                case 8:
                    imgInput.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }
            // This EXIF data is now invalid and should be removed.
            imgInput.RemovePropertyItem(274);
        }
    }

    public void ResizeFromStream(string ImageSavePath, int MaxSideSize, Stream Buffer)
	{
		int intNewWidth;
		int intNewHeight;
		System.Drawing.Image imgInput = System.Drawing.Image.FromStream(Buffer);

        FixOrientation(imgInput);

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

    protected void GV_Docs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Down")
        {            
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlCommand sqlComm = new SqlCommand("Photos_Priority_Decrease", sqlConn);
            sqlComm.CommandType = CommandType.StoredProcedure; 
            
            sqlComm.Parameters.AddWithValue("@ID", e.CommandArgument);
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
            binddocuments();

            WriteXMLFile();

        }
        else if (e.CommandName == "Up")
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
            SqlCommand sqlComm = new SqlCommand("Photos_Priority_Increase", sqlConn);
            sqlComm.CommandType = CommandType.StoredProcedure;

            sqlComm.Parameters.AddWithValue("@ID", e.CommandArgument);
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
            binddocuments();

            WriteXMLFile();
        }
    }

    //protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}

    protected void ImageOverButton6_Click(object sender, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("update PhotoGroups set name = @name, title = @title, language=@language, galleryStyle=@galleryStyle, changeImageOnHover=@changeImageOnHover, flickr=@flickr, FlickrUserName=@FlickrUserName, FlickrSetId=@FlickrSetId where id=@id update Content set name='Photos - ' + @name, language=@language where control='PhotoNanogallery' and param=@id", sqlConn);

        sqlComm.Parameters.AddWithValue("@id", Session["docgroup"].ToString());
        sqlComm.Parameters.AddWithValue("@name", txtName2.Text);
        sqlComm.Parameters.AddWithValue("@title", txtTitle2.Text);
        sqlComm.Parameters.AddWithValue("@language", ddlLanguage2.SelectedValue);
        sqlComm.Parameters.AddWithValue("@galleryStyle", ddlMode.SelectedValue);
        sqlComm.Parameters.AddWithValue("@changeImageOnHover", cbHover.Checked.ToString());
        sqlComm.Parameters.AddWithValue("@flickr", cbFlickr.Checked.ToString());
        sqlComm.Parameters.AddWithValue("@FlickrUserName", txtFlickrUserName.Text);
        sqlComm.Parameters.AddWithValue("@FlickrSetId", txtflickrSetId.Text);

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        if (cbFlickr.Checked)
            WriteXMLFile_Flickr();
        else
            WriteXMLFile();

        backtolist(sender, e);
        //binddocuments();
    }

    private void WriteXMLFile()
    {
        // Create a new XmlTextWriter instance
        XmlTextWriter writer = new
             XmlTextWriter(Server.MapPath("~/data/photos/" + Session["docgroup"].ToString() + "/gallery.xml"), Encoding.UTF8);

        // start writing!
        writer.WriteStartDocument();
        writer.WriteStartElement("simpleviewergallery");
        writer.WriteAttributeString("maxImageWidth", "700");
        writer.WriteAttributeString("maxImageHeight", "500");
        writer.WriteAttributeString("imageQuality", "80");
        writer.WriteAttributeString("thumbWidth", "75");
        writer.WriteAttributeString("thumbHeight", "75");
        writer.WriteAttributeString("thumbQuality", "90");
        writer.WriteAttributeString("useFlickr", "false");
        writer.WriteAttributeString("resizeOnImport", "true");
        writer.WriteAttributeString("cropToFit", "false");
        writer.WriteAttributeString("galleryStyle", ddlMode.SelectedValue);
        writer.WriteAttributeString("backgroundTransparent", "true");
        writer.WriteAttributeString("galleryWidth", "665");
        writer.WriteAttributeString("galleryHeight", "700");
 
        if (ddlMode.SelectedValue == "COMPACT")
        {
            writer.WriteAttributeString("thumbPosition", "BOTTOM");
            writer.WriteAttributeString("thumbColumns", "7");
            writer.WriteAttributeString("thumbRows", "1");
            writer.WriteAttributeString("frameWidth", "0");
            writer.WriteAttributeString("textColor", "#ffffff");

        }
        else
        {
            writer.WriteAttributeString("useFlash", "true");
            writer.WriteAttributeString("thumbNavStyle", "CLASSIC");
            writer.WriteAttributeString("thumbDropShadow", "true");
            writer.WriteAttributeString("changeImageOnHover", cbHover.Checked.ToString().ToLower());
            writer.WriteAttributeString("changeCaptionOnHover", "true");
            writer.WriteAttributeString("mobileShowNav", "true");
            writer.WriteAttributeString("thumbNavPosition", "BOTTOM");
            writer.WriteAttributeString("imageScaleMode", "SCALE");
            writer.WriteAttributeString("imageTransitionType", "FADE");
            writer.WriteAttributeString("showImageNav", "HOVER");
            writer.WriteAttributeString("showBigPlayButton", "false");
            writer.WriteAttributeString("thumbFrameStyle", "SQUARE");
            writer.WriteAttributeString("useFixedLayout", "true");
            writer.WriteAttributeString("imageAreaWidth", "360");
            writer.WriteAttributeString("imageAreaHeight", "500");
            writer.WriteAttributeString("textColor", "#0d4134");
        }

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from photos where groupid=@ID order by priority", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@ID", Session["docgroup"].ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        DataTable dt = ds.Tables[0];

        foreach (DataRow dr in dt.Rows)
        {
            // Creating the <imageURL> elements
            writer.WriteStartElement("image");
            writer.WriteAttributeString("imageURL", "/data/photos/" + Session["docgroup"].ToString() + "/" + dr["filename"].ToString());
            writer.WriteAttributeString("thumbURL", "/data/photos/" + Session["docgroup"].ToString() + "/thumbs/" + dr["filename"].ToString());
            writer.WriteAttributeString("linkURL", "/data/photos/" + Session["docgroup"].ToString() + "/" + dr["filename"].ToString());
            writer.WriteAttributeString("linkTarget", "_blank");
            writer.WriteStartElement("caption");
            writer.WriteCData( dr["caption"].ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();    


    }

    private void WriteXMLFile_Flickr()
    {
        // Create a new XmlTextWriter instance
        XmlTextWriter writer = new
             XmlTextWriter(Server.MapPath("~/data/photos/" + Session["docgroup"].ToString() + "/gallery.xml"), Encoding.UTF8);

        // start writing!
        writer.WriteStartDocument();
        writer.WriteStartElement("simpleviewergallery");

        writer.WriteAttributeString("useFlickr", "true");
        writer.WriteAttributeString("flickrUserName", txtFlickrUserName.Text);
        writer.WriteAttributeString("flickrSetId", txtflickrSetId.Text);
        writer.WriteAttributeString("galleryStyle", ddlMode.SelectedValue);
        writer.WriteAttributeString("backgroundTransparent", "true");
        writer.WriteAttributeString("galleryWidth", "665");
        writer.WriteAttributeString("galleryHeight", "600");
        

        if (ddlMode.SelectedValue == "COMPACT")
        {
            writer.WriteAttributeString("thumbPosition", "BOTTOM");
            writer.WriteAttributeString("thumbColumns", "7");
            writer.WriteAttributeString("thumbRows", "1");
            writer.WriteAttributeString("textColor", "#ffffff");
        }
        else
        {
            writer.WriteAttributeString("textColor", "#0d4134");
        }


        writer.WriteAttributeString("flickrShowTitle", "false");
        writer.WriteAttributeString("flickrShowDescription", "false");
        writer.WriteAttributeString("changeImageOnHover", cbHover.Checked.ToString().ToLower());

        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();


    }
    protected void cbFlickr_CheckedChanged(object sender, EventArgs e)
    {

        ImageOverButton7.Visible = pnlFlickr.Visible = cbFlickr.Checked;

        if (pnlImages.Visible = !cbFlickr.Checked)
        {
            txtFlickrUserName.Text = "";
            txtflickrSetId.Text = "";
        }
    }
}
