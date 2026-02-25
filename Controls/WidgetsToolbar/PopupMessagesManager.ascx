<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PopupMessagesManager.ascx.cs" Inherits="Controls_PopupMessagesManager_PopupMessagesManager" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>
<style type="text/css">
    .req {color:red; font-size: 150%;}
    #pnlPopupMessagesAddWrapper
    {
        position:relative;
    }
    #pnlPopupMessagesAddWrapper .btnPopupMessagesAdd
    {
        position:absolute;
        top:-40px;
        left:150px;
        z-index:100;
        display:none;
    }
    #pnlPopupMessagesAdd
    {
        position:relative;
        display:none;
        padding:20px;
        border:1px dashed #004b85;
    }
    #pnlPopupMessagesAdd .textbox
    {
        width:50%;
        min-width:276px;
    }
    #pnlPopupMessagesAdd .PopupMessagesBtns
    {
	    font-size:12px;
	    font-family:calibri;
	    text-decoration:none;
	    color: #ffffff;
        background-color:#4398cf;
        border:1px solid #004b85;
        cursor:pointer;
    }
    #pnlPopupMessagesAdd .watermarked, #pnlPopupMessagesAdd .watermarked2
    {
	    /*background-color:Transparent;
	    border:0px;*/
	    font-family:Verdana, Arial, Tahoma, Sans-Serif;
	    font-size : 12px;
        font-style: italic;
        color: Gray;
	    vertical-align:middle;
    }
    #pnlPopupMessagesAdd .watermarked2
    {
	    background-color:Transparent;
	    border:0px;
    }
</style>
<%--<asp:ScriptManager ID="sman" runat="server" EnablePageMethods="true" >
    <Scripts>
         <asp:ScriptReference Path="~/js/webkit.js" />
    </Scripts>
</asp:ScriptManager>--%>
<div id="pnlPopupMessagesAddWrapper">
    <div id="btnPopupMessagesAdd" class="btnPopupMessagesAdd" runat="server" visible="false"><a href="javascript:void(0)" class="BtnAddPopupMessages"><img src="/images/lemonaid/buttons/addalbum.png" border="0" alt="Add Popup Messages" width="150" height="30" /></a></div>
    <div id="pnlPopupMessagesAdd">
        <p> Required fields are marked with an asterisk (<abbr class="req" title="required">*</abbr>).</p><br />
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
              <div><asp:Image ID="ImgSpinner" runat="server" ImageUrl="~/images/ajax_loader_small.gif" ImageAlign="Middle" GenerateEmptyAlternateText="true" /></div><br />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div><abbr class="req" title="required">*</abbr>&nbsp;<asp:TextBox ID="txtName" CssClass="textbox" title="Popup Message name" runat="server" MaxLength="500"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Popup Message name required" ControlToValidate="txtName" Display="Dynamic" SetFocusOnError="true" ValidationGroup="PopupMessagesAdd"> *</asp:RequiredFieldValidator><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtName" WatermarkText="Popup message name" WatermarkCssClass="watermarked" runat="server" ID="NameWMExtender" BehaviorID="wmePopupMessagesName"></ajaxToolkit:TextBoxWatermarkExtender></div>
            <br />
            <CE:Editor CssClass="bodytext" id="txtContent" AutoParseClasses="false" runat="server" Width="600" height="300"/>
        <div style="padding-top:30px;"><asp:Button ID="btnSave" class="PopupMessagesBtns" runat="server" Text="Save" onclick="btnSave_Click" ValidationGroup="PopupMessagesAdd" />&nbsp;&nbsp;<asp:Button ID="btnCancel" class="PopupMessagesBtns" runat="server" Text="Cancel" CausesValidation="false" /><asp:Button ID="btnEdit" class="btnFrontPopupMessagesEdit hide" style="display:none;" runat="server" Text="Edit" onclick="btnEdit_Click" CausesValidation="false" /></div>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="PopupMessagesAdd" />
        </ContentTemplate>
        </asp:UpdatePanel>
        <asp:TextBox ID="hfTempFolder" title="Temp Folder" class="hfPMTempFolder hide" runat="server"></asp:TextBox>
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

    function BindPMControlEvents() {

        $('#<%= btnCancel.ClientID %>').click(function (e) {
            $('#pnlPopupMessagesAdd').slideUp(600);

            return false;
            //e.preventDefault();
        });

    }

    //Initial bind
    $(document).ready(function () {
        $('.BtnAddPopupMessages').click(function (e) {
            if (!$('#pnlPopupMessagesAdd').is(':visible')) {
                $('#<%= hfTempFolder.ClientID %>').val('temp');

                $find('wmePopupMessagesName').set_Text('');
                var txtContent = document.getElementById('<%= txtContent.ClientID %>');
                txtContent.setHTML("<div></div>");

                $('#pnlPopupMessagesAdd').slideDown(600);
            }

            return false;
        });

        BindPMControlEvents();
    });

    //Re-bind for callbacks 
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        BindPMControlEvents();
    });

    /*prm.add_initializeRequest(function () {
    var ImgPanelTop = $('#DocPanel').offset().top;
    var HRImgPanelTop = $('#HRImgPanel').offset().top;

    $('.uprImages').css('top', ImgPanelTop);
    $('.uprHRImages').css('top', HRImgPanelTop);
    });*/

</script>
