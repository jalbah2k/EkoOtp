using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Languages_Languages : System.Web.UI.UserControl
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer"));

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindLanguages();
    }

    private void BindLanguages()
    {
        SqlDataAdapter dapt = new SqlDataAdapter("select *, case enabled when 0 then 'false' else 'check' end as StatusImg, case enabled when 0 then 'Disabled' else 'Enabled' end as status from Languages", conn);
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        gvLanguages.DataSource = dt;
        gvLanguages.DataBind();
    }


    public void rowcommand(object o, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "deletelanguage":
                DeleteRow(e.CommandArgument.ToString());
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                break;
            case "editlanguage":
                Edit(e.CommandArgument.ToString());
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                break;
            case "EnableLang":
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
                {
                    SqlCommand cmd = new SqlCommand("update Languages set enabled = 1 - enabled where id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", e.CommandArgument.ToString());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                BindLanguages();
                RouteConfig.RegisterRoutes(RouteTable.Routes);

                break;
            default:
                break;
        }
    }
    public void rowbound(object o, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            ImageButton btnStatus = (ImageButton)e.Row.FindControl("btnStatus");
            btnStatus.Enabled = drv["id"].ToString() != "1";

            //ImageButton lb = (ImageButton)e.Row.FindControl("ibDelete");
            //lb.Attributes.Add("OnClick", "return confirm('Are you sure you wish to delete this language?');");
        }
    }


    public void DeleteRow(string id)
    {
        SqlCommand command = new SqlCommand("delete from Languages where id=@id", conn);
        command.Parameters.AddWithValue("@id", id);

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();

        BindLanguages();
    }

    private void Edit(string id)
    {
        pnlEdit.Visible = true;
        pnlList.Visible = false;

        SqlDataAdapter dapt = new SqlDataAdapter("select * from Languages where id=@id", conn);
        dapt.SelectCommand.Parameters.AddWithValue("@id", id);
        DataTable dt = new DataTable();
        dapt.Fill(dt);

        txtID.Text = dt.Rows[0]["id"].ToString();
        txtName.Text = dt.Rows[0]["name"].ToString();
        txtLocale.Text = dt.Rows[0]["locale"].ToString();
        txtPrefix.Text = dt.Rows[0]["prefix"].ToString();

        ibSave.CommandName = "save";
        ibSave.CommandArgument = id;
    }

    public void Add(object o, EventArgs e)
    {
        pnlEdit.Visible = true;
        pnlList.Visible = false;
        ibSave.CommandName = "add";

        txtID.Text = "";
        txtName.Text = "";
        txtLocale.Text = "";
        txtPrefix.Text = "";
    }

    public void SAVE(object o, CommandEventArgs e)
    {
        pnlEdit.Visible = false;
        pnlList.Visible = true;

        SqlCommand command = new SqlCommand();

        if (e.CommandName == "save")
        {
            command = new SqlCommand("update Languages set id=@id, name=@name, locale=@locale, prefix=@prefix where id=@idOld", conn);
            command.Parameters.AddWithValue("@idOld", e.CommandArgument.ToString());
            //command.Parameters.AddWithValue("@id", txtID.Text.Trim());
        }
        else
        {
            command = new SqlCommand("insert into Languages(id, name, locale, prefix) values(@id, @name, @locale, @prefix)", conn);
        }
        command.Parameters.AddWithValue("@id", txtID.Text.Trim());
        command.Parameters.AddWithValue("@name", txtName.Text.Trim());
        command.Parameters.AddWithValue("@locale", txtLocale.Text.Trim());
        command.Parameters.AddWithValue("@prefix", txtPrefix.Text.Trim());

        conn.Open();
        command.ExecuteNonQuery();
        conn.Close();

        BindLanguages();
    }
    public void ibCancel_Click(object o, EventArgs e)
    {
        pnlEdit.Visible = false;
        pnlList.Visible = true;

        BindLanguages();
    }
}