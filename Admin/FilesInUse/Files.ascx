<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Files.ascx.cs" Inherits="Admin_FilesInUse_Files" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />

<div class="admin-header-wrapper noprint">
    <div class="admin-header">Files</div>
    <div class="admin-header-subtitle">This is a list of uploaded files.</div>
</div>
<div class="admin-control-wrapper">
    <asp:Panel ID="pnlList" runat="server">
        <div class="admin-white-box">
            <div class="admin-white-box-header">Filter</div>
            <div class="admin-white-box-inner">
                <table border="0" cellpadding="0" cellspacing="0" style="padding:0px;">
                <tr><td class="admin-prompt-right">Text</td><td><asp:DropDownList runat="server" ID="ddlFilter" CssClass="dropdownlist" AutoPostBack="True" onselectedindexchanged="ddlFilter_SelectedIndexChanged"><asp:ListItem Text="Show files not in use" Value="0"></asp:ListItem><asp:ListItem Text="Show all" Value="1"></asp:ListItem></asp:DropDownList></td></tr>
                </table>
            </div>
        </div>
        <br />
        <div class="admin-white-box" style="min-width: 640px;">
            <div class="admin-white-box-header">
                <asp:LinkButton ID="ibDownload" runat="server" CssClass="admin-button-blue" Text="Download" OnClick="ibDownload_Click" />
                <asp:LinkButton ID="ibBack" runat="server" CssClass="admin-button" Text="Back to Root" onclick="ibBack_Click" />
            </div>
            <div class="admin-white-box-inner">
                <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;" id="gridarea" runat="server">
			    <tr><td colspan="2">
                    <asp:GridView ID="gvMain" runat="server" CssClass="admin-grid" AutoGenerateColumns="false" GridLines="None" CellPadding="0" CellSpacing="0" 
                        AllowSorting="true" AllowPaging="true" onrowdatabound="gvMain_RowDataBound" onsorting="GV_Main_Sorting">
                        <HeaderStyle CssClass="admin-grid-header" />
                        <PagerSettings Visible="false" />
                        <Columns>
                            <%--<asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="320" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" DataField="FileName" SortExpression="FileName" />--%>
                            <asp:TemplateField  ItemStyle-CssClass="itemrow" ItemStyle-Width="320" HeaderText="Name" SortExpression="FileName" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litName"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="320" HeaderText="Path" HeaderStyle-HorizontalAlign="Left" DataField="Path" SortExpression="Path" />
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Used">
                                <ItemStyle CssClass="itemrow" Width="65" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="cbxActive" Enabled="false" />
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
</div>

 