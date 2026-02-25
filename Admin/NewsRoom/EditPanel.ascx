<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditPanel.ascx.cs" Inherits="Admin_NewsRoom_EditPanel" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register assembly="skmValidators" namespace="skmValidators" tagprefix="skm" %>

<script type="text/javascript" >
////        var winH = 520;
////        var winW = 860;

    $(document).ready(function () {

        $('#<%= ddlType.ClientID %>').change(function () {
            //alert($(this).val());
            if ($(this).val() == "0") {
                $('#<%=fulldescription.ClientID %>').show();
                $('#<%=fileuploader.ClientID %>').hide();
                $('#<%=trwebpage.ClientID %>').hide();
            }
            else if ($(this).val() == "1") {
                $('#<%=fulldescription.ClientID %>').hide();
                $('#<%=fileuploader.ClientID %>').show();
                $('#<%=trwebpage.ClientID %>').hide();
            }
            else {
                $('#<%=fulldescription.ClientID %>').hide();
                $('#<%=fileuploader.ClientID %>').hide();
                $('#<%=trwebpage.ClientID %>').show();
                checkExtInt_<%=Language %>();
            }
        });

        $('#<%=fulldescription.ClientID %>').toggleClass('hide', $('#<%= ddlType.ClientID %>').val() != "0");
        $('#<%=fileuploader.ClientID %>').toggleClass('hide', $('#<%= ddlType.ClientID %>').val() != "1");
        $('#<%=trwebpage.ClientID %>').toggleClass('hide', $('#<%= ddlType.ClientID %>').val() != "2");

    });

    function checkExtInt_<%=Language %>() {
        var trInternal = document.getElementById("<%=trInternal.ClientID %>");
        var trExternal = document.getElementById("<%=trExternal.ClientID %>");


        if (document.getElementById('<%=rbInternal.ClientID %>').checked) {
            trInternal.style.display = '';
            trExternal.style.display = 'none';
        } else if (document.getElementById('<%=rbExternal.ClientID %>').checked) {
            trInternal.style.display = 'none';
            trExternal.style.display = '';
        }
    }
</script>

<div>
    <asp:Label ID="ErrorMessage" runat="server" Visible="False" ForeColor="Red"></asp:Label>
    
	<table cellpadding="0" cellspacing="0" border="0">
	<tr><td class="admin-prompt-right"><span class="required">*</span>Title</td><td><asp:TextBox ID="tbTitle" runat="server" CssClass="textbox" Width="600"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbTitle" Display="Dynamic" ErrorMessage="Title is required" SetFocusOnError="True"> *</asp:RequiredFieldValidator></td></tr>
    <tr><td class="admin-prompt-right"><span class="required">*</span>Date</td><td><div class="datepicker-wraper"><asp:TextBox ID="tbDate" runat="server" /><asp:ImageButton ID="Img_EventDate_Start" runat="Server" AlternateText="Click to show calendar" CausesValidation="False" ImageUrl="/images/lemonaid/buttonsNew/datepicker.png" /><ACT:CalendarExtender ID="Cal_Start_Date" runat="server" Format="yyyy-MM-dd" PopupButtonID="Img_EventDate_Start" TargetControlID="tbDate" /></div><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbDate" Display="Dynamic" ErrorMessage="Date is required" SetFocusOnError="True"> *</asp:RequiredFieldValidator></td></tr>
    <tr><td class="admin-prompt-right">Go Live Date</td><td>
        <div class="datepicker-wraper"><asp:TextBox ID="tbDateLive" runat="server" /><asp:ImageButton ID="imgGoLiveBtn" runat="Server" AlternateText="Click to show calendar" CausesValidation="False" ImageUrl="/images/lemonaid/buttonsNew/datepicker.png" /><ACT:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" PopupButtonID="imgGoLiveBtn" TargetControlID="tbDateLive" /></div>
        &nbsp;<asp:DropDownList ID="ddlStartTime" runat="server" CssClass="dropdownlist" Width="105"/>
    </td></tr>

     <tr>
                <td class="admin-prompt-right" style="padding-top:0;vertical-align:middle;">
                    Categories
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="cblCategories" RepeatDirection="Horizontal" DataValueField="id" DataTextField="name" RepeatColumns="3" CellPadding="5"></asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rfvCategories" runat="server" ControlToValidate="cblCategories" Display="Dynamic" SetFocusOnError="false" ErrorMessage="Category is required"> *</asp:RequiredFieldValidator>
                </td>
            </tr>
    <tr style="display:none;"><td class="admin-prompt-right">Feature</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbFeature" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
    <tr style="display:none;"><td class="admin-prompt-right">Redevelopment</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbRedevelopment" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
	<%--<tr><td class="admin-prompt-right">Icon</td><td><asp:UpdatePanel runat="server"><ContentTemplate><asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="iconchanged" AutoPostBack="true" /><br /><asp:Image id="imgIcon" runat="server" /></ContentTemplate></asp:UpdatePanel></td></tr>--%>
    <%--<tr class="hide"><td class="admin-prompt-right">Layout</td><td><asp:DropDownList ID="ddlLayout" runat="server" CssClass="dropdownlist" Width="600"></asp:DropDownList></td></tr>--%>
	<tr><td class="admin-prompt-right">Type</td><td><asp:DropDownList ID="ddlType" runat="server" CssClass="dropdownlist" Width="600"><asp:ListItem Text="Content" Value="0" Selected="True" /><asp:ListItem Text="File" Value="1" /><asp:ListItem Text="Web page" Value="2"></asp:ListItem></asp:DropDownList></td></tr>
	<tr style="display:none;"><td class="admin-prompt-right">Feature Type</td><td><asp:DropDownList ID="ddlFeatureType" runat="server" CssClass="dropdownlist" Width="600"><asp:ListItem Text="Newsroom" Value="0" /><asp:ListItem Text="Homepage Highlight" Value="1" /></asp:DropDownList></td></tr>
    <tr style="display:none;"><td class="admin-prompt-right">Language</td><td><asp:DropDownList ID="ddlLang" runat="server" CssClass="dropdownlist" Width="600" /></td></tr>
    <tr><td class="admin-prompt-right">Image</td><td><asp:FileUpload runat="server" ID="fuImage" CssClass="fupload" />&nbsp;<asp:LinkButton ID="btnUpload" runat="server" CssClass="admin-button-blue" Text="Upload" CausesValidation="false" onclick="UploadImage_Click" style="color: #FFFFFF;" /><div id="pnlUploadedPhoto" runat="server"><asp:Image ID="imgUploadedPhoto" runat="server" CssClass="hide" style="width: 125px; margin-bottom: 15px;" /></div></td></tr>
	<tr id="trCurrentImage" runat="server" visible="false"><td class="admin-prompt-right">Current Image</td><td><asp:Image ID="imgCurrentImage" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton runat="server" ID="lnkDlete" Text="Delete Image" OnClick="lnkDlete_Click" ForeColor="#265892" Font-Underline="true" Font-Size="14px" ></asp:LinkButton></td></tr>
	<tr><td class="admin-prompt-right">Image Alternate Text</td><td><asp:TextBox ID="tbImgAltText" runat="server" CssClass="textbox" Width="600"></asp:TextBox></td></tr>
    <tr><td class="admin-prompt-right">Short Description<div class="admin-note">(225 characters maximum)</div></td><td style="padding-top: 15px;">
        <%--<CE:Editor id="Editor2" runat="server" CssClass="Count[225]" width="600" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor>--%>
        <asp:TextBox runat="server" ID="txtShortDesc" TextMode="MultiLine" Rows="4" CssClass="textbox" Width="600" Height="70" ></asp:TextBox>
        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Short description characters limit exceeded" OnServerValidate="CheckWordLimit" ClientValidationFunction="CheckWordLimit" ControlToValidate="txtShortDesc" EnableClientScript="true" SetFocusOnError="false" Display="Dynamic" ForeColor="Red"> Short description characters limit exceeded</asp:CustomValidator></td></tr>
    <tr id="fulldescription" runat="server"><td class="admin-prompt-right">Description</td><td style="padding-top: 15px;"><CE:Editor id="Editor1" runat="server" width="600" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor></td></tr>
    <tr id="fileuploader" class="hide" runat="server"><td class="admin-prompt-right">File</td><td><asp:FileUpload ID="fuFile" runat="server" CssClass="fupload" />&nbsp;&nbsp;&nbsp;<asp:HyperLink runat="server" ID="hlkFile" Target="_blank"></asp:HyperLink></td></tr>
    <tr id="trwebpage" class="hide" runat="server">
        <td colspan="2" style="vertical-align:top;">
            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td class="admin-prompt-right" style="width:120px;">&nbsp;</td>
                <td><asp:RadioButton runat="server" ID="rbInternal" GroupName="IntExtGrp" CssClass="rb-enhanced" Text="Internal" Checked="true" />&nbsp;<asp:RadioButton runat="server" ID="rbExternal" GroupName="IntExtGrp" CssClass="rb-enhanced" Text="External" /></td>
            </tr>
            <tr id="trInternal" runat="server">
                <td class="admin-prompt-right">Internal Page</td>
                <td><asp:DropDownList ID="ddlPages" runat="server" CssClass="dropdownlist" DataTextField="name" DataValueField="seo"></asp:DropDownList></td>
            </tr>
            <tr id="trExternal" runat="server">
                <td class="admin-prompt-right">External Page</td>
                <td><asp:TextBox ID="txtUrl" runat="server" CssClass="textbox"></asp:TextBox></td>
            </tr>
            </table>
        </td>
    </tr>
        <tr style="display:none;">
                <td class="admin-prompt-right">
                    Author Name:
                </td>
                <td>
                    <asp:TextBox ID="txtAuthor" runat="server" CssClass="textbox" Width="600"></asp:TextBox>
                </td>

            </tr>
            <tr style="display:none;">
                <td class="admin-prompt-right">
                    Comments:
                </td>
                <td>
                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="4" CssClass="textbox" Width="600" Height="100"></asp:TextBox>
                </td>

            </tr>
    <tr style="display:none;"><td class="admin-prompt-right">Ignore on homepage</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbIgnore" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
        <tr>
                <td class="admin-prompt-right">
                    Status:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlStatus" class="dropdownlist">
                    </asp:DropDownList>
                </td>
            </tr>
        <tr>
                <td class="admin-prompt-right">
                    Publish:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPublish" class="dropdownlist">
                    </asp:DropDownList>
                </td>
            </tr>
        <tr runat="server" >
                <td class="admin-prompt-right" >
                    Exclude from Weekly Roundup:
                </td>
                <td style="vertical-align:bottom;">
                    <asp:CheckBox runat="server" ID="cbWeekly" CssClass="cb-enhanced nolabel" Text="&nbsp;" />
                </td>
        </tr>
    </table>
</div>
<br />
<asp:LinkButton ID="imgSave" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="imgSave_Click" style="color: #FFFFFF;" />
<%if (!string.IsNullOrEmpty(NewsID))
    {  %>
&nbsp;<a class="admin-button-blue mw150" href="/newsroom?newsid=<%=NewsID %>&pv=1" style="color: #FFFFFF;" target="_blank">Preview on Newsroom</a>
&nbsp;<a class="admin-button-blue mw150" href="/membernews?newsid=<%=NewsID %>&pv=1" style="color: #FFFFFF;" target="_blank">Preview on Member Newsroom</a>

<%} %>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" />
<script type="text/javascript">
    //Callback function
    function cropperCallback_<%= Language %>() {
        //alert('callback function');
        $('#<%= imgUploadedPhoto.ClientID %>').attr('src', '/data/ImageCropper/<%= imageName %>.jpg');
        $('#<%= imgUploadedPhoto.ClientID %>').show();
    }
</script>
   