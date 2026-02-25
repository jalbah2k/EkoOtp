using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestimonialsWidget : System.Web.UI.UserControl
{
    public string Gallery;
    public TestimonialsWidget() { }
    public TestimonialsWidget(string p) { Gallery = p; }


    public int Top = 1000;
 
    public string Language
    {
        get
        {
            return "1";             // CMSHelper.GetCleanQueryString("lang", "1");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindItems();
        }
    }

    private void BindItems()
    {
        DataSet ds = new DataSet();
        string sqlstr = String.Format(" select top {0} *, ROW_NUMBER() OVER( ORDER BY [id] ) Num from WidgetImages where galleryid=@galleryid order by priority", Top.ToString());
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@galleryid", Gallery);
        ds = getDataset(sq, CommandType.Text);


        this.repItems.DataSource = ds.Tables[0];
        this.repItems.DataBind();

    }

    protected string _connection = ConfigurationManager.AppSettings.Get("CMServer");
    private DataSet getDataset(SqlCommand cmd, CommandType type)
    {
        DataSet dt = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandType = type;
        da.SelectCommand.Connection = new SqlConnection(_connection);
        da.Fill(dt);
        return dt;
    }

    protected void repItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((DataRowView)e.Item.DataItem != null)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            #region Photo
            string filename = "placeholder.png";

            if (drv["image"].ToString() != "")
            {
                if (File.Exists(Server.MapPath(ConfigurationManager.AppSettings["WidgetImagesPath"] + drv["image"].ToString())))
                {
                    filename = drv["image"].ToString();
                }
            }

            Image imgPhoto = (Image)e.Item.FindControl("imgPhoto");
            imgPhoto.ImageUrl = ConfigurationManager.AppSettings["WidgetImagesPath"] + filename;

            if (drv["alt"].ToString() != "")
                imgPhoto.AlternateText = drv["alt"].ToString();
            else
                imgPhoto.Attributes.Add("alt", "");

            #endregion

            #region Content
            Literal litContent = (Literal)e.Item.FindControl("litContent");

            if (drv["name"].ToString() != "")
                litContent.Text = "<h2>" + drv["name"].ToString() + "</h2>";

            if (drv["text"].ToString() != "")
                litContent.Text += "<p>" + drv["text"].ToString().Replace(Environment.NewLine, "<br>") + "</p>";

            #endregion

        }
    }
}