<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Resources.ascx.cs" Inherits="Admin_Controls_Resources" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%--<%@ Register assembly="skmValidators" namespace="skmValidators" tagprefix="skm" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Admin/Controls_Pie/LibrariesPartial.ascx" TagPrefix="uc1" TagName="LibrariesPartial" %>




<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />

<link href="../../CSS/ui.dropdownchecklist.standalone.backend.css" rel="stylesheet" type="text/css" />
<link href="../../CSS/ui.dropdownchecklist.standalone.css" rel="stylesheet" type="text/css" />
<link type="text/css" href="../css/chilAdmin.css" rel='Stylesheet'  />
<link type="text/css" href="../../css/chilAdmin.css" rel='Stylesheet'  />
<%--<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false"></asp:ScriptManager>--%>
<link href="/CSS/Admin.css" rel="stylesheet" type="text/css" />
<link href="/Admin/Controls_Pie/resources.css" rel="stylesheet" type="text/css" />
<style type="text/css">
th.leftPadding, td.leftPadding{ padding-left:10px}
.currentwidth{width:700px}
.newpagerstyle{
	font-size:14px;
	color:#ffbb51;
	font-family:Arial;
	font-weight:bold;
	background-image:url('../images/lemonaid/partials/shadow.png');
	background-repeat:repeat-x;
	background-position:5px 0px;
	display:none;
	text-decoration:none;
}
div.empty{ margin-top:20px}
.hide {display: none;}
.show {display: block;}
div.select-wrapper{
    position:relative;
}
span.ui-dropdownchecklist-text{
    max-width:1000px;
    overflow-x:scroll!important;
}

.divCateg-Plus div{
    display:inline-block;
}
#ctl03_CategoryPartial3_pnlPlus{
    display:none!important;
}
.tbLibrary{
    opacity:0.8;
}
strong{
    font-weight:800;
}
.pnlDetails table tr td:first-child{
    width:155px;
}

</style>

<div class="admin-header-wrapper noprint">
    <div class="admin-header">Resource Library</div>
    <div class="admin-header-subtitle">Manage resource library</div>
</div>
<div class="admin-control-wrapper">
  

<asp:Panel ID="pnlResourcesGroups" runat="server">
    <div class="admin-white-box" style="min-width: 640px;">
        <div class="admin-white-box-header">
            <asp:LinkButton ID="btnMake" runat="server" CssClass="admin-button-blue" OnClick="Add" Text="Add Libray"></asp:LinkButton>&nbsp;&nbsp;
        </div>
		<div class="admin-white-box-inner">
        <span class="admin_bodytext_white" style="font-size:18px" id="noMain" runat="server">There are currently no resource libraries.<br /></span>
            <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;" id="gridarea" runat="server">
			    <tr><td colspan="2" >
                    <asp:GridView ID="gvMainRG" runat="server" CssClass="admin-grid" AutoGenerateColumns="false" DataKeyNames="id" GridLines="None" CellPadding="0" CellSpacing="0" onrowdatabound="gvMainRG_RowDataBound" AllowSorting="True" OnSorting="dosort" AllowPaging="true" >
                        <HeaderStyle CssClass="admin-grid-header" />
                        <PagerSettings Visible="false" />
                        <Columns>
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="400" HeaderText="Resource Library" HeaderStyle-HorizontalAlign="Left" DataField="name" SortExpression="name" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="100" HeaderText="Type" HeaderStyle-HorizontalAlign="Left" DataField="typename" SortExpression="typename" />
                            <asp:BoundField  ItemStyle-CssClass="itemrow" ItemStyle-Width="100" HeaderText="Language" HeaderStyle-HorizontalAlign="Left" DataField="LanguageName" SortExpression="LanguageName" Visible="false" />
                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="editresourcesgroup" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit"  Tooltip="Edit"  />
                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="editresources" ImageUrl="/images/lemonaid/buttonsNew/editfields.png" AlternateText="Edit Item"  Tooltip="Edit Item"  />
                                    <asp:ImageButton ID="btnDeleteResourceGroup" runat="server" CausesValidation="False" OnCommand="rowcommand" CommandArgument='<%# Eval("id") %>' CommandName="deleteresourcesgroup" ImageUrl="/images/lemonaid/buttonsNew/ex.png"  AlternateText="Delete" Text="" ToolTip="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView></td></tr>
                  <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><asp:ListItem Text="2 per page" Value="2" /><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="PagerV2_1" runat="server" OnCommand="pager_CommandRG" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
            </table>
        </div>
    </div>
</asp:Panel>
<!-- Add Gallery Page -->
<asp:Panel ID="pnlAddResourcesGroups" runat="server" Visible="false">
    <div>
        <asp:LinkButton ID="ibCancel2" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click" CausesValidation="false" />&nbsp;&nbsp;
        <asp:LinkButton ID="ibSave2" runat="server" CssClass="admin-button-green mw150" Text="Save" OnCommand="SAVE"  ValidationGroup="EditForm"/>
    </div>
    <div class="admin-white-box" >
		<div class="admin-white-box-inner">
        <table cellpadding="0" cellspacing="0" border="0" style=" padding:0px;">
            <tr>
                <td style="width:80px">Name:</td>
                <td>
                <asp:TextBox Width="200" ID="tbGalleryName" runat="server" CssClass="textbox" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbGalleryName" ErrorMessage='Name required' SetFocusOnError="True" ValidationGroup="EditForm" ForeColor="Red"/>
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top; padding-top:10px">Description:&nbsp;</td>
                <td><asp:TextBox ID="tbGalleryDesc" runat="server" Width='500px' CssClass='firstname tbFullName textbox' TextMode="MultiLine" Rows="7" Height="120" ></asp:TextBox>
                </td>
            </tr> 
            <%--<tr>
                <td>Language:</td>
                <td>
                   <asp:DropDownList ID="ddlGalleryLanguage" CssClass="dropdownlist" Width="200" runat="server" AutoPostBack="true" onselectedindexchanged="ddlGalleryLanguage_SelectedIndexChanged"/>
                </td>
            </tr>--%>
            <tr>
                <td>Group:</td>
                <td><asp:DropDownList ID="ddlGroup" runat="server" CssClass="dropdownlist" DataTextField="name" DataValueField="id" Width="200" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlGroup" ErrorMessage='Group required' SetFocusOnError="True" ValidationGroup="EditForm" ForeColor="Red"/>
                </td>
            </tr>
            <tr>
                <td>Type:</td>
                <td><asp:RadioButtonList ID="rblLibraryTypes" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblLibraryTypes_SelectedIndexChanged" >
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
        <div class="admin-white-box" runat="server" id="pnl_categ">
		<div class="admin-white-box-inner">
		
        <asp:CheckBoxList ID="cblCategories" runat="server" CellPadding="3" CssClass="cb-enhanced"></asp:CheckBoxList>        
        
    </div>
    </div>

    <br />
    <asp:LinkButton ID="ibCancel" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click" CausesValidation="false" />&nbsp;&nbsp;
    <asp:LinkButton ID="ibSave" runat="server" CssClass="admin-button-green mw150" Text="Save" OnCommand="SAVE"  ValidationGroup="EditForm"/>

</asp:Panel>

<asp:Panel ID="pnlMain" runat="server" Visible="false">

<asp:Panel ID="pnlFilter" runat="server" DefaultButton="ImageOverButton2"> 
             <asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>

    <div class="admin-white-box" style="min-width: 640px;">
        <div class="admin-white-box-header"><asp:Literal runat="server" ID="litLibraryName"></asp:Literal></div>

        <div class="admin-white-box-header">
            <table style="width:600px;" border="0">
    
    
                <tr><td style=" width:40%;">
                    <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="True"  CssClass="dropdownlist"
                        DataValueField="id" DataTextField="status" 
                        onselectedindexchanged="ddlFilter_SelectedIndexChanged"></asp:DropDownList>
                        </td><td class="hide" style="padding-left:20px;">
                        <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="True"  CssClass="dropdownlist"         
                        onselectedindexchanged="ddlFilter_SelectedIndexChanged">
                            <asp:ListItem Text="English" Value="1"></asp:ListItem>
                            <asp:ListItem Text="French" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                </td>
                <td><asp:DropDownList runat="server" ID="ddlCategoryFilter"  CssClass="dropdownlist" DataTextField="name" DataValueField="id" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoryFilter_SelectedIndexChanged"></asp:DropDownList></td>
                <td style="width:80%; text-align:right;">
            
                    <tr><td>Filter :&nbsp;</td><td><asp:TextBox ID="txtFilter" runat="server" Width="100%" CssClass="textbox" /></td>
        <td style="text-align:center;">
            <asp:LinkButton ID="ImageOverButton2" runat="server" CssClass="admin-button-green mw150" Text="Run Filter" onclick="filter" /><br />
                            <asp:LinkButton ID="btnClearFilter" runat="server" CssClass="adminHL-button-question" Text="clear filter?" CommandName="Clear" onclick="btnClearFilter_Click"/>
                    
                </td></tr></table>
        </div>
    </div>
    <br />
    
    
     <div class="admin-white-box" style="min-width: 640px;">
        <div class="admin-white-box-header">

            <table border="0" cellpadding="0" cellspacing="0"  style=" padding:0px; width:100%;">
            <tr><td >Start date :&nbsp;</td><td>
                           <asp:TextBox Width="100" CssClass="textbox dates" ID="txtStartDate" runat="server" /><asp:Image ID="imgStartDate" ImageUrl="/images/icons/datepicker.gif" runat="server" /><cc1:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" PopupPosition="BottomLeft" TargetControlID="txtStartDate" runat="server" PopupButtonID="imgStartDate" /><cc1:TextBoxWatermarkExtender TargetControlID="txtStartDate" WatermarkText="yyyy-MM-dd" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="txtStartDateExtender"></cc1:TextBoxWatermarkExtender><asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="txtStartDate" Operator="DataTypeCheck" Type="Date" ErrorMessage="Start date invalid" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"></asp:CompareValidator>           
                           </td>
            <td style="padding-left:25px;">End date :</td><td>
                           <asp:TextBox Width="100" CssClass="textbox dates" ID="txtEndDate" runat="server" /><asp:Image ID="imgEndDate" ImageUrl="/images/icons/datepicker.gif" runat="server" /><cc1:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" PopupPosition="BottomLeft" TargetControlID="txtEndDate" runat="server" PopupButtonID="imgEndDate" /><cc1:TextBoxWatermarkExtender TargetControlID="txtEndDate" WatermarkText="yyyy-MM-dd" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="txtEndDateExtender"></cc1:TextBoxWatermarkExtender><asp:CompareValidator ID="cvEndDate" runat="server" ControlToValidate="txtEndDate" Operator="DataTypeCheck" Type="Date" ErrorMessage="End date invalid" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"></asp:CompareValidator>
            </td><td class="admin_bodytext_blue" style="text-align:right;">
                <asp:LinkButton ID="lbtnDownloadAll" runat="server" CssClass="admin-button-blue" OnClick="lbtnDownloadAll_Click" Text="Export"></asp:LinkButton>     
            </td></tr>
                <tr>
                    <td colspan="6" style="text-align:right;">
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="adminHL-button-question" Text="clear dates" CommandName="Clear" onclick="btnClear_Click"/>
                    </td>
                </tr>

            </table>
        </div>
    </div>

                <div id="tbl_noresults" runat="server" class="admin_bodytext_white">No results were found with the current filter.<asp:Label ID="lbl_msg" runat="server" CssClass="alert"></asp:Label></div>         
        </asp:Panel>

<asp:Panel id="pnlGridView"  runat="server">

    
     <div class="admin-white-box" style="min-width: 640px;">
        <div class="admin-white-box-header">       
            <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
                <tr>
                    <td style="text-align:left;">
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click" CausesValidation="false" />&nbsp;&nbsp;

                    </td>
                    <td style="text-align:right;">                 
                        <asp:LinkButton ID="btnAdd" runat="server" CssClass="admin-button-blue" OnClick="btnAdd_Click" Text="Add Resource"></asp:LinkButton>
                    </td>
                </tr>
                <tr><td colspan="2">       
                <asp:GridView ID="gvMain" runat="server" AllowSorting="True" AutoGenerateColumns="False" CssClass="admin-grid"
        AllowPaging="true" CellPadding="0" DataKeyNames="id" GridLines="None" Width="100%"
        OnRowDataBound="gvMain_RowDataBound" OnPageIndexChanging="gvMain_PageIndexChanging"
        OnRowCommand="gvMain_RowCommand" OnRowEditing="gvMain_RowEditing" PageSize="10" 
                    onsorting="gvMain_Sorting">
            <HeaderStyle CssClass="admin-grid-header" />
            <PagerSettings Visible="false" />
            <Columns>
                 <asp:TemplateField ShowHeader="false" HeaderText="" > 
                    <HeaderStyle  />
                    <ItemStyle   />
                    <ItemTemplate><asp:ImageButton ID="imStatus" runat="server" CommandName="status" ImageUrl="~/images/icons/pending.png" OnClick="btStatus_Click" 
                    CommandArgument='<%# Eval("id") %>' ToolTip="Pending" />
                    
                    <asp:Panel ID='ibStatus' runat="server"><img src='../images/icons/pending.png' alt='Pending' title='Pending' class='pending' style='cursor:pointer;'/><div style='position:relative'><div 
                    style='position: absolute; background-image:url(../images/icons/transpopLeft.png); background-repeat:no-repeat;
                    width:244px; height:118px; left:26px; top:-74px; display:none'><div style="margin:20px 0 0 50px">
                    <div><asp:ImageButton ID="ImageButton2" runat="server" CommandName="approve" ImageUrl="~/images/icons/transapp.png"
                            OnClick="btStatus_Click" CommandArgument='<%# Eval("id") %>' ToolTip="approve" /></div>
                    <div><asp:ImageButton ID="ImageButton3" runat="server" CommandName="decline" ImageUrl="~/images/icons/transdec.png"
                            OnClick="btStatus_Click" CommandArgument='<%# Eval("id") %>' ToolTip="decline" /></div>
                    </div></div></div></asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="Title" SortExpression="Title"> 
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" CssClass="itemrow" Width="500"  />
                    <ItemTemplate>
                        <asp:Literal ID="ltTitle" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Category" HeaderStyle-HorizontalAlign="left" Visible="false">
                    <ItemStyle CssClass="itemrow" Width="200" HorizontalAlign="left" />
                    <ItemTemplate>
                        <asp:Literal ID="ltType" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Show" HeaderStyle-HorizontalAlign="Right" Visible="false">
                    <ItemStyle CssClass="itemrow" Width="150" HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Literal ID="ltShow" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Right" Visible="false">
                    <ItemStyle CssClass="itemrow" Width="150" HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Literal ID="ltStatus" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>

                        <asp:ImageButton ID="imgUP" runat="server" AlternateText="Move Up" CommandName="Up"
                                    ImageUrl="/images/lemonaid/buttonsNew/move-up.png" CommandArgument='<%# Eval("id") %>' />
                        <asp:ImageButton ID="imgDown" runat="server" CommandArgument='<%#Eval("id") %>'
                                    CommandName="Down" ImageUrl="/images/lemonaid/buttonsNew/move-down.png" />
                                                
                        <asp:ImageButton ID="btView" runat="server" CommandName="View" CommandArgument='<%#Eval("id") %>' ImageUrl="~/images/lemonaid/buttonsNew/view.png" ToolTip="View Details" Visible="false"  />
                        <asp:ImageButton ID="btEdit" runat="server" CommandName="Edit" CommandArgument='<%#Eval("id") %>' ImageUrl="~/images/lemonaid/buttonsNew/pencil.png" ToolTip="Edit"/>
                        <asp:ImageButton ID="btDelete" runat="server" CommandName="Remove" CommandArgument='<%#Eval("id") %>' ImageUrl="~/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView></td></tr>
                <tr><td style="padding-top: 10px;">
                    <asp:DropDownList ID="ddlPageSize2" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange2" AutoPostBack="true">
                        <asp:ListItem Text="2 per page" Value="2" /><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="Literal1" runat="server" /></span>
                    </td>
                    <td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		    </table>
            <asp:LinkButton ID="btnBackToResourceLibrary" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="ibCancel_Click" CausesValidation="false" />&nbsp;&nbsp;
        </div>
    </div>
            <script type="text/javascript" src="../js/admin/chilstatus.js"></script>
              <%--</ContentTemplate><Triggers><asp:AsyncPostBackTrigger ControlID="ddlFilter" EventName="SelectedIndexChanged"  /> </Triggers>
</asp:UpdatePanel>--%> </asp:Panel>
<asp:Panel id="pnlListEmpty"  runat="server" CssClass="admin_title_blue empty" Visible="false">Current list is empty</asp:Panel>
</asp:Panel>
<asp:Panel id="pnlDetails"  runat="server" CssClass="admin_title_blue pnlDetails" Visible="false" DefaultButton="btnSave">
            <script type="text/javascript" src="../js/ui.dropdownchecklist-1.4-min.js"></script>

<%if ( Request.ServerVariables["HTTP_USER_AGENT"].ToString().ToLower().Contains("firefox")
        )
    { %>

<style>
.div-Rep .plus:after {

    background-image: url('/images/icons/plus.png');
    background-repeat: no-repeat;
    background-size: 9px 9px;
    display: inline-block;
    width: 9px;
    height: 9px;
    content: "";
    position: absolute;
    right: -12px;
    top: 0px;
}

.div-Rep .minus:after {
    background-image: url('/images/icons/minus.png');
    background-repeat: no-repeat;
    background-size: 9px 9px;
    display: inline-block;
    width: 9px;
    height: 9px;
    content: "";
    position: absolute;
    right: -12px;
    top: 6px;
}
</style>
<%} %>
<script type="text/javascript">
   

    function getCheckedBoxes(chkboxClass) {
        var checkboxes = document.getElementsByClassName(chkboxClass);
        var checked = [];
        for (var i = 0; i < checkboxes.length; i++) {

            if (checkboxes[i].firstChild.checked) {
                checked.push(checkboxes[i].value)
            }
        }

        // Return the array if it is non-empty, or null
        return checked.length > 0 ? checked : null;
    }

    function ValidateCateg() {

        var ret = false;
        var err = "Library required";

        // Call as
        var checkedBoxes = getCheckedBoxes("rb-res-lib");
        if (checkedBoxes != null && checkedBoxes.length > 0) {
            ret = true;
        }
        else {
            alert(err);
            return ret;
        }

        checkedBoxes = getCheckedBoxes("rb-res-cat");
        if (checkedBoxes != null && checkedBoxes.length > 0) {
            ret = true;
        }
        else {
            err = "Category required"
            alert(err);
            ret = false;
        }
        return ret;
    }

    $(document).ready(function () {

        $('#<%=rblType.ClientID %>_0').click(function () {
            ValidatorEnable(document.getElementById('<%= RequiredFieldValidator3.ClientID %>'), false);
            $('#trUrl').addClass("hide");
            $('#trDocument').removeClass("hide");
        });

        $('#<%=rblType.ClientID %>_1').click(function () {
            $('#trUrl').removeClass("hide");
            $('#trDocument').addClass("hide");
            ValidatorEnable(document.getElementById('<%= RequiredFieldValidator3.ClientID %>'), true);
        });

        <%if (rblType.SelectedValue == "1") {%>
            ValidatorEnable(document.getElementById('<%= RequiredFieldValidator3.ClientID %>'), false);
            $('#trUrl').addClass("hide").removeClass("show");
        <%} else { %>
            $('#trDocument').addClass("hide").removeClass("show");
        <%} %>


        <%if (!Request.ServerVariables["HTTP_USER_AGENT"].ToString().ToLower().Contains("firefox")) { %>

        $('.gvLib > .plus.categ, .gvLib > .minus.categ').click(function (e) {
            
            if (e.offsetX > this.offsetWidth - 20) {
                $(this).siblings('div').toggle();
                if ($(this).attr("class") == "plus categ")
                    $(this).attr("class", "minus categ");
                else
                    $(this).attr("class", "plus categ");

            }
        });

        $('.gvCateg > .plus.categ, .gvCateg > .minus.categ').click(function (e) {

            if (e.offsetX > this.offsetWidth - 20) {
                $(this).siblings('div').toggle();
                if ($(this).attr("class") == "plus categ")
                    $(this).attr("class", "minus categ");
                else
                    $(this).attr("class", "plus categ");

            }
        });
        <%} else{%>
        $('.gvLib > .plus.categ, .gvLib > .minus.categ').click(function (e) {

            $(this).siblings('div').toggle();
            if ($(this).attr("class") == "plus categ")
                $(this).attr("class", "minus categ");
            else
                $(this).attr("class", "plus categ");

        });

        $('.gvCateg > .plus.categ, .gvCateg > .minus.categ').click(function (e) {

            $(this).siblings('div').toggle();
            if ($(this).attr("class") == "plus categ")
                $(this).attr("class", "minus categ");
            else
                $(this).attr("class", "plus categ");

        });

        <%}%>

        $('.gvCateg input, .gvSubCateg input').click(function () {
            var items = $('#<%=hfCheckedBoxes.ClientID%>').val();
            items += "," + $(this).attr("name");
            $('#<%=hfCheckedBoxes.ClientID%>').val(items);
        });
    });
</script>
          
<table border='0' cellpadding='0' cellspacing='0' class='currentwidth'>
        <tr>
            <td style='text-align:left'><table><tr><td><asp:Image ID="imStatus" runat="server" ImageUrl="~/images/icons/pending.jpg" /></td><td><span style='padding-left: 10px; font-family:Arial, Sans-Serif; font-size:28px; color:#ffffff; font-weight:bold;'></span></td></tr></table>
                <asp:HiddenField runat="server" ID="hfCheckedBoxes" />
            </td>
            <td style='text-align:right'>
                <asp:Button ID="btnBackToList" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btnBackToList_Click" CausesValidation="false" />&nbsp;&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btEdit_Click"  ValidationGroup="Resource" OnClientClick="if( !ValidateCateg()){return false;}"/>
                <asp:Button ID="btnDone" runat="server" CssClass="admin-button-blue mw150" Text="Done" onclick="LinkButtonDone_Click"  CausesValidation="false" Visible="false" />
            </td>
        </tr>
        
    </table>
     
    <br />
    <asp:Literal runat="server" ID="litErr"></asp:Literal>

    <table cellpadding="0" cellspacing="0" border="0" style="width:100%;">
        <tr><td style="vertical-align:top;">

            <div class="admin-white-box" >
                <div class="admin-white-box-header">Details</div>
                <div class="admin-white-box-header"> 
                    <div style='padding: 10px 10px 10px 30px;width:unset!important;' class="currentwidth">
    
                        <table border="0" cellpadding="4" cellspacing="4"  style=" padding:0px; width:800px;">


                          <%-- <tr runat="server" id="trLanguage" class="hide">
                                <td style="vertical-align:top; width:210px;" >Language:</td>
                                <td><asp:DropDownList ID="ddlLanguage2" runat="server" AutoPostBack="True" CssClass="dropdownlist" 
                                        onselectedindexchanged="ddlLanguage2_SelectedIndexChanged">
                                    <asp:ListItem Text="English" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="French" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                    </td>
                                <td></td>
                            </tr> --%>

                           <tr>
                                <td>Title:</td>
                                <td><asp:TextBox ID="tbTitle" runat="server" Width='500px' CssClass='firstname tbFullName textbox' ></asp:TextBox>
                                    </td>
                                <td>&nbsp;<asp:RequiredFieldValidator 
                                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="Title Required" 
                                        SetFocusOnError="True" ValidationGroup="Resource" ControlToValidate="tbTitle">*</asp:RequiredFieldValidator></td>
                            </tr> 
                            <tr>
                                <td style="vertical-align:top; padding-top:10px;">Description:</td>
                                <td><asp:TextBox ID="tbDesc" runat="server" Width='500px' CssClass='firstname tbFullName textbox' TextMode="MultiLine" Rows="7" Height="120"  ></asp:TextBox>
                                </td>
                                <td></td>
                            </tr> 
                            <tr>
                                <td>Keywords:</td>
                                <td><asp:TextBox ID="tbKeywords" runat="server" Width='500px' CssClass='firstname tbFullName textbox' MaxLength="2000" ></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>                             
    
                            <tr><td>Resource Library / Link:</td><td>
                                <asp:RadioButtonList RepeatDirection="Horizontal" runat="server" ID="rblType" CssClass="rb-enhanced">
                                    <asp:ListItem Text="Document" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Link" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td><td></td></tr>

                            <tr id="trUrl"><td>Url:</td>
                                <td><asp:TextBox ID="tbUrl" runat="server" Width='500px' CssClass='firstname tbFullName textbox' ></asp:TextBox>
                                </td>
                                <td>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Url Required" SetFocusOnError="True" ValidationGroup="Resource" ControlToValidate="tbUrl">*</asp:RequiredFieldValidator></td>
                            </tr>

                             <tr>
                                <td>Icon Selection:</td>
                                <td><asp:DropDownList runat="server" ID="ddlIcon" CssClass="dropdownlist"  DataTextField="IconGroup" DataValueField="IconGroup"></asp:DropDownList>
                                </td>
                                <td></td>
                            </tr> 


                            <tr id="trDocument"><td>Document:</td>                    
                                <td><asp:FileUpload runat="server" ID="fuDocu" Width='500px' CssClass='firstname tbFullName textbox' /><div>
                                    <asp:LinkButton runat="server" ID="litDocu" onclick="litDocu_Click" ForeColor="#ffffff"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton 
                                        runat="server" ID="lnkDeleteDocu" onclick="lnkDeleteDocu_Click"></asp:LinkButton></div> </td><td></td>
                            </tr>

                            <tr>
                                <td>Associated Resources: </td>
                                <td>
                                    <div class="select-wrapper">
                                    <asp:ListBox runat="server" ID="ddlOtherResources" SelectionMode="Multiple" CssClass="select-wrapper dropdownlist" DataTextField="Title" DataValueField="id">
                                    </asp:ListBox>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                            </tr>

                            <tr><td>Hide:</td><td><asp:CheckBox runat="server" ID="cbShow"  /></td><td></td>
                            </tr>   
          
                             </table>
       
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                            ShowMessageBox="True" ShowSummary="False" ValidationGroup="Resource" />
                   </div>
                </div>
            </div>  

        </td><td style="padding-left:50px;  vertical-align:top;">
            <uc1:LibrariesPartial runat="server" ID="LibrariesPartial1" />
        </td></tr>
    </table>                
    
    <br />
    <table border='0' cellpadding='0' cellspacing='0' class='currentwidth'>
        <tr>
            <td style='text-align:left'><table><tr><td><asp:Image ID="imStatus2" runat="server" ImageUrl="~/images/icons/pending.jpg" /></td><td><span style='padding-left: 10px; font-family:Arial, Sans-Serif; font-size:28px; color:#ffffff; font-weight:bold;'></span></td></tr></table>
                
            </td>
            <td style='text-align:right'>
                <asp:Button ID="btnBackToList2" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btnBackToList_Click" CausesValidation="false" />&nbsp;&nbsp;
                <asp:Button ID="btnSave2" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btEdit_Click"   ValidationGroup="Resource"  OnClientClick="if( !ValidateCateg()){return false;}"/>
                <asp:Button ID="btnDone2" runat="server" CssClass="admin-button-blue mw150" Text="Done" onclick="LinkButtonDone_Click"  CausesValidation="false" Visible="false" />
            </td>
        </tr>
        
    </table>
    <script>
        $(document).ready(function () {
            
            $("#<%=ddlOtherResources.ClientID%>").dropdownchecklist();

        });
    </script> 
</asp:Panel>
</div>