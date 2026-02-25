<%@ Control Language="C#" AutoEventWireup="true" CodeFile="eFormSubmissions.ascx.cs" Inherits="Admin_eForms_eFormSubmisions" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register Src="eFormDal.ascx" TagName="eFormDal" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
<ContentTemplate>
<div style="position: relative;">
    
    <table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td colspan="2">
            <div class="admin-title" style="padding-left: 10px; display:none;">Form:&nbsp;<asp:Label ID="lbFormCaption" runat="server" /></div>
        </td>
    </tr>
    <tr>
        <td>
            <div class="admin-white-box">
            <div class="admin-white-box-header">Filter</div>
            <div class="admin-white-box-inner">
                <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="admin-prompt-right">From</td>
                    <td><div class="datepicker-wraper"><asp:TextBox ID="txtFilterStartDate" runat="server" /><asp:ImageButton ID="Img_EventDate_Start" runat="Server" AlternateText="Click to show calendar" CausesValidation="False" ImageUrl="/images/lemonaid/buttonsNew/datepicker.png" /><cc1:CalendarExtender ID="Cal_Start_Date" runat="server" Format="yyyy-MM-dd" PopupButtonID="Img_EventDate_Start" TargetControlID="txtFilterStartDate" /></div></td>
                    <td class="admin-prompt-right" style="padding-left: 15px;">To</td>
                    <td><div class="datepicker-wraper"><asp:TextBox ID="txtFilterEndDate" runat="server" /><asp:ImageButton ID="Img_EventDate_End" runat="Server" AlternateText="Click to show calendar" CausesValidation="False" ImageUrl="/images/lemonaid/buttonsNew/datepicker.png" /><cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" PopupButtonID="Img_EventDate_End" TargetControlID="txtFilterEndDate" /></div></td>
                    <td style="padding-left: 25px;"><asp:LinkButton ID="btnExport" runat="server" CssClass="admin-button-blue mw150" Text="Export" onclick="export" /></td>
                </tr>
                <tr><td colspan="5" class="admin-prompt-right" ><asp:LinkButton ID="btnClearFilter" runat="server" CssClass="adminHL-button-question" Text="clear filter?" onclick="btnClearFilter_Click"></asp:LinkButton></td></tr>    
                </table>
            </div>
        </div>
       
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top; text-align: left;">
            <div class="admin-white-box" style="min-width: 640px;">
                <div class="admin-white-box-header">Current Submission(s)</div>
                <div class="admin-white-box-inner">
                    <asp:Panel ID="pnlSubmissions" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			            <tr><td colspan="2">
                            <asp:GridView ID="gvSubmissions" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="True" AllowPaging="true" 
                                AutoGenerateColumns="False" CellPadding="0" GridLines="None"
                                OnRowDataBound="gvSubmissions_RowDataBound" OnRowCommand="gvSubmissions_RowCommand" 
                                onrowdeleting="gvSubmissions_RowDeleting" OnSorting="gvSubmissions_Sorting">
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                    <asp:BoundField DataField="SubmitDate" HeaderText="Submission Date" SortExpression="SubmitDate" DataFormatString="{0:MMMM d, yyyy}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle CssClass="itemrow" Width="400" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lbView" runat="server"  AlternateText="View" CommandName="Select" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/magnify.png" ToolTip="View Details" />
                                            <asp:ImageButton ID="lbArchive" runat="server"  AlternateText="archive" CommandName="archive" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/archive.png" ToolTip="Archive" />
                                            <asp:ImageButton ID="lbDelete" runat="server"  AlternateText="Delete" CommandName="Delete" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td></tr>
                        <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		                </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlNoSubmissions" runat="server" style="padding: 20px 0;" Visible="false">No Submissions.<br /></asp:Panel>
                </div>
            </div>
            <br />
            <div class="admin-white-box" style="min-width: 640px;">
                <div class="admin-white-box-header">Archived Submission(s)</div>
                <div class="admin-white-box-inner">
                    <asp:Panel ID="pnlArchive" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			            <tr><td colspan="2">
                            <asp:GridView ID="gvArchive" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="True" AllowPaging="true" 
                                AutoGenerateColumns="False" CellPadding="0" GridLines="None"
                                OnRowDataBound="gvSubmissions_RowDataBound" OnRowCommand="gvSubmissions_RowCommand" 
                                onrowdeleting="gvSubmissions_RowDeleting" OnSorting="gvSubmissions_Sorting">
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                    <asp:BoundField DataField="SubmitDate" HeaderText="Submission Date" SortExpression="SubmitDate" DataFormatString="{0:MMMM d, yyyy}">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle CssClass="itemrow" Width="400" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lbView" runat="server"  AlternateText="View" CommandName="Select" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/magnify.png" ToolTip="View Details" />
                                            <asp:ImageButton ID="lbArchive" runat="server"  AlternateText="unarchive" CommandName="unarchive" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/unarchive.png" ToolTip="Unarchive" />
                                            <asp:ImageButton ID="lbDelete" runat="server"  AlternateText="Delete" CommandName="Delete" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td></tr>
                        <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSizeArc" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChangeArc" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowingArc" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pagerArc" runat="server" OnCommand="pagerArc_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		                </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlNoArchive" runat="server" style="padding: 20px 0;" Visible="false">No Archive Submissions.<br /></asp:Panel>
                </div>
            </div>
        </td>
        <td style="vertical-align: top; padding-left: 25px;">
            <asp:Panel ID="pnlSubmission" runat="server">
                <div class="admin-white-box" style="min-width: 640px;">
                    <div class="admin-white-box-header">Submission Details</div>
                    <div class="admin-white-box-inner">
                        <asp:GridView ID="gvSubmission" runat="server" CssClass="admin-grid" AutoGenerateColumns="False" CellPadding="0" GridLines="None">
                            <HeaderStyle CssClass="admin-grid-header" />
                            <PagerSettings Visible="false" />
                            <Columns>
                                <asp:TemplateField HeaderText="Caption">
                                    <ItemStyle CssClass="itemrow" Width="250" />
                                    <ItemTemplate><%# Eval("Caption") %>:</ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value">
                                    <ItemStyle CssClass="itemrow" Width="200" />
                                    <ItemTemplate><%# Eval("Value")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </td>
    </tr>
    </table>
</div>
</ContentTemplate>
</asp:UpdatePanel>
<uc1:eFormDal ID="eDAL" runat="server" />
