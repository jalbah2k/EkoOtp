using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class Admin_RelatedLinks_RelatedLinks : System.Web.UI.UserControl
{
    #region DAL

    StringBuilder sb;

    protected string par;

	public Admin_RelatedLinks_RelatedLinks()
	{
		
	}

	public Admin_RelatedLinks_RelatedLinks(string p)
	{
        this.par = p;
	}


    protected void GV_Main_DataBound(object sender, EventArgs e)
    {
   //////     for (int i = 0; i < this.GV_Main.Rows.Count; i++)
   //////     {

   //////         //for delete button
   //////         //ImageButton lb;
   //////        // lb = (ImageButton)this.GV_Main.Rows[i].Cells[3].FindControl("LB_Delete");
   //////         //lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this message?');");

   //////         Image ib = (Image)this.GV_Main.Rows[i].Cells[0].FindControl("imgStatus");
   //////         if (ib.ToolTip.ToLower() == ".doc" || ib.ToolTip.ToLower() == ".docx")
   //////             ib.ImageUrl = "/images/icons/types/doc.png";
			//////else if (ib.ToolTip.ToLower() == ".pdf")
   //////             ib.ImageUrl = "/images/icons/types/pdf.png";
			//////else if (ib.ToolTip.ToLower() == ".ppt" || ib.ToolTip.ToLower() == ".pptx")
   //////             ib.ImageUrl = "/images/icons/types/ppt.png";
			//////else if (ib.ToolTip.ToLower() == ".txt")
   //////             ib.ImageUrl = "/images/icons/types/txt.png";
			//////else if (ib.ToolTip.ToLower() == ".xls" || ib.ToolTip.ToLower() == ".xlsx")
   //////             ib.ImageUrl = "/images/icons/types/xls.png";
			//////else if (ib.ToolTip.ToLower() == ".zip" || ib.ToolTip.ToLower() == ".rar")
			//////	ib.ImageUrl = "/images/icons/types/zip.png";
			//////else if (ib.ToolTip.ToLower() == ".gif")
			//////	ib.ImageUrl = "/images/icons/types/gif.png";
			//////else if (ib.ToolTip.ToLower() == ".jpg" || ib.ToolTip.ToLower() == ".jpeg")
			//////	ib.ImageUrl = "/images/icons/types/jpg.png";
			//////else if (ib.ToolTip.ToLower() == ".png")
			//////	ib.ImageUrl = "/images/icons/types/png.png";
   //////     }
    }


    public DataTable mGet_One_RelatedLinks_ByPageid(string mPageid)
    {
        string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
        string commandString = "select * from documents where groupid = @mPageid order by priority select * from documentgroups where id=@mPageid";
        DataSet ds = new DataSet();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.AddWithValue("@mPageid", mPageid);
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds, "table1");
        }

        if (ds.Tables[1].Rows.Count > 0)
        {
            //if(listname.Text != "")
                listname.Text = "<h2>" + ds.Tables[1].Rows[0]["listname"].ToString() + "</h2>";
            litBtnAddDoc.Text = "<a href='javascript:void(0)' class='BtnAddDoc' title='Add Documents' GalleryId='" + ds.Tables[1].Rows[0]["id"] + "' GalleryName=\"" + ds.Tables[1].Rows[0]["name"] + "\" GalleryTitle=\"" + ds.Tables[1].Rows[0]["listname"] + "\" GalleryGroup='" + ds.Tables[1].Rows[0]["groupid"] + "'><img src='/images/lemonaid/buttons/plus.png' alt='add documents' width='20px' /></a>";
            
            string script = "$(document).ready(function () {" + Environment.NewLine;
            script += "     $('.BtnAddDoc').click(function (e) {" + Environment.NewLine;
            script += "         if (!$('#pnlDocumentsAdd').is(':visible')) {" + Environment.NewLine;
            script += "             $('.hfDocTempFolder').val($(this).attr('GalleryId'));" + Environment.NewLine;
            script += "             $find('wmeDocumentsName').set_Text($(this).attr('GalleryName'));" + Environment.NewLine;
            script += "             $find('wmeDocumentsTitle').set_Text($(this).attr('GalleryTitle'));" + Environment.NewLine;
            script += "             $('.ddlDocumentGroup').val($(this).attr('GalleryGroup'));" + Environment.NewLine;
            script += "             $('.btnFrontDocumentsEdit').click();" + Environment.NewLine;
            script += "             $('#pnlDocumentsAdd').slideDown(600);" + Environment.NewLine;
            script += "         }" + Environment.NewLine;
            script += "         return false;" + Environment.NewLine;
            script += "     });" + Environment.NewLine;
            script += "});" + Environment.NewLine;

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "DocGallery", script, true);
        }

        return ds.Tables[0];
    }
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        sb = new StringBuilder(60);

        if (Session["LoggedInID"] != null)
        {
            if (Permissions.Get(int.Parse(Session["LoggedInID"].ToString()), int.Parse(Session["PageID"].ToString())) > 1)
            //if(Session["LoggedInID"].ToString()=="1")
            {
                //pnlDocument.Attributes.Add("Widget", "documents_" + par);
                btnDocAdd.Visible = true;
            }
        }

        if (!IsPostBack)
        {
			mBindData("");

        }
    }

    #region My_Functions

    private void mBindData(string sortExp)
    {

       // string id = Session["PageID"].ToString();
        DataTable dt = new DataTable();
        dt = mGet_One_RelatedLinks_ByPageid(par);
        DataView DV = dt.DefaultView;
        if (!(sortExp == string.Empty))
        {
            DV.Sort = sortExp;
        }
        this.GV_Main.DataSource = DV;
        this.GV_Main.DataBind();

        if (dt.Rows.Count < 1)
        {
            thetable.Visible = false;
        }
    }

   

    #endregion



    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToString().ToLower() == "filename")
        {
            string s = e.CommandArgument.ToString();
            Response.Redirect(s);
        }
    }


    protected void GV_Main_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //{
        //    DataRowView drv = (DataRowView)e.Item.DataItem;

        //    Image ib = (Image)e.Item.FindControl("imgStatus");
        //    if (ib.ToolTip.ToLower() == ".doc" || ib.ToolTip.ToLower() == ".docx")
        //        ib.ImageUrl = "/images/icons/types/doc.png";
        //    else if (ib.ToolTip.ToLower() == ".pdf")
        //        ib.ImageUrl = "/images/icons/types/pdf.png";
        //    else if (ib.ToolTip.ToLower() == ".ppt" || ib.ToolTip.ToLower() == ".pptx")
        //        ib.ImageUrl = "/images/icons/types/ppt.png";
        //    else if (ib.ToolTip.ToLower() == ".txt")
        //        ib.ImageUrl = "/images/icons/types/txt.png";
        //    else if (ib.ToolTip.ToLower() == ".xls" || ib.ToolTip.ToLower() == ".xlsx")
        //        ib.ImageUrl = "/images/icons/types/xls.png";
        //    else if (ib.ToolTip.ToLower() == ".zip" || ib.ToolTip.ToLower() == ".rar")
        //        ib.ImageUrl = "/images/icons/types/zip.png";
        //    else if (ib.ToolTip.ToLower() == ".gif")
        //        ib.ImageUrl = "/images/icons/types/gif.png";
        //    else if (ib.ToolTip.ToLower() == ".jpg" || ib.ToolTip.ToLower() == ".jpeg")
        //        ib.ImageUrl = "/images/icons/types/jpg.png";
        //    else if (ib.ToolTip.ToLower() == ".png")
        //        ib.ImageUrl = "/images/icons/types/png.png";
        //}
    }
}
