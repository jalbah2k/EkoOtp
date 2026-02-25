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
using System.IO;
using CuteEditor;

public partial class WeeklyRoundup : System.Web.UI.UserControl
{
    public string strConnectionString = ConfigurationManager.AppSettings["CMServer"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindWeeks();
            BindEditor(Editor1, 300);
            PopulateIntro();
        }
    }

    private void BindWeeks()
    {
        string commandString = "select * from WeeklyRoundup order by id desc";
        DataTable dt = new DataTable();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(dt);
        }

        ddlWeeks.DataSource = dt;
        ddlWeeks.DataBind();
    }

    private void BindEditor(Editor editor, int height)
    {
        string TemplateItemList = "FormatBlock,|,PasteText,|,Bold,Italic,Underline,|,InsertLink";
        editor.TemplateItemList = TemplateItemList;
        editor.ResizeMode = CuteEditor.EditorResizeMode.None;
        editor.Height = height; // 200;

        editor.EmptyAlternateText = EmptyAlternateText.ForceAdd;
        editor.EnableStripScriptTags = false;
        editor.EnableStripIframeTags = false;
        editor.BreakElement = BreakElement.P;

        editor.EditorWysiwygModeCss = string.Format(ConfigurationManager.AppSettings.Get("EditorCss") + "?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        editor.PreviewModeCss = string.Format(ConfigurationManager.AppSettings.Get("EditorCss") + "?v={0},{1}", ConfigurationManager.AppSettings.Get("CSSVersion"), ConfigurationManager.AppSettings.Get("FontsUrl"));
        editor.EditorBodyStyle = "background-color:#FFFFFF; background-image:none; width:auto; height:auto;";


        CuteEditor.ToolControl toolctrl = Editor1.ToolControls["FormatBlock"];
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
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand("update WeeklyRoundup set Intro =@intro where id=@id", connection);
            cmd.Parameters.Add(new SqlParameter("@id", ddlWeeks.SelectedValue));
            cmd.Parameters.Add(new SqlParameter("@intro", Editor1.Text));
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    protected void ddlWeeks_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateIntro();
    }

    private void PopulateIntro()
    {
        string commandString = "select * from WeeklyRoundup where id=@id";
        DataTable dt = new DataTable();

        using (SqlConnection connection = new SqlConnection(strConnectionString))
        {
            SqlCommand cmd = new SqlCommand(commandString, connection);
            cmd.Parameters.Add(new SqlParameter("@id", ddlWeeks.SelectedValue));
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                Editor1.Text = dt.Rows[0]["Intro"].ToString();
            }
        }

        if(ddlWeeks.SelectedIndex == 0)
        {
            Editor1.Enabled = true;
            btnSave.Enabled = true;
            btnSave.CssClass = "admin-button-green btn-enabled";
        }
        else
        {
            Editor1.Enabled = false;
            btnSave.Enabled = false;
            btnSave.CssClass = "admin-button-green btn-disabled";
        }
    }
}
