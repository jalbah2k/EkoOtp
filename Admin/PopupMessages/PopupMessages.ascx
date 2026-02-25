<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PopupMessages.ascx.cs" Inherits="Admin_PopupMessages_PopupMessages" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<style>    .cb-enhanced input{
        display: block;
    }
</style>
<asp:ScriptManager ID="sman" runat="server" EnablePartialRendering="true" />
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Manage Popup Messages</div>
    <div class="admin-header-subtitle">Here you can manage, add and delete popup messages.</div>
</div>
<div class="admin-control-wrapper">
    <asp:Panel ID="pnlList" runat="server">
        <div class="admin-white-box">
            <div class="admin-white-box-header">Filter</div>
            <div class="admin-white-box-inner">
                <table border="0" cellpadding="0" cellspacing="0">
                <tr><td class="admin-prompt-right">Text</td><td><asp:TextBox Width="300" ID="txtFilterText" CssClass="textbox" runat="server" /><ACT:TextBoxWatermarkExtender TargetControlID="txtFilterText" WatermarkText="name" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="FilterTextExtender"></ACT:TextBoxWatermarkExtender></td></tr>
                <tr><td></td><td><asp:LinkButton ID="btnFilter" runat="server" CssClass="admin-button-green mw150" Text="Filter" onclick="btnFilter_Click" /><asp:LinkButton ID="btnClearFilter" runat="server" CssClass="admin-button-question" Text="clear filter?" CommandName="Clear" onclick="btnClearFilter_Click" /></td></tr>
                </table>
            </div>
        </div>
        <br />
        <div class="admin-white-box" style="min-width: 640px;">
            <div class="admin-white-box-header">
                <asp:LinkButton ID="btnMake" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="Add" />
            </div>
            <div class="admin-white-box-inner">
                <div id="noMain" runat="server" style="padding: 20px 0;">There are currently no popup messages.<br /></div>
                <table id="gridarea" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			    <tr><td colspan="2">
                    <asp:GridView ID="gvMain" runat="server" CssClass="admin-grid" DataKeyNames="id" AutoGenerateColumns="false" GridLines="None" 
                        CellPadding="0" CellSpacing="0" AllowSorting="True" AllowPaging="true"
                        onrowdatabound="gvMain_RowDataBound" OnSorting="dosort">
                        <HeaderStyle CssClass="admin-grid-header" />
                        <PagerSettings Visible="false" />
                        <Columns>
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="500" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" DataField="name" SortExpression="name" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" HeaderText="First Time Only" HeaderStyle-HorizontalAlign="Left" DataField="firsttimeonly"  />
                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="editpopup" ToolTip="Edit" />
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="deletepopup" ToolTip="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
			    </td></tr>
                <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
                </table>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <div class="admin-white-box">
            <div class="admin-white-box-inner">
                <table border="0" cellpadding="0" cellspacing="0">
                <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox Width="600" CssClass="textbox" ID="txtName" runat="server" /><asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                <tr><td class="admin-prompt-right">Popup Content</td><td><CE:Editor id="txtContent" AutoParseClasses="false" runat="server" Width="600" height="500"/></td></tr>
                <tr><td class="admin-prompt-right">First Time Only</td><td style="padding-top: 15px;"><asp:CheckBox runat="server" ID="cbFTO" CssClass="cb-enhanced nolabel" /></td></tr>
                </table>
            </div>
        </div>
        <br />
        <asp:LinkButton ID="ibCancel" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click" CausesValidation="false" />
        <asp:LinkButton ID="ibSave" runat="server" CssClass="admin-button-green mw150" Text="Save" OnCommand="SAVE" ValidationGroup="EditForm" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditForm" />
    </asp:Panel>
</div>