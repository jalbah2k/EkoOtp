using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class CarouselWidget : System.Web.UI.UserControl
{
    public string Gallery;

    public CarouselWidget() { }
    public CarouselWidget(string param) { Gallery = param; }

    public int Top = 10;
    //public int QtyShow = 3;
    //public int QtyItems;

    public string Language
    {
        get
        {
            return CMSHelper.GetCleanQueryString("lang", "1");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindItems();
        }
    }

    private void BindItems()
    {
        DataSet ds = new DataSet();
        string sqlstr = String.Format(" select top {0} *, ROW_NUMBER() OVER( ORDER BY [id] ) Num from WidgetImages where galleryid=@galleryid  select * from WidgetGallery where id=@galleryid", Top.ToString());
        SqlCommand sq = new SqlCommand(sqlstr);
        sq.Parameters.AddWithValue("@galleryid", Gallery);
        ds = getDataset(sq, CommandType.Text);


        this.repItems.DataSource = ds.Tables[0];
        this.repItems.DataBind();
        //QtyItems = ds.Tables[0].Rows.Count;

        if (ds.Tables[1].Rows.Count > 0 && ds.Tables[1].Rows[0]["link"].ToString() != "")
        {
            litViewAll.Text = String.Format("<a href=\"{0}\" class=\"buttonNew\" target=\"{2}\">{1}</a>", ds.Tables[1].Rows[0]["link"].ToString(), (Language == "1" ? "View All" : "Voir tous"), 
                Convert.ToBoolean(ds.Tables[1].Rows[0]["newwindow"]) ? "_blank" : "_self");
        }

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

            HtmlGenericControl div_item = (HtmlGenericControl)e.Item.FindControl("div_item");
            int length = 150;
            int item = Convert.ToInt32(drv["num"]);
            if (drv["num"].ToString() == "1")
                div_item.Attributes.Add("class", "item active");
            else
                div_item.Attributes.Add("class", "item");


            if(drv["text"].ToString().Length > 0)
            {
                Literal litText = (Literal)e.Item.FindControl("litText");
                litText.Text = "<p>" + (drv["text"].ToString().Length > length ? drv["text"].ToString().Substring(0, length) + "..." : drv["text"].ToString()) + "</p>";
            }
            
            Literal litReadmore = (Literal)e.Item.FindControl("litReadmore");
            litReadmore.Text = Language == "1" ? "read more" : "Lire la suite";
   
        }
    }
}