<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Alerts.ascx.cs" Inherits="Alerts" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />

<asp:ScriptManager ID="sman" runat="server" EnablePartialRendering="true" />
 <div class="admin-header-wrapper noprint">
    <div class="admin-header">Alerts</div>
    <div class="admin-header-subtitle"><asp:Literal ID="Label2" runat="server" Text="Manage Alerts"></asp:Literal></div>
</div>
<div class="admin-control-wrapper">
    <div class="admin-white-box" style="min-width: 640px;">
        <asp:Panel ID="pnlList" runat="server">
            <div class="admin-white-box-inner">
                <div id="tbl_noresults" runat="server" style="padding: 20px 0;">There are currently no popup messages.<br /></div>
                <div id="tbl_Grids" runat="server">
                    <asp:Panel ID="pnlGriedView" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
                            <tr><td colspan="2">
                                <asp:GridView ID="gvMain" runat="server" AutoGenerateColumns="false" DataKeyNames="id" GridLines="None" CellPadding="0" CellSpacing="0" onrowdatabound="gvMain_RowDataBound" 
                                        AllowSorting="false" OnSorting="dosort" AllowPaging="false" CssClass="admin-grid">
                                    <HeaderStyle CssClass="admin-grid-header" />
                                    <PagerSettings Visible="false" />
                                        <Columns>
                                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="200" HeaderText="Title" HeaderStyle-HorizontalAlign="Left" DataField="Title" SortExpression="Title" />
                                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="200" HeaderText="Language" HeaderStyle-HorizontalAlign="Left" DataField="Language" SortExpression="Language" />
                                             <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" AlternateText="Edit" title="Edit" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="editpopup" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" ToolTip="Edit"  />
                                                    <asp:ImageButton ID="ImageButton2" runat="server" Visible="false" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete" title="Delete" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="deletepopup" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                            </td></tr>
                            <tr><td style="padding-top: 10px;">
                                    <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true">
                                        <asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" />
                                </asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td>
                                <td style="text-align: right;">
                                    <cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" />
                            </td></tr>
		                </table>
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>
    </div>

<asp:Panel ID="pnlEdit" runat="server" Visible="false">
    <table cellpadding="0" cellspacing="0" border="0" style="width:100%;">
		<tr>
            <td style="vertical-align:top;">
                <div class="admin-white-box">
                    <div class="admin-white-box-header">Details</div>
                    <div class="admin-white-box-inner">
                        <table border="0" cellpadding="0" cellspacing="0" >
                            <tr><td class="admin-prompt-right">Title:</td><td><asp:TextBox Width="500" CssClass="textbox" ID="txtName" runat="server" /><asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name required" Display="None" SetFocusOnError="True" ValidationGroup="EditForm"></asp:RequiredFieldValidator></td></tr>
                            <tr><td class="admin-prompt-right">Language:</td><td>
                                <asp:DropDownList runat="server" ID="ddlLanguage" Enabled="false" CssClass="dropdownlist">
                                    <asp:ListItem Text=""></asp:ListItem>
                                    <asp:ListItem Text="English" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="French" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvLang" runat="server" ControlToValidate="ddlLanguage" ErrorMessage="Language required" Display="None" SetFocusOnError="True" ValidationGroup="EditForm"></asp:RequiredFieldValidator>
                            </td></tr>
                                <tr><td class="admin-prompt-right">Status:</td><td style="vertical-align:bottom;">
                                <asp:RadioButtonList runat="server" ID="rblStatus" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="On" Value="1" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Off" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>    
                            </td></tr>
                        </table>
                    </div>                   
                </div>
            </td>
        </tr>
    </table>

    <table cellpadding="0" cellspacing="0" border="0" style="width:100%;">
		<tr>
            <td style="vertical-align:top;">
                <div class="admin-white-box">
                    <div class="admin-white-box-header">Alert content</div>
                    <div class="admin-white-box-inner">
                        <table cellpadding="0" cellspacing="0" border="0" style="  margin:0px; padding:0px; border-collapse:collapse;">
                            <tr><td align="left" style="font-size:14px;background-color:#409ae2; padding:0px;"><CE:Editor  CssClass="bodytext" id="txtContent" AutoParseClasses="false" runat="server" Width="750" height="200"/></td></tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:LinkButton ID="ibCancel" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click1" CausesValidation="False" />
            <asp:LinkButton ID="ibSave" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="ibSave_Click" ValidationGroup="EditForm" />

<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditForm" />
</asp:Panel>
</div>