<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Redirect.ascx.cs" Inherits="Admin_Groups_Groups" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<%--<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
<asp:UpdatePanel runat="server" ID="up1">
<ContentTemplate>--%>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Redirect URLs</div>
    <div class="admin-header-subtitle">Here you can create and modify the redirect URLs you website handles.</div>
</div>
<div class="admin-control-wrapper">
    <table cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td align="left">
                <asp:Label ID="lbl_msg" runat="server" CssClass="alert"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table id="tbl_Grid" runat="server">
                <tr>
                    <td id="tabletop" runat="server">
                        <div class="admin-white-box" style="min-width: 640px;">
                            <div class="admin-white-box-header">
                                <asp:LinkButton ID="bttn_Add_Question" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="bttn_Add_Question_Click" />
                            </div>
                            <div class="admin-white-box-inner">
                                <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			                    <tr><td colspan="2">
                                    <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="True" 
                                        AllowSorting="True" AutoGenerateColumns="False" CellPadding="0" 
                                        onrowdatabound="gvMain_RowDataBound" OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" 
                                        onsorting="GV_Main_Sorting" GridLines="None" >
                                        <HeaderStyle CssClass="admin-grid-header" />
                                        <PagerSettings Visible="false" />
                                        <FooterStyle/>
                                        <Columns>
                                            <asp:BoundField DataField="oldseo" HeaderText="Link" SortExpression="oldseo">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200"  CssClass="itemrow" />
                                                <HeaderStyle HorizontalAlign="Left"/>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="newseo" HeaderText="URL" SortExpression="newseo">
                                                <ItemStyle HorizontalAlign="Left"  Width="300" VerticalAlign="Middle"  CssClass="itemrow" />
                                                <HeaderStyle HorizontalAlign="Left"/>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png"  AlternateText="Edit Item" ToolTip="Edit"  />
                                                    <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete" ToolTip="Delete" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
			                    </td></tr>
                                <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		                        </table>
                            </div>
                        </div>
                    </td>
                </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="tbl_add_edit" runat="server">
                    <asp:LinkButton ID="ImageButton6" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btn_Cancel_step1_Click" CausesValidation="False" Visible="false" />
                    <asp:LinkButton ID="ImageButton5" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btn_Submit_Click" ValidationGroup="EditForm" Visible="false" />
                    <div class="admin-white-box">
                        <div class="admin-white-box-inner">
                            <table cellspacing="0" cellpadding="0" border="0">
                            <tr><td class="admin-prompt-right"><span class="required">*</span>Link</td><td><asp:TextBox ID="txt_Name" runat="server" CssClass="textbox" Width="450"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_Name" ErrorMessage="Link required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                            <tr><td class="admin-prompt-right">URL</td><td><asp:TextBox ID="newseo" runat="server" Text="https://" CssClass="textbox" Width="450" /></td></tr>
                            </table>
                        </div>
                    </div>
                    <br />
                    <asp:LinkButton ID="btn_Cancel_step5" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btn_Cancel_step1_Click" CausesValidation="False" />
                    <asp:LinkButton ID="btn_Submit" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btn_Submit_Click" ValidationGroup="EditForm" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="EditForm" ShowMessageBox="True" ShowSummary="False" />
                    
                </div>
            </td>
        </tr>
    </table>
</div>
<%--</ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
      <div class="updateprogress">Update in progress...</div>
    </ProgressTemplate>
    </asp:UpdateProgress>--%>
