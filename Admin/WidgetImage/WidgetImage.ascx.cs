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

public partial class WidgetImage : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindGroups();
        }
    }

    public string Parameter
    {
        set { ViewState["Parameter"] = value; }
        get { return ViewState["Parameter"].ToString(); }
    }

    int Gallery
    {
        set { ViewState["Gallery"] = value; }
        get { return Convert.ToInt32(ViewState["Gallery"]); }
    }

    int Item
    {
        set { ViewState["Item"] = value; }
        get { return Convert.ToInt32(ViewState["Item"]); }
    }

    private void BindGroups()
    {
        DataSet ds = new DataSet();
        string sqlstr = " select * from WidgetGallery where type=@type  select * from languages";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@type", Parameter);
        ds = getDataset(sq, true);


        this.GV_Main.DataSource = ds.Tables[0];
        this.GV_Main.DataBind();

        ddlLanguage.DataSource = ds.Tables[1];
        ddlLanguage.DataTextField = "name";
        ddlLanguage.DataValueField = "id";
        ddlLanguage.DataBind();
    }
    private void BindItems(int gallery)
    {
        Gallery = gallery;
        BindItems();

    }
    private void BindItems()
    {
        DataSet ds = new DataSet();
        string sqlstr = " select * from WidgetImages where galleryid=@galleryid order by priority";
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@galleryid", Gallery);
        ds = getDataset(sq, true);


        this.GV_Images.DataSource = ds.Tables[0];
        this.GV_Images.DataBind();

 
    }
    protected void GV_Main_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditGroup")
        {
            txtNameGrp.Text = "";
            txtLinkGrp.Text = "";

            SwitchView(pnlEditGrp);
            Gallery = Convert.ToInt32(e.CommandArgument.ToString());

            DataTable dt = new DataTable();
            string sqlstr = @" select * from WidgetGallery where id=@id";
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@id", Gallery);

            dt = getTable(sq, true);

            if (dt.Rows.Count > 0)
            {
                txtNameGrp.Text = dt.Rows[0]["name"].ToString();
                txtLinkGrp.Text = dt.Rows[0]["link"].ToString();
                ddlLanguage.SelectedValue = dt.Rows[0]["language"].ToString();
            }

            ddlLanguage.Enabled = false;
        }
        if (e.CommandName == "DelGroup")
        {
            RemoveRecord("delete from WidgetGallery where id=@id; delete from content where control='TestimonialsWidget' and param=@id", Convert.ToInt32(e.CommandArgument.ToString()));

            BindGroups();
        }

    }

    protected void btnCancelGrp_Click(object sender, EventArgs e)
    {
        SwitchView(pnlGroups);

        BindGroups();
    }

    protected void btAddItem_Click(object sender, EventArgs e)
    {
        ClearFields();
        SwitchView(pnlEditItem);

    }

    private void SwitchView(Panel pnl)
    {
        pnlGroups.Visible = false;
        pnlEditGrp.Visible = false;
        pnlItems.Visible = false;
        pnlEditItem.Visible = false;

        pnl.Visible = true;

    }
    protected void GV_Images_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if(e.CommandName == "EditItem")
        {
            txtText.Text = "";
            txtTitle.Text = "";
            txtName.Text = "";
            txtLink.Text = "";

            SwitchView(pnlEditItem);
            Item = Convert.ToInt32(e.CommandArgument.ToString());

            DataTable dt = new DataTable();
            string sqlstr = @" select * from WidgetImages where id=@id";
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@id", Item);

            dt = getTable(sq, true);

            if (dt.Rows.Count > 0)
            {
                imgWD.ImageUrl = ConfigurationManager.AppSettings["WidgetImagesPath"] + dt.Rows[0]["image"].ToString();
                imgWD.AlternateText = tbImgAltText.Text = dt.Rows[0]["alt"].ToString();
                imgWD.Visible = true;

                txtText.Text = dt.Rows[0]["text"].ToString();
                txtTitle.Text = dt.Rows[0]["title"].ToString();
                txtName.Text = dt.Rows[0]["name"].ToString();
                txtLink.Text = dt.Rows[0]["link"].ToString();
                rfvImage.Enabled = false;
            }
        }
        else if(e.CommandName == "DelItem")
        {
            Item = Convert.ToInt32(e.CommandArgument.ToString());

            DataTable dt = new DataTable();
            string sqlstr = @" select * from WidgetImages where id=@id";
            SqlCommand sq = new SqlCommand(sqlstr);
            sq.Parameters.AddWithValue("@id", Item);

            dt = getTable(sq, true);

            if (dt.Rows.Count > 0)
            {
                string filename = dt.Rows[0]["image"].ToString();

                RemoveRecord("delete from WidgetImages where id=@id", Item);
                System.IO.File.Delete(Server.MapPath(ConfigurationManager.AppSettings["WidgetImagesPath"] + filename));
            }
            BindItems();
        }

    }

    private void ClearFields()
    {
        txtNameGrp.Text = "";
        txtLinkGrp.Text = "";
        txtText.Text = "";
        txtTitle.Text = "";
        txtName.Text = "";
        txtLink.Text = "";
        tbImgAltText.Text = "";
        Item = 0;
        rfvImage.Enabled = false;
        imgWD.ImageUrl = "";
        imgWD.Visible = false;
       // Gallery = 0;
    }


    protected void bttn_Cancel_Ggroup_Click(object sender, EventArgs e)
    {
        SwitchView(pnlItems );
        BindItems();
    }


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

        foreach(SqlParameter p in parms)
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


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        SwitchView(pnlItems);

        BindItems();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parms = new List<SqlParameter>();
        string sql = "";

        if (Item > 0)
        {
            //Update
            sql = "update WidgetImages set [image]=isnull(@img, [image]), alt=@alt, text=@text, link=@link, title=@title, name=@name where id=@id";
            parms.Add(new SqlParameter("@id", Item));
        }
        else
        {
            sql = "insert into WidgetImages (GalleryId, [image], alt, text, link, title, name) values(@gallery, @img, @alt, @text, @link, @title, @name)";
            parms.Add(new SqlParameter("@gallery", Gallery));
        }

        if (fuImage.HasFile)
        {
            fuImage.SaveAs(Server.MapPath(ConfigurationManager.AppSettings["WidgetImagesPath"] + fuImage.FileName));
            parms.Add(new SqlParameter("@img", fuImage.FileName));
        }
        else
            parms.Add(new SqlParameter("@img", DBNull.Value));

        parms.Add(new SqlParameter("@alt", tbImgAltText.Text));
        parms.Add(new SqlParameter("@text", txtText.Text));
        parms.Add(new SqlParameter("@title", txtTitle.Text));
        parms.Add(new SqlParameter("@name", txtName.Text));
        
        parms.Add(new SqlParameter("@link", txtLink.Text));

        ProcessRecord(sql, parms, CommandType.Text);

        ClearFields();
        SwitchView(pnlItems);
        BindItems();
    }

    protected void GV_Images_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((DataRowView)e.Row.DataItem != null)
        {
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("btnDelete");
            DataRowView dr = (DataRowView)e.Row.DataItem;
            LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this element?');");

            Literal litHfId = (Literal)e.Row.FindControl("litHfId");
            litHfId.Text = String.Format("<input type='hidden' class='HfId' value='{0}' />", dr["id"].ToString());

        }
    }

    protected void btnSaveGrp_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parms = new List<SqlParameter>();
        string sql = "";

        if(Gallery != 0)
        {
            //Update
            sql = "update WidgetGallery set name=@name, link=@link where id=@id; update Content set name='Testimonials - ' + @name where control='TestimonialsWidget' and param=@id";
            parms.Add(new SqlParameter("@id", Gallery));
        }
        else
        {
            sql = "declare @id int; insert into WidgetGallery (name, type, link, language) values (@name, @type, @link, @lang); select @id=SCOPE_IDENTITY(); insert into Content (name, control, param, language) values('Testimonials - ' + @name, 'TestimonialsWidget', @id, @lang)";
            parms.Add(new SqlParameter("@type", Parameter));
            parms.Add(new SqlParameter("@lang", ddlLanguage.SelectedValue));

        }
        parms.Add(new SqlParameter("@name", txtNameGrp.Text));
        parms.Add(new SqlParameter("@link", txtLinkGrp.Text));
        

        ProcessRecord(sql, parms, CommandType.Text);

        ClearFields();
        SwitchView(pnlGroups);
        BindGroups();
    }

    protected void btnAddGallery_Click(object sender, EventArgs e)
    {
        ClearFields();
        Gallery = 0;
        ddlLanguage.Enabled = true;

        SwitchView(pnlEditGrp);
    }

    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Gallery = Convert.ToInt32(GV_Main.DataKeys[e.NewEditIndex].Values[0]);
        SwitchView(pnlItems);
        BindItems(Gallery);

    }

    protected void btnBackItems_Click(object sender, EventArgs e)
    {
        SwitchView(pnlGroups);
    }

    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((DataRowView)e.Row.DataItem != null)
        {
            ImageButton LB_Delete = (ImageButton)e.Row.FindControl("btnDelete");
            DataRowView dr = (DataRowView)e.Row.DataItem;
            LB_Delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this element?');");
        }
    }
}