using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class eFormPopup : System.Web.UI.UserControl
{
    public string param;

    public eFormPopup() { param = ""; }
    public eFormPopup(string a) { param = a; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (param == "")
            return;

        bool show = false;

        //if (param != "1119")
            show = true;
        //else
        //{
        //    show = IsDiplayed();
        //}

        if (Request.QueryString["th"] == null && Request.QueryString["th"] != "1")
        {
            if (show)
            {
                UserControl userControl = LoadControlExtension.LoadControl(this, "~/Controls/eform/eform.ascx", param);
                ((Controls_eForm_eForm)userControl).afterSubmit += new AfterSubmit(GotoClose);
                PlaceHolder1.Controls.Add(userControl);

                if (!IsPostBack)
                {


                    string script = string.Empty;
                    script = "$(document).ready(function () {" + Environment.NewLine;
                    script += "     $('.BtnAddPM').click(function (e) {" + Environment.NewLine;
                    script += "         if (!$('#pnlPopupMessagesAdd').is(':visible')) {" + Environment.NewLine;
                    script += "             $('.hfPMTempFolder').val($(this).attr('PMId'));" + Environment.NewLine;
                    //script += "             $find('wmePopupMessagesName').set_Text($(this).attr('PMName'));" + Environment.NewLine;
                    script += "             $('.btnFrontPopupMessagesEdit').click();" + Environment.NewLine;
                    script += "             $('#pnlPopupMessagesAdd').slideDown(600);" + Environment.NewLine;
                    script += "         }" + Environment.NewLine;
                    script += "         return false;" + Environment.NewLine;
                    script += "     });" + Environment.NewLine;
                    script += "});" + Environment.NewLine;

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopupMessage", script, true);
                }
            }
            else
                pnlPopupMessage.Visible = false;

        }
        else
            pnlPopupMessage.Visible = false;

    }

    private bool IsDiplayed()
    {
        bool bret = false;
        if (Session["LoggedInId"] != null)
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select * from eFormSubmissions where formid=@formid and userid=@userid", 
                ConfigurationManager.AppSettings["CMServer"].ToString());

            dapt.SelectCommand.Parameters.AddWithValue("@formid", param);
            dapt.SelectCommand.Parameters.AddWithValue("@userid", Session["LoggedInId"].ToString());

            DataSet ds = new DataSet();
            dapt.Fill(ds);

            bret = ds.Tables[0].Rows.Count == 0;
        }
        return bret;
    }

    protected void GotoClose()
    {
        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopupMessage", "$('#PopupMessage').animate({ 'width': '0%', 'height': '0%' }, 300, 'easeInBack', function () { $('#PopupMessage').hide(); })", true);
        pnlPopupMessage.Visible = false;
    }
}