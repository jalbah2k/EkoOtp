<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PhotoGalleryAddDnD.ascx.cs" Inherits="Controls_PhotoGalleryAdd_PhotoGalleryAdd" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<style type="text/css">
    .req {color:red; font-size: 150%;}
    #pnlPhotoGalleryAddWrapper
    {
        position:relative;
    }
    #pnlPhotoGalleryAddWrapper .btnPhotoGalleryAdd
    {
        position:absolute;
        top:-40px;
        left:150px;
        z-index:100;
        display:none;
    }
    #pnlPhotoGalleryAdd
    {
        position:relative;
        display:none;
        padding:20px;
        border:1px dashed #004b85;
    }
    #pnlPhotoGalleryAdd .textbox
    {
        width:50%;
        min-width:276px;
    }
    #pnlPhotoGalleryAdd .PhotoGalleryBtns
    {
	    font-size:12px;
	    font-family:calibri;
	    text-decoration:none;
	    color: #ffffff;
        background-color:#4398cf;
        border:1px solid #004b85;
        cursor:pointer;
    }
    #pnlPhotoGalleryAdd .watermarked, #pnlPhotoGalleryAdd .watermarked2
    {
	    /*background-color:Transparent;
	    border:0px;*/
	    font-family:Verdana, Arial, Tahoma, Sans-Serif;
	    font-size : 12px;
        font-style: italic;
        color: Gray;
	    vertical-align:middle;
    }
    #pnlPhotoGalleryAdd .watermarked2
    {
	    background-color:Transparent;
	    border:0px;
    }

    #pnlPhotoGalleryAdd .pg_item
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
    #pnlPhotoGalleryAdd .pg_item.custom_cursor
    {
        cursor: url(https://mail.google.com/mail/images/2/openhand.cur), move;
        /*cursor: url(../../images/openhand.cur), move;*/
    }
    #pnlPhotoGalleryAdd .itemContainer
    {
        position:relative;
        margin:10px;
        width:284px;
        float:left;
    }
    #pnlPhotoGalleryAdd .delPhoto
    {
        position:absolute;
        top:-10px;
        right:-10px;
        cursor:pointer;
    }
    #pnlPhotoGalleryAdd .pg_item td
    {
        border:1px solid #CCCCCC;
    }
    #pnlPhotoGalleryAdd .Caption
    {
        background-color:transparent;
        border:0px;
    }
    #pnlPhotoGalleryAddWrapper br.clear
    {
        display:block;
        clear:both;
    }
    .dlPhotos br
    {
        display:none;
    }
</style>
<%--<asp:ScriptManager ID="sman" runat="server" EnablePageMethods="true" >
    <Scripts>
         <asp:ScriptReference Path="~/js/webkit.js" />
    </Scripts>
</asp:ScriptManager>--%>
<div id="pnlPhotoGalleryAddWrapper">
    <div id="btnPhotoGalleryAdd" class="btnPhotoGalleryAdd" runat="server" visible="false"><a href="javascript:void(0)" class="BtnAddAlbum"><img src="/images/lemonaid/buttons/addalbum.png" border="0" alt="Add Album" width="150" height="30" /></a></div>
    <div id="pnlPhotoGalleryAdd">
        <p> Required fields are marked with an asterisk (<abbr class="req" title="required">*</abbr>).</p><br />
        <div><abbr class="req" title="required">*</abbr>&nbsp;<asp:TextBox ID="txtName" CssClass="textbox" title="Photo gallery name" runat="server" MaxLength="500"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Album name required" ControlToValidate="txtName" Display="Dynamic" SetFocusOnError="true" ValidationGroup="PhotoGalleryAdd"> *</asp:RequiredFieldValidator><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtName" WatermarkText="Untitled Album" WatermarkCssClass="watermarked txtPhotoGalleryName" runat="server" ID="NameWMExtender" BehaviorID="wmePhotoGalleryName"></ajaxToolkit:TextBoxWatermarkExtender></div>
        <br />
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
              <div><asp:Image ID="ImgSpinner" runat="server" ImageUrl="~/images/ajax_loader_small.gif" ImageAlign="Middle" GenerateEmptyAlternateText="true" /></div><br />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <CuteWebUI:Uploader ID="ImagesUploader" runat="server" InsertButtonStyle-CssClass="PhotoGalleryBtns" CancelButtonStyle-CssClass="PhotoGalleryBtns" InsertText="Upload Photos" MultipleFilesUpload="true" OnFileUploaded="fileup" OnUploadCompleted="refresh" UploadingMsg="Uploading...">
                <ValidateOption AllowedFileExtensions="jpeg,png,jpg,gif" />
            </CuteWebUI:Uploader>
            <div id="ImgPanel" style="position:relative;">
                <%--<asp:DataList ID="dlPhotos" CssClass="dlPhotos" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" 
                    ShowFooter="False" ShowHeader="False" CellSpacing="10" 
                    onitemdatabound="dlPhotos_ItemDataBound" 
                    DataKeyField="id" ondeletecommand="dlPhotos_DeleteCommand">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" CssClass="itemContainer"/>
                <ItemTemplate>
                    <div class="pg_item">
                        <div class="delPhoto"><asp:ImageButton ID="btnDeletePhoto" runat="server" CommandName="delete" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/icons/delete.png" AlternateText="delete photo" ToolTip="Delete Photo" /></div>
                        <table class="bodytext" border="0" cellpadding="2" cellspacing="0" width="280px">
                        <tr><td style="text-align:center;"><asp:Image ID="Photo" runat="server" AlternateText="photo" Width="250px" /></td></tr>
                        <tr><td><asp:TextBox ID="txtCaption" title="Caption" CssClass="Caption" runat="server" Width="276px" MaxLength="200" ToolTip="Caption"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtCaption" WatermarkText="Say something about this photo..." WatermarkCssClass="watermarked2" runat="server" Enabled="True" ID="CaptionWMExtender"></ajaxToolkit:TextBoxWatermarkExtender></td></tr>
                        <tr><td><asp:TextBox ID="txtAltText" title="Alternative Text" CssClass="Caption" runat="server" Width="276px" MaxLength="50" ToolTip="Alternative Text"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtAltText" WatermarkText="Alternative text" WatermarkCssClass="watermarked2" runat="server" Enabled="True" ID="TextBoxWatermarkExtender1"></ajaxToolkit:TextBoxWatermarkExtender></td></tr>
                        </table>
                    </div>
                </ItemTemplate>
                </asp:DataList>--%>
                <asp:Repeater ID="dlPhotos" runat="server" onitemcommand="dlPhotos_ItemCommand" 
                    onitemdatabound="dlPhotos_ItemDataBound1">
                <ItemTemplate>
                    <div id='pg_item_<%# Eval("id") %>' class="pg_item">
                        <div class="delPhoto"><asp:ImageButton ID="btnDeletePhoto" runat="server" CommandName="delete" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/icons/delete.png" AlternateText="delete photo" ToolTip="Delete Photo" /></div>
                        <table class="bodytext" border="0" cellpadding="2" cellspacing="0" width="280px">
                        <tr><td style="text-align:center;"><asp:Image ID="Photo" runat="server" AlternateText="photo" Width="250px" /></td></tr>
                        <tr><td><asp:TextBox ID="txtCaption" title="Caption" CssClass="Caption" runat="server" Width="276px" MaxLength="200" ToolTip="Caption"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtCaption" WatermarkText="Say something about this photo..." WatermarkCssClass="watermarked2" runat="server" Enabled="True" ID="CaptionWMExtender"></ajaxToolkit:TextBoxWatermarkExtender></td></tr>
                        <tr><td><asp:TextBox ID="txtAltText" title="Alternative Text" CssClass="Caption" runat="server" Width="276px" MaxLength="50" ToolTip="Alternative Text"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtAltText" WatermarkText="Alternative text" WatermarkCssClass="watermarked2" runat="server" Enabled="True" ID="TextBoxWatermarkExtender1"></ajaxToolkit:TextBoxWatermarkExtender></td></tr>
                        </table>
                    </div>
                </ItemTemplate>
                </asp:Repeater>
                <br class="clear" />
            </div>
        <div style="padding-top:30px;"><asp:Button ID="btnSave" class="PhotoGalleryBtns" runat="server" Text="Save" onclick="btnSave_Click" ValidationGroup="PhotoGalleryAdd" />&nbsp;&nbsp;<asp:Button ID="btnCancel" class="PhotoGalleryBtns" runat="server" Text="Cancel" onclick="btnCancel_Click" CausesValidation="false" /><asp:Button ID="btnEdit" class="btnFrontPhotoGalleryEdit hide" style="display:none;" runat="server" Text="Edit" onclick="btnEdit_Click" CausesValidation="false" /></div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="PhotoGalleryAdd" />
        </ContentTemplate>
        </asp:UpdatePanel>
        <asp:TextBox ID="hfTempFolder" title="Temp Folder" class="hfTempFolder hide" runat="server"></asp:TextBox>
        <asp:HiddenField ID="hfSort" runat="server" />
        <asp:TextBox ID="hfPhotoParameters" title="Parameters" class="hfPhotoParameters hide" runat="server" Text="0,0"></asp:TextBox> <%-- Parameters ZoneId,Priority --%>
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
        $('#ImgPanel').children('div').each(function () {
            IdList += $(this).attr('id') + ',';
        });
        IdList = IdList.substring(0, (IdList.length - 1));

        return IdList;
    }*/

    function BindPGControlEvents() {

        if (IEVersion > 0) {
            $('.pg_item').addClass('custom_cursor');
        }

        $('#<%= btnCancel.ClientID %>').click(function (e) {
            $('#pnlPhotoGalleryAdd').slideUp(600);

            //return false;
            //e.preventDefault();
        });

        /*var tdImg = $("#<%= dlPhotos.ClientID %> .itemContainer");
        tdImg.attr("style",'width:284px;float:left;');*/

        $("#ImgPanel").sortable({
            cursor: 'grabbing'
            //, cursorAt: { top: 1 }
            , revert: true
            , update: function (e, ui) {
                //var ReorderedIds = GetReorderedIds();
                var ReorderedIds = $("#ImgPanel").sortable("toArray").toString().replace(/pg_item_/gi, "");
                ReorderedIds = ReorderedIds.substring(0, (ReorderedIds.length - 1));
                //alert(ReorderedIds);
                $('#<%= hfSort.ClientID %>').val(ReorderedIds);

                //return false;
            }
        });
        //$("#ImgPanel").disableSelection();

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
            $("#ImgPanel").sortable("option", "cursor", DraggingCursor);
        }
    }

    //Initial bind
    $(document).ready(function () {
        $('.BtnAddAlbum').click(function (e) {
            //$('#pnlPhotoGalleryAdd').slideToggle(600);
            if (!$('#pnlPhotoGalleryAdd').is(':visible')) {
                /*PageMethods.PhotoGalleryNew(function (retVal) {
                    $('#<%= hfTempFolder.ClientID %>').val(retVal);
                });*/
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/PhotoGalleryNew",
                    data: '{}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('#<%= hfTempFolder.ClientID %>').val(response.d);
                    }
                });
                $find('wmePhotoGalleryName').set_Text('');
                $('#pnlPhotoGalleryAdd').slideDown(600);
            }

            return false;
        });

        BindPGControlEvents();
    });

    //Re-bind for callbacks 
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        BindPGControlEvents();
    });

    /*prm.add_initializeRequest(function () {
    var ImgPanelTop = $('#ImgPanel').offset().top;
    var HRImgPanelTop = $('#HRImgPanel').offset().top;

    $('.uprImages').css('top', ImgPanelTop);
    $('.uprHRImages').css('top', HRImgPanelTop);
    });*/

</script>
