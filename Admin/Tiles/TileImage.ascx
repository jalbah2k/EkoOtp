<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TileImage.ascx.cs" Inherits="TileImage" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>
<%--<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<script type="text/javascript" src="/js/jquery.tablednd.0.8.min.js"></script>
<link href="/CSS/LemonAid.css" rel="stylesheet" type="text/css" />
<link href="/CSS/AdminNew.css" rel="stylesheet" type="text/css" />
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="/js/jquery.tablednd.0.8.min.js"></script>

<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false">
</asp:ScriptManager>

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
                                CellPadding="0" GridLines="None" OnRowCommand="GV_Main_RowCommand" OnRowEditing="GV_Main_RowEditing" OnRowDataBound="GV_Main_RowDataBound" >
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
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DelGroup" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete Item" CommandArgument='<%# Eval("id") %>' ToolTip="Delete Gallery" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
			            </td></tr>
		                </table>
                    </div>
                </div>
                <div class="admin-white-box-header">
                        <asp:LinkButton ID="btnAddGallery" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="btnAddGallery_Click"  />
                    </div>
            </asp:Panel>
			
			<asp:Panel ID="pnlEditGrp" runat="server" Visible="false">
                <div class="admin-white-box">
                    <div class="admin-white-box-inner">
                        <table cellpadding="0" cellspacing="0" border="0">
                        <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox ID="txtNameGrp" runat="server" CssClass="textbox" Width="450" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNameGrp" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="Group" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator></td></tr>
                        <tr runat="server" visible="false"><td class="admin-prompt-right">Language</td><td><asp:DropDownList ID="ddlLanguage" runat="server" CssClass="dropdownlist" Width="450" /></td></tr>

                        <tr runat="server" visible="false"><td class="admin-prompt-right">Link</td><td><asp:TextBox ID="txtLinkGrp" runat="server" CssClass="textbox" Width="450" />
                            </td></tr>
                        </table>
                    </div>
                </div>
                <br />
                <asp:LinkButton ID="btnCancelGrp" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="btnCancelGrp_Click" CausesValidation="false" />
                <asp:LinkButton ID="btnSaveGrp" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="btnSaveGrp_Click" ValidationGroup="Group" />
            </asp:Panel>

            <asp:Panel ID="pnlItems" runat="server" Visible="false">
                <div class="admin-white-box" style="min-width: 640px;">
                    <div class="admin-white-box-inner">
                       <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			            <tr><td colspan="2">
                            <asp:GridView ID="GV_Images" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="False" AutoGenerateColumns="False"
                                CellPadding="0" GridLines="None" OnRowCommand="GV_Images_RowCommand" OnRowDataBound="GV_Images_RowDataBound">
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Right" >
                                        <ItemStyle CssClass="itemrow" Width="1" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="litHfId"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="name" HeaderText="Name" 
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditItem" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Item" />
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DelItem" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete Item" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Item" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
			            </td></tr>
		                </table>
                    </div>
                </div>
                <div class="admin-white-box-header">
                        <asp:LinkButton ID="btnBackItems" runat="server" CssClass="admin-button-gray" Text="Back" OnClick="btnBackItems_Click" />
                        <asp:LinkButton ID="btAddItem" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="btAddItem_Click"  />
                </div>
                <script type="text/javascript">

                var strorder;

                function reorder() {
                    strorder = "";

                    var totalid = $("#<%= GV_Images.ClientID %> tr td input[class='HfId']").length;
                    for (var i = 0; i < totalid; i++) {
                        strorder += $("#<%= GV_Images.ClientID %> tr td input[class='HfId']")[i].getAttribute("value") + "|";
                                }

                   // alert("totalid: " + totalid + " - strorder: " + strorder);
                }

                function BindControlEvents() {
                    $('#<%= GV_Images.ClientID %>').tableDnD(
                    {
                        onDragClass: "myDragClass",
                        onDrop: function (table, row) {
                            reorder();
                            $.ajax({
                                type: "POST",
                                url: "admin.aspx/AwardImages_Reorders",
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
                    $('#<%= GV_Images.ClientID %>').find('tr').each(function () {
                        var id = $(this).find("input[class='HfId']").val();


                        if (id != null && id != 'undefined')
                            $(this).attr('id', 'tr_' + id);
                    });

                    $("#<%= GV_Images.ClientID %> tr[id^='tr_']").hover(function () {
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


            </asp:Panel>
            <asp:Panel ID="pnlEditItem" runat="server" Visible="false">
                <div class="admin-white-box">
                    <div class="admin-white-box-inner">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr><td class="admin-prompt-right"><%--<span class="required">*</span>--%>Image</td><td><asp:FileUpload runat="server" ID="fuImage" CssClass="fupload" />
                                <asp:RequiredFieldValidator ID="rfvImage" runat="server" ControlToValidate="fuImage" ValidationGroup="Item" ErrorMessage="Image required" Display="Dynamic" SetFocusOnError="True" ForeColor="Red" Enabled="false"> &nbsp;Required</asp:RequiredFieldValidator>
                                <br /><asp:Image runat="server" ID="imgWD" />
                             </td></tr>
                            <tr><td class="admin-prompt-right"><span class="required">*</span>Image Alternate Text</td><td><asp:TextBox ID="tbImgAltText" runat="server" CssClass="textbox" Width="450"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="tbImgAltText" ValidationGroup="Item" ErrorMessage="Alternate Text required" Display="Dynamic" SetFocusOnError="True" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator>
                            </td></tr>

                            <tr><td class="admin-prompt-right"><span class="required">*</span>Title</td><td><asp:TextBox ID="txtTitle" runat="server" CssClass="textbox" Width="450" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" ValidationGroup="Item" ErrorMessage="Title required" Display="Dynamic" SetFocusOnError="True" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator>
                             </td></tr>

                            <tr><td class="admin-prompt-right"><span class="required">*</span>Text</td><td><asp:TextBox ID="txtText" runat="server" CssClass="textbox" Width="450" TextMode="MultiLine" Rows="8" Height="250px" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtText" ValidationGroup="Item" ErrorMessage="Text required" Display="Dynamic" SetFocusOnError="True" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator></td></tr>
                             <tr><td class="admin-prompt-right">Button Text</td><td><asp:TextBox ID="txtButton" runat="server" CssClass="textbox" Width="450" /></td></tr>
                           <tr><td class="admin-prompt-right">Link</td><td><asp:TextBox ID="txtLink" runat="server" CssClass="textbox" Width="450" /></td></tr>
                            <tr><td class="admin-prompt-right" style="padding-top:0;">New window</td><td><asp:CheckBox runat="server" ID="cbNewWindow" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
                        </table>
                    </div>
                </div>
                <br />
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="btnCancel_Click" CausesValidation="false" />
                <asp:LinkButton ID="btnSave" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="btnSave_Click" ValidationGroup="Item" />
            </asp:Panel>
       
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

