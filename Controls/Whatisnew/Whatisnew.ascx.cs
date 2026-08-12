using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Whatisnew : System.Web.UI.UserControl
{
    public int records = 6;
    public bool bLoadMore = false;

    public Whatisnew() { }

    public Whatisnew(string param) { }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        int userid = 0;

        if (Session["LoggedInID"] == null ||
            !int.TryParse(Session["LoggedInID"].ToString(), out userid))
        {
            return;
        }


        //get yaf_LogLogins and take CreatedOn  
        DateTime logindate = DateTime.Now;
        DataTable dt = WhatisNewHelper.LoadPage(1, records, userid);

        bLoadMore = dt.Rows.Count >= records;
        if (bLoadMore)
        {
            dt.Rows.Remove(dt.Rows[records - 1]);
        }


        if (dt.Rows.Count == 0)
        {
            //this.Visible = false;
            // return;

            dt = WhatisNewHelper.LoadPage(1, records - 1, userid, true);
        }

        Repeater1.DataSource = dt;
        Repeater1.DataBind();


        if (bLoadMore)
        {
            try
            {
                ((_Default)this.Page).InjectContent("Scripts",
                    "<script src=\"/Controls/Whatisnew/LoadWhatisNew.js\"></script>");
            }
            catch
            {
                Page.ClientScript.RegisterClientScriptInclude(
                    "LoadWhatisNew",
                    "/Controls/Whatisnew/LoadWhatisNew.js");
            }
        }
    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item &&
            e.Item.ItemType != ListItemType.AlternatingItem)
        {
            return;
        }

        DataRowView dr = (DataRowView)e.Item.DataItem;

        string type = Convert.ToString(dr["Type"]);
        string id = Convert.ToString(dr["Id"]);
        string title = Convert.ToString(dr["Title"]);

        Literal litType = (Literal)e.Item.FindControl("litType");
        Literal litTitle = (Literal)e.Item.FindControl("litTitle");
        Literal litDate = (Literal)e.Item.FindControl("litDate");
        Literal litAction = (Literal)e.Item.FindControl("litAction");
        HtmlAnchor theLink = (HtmlAnchor)e.Item.FindControl("theLink");

        litType.Text = HttpUtility.HtmlEncode(type);
        litTitle.Text = HttpUtility.HtmlEncode(title);

        if (dr["ActivityDate"] != DBNull.Value)
        {
            DateTime activityDate = Convert.ToDateTime(dr["ActivityDate"], CultureInfo.InvariantCulture);
            litDate.Text = activityDate.ToString("MMM dd", CultureInfo.InvariantCulture);
        }

        string actionText;
        string url;
        WhatisNewHelper.GetAction(type, id, out actionText, out url);

        litAction.Text = actionText;
        theLink.HRef = url;
        theLink.Attributes["title"] = title;
    }
}
