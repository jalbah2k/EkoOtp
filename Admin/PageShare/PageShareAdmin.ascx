<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PageShareAdmin.ascx.cs" Inherits="Admin_PageShare_PageShareAdmin" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
<asp:UpdatePanel runat="server" ID="up1">
<ContentTemplate>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Share This Page</div>
    <div class="admin-header-subtitle">This is where you can view which pages have been shared throughout your website.</div>
</div>
<div class="admin-control-wrapper">
    <div class="admin-white-box" style="min-width: 640px;">
        <div class="admin-white-box-inner">
            <div id="tbl_noresults" runat="server" style="padding: 20px 0;">No results were found.</div>
            <table id="tbl_Grid" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
		    <tr><td colspan="2">
                <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="0" 
                    onrowdatabound="gvMain_RowDataBound" onsorting="GV_Main_Sorting" GridLines="None">
                    <HeaderStyle CssClass="admin-grid-header" />
                    <PagerSettings Visible="false" />
                    <FooterStyle/>
                    <Columns>
                        <asp:BoundField DataField="name" HeaderText="Page Name" 
                            SortExpression="name">
                        <ItemStyle CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                        <HeaderStyle HorizontalAlign="Left"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="gname" HeaderText="Group" 
                            SortExpression="gname">
                        <ItemStyle CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                        <HeaderStyle HorizontalAlign="Left"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="Counter" HeaderText="Counter" 
                            SortExpression="Counter">
                        <ItemStyle CssClass="itemrow" Width="150"  HorizontalAlign="Left" VerticalAlign="Middle" />
                        <HeaderStyle HorizontalAlign="Left"/>
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
		    </td></tr>
            <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><asp:ListItem Text="2 per page" Value="2" /><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		    </table>
        </div>
    </div>
</div>
</ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
      <div class="updateprogress">Update in progress...</div>
    </ProgressTemplate>
    </asp:UpdateProgress>