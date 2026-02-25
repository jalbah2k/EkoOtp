<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Pages.ascx.cs" Inherits="Admin_Pages_Pages" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />

<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
<script type="text/javascript">

    $(document).ready(function () {
        $(".cbxActive").click(function () { return false; });
    });
</script>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Pages</div>
    <div class="admin-header-subtitle">This is a list of pages on your website, where you can edit or delete them.</div>
</div>
<div class="admin-control-wrapper">
    <asp:Panel ID="pnlList" runat="server">
        <%--<asp:UpdatePanel runat="server" ID="up1" UpdateMode="Always">
        <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="txtFilter" EventName="TextChanged"/>
              </Triggers>
        <ContentTemplate>--%>
        <div class="admin-white-box">
            <div class="admin-white-box-header">Filter</div>
            <div class="admin-white-box-inner">
                <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="admin-prompt-right">Text</td>
                    <td><asp:TextBox ID="txtFilter" runat="server" OnTextChanged="filter" AutoPostBack="true" CssClass="textbox" style="width:238px;" /><ACT:TextBoxWatermarkExtender TargetControlID="txtFilter" WatermarkText="name or seo" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="TextBoxWatermarkExtender1" /></td>
                    <td style="padding-left: 25px;"><asp:DropDownList ID="ddlGroups" runat="server" CssClass="dropdownlist" Width="200"></asp:DropDownList></td>
                    <td style="padding-left: 25px;"><asp:LinkButton ID="btnFilter" runat="server" CssClass="admin-button-green mw150" Text="Filter" onclick="filter" /></td>
                </tr>
                </table>
            </div>
        </div>
        <br />
        <div class="admin-white-box" style="min-width: 640px;">
            <div class="admin-white-box-header">
                <asp:LinkButton ID="bttn_Add_Question" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="bttn_Add_Question_Click" />
            </div>
            <div class="admin-white-box-inner">
                <div id="tbl_noresults" runat="server" style="padding: 20px 0;">No results were found with the current filter.<asp:Label ID="lbl_msg" runat="server" CssClass="alert"></asp:Label></div>
		        <table id="tbl_Grid" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			    <tr><td colspan="2">
                    <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                        onrowdatabound="gvMain_RowDataBound" OnRowDeleting="GV_Main_RowDeleting" OnRowCommand="rowcommand"
                        onsorting="GV_Main_Sorting" GridLines="None" CellPadding="0" CellSpacing="0">
                    <HeaderStyle CssClass="admin-grid-header" />
                    <PagerSettings Visible="false" />
                    <FooterStyle/>
                    <Columns>
                        <asp:BoundField DataField="EnglishName" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" SortExpression="EnglishName">
                            <ItemStyle CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" Width="250"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="gname" HeaderText="Group" HeaderStyle-HorizontalAlign="Left" SortExpression="layout_name">
                            <ItemStyle CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="language_name" HeaderText="Language" HeaderStyle-HorizontalAlign="Left" SortExpression="language_name">
                            <ItemStyle CssClass="itemrow" Width="65" HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="seo" HeaderText="SEO" SortExpression="seo" HeaderStyle-HorizontalAlign="Left" >
                            <ItemStyle CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="title" HeaderText="Title" SortExpression="title" HeaderStyle-HorizontalAlign="Left" >
                            <ItemStyle CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Active">
                            <ItemStyle CssClass="itemrow" Width="65" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbxActive" CssClass="cbxActive cb-enhanced nolabel" Text="&nbsp;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <a target="_blank" runat="server" id="lkSeo" title="View"><img src="/images/lemonaid/buttonsNew/magnify.png" border="0" alt="View"/></a>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit" CommandArgument='<%# Eval("id") %>' CommandName="managetimedcontent" CausesValidation="False" ToolTip="Edit" />
                                <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False"  AlternateText="Delete" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
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
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    </asp:Panel>

    <asp:Panel ID="pnlManageTimedContent" runat="server" Visible="false">

        <div class="admin-title" style="padding: 25px 0;">Manage Timed Content</div>
        <asp:Panel ID="pnlTCList" runat="server">
            <div class="admin-white-box" style="min-width: 640px;">
                <%--<div class="admin-white-box-header">
                    <i386:ImageOverButton ID="btnAdd" runat="server" oncommand="btnAdd_Command" CommandName="addtc" CssClass="button" ImageOverUrl="/images/lemonaid/buttons/addtimedcontent_over.png" ImageUrl="/images/lemonaid/buttons/addtimedcontent.png" />
                </div>--%>
                <div class="admin-white-box-inner">
                    <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
                    <tr><td colspan="2">
                        <asp:GridView ID="gvTimedContent" runat="server" CssClass="admin-grid" AutoGenerateColumns="false" DataKeyNames="id" GridLines="None" CellPadding="0" CellSpacing="0" onrowdatabound="gvTimedContent_RowDataBound" AllowSorting="True" OnSorting="gvTimedContent_Sorting" OnRowCommand="gvTimedContent_RowCommand" AllowPaging="true">
                        <HeaderStyle CssClass="admin-grid-header" />
                        <PagerSettings Visible="false" />
                        <Columns>
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="250" HeaderText="Content" HeaderStyle-HorizontalAlign="Left" DataField="ContentName" SortExpression="ContentName" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="150" HeaderText="Zone" HeaderStyle-HorizontalAlign="Left" DataField="ZoneName" SortExpression="ZoneName" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="70" HeaderText="Priority" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataField="Priority" SortExpression="Priority" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="100" HeaderText="Start Date" HeaderStyle-HorizontalAlign="Left" DataField="StartDate" DataFormatString="{0:yyyy-MM-dd}" SortExpression="StartDate" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="100" HeaderText="End Date" HeaderStyle-HorizontalAlign="Left" DataField="EndDate" DataFormatString="{0:yyyy-MM-dd}" SortExpression="EndDate" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="100" HeaderText="Start Time" HeaderStyle-HorizontalAlign="Left" DataField="StartTime" SortExpression="StartTime" DataFormatString="{0:h:mm tt}" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="100" HeaderText="End Time" HeaderStyle-HorizontalAlign="Left" DataField="EndTime" SortExpression="EndTime" DataFormatString="{0:h:mm tt}" />
                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibEdit" runat="server" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit" CommandArgument='<%# Eval("id") %>' CommandName="edittc" ToolTip="Edit" />
                                    <asp:HyperLink ID="hlEditContent" runat="server" ImageUrl="/images/lemonaid/buttons/editadmin.png" ToolTip="Edit content" Target="_blank"></asp:HyperLink>
                                    <asp:ImageButton ID="ibDelete" runat="server" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete" CommandArgument='<%# Eval("id") %>' CommandName="deletetc" ToolTip="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        </asp:GridView>
                    </td></tr>
                    <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSizeTC" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChangeTC" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowingTC" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager2" runat="server" OnCommand="pager2_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
                    </table>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="ibCancel" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click" />

        </asp:Panel>
    
        <asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <div class="admin-white-box">
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0">
                    <tr class="hide"><td class="admin-prompt-right">Zone</td><td><asp:DropDownList ID="ddlZones" runat="server" CssClass="dropdownlist" Width="450"></asp:DropDownList><asp:RequiredFieldValidator ID="rfvZones" runat="server" ControlToValidate="ddlZones" ErrorMessage="Zone required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                    <tr class="hide"><td class="admin-prompt-right">Priority</td><td><asp:TextBox ID="txtPriority" runat="server" CssClass="textbox" Width="450" /><asp:RequiredFieldValidator ID="rfvPriority" runat="server" ControlToValidate="txtPriority" ErrorMessage="Priority required" Display="None" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator><asp:CompareValidator ID="cvPriority" runat="server" ErrorMessage="Priority invalid" ControlToValidate="txtPriority" Operator="DataTypeCheck" Type="Integer" ValidationGroup="EditForm" SetFocusOnError="false" Display="Dynamic"> *</asp:CompareValidator></td></tr>
                    <%--<tr><td class="admin-prompt-right">&nbsp;</td><td><div style="font-size:12px;">Note: Priority must match the priority<br />of the content to be overridden</div></td></tr>--%>
                    <tr><td class="admin-prompt-right">Start date</td><td><div class="datepicker-wraper"><asp:TextBox ID="txtStartDate" runat="server" CssClass="dates" /><asp:Image ID="imgStartDate" ImageUrl="/images/Lemonaid/buttonsNew/datepicker.png" runat="server" /><ACT:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" PopupPosition="BottomLeft" TargetControlID="txtStartDate" runat="server" PopupButtonID="imgStartDate" /><ACT:TextBoxWatermarkExtender TargetControlID="txtStartDate" WatermarkText="yyyy-MM-dd" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="txtStartDateExtender"></ACT:TextBoxWatermarkExtender></div><asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Start date required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm" Enabled="false"> *</asp:RequiredFieldValidator><asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="txtStartDate" Operator="DataTypeCheck" Type="Date" ErrorMessage="Start date invalid" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:CompareValidator></td></tr>
                    <tr><td class="admin-prompt-right">End date</td><td><div class="datepicker-wraper"><asp:TextBox ID="txtEndDate" runat="server" CssClass="dates" /><asp:Image ID="imgEndDate" ImageUrl="/images/Lemonaid/buttonsNew/datepicker.png" runat="server" /><ACT:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" PopupPosition="BottomLeft" TargetControlID="txtEndDate" runat="server" PopupButtonID="imgEndDate" /><ACT:TextBoxWatermarkExtender TargetControlID="txtEndDate" WatermarkText="yyyy-MM-dd" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="txtEndDateExtender"></ACT:TextBoxWatermarkExtender></div><asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="End date required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm" Enabled="false"> *</asp:RequiredFieldValidator><asp:CompareValidator ID="cvEndDate" runat="server" ControlToValidate="txtEndDate" Operator="DataTypeCheck" Type="Date" ErrorMessage="End date invalid" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:CompareValidator><asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtEndDate" Operator="GreaterThanEqual" Type="Date" ErrorMessage="End date must be greather than or equal to start date" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm" ControlToCompare="txtStartDate"> *</asp:CompareValidator></td></tr>
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Start time</td><td><asp:DropDownList ID="ddlStartTime" runat="server" CssClass="dropdownlist" Width="127"></asp:DropDownList><asp:RequiredFieldValidator ID="rfvStartTime" runat="server" ControlToValidate="ddlStartTime" ErrorMessage="Start time required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                    <tr><td class="admin-prompt-right"><span class="required">*</span>End time</td><td><asp:DropDownList ID="ddlEndTime" runat="server" CssClass="dropdownlist" Width="127"></asp:DropDownList><asp:RequiredFieldValidator ID="rfvEndTime" runat="server" ControlToValidate="ddlEndTime" ErrorMessage="End Time required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlEndTime" Operator="GreaterThan" ErrorMessage="End time must be greather than start time" Display="None" SetFocusOnError="True" ValidationGroup="EditForm" ControlToCompare="ddlStartTime"> *</asp:CompareValidator></td></tr>
                    </table>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="ibCancelTC" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancelTC_Click" CausesValidation="false" />&nbsp;&nbsp;
            <asp:LinkButton ID="ibSaveTC" runat="server" CssClass="admin-button-green mw150" Text="Save" OnCommand="SaveTimedContent" ValidationGroup="EditForm" ToolTip="Save" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditForm" />
            <script type="text/javascript">

                $(document).ready(function () {

                    var rfvStartDate = document.getElementById('<%= rfvStartDate.ClientID %>');
                    var rfvEndDate = document.getElementById('<%= rfvEndDate.ClientID %>');

                    var EnableDatesValidators = function () {
                        var DatesRequired = ($('#<%= txtStartDate.ClientID %>').val() != '' && $('#<%= txtStartDate.ClientID %>').val() != 'yyyy-MM-dd') || ($('#<%= txtEndDate.ClientID %>').val().length != '' && $('#<%= txtEndDate.ClientID %>').val() != 'yyyy-MM-dd');
                    
                        rfvStartDate.enabled = DatesRequired;
                        ValidatorUpdateDisplay(rfvStartDate);
                        rfvEndDate.enabled = DatesRequired;
                        ValidatorUpdateDisplay(rfvEndDate);
                    }

                    $('.dates').change(function () {
                        EnableDatesValidators();
                    });
                
                    EnableDatesValidators();

                });

            </script>
        </asp:Panel>

    </asp:Panel>
</div>