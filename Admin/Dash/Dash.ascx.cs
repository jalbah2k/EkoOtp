using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Dash_Dash : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select * from users where id=" + Session["LoggedInID"].ToString(), ConfigurationManager.AppSettings["CMServer"]);
            DataSet ds = new DataSet();
            dapt.Fill(ds);

            litName.Text = ds.Tables[0].Rows[0]["name"].ToString();

            sortExp = "timestamp"; // default sorted column
            sortOrder = "desc";    // default sort order

            bindmessages();
            bindcontacts();
        }

		lblUsers.Text = Application["usersonline"].ToString();
    }

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

    public void dosort(object o, GridViewSortEventArgs e)
    {
        sortExp = e.SortExpression;

        if (sortOrder == "desc")
            sortOrder = "asc";
        else
            sortOrder = "desc";

        bindmessages(e.SortExpression, sortOrder);
    }

    private void bindcontacts()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select * from groups order by name select id,username + ' (' + name + ')' as username from users order by username", sqlConn);
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        ddlToGroup.Items.Clear();
        ddlToUser.Items.Clear();

        ddlToGroup.DataSource = ds.Tables[0];
        ddlToGroup.DataBind();
        ddlToUser.DataSource = ds.Tables[1];
        ddlToUser.DataBind();

        ddlToGroup.Items.Insert(0,new ListItem("SINGLE USER", "-1"));
        ddlToGroup.Items.Insert(0,new ListItem("EVERYONE", "0"));
    }

    public void updateddls(object o, EventArgs e)
    {
        if (ddlToGroup.SelectedValue == "-1")
        {
            ddlToUser.Visible = true;
        }
        else
        {
            ddlToUser.Visible = false;
        }

    }

    public void sendmessage(object o, EventArgs e)
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm;

        
        sqlComm = new SqlCommand("insert into messages(fromuser,touser,togroup,subject,message) values(@fromuser,@touser,@togroup,@subject,@message)", sqlConn);
        sqlComm.Parameters.AddWithValue("@fromuser", Session["LoggedInID"].ToString());
        if (ddlToGroup.SelectedValue == "-1")
        {
            sqlComm.Parameters.AddWithValue("@touser", ddlToUser.SelectedValue);
        }
        else
            sqlComm.Parameters.AddWithValue("@touser", "-1");

        sqlComm.Parameters.AddWithValue("@togroup", ddlToGroup.SelectedValue);
        sqlComm.Parameters.AddWithValue("@subject", txtSubject.Text);
        sqlComm.Parameters.AddWithValue("@message", txtMessage.Text);

        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();



        bindmessages();
        backtolist(o, e);
    }

    private void bindmessages()
    {
        bindmessages(sortExp, sortOrder);
    }

    private void bindmessages(string sortExp, string sortDir)
    {
        DataSet ds = new DataSet();
        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select a.id,a.subject,a.timestamp,a.username,isnull(messagestatus.viewed,0) as viewed from (select m.id,m.subject,m.timestamp,u.username from messages m,users u where u.id=m.fromuser and (m.touser=@ID or m.togroup in (select group_id from users_groups_access where user_id=@ID) or m.togroup=0)) a left join messagestatus on a.id=messagestatus.messageid and MessageStatus.userid=@ID order by a.timestamp desc", sqlConn);
            dapt.SelectCommand.Parameters.AddWithValue("@ID", Session["LoggedInID"].ToString());
            dapt.Fill(ds);
        }

        DataView myDataView = new DataView();
        myDataView = ds.Tables[0].DefaultView;

        if (sortExp != string.Empty)
        {
            myDataView.Sort = string.Format("{0} {1}", sortExp, sortDir);
        }

        GV_Main.DataSource = myDataView;
        GV_Main.DataBind();

        pager1.ItemCount = ds.Tables[0].Rows.Count;
        pager1.Visible = myDataView.Count > GV_Main.PageSize;

        litPagerShowing.Text = CMSHelper.GetPagerInfo(GV_Main, myDataView.Count);

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

    protected void GV_Main_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        #region Add sorted class to headers
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int colIndex = CMSHelper.GetColumnIndex((sender as GridView), sortExp);
            e.Row.Cells[colIndex].CssClass = sortOrder == "asc" ? "sortasc" : "sortdesc";
        }
        #endregion Add sorted class to headers

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Image ib = (Image)e.Row.FindControl("imgStatus");
            if (ib.ToolTip == "True" || ib.ToolTip == "1")
                ib.ImageUrl = "/images/icons/mailread.png";

            //for delete button
            ImageButton lb;
            lb = (ImageButton)e.Row.FindControl("LB_Delete");
            lb.Attributes.Add("OnClick", "return confirm('Are you sure to delete this message?');");
        }
    }

    /*protected void GV_Main_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GV_Main.PageIndex = e.NewPageIndex;
        bindmessages();
    }*/
    public void PageSizeChange(object o, EventArgs e)
    {
        this.GV_Main.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pager1.CurrentIndex = 1;
        this.GV_Main.PageIndex = 0;
        bindmessages();
    }

    public void pager_Command(object sender, CommandEventArgs e)
    {
        int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
        pager1.CurrentIndex = currnetPageIndx;
        this.GV_Main.PageIndex = currnetPageIndx - 1;

        //BindRepeater();

        bindmessages();
    }



    protected void GV_Main_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.RowIndex].Value.ToString());

        //Delete
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlCommand sqlComm = new SqlCommand("delete from messages where id=@ID", sqlConn);
        sqlComm.Parameters.AddWithValue("@ID", id.ToString());
        sqlConn.Open();
        sqlComm.ExecuteNonQuery();
        sqlConn.Close();

        bindmessages();

    }

    protected void GV_Main_RowEditing(object sender, GridViewEditEventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = false;
        pnlView.Visible = true;


        //ViewState["action"] = "edit";

        Int32 id = Convert.ToInt32(this.GV_Main.DataKeys[e.NewEditIndex].Value.ToString());
        Session["mailrecord"] = id;

        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]);
        SqlDataAdapter dapt = new SqlDataAdapter("select m.id,m.fromuser,m.subject,m.message,m.timestamp,u.username from messages m,users u where u.id=m.fromuser and m.id=@MID delete from messagestatus where userid=@ID and messageid=@MID insert into messagestatus(userid,messageid,viewed) values(@ID,@MID,1)", sqlConn);
        dapt.SelectCommand.Parameters.AddWithValue("@ID", Session["LoggedInID"].ToString());
        dapt.SelectCommand.Parameters.AddWithValue("@MID", id.ToString());
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        lblFrom.Text = ds.Tables[0].Rows[0]["username"].ToString();
        lblSubject.Text = ds.Tables[0].Rows[0]["subject"].ToString();
        litMessage.Text = ds.Tables[0].Rows[0]["message"].ToString();

        Session["mailfrom"] = ds.Tables[0].Rows[0]["fromuser"].ToString();
    }

    public void gotoreply(object o, EventArgs e)
    {
        newmessage(o, e);

        ddlToGroup.ClearSelection();
        ddlToGroup.Items.FindByValue("-1").Selected = true;
        ddlToUser.Visible = true;
        ddlToUser.ClearSelection();
        ddlToUser.Items.FindByValue(Session["mailfrom"].ToString()).Selected = true;

        txtSubject.Text = "RE: " + lblSubject.Text;
    }

    public void clear()
    {
        
        txtSubject.Text = "";
        txtMessage.Text = "";
    }

    public void backtolist(object sender, EventArgs e)
    {
        pnlList.Visible = true;
        pnlSend.Visible = false;
        pnlView.Visible = false;

        GV_Main.EditIndex = -1;

        bindmessages();
    }

    public void newmessage(object s, EventArgs e)
    {
        pnlList.Visible = false;
        pnlSend.Visible = true;
        pnlView.Visible = false;

        clear();
    }

    public void ExportSearches(object s, EventArgs e)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            string sqlcmd = "SELECT id, name as Keyword, Count, resultscount as 'Last Results Count' from Searches";
            SqlDataAdapter dapt = new SqlDataAdapter(sqlcmd, conn);

            DataTable dt = new DataTable();
            dapt.Fill(dt);

            string attachment = "attachment; filename=Searches.xls";

            Response.ClearContent();

            Response.AddHeader("content-disposition", attachment);

            Response.ContentType = "application/vnd.ms-excel";

            string tab = "";

            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);

                tab = "\t";
            }

            Response.Write("\n");


            //int i;

            foreach (DataRow dr in dt.Rows)
            {
                tab = "";

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + "\"" + dr[i].ToString().Replace("\"", "'") + "\"");

                    tab = "\t";
                }

                Response.Write("\n");
            }

            Response.End();
        }
    }
    
}
