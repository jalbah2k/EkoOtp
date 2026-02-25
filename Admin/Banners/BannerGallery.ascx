<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BannerGallery.ascx.cs" Inherits="Admin_Banners_BannerGallery" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<script type="text/javascript" src="/js/jquery.tablednd.0.8.min.js"></script>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false">
</asp:ScriptManager>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Banner Galleries</div>
    <div class="admin-header-subtitle">Upload photos to be displayed in a fading Banner Gallery anywhere on the site.</div>
</div>
<div class="admin-control-wrapper">
    <asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>
            <!-- Main Page - Galleries -->
            <asp:Panel ID="pnlGalleryList" runat="server">
                <div class="admin-white-box" style="min-width: 640px;">
                    <div class="admin-white-box-header">
                        <asp:LinkButton ID="btGallery" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="btGallery_Click" />
                    </div>
                    <div class="admin-white-box-inner">
                        <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			            <tr><td colspan="2">
                            <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id,GalleryName,Language" AllowSorting="True" AutoGenerateColumns="False"
                                CellPadding="0" GridLines="None" onrowdatabound="GV_Main_RowDataBound" 
                                OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" OnSorting="GV_Main_Sorting" OnRowCommand="GV_Main_RowCommand">
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                    <asp:BoundField DataField="GalleryName" HeaderText="Name" SortExpression="GalleryName"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="language" HeaderText="Language" SortExpression="Language"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditGallery" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Gallery" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Gallery" />
                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/editfields.png" AlternateText="Edit Items" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Items" />
                                            <asp:ImageButton ID="LB_Delete" runat="server"  AlternateText="Delete" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
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
            <!-- Add Gallery Page -->
            <asp:Panel ID="pnlAddGallery" runat="server">
                <div class="admin-white-box">
                    <div class="admin-white-box-header">Gallery Details</div>
                    <div class="admin-white-box-inner">
                        <table cellpadding="0" cellspacing="0" border="0">
                        <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox ID="tbGalleryName" runat="server" CssClass="textbox" Width="450"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbGalleryName" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="grGallery"> *</asp:RequiredFieldValidator></td></tr>
                        <tr id="trLanguage" runat="server" visible="false"><td class="admin-prompt-right">Language</td><td><asp:DropDownList ID="ddlLanguage" runat="server" CssClass="dropdownlist" Width="450" /></td></tr>
                        <tr><td class="admin-prompt-right">Group</td><td><asp:DropDownList ID="ddlGroup" runat="server" CssClass="dropdownlist" DataTextField="name" DataValueField="id" Width="450" /></td></tr>
                        <tr><td class="admin-prompt-right">Width</td><td><asp:TextBox ID="txtWidth" runat="server" CssClass="textbox" Width="450"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="txtWidth" /></td></tr>
                        <tr><td class="admin-prompt-right">Height</td><td><asp:TextBox ID="txtHeight" runat="server" CssClass="textbox" Width="450"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" TargetControlID="txtHeight" /></td></tr>
                        <tr><td class="admin-prompt-right">Autoplay</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbAutoplay" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" Checked="true" /></td></tr>
                        <tr><td class="admin-prompt-right">Shuffle</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbShuffle" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
                        <tr><td class="admin-prompt-right">Direction</td><td><asp:DropDownList ID="ddlDirection" runat="server" CssClass="dropdownlist" Width="450"><asp:ListItem Text="Horizontal" Value="h" /><asp:ListItem Text="Vertical" Value="v" /></asp:DropDownList></td></tr>
                        <tr><td class="admin-prompt-right">Transitions</td><td><asp:DropDownList ID="ddlTransitions" runat="server" CssClass="dropdownlist" Width="450"><asp:ListItem Text="Basic" Value="basic" /><asp:ListItem Text="Fade" Value="fade" /><asp:ListItem Text="Mask" Value="mask" /><asp:ListItem Text="Wave" Value="wave" /><asp:ListItem Text="Flow" Value="flow" /><asp:ListItem Text="Scale" Value="scale" /></asp:DropDownList></td></tr>
                        </table>
                    </div>
                </div>
                <br />
                <asp:LinkButton ID="imgBack" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="imgBack_Click" CausesValidation="false" />
                <asp:LinkButton ID="imgSubmit" runat="server" CssClass="admin-button-green mw150" Text="Save" OnCommand="imgSubmit_Click" ValidationGroup="grGallery"/>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="grGallery" />
            </asp:Panel>
            <asp:Panel ID="pnlBanners" runat="server">
                <!-- Banners Page-->
                <div class="admin-white-box" style="min-width: 640px;">
                    <div class="admin-white-box-header">
                        <CuteWebUI:Uploader runat="server" ID="Uploader1" InsertText="Upload Banners"
                            MultipleFilesUpload="true" OnFileUploaded="fileup" OnUploadCompleted="refresh" InsertButtonStyle-CssClass="admin-button-green">
                        </CuteWebUI:Uploader>
                        <div style="float:right">
                            <asp:LinkButton ID="btnAddItem" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="btnAddItem_Click" />
                        </div>
                    </div>
                    <div class="admin-white-box-inner">
                        <asp:Panel ID="pnlBannersVisible" runat="server">
                            <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			                <tr><td>
                                
                                <asp:GridView ID="GV_Banners" runat="server" CssClass="admin-grid" AllowSorting="True" AutoGenerateColumns="False"
                                    CellPadding="0" GridLines="None"
                                    DataKeyNames="bannerid,bannername,bannerlink,target,name,caption,AltText,StartDate,EndDate,title,body,ButtonText,ButtonTitle,ButtonLink,PresentationClass,ButtonText1,ButtonTitle1,ButtonLink1" 
                                    OnRowCommand="GV_Banners_RowCommand" OnRowDataBound="GV_Banners_RowDataBound" 
                                    OnRowDeleting="GV_Banners_RowDeleting" OnRowEditing="GV_Banners_RowEditing" OnSorting="GV_Banners_Sorting">
                                    <HeaderStyle CssClass="admin-grid-header" />
                                    <PagerSettings Visible="false" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" >
                                            <ItemStyle  CssClass="itemrow" Width="1" HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <%--<asp:HiddenField ID="hfId" runat="server" Visible="true" Value='<%# Eval("BannerID") %>' />--%>
                                                <asp:Literal runat="server" ID="litHfId"></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField  HeaderText="Name">
                                            <ItemTemplate>
                                                <a href="#" onclick="javascript:document.getElementById('<%# "divThumbnail_" + Eval("BannerId").ToString() %>').style.visibility='visible';" style="text-decoration:none;"><div><%# Eval("BannerName") %></div></a>
                                                <div id='<%# "divThumbnail_" + Eval("BannerId").ToString() %>' style="visibility:hidden; position:relative;"><div style=" position:absolute; top:0; left:0;"><img src='<%# "/admin/banners/ThumbNail.ashx?file=" + Eval("BannerFileLocation").ToString() + Eval("Gallery").ToString() + "/" + Eval("BannerName").ToString() + "&maxsz=150" %>' onclick="javascript:document.getElementById('<%# "divThumbnail_" + Eval("BannerId").ToString() %>').style.visibility='hidden';" />
                                                </div></div>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="200" VerticalAlign="middle" CssClass="itemrow" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="URL">
                                            <HeaderStyle  HorizontalAlign="Left" />
                                            <ItemStyle Width="200" VerticalAlign="Middle" CssClass="itemrow" />
                                            <ItemTemplate>
                                                <%--<div><%# (Eval("BannerLink").ToString().Length > 40) ? Eval("BannerLink").ToString().Substring(0, 37) + "..." : Eval("BannerLink").ToString()%></div>--%>
                                                <div><%# Eval("ButtonLink")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="BannerPriority" HeaderText="Priority" SortExpression="BannerPriority" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle  CssClass="itemrow" Width="50" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgUP" runat="server" CommandName="Up" CommandArgument='<%# Eval("bannerid") %>' ImageUrl="/images/lemonaid/buttonsNew/move-up.png" AlternateText="Move Up" ToolTip="Move Up" />
                                                <asp:ImageButton ID="imgDown" runat="server" CommandName="Down" CommandArgument='<%#Eval("bannerid") %>' ImageUrl="/images/lemonaid/buttonsNew/move-down.png" AlternateText="Move Down" ToolTip="Move Down" />
                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item" ToolTip="Edit" />
                                                <asp:ImageButton ID="LB_Delete" runat="server" AlternateText="Delete" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
			                </td></tr>
		                    </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlNoBanners" runat="server" style="padding: 20px 0;">No banners currently in this list.<br /></asp:Panel>
                    </div>
                </div>
                <br />
                <asp:LinkButton ID="imgBannerBack" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="imgBannerBack_Click" />
            </asp:Panel>
         
            <asp:Panel ID="pnlEditBanner" runat="server">
                <!-- Add Banners page -->
                <div class="admin-white-box">
                    <div class="admin-white-box-inner">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr style="display:none;"><td class="admin-prompt-right">Name</td><td><asp:TextBox ID="tbName2" runat="server" CssClass="textbox" Width="450" /></td></tr>
<%--                            <tr><td class="admin-prompt-right">File</td><td><asp:TextBox ID="txtfilename" runat="server" CssClass="textbox" Width="450" Enabled="false"></asp:TextBox></td></tr>--%>
                            <tr><td class="admin-prompt-right">File</td><td>
                                <asp:DropDownList ID="ddlFilename" runat="server" CssClass="dropdownlist" Width="450" DataValueField="value" DataTextField="name" >
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlFilename" ErrorMessage="File required" 
                                    Display="Dynamic" SetFocusOnError="True" ValidationGroup="grBanner"> *</asp:RequiredFieldValidator>
                            </td></tr>
                            <tr><td class="admin-prompt-right">Title</td><td><asp:TextBox ID="txtTitle" runat="server" CssClass="textbox" Width="450" Text="https://" MaxLength="1000" /></td></tr>                     
                            <tr><td class="admin-prompt-right">Body<br />
                                <span style="font-size:12px;">(Max length: 1000 characters)</span>
                                </td>
                            <td><asp:TextBox ID="tbCaption2" runat="server" CssClass="textbox" Width="450" Height="150" TextMode="MultiLine" MaxLength="1000" />
                                <%--<div id="ng-app" ng-app="textAngularDemo" ng-controller="demoController" class="ng-scope">
                                    <div text-angular name="tbCaption2" ta-text-editor-class="clearfix border-around container" ta-html-editor-class="border-around"></div>
                                </div>--%>
                            </td></tr>
                            <tr style="display:none;"><td class="admin-prompt-right">URL</td><td><asp:TextBox ID="tbUrl2" runat="server" CssClass="textbox" Width="450" Text="https://" /></td></tr>                     
                            <tr><td class="admin-prompt-right">Alt Text</td><td><asp:TextBox ID="txtAltText" runat="server" CssClass="textbox" Width="450" MaxLength="100" /></td></tr>                     
                      
                            <tr><td class="admin-prompt-right">Button Text</td><td><asp:TextBox ID="txtButtonText" runat="server" CssClass="textbox" Width="450" MaxLength="30" /></td></tr>                     
                            <tr style="display:none;"><td class="admin-prompt-right">Button Title</td><td><asp:TextBox ID="txtButtonTitle" runat="server" CssClass="textbox" Width="450" MaxLength="30" /></td></tr>                     
                            <tr><td class="admin-prompt-right">Button Link</td><td><asp:TextBox ID="txtButtonLink" runat="server" CssClass="textbox" Width="450" MaxLength="100" /></td></tr>                     

                            <tr><td class="admin-prompt-right">Button 2 Text</td><td><asp:TextBox ID="txtButtonText1" runat="server" CssClass="textbox" Width="450" MaxLength="30" /></td></tr>                     
                            <tr style="display:none;"><td class="admin-prompt-right">Button 2 Title</td><td><asp:TextBox ID="txtButtonTitle1" runat="server" CssClass="textbox" Width="450" MaxLength="30" /></td></tr>                     
                            <tr><td class="admin-prompt-right">Button 2 Link</td><td><asp:TextBox ID="txtButtonLink1" runat="server" CssClass="textbox" Width="450" MaxLength="100" /></td></tr>                     


                            <tr><td class="admin-prompt-right">Start Date</td><td>
                                <div class="datepicker-wraper">
                                    <asp:Textbox ID="txtStartDate" runat="server" />
                                    <asp:Image ID="Img_EventDate_Start1" runat="Server" AlternateText="Click to show calendar" ImageUrl="/images/Lemonaid/buttonsNew/datepicker.png" />
                                    <ACT:CalendarExtender ID="Cal_Start_Date" runat="server" Format="yyyy/MM/dd" PopupButtonID="Img_EventDate_Start1" TargetControlID="txtStartDate" />
                                </div>
                                &nbsp;&nbsp;<asp:DropDownList ID="ddlStartTime" runat="server" CssClass="dropdownlist" />

                            </td></tr>                     
                                                     
                            <tr><td class="admin-prompt-right">End Date</td><td>
                                <div class="datepicker-wraper">
                                    <asp:Textbox ID="txtEndDate" runat="server" />
                                    <asp:Image ID="Img_EventDate_End1" runat="Server" AlternateText="Click to show calendar" ImageUrl="/images/Lemonaid/buttonsNew/datepicker.png" />
                                    <ACT:CalendarExtender ID="Cal_End_Date" runat="server" Format="yyyy/MM/dd" PopupButtonID="Img_EventDate_End1" TargetControlID="txtEndDate" />
                                </div>
                                &nbsp;&nbsp;<asp:DropDownList ID="ddlEndTime" runat="server" CssClass="dropdownlist" />

                            </td></tr>      
                            <tr><td class="admin-prompt-right">&nbsp;</td><td style="padding-top: 15px;"><asp:RadioButton ID="rbOpen2" runat="server" CssClass="rb-enhanced" GroupName="rbgOpen2" Text="Same Window" Checked="true" /><asp:RadioButton ID="rbOpenNew2" runat="server" CssClass="rb-enhanced" GroupName="rbgOpen2" Text="New Window" /></td></tr>
                            <tr><td class="admin-prompt-right">Class</td><td>
                                <asp:DropDownList ID="ddlClass" runat="server" CssClass="dropdownlist">                                   
                                    <asp:ListItem Value="contained" Text="Contained"></asp:ListItem>
                                    <asp:ListItem Value="full-width" Text="Full Width"></asp:ListItem>
                                </asp:DropDownList></td></tr>
                        

                        </table>
                    </div>
                </div>
                <br />
                <asp:LinkButton ID="ImageOverButton1" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="imbBannerEditCancel_Click" CausesValidation="false" />
                <asp:LinkButton ID="bttn_Add_Question" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="imgBannerEditSave_Click2" ValidationGroup="grBanner" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="grBanner" />
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
        var totalid = $("#<%= GV_Banners.ClientID %> tr td input[class='HfId']").length;
        for (var i = 0; i < totalid; i++) {
            strorder += $("#<%= GV_Banners.ClientID %> tr td input[class='HfId']")[i].getAttribute("value") + "|";
        }
        //alert("totalid: " + totalid + " - strorder: " + strorder);
    }

    function BindControlEvents() {
        $('#<%= GV_Banners.ClientID %>').tableDnD(
        {
            onDragClass: "myDragClass",
            onDrop: function (table, row) {
                //alert("Test");
                reorder();
                $.ajax({
                    type: "POST",
                    //url: "testpga.aspx/PhotosReorders",
                    url: "admin.aspx/BannersReorders",
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
        $('#<%= GV_Banners.ClientID %>').find('tr').each(function () {
            var id = $(this).find("input[class='HfId']").val();


            if (id != null && id != 'undefined')
                $(this).attr('id', 'tr_' + id);
        });

        $("#<%= GV_Banners.ClientID %> tr[id^='tr_']").hover(function () {
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