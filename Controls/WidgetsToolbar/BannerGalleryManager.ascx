<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BannerGalleryManager.ascx.cs" Inherits="Controls_BannerGalleryManager_BannerGalleryManager" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<style type="text/css">
    .req {color:red; font-size: 150%;}
    #pnlBannerGalleryAddWrapper
    {
        position:relative;
    }
    #pnlBannerGalleryAddWrapper .btnBannerGalleryAdd
    {
        position:absolute;
        top:-40px;
        left:150px;
        z-index:100;
        display:none;
    }
    #pnlBannerGalleryAdd
    {
        position:relative;
        display:none;
        padding:20px;
        border:1px dashed #004b85;
    }
    #pnlBannerGalleryAdd .textbox
    {
        width:50%;
        min-width:276px;
    }
    #pnlBannerGalleryAdd .BannerGalleryBtns
    {
	    font-size:12px;
	    font-family:calibri;
	    text-decoration:none;
	    color: #ffffff;
        background-color:#4398cf;
        border:1px solid #004b85;
        cursor:pointer;
    }
    #pnlBannerGalleryAdd .watermarked, #pnlBannerGalleryAdd .watermarked2
    {
	    /*background-color:Transparent;
	    border:0px;*/
	    font-family:Verdana, Arial, Tahoma, Sans-Serif;
	    font-size : 12px;
        font-style: italic;
        color: Gray;
	    vertical-align:middle;
    }
    #pnlBannerGalleryAdd .watermarked2
    {
	    background-color:Transparent;
	    border:0px;
    }

    #pnlBannerGalleryAdd .bg_item
    {
        position:relative;
        width:284px;
        margin:10px;
        float:left;

        cursor: move;
        cursor:hand;
        cursor:grab;
        cursor:-moz-grab;
        cursor:-webkit-grab;
    }
    #pnlBannerGalleryAdd .bg_item.custom_cursor
    {
        cursor: url(https://mail.google.com/mail/images/2/openhand.cur), move;
        /*cursor: url(../../images/openhand.cur), move;*/
    }
    /*#pnlBannerGalleryAdd .bg_item.grabbing
    {
        cursor: grabbing;
        cursor: -moz-grabbing;
        cursor: -webkit-grabbing;
    }
    #pnlBannerGalleryAdd .bg_item.grabbing.custom_cursor
    {
        cursor: url(../../images/closedhand.cur), move !important;
    }*/
    #pnlBannerGalleryAdd .itemContainer
    {
        position:relative;
        margin:10px;
        width:284px;
        float:left;
    }
    #pnlBannerGalleryAdd .delBanner
    {
        position:absolute;
        top:-10px;
        right:-10px;
        cursor:pointer;
    }
    #pnlBannerGalleryAdd .bg_item td
    {
        border:1px solid #CCCCCC;
    }
    #pnlBannerGalleryAdd .Caption
    {
        background-color:transparent;
        border:0px;
    }
    #pnlBannerGalleryAddWrapper br.clear
    {
        display:block;
        clear:both;
    }
    .dlBanners br
    {
        display:none;
    }
</style>
<%--<asp:ScriptManager ID="sman" runat="server" EnablePageMethods="true" >
    <Scripts>
         <asp:ScriptReference Path="~/js/webkit.js" />
    </Scripts>
</asp:ScriptManager>--%>
<div id="pnlBannerGalleryAddWrapper">
    <div id="btnBannerGalleryAdd" class="btnBannerGalleryAdd" runat="server" visible="false"><a href="javascript:void(0)" class="BtnAddBannerGallery"><img src="/images/lemonaid/buttons/addalbum.png" border="0" alt="Add Album" width="150" height="30" /></a></div>
    <div id="pnlBannerGalleryAdd">
        <p> Required fields are marked with an asterisk (<abbr class="req" title="required">*</abbr>).</p><br />
        <div><abbr class="req" title="required">*</abbr>&nbsp;<asp:TextBox ID="txtName" CssClass="textbox" title="Banner gallery name" runat="server" MaxLength="500"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Banner gallery name required" ControlToValidate="txtName" Display="Dynamic" SetFocusOnError="true" ValidationGroup="BannerGalleryAdd"> *</asp:RequiredFieldValidator><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtName" WatermarkText="Untitled Banner Gallery" WatermarkCssClass="watermarked txtBannerGalleryName" runat="server" ID="NameWMExtender" BehaviorID="wmeBannerGalleryName"></ajaxToolkit:TextBoxWatermarkExtender></div>
        <br />
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
              <div><asp:Image ID="ImgSpinner" runat="server" ImageUrl="~/images/ajax_loader_small.gif" ImageAlign="Middle" GenerateEmptyAlternateText="true" /></div><br />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <CuteWebUI:Uploader ID="ImagesUploader" runat="server" InsertButtonStyle-CssClass="BannerGalleryBtns" CancelButtonStyle-CssClass="BannerGalleryBtns" InsertText="Upload Banners" MultipleFilesUpload="true" OnFileUploaded="fileup" OnUploadCompleted="refresh" UploadingMsg="Uploading..." TempDirectory="~/uploads/banners">
                <ValidateOption AllowedFileExtensions="jpeg,png,jpg,gif" />
            </CuteWebUI:Uploader>
            <div id="BannerImgPanel" style="position:relative;">
                <asp:Repeater ID="dlBanners" runat="server" onitemcommand="dlBanners_ItemCommand" 
                    onitemdatabound="dlBanners_ItemDataBound1">
                <ItemTemplate>
                    <div id='bg_item_<%# Eval("id") %>' class="bg_item">
                        <div class="delBanner"><asp:ImageButton ID="btnDeleteBanner" runat="server" CommandName="delete" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/icons/delete.png" AlternateText="delete Banner" ToolTip="Delete Banner" /></div>
                        <table class="bodytext" border="0" cellpadding="2" cellspacing="0" width="280px">
                        <tr><td style="text-align:center;"><asp:Image ID="Banner" runat="server" AlternateText="Banner" Width="250px" /></td></tr>
                        <tr><td><asp:TextBox ID="txtBannerName" title="Banner name" CssClass="Caption" runat="server" Width="276px" MaxLength="200" ToolTip="Banner name"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtBannerName" WatermarkText="Banner name" WatermarkCssClass="watermarked2" runat="server" Enabled="True" ID="TextBoxWatermarkExtender2"></ajaxToolkit:TextBoxWatermarkExtender></td></tr>
                        <tr><td><asp:TextBox ID="txtCaption" title="Banner caption" CssClass="Caption" runat="server" Width="276px" MaxLength="200" ToolTip="Banner caption"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtCaption" WatermarkText="Banner caption" WatermarkCssClass="watermarked2" runat="server" Enabled="True" ID="CaptionWMExtender"></ajaxToolkit:TextBoxWatermarkExtender></td></tr>
                        <tr><td><asp:TextBox ID="txtURL" title="Banner URL" CssClass="Caption" runat="server" Width="276px" MaxLength="50" ToolTip="Banner URL"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtURL" WatermarkText="Banner URL" WatermarkCssClass="watermarked2" runat="server" Enabled="True" ID="TextBoxWatermarkExtender1"></ajaxToolkit:TextBoxWatermarkExtender></td></tr>
                        </table>
                    </div>
                </ItemTemplate>
                </asp:Repeater>
                <br class="clear" />
            </div>
        <div style="padding-top:30px;"><asp:Button ID="btnSave" class="BannerGalleryBtns" runat="server" Text="Save" onclick="btnSave_Click" ValidationGroup="BannerGalleryAdd" />&nbsp;&nbsp;<asp:Button ID="btnCancel" class="BannerGalleryBtns" runat="server" Text="Cancel" onclick="btnCancel_Click" CausesValidation="false" /><asp:Button ID="btnEdit" class="btnFrontBannerGalleryEdit hide" style="display:none;" runat="server" Text="Edit" onclick="btnEdit_Click" CausesValidation="false" /></div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="BannerGalleryAdd" />
        </ContentTemplate>
        </asp:UpdatePanel>
        <asp:TextBox ID="hfTempFolder" title="Temp Folder" class="hfBannerTempFolder hide" runat="server"></asp:TextBox>
        <asp:HiddenField ID="hfSort" runat="server" />
        <asp:TextBox ID="hfBannerParameters" title="Parameters" class="hfBannerParameters hide" runat="server" Text="0,0"></asp:TextBox> <%-- Parameters ZoneId,Priority --%>
        <div id="divSubmitted" class="hide" runat="server" style="position:fixed; left:0; top:0; z-index:99999; width:100%; height:100%; min-height:100%;">
            <div class="SemiTransparentBg">&nbsp;</div> <%-- Set the semitransparent background for the parent div in order to allow opaque content --%>
            <div style="position:absolute; top:50%; width: 100%; text-align: center;">
                <div id="inner" style="position:relative; top:-95px; margin-left: auto; margin-right: auto;"> <%-- Set top = -height/2 to center the inner div --%>
                    <span class="title" style="color:#ff0000; font-weight:bold; vertical-align:middle;">We are processing your submission.<br />Please do not press back or refresh.</span><br /><br />
                    <asp:Image ID="imgSubmitted" runat="server" ImageUrl="/images/ajax_loader_large.gif" ImageAlign="Middle" GenerateEmptyAlternateText="true" />
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">

    /*function GetReorderedIds() {
        var IdList = '';
        $('#BannerImgPanel').children('div').each(function () {
            IdList += $(this).attr('id') + ',';
        });
        IdList = IdList.substring(0, (IdList.length - 1));

        return IdList;
    }*/

    function BindBGControlEvents() {

        if (IEVersion > 0) {
            $('.bg_item').addClass('custom_cursor');
        }

        $('#<%= btnCancel.ClientID %>').click(function (e) {
            $('#pnlBannerGalleryAdd').slideUp(600);

            //return false;
            //e.preventDefault();
        });

        /*var tdImg = $("#<%= dlBanners.ClientID %> .itemContainer");
        tdImg.attr("style",'width:284px;float:left;');*/

        $("#BannerImgPanel").sortable({
            cursor: 'grabbing'
            , revert: true
            , update: function (e, ui) {
                //var ReorderedIds = GetReorderedIds();
                var ReorderedIds = $("#BannerImgPanel").sortable("toArray").toString().replace(/bg_item_/gi, "");
                ReorderedIds = ReorderedIds.substring(0, (ReorderedIds.length - 1));
                //alert(ReorderedIds);
                $('#<%= hfSort.ClientID %>').val(ReorderedIds);

                //return false;
            }
        });
        //$("#BannerImgPanel").disableSelection();

        var DraggingCursor = null;
        //Check if browser is IE
        if (IEVersion > 0) {
            DraggingCursor = "url(https://mail.google.com/mail/images/2/closedhand.cur), move";
        }
        //Check if browser is Chrome or not
        else if (navigator.userAgent.search("Chrome") >= 0 || navigator.userAgent.search("Safari") >= 0) {
            DraggingCursor = "-webkit-grabbing";
        }
        //Check if browser is Opera or not
        else if (navigator.userAgent.search("Opera") >= 0) {
            DraggingCursor = "move";
        }
        if (DraggingCursor != null) {
            $("#BannerImgPanel").sortable("option", "cursor", DraggingCursor);
        }
    }

    //Initial bind
    $(document).ready(function () {
        $('.BtnAddBannerGallery').click(function (e) {
            //$('#pnlBannerGalleryAdd').slideToggle(600);
            if (!$('#pnlBannerGalleryAdd').is(':visible')) {
                /*PageMethods.BannerGalleryNew(function (retVal) {
                    $('#<%= hfTempFolder.ClientID %>').val(retVal);
                });*/
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/BannerGalleryNew",
                    data: '{}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('#<%= hfTempFolder.ClientID %>').val(response.d);
                    }
                });
                $find('wmeBannerGalleryName').set_Text('');
                $('#pnlBannerGalleryAdd').slideDown(600);
            }

            return false;
        });

        BindBGControlEvents();
    });

    //Re-bind for callbacks 
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        BindBGControlEvents();
    });

    /*prm.add_initializeRequest(function () {
    var ImgPanelTop = $('#ImgPanel').offset().top;
    var HRImgPanelTop = $('#HRImgPanel').offset().top;

    $('.uprImages').css('top', ImgPanelTop);
    $('.uprHRImages').css('top', HRImgPanelTop);
    });*/

</script>
