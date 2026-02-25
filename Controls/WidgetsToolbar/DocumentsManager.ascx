<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DocumentsManager.ascx.cs" Inherits="Controls_DocumentsManager_DocumentsManager" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>
<style type="text/css">
    .hide{
        display:none;
    }
    .req {color:red; font-size: 150%;}
    #pnlDocumentsAddWrapper
    {
        position:relative;
    }
    #pnlDocumentsAddWrapper .btnDocumentsAdd
    {
        position:absolute;
        top:-40px;
        left:150px;
        z-index:100;
        display:none;
    }
    #pnlDocumentsAdd
    {
        position:relative;
        display:none;
        padding:20px;
        border:1px dashed #004b85;
    }
    #pnlDocumentsAdd #DocPanel
    {
        margin-top:10px;
    }
    #pnlDocumentsAdd .textbox
    {
        width:50%;
        min-width:276px;
    }
    #pnlDocumentsAdd .DocumentsBtns
    {
	    font-size:12px;
	    font-family:calibri;
	    text-decoration:none;
	    color: #ffffff;
        background-color:#4398cf;
        border:1px solid #004b85;
        cursor:pointer;
    }
    #pnlDocumentsAdd .watermarked, #pnlDocumentsAdd .watermarked2
    {
	    /*background-color:Transparent;
	    border:0px;*/
	    font-family:Verdana, Arial, Tahoma, Sans-Serif;
	    font-size : 12px;
        font-style: italic;
        color: Gray;
	    vertical-align:middle;
    }
    #pnlDocumentsAdd .watermarked2
    {
	    background-color:Transparent;
	    border:0px;
    }

    #pnlDocumentsAdd .doc_item
    {
        position:relative;
        /*width:284px;
        margin:10px;*/

        cursor: move;
        cursor:hand;
        cursor:grab;
        cursor:-moz-grab;
        cursor:-webkit-grab;
    }
    #pnlDocumentsAdd .doc_item.custom_cursor
    {
        cursor: url(https://mail.google.com/mail/images/2/openhand.cur), move;
        /*cursor: url(../../images/openhand.cur), move;*/
    }
    #pnlDocumentsAdd .Caption
    {
        background-color:transparent;
        border:0px;
    }
    #pnlDocumentsAddWrapper br.clear
    {
        display:block;
        clear:both;
    }
</style>
<%--<asp:ScriptManager ID="sman" runat="server" EnablePageMethods="true" >
    <Scripts>
         <asp:ScriptReference Path="~/js/webkit.js" />
    </Scripts>
</asp:ScriptManager>--%>
<div id="pnlDocumentsAddWrapper">
    <div id="btnDocumentsAdd" class="btnDocumentsAdd" runat="server" visible="false"><a href="javascript:void(0)" class="BtnAddDocuments"><img src="/images/lemonaid/buttons/addalbum.png" border="0" alt="Add Documents" width="150" height="30" /></a></div>
    <div id="pnlDocumentsAdd">
        <p> Required fields are marked with an asterisk (<abbr class="req" title="required">*</abbr>).</p><br />
        <div><abbr class="req" title="required">*</abbr>&nbsp;<asp:TextBox ID="txtName" CssClass="textbox" title="Documents name" runat="server" MaxLength="500"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Documents name required" ControlToValidate="txtName" Display="Dynamic" SetFocusOnError="true" ValidationGroup="DocumentsAdd"> *</asp:RequiredFieldValidator><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtName" WatermarkText="Documents name" WatermarkCssClass="watermarked" runat="server" ID="NameWMExtender" BehaviorID="wmeDocumentsName"></ajaxToolkit:TextBoxWatermarkExtender></div>
        <div><abbr class="req" title="required" style="color:#fff;">*</abbr>&nbsp;<asp:TextBox ID="txtTitle" CssClass="textbox" title="Documents title" runat="server" MaxLength="500"></asp:TextBox><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Documents name required" ControlToValidate="txtTitle" Display="Dynamic" SetFocusOnError="true" ValidationGroup="DocumentsAdd"> *</asp:RequiredFieldValidator>--%><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtTitle" WatermarkText="Documents title" WatermarkCssClass="watermarked" runat="server" ID="TitleWMExtender" BehaviorID="wmeDocumentsTitle"></ajaxToolkit:TextBoxWatermarkExtender></div>
        <div><abbr class="req" title="required">*</abbr>&nbsp;<asp:DropDownList ID="ddlGroup" CssClass="ddlDocumentGroup" runat="server"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Documents group required" ControlToValidate="ddlGroup" Display="Dynamic" SetFocusOnError="true" ValidationGroup="DocumentsAdd"> *</asp:RequiredFieldValidator></div>
        <br />
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
              <div><asp:Image ID="ImgSpinner" runat="server" ImageUrl="~/images/ajax_loader_small.gif" ImageAlign="Middle" GenerateEmptyAlternateText="true" /></div><br />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <CuteWebUI:Uploader ID="ImagesUploader" runat="server" InsertButtonStyle-CssClass="DocumentsBtns" CancelButtonStyle-CssClass="DocumentsBtns" InsertText="Upload Documents" MultipleFilesUpload="true" OnFileUploaded="fileup" OnUploadCompleted="refresh" UploadingMsg="Uploading...">
                <%--<ValidateOption AllowedFileExtensions="jpeg,png,jpg,gif" />--%>
            </CuteWebUI:Uploader>
            <div id="DocPanel" style="position:relative;">
                <asp:Repeater ID="repDocuments" runat="server" onitemcommand="repDocuments_ItemCommand" onitemdatabound="repDocuments_ItemDataBound">
                <ItemTemplate>
                    <div id='doc_<%# Eval("id") %>' class="doc_item">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Document name required" ControlToValidate="txtDocName" Display="Dynamic" SetFocusOnError="true" ValidationGroup="DocumentsAdd">* </asp:RequiredFieldValidator>
                        <asp:Image ID="imgStatus" runat="server" ImageUrl="/images/icons/types/file.png" AlternateText='<%# Eval("name") %>' ToolTip='<%# Eval("mime") %>'/>&nbsp;
                        <asp:TextBox ID="txtDocName" title="Document name" CssClass="Caption textbox" runat="server" Text='<%# Eval("name") %>' MaxLength="500"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtDocName" WatermarkText="Document name" WatermarkCssClass="watermarked2" runat="server" Enabled="True" ID="DocNameWMExtender"></ajaxToolkit:TextBoxWatermarkExtender>
                        &nbsp;&nbsp;<asp:ImageButton ID="btnDeleteDocument" runat="server" CommandName="delete" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/icons/delete.png" AlternateText="delete document" ToolTip="Delete Document" style="cursor:pointer;" />
                    </div>
                </ItemTemplate>
                </asp:Repeater>
                <br class="clear" />
            </div>
        <div style="padding-top:30px;"><asp:Button ID="btnSave" class="DocumentsBtns" runat="server" Text="Save" onclick="btnSave_Click" ValidationGroup="DocumentsAdd" />&nbsp;&nbsp;<asp:Button ID="btnCancel" class="DocumentsBtns" runat="server" Text="Cancel" onclick="btnCancel_Click" CausesValidation="false" /><asp:Button ID="btnEdit" class="btnFrontDocumentsEdit hide" style="display:none;" runat="server" Text="Edit" onclick="btnEdit_Click" CausesValidation="false" /></div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="DocumentsAdd" />
        </ContentTemplate>
        </asp:UpdatePanel>
        <asp:TextBox ID="hfTempFolder" title="Temp Folder" class="hfDocTempFolder hide" runat="server"></asp:TextBox>
        <asp:HiddenField ID="hfSort" runat="server" />
        <asp:TextBox ID="hfDocParameters" title="Parameters" class="hfDocParameters hide" runat="server" Text="0,0"></asp:TextBox> <%-- Parameters ZoneId,Priority --%>
        <div id="divSubmitted" class="hide" runat="server" style="position:fixed; left:0; top:0; z-index:99999; width:100%; height:100%; min-height:100%;">
            <div class="SemiTransparentBg">&nbsp;</div> <%-- Set the semitransparent background for the parent div in order to allow opaque content --%>
            <div style="position:absolute; top:50%; width: 100%; text-align: center;">
                <div id="inner" style="position:relative; top:-95px; margin-left: auto; margin-right: auto;display: grid;justify-content: center;"> <%-- Set top = -height/2 to center the inner div --%>
                    <span class="title" style="color:#ff0000; font-weight:bold; vertical-align:middle;">We are processing your submission.<br />Please do not press back or refresh.</span><br /><br />
                    <asp:Image ID="imgSubmitted" runat="server" ImageUrl="/images/ajax_loader_large.gif" AlternateText="spinner" ImageAlign="Middle" GenerateEmptyAlternateText="true" />
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">

    /*function GetReorderedIds() {
        var IdList = '';
        $('#DocPanel').children('div').each(function () {
            IdList += $(this).attr('id') + ',';
        });
        IdList = IdList.substring(0, (IdList.length - 1));

        return IdList;
    }*/

    function BindDocControlEvents() {

        if (IEVersion > 0) {
            $('.doc_item').addClass('custom_cursor');
        }

        $('#<%= btnCancel.ClientID %>').click(function (e) {
            $('#pnlDocumentsAdd').slideUp(600);

            //return false;
            //e.preventDefault();
        });

        $("#DocPanel").sortable({
            cursor: 'grabbing'
            , revert: true
            , update: function (e, ui) {
                //var ReorderedIds = GetReorderedIds();
                var ReorderedIds = $("#DocPanel").sortable("toArray").toString().replace(/doc_/gi, "");
                ReorderedIds = ReorderedIds.substring(0, (ReorderedIds.length - 1));
                //alert(ReorderedIds);
                $('#<%= hfSort.ClientID %>').val(ReorderedIds);
                //return false;
            }
        });
        //$("#DocPanel").disableSelection();

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
            $("#DocPanel").sortable("option", "cursor", DraggingCursor);
        }
    }

    //Initial bind
    $(document).ready(function () {
        $('.BtnAddDocuments').click(function (e) {
            //$('#pnlDocumentsAdd').slideToggle(600);
            if (!$('#pnlDocumentsAdd').is(':visible')) {
                /*PageMethods.PhotoGalleryNew(function (retVal) {
                    $('#< %= hfTempFolder.ClientID %>').val(retVal);
                });*/
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/DocumentsNew",
                    data: '{}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('#<%= hfTempFolder.ClientID %>').val(response.d);
                    }
                });
                $find('wmeDocumentsName').set_Text('');
                $find('wmeDocumentsTitle').set_Text('');
                $('#<%= ddlGroup.ClientID %> option:first-child').attr("selected", "selected");
                $('#pnlDocumentsAdd').slideDown(600);
            }

            return false;
        });

        BindDocControlEvents();
    });

    //Re-bind for callbacks 
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        BindDocControlEvents();
    });

    /*prm.add_initializeRequest(function () {
    var ImgPanelTop = $('#DocPanel').offset().top;
    var HRImgPanelTop = $('#HRImgPanel').offset().top;

    $('.uprImages').css('top', ImgPanelTop);
    $('.uprHRImages').css('top', HRImgPanelTop);
    });*/

</script>
