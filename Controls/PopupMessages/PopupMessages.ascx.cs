using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

public partial class Controls_PopupMessages_PopupMessages : System.Web.UI.UserControl
{
    #region Properties
    protected string PopupID
    {
        set { ViewState["id"] = value; }
        get { return ViewState["id"].ToString(); }
    }

    protected int iLang
    {
        get { return Convert.ToInt32(Session["Language"]) - 1; }
    }

    protected string Lang
    {
        get
        {
            string[] lang = { "en", "fr" };
            return lang[iLang];
        }
    }
    public int tabs
    {
        set; get;
    }
    #endregion Properties

    public Controls_PopupMessages_PopupMessages()
    {
        PopupID = "1";
    }

    public Controls_PopupMessages_PopupMessages(string a)
    {
        PopupID = a;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["LoggedInID"] != null)
        //{
        //    if (Permissions.Get(int.Parse(Session["LoggedInID"].ToString()), int.Parse(Session["PageID"].ToString())) > 1)
        //    {
        //        //pnlPopupMessage.Attributes.Add("Widget", "popupmessages_" + PopupID);
        //        pnlPMAdmin.Visible = true;
        //    }
        //}

        if (!Page.IsPostBack)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
            {
                SqlDataAdapter dapt = new SqlDataAdapter("PopupMessagesSelect", conn);
                dapt.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dapt.SelectCommand.Parameters.AddWithValue("@id", PopupID);
                DataTable dt = new DataTable();
                dapt.Fill(dt);

                string content = "";
                if (dt.Rows.Count > 0)
                {
                    string pattern = "<a tabindex='1' ";
                    string source = dt.Rows[0]["html"].ToString();
                    string s = source.Replace("<a ", pattern);


                    int count = Regex.Matches(source, "<a ").Count;
                    tabs = count + 1;

                    
                    int npos = s.IndexOf(pattern);
                    for (int i = 1; i < count; i++)
                    {
                        string temp = s.Substring(0, npos + pattern.Length);
                        s = s.Substring(npos + pattern.Length);
                        string newpatter = pattern.Replace("<a tabindex='" + i.ToString() + "'", "<a tabindex='" + (i + 1).ToString() + "'");
                        s = s.Replace(pattern, newpatter);
                        pattern = newpatter;
                        npos = s.IndexOf(pattern);

                        s = temp + s;
                    }

                    content = s;


                    //    //litbtnPopupMessageAdd.Text = "<a href='javascript:void(0)' class='BtnAddPM' title='Edit Popup Message' PMId='" + PopupID + "' PMName='" + dt.Rows[0]["name"] + "'><img src='/images/lemonaid/buttons/plus.png' alt='Edit Popup Message' width='70%' /></a>";
                    //    litbtnPopupMessageAdd.Text = "<a href='javascript:void(0)' class='BtnAddPM' title='Edit Popup Message' PMId='" + PopupID + "'><img src='/images/lemonaid/icons/edit.png' alt='Edit Popup Message' /></a>";

                    //    string script = string.Empty;
                    //    script = "$(document).ready(function () {" + Environment.NewLine;
                    //    script += "     $('.BtnAddPM').click(function (e) {" + Environment.NewLine;
                    //    script += "         if (!$('#pnlPopupMessagesAdd').is(':visible')) {" + Environment.NewLine;
                    //    script += "             $('.hfPMTempFolder').val($(this).attr('PMId'));" + Environment.NewLine;
                    //    //script += "             $find('wmePopupMessagesName').set_Text($(this).attr('PMName'));" + Environment.NewLine;
                    //    script += "             $('.btnFrontPopupMessagesEdit').click();" + Environment.NewLine;
                    //    script += "             $('#pnlPopupMessagesAdd').slideDown(600);" + Environment.NewLine;
                    //    script += "         }" + Environment.NewLine;
                    //    script += "         return false;" + Environment.NewLine;
                    //    script += "     });" + Environment.NewLine;
                    //    script += "});" + Environment.NewLine;

                    //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopupMessage", script, true);


                    if (Convert.ToBoolean(dt.Rows[0]["firsttimeonly"]))
                    {
                        string cookiename = dt.Rows[0]["name"].ToString() + "_firsttimeonly";
                        HttpCookie _userInfoCookies = Request.Cookies[cookiename];

                        if (_userInfoCookies != null)
                        {
                            pnlPopupMessage.Visible = false;
                        }
                        else
                        {
                            pnlPopupMessage.Visible = true;
                            _userInfoCookies = new HttpCookie(cookiename);
                        }


                        _userInfoCookies.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(_userInfoCookies); // overwrite it
                    }
                    else
                    {
                        string cookiename = dt.Rows[0]["name"].ToString() + "_firsttimeonly";
                        HttpCookie _userInfoCookies = Request.Cookies[cookiename];
                        if (_userInfoCookies != null)
                        {
                            _userInfoCookies.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(_userInfoCookies); // overwrite it
                        }
                        pnlPopupMessage.Visible = true;

                    }
                }

                if(pnlPopupMessage.Visible)
                    litContent.Text = content;

            }
        }
    }
}