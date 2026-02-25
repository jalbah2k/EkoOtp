<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Groups.ascx.cs" Inherits="Admin_Groups_Groups" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
<%--<asp:UpdatePanel runat="server" ID="up1">
<ContentTemplate>--%>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Groups</div>
    <div class="admin-header-subtitle">Here you can create and modify groups, as well as select which admin sections they have access to.</div>
</div>
<div class="admin-control-wrapper">
    <div><asp:Label ID="lbl_msg" runat="server" CssClass="alert"></asp:Label></div>
    
    <table id="tbl_Grid" class="style3" runat ="server">
    <tr>
        <td>
            <div class="admin-white-box">
                <div class="admin-white-box-header">Filter</div>
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr><td class="admin-prompt-right">Text</td><td><asp:TextBox ID="txtFilter" runat="server" OnTextChanged="filter" AutoPostBack="true" CssClass="textbox" style="width:250px;" /><ACT:TextBoxWatermarkExtender TargetControlID="txtFilter" WatermarkText="name or email" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="TextBoxWatermarkExtender1" /></td>
                            <td style="padding-left: 25px;"><asp:LinkButton ID="ImageOverButton2" runat="server" CssClass="admin-button-green mw150" Text="Filter" onclick="filter" />
                            </td></tr>
                        <tr><td></td><td><asp:CheckBox runat="server" ID="cbRoleFilter" CssClass="cb-enhanced" Text="Role" /></td></tr>
                    </table>
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div class="admin-white-box" style="min-width: 640px;">
                <div class="admin-white-box-header">
                    <asp:LinkButton ID="ImageOverButton1" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="bttn_Add_Question_Click" />
                </div>
                <div class="admin-white-box-inner">
                    <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			        <tr><td colspan="2">
                        <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="True" PageSize="30"
                            AllowSorting="True" AutoGenerateColumns="False" CellPadding="0" CellSpacing="0"  GridLines="None"
                            onrowdatabound="gvMain_RowDataBound" OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" 
                            onsorting="GV_Main_Sorting">
                            <HeaderStyle CssClass="admin-grid-header" />
                            <PagerSettings Visible="false" />
                            <Columns>
                                <asp:BoundField DataField="name" HeaderText="Group Name" SortExpression="name">
                                    <ItemStyle CssClass="itemrow" Width="400" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="yaf_GroupName" HeaderText="Role" SortExpression="yaf_GroupName">
                                    <ItemStyle CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="color" HeaderText="Admin E-mail" SortExpression="color" Visible="false">
                                    <ItemStyle CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item"  Tooltip="Edit"  />
                                        <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png"  AlternateText="Delete" Text="" ToolTip="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
			        </td></tr>
                    <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10"/><asp:ListItem Text="30 per page" Value="30" Selected="True"/><asp:ListItem Text="100 per page" Value="100"  /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="30" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		            </table>
                </div>
            </div>
            <br />
        </td>
    </tr>
    </table>

    <div id="tbl_add_edit" runat="server">
        <div class="admin-white-box">
            <div class="admin-white-box-inner">
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox ID="txt_Name" runat="server" CssClass="textbox" Width="450"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_Name" ValidationGroup="EditForm" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True"> *</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_Name" ErrorMessage="Name invalid" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^[a-zA-Z0-9-]+$" ValidationGroup="EditForm"> *</asp:RegularExpressionValidator>
                    </td></tr>
                    <tr>
                        <td class="admin-prompt-right">Role (Forum)</td>
                        <td><asp:TextBox ID="txt_yafName" runat="server" CssClass="textbox" Width="450" Enabled="false" style="opacity:0.5;"></asp:TextBox>
                    </td></tr>
                    <tr><td class="admin-prompt-right">Description</td><td><asp:TextBox ID="txt_Description" Width="450" CssClass="textbox" runat="server"></asp:TextBox></td></tr>
                    <tr><td class="admin-prompt-right">Admin E-mail</td><td><asp:TextBox ID="txt_Color" runat="server" Width="450" CssClass="textbox"></asp:TextBox></td></tr>
                    <tr><td class="admin-prompt-right">Reviewer</td><td><asp:DropDownList ID="ddlReviewer" runat="server" CssClass="dropdownlist" Width="450" DataTextField="username" DataValueField="id"></asp:DropDownList><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlReviewer" ValidationGroup="EditForm" ErrorMessage="Reviewer required" Display="Dynamic" SetFocusOnError="True"> *</asp:RequiredFieldValidator>--%></td></tr>
                    <tr><td colspan="2"><asp:CheckBox ID="cbPrivate" runat="server" CssClass="cb-enhanced-left" Text="Click here to make mini-site password protected" /></td></tr>
                    <tr style="display:none;">
                        <td colspan="2" class="admin_bodytext_blue" style="padding-left:5px;">
                            <br /><br /><strong>Admin Menus Available</strong><br /><br />
                            <asp:DataGrid ID="dgMenu" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="false" OnItemDataBound="onitembound">
                            <Columns>
                            <asp:TemplateColumn >
						        <ItemTemplate><asp:CheckBox ID="cbVisible" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"visible")%>'/></ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="name">
						        <ItemStyle CssClass="admin_bodytext_blue" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn Visible="false">
                            <ItemTemplate><asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"id")%>'/></ItemTemplate>
                            </asp:TemplateColumn>
                            </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr runat="server" id="trSupergroup"><td colspan="2"><br /><asp:RadioButtonList runat="server" ID="rblSupergroup" DataTextField="name" DataValueField="id" RepeatDirection="Horizontal"></asp:RadioButtonList></td></tr>
                    <tr runat="server" id="trShowOnReg"><td colspan="2"><br /><asp:CheckBox ID="cbShowOnRegForm" runat="server" CssClass="cb-enhanced-left" Text="Show this group on Registration form" /></td></tr>

                </table>
            </div>
        </div>
        <br />
        <asp:LinkButton ID="btn_Cancel_step5" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btn_Cancel_step1_Click" CausesValidation="False" />
		<asp:LinkButton ID="btn_Submit" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btn_Submit_Click" ValidationGroup="EditForm" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="EditForm" ShowMessageBox="True" ShowSummary="False" />
    </div>
</div>
<%--</ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
      <div class="updateprogress">Update in progress...</div>
    </ProgressTemplate>
    </asp:UpdateProgress>--%>