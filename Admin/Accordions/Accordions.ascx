<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Accordions.ascx.cs" Inherits="Admin_Accordions_Accordions" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>
<%--<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<script type="text/javascript" src="/js/jquery.tablednd.0.8.min.js"></script>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<style>
    #CE_ctl11_Editor1_ID_Frame{
        height:600px!important;
    }
</style>
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false">
</asp:ScriptManager>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Accordions</div>
    <div class="admin-header-subtitle">Manage accordions content</div>
</div>
<div class="admin-control-wrapper">
    <asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>
            <!-- Main Page - Galleries -->
            <asp:Panel ID="pnlGroups" runat="server">
                <div class="admin-white-box" style="min-width: 640px;">
                    <div class="admin-white-box-inner">
                       <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			            <tr><td colspan="2">
                            <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="False" AutoGenerateColumns="False"
                                CellPadding="0" GridLines="None" OnRowEditing="GV_Main_RowEditing" OnRowCommand="GV_Main_RowCommand">
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Group" 
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditGroup" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Group" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Gallery" />
                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/editfields.png" AlternateText="Edit Items" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Items" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
			            </td></tr>
		                </table>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlEditGrp" runat="server" Visible="false">
                <div class="admin-white-box">
                    <div class="admin-white-box-inner">
                        <table cellpadding="0" cellspacing="0" border="0">
                        <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox ID="txtNameGrp" runat="server" CssClass="textbox" Width="450" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNameGrp" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="Group" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator></td></tr>
                        </table>
                    </div>
                </div>
                <br />
                <asp:LinkButton ID="btnCancelGrp" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="btnCancelGrp_Click" CausesValidation="false" />
                <asp:LinkButton ID="btnSaveGrp" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="btnSaveGrp_Click" ValidationGroup="Group" />
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlSubGroup" Visible="false">
                <div class="admin-white-box" style="min-width: 640px;">
                    <div class="admin-white-box-header"><asp:Literal runat="server" ID="litTtile_Group"></asp:Literal></div>
                    <div class="admin-white-box-inner">
                       <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			            <tr><td colspan="2">
                            <asp:GridView ID="GridView2" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="false" AutoGenerateColumns="False" 
                                CellPadding="0" GridLines="None" OnRowDataBound="GridView2_RowDataBound" OnRowDeleting="GridView2_RowDeleting" OnRowEditing="GridView2_RowEditing" 
                                OnRowCommand="GridView2_RowCommand">
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name"  HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="langname" HeaderText="Language" SortExpression="langname"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                   <asp:BoundField DataField="columns" HeaderText="Columns" SortExpression="columns" 
                                        HeaderStyle-HorizontalAlign="Center">
                                    </asp:BoundField>

                                    <asp:BoundField DataField="CMSGroup" HeaderText="Group" SortExpression="CMSGroup" >
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle  CssClass="itemrow" Width="100" HorizontalAlign="Right" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    
                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditGrp" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Gallery" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Gallery" />
                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/editfields.png" AlternateText="Edit Items" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Items" />
                                            <asp:ImageButton ID="LB_Delete" runat="server"  AlternateText="Delete" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
			            </td></tr>
		                </table>
                    </div>
                    <div class="admin-white-box-header">
                        <asp:LinkButton ID="bttn_Cancel_Ggroup" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="bttn_Cancel_Ggroup_Click" />
                        <asp:LinkButton ID="btAddSubgroup" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="btAddSubgroup_Click"  />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlEdit_Subgrp" runat="server" Visible="false">
                <div class="admin-white-box">
                    <div class="admin-white-box-inner">
                        <table cellpadding="0" cellspacing="0" border="0">
                        <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox ID="tbName" runat="server" CssClass="textbox" Width="450" /><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbName" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="Subgroup" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator></td></tr>
                        <tr><td class="admin-prompt-right">Language</td><td><asp:DropDownList ID="ddlLanguage" runat="server" CssClass="dropdownlist" Width="450" /></td></tr>
                        <tr><td class="admin-prompt-right">Title</td><td><asp:TextBox ID="tbTitle" runat="server" CssClass="textbox" Width="450" /></td></tr>
                        <tr><td class="admin-prompt-right">Title French</td><td><asp:TextBox ID="tbTitleFR" runat="server" CssClass="textbox" Width="450" /></td></tr>
                        <tr><td class="admin-prompt-right">Columns</td><td style="padding-top:15px;"><asp:RadioButtonList runat="server" ID="rblColumns" RepeatDirection="Horizontal"><asp:ListItem Text="One" Value="1" Selected="True"></asp:ListItem><asp:ListItem Text="Two" Value="2"></asp:ListItem></asp:RadioButtonList></td></tr>
						<tr><td class="admin-prompt-right">Group</td><td><asp:DropDownList ID="ddlGroup" runat="server" CssClass="dropdownlist" Width="450" /></td></tr>
                        </table>
                    </div>
                </div>
                <br />
                <asp:LinkButton ID="bttn_Cancel_Subgroup" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="bttn_Cancel_Subgroup_Click" CausesValidation="false" />
                <asp:LinkButton ID="bttn_Save_Subgroup" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="bttn_Save_Subgroup_Click" ValidationGroup="Subgroup" />
            </asp:Panel>
            <asp:Panel ID="pnlList_Items" runat="server" Visible="false">
                <div class="admin-white-box">
                    <div class="admin-white-box" style="min-width: 640px;">
                        <div class="admin-white-box-header"><asp:Literal runat="server" ID="litTtile_SubGroup"></asp:Literal></div>
                        <div class="admin-white-box-inner">
                           <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			                <tr><td colspan="2">
                                <asp:GridView ID="GridViewItems" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="false" AutoGenerateColumns="False" 
                                    CellPadding="0" GridLines="None" OnRowDataBound="GridViewItems_RowDataBound" OnRowDeleting="GridViewItems_RowDeleting" OnRowCommand="GridViewItems_RowCommand">
                                    <HeaderStyle CssClass="admin-grid-header" />
                                    <PagerSettings Visible="false" />
                                    <Columns>
                                        <asp:BoundField DataField="Title" HeaderText="Title" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="langname" HeaderText="Language" SortExpression="langname"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgUP" runat="server" CommandName="Up" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/move-up.png" AlternateText="Move Up" ToolTip="Move Up" />
                                                <asp:ImageButton ID="imgDown" runat="server" CommandName="Down" CommandArgument='<%#Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/move-down.png" AlternateText="Move Down" ToolTip="Move Down" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CommandName="EditItem" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Items" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Items" />
                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="EditItems" ImageUrl="/images/lemonaid/buttonsNew/editfields.png" AlternateText="Edit Items" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Items" />
                                                <asp:ImageButton ID="LB_Delete" runat="server"  AlternateText="Delete" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
			                </td></tr>
                            <%--<tr><td style="padding-top: 10px;"><asp:DropDownList ID="DropDownList1" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="" AutoPostBack="true"><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="Literal2" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="PagerV2_1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>--%>
		                    </table>
                        </div>
                        <div class="admin-white-box-header">
                            <asp:LinkButton ID="btnBackItems" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="btnBackItems_Click" />
                            <asp:LinkButton ID="btAddItem" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="btAddItem_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlEdit_Item" runat="server" Visible="false">
                <div class="admin-white-box" style="padding:30px;">
                     <table cellpadding="0" cellspacing="0" border="0">
                        <tr><td class="admin-prompt-right"><span class="required">*</span>Title</td><td><asp:TextBox ID="txtTitle" runat="server" CssClass="textbox" Width="450" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="Item" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator></td></tr>
                        <tr><td class="admin-prompt-right">Content</td><td><CE:Editor id="Editor1" runat="server" width="1000" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor></td></tr>
                         <tr><td style="height:20px;">&nbsp;</td></tr>
                        <tr runat="server" visible="false"><td class="admin-prompt-right">Footer</td><td><CE:Editor id="Editor3" runat="server" width="1000" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor></td></tr>
                        </table>
                </div>
                 <br />
                <asp:LinkButton ID="btnBack_item" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="btnBack_item_Click" CausesValidation="false" />
                <asp:LinkButton ID="btnSave_item" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="btnSave_item_Click" ValidationGroup="Item" />
            </asp:Panel>

            <asp:Panel ID="pnlList_SubItems" runat="server" Visible="false">
                <div class="admin-white-box">
                    <div class="admin-white-box" style="min-width: 640px;">
                        <div class="admin-white-box-header"><asp:Literal runat="server" ID="litSubItem_Title"></asp:Literal></div>
                        <div class="admin-white-box-inner">
                           <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			                <tr><td colspan="2">
                                <asp:GridView ID="GridViewSubItems" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="false" AutoGenerateColumns="False" 
                                    CellPadding="0" GridLines="None" OnRowDataBound="GridViewSubItems_RowDataBound" OnRowDeleting="GridViewSubItems_RowDeleting" 
                                    OnRowCommand="GridViewSubItems_RowCommand">
                                    <HeaderStyle CssClass="admin-grid-header" />
                                    <PagerSettings Visible="false" />
                                    <Columns>
                                        <asp:BoundField DataField="Title" HeaderText="Title" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:BoundField>
                                       
                                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgUP" runat="server" CommandName="Up" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/move-up.png" AlternateText="Move Up" ToolTip="Move Up" />
                                                <asp:ImageButton ID="imgDown" runat="server" CommandName="Down" CommandArgument='<%#Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/move-down.png" AlternateText="Move Down" ToolTip="Move Down" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CommandName="EditItem" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Items" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Items" />
                                                <asp:ImageButton ID="LB_Delete" runat="server"  AlternateText="Delete" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
			                </td></tr>
                            <%--<tr><td style="padding-top: 10px;"><asp:DropDownList ID="DropDownList1" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="" AutoPostBack="true"><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="Literal2" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="PagerV2_1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>--%>
		                    </table>
                        </div>
                        <div class="admin-white-box-header">
                            <asp:LinkButton ID="btnBackToItems" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="btnBackToItems_Click" />
                            <asp:LinkButton ID="btnAddSubItem" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="btnAddSubItem_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlEdit_SubItem" runat="server" Visible="false">
                <div class="admin-white-box" style="padding:30px;">
                       <div class="admin-white-box-header">Sub-Item</div>  
                     <table cellpadding="0" cellspacing="0" border="0">
                        <tr><td class="admin-prompt-right"><span class="required">*</span>Title</td><td><asp:TextBox ID="txtSubTitle" runat="server" CssClass="textbox" Width="450" /><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTitle" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="Item" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator></td></tr>
                        <tr><td class="admin-prompt-right">Content</td><td><CE:Editor id="Editor2" runat="server" width="1000" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor></td></tr>
                        </table>
                </div>
                 <br />
                <asp:LinkButton ID="btnBack_subitem" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="btnBack_subitem_Click" CausesValidation="false" />
                <asp:LinkButton ID="btnSave_subitem" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="btnSave_subitem_Click" ValidationGroup="Item" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

