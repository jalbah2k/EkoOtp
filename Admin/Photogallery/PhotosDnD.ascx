<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PhotosDnD.ascx.cs" Inherits="Admin_PhotosDnD" %>
<%--<%@ Register TagPrefix="cust" Namespace="DocProc" %>--%>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<script type="text/javascript" src="/js/jquery.tablednd.0.8.min.js"></script>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Photo Galleries</div>
    <div class="admin-header-subtitle">Here you can manage photo galleries to display on your website.</div>
</div>
<div class="admin-control-wrapper">
    <asp:UpdatePanel runat="server" ID="up1">
    <ContentTemplate>
        <asp:Panel ID="pnlList" runat="server">
            <div class="admin-white-box" style="min-width: 640px;">
                <div class="admin-white-box-header">
                    <asp:LinkButton ID="ImageOverButton2" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="newgroup" />
                </div>
                <div class="admin-white-box-inner">
                    <div id="nolist" runat="server" style="padding: 20px 0;" visible="false">There are currently no photo galleries.<br /></div>
                    <table id="list" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			        <tr><td colspan="2">
                        <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                            CellPadding="0" GridLines="None" 
                            onrowdatabound="GV_Main_RowDataBound" OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" OnSorting="GV_Main_Sorting">
                            <HeaderStyle CssClass="admin-grid-header" />
                            <PagerSettings Visible="false" />
                            <FooterStyle/>
                            <Columns>
                                <asp:BoundField DataField="name" HeaderText="Galleries" SortExpression="name">
                                    <ItemStyle CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle  HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="group" HeaderText="Group" SortExpression="group">
                                    <ItemStyle  CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="language" HeaderText="Language" SortExpression="language">
                                    <ItemStyle  CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item" ToolTip="Edit" />
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
        </asp:Panel>
        <asp:Panel ID="pnlView" runat="server" Visible="false">
            <div class="admin-white-box">
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:Textbox ID="txtName2" runat="server" CssClass="textbox" width="450" /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName2" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Title</td><td><asp:Textbox ID="txtTitle2" runat="server" CssClass="textbox" width="450" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTitle2" ErrorMessage="Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                    <tr id="trLanguage2" runat="server" visible="false"><td class="admin-prompt-right">Language</td><td><asp:DropDownList ID="ddlLanguage2" runat="server" CssClass="dropdownlist" width="450" /></td></tr>
                    <tr class="hide"><td class="admin-prompt-right">Style</td><td><asp:DropDownList ID="ddlMode" runat="server" CssClass="dropdownlist" width="300"><asp:ListItem Text="MODERN"></asp:ListItem><asp:ListItem Text="COMPACT"></asp:ListItem></asp:DropDownList></td></tr>
                    <tr class="hide"><td class="admin-prompt-right">Change Image on Hover</td><td><asp:CheckBox ID="cbHover" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
                    <tr class="hide"><td class="admin-prompt-right">Flickr</td><td><asp:CheckBox ID="cbFlickr" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" AutoPostBack="True" oncheckedchanged="cbFlickr_CheckedChanged" /></td></tr>
                    </table>
                </div>
            </div>
            <br />
            <asp:Panel runat="server" ID="pnlFlickr">
                <table border="0" cellpadding="0" cellspacing="0">
                <tr><td class="admin-prompt-right">Flickr UserName</td><td><asp:Textbox ID="txtFlickrUserName" runat="server" CssClass="textbox" width="300" /><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFlickrUserName" ErrorMessage="FlickrUserName required" ValidationGroup="EditForm" Display="Dynamic"> *</asp:RequiredFieldValidator>--%></td></tr>
                <tr><td class="admin-prompt-right">Flickr SetId</td><td><asp:TextBox ID="txtflickrSetId" runat="server" CssClass="textbox" width="300" /><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtflickrSetId" ErrorMessage="FlickrSetId required" ValidationGroup="EditForm" Display="Dynamic"> *</asp:RequiredFieldValidator>--%></td></tr>
                </table>
                <br />
                <asp:LinkButton ID="ImageOverButton7" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="backtolist" CausesValidation="false" />
                <br />
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlImages">
                <div class="admin-white-box" style="min-width: 640px;">
                    <div class="admin-white-box-header">
                        <CuteWebUI:Uploader runat="server" ID="Uploader1" InsertText="Upload Images" MultipleFilesUpload="true" 
                            OnFileUploaded="fileup" OnUploadCompleted="refresh" ValidateOption-AllowedFileExtensions=".jpg,.jpeg,.gif,.png,.bmp" 
                            InsertButtonStyle-CssClass="admin-button-green">
                        </CuteWebUI:Uploader>
                    </div>
                    <div class="admin-white-box-inner">
                        <table id="doctbl" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			            <tr><td>
                            <asp:GridView ID="GV_Docs" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="False" AllowSorting="True" 
                                AutoGenerateColumns="False" CellPadding="0" GridLines="None" 
                                OnRowDeleting="GV_Docs_RowDeleting" OnRowEditing="GV_Docs_RowEditing" onrowdatabound="GV_Docs_RowDataBound" OnRowCommand="GV_Docs_RowCommand">
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <RowStyle CssClass="noselect" />
                                <AlternatingRowStyle CssClass="noselect" />
                                <FooterStyle/>
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" >
                                        <ItemStyle  CssClass="itemrow" Width="1" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="litHfId"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="name" HeaderText="Name">
                                        <ItemStyle  CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <HeaderStyle  HorizontalAlign="Left"/>
                                    </asp:BoundField>
                                        <%--<asp:BoundField DataField="filename" HeaderText="File">
                                    <ItemStyle  CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <HeaderStyle  HorizontalAlign="Left"/>
                                    </asp:BoundField>--%>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="File Name">
                                        <ItemStyle  CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <a href="javascript:void(0);" onclick="javascript:document.getElementById('<%# "divThumbnail_" + Eval("id").ToString() %>').style.visibility='visible';" style="text-decoration:none;"><div>
                                            <asp:Literal runat="server" ID="litFName" ></asp:Literal></div></a>
                                            <div id='<%# "divThumbnail_" + Eval("id").ToString() %>' style="visibility:hidden; position:relative;"><div style=" position:absolute; top:0; left:0;"><img src='<%# "/admin/Photogallery/ThumbNail.ashx?file=/data/photos/" + Eval("groupid").ToString() + "/" + Eval("filename").ToString() + "&maxsz=150" %>' onclick="javascript:document.getElementById('<%# "divThumbnail_" + Eval("id").ToString() %>').style.visibility='hidden';" style="cursor:pointer;" alt="thumbnail" />
                                            </div></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%--<asp:ImageButton ID="imgUP" runat="server" CommandName="Up" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/move-up.png" AlternateText="Move Up" ToolTip="Move Up" />
                                            <asp:ImageButton ID="imgDown" runat="server" CommandName="Down" CommandArgument='<%#Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/move-down.png" AlternateText="Move Down" ToolTip="Move Down" />--%>
                                            <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" style="cursor:pointer;" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item" ToolTip="Edit Item" />
                                            <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False"  AlternateText="Delete" style="cursor:pointer;" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
			            </td></tr>
		                </table>
                        <div id="nopic" runat="server" style="padding: 20px 0;">No photos currently in this gallery.<br /></div>
                    </div>
                </div>
                <br />
                <asp:LinkButton ID="ImageOverButton5" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="backtolist" CausesValidation="false" />
                <asp:LinkButton ID="ImageOverButton6" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="ImageOverButton6_Click" ValidationGroup="EditForm" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditForm" />
                
            </asp:Panel>
        </asp:Panel>   
        <asp:Panel ID="pnlSend" runat="server" Visible="false">
            <div class="admin-white-box">
                <div class="admin-white-box-header">Gallery Details</div>
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:Textbox ID="txtName" runat="server" class="textbox" width="450" /><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtName" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="grGallery"> *</asp:RequiredFieldValidator></td></tr>
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Title</td><td><asp:Textbox ID="txtTitle" runat="server" class="textbox" width="450" /><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTitle" ErrorMessage="Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="grGallery"> *</asp:RequiredFieldValidator></td></tr>
                    <tr><td class="admin-prompt-right">Group</td><td><asp:DropDownList ID="ddlGroup" runat="server" CssClass="dropdownlist" width="450" DataTextField="name" DataValueField="id" /></td></tr>
                    <tr id="trLanguage" runat="server" visible="false"><td class="admin-prompt-right">Language</td><td><asp:DropDownList ID="ddlLanguage" runat="server" CssClass="dropdownlist" width="450" /></td></tr>
                    </table>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="ImageOverButton1" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="backtolist" CausesValidation="false" />
            <asp:LinkButton ID="ImageOverButton4" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="addnewgroup" ValidationGroup="grGallery" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="grGallery" />
        </asp:Panel>
        <asp:Panel ID="pnlEditDoc" runat="server" Visible="false">
            <div class="admin-white-box">
                <div class="admin-white-box-inner">
                    <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:Textbox ID="txtDocName" runat="server" CssClass="textbox" width="450" /><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDocName" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="PhotoEditForm"> *</asp:RequiredFieldValidator></td></tr>
                    <tr><td class="admin-prompt-right">Alt Tag</td><td><asp:Textbox ID="txtCH" runat="server" CssClass="textbox" width="450" /></td></tr>
                    <tr><td class="admin-prompt-right">Caption</td><td><asp:Textbox ID="txtC" runat="server" CssClass="textbox" width="450" /></td></tr>
                    </table>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="imgBack" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="backtodoclist" CausesValidation="false" />
            <asp:LinkButton ID="ImageOverButton3" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="savedocname" ValidationGroup="PhotoEditForm" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="PhotoEditForm" />
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
                           // alert("Test");
                            reorder();
                            $.ajax({
                                type: "POST",
                                //url: "testpga.aspx/PhotosReorders",
                                url: "admin.aspx/PhotosReorders",
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
                        var id = $(this).find("input[class='HfId']").val();
                        //$(this).attr('id', i++);
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
