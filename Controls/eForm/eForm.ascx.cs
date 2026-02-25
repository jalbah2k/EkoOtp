#define TEXT_ANGULAR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using AjaxControlToolkit;
using System.IO;
using System.Web.UI.HtmlControls;
using CuteEditor;
using System.Text.RegularExpressions;
using Recaptcha.Web;
using skmValidators;

public delegate void AfterSubmit();

public partial class Controls_eForm_eForm : System.Web.UI.UserControl
{
    public string param;
    private DataSet ds;
    private string sTitle;
    private string sIntro;
    private string email;
    private bool notify;
    private string validgroup = "eForm";
    protected string myTerms;
    //private string captchaPublicKey = "6LeRUQYAAAAAAE_WfWemtlRWO1Oz6soZ9SA_ySbs";
    //private string captchaPrivateKey = "6LeRUQYAAAAAAInoMAa_jnDtaHxDzs844OpHWs6W";
    //reCaptcha v2 keys
    private string captchaPublicKey = "6Lf0gMoSAAAAAGMeDpnjHwSLy2Utp8c5dL6aP0VY";
    private string captchaPrivateKey = "6Lf0gMoSAAAAAGRZGBtePdzHeQ6pYl62bfs3NNfV";

    protected int MAX_FILE_SIZE = 4096000;          // in bytes
    protected int MAX_FOLDER_SIZE = 102400000;      // in bytes
    //protected int MAX_FOLDER_SIZE = 5342895;      // in bytes
    protected string eFormsUploadsPath = "/data/eFormsUploads/";

    protected string _clientIDPrefix = "";
    protected string _rowTemplateFullWidth = "<div class=\"twelve columns\">{0}</div>";
    protected string _rowTemplateTwoColumns = "<div class=\"six columns\">{0}</div><div class=\"six columns\">{1}</div>";

    protected string _confirmationMsg = "";
    public AfterSubmit afterSubmit;
    private bool Popup;

    protected Dictionary<string, string> dict;
    protected string formID
    {
        set { ViewState["id"] = value; }
        get { return ViewState["id"].ToString(); }
    }

    public bool captcha
    {
        set { ViewState["captcha"] = value; }
        get { return ViewState["captcha"] == null ? false : (bool)ViewState["captcha"]; }
    }

    protected int Language
    {
        get
        {
            //return Convert.ToInt32(CMSHelper.GetCleanQueryString("lang", "1"));
            return Convert.ToInt32(CMSHelper.GetLanguageNumber());
        }
    }

    protected string rowTemplate
    {
        set { ViewState["rowTemplate"] = value; }
        get { return ViewState["rowTemplate"] != null ? ViewState["rowTemplate"].ToString() : _rowTemplateFullWidth; }
    }

    public Controls_eForm_eForm()
    {
    }

    public Controls_eForm_eForm(string p)
    {
        param = p;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (param == null)
            param = "1";
        formID = param;
        
        ScriptManager sm = ScriptManager.GetCurrent(this.Page);
        if (sm == null)
        {
            sm = new ScriptManager();
            sm.EnablePartialRendering = true;
            if (File.Exists(Server.MapPath("/js/webkit.js")))
                sm.Scripts.Add(new ScriptReference("/js/webkit.js"));
            this.pnEForm.Controls.Add(sm);
        }

        CretateDictionary();
        BindEForm();

        //if (!IsPostBack)
        //      {
        //          if (Session["eFormSubmitted"] != null && (bool)Session["eFormSubmitted"])
        //          {
        //              Session.Remove("eFormSubmitted");

        //              pnEForm.Controls.Clear();
        //              Panel pnl = new Panel();
        //              Label lbl = new Label();
        //              string ThankYouMsg = ds.Tables[0].Rows[0]["confirmation"].ToString();

        //              int npos = ThankYouMsg.IndexOf("<http");
        //              while (npos > -1)
        //              {
        //                  int npos2 = ThankYouMsg.IndexOf(">", npos);
        //                  string url = ThankYouMsg.Substring(npos, npos2 - npos + 1);
        //                  ThankYouMsg = ThankYouMsg.Replace(url, string.Format("<a href='{0}' target='_blank'>{0}</a>", url.TrimStart('<').TrimEnd('>')));

        //                  npos = ThankYouMsg.IndexOf("<http", ThankYouMsg.IndexOf("</a>", npos));
        //              }

        //              lbl.Text = "<h2>" + sTitle + "</h2>";

        //              lbl.CssClass = "subtitle";
        //              if (ThankYouMsg.Length == 0)
        //                  lbl.Text += Language == 1 ? "Form has been successfully submitted" : "Formulaire a été soumis avec succès";
        //              else
        //                  lbl.Text += ThankYouMsg.Replace("\n", "<br />").Replace("\r", "");
        //              pnl.Controls.Add(lbl);
        //              pnl.Attributes.Add("style", "margin:20px 0 10px 20px");
        //              pnEForm.Controls.Add(pnl);

        //              return;
        //          }

        //          Button btnSave = (Button)this.FindControl("btSubmit" + formID);
        //          BuildClickOnceButton(btnSave, divSubmitted);
        //      }
    }

    private void BindEForm()
    {
        SqlParameter[] prms = { new SqlParameter("@id", formID) };
        ds = getTables(sqlGet, prms);

        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return;

        DataRow rwForm = ds.Tables[0].Rows[0];

        sTitle = rwForm["Title"].ToString();
        sIntro = rwForm["Intro"].ToString();
        email = rwForm["email"].ToString();
        notify = Convert.ToBoolean(rwForm["sendemail"]);
        captcha = Convert.ToBoolean(rwForm["captcha"]);
        Popup = Convert.ToBoolean(rwForm["Popup"]);

        rowTemplate = _rowTemplateFullWidth;
        
        //Form's Header

        // Add Title
        Literal litTitle = new Literal();
        if (sTitle != "")
        {
            if (param == "1119" && Session["LoggedInId"] != null)
            {
                string sMemberName = GetMemberName(Session["LoggedInId"].ToString());
                    
                //JUAN - THIS IS ONLY FOR THE ONE SURVEY:
                litTitle.Text = string.Format(_rowTemplateFullWidth, 
                                                string.Format(@"<h2>{0}</h2><p class='bigP'>{1}, 
                                                    we value your insights and invite you to share your feedback through this brief survey to help us enhance your experience on our website.</p>",
                                                sTitle, sMemberName));
            }
            else if (param == "2129" && Session["LoggedInId"] != null)
            {
                string sMemberName = GetMemberName(Session["LoggedInId"].ToString());

                //JUAN - THIS IS ONLY FOR THE ONE SURVEY:
                litTitle.Text = string.Format(_rowTemplateFullWidth,
                                                string.Format(@"<h2>{0}</h2><p class='bigP'>
                                                Thanks for visiting the EKO Members’ Portal. We’re always looking for ways to improve these pages for you. This quick, two-minute (at the most!) survey will help us understand your experience. All responses are anonymous. Once you’ve responded, you won’t see these questions again.</p>",
                                                sTitle, sMemberName));
            }
            else
            {
                //JUAN - THIS IS ONLY FOR ALL OTHERS:
                litTitle.Text = string.Format(_rowTemplateFullWidth, "<h2>" + sTitle + "</h2>");
            }
            
            litTitle.ID = "pnlTitle";

            pnEForm.Controls.Add(litTitle);
        }

        if (sIntro.Length > 0)
        {
            Literal litSubTitle = new Literal();
            litSubTitle.Text = string.Format(_rowTemplateFullWidth, "<h3>" + sIntro + "</h3>");
            litSubTitle.ID = "spSubtitle";

            pnEForm.Controls.Add(litSubTitle);
        }

        _clientIDPrefix = pnEForm.ClientID.Replace("pnEForm", "");
        string _FieldClientID = string.Empty;
        string script = string.Empty;

        //Form's Body
        DataRowCollection rwFields = ds.Tables[1].Rows;

        foreach (DataRow rw in rwFields)
        {
            if (!Convert.ToBoolean(rw["active"]))
                continue;

            switch (rw["FieldType"].ToString())
            {
                //predefined
                case "eMail":
                case "Email":
                case "email":
                    vEMail(rw, ref pnEForm);
                    break;
                case "Name":
                    vTextBox(rw, ref pnEForm);
                    break;
                case "FullName":
                    vFullName(rw, ref pnEForm);
                    break;
                case "Address":
                    vAddress(rw, ref pnEForm);
                    break;
                case "Phone":
                    vPhone(rw, ref pnEForm);
                    break;
                case "Website":
                    vWebsite(rw, ref pnEForm);
                    break;
                case "Date":
                    vDate(rw, ref pnEForm);
                    break;
                //Modified to add new Datetime field
                case "DateTime":
                    vDateTime(rw, ref pnEForm);
                    break;
                case "DateTimeRange":
                    vDateTimeRange(rw, ref pnEForm);
                    break;
                case "City":
                    vTextBox(rw, ref pnEForm);
                    break;
                case "Country":
                    vCountry(rw, ref pnEForm);
                    break;
                case "Province":
                    vProvince(rw, ref pnEForm);
                    break;
                case "State":
                    vState(rw, ref pnEForm);
                    break;
                case "Salutation":
                    vSalutation(rw, ref pnEForm);
                    break;
                case "Comment":
                    vTextArea(rw, ref pnEForm);
                    break;
                case "Postal":
                    vTextBox(rw, ref pnEForm);
                    break;
                case "Terms":
                    vTerms(rw, ref pnEForm);
                    break;
                case "Browse":
                    vBrowse(rw, ref pnEForm);
                    break;

                case "Image":
                case "Document":
                    vBrowse(rw, ref pnEForm, rw["FieldType"].ToString());
                    break;

                //custom controls
                case "TextBox":
                    vTextBox(rw, ref pnEForm);
                    break;
                case "TextArea":
                    vTextArea(rw, ref pnEForm);
                    break;
                case "HtmlEditor":
                    vTextArea(rw, ref pnEForm);
                    //vEditor(rw, ref pnEForm);
                    break;
                case "Title":
                    vTitle(rw, ref pnEForm);
                    break;
                case "SubTitle":
                    vSubTitle(rw, ref pnEForm);
                    break;
                case "Label":
                    vLabel(rw, ref pnEForm);
                    break;
                case "Password":
                    vTextPassword(rw, ref pnEForm);
                    break;
                case "DropDown":
                    vDropDown(rw, ref pnEForm);
                    break;
                case "DropDownValue":
                    vDropDownValue(rw, ref pnEForm);
                    break;
                case "CheckBox":
                    vCheckBox(rw, ref pnEForm);
                    break;
                case "RadioButton":
                    //vRadioButton(rw, ref pnEForm);
                    vRadioButtonList(rw, ref pnEForm);
                    break;
                case "RadioButtonByValue":
                    vRadioButtonList(rw, ref pnEForm, "id");
                    break;

                case "HorizontalLine":
                    vHorizontalLine(rw, ref pnEForm);
                    break;
                case "BlankLine":
                    vBlankLine(rw, ref pnEForm);
                    break;
                case "Numeric":
                    vNumeric(rw, ref pnEForm);
                    break;
                case "ListBox":
                    vListBox(rw, ref pnEForm);
                    break;
                case "CheckboxList":
                    //vRadioButton(rw, ref pnEForm);
                    vCheckboxList(rw, ref pnEForm);
                    break;
            }

            //Logic
            #region Logic
            _FieldClientID = _clientIDPrefix + "fld" + rw["id"].ToString();
            DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString() + " and optionText='Field_Logic'");
            if (rws.Length > 0)
            {
                string[] _logicElements = rws[0]["optionValue"].ToString().Split('|');
                if (_logicElements != null && _logicElements.Length > 0)
                {
                    string[] _logic = null;
                    string _fieldid = string.Empty;
                    string _operator = string.Empty;
                    string _value = string.Empty;
                    string _fieldtype = string.Empty;
                    string _rule = string.Empty;

                    string _lfieldClientID = string.Empty;
                    string _lfieldClientName = string.Empty;
                    string _OnChangeFnName = string.Empty;

                    string _show = _logicElements[0];
                    string _logicOperator = _logicElements[1].ToLower() == "and" ? " && " : " || ";

                    _rule += _show == "show" ? "!(" : "(";

                    for (int i = 2; i < _logicElements.Length; i++)
                    {
                        _logic = _logicElements[i].Split('^');
                        _fieldid = _logic[0];
                        _operator = _logic[1];
                        _value = _logic[2];
                        _fieldtype = _logic[3].ToLower();

                        _lfieldClientID = _clientIDPrefix + _fieldid;
                        _lfieldClientName = _clientIDPrefix.Replace("_", "$") + _fieldid;

                        if (_fieldtype == "radiobutton")
                            _lfieldClientName = "ctl00$" + _lfieldClientName;


                        //Response.Write(_lfieldClientID + "<br />");
                        //Response.Write(_value);

                        if (_fieldtype == "checkbox")
                            _rule += (_operator == "==" ? "" : "!") + "$('#" + _lfieldClientID + "').is(':checked')";
                        else if (_fieldtype == "checkboxlist")
                        {
                            CheckBoxList cbl = (CheckBoxList)FindControl(_fieldid);
                            List<ListItem> selected = new List<ListItem>();
                            int j = 0;
                            foreach (ListItem item in cbl.Items)
                            {
                                //Response.Write(item.Value + " : " + _value + "<br>");
                                if (item.Value == _value)
                                {
                                    // Response.Write(_operator + "");
                                    _rule += ((_operator == "==" || _operator == "is") ? "" : "!") + "$('#" + _lfieldClientID + "_" + j.ToString() + "').is(':checked')";

                                }
                                j++;
                            }
                        }
                        else if (_fieldtype == "radiobutton")
                        {
                            //_rule += "$('input[name=\"" + _lfieldClientName + "\"]:checked').val() " + _operator + " '" + _value + "'";
                            _rule += "$('input[name=\"" + _lfieldClientName + "\"]:checked').val() == '" + _value + "'";
                        }
                        else if (_fieldtype == "dropdown")
                        {
                            _rule += "$('#" + _lfieldClientID + "').val() == '" + _value + "'";
                        }
                        else
                            _rule += "$('#" + _lfieldClientID + "').val() " + _operator + " '" + _value + "'";

                        if (i < _logicElements.Length - 1)
                            _rule += _logicOperator;

                        _OnChangeFnName += _fieldid;

                        if (_fieldtype == "radiobutton")
                            script += "\n   $('input[name=\"" + _lfieldClientName + "\"]:radio').change(function () {";
                        else
                            script += "\n   $('#" + _lfieldClientID + "').change(function () {";
                        script += "\n       ##OnChangeFnName##();";
                        script += "\n   });";
                    }
                    //_OnChangeFnName += "OnChange";
                    _OnChangeFnName += "OnChangeOver" + "fld" + rw["id"].ToString();
                    _rule += ")";
                    // Response.Write(_rule);

                    script = script.Replace("##OnChangeFnName##", _OnChangeFnName);

                    script += string.Format("\n   var {0} = function() {{", _OnChangeFnName);
                    script += string.Format("\n       var isNotVisible = {0};", _rule);
                    // script += "\n       alert(isNotVisible);";
                    
                    if (rw["FieldType"].ToString().ToLower() == "radiobutton")
                        script += string.Format("\n       $('#{0}').parent().parent().parent().toggleClass('hide', isNotVisible);", _FieldClientID);
                    else
                        script += string.Format("\n       $('#{0}').parent().parent().toggleClass('hide', isNotVisible);", _FieldClientID);

                    script += "\n       if (isNotVisible) {";
                    /*switch (rw["FieldType"].ToString().ToLower())
                    {
                        case "phone":
                            //script += string.Format("\n       $('#{0}p1').val('');", _FieldClientID);
                            script += string.Format("\n       $('#{0}').val('');", _FieldClientID);
                            script += string.Format("\n       $('#{0}p2').val('');", _FieldClientID);
                            script += string.Format("\n       $('#{0}p3').val('');", _FieldClientID);
                            script += string.Format("\n       $('#{0}p4').val('');", _FieldClientID);
                            break;
                        case "dropdown":
                        case "province":
                        case "country":
                        case "salutation":
                        case "state":
                            script += string.Format("\n       $('#{0}').removeAttr('selected').find('option:first').attr('selected', 'selected');", _FieldClientID);
                            break;
                        case "checkbox":
                        case "terms":
                            script += string.Format("\n       $('#{0}').removeAttr('checked');", _FieldClientID);
                            break;
                        case "radiobutton":
                            script += string.Format("\n       $('input[name=\"{0}\"]:radio').attr('checked', false);", _FieldClientID.Replace("_", "$"));
                            break;
                        default:
                            script += string.Format("\n       $('#{0}').val('');", _FieldClientID);
                            break;
                    }*/
                    //script += string.Format("\n       $('#{0}').parent().wrapInner('<form>').closest('form').get(0).reset();", _FieldClientID);
                    script += string.Format("\n         $('#{0}').parent().wrapInner('<form>').children('form').trigger('reset');", _FieldClientID);
                    script += string.Format("\n         $('#{0}').unwrap();", _FieldClientID);
                    script += "\n       }";
                    if (rw["aspType"].ToString().ToLower() == "checkbox")
                        _FieldClientID = _FieldClientID.Replace("fld", "fldV");
                    script += "\n       try {";
                    script += string.Format("\n         if ({0}_rfv != null && {0}_rfv != 'undefined') {{", _FieldClientID);
                    script += string.Format("\n             {0}_rfv.enabled = !isNotVisible;", _FieldClientID);
                    script += string.Format("\n             ValidatorUpdateDisplay({0}_rfv);", _FieldClientID);
                    script += string.Format("\n             $('#{0}_rfv_hf').val(!isNotVisible);", _FieldClientID);
                    script += "\n           }";
                    script += "\n       }";
                    script += "\n       catch (err) {}";
                    script += "\n   }";
                    script += string.Format("\n   {0}();", _OnChangeFnName);
                }
            }
            #endregion Logic
        }

        //pnEForm.Controls.Add(tbl);

        if (!string.IsNullOrEmpty(script))
            Page.ClientScript.RegisterStartupScript(GetType(), "logic", string.Format("\n$(document).ready(function () {{{0}\n}});\n", script), true);

        //Form's Footer

        if (captcha)
        {
            HtmlGenericControl divRow = new HtmlGenericControl("div");
            divRow.Attributes.Add("class", "twelve columns");

            Panel pnlCaptcha = new Panel();
            pnlCaptcha.ID = "pnlCaptcha";
            Recaptcha.Web.UI.Controls.Recaptcha rc = new Recaptcha.Web.UI.Controls.Recaptcha();
            rc.ID = "reCaptcha";
            rc.PublicKey = captchaPublicKey;
            rc.PrivateKey = captchaPrivateKey;
            rc.ApiVersion = "2";
            rc.UseSsl = SslBehavior.SameAsRequestUrl;
            rc.DataType = RecaptchaDataType.Image;
            rc.DataSize = RecaptchaDataSize.Normal;
            rc.Theme = RecaptchaTheme.White;
            if (Language == 2)
                rc.Language = "fr";

            pnlCaptcha.Controls.Add(rc);
            divRow.Controls.Add(pnlCaptcha);

            pnEForm.Controls.Add(divRow);
        }

        Panel pnlFooter = new Panel();
        pnlFooter.CssClass = "twelve columns efFooter";

        Button btn = new Button();
        btn.CssClass = "efButton button-primary";
        btn.Text = Translate("Submit");
        btn.ToolTip = sTitle;
        btn.ID = "btSubmit" + formID;
        btn.Click += new System.EventHandler(btSubmit_Click);
        btn.ValidationGroup = validgroup + formID;
        //      btn.Attributes.Add("onclick", "return ValidateForm();");
        if (Request.QueryString["previewid"] != null)
            btn.Enabled = false;

        pnlFooter.Controls.Add(btn);

        ValidationSummary vs = new ValidationSummary();
        vs.ID = "MyValidationSummary" + formID;
        vs.ValidationGroup = validgroup + formID;
     
        if (formID == "1119")
            vs.HeaderText = "Please fill out all required fields";

        vs.ShowMessageBox = true;
        vs.ShowSummary = false;
        pnlFooter.Controls.Add(vs);

        pnEForm.Controls.Add(pnlFooter);


        if (Popup)
            _confirmationMsg = rwForm["confirmation"].ToString().Replace("'", "’").Replace(Environment.NewLine, "\\n");  //.Replace(Environment.NewLine, "<br />");
        else
        {
            if (Session["eFormSubmitted"] != null && (bool)Session["eFormSubmitted"])
            {
                Session.Remove("eFormSubmitted");

                string confirmationMsg = rwForm["confirmation"].ToString().Replace("'", "’").Replace(Environment.NewLine, "\\n");  //.Replace(Environment.NewLine, "<br />");
                if (confirmationMsg.Length == 0)
                    confirmationMsg = Language == 1 ? "Form has been successfully submitted" : "Formulaire a été soumis avec succès";

                Page.ClientScript.RegisterStartupScript(GetType(), "msg", "alert('" + confirmationMsg + "');", true);
            }
        }

        BuildClickOnceButton(btn, divSubmitted);
    }

    private string GetMemberName(string id)
    {
        string name = "";
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings.Get("CMServer")))
        {
            SqlDataAdapter dapt = new SqlDataAdapter("select FirtsName, LastName from eko.Members where userid=@userid", conn);
            dapt.SelectCommand.CommandType = CommandType.Text;
            dapt.SelectCommand.Parameters.AddWithValue("@userid", id);

            dapt.Fill(ds);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                name = dt.Rows[0]["FirtsName"].ToString();
            }
        }

        return name;
    }

    protected string test = "";


    private void AddSubscriber(string email)
    {
        //SqlDataAdapter dapt = new SqlDataAdapter("select * from config where name in ('Mailer_User','Mailer_Connection')", ConfigurationManager.AppSettings["CMServer"]);
        //DataTable dt = new DataTable();
        //dapt.Fill(dt);
        //Dictionary<string, string> prms = new Dictionary<string, string>();
        //foreach (DataRow rw in dt.Rows)
        //    prms.Add(rw["name"].ToString(), rw["value"].ToString());

        ////populate subscriber

        //SqlCommand cmd = new SqlCommand("eFormAddSubscriber", new SqlConnection(prms["Mailer_Connection"]));
        //cmd.CommandType = CommandType.StoredProcedure;
        //cmd.Parameters.AddWithValue("@email", email);
        //cmd.Parameters.AddWithValue("@owner", prms["Mailer_User"]);
        //cmd.Parameters.AddWithValue("@group", EmailSignupGroup);
        //cmd.Connection.Open();
        //cmd.ExecuteNonQuery();
        //cmd.Connection.Close();
    }

    protected void btSubmit_Click(object sender, EventArgs e)
    {
        string fldName, submissionid, fieldid, fieldvalue, prefix, fldType;
        Control ctl;

        List<SqlParameter> parms = new List<SqlParameter>();
        parms.Add(new SqlParameter("@formid", param));

        if (Session["LoggedInId"] != null)
            parms.Add(new SqlParameter("@userid", Session["LoggedInId"]));
        else
            parms.Add(new SqlParameter("@userid", DBNull.Value));


        //SqlParameter[] prms = { new SqlParameter("@formid", param) };
        SqlParameter[] prms = parms.ToArray();

        DataRowCollection rwFields = ds.Tables[1].Rows;

        //validate captcha
        Recaptcha.Web.UI.Controls.Recaptcha reCaptcha = (Recaptcha.Web.UI.Controls.Recaptcha)this.pnEForm.FindControl("reCaptcha");
        if (reCaptcha != null)
        {
            Panel pnlCaptcha = (Panel)this.pnEForm.FindControl("pnlCaptcha");
            Label lblError = new Label();
            lblError.ID = "lblError";
            lblError.CssClass = "error";

            if (String.IsNullOrEmpty(reCaptcha.Response))
            {
                lblError.Text = "Captcha cannot be empty.";
                if (Language == 2)
                {
                    lblError.Text = "Captcha ne peut pas être vide.";
                }

                pnlCaptcha.Controls.Add(lblError);
                return;
            }
            else
            {
                RecaptchaVerificationResult result = reCaptcha.Verify();
                //RecaptchaVerificationResult result = await reCaptcha.VerifyTaskAsync();

                if (result != RecaptchaVerificationResult.Success)
                {
                    lblError.Text = "Captcha Failed!! Please try again!!";

                    if (Language == 2)
                    {
                        lblError.Text = "Erreur : Le code de sécurité que vous avez entré ne correspond pas. Veuillez essayer de nouveau.";
                    }

                    pnlCaptcha.Controls.Add(lblError);
                    return;
                }
            }
        }
        else
        {
            if (!Page.IsValid)
            {
                return;
            }
        }

        //bool valid = true;

        //validate terms and conditions
        //foreach (DataRow rw in rwFields)
        //{
        //    fieldid = rw["id"].ToString();
        //    fldType = rw["type"].ToString();
        //    if (fldType == "Terms")
        //    {
        //        fldName = string.Format("fld{0}", fieldid);
        //        ctl = this.FindControl(fldName);
        //        if (ctl != null)
        //        {
        //            valid = ((CheckBox)ctl).Checked;
        //        }
        //    }
        //}

        //if (!valid)
        //{
        //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ff", "alert('Please confirm Terms and conditions');", true);
        //    return;
        //}

        // ---------------------------------------------------------------------------------------------------------------------------------
        string data = string.Empty;

        data = "<table border=\"0\" CellPadding=\"2\" CellSpacing=\"2\" >";

        //Form's Header
        data += "<tr><td colspan=\"2\" style=\"font-weight: bold;width:100%!important;\">" + sTitle + "</td></tr>";

        if (sIntro.Length > 0)
        {
            data += "<tr><td colspan=\"2\" style=\"font-weight: bold;width:100%!important;\">" + sIntro + "</td></tr>";
        }

        string myPrompt = string.Empty;
        string myFieldValue = string.Empty;
        // ---------------------------------------------------------------------------------------------------------------------------------

        submissionid = ProcessRecord(sqlCreateSubmission, prms);
        //        prefix = pnEForm.UniqueID.ToString().Substring(0, pnEForm.UniqueID.ToString().IndexOf(pnEForm.ID.ToString()));
        prefix = this.UniqueID.ToString() + "$";
        string subjecttitle = "";
        foreach (DataRow rw in rwFields)
        {
            fieldid = rw["id"].ToString();
            fldType = rw["type"].ToString();
            fldName = string.Format("fld{0}", fieldid);
            myPrompt = rw["prompt"].ToString();// + (formID != "1119" ? ":" : "");
            fieldvalue = "";
            ctl = this.FindControl(fldName);

            if (fieldid == "3799"              //Title - Event_Name_English
                )
            {
                //Fields Title "Submit Event"
                subjecttitle = "Event submission – " + Request.Form[string.Format("{0}fld{1}", prefix, fieldid)];
            }

            switch (fldType)
            {
                case "FullName":
                    fldName = string.Format("{0}fld{1}FN", prefix, fieldid);
                    fieldvalue = Request.Form[fldName];
                    myFieldValue = Request.Form[fldName];

                    fldName = string.Format("{0}fld{1}LN", prefix, fieldid);
                    if (fieldvalue.Length > 0)
                    {
                        fieldvalue += " ";
                        myFieldValue += " ";
                    }
                    fieldvalue += Request.Form[fldName];
                    myFieldValue += Request.Form[fldName];
                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;
                case "Phone":
                    //fldName = string.Format("{0}fld{1}p1", prefix, fieldid);
                    fldName = string.Format("{0}fld{1}", prefix, fieldid);
                    fieldvalue = Request.Form[fldName];
                    myFieldValue = Request.Form[fldName];

                    fldName = string.Format("{0}fld{1}p2", prefix, fieldid);
                    fieldvalue += Request.Form[fldName];
                    myFieldValue += "-" + Request.Form[fldName];

                    fldName = string.Format("{0}fld{1}p3", prefix, fieldid);
                    fieldvalue += Request.Form[fldName];
                    myFieldValue += "-" + Request.Form[fldName];

                    fldName = string.Format("{0}fld{1}p4", prefix, fieldid);
                    fieldvalue += Request.Form[fldName];
                    if (!string.IsNullOrEmpty(Request.Form[fldName].ToString()))
                        myFieldValue += " x" + Request.Form[fldName];
                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;
                case "Browse":
                case "Image":
                case "Document":
                    //string _base = "eFormsUploads";
                    myFieldValue = "";
                    if (ctl != null)
                    {
                        HttpPostedFile hpf = ((FileUpload)ctl).PostedFile;
                        if (((FileUpload)ctl).HasFile)
                        {
                            string eFormFolderPath = GetFolder(string.Format("{0}{1}/", eFormsUploadsPath, param));
                            string msg = "alert('{0}');";
                            if (hpf.ContentLength > MAX_FILE_SIZE)
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "fileSizeError", string.Format(msg, string.Format(Translate("File size should be {0}MB or less."), MAX_FILE_SIZE / 1024000)), true);
                                return;
                            }
                            if (CMSHelper.getDirectorySize(eFormFolderPath) + hpf.ContentLength > MAX_FOLDER_SIZE)
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "folderSizeError", string.Format(msg, Translate("Folder size limit exceeded")), true);
                                return;
                            }
                                
                            //fieldvalue = string.Concat(submissionid, "_", System.IO.Path.GetFileName(hpf.FileName));
                            fieldvalue = string.Concat(submissionid, "_", System.IO.Path.GetFileName(hpf.FileName.Replace("..", ".")));
                            myFieldValue = fieldvalue;
                            //hpf.SaveAs(GetFolder(string.Format("/{0}/{1}/", _base, param)) + fieldvalue);
                            hpf.SaveAs(eFormFolderPath + fieldvalue);
                            //Response.Write(GetFolder(string.Format("/{0}/{1}/", _base, param)) + fieldvalue);
                            //return;

                            if (fieldvalue.ToLower().Contains(".pdf")
                                || fieldvalue.ToLower().Contains(".docx")
                                || fieldvalue.ToLower().Contains(".doc")
                                || fieldvalue.ToLower().Contains(".xls")
                                || fieldvalue.ToLower().Contains(".xlsx")
                                || fieldvalue.ToLower().Contains(".jpg")
                                )
                            {
                                attachements.Add(eFormFolderPath + fieldvalue);
                            }
                        }
                    }
                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;
                case "CheckBox":
                case "Terms":
                    fldName = string.Format("{0}fld{1}", prefix, fieldid);
                    fieldvalue = Request.Form[fldName];
                    fieldvalue = (fieldvalue == null ? "No" : "Yes");
                    myFieldValue = fieldvalue;
                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;

                case "Email":
                    fldName = string.Format("fld{0}", fieldid);
                    ctl = this.FindControl(fldName);
                    if (ctl != null)
                    {
                        fieldvalue = ((TextBox)ctl).Text.Trim();
                        //        if (fieldvalue.Length > 0)
                        //        {
                        //            SendMail(fieldvalue, "eForm Submit Notification", "Your form has been successfully submitted");
                        //        }
                        myFieldValue = fieldvalue;
                    }

                    //fldName = string.Format("cbSubscriber{0}", fieldid);
                    //ctl = this.FindControl(fldName);
                    if (EmailSignup)
                    {
                        AddSubscriber(fieldvalue);
                    }
                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;

                case "CheckboxList":

                    DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString(), "priority");
                    int j = 0;
                    fieldvalue = "";
                    foreach (DataRow rwO in rws)
                    {
                        fldName = string.Format("{0}fld{1}${2}", prefix, fieldid, j++);
                        string s = Request.Form[fldName];
                        if (!String.IsNullOrEmpty(s))
                            fieldvalue += (fieldvalue != "" ? "; " : "") + s;

                    }
                    myFieldValue = fieldvalue;

                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;

                /*case "RadioButton":
                    fldName = string.Format("{0}fld{1}", prefix, fieldid);
                    string.Format("{0}fld{1}p1", prefix, fieldid);
                    fieldvalue = Request.Form[fldName];
                    if (string.IsNullOrEmpty(fieldvalue))
                    {
                        myFieldValue = "&nbsp;";
                    }
                    else
                    {
                        DataRow[] rws = ds.Tables[2].Select("fieldid=" + fieldid + " AND id=" + fieldvalue);
                        myFieldValue = rws[0]["optionValue"].ToString();
                    }

                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;*/

                /*case "ListBox":
                    fldName = string.Format("{0}fld{1}", prefix, fieldid);
                    //string.Format("{0}fld{1}p1", prefix, fieldid);
                    fieldvalue = Request.Form[fldName];
                    myFieldValue = fieldvalue;

                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;*/

                case "RadioButton":
                case "DropDown":
                case "DropDownValue":
                    fldName = string.Format("{0}fld{1}", prefix, fieldid);
                    fieldvalue = Request.Form[fldName];
                    if (string.IsNullOrEmpty(fieldvalue))
                    {
                        myFieldValue = "&nbsp;";
                    }
                    else
                    {
                        bool isnumber = false;
                        int n = 0;
                        if (int.TryParse(fieldvalue, out n))
                            isnumber = true;

                        DataRow[] rws2 = ds.Tables[2].Select("fieldid=" + fieldid + (isnumber ? " AND id=" + fieldvalue : " AND optionValue='" + fieldvalue.Replace("'", "''") + "'"));
                        if (rws2.Count() > 0)
                        {
                            myFieldValue = rws2[0]["optionValue"].ToString();
                        }
                    }

                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;

                case "HorizontalLine":
                    data += "<tr><td colspan=\"2\"><hr /></td></tr>";
                    break;
                case "BlankLine":
                    data += "<tr><td style=\"vertical-align:top;\">&nbsp;</td><td style=\"padding-left:10px;\">&nbsp;</td></tr>";
                    break;
                case "Label":
                    data += "<tr><td style=\"vertical-align:top;\" colspan=\"2\">" + myPrompt + "</td></tr>";
                    break;

                case "HtmlEditor":
#if TEXT_ANGULAR
                    fldName = string.Format("fld{0}", fieldid);
#else
                    fldName = string.Format("{0}_fld{1}", "_content_" + this.ClientID, fieldid + "_ctl02");
                    fldName = fldName.Replace("$", "_");
#endif
                    fieldvalue = Server.HtmlDecode(Request.Form[fldName]);
                    myFieldValue = fieldvalue;

                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;

                case "RadioButtonByValue":
                    fldName = string.Format("{0}fld{1}", prefix, fieldid);
                    //string.Format("{0}fld{1}p1", prefix, fieldid);
                    fieldvalue = Request.Form[fldName];
                    //myFieldValue = fieldvalue;

                    Char delimiter = ';';
                    string[] v = fieldvalue.Split(delimiter);
                    fieldvalue = v[0];

                    if (v.Length > 0)
                    {
                        myFieldValue = v[1];
                        data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    }
                    break;

                //---------- Modified to add new Datetime field -------------------------
                case "DateTime":
                    fldName = string.Format("{0}fld{1}d", prefix, fieldid);
                    string hours = string.Format("{0}fld{1}h", prefix, fieldid);
                    string minutes = string.Format("{0}fld{1}m", prefix, fieldid);

                    if (Request.Form[fldName].Length > 0)
                        fieldvalue = Request.Form[fldName] + " " + Request.Form[hours] + ":" + Request.Form[minutes];

                    break;

                //---------- Modified to add new Time field -------------------------
                case "DateTimeRange":
                    fldName = string.Format("{0}fld{1}d", prefix, fieldid);
                    string hours1 = string.Format("{0}fld{1}h", prefix, fieldid);
                    string minutes1 = string.Format("{0}fld{1}m", prefix, fieldid);
                    string hours2 = string.Format("{0}fld{1}h2", prefix, fieldid);
                    string minutes2 = string.Format("{0}fld{1}m2", prefix, fieldid);

                    if (Request.Form[fldName].Length > 0)
                        fieldvalue = Request.Form[fldName] + " " + Request.Form[hours1] + ":" + Request.Form[minutes1] + " to " + Request.Form[hours2] + ":" + Request.Form[minutes2];

                    break;
                //---------------------------------------------

                default:
                    fldName = string.Format("{0}fld{1}", prefix, fieldid);
                    //string.Format("{0}fld{1}p1", prefix, fieldid);
                    fieldvalue = Request.Form[fldName];
                    myFieldValue = fieldvalue;

                    data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
                    break;
            }

            if (fldType != "Label" && fldType != "Title" && fldType != "HorizontalLine" && fldType != "BlankLine")
            {
                if(fldType == "TextBox" ||
                    fldType == "TextArea" ||
                    fldType == "Name" ||
                    fldType == "City" ||
                    fldType == "Phone" ||
                    fldType == "Postal" ||
                    fldType == "Comment" 
                    )
                {
                    fieldvalue = QueryStringHelper.AntiXssEncoder_HtmlEncode(fieldvalue, true);
                }

                ProcessRecord(sqlSaveField, new SqlParameter[] {
                    new SqlParameter("@submissionid", submissionid),
                    new SqlParameter("@fieldid", fieldid),
                    new SqlParameter("@value", fieldvalue) });
            }

            //Form's Body
            //data += "<tr><td style=\"vertical-align:top;\">" + myPrompt + "</td><td style=\"padding-left:10px;\">" + myFieldValue + "</td></tr>";
        }

        data += "</table>";

        /*pnEForm.Controls.Clear();
        Panel pnl = new Panel();
        Label lbl = new Label();
        //lbl.Text = Convert.ToString(ds.Tables[0].Rows[0]["confirmation"]);
        string ThankYouMsg = Convert.ToString(ds.Tables[0].Rows[0]["confirmation"]);

        int npos = ThankYouMsg.IndexOf("<http");
        while (npos > -1)
        {
            int npos2 = ThankYouMsg.IndexOf(">", npos);
            string url = ThankYouMsg.Substring(npos, npos2 - npos + 1);
            ThankYouMsg = ThankYouMsg.Replace(url, string.Format("<a href='{0}' target='_blank'>{0}</a>", url.TrimStart('<').TrimEnd('>')));

            npos = ThankYouMsg.IndexOf("<http", ThankYouMsg.IndexOf("</a>", npos));
        }

        lbl.Text = ThankYouMsg;

        lbl.CssClass = "subtitle";
        if (lbl.Text.Length == 0)
            lbl.Text = Language == 1 ? "Form has been successfully submitted" : "Formulaire a été soumis avec succès";
        else
            lbl.Text = lbl.Text.Replace("\n", "<br/>").Replace("\r","");
        pnl.Controls.Add(lbl);
        pnl.Attributes.Add("style", "margin:20px 0 10px 20px");
        pnEForm.Controls.Add(pnl);*/

        string[] emails = email.Split(new char[] { ',', ';' });
        if (emails.Length > 0 && notify)
        {
            foreach (string email_address in emails)
            {
                string hostUrls = string.Format("{0}://{1}/", Request.Url.Scheme, Request.Url.Host);
                string link = string.Format("{0}login.aspx?c=eforms%26id%3d{1}", hostUrls, submissionid);

            string subj = Language == 1 ?
                "eForm Submit Notification" :
                "eForm présenter une notification";

            if (subjecttitle != "")
                subj = subjecttitle;

            string body = Language == 1 ?
                "Form <strong>{0}</strong> has been successfully submitted" :
                "Formulaire <strong>{0}</strong> a été soumis avec succès";
            body += "<br />";
            body += Language == 1 ?
                "click <a href='{1}'>here</a> to view the submission" :
                "cliquez <a href='{1}'>ici</a> pour visualiser la présentation";
            body += "<br /><br />";
            body += Language == 1 ?
                "Submitted information:" :
                "Informations soumises:";
            body += "<br /><br />";
            body += data;

                body = string.Format(body, sTitle, link);
                // body = "<style>td::first-child{width:55%;}td::last-child{width:45%;}</style>" + body;
                body = "<style>td{width:50%;}</style>" + body;
                //SendMail(email_address, subj, body);
                SendMail("juan@bluelemonmedia.com", subj, body);
            }
        }


        if (Popup)
        {
            if (_confirmationMsg.Length == 0)
                _confirmationMsg = Language == 1 ? "Form has been successfully submitted" : "Formulaire a été soumis avec succès";

            Page.ClientScript.RegisterStartupScript(GetType(), "msg", "alert('" + _confirmationMsg + "');", true);
            afterSubmit();
        }
        else
        {
            Session["eFormSubmitted"] = true;
            Response.Redirect(CMSHelper.GetSeoWithLanguagePrefix() + "?th=1");
        }

        if (param == "2129" && Session["LoggedInID"] != null)
        {
            SqlCommand cmd = new SqlCommand("insert into eko.MemberSurvey(UserId, EformSubmissionId) values(@UserId, @EformSubmissionId)", new SqlConnection(ConfigurationManager.AppSettings["CMServer"]));
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@UserId", Session["LoggedInID"].ToString());
            cmd.Parameters.AddWithValue("@EformSubmissionId", submissionid);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

    }

    private List<string> attachements = new List<string>();

    private string GetFieldValue(string fldName, string fldType)
    {
        return "";
    }

    private void UploadFile()
    {
        /*string _base = "/eFormsUploads/";
        string _path = GetFolder(_base);*/
        string _path = GetFolder(eFormsUploadsPath);
    }

    private string GetFolder(string _folder)
    {
        string physDir = HttpContext.Current.Request.MapPath(_folder);
        if (!System.IO.Directory.Exists(physDir))
        {
            System.IO.Directory.CreateDirectory(physDir);
        }
        return physDir;
    }


    #region Fields

    private Label CreatePrompt(string text, string AssociatedControlID, bool hidden = false)
    {
        Label lb = new Label();
        if (!hidden)
            lb.Text = text + ":";
        else
            lb.Text = "<span class=\"sp-hidden\">" + text + "</span>";
        //lb.Text = text;
        lb.AssociatedControlID = AssociatedControlID;

        return lb;
    }
    private Label CreatePrompt(DataRow rw, string AssociatedControlID)
    {
        Label lb = new Label();
        // lb.Text = ((bool)rw["required"] ? "<span class=\"red-star\">*</span>" : "") + rw["prompt"].ToString() + ":";
        lb.Text = rw["prompt"].ToString(); //+ (formID != "1119" ? ":" : "");

        //lb.Text = text;
        lb.AssociatedControlID = AssociatedControlID;

        return lb;
    }

    private Label CreatePrompt(DataRow rw)
    {
        Label lb = new Label();
        //lb.Text = ((bool)rw["required"] ? "<span class=\"red-star\">*</span>" : "") + rw["prompt"].ToString() + ":";
        lb.Text = rw["prompt"].ToString();//+ (formID != "1119" ? ":" : "");

        if (Convert.ToBoolean(rw["required"]))
            lb.Text += " <span class='error'>*</span>";

        return lb;
    }

    private void vTextBox(DataRow rw, ref PlaceHolder ph, bool pwd, int rows)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        int maxWords = 0;

        TextBox tb = new TextBox();
        if (pwd)
            tb.TextMode = TextBoxMode.Password;

        tb.Columns = 40;
        tb.ID = "fld" + rw["id"].ToString();
        if (rows > 0)
        {
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Rows = rows;

            DataRow[] rws = ds.Tables[2].Select(String.Format("fieldid={0} and optiontext='{1}'", rw["id"].ToString(), "MaxWords"));
            if (rws.Length > 0)
            {
                maxWords = Convert.ToInt16(rws[0]["optionvalue"].ToString());
            }

            if (maxWords > 0)
            {
                //tb.Attributes.Add("class", "count[" + maxWords + "] cctl00_Content_ctl01_" + tb.ID);
                tb.CssClass = "count[" + maxWords + "] c" + _clientIDPrefix + tb.ID;
                //tb.Attributes.Add("class", "c" + tb.ID);

                Panel pnl = new Panel();
                pnl.Width = 343;

                pnl.Controls.Add(tb);

                HtmlGenericControl myDiv = new HtmlGenericControl("div");
                //myDiv.Attributes.Add("class", "ctl00_Content_ctl01_" + tb.ID + " sbar");
                myDiv.Attributes.Add("class", _clientIDPrefix + tb.ID + " sbar");
                //myDiv.Attributes.Add("class", "sbar");
                myDiv.InnerHtml = "<strong>0 / 5</strong> Words";

                pnl.Controls.Add(myDiv);

                divControl.Controls.Add(pnl);
            }
            else
            {
                divControl.Controls.Add(tb);
            }
        }
        else
        {
            divControl.Controls.Add(tb);
        }

        /*HiddenField hf = new HiddenField();
        hf.ID = tb.ID + "_rfv_hf";
        tcl[1].Controls.Add(hf);*/

        /*Label lb = new Label();
        lb.Text = rw["prompt"].ToString() + ":";
        lb.AssociatedControlID = tb.ID;

        divPrompt.Controls.Add(lb);*/
        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), tb.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, tb.ID));
        fieldValidation(rw, divPrompt, tb);

        if (rows > 0 && maxWords > 0)
        {
            fieldWordLimitValidation(rw, divPrompt, tb);

            Label lb2 = new Label();
            lb2.Text = "<br />(" + maxWords.ToString() + " words or less)";
            lb2.AssociatedControlID = tb.ID;
            lb2.Attributes.Add("class", "wl");

            divControl.Controls.Add(lb2);
        }

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vFullName(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        //TableCell[] tcl = GetCells(rw);

        TextBox[] tb = { new TextBox(), new TextBox() };

        tb[0].ID = string.Format("fld{0}FN", rw["id"]);
        tb[0].Width = new Unit(49, UnitType.Percentage);
        tb[0].Attributes.Add("title", "first name");

        TextBoxWatermarkExtender FNExtender = new TextBoxWatermarkExtender();
        FNExtender.ID = tb[0].ID + "_TextBoxWatermarkExtender";
        FNExtender.TargetControlID = tb[0].ID;
        FNExtender.WatermarkText = "First name";
        FNExtender.WatermarkCssClass = "efWatermarked";

        tb[1].ID = string.Format("fld{0}LN", rw["id"]);
        tb[1].Width = new Unit(49, UnitType.Percentage);
        //tb[1].MaxLength = 3;
        tb[1].Attributes.Add("style", "margin-left: 2%;");
        tb[1].Attributes.Add("title", "last name");

        TextBoxWatermarkExtender LNExtender = new TextBoxWatermarkExtender();
        LNExtender.ID = tb[1].ID + "_TextBoxWatermarkExtender";
        LNExtender.TargetControlID = tb[1].ID;
        LNExtender.WatermarkText = "Last name";
        LNExtender.WatermarkCssClass = "efWatermarked";

        divControl.Controls.Add(tb[0]);
        divControl.Controls.Add(FNExtender);
        divControl.Controls.Add(tb[1]);
        divControl.Controls.Add(LNExtender);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), tb[0].ID));
        divPrompt.Controls.Add(CreatePrompt(rw, tb[0].ID));
        fieldValidation(rw, divPrompt, tb[0], "First Name");
        fieldValidation(rw, divPrompt, tb[1], "Last Name");

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vEMail(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        TextBox tb = new TextBox();

        tb.ID = "fld" + rw["id"].ToString();
        divControl.Controls.Add(tb);
        //if (true)
        //{
        //    Literal lt = new Literal();
        //    lt.Text = "<br />";
        //    //Label lb = new Label();
        //    //lb.Text = " Subscribe?";
        //    CheckBox cb = new CheckBox();
        //    cb.ID = "cbSubscriber" + rw["id"].ToString();
        //    cb.Text =string.Format("{0}?", Translate("Sign up"));
        //    tcl[1].Controls.Add(lt);
        //    tcl[1].Controls.Add(cb);
        //    //tcl[1].Controls.Add(lb);
        //}

        /*Label lb = new Label();
        lb.Text = rw["prompt"].ToString() + ":";
        lb.AssociatedControlID = tb.ID;

        divPrompt.Controls.Add(lb);*/
        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), tb.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, tb.ID));
        fieldValidation(rw, divPrompt, tb);
        fieldValidateEmail(divPrompt, tb, rw["prompt"].ToString());

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);

        DataRow[] rws = ds.Tables[2].Select(String.Format("fieldid={0} and optiontext='{1}'", rw["id"].ToString(), "SignUp"));
        if (rws.Length > 0)
        {
            EmailSignup = true;
            EmailSignupGroup = rws[0]["optionvalue"].ToString();
        }
    }

    protected bool EmailSignup
    {
        set { ViewState["Signup"] = value; }
        get { return Convert.ToBoolean(ViewState["Signup"]); }
    }

    protected string EmailSignupGroup
    {
        set { ViewState["SignupGroup"] = value; }
        get { return Convert.ToString(ViewState["SignupGroup"]); }
    }

    private void AddRow(Table tbl, TableCell[] tcl)
    {
        TableRow tr = new TableRow();
        tr.Cells.AddRange(tcl);
        tbl.Rows.Add(tr);
    }

    private TableCell[] GetCells(DataRow rw)
    {
        TableCell[] tcl = { new TableCell(), new TableCell() };
        tcl[0].Attributes.Add("style", "vertical-align:top");
        tcl[0].Text = Translate(rw["prompt"].ToString());       // + (formID != "1119" ? ":" : "");
        tcl[1].CssClass = "rightTD";
        return tcl;
    }

    private void vPhone(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        TextBox[] tb = { new TextBox(), new TextBox(), new TextBox(), new TextBox() };

        //tb[0].ID = string.Format("fld{0}p1", rw["id"]);
        tb[0].ID = string.Format("fld{0}", rw["id"]);
        tb[1].ID = string.Format("fld{0}p2", rw["id"]);
        tb[2].ID = string.Format("fld{0}p3", rw["id"]);
        tb[3].ID = string.Format("fld{0}p4", rw["id"]);

        tb[0].MaxLength = 3;
        tb[0].Width = 55;
        tb[1].Attributes.Add("title", "phone area code");
        tb[1].MaxLength = 3;
        tb[1].Width = 55;
        tb[1].Attributes.Add("style", "margin:0 5px 0 5px");
        tb[1].Attributes.Add("title", "phone prefix code");
        tb[2].MaxLength = 4;
        tb[2].Width = 65;
        tb[2].Attributes.Add("title", "phone line number");
        tb[3].MaxLength = 10;
        tb[3].Width = 60;
        tb[3].Attributes.Add("title", "extension");

        divControl.Controls.Add(tb[0]);
        divControl.Controls.Add(tb[1]);
        divControl.Controls.Add(tb[2]);
        Literal lit = new Literal();
        lit.Text = "ext.";
        divControl.Controls.Add(lit);
        divControl.Controls.Add(tb[3]);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), tb[0].ID));
        divPrompt.Controls.Add(CreatePrompt(rw, tb[0].ID));
        divPrompt.Controls.Add(CreatePrompt("Phone Prefix", tb[1].ID, true));
        divPrompt.Controls.Add(CreatePrompt("Phone Line Number", tb[2].ID, true));
        divPrompt.Controls.Add(CreatePrompt("Phone Extension", tb[3].ID, true));

        fieldValidateNumber(divPrompt, tb[0], 3, "Phone Area Invalid");
        fieldValidateNumber(divPrompt, tb[1], 3, "Phone Prefix Invalid");
        fieldValidateNumber(divPrompt, tb[2], 4, "Phone Line Number Invalid");
        fieldPhoneValidation(rw, divPrompt, tb[0]);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vTextPassword(DataRow rw, ref PlaceHolder ph)
    {
        vTextBox(rw, ref ph, true, 0);
    }
    private void vTextArea(DataRow rw, ref PlaceHolder ph)
    {
        vTextBox(rw, ref ph, false, 10);
    }

    private void vTextBox(DataRow rw, ref PlaceHolder ph)
    {
        vTextBox(rw, ref ph, false, 0);
    }

    private void vDropDown(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        DropDownList ddl = new DropDownList();
        ddl.ID = "fld" + rw["id"].ToString();
        //ddl.Width = 140;
        //DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString(), "priority");
        DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString() + " AND optiontext <> 'Field_Logic'", "priority");
        foreach (DataRow rwO in rws)
        {
            ddl.Items.Add(new ListItem(rwO["optiontext"].ToString(), rwO["optionvalue"].ToString()));
        }

        divControl.Controls.Add(ddl);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), ddl.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, ddl.ID));
        fieldDropDownValidation(rw, divPrompt, ddl);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vDropDownValue(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        DropDownList ddl = new DropDownList();
        ddl.ID = "fld" + rw["id"].ToString();
        //ddl.Width = 140;
        //DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString(), "priority");
        DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString() + " AND optiontext <> 'Field_Logic'", "priority");
        foreach (DataRow rwO in rws)
        {
            ddl.Items.Add(new ListItem(rwO["optiontext"].ToString(), rwO["id"].ToString()));
        }

        divControl.Controls.Add(ddl);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), ddl.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, ddl.ID));
        fieldDropDownValidation(rw, divPrompt, ddl);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vListBox(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        ListBox lb = new ListBox();
        lb.ID = "fld" + rw["id"].ToString();
        //lb.Width = 140;
        lb.Height = Unit.Percentage(100);

        //DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString(), "priority");
        DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString() + " AND optiontext <> 'Field_Logic' AND optiontext not like 'ListBox_%'", "priority");
        foreach (DataRow rwO in rws)
        {
            lb.Items.Add(new ListItem(rwO["optiontext"].ToString(), rwO["optionvalue"].ToString()));
        }

        int _rows = 10;
        bool _multiple = false;

        rws = ds.Tables[2].Select(String.Format("fieldid={0} and optiontext='{1}'", rw["id"].ToString(), "ListBox_Rows"));
        if (rws.Length > 0 && rws[0]["optionvalue"] != DBNull.Value)
        {
            int.TryParse(rws[0]["optionvalue"].ToString(), out _rows);
        }
        lb.Rows = _rows;

        rws = ds.Tables[2].Select(String.Format("fieldid={0} and optiontext='{1}'", rw["id"].ToString(), "ListBox_Multiple"));
        if (rws.Length > 0 && rws[0]["optionvalue"] != DBNull.Value)
        {
            bool.TryParse(rws[0]["optionvalue"].ToString().ToLower(), out _multiple);
        }
        lb.SelectionMode = _multiple ? ListSelectionMode.Multiple : ListSelectionMode.Single;

        divControl.Controls.Add(lb);

        if (_multiple)
        {
            Literal lit = new Literal();
            lit.Text = "<p>(For multiple selections, hold down the CTRL key)</p><p>&nbsp;</p>";
            divControl.Controls.Add(lit);
        }

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), ddl.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, lb.ID));
        fieldValidation(rw, divPrompt, lb);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vWebsite(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        TextBox tb = new TextBox();
        tb.ID = "fld" + rw["id"].ToString();
        tb.Text = "https://";
        //tb.Width = 200;

        divControl.Controls.Add(tb);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), tb.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, tb.ID));
        fieldValidation(rw, divPrompt, tb);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vTitle(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");

        Literal lit = new Literal();
        lit.Text = "<h2>" + rw["prompt"].ToString() + "</h2>";
        divRow.Controls.Add(lit);

        ph.Controls.Add(divRow);
    }

    private void vLabel(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");

        HtmlGenericControl divControl = new HtmlGenericControl("div");

        TextBox tb = new TextBox();

        Literal lit = new Literal();
        lit.Text = "<p id='" + "Content_ctl01_fld" + rw["id"].ToString() + "'>" + rw["prompt"].ToString().Replace("\r\n", "<br />") + "</p>";

        divControl.Controls.Add(lit);
        divRow.Controls.Add(divControl);

        ph.Controls.Add(divRow);
    }

    private void vHorizontalLine(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns ef-horizontal-line");

        Literal lit = new Literal();
        lit.Text = "&nbsp;";
        divRow.Controls.Add(lit);

        ph.Controls.Add(divRow);
    }

    private void vBlankLine(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");

        Literal lit = new Literal();
        lit.Text = "&nbsp;";
        divRow.Controls.Add(lit);

        ph.Controls.Add(divRow);
    }


    private void vDate(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        TextBox tb = new TextBox();
        tb.ID = "fld" + rw["id"].ToString();
        tb.Width = 100;
        tb.Attributes.Add("readonly", "readonly");

        Image img = new Image();
        img.ID = "img" + rw["id"].ToString();
        /*img.Attributes.Add("onclick",
            string.Format("cal1x.select(document.getElementById('{0}'),'{1}','yyyy/MM/dd'); return false;",
            pnEForm.ClientID.Replace(pnEForm.ID, tb.ID), pnEForm.ClientID.Replace(pnEForm.ID, img.ID)));
        img.Attributes.Add("style", "cursor: pointer;margin-left:5px;");*/
        img.Attributes.Add("style", "margin-left:5px;");
        img.ImageUrl = "/images/icons/DatePicker.gif";
        img.ImageAlign = ImageAlign.AbsBottom;
        img.AlternateText = "calendar icon";

        CalendarExtender MyCalendarExtender = new CalendarExtender();
        MyCalendarExtender.ID = tb.ID + "_CalendarExtender";
        MyCalendarExtender.TargetControlID = tb.ID;
        MyCalendarExtender.PopupButtonID = img.ID;
        MyCalendarExtender.Format = "yyyy-MM-dd";
        MyCalendarExtender.PopupPosition = CalendarPosition.BottomLeft;

        TextBoxWatermarkExtender MyTextBoxWatermarkExtender = new TextBoxWatermarkExtender();
        MyTextBoxWatermarkExtender.ID = tb.ID + "_TextBoxWatermarkExtender";
        MyTextBoxWatermarkExtender.TargetControlID = tb.ID;
        MyTextBoxWatermarkExtender.WatermarkText = "yyyy/MM/dd";
        MyTextBoxWatermarkExtender.WatermarkCssClass = "efWatermarked";

        divControl.Controls.Add(tb);
        divControl.Controls.Add(img);
        divControl.Controls.Add(MyCalendarExtender);
        divControl.Controls.Add(MyTextBoxWatermarkExtender);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), tb.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, tb.ID));
        fieldValidation(rw, divPrompt, tb);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    //Modified to add new Datetime field
    #region Datetime
    private void vDateTime(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        TextBox tb = new TextBox();
        tb.ID = string.Format("fld{0}d", rw["id"]);
        tb.Width = 100;

        Image img = new Image();
        img.ID = "img" + rw["id"].ToString();
        //img.Attributes.Add("onclick", 
        //    string.Format("cal1x.select(document.getElementById('{0}'),'{1}','yyyy/MM/dd');return false;",
        //    pnEForm.ClientID.Replace(pnEForm.ID, tb.ID), pnEForm.ClientID.Replace(pnEForm.ID, img.ID)));
        img.Attributes.Add("style", "cursor: pointer;margin-left:5px;");
        img.ImageUrl = "~/images/icons/DatePicker.gif";
        img.AlternateText = "calendar icon";
        img.ImageAlign = ImageAlign.AbsMiddle;

        CalendarExtender MyCalendarExtender = new CalendarExtender();
        MyCalendarExtender.ID = tb.ID + "_CalendarExtender";
        MyCalendarExtender.TargetControlID = tb.ID;
        MyCalendarExtender.PopupButtonID = img.ID;
        MyCalendarExtender.Format = "yyyy-MM-dd";
        MyCalendarExtender.PopupPosition = CalendarPosition.BottomLeft;

        TextBoxWatermarkExtender MyTextBoxWatermarkExtender = new TextBoxWatermarkExtender();
        MyTextBoxWatermarkExtender.ID = tb.ID + "_TextBoxWatermarkExtender";
        MyTextBoxWatermarkExtender.TargetControlID = tb.ID;
        MyTextBoxWatermarkExtender.WatermarkText = "yyyy/MM/dd";
        MyTextBoxWatermarkExtender.WatermarkCssClass = "efWatermarked";

        divControl.Controls.Add(tb);
        divControl.Controls.Add(img);
        divControl.Controls.Add(MyCalendarExtender);
        divControl.Controls.Add(MyTextBoxWatermarkExtender);

        Literal lit = new Literal();
        lit.Text = "&nbsp;&nbsp;&nbsp;";
        divControl.Controls.Add(lit);

        #region Hours
        DropDownList ddlHours = new DropDownList();
        ddlHours.ID = string.Format("fld{0}h", rw["id"]);
        ddlHours.Items.Add(new ListItem("12 AM", "0"));
        for (int i = 1; i < 12; i++)
        {
            ddlHours.Items.Add(new ListItem(i.ToString() + " AM", i.ToString()));
        }
        ddlHours.Items.Add(new ListItem("12 PM", "12"));
        for (int i = 1; i < 12; i++)
        {
            ddlHours.Items.Add(new ListItem(i.ToString() + " PM", (i + 12).ToString()));
        }
        ddlHours.Attributes.Add("class", "no-width");
        divControl.Controls.Add(ddlHours);
        #endregion

        Literal lit2 = new Literal();
        lit2.Text = "&nbsp;";
        divControl.Controls.Add(lit2);

        #region Minutes
        DropDownList ddlMinutes = new DropDownList();
        ddlMinutes.ID = string.Format("fld{0}m", rw["id"]);
        for (int i = 0; i < 10; i++)
        {
            ddlMinutes.Items.Add(new ListItem("0" + i.ToString()));
        }
        for (int i = 10; i < 60; i++)
        {
            ddlMinutes.Items.Add(new ListItem(i.ToString()));
        }
        ddlMinutes.Attributes.Add("class", "no-width");
        divControl.Controls.Add(ddlMinutes);
        #endregion

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), tb.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, tb.ID));
        fieldValidation(rw, divPrompt, tb);

        divPrompt.Controls.Add(CreatePrompt("Hours", ddlHours.ID, true));
        divPrompt.Controls.Add(CreatePrompt("Minutes", ddlMinutes.ID, true));

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }
    #endregion

    //Modified to add new Time field 
    #region Datetime
    private void vDateTimeRange(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        TextBox tb = new TextBox();
        tb.ID = string.Format("fld{0}d", rw["id"]);
        tb.Width = 100;

        Image img = new Image();
        img.ID = "img" + rw["id"].ToString();
        //img.Attributes.Add("onclick", 
        //    string.Format("cal1x.select(document.getElementById('{0}'),'{1}','yyyy/MM/dd');return false;",
        //    pnEForm.ClientID.Replace(pnEForm.ID, tb.ID), pnEForm.ClientID.Replace(pnEForm.ID, img.ID)));
        img.Attributes.Add("style", "cursor: pointer;margin-left:5px;");
        img.ImageUrl = "~/images/icons/DatePicker.gif";
        img.AlternateText = "calendar icon";
        img.ImageAlign = ImageAlign.AbsMiddle;

        CalendarExtender MyCalendarExtender = new CalendarExtender();
        MyCalendarExtender.ID = tb.ID + "_CalendarExtender";
        MyCalendarExtender.TargetControlID = tb.ID;
        MyCalendarExtender.PopupButtonID = img.ID;
        MyCalendarExtender.Format = "yyyy-MM-dd";
        MyCalendarExtender.PopupPosition = CalendarPosition.BottomLeft;

        TextBoxWatermarkExtender MyTextBoxWatermarkExtender = new TextBoxWatermarkExtender();
        MyTextBoxWatermarkExtender.ID = tb.ID + "_TextBoxWatermarkExtender";
        MyTextBoxWatermarkExtender.TargetControlID = tb.ID;
        MyTextBoxWatermarkExtender.WatermarkText = "yyyy/MM/dd";
        MyTextBoxWatermarkExtender.WatermarkCssClass = "efWatermarked";

        divControl.Controls.Add(tb);
        divControl.Controls.Add(img);
        divControl.Controls.Add(MyCalendarExtender);
        divControl.Controls.Add(MyTextBoxWatermarkExtender);

        Literal lit = new Literal();
        lit.Text = "&nbsp;&nbsp;&nbsp;";
        divControl.Controls.Add(lit);

        #region Hours
        DropDownList ddlHours = new DropDownList();
        ddlHours.ID = string.Format("fld{0}h", rw["id"]);
        ddlHours.Items.Add(new ListItem("12 AM", "0"));
        for (int i = 1; i < 12; i++)
        {
            ddlHours.Items.Add(new ListItem(i.ToString() + " AM", i.ToString()));
        }

        ddlHours.Items.Add(new ListItem("12 PM", "12"));
        for (int i = 1; i < 12; i++)
        {
            ddlHours.Items.Add(new ListItem(i.ToString() + " PM", (i + 12).ToString()));
        }
        ddlHours.Attributes.Add("class", "no-width");
        divControl.Controls.Add(ddlHours);
        #endregion

        Literal lit2 = new Literal();
        lit2.Text = "&nbsp;";
        divControl.Controls.Add(lit2);

        #region Minutes
        DropDownList ddlMinutes = new DropDownList();
        ddlMinutes.ID = string.Format("fld{0}m", rw["id"]);
        for (int i = 0; i < 10; i++)
        {
            ddlMinutes.Items.Add(new ListItem("0" + i.ToString()));
        }
        for (int i = 10; i < 60; i++)
        {
            ddlMinutes.Items.Add(new ListItem(i.ToString()));
        }
        ddlMinutes.Attributes.Add("class", "no-width");
        divControl.Controls.Add(ddlMinutes);
        #endregion


        Literal lit3 = new Literal();
        lit3.Text = "&nbsp;&nbsp;-&nbsp;&nbsp;";
        divControl.Controls.Add(lit3);

        #region Hours
        DropDownList ddlHours2 = new DropDownList();
        ddlHours2.ID = string.Format("fld{0}h2", rw["id"]);
        ddlHours2.Items.Add(new ListItem("12 AM", "0"));
        for (int i = 1; i < 12; i++)
        {
            ddlHours2.Items.Add(new ListItem(i.ToString() + " AM", i.ToString()));
        }
        ddlHours2.Items.Add(new ListItem("12 PM", "12"));
        for (int i = 1; i < 12; i++)
        {
            ddlHours2.Items.Add(new ListItem(i.ToString() + " PM", (i + 12).ToString()));
        }
        ddlHours2.Attributes.Add("class", "no-width");
        divControl.Controls.Add(ddlHours2);
        #endregion

        Literal lit4 = new Literal();
        lit4.Text = "&nbsp;";
        divControl.Controls.Add(lit4);

        #region Minutes
        DropDownList ddlMinutes2 = new DropDownList();
        ddlMinutes2.ID = string.Format("fld{0}m2", rw["id"]);
        for (int i = 0; i < 10; i++)
        {
            ddlMinutes2.Items.Add(new ListItem("0" + i.ToString()));
        }
        for (int i = 10; i < 60; i++)
        {
            ddlMinutes2.Items.Add(new ListItem(i.ToString()));
        }
        ddlMinutes2.Attributes.Add("class", "no-width");
        divControl.Controls.Add(ddlMinutes2);
        #endregion



        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), tb.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, tb.ID));
        fieldValidation(rw, divPrompt, tb);

        divPrompt.Controls.Add(CreatePrompt("Hours", ddlHours.ID, true));
        divPrompt.Controls.Add(CreatePrompt("Minutes", ddlMinutes.ID, true));
        divPrompt.Controls.Add(CreatePrompt("Hours", ddlHours2.ID, true));
        divPrompt.Controls.Add(CreatePrompt("Minutes", ddlMinutes2.ID, true));

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }
    #endregion

    private void vNumeric(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        TextBox tb = new TextBox();
        tb.ID = "fld" + rw["id"].ToString();
        //tb.Width = 100;
        //tb.Height = 16;
        tb.Height = 22;
        tb.CssClass = "efNumeric";

        string RefValues = string.Empty;
        int Minimum = 0;
        int Maximum = 100;
        int Step = 1;
        int Width = 100;
        RangeValidator rvNumeric = null;

        DataRow[] rws = ds.Tables[2].Select(String.Format("fieldid={0} and optiontext='{1}'", rw["id"].ToString(), "Numeric_RefValues"));
        if (rws.Length > 0 && rws[0]["optionvalue"] != DBNull.Value)
        {
            RefValues = rws[0]["optionvalue"].ToString();
        }
        else
        {
            rws = ds.Tables[2].Select(String.Format("fieldid={0} and optiontext='{1}'", rw["id"].ToString(), "Numeric_Minimum"));
            if (rws.Length > 0 && rws[0]["optionvalue"] != DBNull.Value)
            {
                int.TryParse(rws[0]["optionvalue"].ToString(), out Minimum);
            }
            //tb.Attributes.Add("Min", Minimum.ToString());

            rws = ds.Tables[2].Select(String.Format("fieldid={0} and optiontext='{1}'", rw["id"].ToString(), "Numeric_Maximum"));
            if (rws.Length > 0 && rws[0]["optionvalue"] != DBNull.Value)
            {
                int.TryParse(rws[0]["optionvalue"].ToString(), out Maximum);
            }
            //tb.Attributes.Add("Max", Maximum.ToString());

            // Adds range validator
            rvNumeric = new RangeValidator();
            rvNumeric.ControlToValidate = tb.ID;
            rvNumeric.ErrorMessage = string.Format(" {0}", string.Format(Translate("Value should be between {0} and {1}."), Minimum, Maximum));
            rvNumeric.Text = " *";
            rvNumeric.SetFocusOnError = true;
            rvNumeric.ValidationGroup = validgroup + formID;
            rvNumeric.Display = ValidatorDisplay.Dynamic;
            rvNumeric.MinimumValue = Minimum.ToString();
            rvNumeric.MaximumValue = Maximum.ToString();
            rvNumeric.Type = ValidationDataType.Integer;
        }

        rws = ds.Tables[2].Select(String.Format("fieldid={0} and optiontext='{1}'", rw["id"].ToString(), "Numeric_Step"));
        if (rws.Length > 0 && rws[0]["optionvalue"] != DBNull.Value)
        {
            int.TryParse(rws[0]["optionvalue"].ToString(), out Step);
        }

        rws = ds.Tables[2].Select(String.Format("fieldid={0} and optiontext='{1}'", rw["id"].ToString(), "Numeric_Width"));
        if (rws.Length > 0 && rws[0]["optionvalue"] != DBNull.Value)
        {
            int.TryParse(rws[0]["optionvalue"].ToString(), out Width);
            if (Width < 25) Width = 25;
        }

        tb.Width = Width - 24;

        ImageButton btnNumericUp = new ImageButton();
        btnNumericUp.ID = tb.ID + "_NumericUpDown_BtnUp";
        btnNumericUp.ImageUrl = "/images/lemonaid/buttons/btn-numeric-up.png";
        btnNumericUp.CausesValidation = false;
        btnNumericUp.AlternateText = "Up";
        btnNumericUp.CssClass = "NumericUpDown_BtnUp";

        ImageButton btnNumericDown = new ImageButton();
        btnNumericDown.ID = tb.ID + "_NumericUpDown_BtnDown";
        btnNumericDown.ImageUrl = "/images/lemonaid/buttons/btn-numeric-down.png";
        btnNumericDown.CausesValidation = false;
        btnNumericDown.AlternateText = "Down";
        btnNumericDown.CssClass = "NumericUpDown_BtnDown";

        NumericUpDownExtender MyExtender = new NumericUpDownExtender();
        MyExtender.ID = tb.ID + "_NumericUpDownExtender";
        MyExtender.TargetControlID = tb.ID;
        MyExtender.TargetButtonUpID = btnNumericUp.ID;
        MyExtender.TargetButtonDownID = btnNumericDown.ID;
        if (string.IsNullOrEmpty(RefValues))
        {
            MyExtender.Minimum = Minimum;
            MyExtender.Maximum = Maximum;
        }
        else
        {
            MyExtender.RefValues = RefValues;
        }
        MyExtender.Step = Step;
        MyExtender.Width = Width;

        divControl.Controls.Add(tb);
        divControl.Controls.Add(btnNumericUp);
        divControl.Controls.Add(btnNumericDown);
        if (rvNumeric != null)
            divControl.Controls.Add(rvNumeric);
        divControl.Controls.Add(MyExtender);

        divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), tb.ID));

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vAddress(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        TextBox tb = new TextBox();
        tb.ID = "fld" + rw["id"].ToString();
        //tb.Width = 200;
        divControl.Controls.Add(tb);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), tb.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, tb.ID));
        fieldValidation(rw, divPrompt, tb);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vProvince(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        DropDownList ddl = new DropDownList();
        ddl.ID = "fld" + rw["id"].ToString();
        //ddl.Width = 140;

        List<SqlParameter> prms = new List<SqlParameter>();
        prms.Add(new SqlParameter("@language", Language));
        DataSet dt = getTables(sqlSelectProvinces, prms);

        ddl.DataSource = dt.Tables[0];
        ddl.DataTextField = "name";
        ddl.DataValueField = "name";

        ddl.DataBind();
        for (int i = 0; i < ddl.Items.Count; i++)
            ddl.Items[i].Text = Translate(ddl.Items[i].Text);

        divControl.Controls.Add(ddl);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), ddl.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, ddl.ID));
        fieldDropDownValidation(rw, divPrompt, ddl);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vState(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        DropDownList ddl = new DropDownList();
        ddl.ID = "fld" + rw["id"].ToString();
        //ddl.Width = 140;

        List<SqlParameter> prms = new List<SqlParameter>();
        prms.Add(new SqlParameter("@language", Language));
        DataSet dt = getTables(sqlSelectStates, prms);

        ddl.DataSource = dt.Tables[0];
        ddl.DataTextField = "name";
        ddl.DataValueField = "name";

        ddl.DataBind();
        for (int i = 0; i < ddl.Items.Count; i++)
            ddl.Items[i].Text = Translate(ddl.Items[i].Text);

        divControl.Controls.Add(ddl);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), ddl.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, ddl.ID));
        fieldDropDownValidation(rw, divPrompt, ddl);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vSalutation(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        DropDownList ddl = new DropDownList();
        ddl.ID = "fld" + rw["id"].ToString();
        //ddl.Width = 140;

        DataSet dt = getTables(sqlSelectSalutation);

        ddl.DataSource = dt.Tables[0];
        ddl.DataTextField = "name";
        ddl.DataValueField = "name";

        ddl.DataBind();

        divControl.Controls.Add(ddl);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), ddl.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, ddl.ID));
        fieldDropDownValidation(rw, divPrompt, ddl);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vCountry(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        DropDownList ddl = new DropDownList();
        ddl.ID = "fld" + rw["id"].ToString();
        //ddl.Width = 140;

        List<SqlParameter> prms = new List<SqlParameter>();
        prms.Add(new SqlParameter("@language", Language));
        DataSet dt = getTables(sqlSelectCountries, prms);

        ddl.DataSource = dt.Tables[0];
        ddl.DataTextField = "name";
        ddl.DataValueField = "name";

        ddl.DataBind();

        divControl.Controls.Add(ddl);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), ddl.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, ddl.ID));
        fieldDropDownValidation(rw, divPrompt, ddl);

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vTerms(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns centered");

        CheckBox cb = new CheckBox();
        cb.ID = "fld" + rw["id"].ToString();

        Label lbl = new Label();
        lbl.Text = string.Format("&nbsp;<a href='javascript:openTerms({1});' onmouseover='javascript:getXY(event);'>{0}</a>", Translate(rw["prompt"].ToString()), rw["id"].ToString());
        lbl.AssociatedControlID = cb.ID;
        lbl.Attributes.Add("class", "span-label");
        DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString(), "priority");

        divRow.Controls.Add(cb);
        divRow.Controls.Add(lbl);
        fieldCheckBoxValidation(rw, divRow, cb);

        //add term and conditions
        Literal lttrms = new Literal();
        lttrms.Text = TermsConditionsDiv(rw["id"].ToString(), rws[0]["optionvalue"].ToString());
        divRow.Controls.Add(lttrms);

        ph.Controls.Add(divRow);
    }

    private void vBrowse(DataRow rw, ref PlaceHolder ph, string type = "Browse")
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        FileUpload fu = new FileUpload();

        fu.ID = "fld" + rw["id"].ToString();
        fu.CssClass = "fu_eForms " + fu.ID;

        Literal lt = new Literal();
        //lt.Text = String.Format("&nbsp;&nbsp;<div class='div-eform-delete-file'><div><a class='button button-eform-delete-file' onclick=\"$('.{0}').val('');\">x</a></div><div><strong>Delete Current File</strong></div></div>", fu.ID);

        divControl.Controls.Add(fu);
        divControl.Controls.Add(lt);
        divControl.Attributes.Add("style", "display:flex; padding:5px;");

        HiddenField hfFileSize = new HiddenField();
        hfFileSize.ID = "fld" + rw["id"].ToString() + "_efuhf1";
        hfFileSize.Value = "0";
        //hfFileSize.Attributes.Add("style", "Visibility: Hidden; width: 1px;");
        //hfFileSize.Text = "0";
        //hfFileSize.Attributes.Add("title", "file size");
        divControl.Controls.Add(hfFileSize);

        HiddenField hfFolderSize = new HiddenField();
        hfFolderSize.ID = "fld" + rw["id"].ToString() + "_efuhf2";
        hfFolderSize.Value = "0";
        //hfFolderSize.Attributes.Add("style", "Visibility: Hidden; width: 1px;");
        //hfFolderSize.Text = "0";
        //hfFolderSize.Attributes.Add("title", "folder size");
        divControl.Controls.Add(hfFolderSize);

        //divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), fu.ID));
        divPrompt.Controls.Add(CreatePrompt(rw, fu.ID));

        if (Convert.ToBoolean(rw["id"]))
            fieldValidation(rw, divPrompt, fu);


        switch (type)
        {
            case "Browse":
                fieldValidateBrowse(divPrompt, fu, rw["prompt"].ToString());
                break;
            case "Image":
                fieldValidateImage(divPrompt, fu);
                break;
            case "Document":
                fieldValidateDocument(divPrompt, fu);
                break;
        }

        // Adds file size validator
        CompareValidator cvFileSize = new CompareValidator();
        cvFileSize.ControlToValidate = hfFileSize.ID;
        //cvFileSize.ErrorMessage = " File size limit exceeded. Files should be 5MB or less.";//string.Format(" {0}", Translate("Required"))
        cvFileSize.ErrorMessage = string.Format(" {0}", string.Format(Translate("File size should be {0}MB or less."), MAX_FILE_SIZE / 1024000));
        cvFileSize.Text = " *";
        cvFileSize.SetFocusOnError = true;
        cvFileSize.ValidationGroup = validgroup + formID;
        cvFileSize.Display = ValidatorDisplay.Dynamic;
        cvFileSize.ValueToCompare = MAX_FILE_SIZE.ToString();
        cvFileSize.Type = ValidationDataType.Integer;
        cvFileSize.Operator = ValidationCompareOperator.LessThanEqual;

#if CLIENT_FILE_SIZE_VALIDATION
        divPrompt.Controls.Add(cvFileSize);
#endif
        // Adds folder size validator
        CompareValidator cvFolderSize = new CompareValidator();
        cvFolderSize.ControlToValidate = hfFolderSize.ID;
        //cvFolderSize.ErrorMessage = " Folder size limit exceeded.";
        cvFolderSize.ErrorMessage = string.Format(" {0}", Translate("Folder size limit exceeded"));
        cvFolderSize.Text = " *";
        cvFolderSize.SetFocusOnError = true;
        cvFolderSize.ValidationGroup = validgroup + formID;
        cvFolderSize.Display = ValidatorDisplay.Dynamic;
        cvFolderSize.ValueToCompare = MAX_FOLDER_SIZE.ToString();
        cvFolderSize.Type = ValidationDataType.Integer;
        cvFolderSize.Operator = ValidationCompareOperator.LessThanEqual;

#if CLIENT_FOLDER_SIZE_VALIDATION
        divPrompt.Controls.Add(cvFolderSize);
#endif
        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void fieldValidateBrowse(HtmlGenericControl tcl, Control tb, string prompt)
    {
        string err = prompt + " " + Translate("Invalid");
        string reg = @"^.+\.([Jj][Pp][Gg])|.+\.([Jj][Pp][Ee][Gg])|.+\.([Pp][Nn][Gg])|.+\.([Gg][Ii][Ff])|.+\.([Bb][Mm][Pp])|.+\.([Pp][Dd][Ff])|.+\.([Xx][Ll][Ss][Xx])|.+\.([Xx][Ll][Ss])|.+\.([Dd][Oo][Cc][Xx])|.+\.([Dd][Oo][Cc])";
        RegularExpValidation(tcl, tb, reg, err);
    }

    private void fieldValidateImage(HtmlGenericControl tcl, Control tb)
    {
        string err = " Invalid Image";
        string reg = @"^.+\.([Jj][Pp][Gg])|.+\.([Jj][Pp][Ee][Gg])|.+\.([Pp][Nn][Gg])|.+\.([Gg][Ii][Ff])|.+\.([Bb][Mm][Pp])";
        RegularExpValidation(tcl, tb, reg, err);
    }
    private void fieldValidateDocument(HtmlGenericControl tcl, Control tb)
    {
        string err = " Invalid Document";
        string reg = @"^.+\.([Pp][Dd][Ff])|.+\.([Xx][Ll][Ss][Xx])|.+\.([Xx][Ll][Ss])|.+\.([Dd][Oo][Cc][Xx])|.+\.([Dd][Oo][Cc])";
        RegularExpValidation(tcl, tb, reg, err);
    }

    private void vCheckBox(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");

        CheckBox cb = new CheckBox();
        cb.ID = "fld" + rw["id"].ToString();
        cb.CssClass = "efCB";
        cb.Text = rw["prompt"].ToString();
        cb.TextAlign = TextAlign.Right;
        //cb.Attributes.Add("style", "margin-bottom:0px; margin-left:10px;");

        //divRow.Controls.Add(CreatePrompt(rw["prompt"].ToString(), cb.ID));
        divRow.Controls.Add(cb);

        fieldCheckBoxValidation(rw, divRow, cb);

        ph.Controls.Add(divRow);
    }

    private void vRadioButton(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        bool first = true;
        bool direction = Convert.ToBoolean(rw["direction"]);
        DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString(), "priority");
        foreach (DataRow rwO in rws)
        {
            if (rwO["optiontext"].ToString() == "Field_Logic")
                continue;


            Literal lit = new Literal();

            lit.Text = direction ? "&nbsp;" : "<br />";
            RadioButton rb = new RadioButton();
            if (!first)
                divControl.Controls.Add(lit);
            rb.Text = rwO["optiontext"].ToString();
            rb.GroupName = string.Format("fld{0}", rw["id"]);
            rb.ID = rwO["id"].ToString();
            //rb.ID = rwO["optionvalue"].ToString();
            divControl.Controls.Add(rb);
            first = false;
        }

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }

    private void vRadioButtonList(DataRow rw, ref PlaceHolder ph, string optionvalue = "optionvalue")
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        HtmlGenericControl filedset = new HtmlGenericControl("fieldset");
        HtmlGenericControl legend = new HtmlGenericControl("legend");
        legend.InnerHtml = rw["prompt"].ToString();
        legend.Attributes.Add("class", "nosize");

        bool direction = Convert.ToBoolean(rw["direction"]);
        DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString(), "priority");

        RadioButtonList rbl = new RadioButtonList();
        rbl.ID = "fld" + rw["id"].ToString();
        rbl.RepeatLayout = RepeatLayout.Flow;
        rbl.CssClass = "efTable";
        foreach (DataRow rwO in rws)
        {
            if (rwO["optiontext"].ToString() == "Field_Logic")
                continue;

            rbl.RepeatDirection = direction ? System.Web.UI.WebControls.RepeatDirection.Horizontal : System.Web.UI.WebControls.RepeatDirection.Vertical;
            //rbl.Items.Add(new ListItem(rwO["optiontext"].ToString(), rwO["id"].ToString()));
            ////rbl.Items.Add(new ListItem(rwO["optiontext"].ToString(), rwO["optionvalue"].ToString()));
            ///
            string s = rwO["optionvalue"].ToString();
            if (optionvalue == "id")
                s = rwO["id"].ToString() + ";" + s;

            rbl.Items.Add(new ListItem(rwO["optiontext"].ToString(), s));
        }
        divControl.Controls.Add(filedset);
        filedset.Controls.Add(legend);
        filedset.Controls.Add(rbl);

        ////divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), rbl.ID));
        //divPrompt.Controls.Add(CreatePrompt(rw, rbl.ID));
        divPrompt.Controls.Add(CreatePrompt(rw));
        if (Convert.ToBoolean(rw["required"]))
        {
            rbl.CausesValidation = true;
            rbl.ValidationGroup = validgroup + formID;
            fieldValidation(rw, divPrompt, rbl);
        }

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }
    private void vCheckboxList(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        HtmlGenericControl filedset = new HtmlGenericControl("fieldset");
        HtmlGenericControl legend = new HtmlGenericControl("legend");
        legend.InnerHtml = rw["prompt"].ToString();
        legend.Attributes.Add("class", "nosize");


        bool direction = Convert.ToBoolean(rw["direction"]);
        DataRow[] rws = ds.Tables[2].Select("fieldid=" + rw["id"].ToString(), "priority");

        CheckBoxList rbl = new CheckBoxList();
        rbl.ID = "fld" + rw["id"].ToString();
        rbl.RepeatLayout = RepeatLayout.Flow;
        rbl.CssClass = "efTable";
        rbl.RepeatDirection = direction ? System.Web.UI.WebControls.RepeatDirection.Horizontal : System.Web.UI.WebControls.RepeatDirection.Vertical;

        foreach (DataRow rwO in rws)
        {
            if (rwO["optiontext"].ToString() != "Field_Logic")
            {
                //rbl.Items.Add(new ListItem(rwO["optiontext"].ToString(), rwO["id"].ToString()));
                rbl.Items.Add(new ListItem(rwO["optiontext"].ToString(), rwO["optionvalue"].ToString()));
            }
        }

        ////divPrompt.Controls.Add(CreatePrompt(rw["prompt"].ToString(), rbl.ID));
        //divPrompt.Controls.Add(CreatePrompt(rw, rbl.ID));
        divPrompt.Controls.Add(CreatePrompt(rw));
        if (Convert.ToBoolean(rw["required"]))
        {
            rbl.CausesValidation = true;
            rbl.ValidationGroup = validgroup + formID;
            fieldCheckBoxListValidation(rw, divPrompt, rbl);
        }

        divControl.Controls.Add(divPrompt);
        divControl.Controls.Add(filedset);
        filedset.Controls.Add(legend);
        filedset.Controls.Add(rbl);

        //divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }
    private void fieldCheckBoxListValidation(DataRow rw, HtmlGenericControl tcl, Control tb, string errorMessage = "")
    {
        if (Convert.ToBoolean(rw["required"]))
        {
            CheckBoxListValidator rfv = new CheckBoxListValidator();
            rfv.MinimumNumberOfSelectedCheckBoxes = 1;
            rfv.ID = tb.ID + "_rfv";
            rfv.ControlToValidate = tb.ID;
            rfv.Display = ValidatorDisplay.Dynamic;
            //rfv.ErrorMessage = string.Format(" {0}", Translate("Required"));
            if (param != "1119")
                rfv.ErrorMessage = string.Format("{0} {1}", errorMessage.Length > 0 ? errorMessage : rw["prompt"].ToString(), Translate("Required"));
            rfv.Text = " *";
            rfv.SetFocusOnError = true;
            rfv.ValidationGroup = validgroup + formID;
            tcl.Controls.Add(rfv);

            HiddenField hfValidatorEnabled = new HiddenField();
            hfValidatorEnabled.ID = tb.ID + "_rfv_hf";
            hfValidatorEnabled.ValueChanged += new EventHandler(hfValidatorEnabled_ValueChanged);
            tcl.Controls.Add(hfValidatorEnabled);
        }
    }

    private void fieldValidation(DataRow rw, HtmlGenericControl tcl, Control tb, string errorMessage = "")
    {
        if (Convert.ToBoolean(rw["required"]))
        {
            RequiredFieldValidator rfv = new RequiredFieldValidator();
            rfv.ID = tb.ID + "_rfv";
            rfv.ControlToValidate = tb.ID;
            rfv.Display = ValidatorDisplay.Dynamic;
            //rfv.ErrorMessage = string.Format(" {0}", Translate("Required"));
            if(param != "1119")
                rfv.ErrorMessage = string.Format("{0} {1}", errorMessage.Length > 0 ? errorMessage : rw["prompt"].ToString(), Translate("Required"));
            rfv.Text = " *";
            rfv.SetFocusOnError = true;
            rfv.ValidationGroup = validgroup + formID;
            tcl.Controls.Add(rfv);

            HiddenField hfValidatorEnabled = new HiddenField();
            hfValidatorEnabled.ID = tb.ID + "_rfv_hf";
            hfValidatorEnabled.ValueChanged += new EventHandler(hfValidatorEnabled_ValueChanged);
            tcl.Controls.Add(hfValidatorEnabled);
        }
    }

    protected void hfValidatorEnabled_ValueChanged(object sender, EventArgs e)
    {
        string rfvID = (sender as HiddenField).ID.Replace("_hf", "");
        //RequiredFieldValidator rfv = (RequiredFieldValidator)this.pnEForm.FindControl(rfvID);
        BaseValidator rfv = (BaseValidator)this.pnEForm.FindControl(rfvID);
        if (rfv != null)
        {
            bool _enabled = true;
            bool.TryParse((sender as HiddenField).Value, out _enabled);
            rfv.Enabled = _enabled;
        }
    }

    /*private void fieldWordLimitValidation(DataRow rw, TableCell tcl, Control tb, string maxWords)
    {
        //if (Convert.ToBoolean(rw["required"]))
        {
            RegularExpressionValidator rev = new RegularExpressionValidator();
            rev.ControlToValidate = tb.ID;
            rev.ValidationExpression = "^([\\W+\\s]*\\w+[\\W+\\s]*){0," + maxWords + "}$";
            //rev.ValidationExpression = "^(([\W+\s]*\w+[\W+\s]*)|([\W+\s]*(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)+[\W+\s]*)|([\W+\s]*(http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)+[\W+\s]*)){0," + maxWords + "}$";
            rev.Display = ValidatorDisplay.Dynamic;
            rev.ErrorMessage = string.Format(" {0}", Translate("Word limit exceeded"));
            rev.SetFocusOnError = true;
            rev.ValidationGroup = validgroup + formID;
            tcl.Controls.Add(rev);
        }
    }*/

    private void fieldWordLimitValidation(DataRow rw, HtmlGenericControl tcl, Control tb)
    {
        //if (Convert.ToBoolean(rw["required"]))
        {
            CustomValidator cv = new CustomValidator();
            cv.ControlToValidate = tb.ID;
            cv.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(CheckWordLimit);
            cv.ClientValidationFunction = "CheckWordLimit";
            cv.EnableClientScript = true;
            cv.ValidateEmptyText = true;
            cv.Display = ValidatorDisplay.Dynamic;
            cv.ErrorMessage = string.Format(" {0}", Translate("Word limit exceeded"));
            cv.Text = " *";
            cv.SetFocusOnError = true;
            cv.ValidationGroup = validgroup + formID;
            tcl.Controls.Add(cv);
        }
    }

    protected void CheckWordLimit(object sender, ServerValidateEventArgs args)
    {
        int number = 0;
        int minWords = 0;
        int maxWords = 0;

        string TextToValidate = string.Empty;
        string MyClass = string.Empty;

        CustomValidator cv = ((CustomValidator)sender);

        if (FindControl(cv.ControlToValidate).GetType().ToString() == "CuteEditor.Editor")
        {
            Editor ed = (Editor)FindControl(cv.ControlToValidate);
            TextToValidate = ed.PlainText;
            MyClass = Regex.Match(ed.CssClass, @"Count\[\d+,?\d*\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value;
        }

        if (FindControl(cv.ControlToValidate).GetType().ToString() == "System.Web.UI.WebControls.TextBox")
        {
            TextBox tb = (TextBox)FindControl(cv.ControlToValidate);
            TextToValidate = args.Value;
            MyClass = Regex.Match(tb.CssClass, @"Count\[\d+,?\d*\]", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value;
        }
        string[] countControl = Regex.Match(MyClass, @"\d+,?\d*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value.Split(',');

        if (countControl.Length > 1)
        {
            //minWords = Convert.ToInt16(countControl[0]);
            //maxWords = Convert.ToInt16(countControl[1]);
            int.TryParse(countControl[0], out minWords);
            int.TryParse(countControl[1], out maxWords);
        }
        else
        {
            //maxWords = Convert.ToInt16(countControl[0]);
            int.TryParse(countControl[0], out maxWords);
        }

        string sPattern = @"(\s*[\w+\S]+\s*)";
        MatchCollection numWords = Regex.Matches(TextToValidate, sPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        if (numWords.Count > 0)
        {
            number = numWords.Count;
        }

        if (number < minWords || (number > maxWords && maxWords != 0))
            args.IsValid = false;
        else
            args.IsValid = true;
    }

    private void fieldPhoneValidation(DataRow rw, HtmlGenericControl tcl, Control tb)
    {
        if (Convert.ToBoolean(rw["required"]))
        {
            CustomValidator cv = new CustomValidator();
            cv.ID = tb.ID + "_rfv";
            cv.ControlToValidate = tb.ID;
            cv.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(PhoneValidation);
            cv.ClientValidationFunction = "PhoneValidation";
            cv.EnableClientScript = true;
            cv.ValidateEmptyText = true;
            cv.Display = ValidatorDisplay.Dynamic;
            //cv.ErrorMessage = string.Format(" {0}", Translate("Required"));
            if (param != "1119")
                cv.ErrorMessage = string.Format("{0} {1}", rw["prompt"].ToString(), Translate("Required"));
            cv.Text = " *";
            cv.SetFocusOnError = true;
            cv.ValidationGroup = validgroup + formID;
            tcl.Controls.Add(cv);

            HiddenField hfValidatorEnabled = new HiddenField();
            hfValidatorEnabled.ID = tb.ID + "_rfv_hf";
            hfValidatorEnabled.ValueChanged += new EventHandler(hfValidatorEnabled_ValueChanged);
            tcl.Controls.Add(hfValidatorEnabled);

            string script = "\n";
            script += "\nfunction PhoneValidation(sender, args) {";
            script += "\n   var tb2 = document.getElementById(sender.controltovalidate + 'p2');";
            script += "\n   var tb3 = document.getElementById(sender.controltovalidate + 'p3');";
            script += "\n   args.IsValid = (args.Value.length > 0 && tb2.value.length > 0 && tb3.value.length > 0);";
            script += "\n}\n";
            Page.ClientScript.RegisterStartupScript(GetType(), "pvf", script, true);
        }
    }

    protected void PhoneValidation(object sender, ServerValidateEventArgs args)
    {
        CustomValidator cv = ((CustomValidator)sender);
        TextBox tb = (TextBox)FindControl(cv.ControlToValidate);
        TextBox tb2 = (TextBox)FindControl(tb.ID + "p2");
        TextBox tb3 = (TextBox)FindControl(tb.ID + "p3");

        /*if (tb.Text.Trim().Length == 0 || tb2.Text.Trim().Length == 0 || tb3.Text.Trim().Length == 0)
            args.IsValid = false;
        else
            args.IsValid = true;*/
        args.IsValid = (args.Value.Trim().Length > 0 && tb2.Text.Trim().Length > 0 && tb3.Text.Trim().Length > 0);
    }

    private void fieldCompareValidation(DataRow rw, HtmlGenericControl tcl, Control tbv, Control tbc)
    {
        //if (Convert.ToBoolean(rw["required"]))
        {
            CompareValidator cv = new CompareValidator();
            cv.ControlToValidate = tbv.ID;
            cv.ControlToCompare = tbc.ID;
            cv.Display = ValidatorDisplay.Dynamic;
            cv.ErrorMessage = string.Format(" {0}", Translate("Emails do not match"));
            cv.Text = " *";
            cv.SetFocusOnError = true;
            cv.ValidationGroup = validgroup + formID;
            tcl.Controls.Add(cv);
        }
    }

    private void fieldDropDownValidation(DataRow rw, HtmlGenericControl tcl, Control ctl)
    {
        DropDownList ddl = (DropDownList)ctl;
        ddl.Items.Insert(0, new ListItem(string.Format("({0})", Translate("select one")), ""));
        ddl.SelectedIndex = 0;

        if (Convert.ToBoolean(rw["required"]))
        {
            RequiredFieldValidator rfv = new RequiredFieldValidator();
            rfv.ID = ddl.ID + "_rfv";
            rfv.ControlToValidate = ddl.ID;
            rfv.Display = ValidatorDisplay.Dynamic;
            //rfv.ErrorMessage = string.Format(" {0}", Translate("Required"));
            if (param != "1119")
                rfv.ErrorMessage = string.Format("{0} {1}", rw["prompt"].ToString(), Translate("Required"));
            rfv.Text = " *";
            rfv.SetFocusOnError = true;
            rfv.ValidationGroup = validgroup + formID;
            tcl.Controls.Add(rfv);

            HiddenField hfValidatorEnabled = new HiddenField();
            hfValidatorEnabled.ID = ddl.ID + "_rfv_hf";
            hfValidatorEnabled.ValueChanged += new EventHandler(hfValidatorEnabled_ValueChanged);
            tcl.Controls.Add(hfValidatorEnabled);
        }
    }

    private void fieldCheckBoxValidation(DataRow rw, HtmlGenericControl tcl, Control cb)
    {

        if (Convert.ToBoolean(rw["required"]))
        {
            TextBox tb = new TextBox();
            tb.ID = "fldV" + rw["id"].ToString();
            tb.Attributes.Add("style", "display:none");
            tb.Attributes.Add("title", "validator");
            CheckBox chb = (CheckBox)cb;

            string cmd = string.Format("document.getElementById('{0}_{1}').value=this.checked?'Yes':''; this.onblur=this.onclick;", this.ClientID, tb.ClientID);
            chb.Attributes.Add("onclick", cmd);
            chb.Attributes.Add("onblur", cmd);

            //System.IO.TextWriter strWriter = new System.IO.StringWriter();
            //HtmlTextWriter ac = new HtmlTextWriter(strWriter);
            //ac.AddAttribute("onclick", cmd);
            //ac.AddAttribute("onblur", cmd);
            //ac.AddAttribute("onfocus", cmd);
            //chb.Attributes.AddAttributes(ac);


            tcl.Controls.Add(tb);
            fieldValidation(rw, tcl, tb);
        }
    }

    private void fieldValidateEmail(HtmlGenericControl tcl, Control tb, string prompt)
    {
        string err = prompt + " " + Translate("Invalid");
        string reg = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        RegularExpValidation(tcl, tb, reg, err);

        //RegularExpressionValidator rfv = new RegularExpressionValidator();
        //rfv.ControlToValidate = tb.ID;
        //rfv.ErrorMessage = " Invalid";
        //rfv.SetFocusOnError = true;
        //rfv.ValidationExpression = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        //rfv.ValidationGroup = validgroup + formID;
        //tcl.Controls.Add(rfv);
    }

    private void fieldValidateNumber(HtmlGenericControl tcl, Control tb, int digits, string prompt)
    {
        string err = prompt + " " + Translate("Invalid");
        string reg = string.Concat(@"^\d{", digits.ToString(), "}$");
        RegularExpValidation(tcl, tb, reg, err);

        //RegularExpressionValidator rfv = new RegularExpressionValidator();
        //rfv.ControlToValidate = tb.ID;
        //rfv.ErrorMessage = " *";
        //rfv.SetFocusOnError = true;
        ////rfv.ValidationExpression = @"^\d{3,4}$";
        //rfv.ValidationExpression = string.Concat(@"^\d{", digits.ToString(), "}$");
        //rfv.ValidationGroup = validgroup + formID;
        //tcl.Controls.Add(rfv);
    }

    private void RegularExpValidation(HtmlGenericControl tcl, Control tb, string regExpr, string errorMessage)
    {
        RegularExpressionValidator rfv = new RegularExpressionValidator();
        rfv.ControlToValidate = tb.ID;
        rfv.ErrorMessage = errorMessage;
        rfv.Text = " *";
        rfv.SetFocusOnError = true;
        rfv.ValidationExpression = regExpr;
        rfv.ValidationGroup = validgroup + formID;
        rfv.Display = ValidatorDisplay.Dynamic;
        tcl.Controls.Add(rfv);
    }

    private string TermsConditionsDiv(string fieldID, string terms)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(string.Format("<div id='tbTerms{0}' style='display: none;'>", fieldID));
        sb.Append("<table style='border: #aeaeae 3px solid; background-color: #ffffff;' cellspacing='1' cellpadding='1' width='500px'>");
        sb.Append(string.Format("<tr><td valign='top' style='height: 300px; width: 500px' id='tdTerms{0}'>", fieldID));
        sb.Append("<table cellspacing='0' cellpadding='0' border='0'>");
        sb.Append("<tr><td style='padding-left: 20px; padding-top: 5px; padding-bottom: 5px;' valign='top'>");
        sb.Append("<div style='overflow: auto; width: 460px; height: 255px'>");
        sb.Append("<span cssclass='bodytext'>");
        sb.Append(terms.Replace("\r\n", "<br />"));
        sb.Append("</span></div></td></tr><tr><td style='padding-right: 10px;'>");
        sb.Append(string.Format("<div id='termClose{0}' class='switcher bodytext' style='float: right;' onclick='return clsTerms({0});'>X</div></td></tr></table></td></tr></table></div>", fieldID));
        return sb.ToString();
    }


    #endregion fields

    #region New Fields
#if TEXT_ANGULAR
    private void vEditor(DataRow rw, ref PlaceHolder ph, int rows = 10)
    {

        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");
        HtmlGenericControl divPrompt = new HtmlGenericControl("div");
        divPrompt.Attributes.Add("class", "efPrompt");
        HtmlGenericControl divControl = new HtmlGenericControl("div");

        Literal lt = new Literal();
        string id = "fld" + rw["id"].ToString();
        //textAngular.min.js: Line 1127:
        //Added code: m.displayElements.forminput.attr("id", m._name), 

        lt.Text = String.Format("<div text-angular name=\"{0}\" ta-text-editor-class=\"clearfix border-around container\" ta-html-editor-class=\"border-around\"></div>",
            id);
        lt.Text += String.Format("<span id=\"validator_{0}\" style=\"color:Red;visibility:hidden;\" >{1}</span>", id, Language == 1 ? "Required" : "Nécessaire");
        divControl.Controls.Add(lt);

        divPrompt.Controls.Add(CreatePrompt(rw, ""));

        divRow.Controls.Add(divPrompt);
        divRow.Controls.Add(divControl);
        ph.Controls.Add(divRow);
    }
#else
    //private void vEditor(DataRow rw, ref PlaceHolder ph, int rows = 10)
    //{

    //    HtmlGenericControl divRow = new HtmlGenericControl("div");
    //    divRow.Attributes.Add("class", "twelve columns");
    //    HtmlGenericControl divPrompt = new HtmlGenericControl("div");
    //    divPrompt.Attributes.Add("class", "efPrompt");
    //    HtmlGenericControl divControl = new HtmlGenericControl("div");


    //    CustomEditor tb = new CustomEditor();
    //    tb.ID = "fld" + rw["id"].ToString();
    //    //tb.Width = new Unit(350);
    //    tb.Height = new Unit(180);

    //    tb.ValidateRequestMode = ValidateRequestMode.Disabled;

    //    divControl.Controls.Add(tb);

    //    divPrompt.Controls.Add(CreatePrompt(rw, tb.ID));
    //    fieldValidation(rw, divPrompt, tb);


    //    divRow.Controls.Add(divPrompt);
    //    divRow.Controls.Add(divControl);
    //    ph.Controls.Add(divRow);
    //}
#endif
    private void vSubTitle(DataRow rw, ref PlaceHolder ph)
    {
        HtmlGenericControl divRow = new HtmlGenericControl("div");
        divRow.Attributes.Add("class", "twelve columns");

        Literal lit = new Literal();
        lit.Text = "<h3>" + rw["prompt"].ToString() + "</h3>";
        divRow.Controls.Add(lit);

        ph.Controls.Add(divRow);
    }

    #endregion

    #region DAL
    private string con = ConfigurationManager.AppSettings.Get("CMServer");
    private string sqlGet = "eFormGet";
    private string sqlCreateSubmission = "eFormSubmissionSave";
    private string sqlSaveField = "eFormFieldSubmissionSave";
    protected string sqlSelectCountries = "eFormCountrySelect";
    protected string sqlSelectProvinces = "eFormProvinceSelect";
    protected string sqlSelectStates = "eFormStateSelect";
    protected string sqlSelectSalutation = "eFormSalutationSelect";
    protected string sqlSelectDictionary = "select * from DictionaryEnFr";

    private DataSet getTables(string cmd)
    {
        return getTables(cmd, new SqlParameter[] { });
    }

    private DataSet getTables(string cmd, List<SqlParameter> prms)
    {
        return getTables(cmd, prms.ToArray());
    }

    private DataSet getTables(string cmd, SqlParameter[] prms)
    {
        DataSet dt = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd, con);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        if (prms != null)
            da.SelectCommand.Parameters.AddRange(prms);
        da.Fill(dt);
        return dt;
    }

    private string ProcessRecord(string sql)
    {
        return ProcessRecord(sql, null);
    }

    private string ProcessRecord(string sql, SqlParameter[] prms)
    {
        SqlCommand cmd = new SqlCommand(sql, new SqlConnection(con));
        cmd.CommandType = CommandType.StoredProcedure;
        if (prms != null)
            cmd.Parameters.AddRange(prms);
        cmd.Connection.Open();
        string ret = Convert.ToString(cmd.ExecuteScalar());
        cmd.Connection.Close();
        return ret;
    }

    #endregion DAL

    #region SMTP

    private void SendMail(string _to, string _subj, string _body)
    {
        SqlDataAdapter dapt = new SqlDataAdapter("select * from config where name in ('SMTP_Server','SMTP_PWD','SMTP_Port','MAIL_FROM','MAIL_FROM_Name','SMTP_UID')", ConfigurationManager.AppSettings["CMServer"]);
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        dapt.Fill(ds);

        string SMTP_Server = ds.Tables[0].Select("name='SMTP_Server'")[0]["value"].ToString();
        string SMTP_Port = ds.Tables[0].Select("name='SMTP_Port'")[0]["value"].ToString();
        string SMTP_UID = ds.Tables[0].Select("name='SMTP_UID'")[0]["value"].ToString();
        string SMTP_PWD = ds.Tables[0].Select("name='SMTP_PWD'")[0]["value"].ToString();
        string MAIL_FROM = ds.Tables[0].Select("name='MAIL_FROM'")[0]["value"].ToString();
        string MAIL_FROM_Name = ds.Tables[0].Select("name='MAIL_FROM_Name'")[0]["value"].ToString();

        SmtpClient smtp = new SmtpClient(SMTP_Server, Int32.Parse(SMTP_Port));
        if (Email.UseSmtpCredentials)
        {
            smtp.Credentials = new System.Net.NetworkCredential(SMTP_UID, SMTP_PWD);
            //smtp.UseDefaultCredentials = true;
        }
        smtp.EnableSsl = Email.EnableSsl;

        MailMessage message = new MailMessage(); ;
        message.From = new MailAddress(MAIL_FROM, MAIL_FROM_Name);
        //message.To.Add(new MailAddress(_to));

        string[] emailsTo = _to.Split(new Char[] { ';', ',' });

        foreach (string s in emailsTo)
        {
            try { message.To.Add(new MailAddress(s)); }
            catch { }

            if (message.To.Count == 0)
                return;
        }

        message.Subject = _subj;
        message.Body = _body;
        message.IsBodyHtml = true;

        if(attachements.Count() > 0 )
        {
            foreach(string att in attachements)
            {
                message.Attachments.Add(new Attachment(att));
            }
        }

        try
        {
            smtp.Send(message);
        }
        catch { }
    }
    #endregion

    private void CretateDictionary()
    {
        dict = new Dictionary<string, string>();

        SqlDataAdapter da = new SqlDataAdapter(sqlSelectDictionary, con);
        DataTable dt = new DataTable("Dictionary");
        da.Fill(dt);
        foreach (DataRow rw in dt.Rows)
            dict.Add(rw["en"].ToString(), rw["fr"].ToString());

        //    dict.Add("Salutation", "Salutation");
        //    dict.Add("Name", "Nom");
        //    dict.Add("Address", "Adresse");
        //    dict.Add("Address1", "Adresse1");
        //    dict.Add("Address2", "Adresse2");
        //    dict.Add("City", "Ville");
        //    dict.Add("Province", "Province");
        //    dict.Add("Select", "Sélectionnez");
        //    dict.Add("Country", "Pays");
        //    dict.Add("Phone", "Téléphone");
        //    dict.Add("Postal Code", "Code postal");
        //    dict.Add("Email", "Courriel");
        //    dict.Add("Website", "Site Web");
        //    dict.Add("Date", "Date");
        //    dict.Add("Comment", "Commentaires");
        //    dict.Add("I Agree", "J'accepte");
        //    dict.Add("Terms and Conditions", "Termes and conditions");
        //    dict.Add("File Download", "Télécharger un fichier");
        //    dict.Add("File Upload", "Envoyer un fichier");
        //    dict.Add("select one", "sélectionnez l'une");
        //    dict.Add("Required", "Nécessaire");
        //    dict.Add("Sign up", "S''inscrire");
    }

    private string Translate(string word)
    {
        if (Language == 2 && dict.ContainsKey(word))
            word = dict[word];
        return word;
    }

    private void BuildClickOnceButton(WebControl ctl, HtmlGenericControl panel)
    {
        System.Text.StringBuilder sbValid = new System.Text.StringBuilder();
        sbValid.Append("if (typeof(Page_ClientValidate) == 'function') { ");
        sbValid.Append("if (Page_ClientValidate('" + (ctl as Button).ValidationGroup + "') == false) { Page_BlockSubmit = false; return false; }} ");
        sbValid.Append("document.getElementById('" + ctl.ClientID + "').style.display = 'none';");
        sbValid.Append("document.getElementById('" + panel.ClientID + "').style.display = 'block';");
        ctl.Attributes.Add("onclick", sbValid.ToString());
    }
}
