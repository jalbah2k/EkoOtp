<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PageWizard.ascx.cs" Inherits="Admin_Template_PageWizard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
const { rootCertificates } = require("tls");

    var cmsWarningMsg = '';

    $(document).ready(function () {
        var PageTitleMinLen = 60;
        var PageTitleMaxLen = 70;
        var PageDescMinLen = 170;
        var PageDescMaxLen = 200;

        try { PageTitleMinLen = parseInt(/*******************************************************************/); }
        catch (err) { }
        try { PageTitleMaxLen = parseInt(/*******************************************************************/); }
        catch (err) { }
        try { PageDescMinLen = parseInt(<%= ConfigurationManager.AppSettings["PageTitleMinLen"].ToString() %><%= ConfigurationManager.AppSettings["PageTitleMaxLen"].ToString() %><%= ConfigurationManager.AppSettings["PageDescMinLen"].ToString() %>
        catch (err) { }
        try { PageDescMaxLen = parseInt(<%= ConfigurationManager.AppSettings["PageDescMaxLen"].ToString() %>); }
        catch (err) { }

        var ValidateFieldLength = function (element, language, field, minLen, maxLen) {
            if ($(element).val().length < minLen) {
                cmsWarningMsg += '\nThe ' + language + 'page ' + field + ' is too short. We suggest a ' + field + ' between ' + minLen + '-' + maxLen + ' characters.';
            }
            else if ($(element).val().length > maxLen) {
                cmsWarningMsg += '\nThe ' + language + 'page ' + field + ' is too long. We suggest a ' + field + ' between ' + minLen + '-' + maxLen + ' characters.';
            }
        }

        $('input[class*="PageTitle_"]').blur(function () {
            cmsWarningMsg = '';
            var language = 'English ';
            if ($(this).attr('class').indexOf("PageTitle_FR") >= 0) {
                language = 'French ';
            }
            ValidateFieldLength($(this), language, 'title', PageTitleMinLen, PageTitleMaxLen);
            if (cmsWarningMsg.length > 0)
                alert(cmsWarningMsg);
        });

        $('input[class*="PageDesc_"]').blur(function () {
            cmsWarningMsg = '';
            var language = 'English ';
            if ($(this).attr('class').indexOf("PageDesc_FR") >= 0) {
                language = 'French ';
            }
            ValidateFieldLength($(this), language, 'description', PageDescMinLen, PageDescMaxLen);
            if (cmsWarningMsg.length > 0)
                alert(cmsWarningMsg);
        });

    });
    
</script>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Wizard</div>
    <div class="admin-header-subtitle">Here you can add pages, mini-sites, etc.</div>
</div>
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false"></asp:ScriptManager>
<div class="admin-control-wrapper">
    <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Always">
    <ContentTemplate>
        <div id="Div1" runat="server" style="position:absolute; top:220px; left:90px; "><asp:Label ID="lbl_msg" runat="server" CssClass="admin-subtitle"></asp:Label></div>
        <div id="tbl_step_0" runat="server" style="position:absolute; top:250px; left:70px;">
            <div class="admin-white-box" style="min-width: 640px;">
                <div class="admin-white-box-header">Select Wizard</div>
                <div class="admin-white-box-inner">
                    <asp:RadioButtonList ID="RB_Step0" runat="server" CssClass="rb-enhanced" ValidationGroup="step1" CellPadding="10">
                        <asp:ListItem Value="1" Selected="True">New Page</asp:ListItem>
                        <asp:ListItem Value="5" Enabled="false">New Mini-Site</asp:ListItem>
                        <asp:ListItem Value="6" Enabled="false">New Program / Service</asp:ListItem>
                    </asp:RadioButtonList>
                    <div>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="admin-prompt-right">
                                    Multi language        
                                    </td>
                                    <td class="admin-prompt-right">
                                        <asp:RadioButtonList ID="rblMultiLang" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                        </table>
                    </div>
                    <div class="admin-buttons-wrapper">
                        <asp:LinkButton ID="Button4" runat="server" CssClass="admin-button-green mw150" Text="Next" ValidationGroup="step0" onclick="Button4_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div id="tbl_depart" runat="server" style="position:absolute; top:240px; left:70px;">
            <div class="admin-white-box">
                <div class="admin-white-box-header">Filter</div>
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="admin-prompt-right"><span class="required">*</span>Name</td>
                        <td>
                            <asp:TextBox CssClass="textbox" ID="txt_Depart_en" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txt_Depart_en" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step_dept"> *</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="frow25" runat="server">
                        <td class="admin-prompt-right"><span class="required">*</span>French Name</td>
                        <td>
                            <asp:TextBox CssClass="textbox" ID="txt_Depart_fr" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txt_Depart_fr" ErrorMessage="French Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step_dept"> *</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="admin-prompt-right"><span class="required">*</span>Title</td>
                        <td>
                            <asp:TextBox ID="txt_TitleDepart_EN" CssClass="textbox PageTitle_EN" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_TitleDepart_EN" ErrorMessage="Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step_dept"> *</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                        <tr id="frow26" runat="server">
                        <td class="admin-prompt-right"><span class="required">*</span>French Title</td>
                        <td>
                            <asp:TextBox ID="txt_TitleDepart_FR" CssClass="textbox PageTitle_FR" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txt_TitleDepart_FR" ErrorMessage="French Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step_dept"> *</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="admin-prompt-right"><span class="required">*</span>URL</td>
                        <td>
                            <asp:TextBox ID="txt_Depart_seo_en" CssClass="textbox" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDepart_seo_en" runat="server" ControlToValidate="txt_Depart_seo_en" ErrorMessage="URL required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step_dept"> *</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_Depart_seo_en" ErrorMessage="URL invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^[a-zA-Z0-9-]+$" ValidationGroup="step_dept"> *</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr id="frow27" runat="server">
                        <td class="admin-prompt-right"><span class="required">*</span>French URL</td>
                        <td>
                            <asp:TextBox ID="txt_Depart_seo_fr" runat="server" CssClass="textbox" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDepart_seo_fr" runat="server" ControlToValidate="txt_Depart_seo_fr" ErrorMessage="French URL required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step_dept"> *</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_Depart_seo_fr" ErrorMessage="French URL invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^[a-zA-Z0-9-]+$" ValidationGroup="step_dept"> *</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="admin-prompt-right"><span class="required">*</span>Group's Name</td>
                        <td>
                            <asp:TextBox CssClass="textbox" ID="txtGroup2" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvGroup2" runat="server" ControlToValidate="txtGroup2" ErrorMessage="Group's Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step_dept"> *</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="admin-prompt-right">Links to</td>
                        <td>
                            <asp:DropDownList ID="DDL_Depart" runat="server" CssClass="dropdownlist" Width="450"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="admin-prompt-right">Featured</td>
                        <td>
                            <asp:CheckBox ID="chbFeatured" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="admin-prompt-right">&nbsp;</td>
                        <td>
                            <asp:RadioButtonList ID="RB_Active" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Selected="True">Active</asp:ListItem>
                                <asp:ListItem Value="0">Inactive</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="admin-prompt-right">Phone Number</td>
                        <td>
                            <asp:TextBox ID="txt_Phone_number" runat="server" CssClass="textbox" Width="450"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="admin-prompt-right">Room Number</td>
                        <td>
                            <asp:TextBox ID="txt_Room_Number" runat="server" CssClass="textbox" Width="450"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="admin-prompt-right">Site / Location</td>
                        <td>
                            <asp:CheckBoxList ID="cblSites" runat="server" CssClass="rb-enhanced" RepeatColumns="1"></asp:CheckBoxList>
                            <asp:DropDownList ID="ddlSite" runat="server" CssClass="dropdownlist" Width="450" Visible="false" />
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="admin-prompt-right">Age</td>
                        <td>
                            <asp:DropDownList ID="ddlAge" runat="server" CssClass="dropdownlist" Width="450" />
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="admin-prompt-right">Department Group</td>
                        <td>
                            <asp:DropDownList ID="ddlDepGrp" runat="server" CssClass="dropdownlist" Width="450"><asp:ListItem Text="Administrative" Value="1" /><asp:ListItem Text="Clinical" Value="2"/><asp:ListItem Text="Other" Value="0" /></asp:DropDownList>
                        </td>
                    </tr>
                    </table>
                    <div class="admin-buttons-wrapper">
                        <asp:LinkButton ID="btn_dept_back" CausesValidation="False" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="btn_dept_back_Click" />
                        <asp:LinkButton ID="btn_Dept_Submit" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="btn_Dept_Submit_Click" ValidationGroup="step_dept" />
                    </div>
                </div>
            </div>
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="step_dept" ShowMessageBox="True" ShowSummary="False" />
            <script type="text/javascript">
                $(document).ready(function() {
                    var rfvDepart_seo_en = document.getElementById('<%= rfvDepart_seo_en.ClientID %>');
                    var rfvDepart_seo_fr = document.getElementById('<%= rfvDepart_seo_fr.ClientID %>');
                    var rfvGroup2 = document.getElementById('<%= rfvGroup2.ClientID %>');

                    var DDL_DepartOnChange = function () {
                        var isDepartment = $('#<%= DDL_Depart.ClientID %>').val() == 'none';
                        rfvDepart_seo_en.enabled = isDepartment;
                        rfvGroup2.enabled = isDepartment;
                        ValidatorUpdateDisplay(rfvDepart_seo_en);
                        ValidatorUpdateDisplay(rfvGroup2);
                        try {
                            rfvDepart_seo_fr.enabled = isDepartment;
                            ValidatorUpdateDisplay(rfvDepart_seo_fr);
                        }
                        catch (err) { }
                    }
                    $('#<%= DDL_Depart.ClientID %>').change(function () {
                        DDL_DepartOnChange();

                        showhideRows();
                    });
                    DDL_DepartOnChange();

                    showhideRows();
                });

                function showhideRows() {
                    var myVal1 = document.getElementById("<%=RequiredFieldValidator8.ClientID%>");
                    var myVal2 = document.getElementById("<%=rfvDepart_seo_en.ClientID%>");
                    var myVal3 = document.getElementById("<%=rfvGroup2.ClientID%>");
        
                    if ($('#<%=DDL_Depart.ClientID%>').val() != 'none') {
                        $('.trDummy').hide();
                        ValidatorEnable(myVal1, false);
                        ValidatorEnable(myVal2, false);
                        ValidatorEnable(myVal3, false);
                    }
                    else {
                        $('.trDummy').show();
                        ValidatorEnable(myVal1, true);
                        ValidatorEnable(myVal2, true);
                        ValidatorEnable(myVal3, true);
                    }
                }
            </script>
        </div>
        <div id="tbl_Facility" runat="server" style="position:absolute; top:240px; left:70px;">
		<div class="WhiteTable" style="padding:6px;width:500px;">
            <table width="100%" border="0" cellpadding="3" cellspacing="0">
            <tr>
                <td class="admin-prompt-right" style="font-weight:bold;">
                    Facility Details</td>
                <td>&nbsp;
                    </td>
            </tr>
            <tr>
                <td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
                    Name</td>
                <td style="color:#004075;">
                    <asp:TextBox ID="txt_Facility_en" runat="server" Width="300"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" 
                        ControlToValidate="txt_Facility_en" ErrorMessage="Required" 
                        SetFocusOnError="True" ValidationGroup="step_facility"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="admin-prompt-right" style="font-weight:bold;background-color:#c2e1f5;">
                    English URL</td>
                <td style="background-color:#c2e1f5;">
                    <asp:TextBox ID="txt_Facility_seo_en" runat="server" Width="300"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" 
                        ControlToValidate="txt_Facility_seo_en" ErrorMessage="Required" 
                        SetFocusOnError="True" ValidationGroup="step_facility"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="frow15" runat="server">
                <td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
                    French Name</td>
                <td style="color:#004075;">
                    <asp:TextBox ID="txt_Facility_fr" runat="server" Width="300"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" 
                        ControlToValidate="txt_Facility_fr" ErrorMessage="Required" 
                        SetFocusOnError="True" ValidationGroup="step_facility"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="frow16" runat="server">
                <td class="admin-prompt-right" style="font-weight:bold;background-color:#c2e1f5;">
                    French URL</td>
                <td style="background-color:#c2e1f5;">
                    <asp:TextBox ID="txt_Facility_seo_fr" runat="server" Width="300"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" 
                        ControlToValidate="txt_Facility_seo_fr" ErrorMessage="Required" 
                        SetFocusOnError="True" ValidationGroup="step_facility"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
                    Active</td>
                <td style="color:#004075;">
                    <asp:RadioButtonList ID="RB_Active_Facility" runat="server" 
                        RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Selected="True">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="admin-prompt-right" style="font-weight:bold;background-color:#c2e1f5;">
                    Links to</td>
                <td style="background-color:#c2e1f5;">
                    <asp:DropDownList ID="DDL_Facility" runat="server" Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table></div><br /><asp:ImageButton ID="btn_dept_back0" runat="server" CausesValidation="False" 
                        ImageURL="/images/buttons/back.gif" onclick="btn_dept_back_Click" />
                    <asp:ImageButton ID="btn_Facility_Submit" runat="server" ImageURL="/images/buttons/submit.gif"
                        ValidationGroup="step_dept" onclick="btn_Facility_Submit_Click" /></div>
        <div id="tbl_Mini" runat="server" visible="false" style="position:absolute; top:240px; left:70px;">
            <div class="admin-white-box">
                <div class="admin-white-box-header">Mini-Site Information</div>
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="admin-prompt-right"><span class="required">*</span>Name</td>
                        <td>
                            <asp:TextBox CssClass="textbox" ID="mininameen" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="mininameen" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="minisite_val"> *</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="frow18" runat="server">
                        <td class="admin-prompt-right"><span class="required">*</span>French Name</td>
                        <td>
                            <asp:TextBox ID="mininamefr" CssClass="textbox" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="mininamefr" ErrorMessage="French Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="minisite_val"> *</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
						<td Class="admin-prompt-right"><span class="required">*</span>Title</td>
						<td>
							<asp:TextBox CssClass="textbox PageTitle_EN" ID="txt_TitleMini_EN" runat="server" ValidationGroup="minisite_val" Width="450" ></asp:TextBox>
							<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_TitleMini_EN" ErrorMessage="Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="minisite_val"> *</asp:RequiredFieldValidator>
						</td>
					</tr>

					<tr id="Tr1" runat="server">
						<td Class="admin-prompt-right"><span class="required">*</span>French Title</td>
						<td>
							<asp:TextBox CssClass="textbox PageTitle_FR" ID="txt_TitleMini_FR" runat="server" ValidationGroup="minisite_val" Width="450"></asp:TextBox>
							<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txt_TitleMini_FR" ErrorMessage="French Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="minisite_val"> *</asp:RequiredFieldValidator>
						</td>
					</tr>                    <tr>
                        <td class="admin-prompt-right"><span class="required">*</span>URL</td>
                        <td>
                            <asp:TextBox CssClass="textbox" ID="miniseoen" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="miniseoen" ErrorMessage="URL required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="minisite_val"> *</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="miniseoen" ErrorMessage="URL invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^[a-zA-Z0-9-]+$" ValidationGroup="minisite_val"> *</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr id="frow17" runat="server">
                        <td class="admin-prompt-right"><span class="required">*</span>French URL</td>
                        <td>
                            <asp:TextBox ID="miniseofr" CssClass="textbox" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="miniseofr" ErrorMessage="French URL required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="minisite_val"> *</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="miniseofr" ErrorMessage="French URL invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^[a-zA-Z0-9-]+$" ValidationGroup="minisite_val"> *</asp:RegularExpressionValidator>
                        </td>
                    </tr>

                    <tr>
                        <td class="admin-prompt-right"><span class="required">*</span>Group's Name</td>
                        <td>
                            <asp:TextBox CssClass="textbox" ID="txtGroup" runat="server" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtGroup" ErrorMessage="Group's name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="minisite_val"> *</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="rexValidator" ControlToValidate="txtGroup" ValidationGroup="minisite_val" ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_])*" SetFocusOnError="true" ErrorMessage="No special characters allowed" Display="Dynamic"> *</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr><td colspan="2"><asp:CheckBox ID="cbPrivate" runat="server" CssClass="cb-enhanced-left" Text="Click here to make mini-site password protected" /></td></tr>
                    </table>
                    <div class="admin-buttons-wrapper">
                        <asp:LinkButton ID="ImageButton1" CausesValidation="False" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="btn_dept_back_Click" />
                        <asp:LinkButton ID="ImageButton2" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="btn_Submit_Mini_Click" ValidationGroup="minisite_val" />
                    </div>
                </div>
            </div>
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="minisite_val" ShowMessageBox="True" ShowSummary="False" />
        </div>
        <div id="tbl_Step1" runat="server" style="position:absolute; top:240px; left:70px;">
            <div class="admin-white-box">
                <div class="admin-white-box-header">Language Information</div>
                <div class="admin-white-box-inner">
                    <asp:RadioButtonList ID="RB_Language" runat="server" Enabled="False" ValidationGroup="step1" CssClass="rb-enhanced">
                        <asp:ListItem Value="1">English</asp:ListItem>
                        <asp:ListItem Value="2">French</asp:ListItem>
                        <asp:ListItem Value="-2">MultiLingual</asp:ListItem>
                    </asp:RadioButtonList>
                    <div class="adminHL-buttons-wrapper">
                        <asp:LinkButton ID="btn_Save_Temp1" runat="server" CssClass="adminHL-button-green mw150" Text="Save" onclick="btn_Save_Temp_Click" visible="False" ValidationGroup="step1" />
                        <asp:ImageButton ID="btn_Cancel_step1" runat="server" CssClass="adminHL-button-gray mw150" Text="Cancel" onclick="btn_Cancel_step1_Click" CausesValidation="False" Visible="false"/>
                        <asp:ImageButton ID="btn_Step1_Next" runat="server" CssClass="adminHL-button-green mw150" Text="Next" onclick="btn_Step1_Next_Click" ValidationGroup="step1" />
                    </div>
                </div>
            </div>
        </div>
        <div runat="server" id="tbl_Step2" style="position:absolute; top:240px; left:70px;">
            <div class="admin-white-box" style="min-width: 668px;">
                <div class="admin-white-box-header">Page Information</div>
                <div class="admin-white-box-inner">
					<table cellspacing="0" cellpadding="0" border="0">
					<tr>
						<td class="admin-prompt-right"><span class="required">*</span>Internal Name</td>
						<td>
							<asp:TextBox CssClass="textbox" ID="txt_Page_Name_EN" runat="server" ValidationGroup="step2" Width="450"></asp:TextBox>
							<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_Page_Name_EN" ErrorMessage="Internal Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr id="frow1" runat="server">
						<td Class="admin-prompt-right"><span class="required">*</span>Internal French Name</td>
						<td>
							<asp:TextBox CssClass="textbox" ID="txt_Page_Name_FR" runat="server" ValidationGroup="step2" Width="450" ></asp:TextBox>
							<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_Page_Name_FR" ErrorMessage="Internal French Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>
						</td>
					</tr>
                
						<tr>
						<td Class="admin-prompt-right"><span class="required">*</span>Title</td>
						<td>
							<asp:TextBox CssClass="textbox PageTitle_EN" ID="txt_Title_EN" runat="server" ValidationGroup="step2" Width="450" ></asp:TextBox>
							<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_Title_EN" ErrorMessage="Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr id="frow2" runat="server">
						<td Class="admin-prompt-right"><span class="required">*</span>French Title</td>
						<td>
							<asp:TextBox CssClass="textbox PageTitle_FR" ID="txt_Title_FR" runat="server" ValidationGroup="step2" Width="450"></asp:TextBox>
							<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_Title_FR" ErrorMessage="French Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
						<td class="admin-prompt-right">Keywords</td>
						<td>
							<asp:TextBox CssClass="textbox" ID="txt_Keywords_EN" runat="server" ValidationGroup="step2" Width="450"></asp:TextBox>
						</td>
					</tr>
					<tr id="frow3" runat="server">
						<td class="admin-prompt-right">French Keywords</td>
						<td>
							<asp:TextBox CssClass="textbox" ID="txt_Keywords_FR" runat="server" ValidationGroup="step2" Width="450"></asp:TextBox>
						</td>
					</tr>
                
						<tr>
						<td class="admin-prompt-right">Description</td>
						<td>
							<asp:TextBox CssClass="textbox PageDesc_EN" ID="txt_Desc_EN" runat="server" ValidationGroup="step2" Width="450"></asp:TextBox>
						</td>
					</tr>
					<tr id="frow4" runat="server">
						<td class="admin-prompt-right">French Description</td>
						<td>
							<asp:TextBox CssClass="textbox PageDesc_FR" ID="txt_Desc_FR" runat="server" ValidationGroup="step2" Width="450"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td class="admin-prompt-right"><span class="required">*</span>URL Name</td>
						<td>
                            <asp:TextBox CssClass="textbox" ID="txt_SEO_EN" runat="server" ValidationGroup="step2" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_SEO_EN" ErrorMessage="URL Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txt_SEO_EN" ErrorMessage="URL Name invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^[\w-_]+$" ValidationGroup="step2"> *</asp:RegularExpressionValidator>--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txt_SEO_EN" ErrorMessage="URL Name invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^[a-zA-Z0-9-]+$" ValidationGroup="step2"> *</asp:RegularExpressionValidator>
                        </td>
					</tr>
                    <tr id="frow5" runat="server">
						<td class="admin-prompt-right"><span class="required">*</span>French URL Name</td>
						<td>
                            <asp:TextBox CssClass="textbox" ID="txt_SEO_FR" runat="server" ValidationGroup="step2" Width="450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txt_SEO_FR" ErrorMessage="French URL Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_SEO_FR" ErrorMessage="French URL Name invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^[\w-_]+$" ValidationGroup="step2"> *</asp:RegularExpressionValidator>--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_SEO_FR" ErrorMessage="French URL Name invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^[a-zA-Z0-9-]+$" ValidationGroup="step2"> *</asp:RegularExpressionValidator>
						</td>
					</tr>
					<tr>
						<td Class="admin-prompt-right"><span class="required">*</span>Groups</td>
						<td>
							<asp:DropDownList CssClass="dropdownlist" ID="DDL_Groups" runat="server" Width="450"></asp:DropDownList>
							<asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="DDL_Groups" ErrorMessage="Groups required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2" InitialValue="select"> *</asp:RequiredFieldValidator>
						</td>
					</tr>
					<tr>
                        <td Class="admin-prompt-right"><span class="required">*</span>Template</td>
						<td>
                            <asp:DropDownList ID="DDL_Template_Select" runat="server" CssClass="dropdownlist" Width="450" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="DDL_Template_Select" ErrorMessage="Template required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2" InitialValue="select"> *</asp:RequiredFieldValidator>
                        </td>
					</tr>
					</table>
                </div>
            </div>
            <%--<br />
            <div class="admin-white-box" style="min-width: 668px;">
                <div class="admin-white-box-header">Select Template</div>
                <div class="admin-white-box-inner">
					<table cellpadding="3" cellspacing="0" border="0">
					</table>
                </div>
            </div>--%>

            <asp:Panel ID="pnlEnable" runat="server" Visible="true">
                <div class="admin-white-box">
                    <div class="admin-white-box-header">Enable Panel</div>
                    <div class="admin-white-box-inner">
                        <table border="0" cellpadding="0" cellspacing="0">
                        <tr><td class="admin-prompt-right">Start date</td><td><div class="datepicker-wraper"><asp:TextBox CssClass="dates" ID="txtStartDate" runat="server" /><asp:Image ID="imgStartDate" ImageUrl="/images/Lemonaid/buttonsNew/datepicker.png" runat="server" /><cc1:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" PopupPosition="BottomLeft" TargetControlID="txtStartDate" runat="server" PopupButtonID="imgStartDate" /><cc1:TextBoxWatermarkExtender TargetControlID="txtStartDate" WatermarkText="yyyy-MM-dd" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="txtStartDateExtender"></cc1:TextBoxWatermarkExtender></div><%--<asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Start date required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2" Enabled="false"> *</asp:RequiredFieldValidator>--%><asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="txtStartDate" Operator="DataTypeCheck" Type="Date" ErrorMessage="Start date invalid" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:CompareValidator></td></tr>
                        <tr><td class="admin-prompt-right">End date</td><td><div class="datepicker-wraper"><asp:TextBox CssClass="dates" ID="txtEndDate" runat="server" /><asp:Image ID="imgEndDate" ImageUrl="/images/Lemonaid/buttonsNew/datepicker.png" runat="server" /><cc1:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" PopupPosition="BottomLeft" TargetControlID="txtEndDate" runat="server" PopupButtonID="imgEndDate" /><cc1:TextBoxWatermarkExtender TargetControlID="txtEndDate" WatermarkText="yyyy-MM-dd" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="txtEndDateExtender"></cc1:TextBoxWatermarkExtender></div><%--<asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="End date required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2" Enabled="false"> *</asp:RequiredFieldValidator>--%><asp:CompareValidator ID="cvEndDate" runat="server" ControlToValidate="txtEndDate" Operator="DataTypeCheck" Type="Date" ErrorMessage="End date invalid" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:CompareValidator><asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtEndDate" Operator="GreaterThanEqual" Type="Date" ErrorMessage="End date must be greather than or equal to start date" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2" ControlToCompare="txtStartDate"> *</asp:CompareValidator></td></tr>
                        <tr><td class="admin-prompt-right">Start time</td><td><asp:DropDownList ID="ddlStartTime" CssClass="dropdownlist" runat="server" Width="127"></asp:DropDownList><%--<asp:RequiredFieldValidator ID="rfvStartTime" runat="server" ControlToValidate="ddlStartTime" ErrorMessage="Start time required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>--%></td></tr>
                        <tr><td class="admin-prompt-right">End time</td><td><asp:DropDownList ID="ddlEndTime" CssClass="dropdownlist" runat="server" Width="127"></asp:DropDownList><%--<asp:RequiredFieldValidator ID="rfvEndTime" runat="server" ControlToValidate="ddlEndTime" ErrorMessage="End Time required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>--%><%--<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlEndTime" Operator="GreaterThan" ErrorMessage="End time must be greather than start time" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2" ControlToCompare="ddlStartTime"> *</asp:CompareValidator>--%></td></tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>

			<asp:Panel ID="pnlReviewer" runat="server" Visible="true">
                <div class="admin-white-box" style="min-width: 668px;">
                    <div class="admin-white-box-header">Reviewer Panel</div>
                    <div class="admin-white-box-inner">
                        <table border="0" cellpadding="0" cellspacing="0">
                        <tr><td class="admin-prompt-right">Reviewer</td><td><asp:DropDownList ID="ddlReviewer" runat="server" CssClass="dropdownlist" Width="450" /></td></tr>
                        <tr><td class="admin-prompt-right">Frequency</td><td><asp:DropDownList ID="ddlFrequency" runat="server" CssClass="dropdownlist" Width="450" /></td></tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>

            <div class="admin-buttons-wrapper">
                <asp:LinkButton ID="btn_Save_Temp2" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btn_Save_Temp_Click" visible="False" ValidationGroup="step2" />
                <asp:LinkButton ID="Btn_Back_Step2" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="Btn_Back_Step2_Click" CausesValidation="False" />
                <%--<i386:ImageOverButton ID="btn_Step2_Next" runat="server" onclick="btn_Step2_Next_Click" ImageURL="/images/lemonaid/buttons/next.png" ImageOverURL="/images/lemonaid/buttons/next_over.png" ValidationGroup="step2" />--%>
                <asp:LinkButton ID="btn_Submit" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btn_Submit_Click" ValidationGroup="step2" />
                <asp:LinkButton ID="btn_Cancel_step2" runat="server" CssClass="admin-button-lightgray mw150" Text="Cancel" onclick="btn_Cancel_step1_Click" CausesValidation="False" Visible="false" />
            </div>
            <br />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="step2" ShowMessageBox="True" ShowSummary="False" />
        </div>
        <div id="tbl_step_before3" visible="false" runat="server" style="position:absolute; top:240px; left:70px;">
            <div class="admin-white-box">
                <div class="admin-white-box-header">Select Menu</div>
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Menu</td><td><asp:DropDownList ID="DDL_Menu" runat="server" Width="450" CssClass="dropdownlist" AutoPostBack="True" onselectedindexchanged="DDL_Menu_SelectedIndexChanged"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DDL_Menu" ErrorMessage="Menu required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="menu_val"> *</asp:RequiredFieldValidator></td></tr>
                    </table>
                    <div class="admin-buttons-wrapper">
                        <asp:LinkButton ID="Btn_Back_Step3_Before" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="Btn_Back_Step3_Before_Click" CausesValidation="False" />
                        <asp:LinkButton ID="btn_Step3_Next_Before" runat="server" CssClass="admin-button-green mw150" Text="Next" onclick="btn_Step3_Next_Before_Click" ValidationGroup="menu_val" />
                        <asp:LinkButton ID="btn_Cancel_step3_Before" runat="server" CssClass="admin-button-lightgray mw150" Text="Cancel" onclick="btn_Cancel_step1_Click" CausesValidation="False"  Visible="false" />
                    </div>
                </div>
            </div>
            <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="menu_val" ShowMessageBox="True" ShowSummary="False" />
        </div>
		<div id="tbl_Step3" runat="server" style="position:absolute; top:240px; left:70px;">
            <table cellpadding="4" cellspacing="0" border="0">
				<tr>
                    <td valign="top">
                        <div class="admin-white-box">
                            <div class="admin-white-box-header">Menu Placement</div>
                            <div class="admin-white-box-inner">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="300" valign="top">
					                        <div style="position:relative;top:-5px;right:10px;z-index:10;" align="right"><asp:ImageButton ID="menudelete"  Visible="false" OnClick="Btn_Delete_EN_Click" runat="server" ImageURL="/images/lemonaid/buttonsNew/delete-red.png" AlternateText="Delete" ToolTip="Delete" />&nbsp;&nbsp;<asp:ImageButton ID="menuup" Visible="false" OnClick="btn_Up_EN_Click" runat="server" ImageURL="/images/lemonaid/buttonsNew/up.png" style="height: 20px;" ToolTip="Move Up" />&nbsp;&nbsp;<asp:ImageButton ID="menudown" OnClick="Btn_Down_EN_Click" runat="server" ImageURL="/images/lemonaid/buttonsNew/down.png" style="height: 20px;" Visible="false" ToolTip="Move Down" /></div>
                   
                                            <asp:TreeView ID="TV_Menu_EN" runat="server" ShowLines="True" OnSelectedNodeChanged="selectedamenu">
                                            <HoverNodeStyle ForeColor="#434343" Font-Bold="true" />
						                    <SelectedNodeStyle ForeColor="#434343" Font-Bold="true" />
						                    <NodeStyle ForeColor="#434343" Font-Bold="false" />
                                            </asp:TreeView>
                                            <br /><span style="position:absolute;right:6px;bottom:3px;display:none;" align="right"><asp:ImageButton Visible="false" ID="ImageButton3" OnClick="Btn_Delete_EN_Click" runat="server" ImageURL="/images/Lemonaid/buttonsNew/delete-red.png"/>&nbsp;&nbsp;<asp:ImageButton ID="ImageButton4" OnClick="btn_Up_EN_Click" Visible="false" runat="server" ImageURL="/images/icons/menuup.gif"/>&nbsp;&nbsp;<asp:ImageButton ID="ImageButton5" OnClick="Btn_Down_EN_Click" Visible="false" runat="server" ImageURL="/images/icons/menudown.gif"/></span>
                                        </td>
                                    </tr>
                                    </table>
                            </div>
                        </div>
                    </td>
                    <td valign="top">
                        <div class="admin-white-box">
                            <div class="admin-white-box-inner">
                                <table border="0" cellpadding="0" cellspacing="0">
                                <tr><td class="admin-prompt-right">Name</td><td><asp:TextBox CssClass="textbox" ID="txt_Menu_EN" runat="server" ValidationGroup="step2" Width="250"></asp:TextBox></td></tr>
                                <tr id="frow6" runat="server"><td class="admin-prompt-right">French Name</td><td><asp:TextBox CssClass="textbox" ID="txt_Menu_FR" runat="server" ValidationGroup="step2" Width="250"></asp:TextBox></td></tr>
                                <tr>
                                    <td colspan="2" style="padding-top:10px;">
                                        <table width="100%" id="tbl_Step3_EN" runat="server" style="display:none;">
                                            <tr>
                                                <td><asp:ImageButton ID="btn_Up_EN" runat="server" ImageURL="/images/buttons/up_dark.gif" ValidationGroup="step3" onclick="btn_Up_EN_Click" Visible="false" /></td>
                                                <td><asp:ImageButton ID="Btn_Down_EN" runat="server" ImageURL="/images/buttons/down_dark.gif" ValidationGroup="step3" Visible="false" onclick="Btn_Down_EN_Click" /></td>
                                                <td><asp:ImageButton ID="Btn_Delete_EN" runat="server" CausesValidation="False" ImageURL="/images/buttons/delete_dark.gif" onclick="Btn_Delete_EN_Click" /></td>
                                            </tr>
                                        </table>
                                        <asp:LinkButton ID="Btn_Add_To_Menu_EN" runat="server" CssClass="admin-button-blue" Text="Add" ValidationGroup="step3" onclick="Btn_Add_To_Menu_EN_Click" Visible="false" />
                                        <asp:LinkButton ID="Btn_AddRoot_To_Menu_EN" runat="server" CssClass="admin-button-green" Text="Add to Root" ValidationGroup="step3" onclick="Btn_AddRoot_To_Menu_EN_Click" />
                                    </td>
                                </tr>
                                </table>
                                <div class="admin-buttons-wrapper">
                                    <asp:LinkButton ID="btn_Save_Temp3" runat="server" CssClass="admin-button-blue mw150" Text="Save" onclick="btn_Save_Temp_Click" ValidationGroup="step3" />
                                    <asp:LinkButton ID="Btn_Back_Step3" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="Btn_Back_Step3_Click" CausesValidation="False"/>
                                    <asp:LinkButton ID="btn_Step3_Next" runat="server" CssClass="admin-button-green mw150" Text="Next" onclick="btn_Step3_Next_Click" ValidationGroup="step3" />
                                    <asp:LinkButton ID="btn_Cancel_step3" runat="server" CssClass="admin-button-lightgray mw150" Text="Cancel" onclick="btn_Cancel_step1_Click" CausesValidation="False" Visible="false"/>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div><asp:Label ID="lbl_msg_step3A" runat="server" CssClass="alert"></asp:Label></div>
                    </td>
				</tr>
            </table>
        </div>
        <div id="tbl_Step5" runat="server" style="position:absolute; top:240px; left:70px;">
			<table cellpadding="4" cellspacing="0" border="0">
			<tr><td>Review Choices</td></tr>
			<tr><td valign="top"><table border="0" cellpadding="0" cellspacing="0">
			<tr><td><img src="/images/lemonaid/partials/menu_top.png" /></td></tr>
            <tr>
                <td valign="top"  style="background-image:url('/images/lemonaid/partials/menu_bg.png');background-repeat:repeat-y;padding-left:10px;">
					<table cellpadding="3" cellspacing="0" border="0" width="400">
							<tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075; width:120px;">
									Language</td>
								<td style="color:#ffffff; width:280px;">
									<asp:Label ID="lbl_Language" runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<td style="color:#004075;">&nbsp;
									</td>
								<td style="color:#004075; visibility:hidden;">
									<asp:ImageButton ID="LB_Step1" ForeColor="#ffbb51" runat="server" onclick="LB_Step1_Click" CssClass="admin-prompt-right" ImageUrl="/images/icons/edit.png"/>
								</td>
							</tr>
                <tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
									Internal Name</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_Page_EN" runat="server"></asp:Label>
								</td>
							</tr>
							<tr id="frow7" runat="server">
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
									French Page Name</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_Page_FR" runat="server"></asp:Label>
								</td>
							</tr>
                
								<tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
									Title</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_Title_EN" runat="server"></asp:Label>
								</td>
							</tr>
							<tr id="frow8" runat="server">
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
									French Title</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_Title_FR" runat="server"></asp:Label>
								</td>
							</tr>
                
								<tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075; vertical-align:top;">
									Keywords</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_Keywords_EN" runat="server"></asp:Label>
								</td>
							</tr>
							<tr id="frow9" runat="server">
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075; vertical-align:top">
									French Keywords</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_Keywords_FR" runat="server"></asp:Label>
								</td>
							</tr>
                
								<tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075; vertical-align:top">
									Description</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_Desc_EN" runat="server"></asp:Label>
								</td>
							</tr>
							<tr id="frow10" runat="server">
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075; vertical-align:top">
									French Description</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_Desc_FR" runat="server"></asp:Label>
								</td>
							</tr>
                
								<tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
									URL</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_SEO_EN" runat="server"></asp:Label>
								</td>
							</tr>
							<tr id="frow11" runat="server">
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
									French URL</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_SEO_FR" runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<td style="color:#004075;">
									&nbsp;&nbsp;</td>
								<td style="color:#ffffff;">
									<asp:LinkButton ID="LB_Step2" ForeColor="#ffbb51" runat="server" onclick="LB_Step2_Click" CssClass="admin-prompt-right"  Text="edit"/>
								</td>
							</tr>

							<tr>
								<td  colspan="2" style="width: 100%;color:#004075;"><table style="width: 100%" id = "tbl_Step5_menu" runat = "server" cellpadding="3" cellspacing="0" border="0">
										<tr>
											<td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
									Menu Name</td>
											<td  style="color:#ffffff;">
									<asp:Label ID="lbl_Menu_EN" runat="server"></asp:Label>
											</td>
										</tr>
										<tr id="frow12" runat="server">
											<td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
									French Menu Name</td>
											<td style="color:#ffffff;">
									<asp:Label ID="lbl_Menu_FR" runat="server"></asp:Label>
											</td>
										</tr>
										<tr>
											<td style="color:#004075;">&nbsp;
												</td>
											<td style="color:#ffffff;">
									<asp:LinkButton ID="LB_Step3" ForeColor="#ffbb51" runat="server" onclick="LB_Step3_Click" CssClass="admin-prompt-right"  Text="edit"/>
											</td>
										</tr>
									</table></td>
							</tr>
							<tr>
								<td style="color:#004075;">&nbsp;
									</td>
								<td style="color:#004075;">&nbsp;
									</td>
							</tr>
							<tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
									Template</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lbl_Template" runat="server"></asp:Label>
								</td>
							</tr>
                            <tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075; padding-top:20px;">
									Start Date</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lblStartDate" runat="server"></asp:Label>
								</td>
							</tr>
                            <tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075;">
									End Date</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lblEndDate" runat="server"></asp:Label>
								</td>
							</tr>

                            <tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075; padding-top:20px;">
									Reviewer</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lblReviewer" runat="server"></asp:Label>
								</td>
							</tr>
                            <tr>
								<td class="admin-prompt-right" style="font-weight:bold;color:#004075; ">
									Frequency</td>
								<td style="color:#ffffff;">
									<asp:Label ID="lblFreq" runat="server"></asp:Label>
								</td>
							</tr>

							<tr>
								<td style="color:#004075;">&nbsp;
									</td>
								<td style="color:#004075;">
									<asp:LinkButton ID="LB_Step4" ForeColor="#ffbb51" runat="server" onclick="LB_Step4_Click" CssClass="admin-prompt-right"  Text="edit"/>
								</td>
							</tr>
								

						</table>
                </td>
            </tr>
			<tr><td><img src="/images/lemonaid/partials/menu_bottom.png" /></td></tr>
			</table>


		<%--<br /><i386:ImageOverButton ID="Btn_Back_Step5" runat="server" onclick="Btn_Back_Step5_Click" 
                        ImageURL="/images/lemonaid/buttons/back.png" ImageOverURL="/images/lemonaid/buttons/back_over.png" CausesValidation="False" />
                    
                &nbsp;
                    <i386:ImageOverButton ID="btn_Submit" runat="server" onclick="btn_Submit_Click" 
                        ImageURL="/images/lemonaid/buttons/save.png" ImageOverURL="/images/lemonaid/buttons/save_over.png" ValidationGroup="step5" />&nbsp;&nbsp;<asp:ImageButton ID="btn_Cancel_step5" runat="server" onclick="btn_Cancel_step1_Click" 
                        ImageURL="/images/buttons/cancel.gif" CausesValidation="False" Visible="false"/>--%>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
          <div class="updateprogress">Update in progress...</div>
        </ProgressTemplate>
        </asp:UpdateProgress>
</div>

