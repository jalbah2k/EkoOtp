<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Languages.ascx.cs" Inherits="Admin_Languages_Languages" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sman" runat="server" EnablePartialRendering="true" />
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Manage Languages</div>
    <div class="admin-header-subtitle">Here you can manage which languages are available.</div>
</div>
<div class="admin-control-wrapper">
    <asp:Panel ID="pnlList" runat="server">
        <div class="admin-white-box" style="min-width: 640px;">
            <%--<div class="admin-white-box-header">
                <asp:LinkButton ID="btnMake" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="Add" />
            </div>--%>
            <div class="admin-white-box-inner">
                <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			    <tr><td colspan="2">
                    <asp:GridView ID="gvLanguages" runat="server" CssClass="admin-grid" AutoGenerateColumns="false" OnRowDataBound="rowbound" DataKeyNames="id" GridLines="None" CellPadding="0" CellSpacing="0">
                    <HeaderStyle CssClass="admin-grid-header" />
                    <PagerSettings Visible="false" />
                    <Columns>
                        <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="300" HeaderText="Language" HeaderStyle-HorizontalAlign="Left" DataField="name" SortExpression="name" />
                        <asp:TemplateField ItemStyle-CssClass="itemrow" HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"><ItemTemplate><asp:ImageButton ID="btnStatus" runat="server" OnCommand="rowcommand" CommandName="EnableLang" CommandArgument='<%# Eval("id") %>' ImageUrl='<%# "/images/lemonaid/buttons/" + Eval("StatusImg") + ".png" %>' ToolTip='<%# Eval("status") %>' style="width:20px;" /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="editlanguage" ToolTip="Edit" />
                                <%--<asp:ImageButton runat="server" ID="ibDelete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete" title="Delete" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="deletelanguage" ToolTip="Delete" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
                <tr><td style="padding-top: 10px;"><%--<asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><asp:ListItem Text="2 per page" Value="2" /><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList>--%><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><%--<cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" />--%></td></tr>
		        </table>
            </div>
        </div>
        <br />
    </asp:Panel>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
        <div class="admin-white-box">
            <div class="admin-white-box-inner">
                <table border="0" cellpadding="0" cellspacing="0">
                <tr class="hide"><td class="admin-prompt-right"><span class="required">*</span>ID</td><td><asp:TextBox Width="450" CssClass="textbox" ID="txtID" runat="server" /><asp:RequiredFieldValidator ID="rfvID" runat="server" ControlToValidate="txtID" ErrorMessage="ID required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator><cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="txtID" /></td></tr>
                <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox Width="450" CssClass="textbox" ID="txtName" runat="server" /><asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                <tr><td class="admin-prompt-right">Locale</td><td><asp:TextBox Width="450" CssClass="textbox" ID="txtLocale" runat="server" MaxLength="5" /><%--<asp:RequiredFieldValidator ID="rfvLocale" runat="server" ControlToValidate="txtLocale" ErrorMessage="Locale required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator>--%></td></tr>
                <tr><td class="admin-prompt-right"><span class="required">*</span>Prefix</td><td><asp:TextBox Width="450" CssClass="textbox" ID="txtPrefix" runat="server" MaxLength="4" /><asp:RequiredFieldValidator ID="rfvPrefix" runat="server" ControlToValidate="txtPrefix" ErrorMessage="Prefix required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                </table>
            </div>
        </div>
        <br />
        <asp:LinkButton ID="ibCancel" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click" CausesValidation="false" />
        <asp:LinkButton ID="ibSave" runat="server" CssClass="admin-button-green mw150" Text="Save" OnCommand="SAVE" ValidationGroup="EditForm" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="EditForm" ShowMessageBox="True" ShowSummary="False" />
    </asp:Panel>
</div>