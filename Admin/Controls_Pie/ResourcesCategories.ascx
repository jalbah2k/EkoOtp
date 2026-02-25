<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResourcesCategories.ascx.cs" Inherits="Admin_Controls_Pie_ResourcesCategories" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>

<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />

<link href="../../CSS/ui.dropdownchecklist.standalone.backend.css" rel="stylesheet" type="text/css" />
<link href="../../CSS/ui.dropdownchecklist.standalone.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="/js/jquery.tablednd.0.8.min.js"></script>
<style>
    div.select-wrapper{
    position:relative;
}
span.ui-dropdownchecklist-text{
    max-width:1000px;
    overflow-x:scroll!important;
}
</style>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Resource Categories</div>
    <div class="admin-header-subtitle">Manage resource categories</div>
</div>
<div class="admin-control-wrapper">
<asp:Panel ID="pnlList" runat="server">
    <div class="admin-white-box">
        <div class="admin-white-box-header">Filter</div>
        <div class="admin-white-box-inner">
            <table border="0" cellpadding="0" cellspacing="0"><tr><td class="admin-prompt-right">Category Name</td><td style="vertical-align:top;"><asp:TextBox ID="txtFilterName" runat="server" CssClass="textbox" style="width:250px;" />
                </td><td style="padding-left: 25px;">
                    <asp:LinkButton ID="btnFilter" runat="server" CssClass="admin-button-green mw150" Text="Filter" onclick="btnFilter_Click" /><br />
                            <asp:LinkButton ID="btnClearFilter" runat="server" CssClass="adminHL-button-question" Text="clear filter?" CommandName="Clear" onclick="btnClearFilter_Click"/>

                     </td></tr></table>
        </div>
    </div>
<br />
    <div class="admin-white-box" style="min-width: 640px;">
        <div class="admin-white-box-header">
            <asp:LinkButton ID="btnMake" runat="server" CssClass="admin-button-blue" OnClick="Add" Text="Add"></asp:LinkButton>&nbsp;&nbsp;
            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="admin-button" OnClick="export" Text="Export"></asp:LinkButton>
        </div>
		<div class="admin-white-box-inner">
            <span class="admin_bodytext_white" style="font-size:18px" id="noMain" runat="server">There are currently no resource categories.<br /></span>
            <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;" id="gridarea" runat="server">
			    <tr><td colspan="2" >
                    <asp:GridView ID="gvMain" runat="server" CssClass="admin-grid"  AutoGenerateColumns="false" DataKeyNames="id" GridLines="None" CellPadding="0" CellSpacing="0" onrowdatabound="gvMain_RowDataBound" AllowSorting="True" OnSorting="dosort" AllowPaging="true">
                        <HeaderStyle CssClass="admin-grid-header" />
                        <PagerSettings Visible="false" />
                        <Columns>
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="400" HeaderText="Category" HeaderStyle-HorizontalAlign="Left" DataField="internal_name" SortExpression="internal_name" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="100" HeaderText="Type" HeaderStyle-HorizontalAlign="Left" DataField="typename" SortExpression="typename" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="100" HeaderText="Language" HeaderStyle-HorizontalAlign="Left" DataField="LanguageName" SortExpression="LanguageName" Visible="false" />
                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="editamenitiestype" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item"  Tooltip="Edit"  />
                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="deleteamenitiestype" ImageUrl="/images/lemonaid/buttonsNew/ex.png"  AlternateText="Delete" Text="" ToolTip="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView></td></tr>
                  <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><asp:ListItem Text="2 per page" Value="2" /><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
            </table>
        </div>
    </div>
    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="dropdownlist" DataTextField="name" DataValueField="id" Visible="false" />
</asp:Panel>
<asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <script type="text/javascript" src="../js/ui.dropdownchecklist-1.4-min.js"></script>

 <div class="admin-white-box" style="min-width: 640px;">
     <div class="admin-white-box-inner">

<table border="0" cellpadding="0" cellspacing="0"  style=" padding:0px;">
<tr><td>Internal Name:</td><td><asp:TextBox Width="300" CssClass="textbox" ID="txtInternalName" runat="server" /><asp:RequiredFieldValidator ID="rfvIntName" runat="server" ControlToValidate="txtInternalName" ErrorMessage='Internal Name required' SetFocusOnError="True" ValidationGroup="EditForm"></asp:RequiredFieldValidator></td></tr>
<tr><td>External Name:</td><td><asp:TextBox Width="300" CssClass="textbox" ID="txtName" runat="server" /><asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage='External Name required' SetFocusOnError="True" ValidationGroup="EditForm"></asp:RequiredFieldValidator></td></tr>


<%--<tr><td>Language:</td><td><asp:DropDownList ID="ddlLanguage" CssClass="dropdownlist" runat="server"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlLanguage" ErrorMessage='Language required>' SetFocusOnError="True" ValidationGroup="EditForm" Display="Dynamic"></asp:RequiredFieldValidator></td></tr>--%>
<tr>
        <td style="vertical-align:top; padding-top:10px">Description:&nbsp;</td>
        <td><asp:TextBox ID="tbDesc" runat="server" Width='500px' CssClass='firstname tbFullName textbox' TextMode="MultiLine" Rows="7" Height="120" ></asp:TextBox>
        </td>
    </tr> 
     <tr>
                <td>Type:</td>
                <td><asp:RadioButtonList ID="rblLibraryTypes" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal" >
                        <asp:ListItem Text="EKO" Value="1"></asp:ListItem>
                        <asp:ListItem Text="PNCA" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="rblLibraryTypes" ErrorMessage="Type required" ValidationGroup="EditForm" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>

</table>

    </div>
     </div>
<br />

<asp:LinkButton ID="ibCancel" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click" />&nbsp;&nbsp;
<asp:LinkButton ID="ibSave" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="SAVE"  ValidationGroup="EditForm"/>
    <br />

 
<asp:Panel ID="pnlSub" runat="server" DefaultButton="imgAddSubb" Visible="false">
     <div class="admin-white-box" style="min-width: 640px;">
     <div class="admin-white-box-inner">
<table border="0" cellpadding="0" cellspacing="0"  >
    <tr><td>New sub category:&nbsp;</td><td>
    <asp:TextBox Width="300" CssClass="textbox" ID="txtSubCateg" runat="server" MaxLength="49" /></td>
    <td>
        <asp:LinkButton ID="imgAddSubb" runat="server" CssClass="admin-button-blue" OnClick="imgAddSubbl_Click" Text="Add" ValidationGroup="EditForm"></asp:LinkButton>
    </td>
    </tr>
    <tr><td style="height:5px;" colspan="2"></td></tr>
    <tr><td>Sub Categories:</td><td colspan="2">
    <div class="select-wrapper">
    <asp:ListBox runat="server" ID="ddlSubcategory" SelectionMode="Multiple" CssClass="select-wrapper " DataTextField="internal_name" DataValueField="id">
            </asp:ListBox>
    </div>
</td></tr>
    <tr>
        <td></td>
        <td>
            <asp:Label runat="server" ID="litErrorSub" ForeColor="Red"></asp:Label>
            <br />
             <asp:GridView ID="gvSubCateg" runat="server" CssClass="admin-grid"  AutoGenerateColumns="false" DataKeyNames="id" GridLines="None" CellPadding="0" CellSpacing="0" AllowPaging="false" 
                 OnRowDataBound="gvSubCateg_RowDataBound1" OnRowCommand="gvSubCateg_RowCommand" >
                        <HeaderStyle CssClass="admin-grid-header" />
                        <PagerSettings Visible="false" />
                        <Columns>
                             <asp:TemplateField HeaderText="Internal Name">
                                <ItemStyle Width="400" />
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtSubInternalName" CssClass="textbox" Width="390" style="margin:0px 20px;"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" Visible="false">
                                <ItemStyle Width="500" />
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtSubName" CssClass="textbox" Width="490" style="margin:0px 20px;"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="txtID" Value='<%# Eval("id") %>' />
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" OnClick="UpdateSub" ImageUrl="/images/lemonaid/buttonsNew/note.png" AlternateText="Update Item"  Tooltip="Update"  />
                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="deletesubcat" ImageUrl="/images/lemonaid/buttonsNew/ex.png"  AlternateText="Delete" Text="" ToolTip="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
        </td>
    </tr>
</table>

    </div>
    </div>
     <script>

         $(document).ready(function () {
             $("#<%=ddlSubcategory.ClientID%>").dropdownchecklist();
         <%--$("#<%=ddlSubsubcategory.ClientID%>").dropdownchecklist();--%>
     });
     </script> 
</asp:Panel>


<%--<asp:Panel runat ="server" ID="pnlSubDetails">
     <div class="admin-white-box" style="min-width: 640px;">
     <div class="admin-white-box-inner">
  

    <asp:GridView ID="gvSubCateg" runat="server" CssClass="admin-grid" AutoGenerateColumns="false" DataKeyNames="id" GridLines="None" OnRowDataBound="gvSubCateg_RowDataBound" CellPadding="0" CellSpacing="0" AllowSorting="false" AllowPaging="false">
<PagerSettings Visible="false" />
    <HeaderStyle CssClass="admin-grid-header" />
    <Columns>
        <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="400" HeaderText="Sub Category" HeaderStyle-HorizontalAlign="Left" DataField="name" />
        <asp:BoundField  ItemStyle-CssClass="itemrow"  HeaderText="Total of Subsub Categories" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataField="qty"  />

        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" OnCommand="rowcommand_sub" CommandArgument='<%# Eval("id") %>' CommandName="editamenitiestype" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item"  Tooltip="Edit"  />
                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" OnCommand="rowcommand_sub" CommandArgument='<%# Eval("id") %>' CommandName="deleteamenitiestype" ImageUrl="/images/lemonaid/buttonsNew/ex.png"  AlternateText="Delete" Text="" ToolTip="Delete" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>                            
</asp:GridView>
               
    </div>
    </div>

<asp:Panel runat ="server" ID="pnlSubSub" Visible="false">
     <div class="admin-white-box" style="min-width: 640px;">
     <div class="admin-white-box-inner">
        
<table border="0" cellpadding="0" cellspacing="0"  style=" padding:0px;">
    <tr><td>Sub category:&nbsp;</td><td style="text-align:left;" colspan="2"><asp:Literal runat="server" ID="lblSubcategory"></asp:Literal></td></tr>

    <tr><td>New sub sub category:&nbsp;</td><td>
    <asp:TextBox Width="300" CssClass="textbox" ID="txtSubsubCateg" runat="server" MaxLength="49" /></td>
    <td>
        <asp:LinkButton ID="imgAddSubSub" runat="server" CssClass="admin-button-blue" OnClick="imgAddSubSub_Click" Text="Add" ></asp:LinkButton>
    </td>
    </tr>
    <tr><td style="vertical-align:top;">Sub sub Categories:&nbsp;</td><td colspan="2">
    <div class="select-wrapper">
    <asp:ListBox runat="server" ID="ddlSubsubcategory" SelectionMode="Multiple" CssClass="select-wrapper" DataTextField="name" DataValueField="id">
            </asp:ListBox>
    </div>
        <br />
    <asp:LinkButton ID="btnSaveSubsub" runat="server" CssClass="admin-button-green mw150" Text="Save" Visible="false"  onclick="btnSaveSubsub_Click"  />

</td></tr>

</table>

    </div>
    </div>
</asp:Panel>

</asp:Panel>--%>
</asp:Panel>
</div>
