<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PagesCore.ascx.cs" Inherits="Admin_Reports_PagesCore" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>

<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false"></asp:ScriptManager>

<div class="admin-control-wrapper">

    <asp:Panel ID="pnlFilter" runat="server">
        <div class="admin-white-box">
            <div class="admin-white-box-header">Filter</div>
            <div class="admin-white-box-inner">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="admin-prompt-right">From Date</td>
                        <td><div class="datepicker-wraper"><asp:Textbox ID="txtStartDate" runat="server" /><asp:Image ID="Img_EventDate_Start1" runat="Server" AlternateText="Click to show calendar" ImageUrl="/images/Lemonaid/buttonsNew/datepicker.png" /><ACT:CalendarExtender ID="Cal_Start_Date" runat="server" PopupButtonID="Img_EventDate_Start1" TargetControlID="txtStartDate" /></div></td>
                        <td class="admin-prompt-right" style="padding-left: 25px;">To Date</td>
                        <td><div class="datepicker-wraper"><asp:Textbox ID="txtEndDate" runat="server" /><asp:Image ID="Img_EventDate_Start2" runat="Server" AlternateText="Click to show calendar" ImageUrl="/images/Lemonaid/buttonsNew/datepicker.png" /><ACT:CalendarExtender ID="Cal_End_Date" runat="server" PopupButtonID="Img_EventDate_Start2" TargetControlID="txtEndDate" /></div></td>
                    </tr>
                    <tr>
                        <td class="admin-prompt-right">Zone</td>
                        <td><asp:DropDownList runat="server" ID="ddlZone" CssClass="dropdownlist" DataTextField="name" DataValueField="id" Width="127"></asp:DropDownList></td>
                        <td colspan="2" style="padding-left: 25px;"><asp:LinkButton ID="btnFilter" runat="server" CssClass="admin-button-green mw150" Text="Filter" onclick="filter" />&nbsp;&nbsp;<asp:LinkButton ID="btnClearFilterAllApp" runat="server" CssClass="admin-button-question" Text="clear filter?" CommandName="Clear" onclick="btnClearFilterAllApp_Click" /></td>
                    </tr>
                </table>
            </div>
        </div>   
        <br />       
        <div class="admin-white-box" id="tbl_noresults" runat="server" visible="false" style="min-width:500px;" >
           <div class="admin-white-box-inner" style="padding: 20px;">No results were found with the current filter.<br /></div>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlList" runat="server" Visible="false">        
        <div class="admin-white-box" style="min-width: 640px;">
            <div class="admin-white-box-header">
                <asp:LinkButton ID="btnExport" runat="server" CssClass="admin-button-blue" Text="Export" OnClick="btnExport_Click" />
            </div>
            <div class="admin-white-box-inner">
		        <table id="tbl_Grid" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			        <tr><td colspan="2">
                        <asp:GridView ID="GV_Main" runat="server" AllowPaging="True" CssClass="admin-grid" 
                            AllowSorting="true" AutoGenerateColumns="False" 
                            DataKeyNames="id"
                            onrowdatabound="gvMain_RowDataBound"
                            onsorting="GV_Main_Sorting" GridLines="None" CellPadding="0" CellSpacing="0">
                            <HeaderStyle CssClass="admin-grid-header" />
                            <PagerSettings Visible="false" />
                            <FooterStyle/>            
                            <Columns>
                                <asp:BoundField DataField="name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" 
                                    SortExpression="name">
                                <ItemStyle CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" Width="250"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="language_name" HeaderText="Language" HeaderStyle-HorizontalAlign="Left"  
                                    SortExpression="language_name">
                                    <ItemStyle CssClass="itemrow" Width="65" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="seo" HeaderText="SEO" SortExpression="seo" HeaderStyle-HorizontalAlign="Left" >
                                    <ItemStyle CssClass="itemrow" HorizontalAlign="Left" Width="150" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserName" HeaderText="Last Editor" HeaderStyle-HorizontalAlign="Left"  
                                    SortExpression="UserName">
                                    <ItemStyle CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Last Modification Date" HeaderStyle-HorizontalAlign="Left" SortExpression="LastUpdated">
                                    <ItemTemplate><asp:Label runat="server" ID="lblDate"></asp:Label></ItemTemplate>                    
                                    <ItemStyle CssClass="itemrow" Width="200" HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <a target="_blank" runat="server" id="lkSeo" title="View"><img src="/images/lemonaid/buttonsNew/magnify.png" border="0" alt="View"/></a>
                                    </ItemTemplate>
                                    <ItemStyle  HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td></tr>
                    <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
                </table>
            </div>
        </div>		  
    </asp:Panel>
</div>