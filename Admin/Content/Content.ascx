<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Content.ascx.cs" Inherits="Admin_Content_Content" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
<asp:UpdatePanel runat="server" ID="up1">
<ContentTemplate>
<style type="text/css">
    .TemplateGridSelectItem
    {
        background-color: #DEEFF3;
        border: 3px solid #FFCCFF;
    }
    .TemplateGridSelectItem td
    {
        color: #FF0099;
    }
</style>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Audit Trail</div>
    <div class="admin-header-subtitle">This allows you to review all edits done to your pages and restore previous versions.</div>
</div>
<div class="admin-control-wrapper">
    <%if ("Main" == CurrentView)
      {%>
        <div class="admin-white-box">
            <div class="admin-white-box-header">Filter</div>
            <div class="admin-white-box-inner">
                <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="admin-prompt-right">Text</td>
                    <td><asp:TextBox ID="txtFilter" runat="server" CssClass="textbox" OnTextChanged="filter" AutoPostBack="true" style="width:250px;"/><ACT:TextBoxWatermarkExtender TargetControlID="txtFilter" WatermarkText="content's name or username" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="TextBoxWatermarkExtender1" /></td>
                    <td style="padding-left: 25px;"><asp:LinkButton ID="btnFilter" runat="server" CssClass="admin-button-green mw150" Text="Filter" onclick="filter" /></td>
                </tr>
                </table>
            </div>
        </div>
        <br />
        <div class="admin-white-box" style="min-width: 640px;">
            <div class="admin-white-box-inner">
                <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			    <tr><td colspan="2">
                    <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id,name" AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="0" GridLines="None" onrowdatabound="gvMain_RowDataBound" 
                        OnRowEditing="GV_Main_RowEditing" OnSorting="GV_Main_Sorting" OnRowCommand="GV_Main_RowCommand">
                        <HeaderStyle CssClass="admin-grid-header" />
                        <PagerSettings Visible="false" />
                        <FooterStyle/>
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="name"
                                HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle  HorizontalAlign="Left" />
                                <ItemStyle Width="200" CssClass="itemrow"   HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Language" HeaderText="Language" SortExpression="Language">
                                <HeaderStyle  HorizontalAlign="Left" />
                                <ItemStyle Width="150" CssClass="itemrow"  HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="username" HeaderText="Last Editor" SortExpression="Username"
                                HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle CssClass="itemrow"  HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item" ToolTip="Edit" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td></tr>
                <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		        </table>
            </div>
        </div>
        <br />
    <%} %>
    <%if ("Vault" == CurrentView)
      {%>
        <div class="admin-white-box" style="min-width: 640px;">
            <div class="admin-white-box-inner">
                <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			    <tr><td colspan="2">
                    <asp:GridView ID="GV_Vault" runat="server" CssClass="admin-grid" AllowSorting="True" AutoGenerateColumns="False"
                        CellPadding="0" DataKeyNames="id"
                        OnRowEditing="GV_Vault_RowEditing" GridLines="None" OnRowCommand="GV_Vault_RowCommand"
                        AllowPaging="True" onrowdatabound="GV_Vault_RowDataBound" OnSorting="GV_Vault_Sorting">
                        <HeaderStyle CssClass="admin-grid-header" />
                        <PagerSettings Visible="false" />
                        <FooterStyle/>
                        <Columns>
                            <asp:BoundField DataField="timestamp" HeaderText="Timestamp" SortExpression="timestamp"
                                HeaderStyle-HorizontalAlign="Left"
                                DataFormatString="{0:yyyy-MM-dd}">
                                <HeaderStyle  HorizontalAlign="Left" />
                                <ItemStyle  CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="itemrow">
                                <ItemTemplate>
                                <asp:Image ImageUrl="/images/icons/l_white_12.gif" ID="imgMarker" runat="server"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="htmlcurrent">
                                <ItemStyle CssClass="itemrow" Width="150"   HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>
                             <asp:BoundField DataField="editor" HeaderText="Editor" SortExpression="editor">
                                <ItemStyle CssClass="itemrow" Width="150"   HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Select" ImageUrl="/images/lemonaid/buttons/magnify.png" CommandArgument='<%#Container.DataItemIndex %>' AlternateText="View" ToolTip="View" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
			    </td></tr>
                <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSizeVault" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChangeVault" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowingVault" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pagerVault" runat="server" OnCommand="pagerVault_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		      </table>
            </div>
        </div>
        <div>
            <asp:LinkButton ID="ibCancel" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click" />
            <%if (VaultID > -1) { %>
            <asp:LinkButton ID="ibSave" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="ibSave_Click" />
            <%} %>
        </div>
        <div class="admin-white-box" style="width: 720px;">
            <div class="admin-white-box-header">Content</div>
            <div class="admin-white-box-inner">
                <asp:Literal ID="litHTML" runat="server"></asp:Literal>
            </div>
        </div>
        <div>
            <asp:LinkButton ID="ibCancelA" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click" />
            <%if (VaultID > -1) { %>
            <asp:LinkButton ID="ibSaveA" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="ibSave_Click" /> 
            <%} %>
        </div>
    <%} %>
</div>
</ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
      <div class="updateprogress">Update in progress...</div>
    </ProgressTemplate>
    </asp:UpdateProgress>