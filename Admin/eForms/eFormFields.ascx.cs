using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_eForms_eFormFields : System.Web.UI.UserControl
{
    private DataTable dt
    {
        set { ViewState["fields"] = value; }
        get { return (DataTable)ViewState["fields"]; }

    }

    private DataTable dtLogicFields
    {
        set { ViewState["dtLogicFields"] = value; }
        get { return ViewState["dtLogicFields"] != null ? (DataTable)ViewState["dtLogicFields"] : new DataTable(); }

    }

    private DataTable dtLogic
    {
        set { ViewState["dtLogic"] = value; }
        get { return ViewState["dtLogic"] != null ? (DataTable)ViewState["dtLogic"] : new DataTable(); }

    }

    private DataTable dtfieldOptions
    {
        get
        {
            return ViewState["dtfieldOptions"] != null ? (DataTable)ViewState["dtfieldOptions"] : new DataTable();
        }
        set
        {
            ViewState["dtfieldOptions"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DataTable _dtLogicFields = new DataTable();
            _dtLogicFields.Columns.Add("id", typeof(int));
            _dtLogicFields.Columns.Add("field", typeof(string));
            _dtLogicFields.Columns.Add("type", typeof(string));
            _dtLogicFields.Columns.Add("asptype", typeof(string));

            dtLogicFields = _dtLogicFields;
            
            DataTable _dtLogic = new DataTable();
            _dtLogic.Columns.Add("id", typeof(int));
            _dtLogic.Columns.Add("field", typeof(string));
            _dtLogic.Columns.Add("fieldid", typeof(string));
            _dtLogic.Columns.Add("fieldtype", typeof(string));
            _dtLogic.Columns.Add("operator", typeof(string));
            _dtLogic.Columns.Add("value", typeof(string));

            dtLogic = _dtLogic;
        }
    }

    private string Thankyou
    {
        get
        {
            if (ViewState["Thankyou"] != null)
                return ViewState["Thankyou"].ToString();
            else
                return "";
        }
        set { ViewState["Thankyou"] = value; }
    }
    public void InitME()
    {
        Hashtable he = (Hashtable)Session["eForm"];
       // lbFormCaption.Text = he["title"].ToString();
        // lbFormName.Text = he["name"].ToString();
        formID = he["formid"].ToString();
        formLang = he["language"].ToString();
        SqlParameter[] prms = { new SqlParameter("@id", Convert.ToInt32(formID)) };
        DataTable dt1 = eFormDAL.getTable("eFormsSelect", prms);
        Thankyou = tbThankyou.Text = Convert.ToString(dt1.Rows[0]["confirmation"]);
        BindData();
        BindMailGroups();
    }

    protected void LinkButton1_Click(object sender, CommandEventArgs e)
    {
        //string type = ((LinkButton)sender).CommandArgument;
        string type = e.CommandName;
        //      Label1.Text = type;
        int priority = -1;
        int.TryParse(Request.Params["__EVENTARGUMENT"], out priority);
        CreatePredefinedField(type, priority);
    }

    protected void BindData()
    {
        SqlParameter[] prms = { new SqlParameter("@id", Convert.ToInt32(formID)) };
        dt = eFormDAL.getTable(sqlSelect, prms);
        this.gvFields.DataSource = dt;
        this.gvFields.DataBind();
        //pnlFields.Visible = gvFields.Rows.Count > 0;
    }

    private void BindMailGroups()
    {
        //SqlDataAdapter dapt = new SqlDataAdapter(sqlConfig, conn);
        //DataTable dt = new DataTable();
        //dapt.Fill(dt);
        //Dictionary<string, string> prms = new Dictionary<string, string>();
        //foreach (DataRow rw in dt.Rows)
        //    prms.Add(rw["name"].ToString(), rw["value"].ToString());


        ////test = prms["Mailer_Connection"];
        ////test += ";";
        ////test += prms["Mailer_User"];
        //SqlDataAdapter daptM = new SqlDataAdapter(sqlMailerGroups, prms["Mailer_Connection"]);
        //daptM.SelectCommand.Parameters.AddWithValue("@owner", prms["Mailer_User"]);
        //DataTable dtM = new DataTable();
        //daptM.Fill(dtM);
        ////test = dtM.Rows.Count.ToString();

        //if (dtM.Rows.Count == 0)
        //    ddlMailGroups.Items.Add(new ListItem("eForm"));
        //else
        //{
        //    ddlMailGroups.DataSource = dtM;
        //    ddlMailGroups.DataTextField = "name";
        //    ddlMailGroups.DataValueField = "name";
        //    ddlMailGroups.DataBind();

        //    ListItem li = ddlMailGroups.Items.FindByValue("eForm");
        //    if (li == null)
        //    {
        //        li = new ListItem("eForm");
        //        ddlMailGroups.Items.Insert(0, li);
        //    }

        //    ddlMailGroups.ClearSelection();
        //    ddlMailGroups.SelectedValue = "eForm";
        //    //test = ddlMailGroups.SelectedValue;
        //    //li.Selected = true;
        //    //test = li.Selected.ToString();
        //}
    }

    protected string test;

    protected void btSubmit_Click(object sender, EventArgs e)
    {
        int priority = -1;
        int.TryParse(hPriority.Value, out priority);

        ////if (hFieldType.Value != "CheckboxList")
        {
            if (hFieldType.Value == "Label")
                addField("", tbCaption2.Text, hFieldType.Value, false, false, priority);
            else if (hFieldType.Value == "Title")
                addField("", tbCaption.Text, hFieldType.Value, false, false, priority);
            else
                addField(tbCaption.Text, tbCaption.Text, hFieldType.Value, cbRequired.Checked, rbH.Checked, priority);

            if ((hFieldType.Value == "DropDown" || hFieldType.Value == "RadioButton" || hFieldType.Value == "CheckboxList") && txOptions.Text.Length > 0)
            {
                string[] soptions = txOptions.Text.Replace("\r", "").Split('\n');
                addFieldOptions(soptions);
            }
            else if (hFieldType.Value == "Numeric")
            {
                int Step = 1;
                int Width = 100;

                if (rblNumericType2.SelectedValue == "0") // Numeric
                {
                    int Minimum = 0;
                    int Maximum = 10;

                    int.TryParse(tbMinimum2.Text.Trim(), out Minimum);
                    addFieldOption("Numeric_Minimum", Minimum.ToString());

                    int.TryParse(tbMaximum2.Text.Trim(), out Maximum);
                    addFieldOption("Numeric_Maximum", Maximum.ToString());
                }
                else // Semicolons Separated List
                {
                    addFieldOption("Numeric_RefValues", tbNumericItems2.Text.Trim());
                }

                int.TryParse(tbStep2.Text.Trim(), out Step);
                addFieldOption("Numeric_Step", Step.ToString());

                int.TryParse(tbWidth2.Text.Trim(), out Width);
                addFieldOption("Numeric_Width", Width.ToString());
            }
            else if (hFieldType.Value == "ListBox" && txOptions.Text.Length > 0)
            {
                //string[] soptions = txOptions.Text.Replace("\r", "").Split('\n');
                string[] soptions = txOptions.Text.Replace(",", " ").Replace("\r", "").Split('\n');
                addFieldOptions(soptions);
                //addFieldOptionsWithIndexAsValue(soptions);

                int _rows = 10;
                int.TryParse(txtRows2.Text.Trim(), out _rows);

                addFieldOption("ListBox_Rows", _rows.ToString());
                addFieldOption("ListBox_Multiple", cbMultiple2.Checked.ToString().ToLower());
            }
        }
        ////else
        ////{
        ////    addField("", tbCaption.Text, "Label", false, false, priority);
        ////    string[] soptions = txtCbOptions.Text.Replace("\r", "").Split('\n');

        ////    int i = 1;
        ////    foreach (string s in soptions)
        ////    {
        ////        addField(s, s, "CheckBox", false, false, (priority > 0 ? priority + i : -1));
        ////        i++;
        ////    }
        ////}

        BindData();
        tbCaption.Text = "";
        txOptions.Text = "";
        cbRequired.Checked = false;
        rbH.Checked = false;
        rbV.Checked = true;
        hPriority.Value = "-1";
        rblNumericType2.SelectedValue = "0";
        tbNumericItems2.Text = "";
        tbMinimum2.Text = "0";
        tbMaximum2.Text = "10";
        tbStep2.Text = "1";
        tbWidth2.Text = "100";
        txtRows2.Text = "10";
        cbMultiple2.Checked = false;
    }

    private void addField(string name, string caption, string fieldtype, bool required, bool direction, int priority)
    {
        SqlParameter[] prms = { 
            new SqlParameter("@name", name),
            new SqlParameter("@prompt", caption),
            new SqlParameter("@type", fieldtype),
            new SqlParameter("@required",required),
            new SqlParameter("@direction",direction),
            new SqlParameter("@formid", formID),
            new SqlParameter("@priority", priority)
        };
        try { fieldID = eFormDAL.ProcessRecord(sqlCreate, prms); }
        catch(Exception ex)
        {
            if(ex.Message.Contains("Two fields with the same name"))
            {
                Response.Write(String.Format("<script>alert('{0}');</script>", ex.Message));
            }
            else
                throw new Exception(ex.Message);
        }
    }

    private void addField(string name, string caption, string fieldtype, bool required, int priority)
    {
        addField(name, caption, fieldtype, required, false, priority);
    }

    private void addField(string name, string caption, string fieldtype, bool required, bool direction)
    {
        addField(name, caption, fieldtype, required, direction, -1);
    }

    private void addFieldChild(string name, string caption, string fieldtype, bool required, int priority)
    {
        SqlParameter[] prms = { 
            new SqlParameter("@name", name),
            new SqlParameter("@prompt", caption),
            new SqlParameter("@type", fieldtype),
            new SqlParameter("@required",required),
            new SqlParameter("@formid", formID),
            new SqlParameter("@parentid", fieldID),
            new SqlParameter("@priority", priority)
        };
        fieldID = eFormDAL.ProcessRecord(sqlCreate, prms);
    }

    private void addFieldOption(string name, string value)
    {
        SqlParameter[] prms1 = {
                    new SqlParameter("@fieldid", fieldID),
                    new SqlParameter("@text",name),
                    new SqlParameter("@value", value)
                 };
        eFormDAL.ProcessRecord(sqlCreateOption, prms1);
    }

    private void addFieldOptions(string[] soptions, int max = 0)
    {
        foreach (string sOption in soptions)
        {
            string s = sOption;

            if (max > 0 && s.Length > max)
            {
                s = sOption.Remove(max);
            }
            addFieldOption(s, s);
        }
    }
    private void addFieldOptionsWithIndexAsValue(string[] soptions)
    {
        int i = 0;
        foreach (string sOption in soptions)
        {
            addFieldOption(sOption, i.ToString());
            i++;
        }
    }

  //  protected void Tab_Click(object sender, ImageClickEventArgs e)
  //  {
  //      SwitchTabs(Convert.ToInt32(((ImageButton)sender).CommandArgument));
  //  }

  //  private void SwitchTabs(int tab)
  //  {
  //      Panel1.Visible = false;
  //      Panel2.Visible = false;

		//if (tab == 1)
		//{
		//	Panel1.Visible = true;
		//	newtab.CommandArgument = "2";
		//	newtab.ImageUrl = "/images/lemonaid/buttons/eforms_default.png";
		//}
		//else
		//{
		//	Panel2.Visible = true;
		//	newtab.CommandArgument = "1";
		//	newtab.ImageUrl = "/images/lemonaid/buttons/eforms_custom.png";
		//}

  //      //tab1.ImageUrl = (tab == 1 ? "/images/icons/default_tab2.gif" : "/images/icons/default_tab.gif");
  //      //tab2.ImageUrl = (tab == 2 ? "/images/icons/custom_tab2.gif" : "/images/icons/custom_tab.gif");
  //  }

    private void SwapPriority(int _dir, int id)
    {
        SqlParameter[] prms = { 
        new SqlParameter("@id", id),
        new SqlParameter("@direction", _dir)};
        eFormDAL.ProcessRecord(sqlSwap, prms);
        BindData();

    }

    private void CreatePredefinedField(string type, int priority)
    {
        switch (type)
        {
            case "Name":
                addField("Name", "Name", type, true, priority);
                break;
            case "FullName":
                addField("FullName", "Full Name", "FullName", true, priority);
                break;
            case "Address":
                addField("Address1", "Street", "Address", true, priority);
                addFieldChild("Address2", "Unit/Apt.", "Address", false, priority > 0 ? priority + 1 : -1);
                break;
            case "Phone":
                addField("Phone", "Phone", "Phone", true, priority);
                break;
            case "Date":
                addField("Date", "Date", "Date", true, priority);
                break;
            case "City":
                addField("City", "City", "City", true, priority);
                break;
            case "Website":
                addField("Website", "Website", "Website", true, priority);
                break;
            case "Email":
                addField("Email", "Email", "Email", true, priority);
                break;
            case "Province":
                addField("Province", "Province", "Province", true, priority);
                break;
            case "State":
                addField("State", "State", "State", true, priority);
                break;
            case "Country":
                addField("Country", "Country", "Country", true, priority);
                break;
            case "Comment":
                addField("Comment", "Comment", "Comment", true, priority);
                break;
            case "Postal":
                addField("Postal", "Postal Code", "Postal", true, priority);
                break;
            case "AddressBlock":
                addField("Address1", "Address1", "Address", true, priority);
                addField("Address2", "Address2", "Address", false, priority > 0 ? priority + 1 : -1);
                addField("City", "City", "City", true, priority > 0 ? priority + 2 : -1);
                addField("Province", "Province", "Province", true, priority > 0 ? priority + 3 : -1);
                addField("Country", "Country", "Country", true, priority > 0 ? priority + 4 : -1);
                addField("Postal", "Postal Code", "Postal", true, priority > 0 ? priority + 5 : -1);
                break;
            case "Salutation":
                addField("Salutation", "Salutation", "Salutation", true, priority);
                break;
            case "Terms":
                addField("Terms", "I Agree", "Terms", true, priority);
                string[] soptions = new string[] { "Lorem ipsum dolor sit amet, convallis mauris quis sed, et turpis ad lobortis integer, nisl ut nec vulputate amet tortor, sem cursus dolor. Morbi pellentesque. " };
                addFieldOptions(soptions);
                break;
            case "Browse":
                addField("Browse", "File Upload", "Browse", false, priority);
                break;
            case "HorizontalLine":
                addField("HorizontalLine", "Horizontal Line", "HorizontalLine", false, priority);
                break;
            case "BlankLine":
                addField("BlankLine", "Blank Line", "BlankLine", false, priority);
                break;
            case "Numeric":
                addField("Numeric", "Numeric", type, false, priority);
                addFieldOption("Numeric_Minimum", "0");
                addFieldOption("Numeric_Maximum", "10");
                break;
            /*case "ListBox":
                addField("ListBox", "ListBox", type, true, priority);
                addFieldOption("ListBox_Rows", "10");
                addFieldOption("ListBox_Multiple", "false");
                break;*/
        }
        BindData();
    }


    #region Grid
    private bool direction = false;
    private string fieldType;
    private string prmpt;

    protected void gvFields_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            e.Row.ToolTip = drv["type"].ToString();

            //Literal lbl = (Literal)e.Row.FindControl("lblField");
            Literal lblC = (Literal)e.Row.FindControl("lblCaption");

            //prmpt = DataBinder.Eval(e.Row.DataItem, "Prompt").ToString();
            prmpt = drv["Prompt"].ToString();
            //fieldID = DataBinder.Eval(e.Row.DataItem, "id").ToString();
            fieldID = drv["id"].ToString();
            //lbl.Text = "<table width=\"100%\">";
            //fieldType = DataBinder.Eval(e.Row.DataItem, "FieldType").ToString();
            fieldType = drv["FieldType"].ToString();
            //direction = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "direction"));
            direction = (bool)drv["direction"];
            //if (fieldType != "Label" && fieldType != "HorizontalLine" && fieldType != "BlankLine" && prmpt.Length > 0)
            //{
            //    lbl.Text += string.Format("<tr><td style='text-align: left;'><span class='fprompt'>{0}:</span></td></tr>", prmpt);
            //}

            //lblC.Text = string.Format("<span class='fprompt'>{0}:</span>", prmpt);
            lblC.Text = string.Format("{0}", prmpt);
            //lblC.Text = string.Format("<span class='fprompt' title='{1}'>{0}:</span>", prmpt, drv["type"].ToString());
            
            if (fieldType == "HorizontalLine" || fieldType == "BlankLine")
            {
                ((ImageButton)e.Row.FindControl("imgEdit")).Visible = false;
            }

            string fld = GetField();

            //lbl.Text += string.Format("<tr><td>{0}</td></tr>", fld);
            //lbl.Text = string.Format("{0}", fld);
            //lbl.Text += "</table>";

            Literal litLogic = (Literal)e.Row.FindControl("litLogic");
            if ((bool)drv["hasLogic"])
                litLogic.Text = "<img src=\"/images/lemonaid/buttons/logic.png\" alt=\"logic\" title=\"with logic\" />";

            e.Row.Attributes.Add("id", drv["id"].ToString());
        }

        //obj = e.Row.FindControl("imgEdit");
        //if (obj != null)
        //{
        //    ;
        //}
    }

    protected void gvFields_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        fieldID = gvFields.DataKeys[e.RowIndex].Value.ToString();
        RemoveRecord(sqlDelete, fieldID);
        BindData();
    }
    //protected bool RB = false;
    //protected bool OPT = false;
    //protected bool RQ = true;
    //protected bool MWL = false;
    //protected bool EM = false;
    //protected bool isNumeric = false;
    protected bool RB
    {
        get
        {
            return ViewState["RB"] != null ? (bool)ViewState["RB"] : false;
        }
        set
        {
            ViewState["RB"] = value;
        }
    }
    protected bool OPT
    {
        get
        {
            return ViewState["OPT"] != null ? (bool)ViewState["OPT"] : false;
        }
        set
        {
            ViewState["OPT"] = value;
        }
    }
    protected bool RQ
    {
        get
        {
            return ViewState["RQ"] != null ? (bool)ViewState["RQ"] : true;
        }
        set
        {
            ViewState["RQ"] = value;
        }
    }
    protected bool MWL
    {
        get
        {
            return ViewState["MWL"] != null ? (bool)ViewState["MWL"] : false;
        }
        set
        {
            ViewState["MWL"] = value;
        }
    }
    protected bool EM
    {
        get
        {
            return ViewState["EM"] != null ? (bool)ViewState["EM"] : false;
        }
        set
        {
            ViewState["EM"] = value;
        }
    }
    protected bool isNumeric
    {
        get
        {
            return ViewState["isNumeric"] != null ? (bool)ViewState["isNumeric"] : false;
        }
        set
        {
            ViewState["isNumeric"] = value;
        }
    }
    protected bool isListBox
    {
        get
        {
            return ViewState["isListBox"] != null ? (bool)ViewState["isListBox"] : false;
        }
        set
        {
            ViewState["isListBox"] = value;
        }
    }
    protected bool Logic
    {
        get
        {
            return ViewState["Logic"] != null ? (bool)ViewState["Logic"] : false;
        }
        set
        {
            ViewState["Logic"] = value;
        }
    }

    protected void gvFields_RowEditing(object sender, GridViewEditEventArgs e)
    {
        fieldID = gvFields.DataKeys[e.NewEditIndex].Value.ToString();
        this.tbTitle.Text = gvFields.DataKeys[e.NewEditIndex].Values[1].ToString();
        this.cbMandatory.Checked = Convert.ToBoolean(gvFields.DataKeys[e.NewEditIndex].Values[2]);
        string fType = gvFields.DataKeys[e.NewEditIndex].Values[4].ToString();
        hfType2.Value = fType;
        string fCaption = gvFields.DataKeys[e.NewEditIndex].Values[5].ToString();

        litFieldType.Text = fCaption;

        RB = false;
        OPT = false;
        RQ = true;
        MWL = false;
        EM = false;
        isNumeric = false;
        isListBox = false;

        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
        {
            //string sqlstr = " select f.id, f.prompt as field from eFormFields f inner join eFormFieldTypes ft on f.type=ft.id where f.formid = @formid and f.id <> @fieldID and (ft.aspType in ('DropDown', 'CheckBox', 'RadioButton') or ft.type = 'Numeric') order by priority ";
            string sqlstr = " declare @fieldIDs table(id int, [type] varchar(50), asptype varchar(50)) ";
            sqlstr += " insert into @fieldIDs(id, [type], asptype) ";
            //sqlstr += " select f.id, case when ft.aspType = 'TextBox' and ft.[type] = 'Numeric' then 'Numeric' else ft.aspType end as 'type' from eFormFields f inner join eFormFieldTypes ft on f.type=ft.id where f.formid = @formid and f.id <> @fieldID and (ft.aspType in ('DropDown', 'CheckBox', 'RadioButton') or ft.type = 'Numeric') order by priority ";
            sqlstr += " select f.id, ft.type, case when ft.aspType = 'TextBox' and ft.[type] = 'Numeric' then 'Numeric' else ft.aspType end as 'asptype' from eFormFields f inner join eFormFieldTypes ft on f.type=ft.id where f.formid = @formid and f.id <> @fieldID and (ft.aspType in ('DropDown', 'CheckBox', 'RadioButton', 'ListBox', 'CheckboxList') or ft.type = 'Numeric') and ft.aspType = ft.Type order by priority ";
            sqlstr += " select f.id, f.prompt as field, i.[type], i.asptype from eFormFields f inner join @fieldIDs i on f.id = i.id order by priority ";
            sqlstr += " select * from eFormFieldOptions where fieldid in (select id from @fieldIDs) order by fieldid, priority ";
            sqlstr += " select optionValue from eFormFieldOptions where optionText = 'Field_Logic' and fieldid = @fieldID ";

            SqlDataAdapter dapt = new SqlDataAdapter(sqlstr, conn);
            dapt.SelectCommand.Parameters.AddWithValue("@formid", Convert.ToInt32(formID));
            dapt.SelectCommand.Parameters.AddWithValue("@fieldID", fieldID);
            dapt.Fill(ds);
        }

        if (ds.Tables.Count > 0 )       //&& ds.Tables[0].Rows.Count > 0)
            dtLogicFields = ds.Tables[0];


        if (ds.Tables.Count > 1 )       //&& ds.Tables[1].Rows.Count > 0)
        {
            dtfieldOptions = ds.Tables[1];
        }

        Logic = dtLogicFields.Rows.Count > 0 && dtfieldOptions.Rows.Count > 0;

        if (Logic)
        {
            dtLogic.Rows.Clear();

            string[] _logicElements = null;
            if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0 && ds.Tables[2].Rows[0]["optionValue"] != DBNull.Value)
                _logicElements = ds.Tables[2].Rows[0]["optionValue"].ToString().Split('|');

            if (_logicElements != null && _logicElements.Length > 0)
            {
                ddlShow.ClearSelection();
                try
                {
                    ddlShow.SelectedValue = _logicElements[0];
                }
                catch { }
                ddlAndOr.ClearSelection();
                try
                {
                    ddlAndOr.SelectedValue = _logicElements[1];
                }
                catch { }
                try
                {
                    string[] _logic = null;
                    string _field = string.Empty;
                    string _fieldid = string.Empty;
                    string _fieldtype = string.Empty;
                    for (int i = 2; i < _logicElements.Length; i++)
                    {
                        _logic = _logicElements[i].Split('^');
                        _fieldid = _logic[0].Replace("fld", "");
                        DataRow dr = dtLogicFields.Select("id=" + _fieldid)[0];
                        _field = dr["field"].ToString();
                        //_fieldtype = dr["type"].ToString();
                        _fieldtype = dr["asptype"].ToString();
                        dtLogic.Rows.Add(i - 1, _field, _fieldid, _fieldtype, _logic[1], _logic[2]);
                    }
                }
                catch { }
            }

            BindLogic();
        }

        if (fType == "Email")
        {
            EM = true;
            DataTable dt2 = getOptions();
            if (dt2.Rows.Count > 0)
            {
                DataRow[] rws = dt2.Select("optionText='SignUp'");
                if (rws.Length > 0)
                {
                    cbSignUp.Checked = true;
                    if (ddlMailGroups.Items.FindByValue(rws[0]["optionValue"].ToString()) != null)
                        ddlMailGroups.SelectedValue = rws[0]["optionValue"].ToString();
                    else if (ddlMailGroups.Items.FindByValue("eForm") != null)
                        ddlMailGroups.SelectedValue = "eForm";
                }
                else if (ddlMailGroups.Items.FindByValue("eForm") != null)
                    ddlMailGroups.SelectedValue = "eForm";
            }
            else if (ddlMailGroups.Items.FindByValue("eForm") != null)
                ddlMailGroups.SelectedValue = "eForm";
        }

        if (fType == "Numeric")
        {
            isNumeric = true;
            RQ = false;
            tbMinimum.Text = "0";
            tbMaximum.Text = "10";
            tbNumericItems.Text = "";
            tbStep.Text = "1";
            tbWidth.Text = "100";
            DataTable dt2 = getOptions();
            if (dt2.Rows.Count > 0)
            {
                rblNumericType.ClearSelection();
                DataRow[] rws = dt2.Select("optionText='Numeric_RefValues'");
                if (rws.Length > 0)
                {
                    rblNumericType.SelectedValue = "1";
                    tbNumericItems.Text = rws[0]["optionValue"].ToString();
                }
                else
                {
                    rblNumericType.SelectedValue = "0";
                    rws = dt2.Select("optionText='Numeric_Minimum'");
                    if (rws.Length > 0)
                    {
                        tbMinimum.Text = rws[0]["optionValue"].ToString();
                    }
                    rws = dt2.Select("optionText='Numeric_Maximum'");
                    if (rws.Length > 0)
                    {
                        tbMaximum.Text = rws[0]["optionValue"].ToString();
                    }
                }
                rws = dt2.Select("optionText='Numeric_Step'");
                if (rws.Length > 0)
                {
                    tbStep.Text = rws[0]["optionValue"].ToString();
                }
                rws = dt2.Select("optionText='Numeric_Width'");
                if (rws.Length > 0)
                {
                    tbWidth.Text = rws[0]["optionValue"].ToString();
                }
            }
        }

        if (fType == "Comment")
        {
            MWL = true;
            tbMaxWords.Text = "0";
            DataTable dt2 = getOptions();
            if (dt2.Rows.Count > 0)
            {
                DataRow[] rws = dt2.Select("optionText='MaxWords'");
                if (rws.Length > 0)
                {
                    tbMaxWords.Text = rws[0]["optionValue"].ToString();
                }
            }
        }

        if (fType == "RadioButton"
            || fType == "CheckboxList")
        {
            RB = true;
            rbH2.Checked = Convert.ToBoolean(gvFields.DataKeys[e.NewEditIndex].Values[3]);
            rbV2.Checked = !rbH2.Checked;

        }
        if (fType == "RadioButton" || fType == "DropDown"
            || fType == "CheckboxList")
        {
            OPT = true;
            DataTable dt2 = getOptions();
            string options = "";
            foreach (DataRow rw in dt2.Rows)
            {
                options += (options.Length > 0 ? "\r\n" : "") + rw["optionText"].ToString();
            }
            tbOptions2.Text = options;
        }
        if (fType == "ListBox")
        {
            OPT = true;
            isListBox = true;
            txtRows.Text = "10";
            cbMultiple.Checked = false;

            DataTable dt2 = getOptions();
            DataRow[] rws = dt2.Select("optionText not like 'ListBox_%'");

            string options = "";
            foreach (DataRow rw in rws)
            {
                options += (options.Length > 0 ? "\r\n" : "") + rw["optionText"].ToString();
            }
            tbOptions2.Text = options;

            if (dt2.Rows.Count > 0)
            {
                rws = dt2.Select("optionText='ListBox_Rows'");
                if (rws.Length > 0)
                {
                    txtRows.Text = rws[0]["optionValue"].ToString();
                }
                rws = dt2.Select("optionText='ListBox_Multiple'");
                if (rws.Length > 0)
                {
                    //cbMultiple.Checked = (bool)rws[0]["optionValue"];
                    bool _multiple = false;
                    bool.TryParse(rws[0]["optionValue"].ToString().ToLower(), out _multiple);
                    cbMultiple.Checked = _multiple;
                }
            }
        }
        tbTitle.TextMode = TextBoxMode.SingleLine;
        lbCaption.Text = "Caption";
        if (fType == "Label")
        {
            tbTitle.TextMode = TextBoxMode.MultiLine;
            tbTitle.Rows = 5;
            tbTitle.Columns = 30;
            tbTitle.Text = tbTitle.Text.Replace("<br />", "\r\n");
            RQ = false;
            lbCaption.Text = "Content";
        }
        else if (fType == "Title")
        {
            //tbTitle.Text = tbTitle.Text;
            RQ = false;
            lbCaption.Text = "Title:";
        }
        else if (fType == "Terms")
        {
            OPT = true;
            DataTable dt2 = getOptions();
            tbOptions2.Text = dt2.Rows[0]["optionValue"].ToString();
            tbTitle.TextMode = TextBoxMode.SingleLine;
        }

        EditView = "Edit";
    }

    protected void gvFields_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Up" || e.CommandName == "Down")
        {
            SwapPriority((e.CommandName == "Up" ? 1 : 0), Convert.ToInt32(e.CommandArgument));
            BindData();
        }
    }

    private DataTable getOptions()
    {
        SqlParameter[] prms = { new SqlParameter("@fieldid", fieldID) };
        //return eFormDAL.getTable("eFormFieldOptionsSelect", prms);
        DataView dv = eFormDAL.getTable("eFormFieldOptionsSelect", prms).DefaultView;
        dv.RowFilter = "optionText <> 'Field_Logic'";
        return dv.ToTable();

    }
    #endregion Grid

    #region dal
    protected string conn = ConfigurationManager.AppSettings["CMServer"];
    protected string sqlSelect = "eFormFieldsSelect";
    protected string sqlDelete = "eFormFieldDelete";
    protected string sqlCreate = "eFormFieldCreate";
    protected string sqlUpdate = "eFormFieldUpdate";
    protected string sqlCreateOption = "eFormFieldOptionAdd";
    protected string sqlSelectOptions = "eFormFieldOptionsSelect";
    protected string sqlSwap = "eFormFieldsSwapPriority";
    protected string sqlSelectCountries = "eFormCountrySelect";
    protected string sqlSelectProvinces = "eFormProvinceSelect";
    protected string sqlSelectStates = "eFormStateSelect";
    protected string sqlSelectSalutation = "eFormSalutationSelect";

    protected string sqlConfig = "select * from config where name in ('Mailer_User','Mailer_Connection')";
    protected string sqlMailerGroups = "select * from mailer_groups where owner in (select id from users where username=@owner)";

    private string RemoveRecord(string sql, string rcrd)
    {
        SqlParameter[] prms = { new SqlParameter("@id", rcrd) };
        return eFormDAL.ProcessRecord(sql, prms);
    }
    #endregion dal

    #region properties
    protected string fieldID
    {
        set { ViewState["id"] = value; }
        get { return (string)ViewState["id"]; }
    }

    protected string formID
    {
        set { ViewState["fid"] = value; }
        get { return (string)ViewState["fid"]; }
    }

    protected string formLang
    {
        set { ViewState["formLang"] = value; }
        get { return (string)ViewState["formLang"]; }
    }
    #endregion properties

    #region Fields

    private string GetField()
    {
        string _field = "";
        switch (fieldType)
        {
            case "TextBox":
                _field = vTextBox();
                break;
            case "TextArea":
                _field = vTextArea();
                break;
            case "Label":
                _field = vLabel();
                break;
            case "Password":
                _field = vTextPassword();
                break;
            case "DropDown":
                _field = vDropDown();
                break;
            case "CheckBox":
                _field = vCheckBox();
                break;
            case "RadioButton":
                _field = vRadioButton();
                break;
            case "ListBox":
                _field = vListBox();
                break;
            case "HorizontalLine":
                _field = vHorizontalLine();
                break;
            case "BlankLine":
                _field = vBlankLine();
                break;

            //predefined
            case "eMail":
            case "Email":
                _field = vEmail();
                break;
            case "Name":
                _field = vName();
                break;
            case "Address":
                _field = vAddress();
                break;
            case "Address1":
                _field = vAddress();
                break;
            case "Address2":
                _field = vAddress();
                break;
            case "Phone":
                _field = vPhone();
                break;
            case "Website":
                _field = vWebsite();
                break;
            case "Date":
                _field = vDate();
                break;
            case "City":
                _field = vCity();
                break;
            case "Country":
                _field = vCountry();
                break;
            case "Province":
                _field = vProvince();
                break;
            case "State":
                _field = vState();
                break;

            case "Comment":
                _field = vComment();
                break;
            case "Postal":
                _field = vPostal();
                break;
            case "Salutation":
                _field = vSalutation();
                break;
            case "Terms":
                _field = vTerms();
                break;
            case "Browse":
                _field = vFile();
                break;
            default:
                _field = "";
                break;
        }
        return _field;
    }


    private string vTextBox()
    {
        return "<input type='text' />";
    }

    private string vFile()
    {
        return "<input type='file' />";
    }
    private string vTextArea()
    {
        return "<textarea  cols='15' rows='3'></textarea>";
    }
    private string vTextPassword()
    {
        return "<input type='password' />";
    }

    private string vLabel()
    {
        return prmpt.Replace("\r\n", "<br />");
    }
    private string vDropDown()
    {
        SqlParameter[] prmsd = { new SqlParameter("@fieldid", fieldID) };
        dt = eFormDAL.getTable(sqlSelectOptions, prmsd);
        DataRowCollection rws = dt.Rows;
        string tempo = "<select name='' style='width:160px'>";
        foreach (DataRow rw in rws)
        {
            tempo += string.Format("<option value='{1}'>{0}</option>", rw["optionText"], rw["optionValue"]);
        }
        tempo += "</select>";
        return tempo;
    }

    private string vCheckBox()
    {
        return "<input type='checkbox' />";
    }

    private string vRadioButton()
    {
        SqlParameter[] prmsp = { new SqlParameter("@fieldid", fieldID) };
        dt = eFormDAL.getTable(sqlSelectOptions, prmsp);
        DataRowCollection rws = dt.Rows;
        string mydirection = (direction ? "span" : "div");
        string tempo = "";
        foreach (DataRow rw in rws)
        {
            tempo += string.Format("<{2}><input type='radio' name='RadioGroup{0}' value='{1}' />{1}</{2}>", fieldID, rw["optionValue"], mydirection);
        }
        return tempo;
    }
    private string vListBox()
    {
        SqlParameter[] prmsd = { new SqlParameter("@fieldid", fieldID) };
        dt = eFormDAL.getTable(sqlSelectOptions, prmsd);
        DataRowCollection rws = dt.Rows;
        string tempo = "<select name='' multiple='multiple' style='width:160px'>";
        foreach (DataRow rw in rws)
        {
            tempo += string.Format("<option value='{1}'>{0}</option>", rw["optionText"], rw["optionValue"]);
        }
        tempo += "</select>";
        return tempo;
    }
    private string vHorizontalLine()
    {
        return "<hr />";
    }
    private string vBlankLine()
    {
        return "<br />";
    }

    // Predifined fields
    private string vEmail()
    {
        return "<input type='text' />";
    }
    private string vName()
    {
        return "<input type='text' />";
    }
    private string vAddress()
    {
        return "<input type='text' />";
    }

    private string vCity()
    {
        return "<input type='text' />";
    }



    private string vComment()
    {
        return "<textarea  cols='15' rows='5'></textarea>";
    }

    private string vPostal()
    {
        return "<input type='text' />";
    }

    private string vTerms()
    {
        return "<input type='checkbox' />";
    }


    private string vSalutation()
    {
        dt = eFormDAL.getTable(sqlSelectSalutation);
        DataRowCollection rws = dt.Rows;
        string tempo = "<select name='' style='width:160px'>";
        foreach (DataRow rw in rws)
        {
            tempo += string.Format("<option value='{0}'>{1}</option>", rw["code"], rw["name"]);
        }
        tempo += "</select>";
        return tempo;
    }

    private string vProvince()
    {
        dt = eFormDAL.getTable(sqlSelectProvinces);
        DataRowCollection rws = dt.Rows;
        string tempo = "<select name='' style='width:160px'>";
        tempo += string.Format("<option value='{0}'>{1}</option>", "", "(select one)");
        foreach (DataRow rw in rws)
        {
            tempo += string.Format("<option value='{0}'>{1}</option>", rw["code"], rw["name"]);
        }
        tempo += "</select>";
        return tempo;
    }
    private string vState()
    {
        dt = eFormDAL.getTable(sqlSelectStates);
        DataRowCollection rws = dt.Rows;
        string tempo = "<select name='' style='width:160px'>";
        tempo += string.Format("<option value='{0}'>{1}</option>", "", "(select one)");
        foreach (DataRow rw in rws)
        {
            tempo += string.Format("<option value='{0}'>{1}</option>", rw["code"], rw["name"]);
        }
        tempo += "</select>";
        return tempo;
    }

    private string vCountry()
    {
        dt = eFormDAL.getTable(sqlSelectCountries);
        DataRowCollection rws = dt.Rows;
        string tempo = "<select name='' style='width:160px'>";
        foreach (DataRow rw in rws)
        {
            //tempo += string.Format("<option value='{0}'>{1}</option>", rw["code"], rw["name"]);
            tempo += string.Format("<option value='{0}'>{1}</option>", rw["name"], rw["name"]);
        }
        tempo += "</select>";
        return tempo;
    }
    private string vDate()
    {
        return string.Format("<input type='text' id='dt{0}' /> <img id='imgDP{0}' onclick=\"cal1x.select(document.getElementById('dt{0}'), 'imgDP{0}', 'yyyy/MM/dd'); return false;\" src='/images/icons/DatePicker.gif' style='cursor: pointer' />", fieldID);
    }
    private string vWebsite()
    {
        return "<input type='text' value='https://'/>";
    }
    private string vPhone()
    {
        return "<input maxlength='3' type='text' style='width:40px' />&nbsp;<input maxlength='3' type='text' style='width:40px' />&nbsp<input maxlength='4' type='text' style='width:50px' />&nbsp;ext.<input maxlength='10' type='text' style='width:60px' />";
    }
    #endregion fields

    protected string EditView
    {
        set { ViewState["View"] = value; }
        get { return (ViewState["View"] == null ? "Main" : ViewState["View"].ToString()); }
    }

    protected void ibSave_Click(object sender, EventArgs e)
    {
        EditView = "Main";
        SqlParameter[] prms = { 
            new SqlParameter("@id", fieldID),
            new SqlParameter("@name", tbTitle.Text),
            new SqlParameter("@prompt", tbTitle.Text),
            new SqlParameter("@required", cbMandatory.Checked),
            new SqlParameter("@direction", rbH2.Checked)
                              };
        eFormDAL.ProcessRecord(this.sqlUpdate, prms);

        deleteOptions();
        
        if (hfType2.Value == "Email")
        {
            //deleteOptions();
            if (cbSignUp.Checked)
                addFieldOption("SignUp", ddlMailGroups.SelectedValue);
        }

        if (hfType2.Value == "Numeric")
        {
            //deleteOptions();
            int Step = 1;
            int Width = 100;

            if (rblNumericType.SelectedValue == "0")
            {
                int Minimum = 0;
                int Maximum = 10;
                
                int.TryParse(tbMinimum.Text.Trim(), out Minimum);
                addFieldOption("Numeric_Minimum", Minimum.ToString());

                int.TryParse(tbMaximum.Text.Trim(), out Maximum);
                addFieldOption("Numeric_Maximum", Maximum.ToString());
            }
            else
            {
                addFieldOption("Numeric_RefValues", tbNumericItems.Text.Trim());
            }

            int.TryParse(tbStep.Text.Trim(), out Step);
            addFieldOption("Numeric_Step", Step.ToString());

            int.TryParse(tbWidth.Text.Trim(), out Width);
            addFieldOption("Numeric_Width", Width.ToString());
        }

        if (hfType2.Value == "Comment")
        {
            //deleteOptions();
            int MaxWords = 0;

            int.TryParse(tbMaxWords.Text.Trim(), out MaxWords);
            if (MaxWords > 0)
                addFieldOption("MaxWords", tbMaxWords.Text.Trim());
        }

        if (hfType2.Value == "RadioButton" || hfType2.Value == "DropDown")
        {
        }

        if ((hfType2.Value == "DropDown" || hfType2.Value == "RadioButton" || hfType2.Value == "CheckboxList") && tbOptions2.Text.Length > 0)
        {
            //deleteOptions();

            string[] soptions = tbOptions2.Text.Replace("\r", "").Split('\n');
            addFieldOptions(soptions, hfType2.Value == "DropDown" ? 50 : 0);
        }
        if (hfType2.Value == "ListBox" && tbOptions2.Text.Length > 0)
        {
            //deleteOptions();

            //string[] soptions = tbOptions2.Text.Replace("\r", "").Split('\n');
            string[] soptions = tbOptions2.Text.Replace(",", " ").Replace("\r", "").Split('\n');
            addFieldOptions(soptions);
            //addFieldOptionsWithIndexAsValue(soptions);

            int _rows = 10;
            int.TryParse(txtRows.Text.Trim(), out _rows);

            addFieldOption("ListBox_Rows", _rows.ToString());
            addFieldOption("ListBox_Multiple", cbMultiple.Checked.ToString().ToLower());
        }
        if (hfType2.Value == "Terms")
        {
            //deleteOptions();
            string[] soptions = new string[] { tbOptions2.Text };
            addFieldOptions(soptions);
        }

        // Logic
        string _logic = string.Empty;

        if (dtLogic.Rows.Count > 0)
        {
            _logic = ddlShow.SelectedValue + "|" + ddlAndOr.SelectedValue;

            foreach (DataRow dr in dtLogic.Rows)
            {
                _logic += "|" + "fld" + dr["fieldid"].ToString() + "^" + dr["operator"].ToString() + "^" + dr["value"].ToString() + "^" + dr["fieldtype"].ToString();
            }
        }

        if (!string.IsNullOrEmpty(_logic))
            addFieldOption("Field_Logic", _logic);

        tbThankyou.Text = Thankyou;
        this.BindData();
    }

    private void deleteOptions()
    {
        SqlParameter[] prms2 = { new SqlParameter("@fieldid", fieldID) };
        eFormDAL.ProcessRecord("eFormFieldOptionDelete", prms2);
    }
    protected void ibCancel_Click(object sender, EventArgs e)
    {
        tbThankyou.Text = Thankyou;
        EditView = "Main";
    }

    protected void imgBackF_Click(object sender, EventArgs e)
    {
        EditView = "Main";
    }

    protected void imgBack_Click(object sender, EventArgs e)
    {
        SqlParameter[] prms = { 
            new SqlParameter("@id",formID),
            new SqlParameter("@message", tbThankyou.Text.Trim())};
        eFormDAL.ProcessRecord("eFormContentSave", prms);
        BindData();
    }

    protected void ibAddLogic_Click(object sender, EventArgs e)
    {
        int id = 1;
        if (dtLogic.Rows.Count > 0)
            id = Convert.ToInt32(dtLogic.Compute("max(id)", string.Empty)) + 1;
        //dtLogic.Rows.Add(id, dtLogicFields.Rows[0]["field"].ToString(), dtLogicFields.Rows[0]["id"].ToString(), dtLogicFields.Rows[0]["type"].ToString(), "is", dtfieldOptions.Select("fieldid=" + dtLogicFields.Rows[0]["id"].ToString())[0]["optionValue"].ToString());

 //       for (int i = 0; i < dtLogicFields.Rows.Count; i++)  //if (dtLogicFields.Rows.Count > 0)
        for (int i = 0; i < 1; i++)  //if (dtLogicFields.Rows.Count > 0)
        {
            DataRow[] drs = dtfieldOptions.Select("fieldid=" + dtLogicFields.Rows[i]["id"].ToString());
            if (drs.Count() > 0)
            {
                ////dtLogic.Rows.Add(id, dtLogicFields.Rows[0]["field"].ToString(), dtLogicFields.Rows[0]["id"].ToString(), dtLogicFields.Rows[0]["asptype"].ToString(), "is", dtfieldOptions.Select("fieldid=" + dtLogicFields.Rows[0]["id"].ToString())[0]["optionValue"].ToString());
                dtLogic.Rows.Add(id, dtLogicFields.Rows[0]["field"].ToString(), dtLogicFields.Rows[0]["id"].ToString(), dtLogicFields.Rows[0]["asptype"].ToString(), "is", drs[0]["optionValue"].ToString());

                //if (dtLogicFields.Rows[0]["type"].ToString().ToLower() == "radiobutton")
                //    dtLogic.Rows.Add(id, dtLogicFields.Rows[0]["field"].ToString(), dtLogicFields.Rows[0]["id"].ToString(), dtLogicFields.Rows[0]["type"].ToString(), "is", dtfieldOptions.Select("fieldid=" + dtLogicFields.Rows[0]["id"].ToString())[0]["id"].ToString());
                //else
                //    dtLogic.Rows.Add(id, dtLogicFields.Rows[0]["field"].ToString(), dtLogicFields.Rows[0]["id"].ToString(), dtLogicFields.Rows[0]["type"].ToString(), "is", dtfieldOptions.Select("fieldid=" + dtLogicFields.Rows[0]["id"].ToString())[0]["optionValue"].ToString());
            }
        }
        BindLogic();
    }

    //protected void BindOperatorDD(DropDownList _ddlOperator, string fieldType)
    protected void BindOperatorDD(DropDownList _ddlOperator, bool isNumber)
    {
        _ddlOperator.Items.Clear();
        _ddlOperator.Items.Add(new ListItem("is", "=="));
        _ddlOperator.Items.Add(new ListItem("is not", "!="));
        //if (fieldType.ToLower() == "numeric")
        if (isNumber)
        {
            _ddlOperator.Items.Add(new ListItem("greater than", ">"));
            _ddlOperator.Items.Add(new ListItem("greater than equal", ">="));
            _ddlOperator.Items.Add(new ListItem("less than", "<"));
            _ddlOperator.Items.Add(new ListItem("less than equal", "<="));
        }
    }

    protected bool BindValueDD(DropDownList _ddlValue, string fieldType, string fieldID)
    {
        bool isNumber = false;

        DataView dv = dtfieldOptions.DefaultView;
        dv.RowFilter = "fieldid=" + fieldID;
        _ddlValue.Items.Clear();
        fieldType = fieldType.ToLower();
        switch (fieldType)
        {
            case "numeric":
                dv.RowFilter = "fieldid=" + fieldID + " and optionText='Numeric_RefValues'";
                if (dv.Count > 0)
                {
                    string[] RefValues = dv[0]["optionValue"].ToString().Split(';');
                    foreach (string item in RefValues)
                    {
                        _ddlValue.Items.Add(new ListItem(item));
                    }
                }
                else
                {
                    isNumber = true;

                    int Minimum = 0;
                    int Maximum = 10;

                    dv.RowFilter = "fieldid=" + fieldID + " and optionText='Numeric_Minimum'";
                    if (dv.Count > 0)
                    {
                        int.TryParse(dv[0]["optionValue"].ToString(), out Minimum);
                    }
                    dv.RowFilter = "fieldid=" + fieldID + " and optionText='Numeric_Maximum'";
                    if (dv.Count > 0)
                    {
                        int.TryParse(dv[0]["optionValue"].ToString(), out Maximum);
                    }

                    for (int i = Minimum; i <= Maximum; i++)
                    {
                        _ddlValue.Items.Add(new ListItem(i.ToString()));
                    }
                }
                break;
            case "checkbox":
                _ddlValue.Items.Add(new ListItem("checked", "true"));
                //_ddlValue.Items.Add(new ListItem("unchecked", "false"));
                break;
            case "salutation":
                DataTable dtSalutation = new DataTable();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
                {
                    SqlDataAdapter dapt = new SqlDataAdapter("eFormSalutationSelect", conn);
                    dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dapt.Fill(dtSalutation);
                }
                _ddlValue.DataSource = dtSalutation;
                _ddlValue.DataValueField = "name";
                _ddlValue.DataTextField = "name";
                _ddlValue.DataBind();
                break;
            case "province":
                DataTable dtProvince = new DataTable();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
                {
                    SqlDataAdapter dapt = new SqlDataAdapter("eFormProvinceSelect", conn);
                    dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dapt.SelectCommand.Parameters.AddWithValue("@language", formLang);
                    dapt.Fill(dtProvince);
                }
                _ddlValue.DataSource = dtProvince;
                _ddlValue.DataValueField = "name";
                _ddlValue.DataTextField = "name";
                _ddlValue.DataBind();
                break;
            case "state":
                DataTable dtState = new DataTable();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
                {
                    SqlDataAdapter dapt = new SqlDataAdapter("eFormStateSelect", conn);
                    dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dapt.SelectCommand.Parameters.AddWithValue("@language", formLang);
                    dapt.Fill(dtState);
                }
                _ddlValue.DataSource = dtState;
                _ddlValue.DataValueField = "name";
                _ddlValue.DataTextField = "name";
                _ddlValue.DataBind();
                break;
            case "country":
                DataTable dtCountry = new DataTable();
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CMServer"]))
                {
                    SqlDataAdapter dapt = new SqlDataAdapter("eFormCountrySelect", conn);
                    dapt.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dapt.SelectCommand.Parameters.AddWithValue("@language", formLang);
                    dapt.Fill(dtCountry);
                }
                _ddlValue.DataSource = dtCountry;
                _ddlValue.DataValueField = "name";
                _ddlValue.DataTextField = "name";
                _ddlValue.DataBind();
                break;
            default:
                _ddlValue.DataSource = dv;
                //_ddlValue.DataValueField = fieldType == "radiobutton" ? "id" : "optionValue";
                _ddlValue.DataValueField = "optionValue";
                _ddlValue.DataTextField = "optionText";
                _ddlValue.DataBind();
                break;
        }

        return isNumber;
    }
    
    protected void gvLogic_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            
            DropDownList ddlField = (DropDownList)e.Row.FindControl("ddlField");
            ddlField.DataSource = dtLogicFields;
            ddlField.DataValueField = "id";
            ddlField.DataTextField = "field";
            ddlField.DataBind();

            try
            {
                ddlField.SelectedValue = drv["fieldid"].ToString();
                //ddlField.Items.FindByText(drv["field"].ToString()).Selected = true;
            }
            catch { }

            DropDownList ddlValue = (DropDownList)e.Row.FindControl("ddlValue");
            //bool isNumber = BindValueDD(ddlValue, drv["fieldtype"].ToString(), drv["fieldid"].ToString());
            bool isNumber = BindValueDD(ddlValue, dtLogicFields.Select("id=" + drv["fieldid"].ToString())[0]["type"].ToString(), drv["fieldid"].ToString());
            try
            {
                ddlValue.SelectedValue = drv["value"].ToString();
                //ddlValue.Items.FindByText(drv["value"].ToString()).Selected = true;
            }
            catch { }

            DropDownList ddlOperator = (DropDownList)e.Row.FindControl("ddlOperator");
            //BindOperatorDD(ddlOperator, drv["fieldtype"].ToString());
            BindOperatorDD(ddlOperator, isNumber);
            try
            {
                ddlOperator.SelectedValue = drv["operator"].ToString();
            }
            catch { }

        }
    }

    protected void gvLogic_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string _logicID = gvLogic.DataKeys[e.RowIndex].Value.ToString();
        int rowIndex = dtLogic.Rows.IndexOf(dtLogic.Select("id=" + _logicID)[0]);

        dtLogic.Rows[rowIndex].Delete();
        BindLogic();
    }
    
    protected void BindLogic()
    {
        this.gvLogic.DataSource = dtLogic;
        this.gvLogic.DataBind();
    }

    protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
    {
       //Looping through each Gridview row to find exact Row 
       //of the Grid from where the SelectedIndex change event is fired.
       foreach (GridViewRow row in gvLogic.Rows)
       {
          //Finding Dropdown control  
           Control ctrl = row.FindControl("ddlField") as DropDownList;
            if (ctrl != null)
            {
                DropDownList ddl = (DropDownList)ctrl;
                //Comparing ClientID of the dropdown with sender
                if ((sender as DropDownList).ClientID == ddl.ClientID)
                {
                    string _logicID = gvLogic.DataKeys[row.RowIndex].Value.ToString();
                    int rowIndex = dtLogic.Rows.IndexOf(dtLogic.Select("id=" + _logicID)[0]);

                    dtLogic.Rows[rowIndex]["field"] = ddl.SelectedItem.Text;
                    dtLogic.Rows[rowIndex]["fieldid"] = ddl.SelectedValue;
                    dtLogic.Rows[rowIndex]["fieldtype"] = dtLogicFields.Select("id=" + ddl.SelectedValue)[0]["asptype"].ToString();

                    DropDownList ddlValue = (DropDownList)row.FindControl("ddlValue");
                    //bool isNumber = BindValueDD(ddlValue, dtLogic.Rows[rowIndex]["fieldtype"].ToString(), dtLogic.Rows[rowIndex]["fieldid"].ToString());
                    bool isNumber = BindValueDD(ddlValue, dtLogicFields.Select("id=" + ddl.SelectedValue)[0]["type"].ToString(), dtLogic.Rows[rowIndex]["fieldid"].ToString());

                    //dtLogic.Rows[rowIndex]["value"] = ddlValue.SelectedItem.Text;
                    dtLogic.Rows[rowIndex]["value"] = ddlValue.SelectedValue;

                    DropDownList ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                    //BindOperatorDD(ddlOperator, dtLogic.Rows[rowIndex]["fieldtype"].ToString());
                    BindOperatorDD(ddlOperator, isNumber);
                    dtLogic.Rows[rowIndex]["operator"] = ddlOperator.SelectedValue;

                    break;
                }
            }
       }
    }

    protected void ddlOperator_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Looping through each Gridview row to find exact Row 
        //of the Grid from where the SelectedIndex change event is fired.
        foreach (GridViewRow row in gvLogic.Rows)
        {
            //Finding Dropdown control  
            Control ctrl = row.FindControl("ddlOperator") as DropDownList;
            if (ctrl != null)
            {
                DropDownList ddl = (DropDownList)ctrl;
                //Comparing ClientID of the dropdown with sender
                if ((sender as DropDownList).ClientID == ddl.ClientID)
                {
                    string _logicID = gvLogic.DataKeys[row.RowIndex].Value.ToString();
                    int rowIndex = dtLogic.Rows.IndexOf(dtLogic.Select("id=" + _logicID)[0]);

                    //dtLogic.Rows[rowIndex]["operator"] = ddl.SelectedItem.Text;
                    dtLogic.Rows[rowIndex]["operator"] = ddl.SelectedValue;

                    break;
                }
            }
        }
    }

    protected void ddlValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Looping through each Gridview row to find exact Row 
        //of the Grid from where the SelectedIndex change event is fired.
        foreach (GridViewRow row in gvLogic.Rows)
        {
            //Finding Dropdown control  
            Control ctrl = row.FindControl("ddlValue") as DropDownList;
            if (ctrl != null)
            {
                DropDownList ddl = (DropDownList)ctrl;
                //Comparing ClientID of the dropdown with sender
                if ((sender as DropDownList).ClientID == ddl.ClientID)
                {
                    string _logicID = gvLogic.DataKeys[row.RowIndex].Value.ToString();
                    int rowIndex = dtLogic.Rows.IndexOf(dtLogic.Select("id=" + _logicID)[0]);

                    //dtLogic.Rows[rowIndex]["value"] = ddl.SelectedItem.Text;
                    dtLogic.Rows[rowIndex]["value"] = ddl.SelectedValue;

                    break;
                }
            }
        }
    }
}
