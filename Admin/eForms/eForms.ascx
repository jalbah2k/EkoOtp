<%@ Control Language="C#" AutoEventWireup="true" CodeFile="eForms.ascx.cs" Inherits="Admin_eForms_eForms" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register Src="eFormFields.ascx" TagName="eFormFields" TagPrefix="uc1" %>
<%@ Register Src="eFormSubmissions.ascx" TagName="eSubmissions" TagPrefix="uc1" %>
<%@ Register Src="eFormDal.ascx" TagName="eFormDal" TagPrefix="uc1" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="False">
</asp:ScriptManager>
<%--<asp:UpdatePanel runat="server" ID="up1">
<ContentTemplate>--%>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">eForms</div>
    <div class="admin-header-subtitle">View and Edit details of eForms.</div>
</div>
<div class="admin-control-wrapper">
    <!-- Main Page - eForms -->
    <asp:Panel ID="pnleFormsList" runat="server">
        <div class="admin-white-box" style="min-width: 640px;">
            <div class="admin-white-box-header">
                <asp:LinkButton ID="btAdd_eForms" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="btAdd_eForms_Click" />
            </div>
            <div class="admin-white-box-inner">
                <asp:Panel ID="pnlGriedView" runat="server">
                    <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			        <tr><td colspan="2">
                        <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                            AllowPaging="true" CellPadding="0" DataKeyNames="id,Name,Title,Subtitle,Language"
                            OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" OnSorting="GV_Main_Sorting"
                            OnRowCommand="GV_Main_RowCommand" OnRowDataBound="GV_Main_RowDataBound">
                            <HeaderStyle CssClass="admin-grid-header" />
                            <PagerSettings Visible="false" />
                            <Columns>
                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle CssClass="itemrow" Width="400" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LanguageName" HeaderText="Language" SortExpression="LanguageName">
                                    <ItemStyle  CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lbSubmissions" runat="server" CommandName="Edit" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item" ToolTip="Edit" />
                                        <asp:ImageButton ID="ImageButton1b" runat="server" CommandName="Content" CausesValidation="False" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/editfields.png" AlternateText="Edit Items" ToolTip="Edit Items" />
                                        <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Submissions" CausesValidation="False" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/submissions.png" AlternateText="View Submissions" ToolTip="View Submissions" />
                                        <asp:ImageButton  OnClick="btCopy_Click" runat="server" CommandArgument='<%# Eval("id") %>' CausesValidation="False" CommandName="Copy" ImageUrl="/images/lemonaid/buttonsNew/copy.png" AlternateText="Copy" ToolTip="Copy" />
                                        <asp:ImageButton ID="lbDelete" runat="server" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete" ToolTip="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td></tr>
                    <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		            </table>
                </asp:Panel>
                <asp:Panel id="pnlListEmpty" runat="server" style="padding: 20px 0;">There are currently no eforms.<br /></asp:Panel>
            </div>
        </div>
        <br />
    </asp:Panel>
    <br />
    <!-- Create eForm -->
    <asp:Panel ID="pnlAddeForm" runat="server">
        <div class="admin-white-box">
            <div class="admin-white-box-inner">
                <table border="0" cellpadding="0" cellspacing="0">
                <tr><td  class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox ID="tbeFormName" Width="450" CssClass="textbox" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbeFormName" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="AddForm"> *</asp:RequiredFieldValidator></td></tr>
                <tr><td class="admin-prompt-right"><%--<span class="required">*</span>--%>Title</td><td><asp:TextBox CssClass="textbox" Width="450" ID="tbeFormTitle" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbeFormTitle" ErrorMessage="Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="AddForm" Enabled="false"> *</asp:RequiredFieldValidator></td></tr>
                <tr><td class="admin-prompt-right">Email</td><td><asp:TextBox CssClass="textbox" Width="450" ID="tbEmail" runat="server"></asp:TextBox>
                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbEmail" ErrorMessage="Email invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="AddForm"> *</asp:RegularExpressionValidator>--%></td></tr>
                <tr id="trLanguage" runat="server" visible="false"><td class="admin-prompt-right">Language</td><td><asp:DropDownList Width="450" ID="ddlLanguage" runat="server" CssClass="dropdownlist"></asp:DropDownList></td></tr>
                <tr><td class="admin-prompt-right">Group</td><td><asp:DropDownList ID="ddlGroup" runat="server" CssClass="dropdownlist" DataTextField="name" DataValueField="id" Width="450"></asp:DropDownList></td></tr>
                <tr><td class="admin-prompt-right">Notify?</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbEmail" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
                <tr><td class="admin-prompt-right">Popup?</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbPopup" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>               
                 <tr><td class="admin-prompt-right">Captcha</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbCaptcha" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
                </table>
            </div>
        </div>
        <br />
        <asp:LinkButton ID="ImageOverButton1" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="imgBack_Click" CausesValidation="false" />
        <asp:LinkButton ID="imgSubmit" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="imgSubmit_Click"  ValidationGroup="AddForm" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="AddForm" />
        <%--<script type="text/javascript">
		    /*
		    document.getElementById('<%=imgSubmit.ClientID%>').onclick=function(){
			    var regEmail = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/; 
			    var err='';
			    var tbeFormName= document.getElementById('<%=tbeFormName.ClientID%>')
			    var tbeFormTitle= document.getElementById('<%=tbeFormTitle.ClientID%>')
				    var cbEmail= document.getElementById('<%=cbEmail.ClientID%>')
				    var tbEmail= document.getElementById('<%=tbEmail.ClientID%>')
				    if(tbeFormName.value.length==0 )
					    err+="Form Name Required\n";				
				    if(tbeFormTitle.value.length==0 )
					    err+="Form Title Required\n";
				    if(cbEmail.checked && tbEmail.value.length==0 )
					    err+="Please provide valid email\n";
				    if(cbEmail.checked && tbEmail.value.length>0  && !regEmail.test(tbEmail.value))
					    err+="Provided email is invalid\n";
				    if (err.length>0)
					    alert(err);
				    return (err.length==0)
		    }*/
		</script>--%>
    </asp:Panel>
    <asp:Panel ID="pnlEditeForm" runat="server">
        <div class="admin-white-box">
            <div class="admin-white-box-inner">
                <table border="0" cellpadding="0" cellspacing="0">
                <tr><td  class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox ID="tbNameF" Width="450" CssClass="textbox" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbNameF" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                <tr><td class="admin-prompt-right"><%--<span class="required">*</span>--%>Title</td><td><asp:TextBox CssClass="textbox" Width="450" ID="tbTitleF" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbTitleF" ErrorMessage="Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm" Enabled="false"> *</asp:RequiredFieldValidator></td></tr>
                <tr><td class="admin-prompt-right">Email</td><td><asp:TextBox CssClass="textbox" Width="450" ID="tbEmailF" runat="server"></asp:TextBox><%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbEmailF" ErrorMessage="Email invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="EditForm"> *</asp:RegularExpressionValidator>--%></td></tr>
                <tr id="trLanguage2" runat="server" visible="false"><td class="admin-prompt-right">Language</td><td><asp:DropDownList Width="450" ID="ddlLanguageF" runat="server" CssClass="dropdownlist"></asp:DropDownList></td></tr>
                <tr><td class="admin-prompt-right">Group</td><td><asp:DropDownList ID="ddlGroupF" runat="server" CssClass="dropdownlist" DataTextField="name" DataValueField="id"  Width="450"></asp:DropDownList></td></tr>
                <tr><td class="admin-prompt-right">Notify?</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbEmailF" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
                  <tr><td class="admin-prompt-right">Popup?</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbPopupF" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>               
              <tr><td class="admin-prompt-right">Captcha</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbCaptchaF" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
                </table>
            </div>
        </div>
        <br />
        <asp:LinkButton ID="imgBackF" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="imgBackF_Click" CausesValidation="false" />
        <asp:LinkButton ID="imgSubmitF" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="imgSubmitF_Click" ValidationGroup="EditForm" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditForm" />
        <%--<script type="text/javascript">/*
            document.getElementById('divBack2').style.display = "none";
            document.getElementById('divBack3').style.display = "none";
			document.getElementById('<%=imgSubmitF.ClientID%>').onclick=function(){
			var regEmail = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/; 
			var err='';
				var cbEmailF= document.getElementById('<%=cbEmailF.ClientID%>')
				var tbEmailF= document.getElementById('<%=tbEmailF.ClientID%>')
				
				if(cbEmailF.checked && tbEmailF.value.length==0 )
					err+="Please provide valid email\n";
				if(cbEmailF.checked && tbEmailF.value.length>0 && !regEmail.test(me.value) )
					err+="Provided email is invalid\n";
				if (err.length>0)
					alert(err);
				return (err.length>0)*/
        </script>--%>
    </asp:Panel>
    <asp:Panel ID="pnleForm" runat="server">
        <div id="divBack2" style="padding-bottom: 20px;"><asp:LinkButton ID="ImageOverButton3" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="imgBack_Click" CausesValidation="false" /><asp:LinkButton ID="ibPreview" runat="server" CssClass="admin-button-green mw150" Text="Preview" CausesValidation="false" /></div>
        <uc1:eFormFields ID="eFormFields" runat="server" />
        <div id="divBack3" style="display:none;"><asp:LinkButton ID="ImageOverButton2" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="imgBack_Click" CausesValidation="false" /></div>
    </asp:Panel>
    <asp:Panel ID="pnleFormSubmission" runat="server">
        <div><asp:LinkButton ID="ImageButton3" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="imgBack_Click" CausesValidation="false" /></div>
        <uc1:eSubmissions ID="eSubmission" runat="server" />
    </asp:Panel>
</div>
<%--</ContentTemplate>
</asp:UpdatePanel>--%>
<uc1:eFormDal ID="eFormDAL" runat="server" />
