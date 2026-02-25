<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Dash.ascx.cs" Inherits="Admin_Dash_Dash" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Dashboard</div>
    <div class="admin-header-subtitle">Welcome <asp:Literal ID="litName" runat="Server" />, here you can view the latest information about the website.</div>
</div>
<div class="admin-control-wrapper">
    <%--<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="up1">
    <ContentTemplate>--%>
    <asp:Panel ID="pnlList" runat="server">
        <div class="admin-white-box">
            <div class="admin-white-box-header">Website Information</div>
            <div class="admin-white-box-inner">
                <table class="tbl-profile-bb" style="min-width: 450px;" border="0" cellpadding="0" cellspacing="0">
                <tr><td class="admin-prompt">People viewing the website</td><td class="ar"><asp:Literal ID="lblUsers" runat="server" /></td></tr>
                <tr><td class="admin-prompt">Pages that need to be reviewed</td><td class="ar"><asp:Literal ID="Label1" runat="server" /></td></tr>
                </table>
            </div>
        </div>
        <br /><br />
        
        <asp:LinkButton ID="btnExportSearches" runat="server" CssClass="admin-button" Text="Export Searches" OnClick="ExportSearches" />
        <br /><br /><br />
        <div class="admin-white-box" style="min-width: 500px;">
            <div class="admin-white-box-header">Inbox<div style="position: absolute; top: 3px; right: 16px;"><asp:LinkButton ID="btnMake" runat="server" CssClass="admin-button-blue" Text="New Message" OnClick="newmessage"/></div></div>
            <div class="admin-white-box-inner">
                <div id="list" runat="server">
                    <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
                    <tr><td colspan="2">
                        <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="0" GridLines="None"
                            OnRowDataBound="GV_Main_RowDataBound" OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" OnSorting="dosort" PageSize="10">
                            <HeaderStyle CssClass="admin-grid-header" />
                            <PagerSettings Visible="false" />
                            <Columns>
                                <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="itemrow">
                                    <ItemTemplate><asp:Image  ID="imgStatus" runat="server" ImageUrl="/images/lemonaid/buttons/newmail.png" ToolTip='<%#DataBinder.Eval(Container.DataItem,"viewed") %>'/></ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="username" HeaderText="From" SortExpression="username">
                                    <ItemStyle Width="200" CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="subject" HeaderText="Subject" SortExpression="subject">
                                    <ItemStyle Width="200" CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="timestamp" HeaderText="Date Received" SortExpression="timestamp">
                                    <ItemStyle Width="200" CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/magnify.png" AlternateText="View" ToolTip="View" />
                                        <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete" ToolTip="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td></tr>
                    <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		            </table>
                </div>
                <div id="nolist" runat="server" style="padding: 20px 0;" visible="false">You have no messages in your inbox.</div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlView" runat="server" Visible="false">
        <div class="admin-white-box">
            <div class="admin-white-box-inner">
		        <table cellpadding="0" cellspacing="0" border="0">
                <tr><td class="admin-prompt-right">From</td><td><asp:Label ID="lblFrom" runat="server" CssClass="textbox" style="width: 450px;" /></td></tr>
                <tr><td class="admin-prompt-right" style="vertical-align:top;">Subject</td><td><asp:Label ID="lblSubject" runat="server" CssClass="textbox" style="width: 450px; height: auto; min-height: 38px;" /></td></tr>
                <tr><td class="admin-prompt-right" style="vertical-align:top;">Message</td><td><asp:Label ID="litMessage" runat="server" CssClass="textbox" style="width: 450px; height: auto; min-height: 38px;" /></td></tr>
                </table>
            </div>
        </div>
        <br />
        <asp:LinkButton ID="btnBack" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="backtolist" />
        <asp:LinkButton ID="ImageOverButton1" runat="server" CssClass="admin-button-green mw150" Text="Reply" OnClick="gotoreply" />
    </asp:Panel>
    <asp:Panel ID="pnlSend" runat="server" Visible="false">
        <div class="admin-white-box">
            <div class="admin-white-box-inner">
		        <table cellpadding="0" cellspacing="0" border="0">
                <tr><td class="admin-prompt-right">To</td><td><asp:DropDownList ID="ddlToGroup" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="updateddls" AutoPostBack="true" DataTextField="name" DataValueField="id" />&nbsp;<asp:DropDownList ID="ddlToUser" runat="server" DataTextField="username" DataValueField="id" Visible="false"/></td></tr>
                <tr><td class="admin-prompt-right">Subject</td><td><asp:Textbox ID="txtSubject" runat="server" CssClass="textbox" width="450" /></td></tr>
                <tr><td class="admin-prompt-right" style="vertical-align:top;">Message</td><td><asp:Textbox ID="txtMessage" runat="server" CssClass="textbox" width="450" Height="300" TextMode="MultiLine" /></td></tr>
                </table>
            </div>
        </div>
        <br />
        <asp:LinkButton ID="btnBack2" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="backtolist" />
        <asp:LinkButton ID="btnSend" runat="server" CssClass="admin-button-green mw150" Text="Send" OnClick="sendmessage" />
    </asp:Panel>
    <%--</ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
          <div class="updateprogress">Update in progress...</div>
        </ProgressTemplate>
        </asp:UpdateProgress>--%>
</div>