<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Documents.ascx.cs" Inherits="Admin_Dash_Dash" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<script type="text/javascript" src="/js/jquery.tablednd.0.8.min.js"></script>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false"></asp:ScriptManager>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Documents</div>
    <div class="admin-header-subtitle">Here you can manage documents to be shared to the public throughout your website.</div>
</div>
<asp:Label runat="server" ID="litMessage" ForeColor="Red"></asp:Label>
<div class="admin-control-wrapper">
    <asp:UpdatePanel runat="server" ID="up1">
    <ContentTemplate>
        <asp:Panel ID="pnlList" runat="server">
            <div class="admin-white-box">
                <div class="admin-white-box-header">Filter</div>
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0"><tr><td class="admin-prompt-right">Text</td><td><asp:TextBox ID="txtFilter" runat="server" CssClass="textbox" Width="200" OnTextChanged="filter" AutoPostBack="true" /><ACT:TextBoxWatermarkExtender TargetControlID="txtFilter" WatermarkText="name" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="TextBoxWatermarkExtender1" /></td><td style="padding-left: 25px;"><asp:linkButton ID="ImageOverButton1" runat="server" CssClass="admin-button-green mw150" Text="Filter" onclick="filter" /></td></tr></table>
                </div>
            </div>
            <br />
            <div class="admin-white-box" style="min-width: 640px;">
                <div class="admin-white-box-header">
                    <asp:LinkButton ID="btnMake" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="newgroup" />
                </div>
                <div class="admin-white-box-inner">
                    <table id="list" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			        <tr><td colspan="2">
                        <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                            CellPadding="0" GridLines="None"
                            onrowdatabound="GV_Main_RowDataBound" OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" 
                            OnSorting="GV_Main_Sorting" onrowcommand="GV_Main_RowCommand">
                            <HeaderStyle CssClass="admin-grid-header" />
                            <PagerSettings Visible="false" />
                            <Columns>
                                <asp:BoundField DataField="name" HeaderText="Backend name" SortExpression="name">
                                    <ItemStyle  CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle   HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="listname" HeaderText="Public title" SortExpression="listname">
                                    <ItemStyle  CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle  HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="group" HeaderText="Group" SortExpression="group">
                                    <ItemStyle CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="language" HeaderText="Language" Visible="true" SortExpression="language">
                                    <ItemStyle CssClass="itemrow" Width="10" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png"  AlternateText="Edit Item" ToolTip="Edit" />
                                        <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="False" CommandArgument='<%#Eval("id") %>' CommandName="View" ImageUrl="/images/lemonaid/buttonsNew/editfields.png" AlternateText="Edit Item Details" ToolTip="Edit Items" />
                                        <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete" ToolTip="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
			        </td></tr>
                    <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		            </table>
                    <div id="nolist" runat="server" style="padding: 20px 0;" visible="false">There are currently no documents.<br /></div>
                </div>
            </div>
            <br />
        </asp:Panel>
        <asp:Panel ID="pnlView" runat="server" Visible="false">
            <div class="admin-white-box" style="min-width: 640px;">
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0"><tr><td class="admin-prompt-right">Public Title</td><td><asp:TextBox ID="txtTitle" runat="server" CssClass="textbox" Width="450" ReadOnly="true" /></td></tr></table>
                </div>
            </div>
            <br />
            <div class="admin-white-box" style="min-width: 640px;">
                <div class="admin-white-box-header">
                    <CuteWebUI:Uploader runat="server" ID="Uploader1" InsertText="Upload Documents"
                        MultipleFilesUpload="true" OnFileUploaded="fileup" OnUploadCompleted="refresh" InsertButtonStyle-CssClass="admin-button-green">
                    </CuteWebUI:Uploader>
                </div>
                <div class="admin-white-box-inner">
                    <div id="nodoc" runat="server" style="padding: 20px 0;">No documents currently in this list.<br /></div>
                    <table id="doctbl" runat="server" cellpadding="0" cellspacing="0" border="0" style="padding:0px; border-collapse:collapse;">
			        <tr><td>
                        <asp:GridView ID="GV_Docs" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="False" 
                            AllowSorting="True" AutoGenerateColumns="False" CellPadding="0" GridLines="None" 
                            OnRowDeleting="GV_Docs_RowDeleting" OnRowEditing="GV_Docs_RowEditing" 
                            OnRowCommand="GV_Docs_RowCommand" onrowdatabound="GV_Docs_RowDataBound">
                            <HeaderStyle CssClass="admin-grid-header" />
                            <PagerSettings Visible="false" />
                            <RowStyle CssClass="noselect" />
                            <AlternatingRowStyle CssClass="noselect" />
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Right" >
                                    <ItemStyle CssClass="itemrow" Width="1" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>--%>
                                        <%--<asp:HiddenField ID="hfId" runat="server" Visible="true" Value='<%# Eval("id") %>'  />--%>
                                        <asp:Literal runat="server" ID="litHfId"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="name" HeaderText="Name">
                                    <ItemStyle  CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="filename" HeaderText="File" Visible="false">
                                    <ItemStyle  CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle  HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%--<asp:ImageButton ID="imgUP" runat="server" CommandName="Up" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/move-up.png" AlternateText="Move Up" ToolTip="Move Up" />
                                        <asp:ImageButton ID="imgDown" runat="server" CommandName="Down" CommandArgument='<%#Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/move-down.png" AlternateText="Move Down" ToolTip="Move Down" />--%>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item" ToolTip="Edit" />
                                        <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete" ToolTip="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
			        </td></tr>
		            </table>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="imgBack" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="savetitle" CausesValidation="false" />
        </asp:Panel>
        <asp:Panel ID="pnlSend" runat="server" Visible="false">
            <div class="admin-white-box">
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Backend Name</td><td><asp:Textbox ID="txtName" runat="server" CssClass="textbox" width="450" /><asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Backend name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                    <tr><td class="admin-prompt-right"><%--<span class="required">*</span>--%>Public Title</td><td><asp:Textbox ID="txtListName" runat="server" CssClass="textbox" width="450" /><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtListName" ErrorMessage="Public title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator>--%></td></tr>
                    <tr><td class="admin-prompt-right">Group</td><td><asp:DropDownList ID="ddlGroup" runat="server" CssClass="dropdownlist" width="450" DataTextField="name" DataValueField="id"/></td></tr>
                    <tr id="trLanguage" runat="server" visible="false"><td class="admin-prompt-right">Language</td><td><asp:DropDownList ID="ddlLanguage" runat="server" CssClass="dropdownlist" width="450"><asp:ListItem Text="English" Value="1" /><asp:ListItem Text="French" Value="2" /></asp:DropDownList></td></tr>
                    <tr><td class="admin-prompt-right">Sort by</td><td style="padding-top: 15px;"><asp:RadioButtonList ID="rblSort" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal"><asp:ListItem Selected="True" Text="Date Added" Value="0" /><asp:ListItem Text="Name" Value="1" /></asp:RadioButtonList></td></tr>
                    </table>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="btGallery" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="backtolist" CausesValidation="false" />
            <asp:LinkButton ID="ImageOverButton2" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="addnewgroup" ValidationGroup="EditForm" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditForm" />
        </asp:Panel>
        <asp:Panel ID="pnlEditDoc" runat="server" Visible="false">
            <div class="admin-white-box">
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0"><tr><td class="admin-prompt-right">Name</td><td><asp:TextBox ID="txtDocName" runat="server" CssClass="textbox" Width="600" /></td></tr></table>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="ImageOverButton3" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="backtodoclist" />
            <asp:LinkButton ID="ImageOverButton4" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="savedocname" />
        </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
          <div class="updateprogress">Update in progress...</div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
<script type="text/javascript">

    var strorder;

    function reorder() {
        strorder = "";
        <%-- var totalid = $("#<%= GV_Docs.ClientID %> tr td input[id$='hfId']").length;
        for (var i = 0; i < totalid; i++) {
            strorder += $("#<%= GV_Docs.ClientID %> tr td input[id$='hfId']")[i].getAttribute("value") + "|";
        }--%>

        var totalid = $("#<%= GV_Docs.ClientID %> tr td input[class='HfId']").length;
        for (var i = 0; i < totalid; i++) {
            strorder += $("#<%= GV_Docs.ClientID %> tr td input[class='HfId']")[i].getAttribute("value") + "|";
        }

        //alert("totalid: " + totalid + " - strorder: " + strorder);
    }

    function BindControlEvents() {
        $('#<%= GV_Docs.ClientID %>').tableDnD(
        {
            onDragClass: "myDragClass",
            onDrop: function (table, row) {
                //alert("Test");
                reorder();
                $.ajax({
                    type: "POST",
                    //url: "testpga.aspx/PhotosReorders",
                    url: "admin.aspx/DocumentsReorders",
                    data: '{"Reorder":"' + strorder + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (msg) {
                        //alert("Successfully Save ReOrder");
                    }
                })
            }
        });

        //var i = 0;
        $('#<%= GV_Docs.ClientID %>').find('tr').each(function () {
            //var id = $(this).find("input[id$='hfId']").val();
            ////$(this).attr('id', i++);

            var id = $(this).find("input[class='HfId']").val();


            if (id != null && id != 'undefined')
                $(this).attr('id', 'tr_' + id);
        });

        $("#<%= GV_Docs.ClientID %> tr[id^='tr_']").hover(function () {
            $(this.cells[0]).addClass('showDragHandle');
            }, function () {
                $(this.cells[0]).removeClass('showDragHandle');
        });
    }

    //Initial bind
    $(document).ready(function () {
        BindControlEvents();
    });

    //Re-bind for callbacks 
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        BindControlEvents();
    });

</script>