<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Admin_Menu_Menu" %>
<%--<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>--%>
<%@ Register TagPrefix="i386" Namespace="i386.UI" Assembly="i386.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register Src="~/Admin/UserLogout/UserLogout.ascx" TagPrefix="Hardcode" TagName="UserLogout" %>

<script type="text/javascript">
    var cmsWarningMsg = '';

    $(document).ready(function () {

        var PageTitleMinLen = 60;
        var PageTitleMaxLen = 70;
        var PageDescMinLen = 170;
        var PageDescMaxLen = 200;

        try { PageTitleMinLen = parseInt(<%= ConfigurationManager.AppSettings["PageTitleMinLen"].ToString() %>); }
        catch (err) { }
        try { PageTitleMaxLen = parseInt(<%= ConfigurationManager.AppSettings["PageTitleMaxLen"].ToString() %>); }
        catch (err) { }
        try { PageDescMinLen = parseInt(<%= ConfigurationManager.AppSettings["PageDescMinLen"].ToString() %>); }
        catch (err) { }
        try { PageDescMaxLen = parseInt(<%= ConfigurationManager.AppSettings["PageDescMaxLen"].ToString() %>); }
        catch (err) { }

        var ValidateFieldLength = function (element, field, minLen, maxLen) {
            if ($(element).val().length < minLen) {
                cmsWarningMsg += '\nThe page ' + field + ' is too short. We suggest a ' + field + ' between ' + minLen + '-' + maxLen + ' characters.';
            }
            else if ($(element).val().length > maxLen) {
                cmsWarningMsg += '\nThe page ' + field + ' is too long. We suggest a ' + field + ' between ' + minLen + '-' + maxLen + ' characters.';
            }
        }

        $('#<%= txtTitle.ClientID %>').blur(function () {
            cmsWarningMsg = '';
            ValidateFieldLength($(this), 'title', PageTitleMinLen, PageTitleMaxLen);
            if (cmsWarningMsg.length > 0)
                alert(cmsWarningMsg);
        });

        $('#<%= txtDescription.ClientID %>').blur(function () {
            cmsWarningMsg = '';
            ValidateFieldLength($(this), 'description', PageDescMinLen, PageDescMaxLen);
            if (cmsWarningMsg.length > 0)
                alert(cmsWarningMsg);
        });

        $(".spacer").hide();
        //$("#AdminMenu").corner("bottom 5px");

        $(".togglebutton").click(function () {
            //$('.spacer').slideToggle('medium');
            $('.spacer').slideToggle('medium', function () {
                if ($('.spacer').is(':visible')) {
                    cmsWarningMsg = '';
                    ValidateFieldLength('#<%= txtTitle.ClientID %>', 'title', PageTitleMinLen, PageTitleMaxLen);
                    ValidateFieldLength('#<%= txtDescription.ClientID %>', 'description', PageDescMinLen, PageDescMaxLen);
                    if (cmsWarningMsg.length > 0)
                        alert(cmsWarningMsg);
                }
            });
        });

        $(".togglebutton2").click(function () {
            $('.spacer').hide();
            return true;
        });


    });

    function change(img_name, img_src) {
        document[img_name].src = img_src;
    }

    function checkdates() {
        if (($('#<%=txtStartDate.ClientID%>').val() != "yyyy-MM-dd" && $('#<%=txtStartDate.ClientID%>').val() != "") ||
                ($('#<%=txtEndDate.ClientID%>').val() != "yyyy-MM-dd" && $('#<%=txtEndDate.ClientID%>').val() != "") ||
                ($('#<%=ddlStartTime.ClientID%>').val() != "") ||
                ($('#<%=ddlEndTime.ClientID%>').val() != "")) {
            $('.trenable').hide();
        }
        else
            $('.trenable').show();
    }

    $(document).ready(function () {

        $('.txtdate').change(function () {
            checkdates();
        });        

        checkdates();
    });

    $(document).ready(function () {

        $('.ajax__calendar_container').css("z-index", "1000");
    });

</script>



<style type="text/css">
.admintopmenu, a .admintopmenu, .admintopmenu a
{
    font-family: Arial,Sans Serif;
    font-size: 22px;
    color: #0078e5;
    cursor: pointer;
    font-weight: bold;
}
.admintopmenu:hover, a .admintopmenu:hover, .admintopmenu:hover a
{
    font-family: Arial,Sans Serif;
    font-size: 22px;
    color: #ff9c00;
    cursor: pointer;
    font-weight: bold;
}

body
{
    background-position:0px 85px!important;
}

div.overlay
{
    opacity:0;
    position:absolute;
    top:68px; bottom:0; left:20px; right:0;
    display:block;
    z-index:999;
    background:#fff;
    width:440px;
    height:420px;
}
div.overlayHome
{
    opacity:0;
    position:absolute;
    top:68px; bottom:0; left:20px; right:0;
    display:block;
    z-index:999;
    background:#fff;
    width:440px;
    height:392px;
}

div#divTopExternal
{
    background-color:#0079d8;
    width:486px;
    
    -moz-border-radius: 0 0 15px 0;
    -webkit-border-radius: 0 0 15px 0;
    -khtml-border-radius: 0 0 15px 0;
    border-radius: 0 0 15px 0;   
    
    behavior: url(/js/PIE-2.0beta1/PIE.htc);
}

div#divTopInternal
{
    padding: 10px 0 10px 0;
    margin-left:20px;
    background-color:#419ae3;
    width:440px;
    
    -moz-border-radius: 15px 15px 15px 15px;
    -webkit-border-radius: 15px 15px 15px 15px;
    -khtml-border-radius:15px 15px 15px 15px;
    border-radius: 15px 15px 15px 15px;
    
    behavior: url(/js/PIE-2.0beta1/PIE.htc);
}

.ajax__calendar_container { z-index : 1000 ; }
</style>
<link href="/CSS/AdminOverlay.css?v=<%= "" + ConfigurationManager.AppSettings.Get("CSSVersion") %>" rel="stylesheet" type="text/css" />
<link href="/css/menstyle.css" rel="stylesheet" type="text/css" media="screen" />
<nav class="u-full-width admin noprint" aria-label="admin menu">
<div style="height:85px;">&nbsp;</div>
<div class="fixedtopcenter" style=" z-index:999999998;top:0px;"></div>
<div class="fixedtopcenter" style="z-index:999999999;top:0px;width:100%;">
    <%--<Hardcode:UserLogout id="UserLogout" runat="server" />--%>
<table class="noprint" cellpadding="0" border="0" cellspacing="0" style="width:100%;height:85px;background-image:url(/images/lemonaid/partials/adminbg.jpg); background-repeat:repeat-x; border-bottom:1px solid #0078d8;">
<tr>
	<td style="padding-left:20px; vertical-align:top; padding-top:4px;"><asp:Literal ID="litLeft" runat="server" /></td><td align="center"><div style=" width:100%;position:relative;"><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><div style="position:relative; z-index:99999;"><%--<div style="position:absolute; top:26px; left:800px;"><a href="/logout.aspx" class="admintopmenu">LOGOUT</a></div>--%><ul id="sdt_menu" class="sdt_menu"><asp:Repeater runat="server" OnItemDataBound="BindMenuItem" ID="repMenu"><ItemTemplate><li><a href="#"><img class="js" src="/images/adminmenu/<%# DataBinder.Eval(Container.DataItem, "id") %>.jpg" alt=""/><span class="sdt_active"></span><span class="sdt_wrap"><span class="sdt_link"><%# DataBinder.Eval(Container.DataItem, "text") %></span><span class="sdt_descr" style="visibility:hidden;"><%# DataBinder.Eval(Container.DataItem, "subtext") %></span></span></a><asp:Literal ID="litLinks" runat="server" /></li></ItemTemplate></asp:Repeater></ul></div></td><td style="padding-right:20px; vertical-align:top; padding-top:4px;"><a href="" onclick="window.open('https://www.bluelemonmedia.com/help/', null, 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, scrolling=auto');return false;"><img src="/images/lemonaid/partials/help_logo.jpg" border="0"/></a></td>
</tr>

</table>
</div></td></tr></table>

<table cellpadding="0" cellspacing="0" border="0" class="admintable" width="900" style="position:fixed; display:none; bottom:50px; right:50px;">
		<tr>
			<td></td><td class="admin_bodytext_white"><img src="/images/icons/vv_orange_20.gif" class="togglebutton" id="tb1" runat="server"/></td><td class="admin_bodytext_white" style="display:none;"><asp:Literal ID="litMessage" runat="server" /></td><td></td><td style="display:none;"><a href="/admin.aspx?c=dash" id="adminareabutton" class="admin_bodytext_white" Visible="false" runat="Server">Control Panel</a></td><td style="display:none;"><a href="/admin.aspx?c=wizard" class="admin_bodytext_white" id="wizardbutton" runat="server" visible="false">The Wizard</a></td><td style="display:none;"><asp:LinkButton class="admin_bodytext_white" ID="lbLogout" runat="server" Text="Logout" OnClick="Logout"/></td><td class="admin_bodytext_white" align="right"><img src="/images/icons/vv_orange_20.gif" width="80" height="80" /></td><td align="right"></td>
		</tr>
	</table>
	<div class="spacer" style="display:none; text-align:left; float:left; background-color:transparent; position:relative;">
    <%if (IsReviewer)
      { %>
    <%if (Layout == "home")
      { %>
    <div class="overlayHome"></div>
    <%}
      else
      { %>
    <div class="overlay"></div>
    <%} %>
    <%} %>
    <div id="divTopExternal">
    
    <%--<table border="0" cellpadding="0" cellspacing="0" width="502" height="540" class="admin_bodytext_white" id="Table1" runat="server" style="background-image:url('/images/lemonaid/partials/topmenbg.png'); background-repeat:no-repeat; background-color:transparent;">--%>
    <table border="0" cellpadding="0" cellspacing="0" class="admin_bodytext_white" id="tbl" runat="server" >
    <tr><td style="text-align:left;">
    <div style="background-color:#0079d8;">
    <div style="float:left;"><img src="/images/lemonaid/partials/TopmenTitle.png" alt="Page Properties"  /></div><div style="float:right; padding:10px 10px 0 0 ;"><i386:ImageOverButton ID="bttn_Add_Question" runat="server" CssClass="togglebutton2" 
                OnClick="ClickedSave" ImageOverUrl="/images/lemonaid/buttons/save_over.png" ImageUrl="/images/lemonaid/buttons/save.png" /></div></div>
<div style="clear:both;"></div>
        <div id="divTopInternal" ><table border="1" cellpadding="0" cellspacing="0">
	<tr><td align="left" style="width:160px;  height:25px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Internal Name : </td><td align="left"><asp:TextBox ID="txtName" BorderWidth="0"  runat="server" MaxLength="500" Width="242" BackColor="Transparent"  style="width:242px; background:#fff; padding-left:10px; font-family:Arial, Sans-Serif; font-size:15px; color:#0078d8; vertical-align:top; padding-top:5px;"/></td></tr>
	<tr><td align="left" style="  height:25px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Title :</td><td align="left"><asp:TextBox ID="txtTitle" BorderWidth="0"  runat="server" MaxLength="500"  Width="350"  BackColor="Transparent" style="width:242px; background:#fff; padding-left:10px; font-family:Arial, Sans-Serif; font-size:15px; color:#0078d8; vertical-align:top; padding-top:5px;"/></td></tr>
	<tr><td align="left" style="  height:25px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Keywords :</td><td align="left"><asp:TextBox ID="txtKeywords" BorderWidth="0"  runat="server" MaxLength="500"  Width="242" BackColor="Transparent" style="width:242px; background:#fff; padding-left:10px; font-family:Arial, Sans-Serif; font-size:15px; color:#0078d8; vertical-align:top; padding-top:5px;"/></td></tr>
	<tr><td align="left" style="  height:25px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Description :</td><td align="left"><asp:TextBox ID="txtDescription" BorderWidth="0"  runat="server" MaxLength="500"  Width="242" BackColor="Transparent" style="width:242px;  background:#fff; padding-left:10px; font-family:Arial, Sans-Serif; font-size:15px; color:#0078d8; vertical-align:top; padding-top:5px;"/></td></tr>
	<tr runat="server"  visible="false"><td align="left">URL Name</td><td align="left"><asp:TextBox ID="txtSeo" runat="server" MaxLength="500"  Width="350"  BackColor="Transparent" BorderWidth="0" style="width:230px; height:20px; background:#fff;"/></td></tr>

	<tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Image :</td>
    <td style="padding-top:10px;" align="left">
        <asp:FileUpload runat="server" ID="flUpload1" Height="22" BackColor="Transparent"  />
    </td></tr>
    <tr runat="server" id="trImage">
        <td colspan="2" style="padding: 10px 20px;"><asp:HyperLink runat="server" ID="hylImage" Target="_blank" ForeColor="#004075"></asp:HyperLink>&nbsp;&nbsp;
            <asp:LinkButton runat="server" ID="btnDeleteImage" Text="Delete" OnClick="btnDeleteImage_Click" ForeColor="#ffffff" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to delete this image?')"></asp:LinkButton></td>
    </tr>
    <tr><td colspan="2" style="height:10px;"></td></tr>

	<tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Group :</td><td align="left"><asp:DropDownList ID="ddlGroup" runat="server" Width="230"/></td></tr>
	<tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Layout :</td><td style="padding-top:10px;" align="left"><asp:DropDownList ID="ddlLayout" runat="server" Width="230"/></td></tr>

	<tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Inside Class :</td>
        <td style="padding-top:10px;" align="left">
            <asp:DropDownList ID="cbLeftMenu" runat="server" >
                <asp:ListItem Text="Left menu" Value="yes-inside-menu"></asp:ListItem>
                <asp:ListItem Text="No left menu" Value="no-inside-menu"></asp:ListItem>
                <asp:ListItem Text="Programs & Services" Value="program-services"></asp:ListItem>
            </asp:DropDownList>            
        </td></tr>



    <tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Inside Banner Image :</td>
    <td style="padding-top:10px;" align="left">
        <asp:FileUpload runat="server" ID="flUploadBanner" Height="22" BackColor="Transparent"  />
    </td></tr>
    <tr runat="server" id="trBanner">
        <td colspan="2" style="padding: 10px 20px;"><asp:HyperLink runat="server" ID="hylBanner" Target="_blank" ForeColor="#004075"></asp:HyperLink>&nbsp;&nbsp;
            <asp:LinkButton runat="server" ID="btnDeleteBanner" Text="Delete" OnClick="btnDeleteBanner_Click" ForeColor="#ffffff" Font-Underline="true" OnClientClick="return confirm('Are you sure you want to delete this image?')"></asp:LinkButton></td>
    </tr>
    <tr><td colspan="2" style="height:10px;"></td></tr>

    <tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Banner Position:</td>
        <td style="padding-top:10px;" align="left">
            <asp:DropDownList ID="ddlBackgroundPosition" runat="server" >
                <asp:ListItem Text="Center" Value="center"></asp:ListItem>
                <asp:ListItem Text="Top" Value="top"></asp:ListItem>
                <asp:ListItem Text="Bottom" Value="bottom"></asp:ListItem>
            </asp:DropDownList>    
            
            <asp:DropDownList ID="ddlBackgroundPosition2" runat="server" >
                <asp:ListItem Text="Center" Value="center"></asp:ListItem>
                <asp:ListItem Text="Left" Value="left"></asp:ListItem>
                <asp:ListItem Text="Right" Value="right"></asp:ListItem>
            </asp:DropDownList>          
        </td></tr>

    <tr><td colspan="2" style="height:10px;"></td></tr>
    <tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Reviewer :</td><td align="left"><asp:DropDownList ID="ddlReviewer" runat="server" Width="230"/></td></tr>
    <tr><td colspan="2" style="height:5px;"></td></tr>

    <tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Frequency :</td><td align="left"><asp:DropDownList ID="ddlFrequency" runat="server" Width="230"/></td></tr>
    <tr><td colspan="2" style="height:5px;"></td></tr>

<tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Start Date :</td><td style="padding-top:10px;" align="left">
    <asp:TextBox Width="100" CssClass="textbox dates txtdate" ID="txtStartDate" runat="server" /><asp:Image ID="imgStartDate" ImageUrl="/images/icons/datepicker.gif" runat="server" /><ACT:CalendarExtender ID="CalendarExtender1" Format="yyyy-MM-dd" PopupPosition="BottomLeft" TargetControlID="txtStartDate" runat="server" PopupButtonID="imgStartDate" OnClientDateSelectionChanged="checkdates" /><ACT:TextBoxWatermarkExtender TargetControlID="txtStartDate" WatermarkText="yyyy-MM-dd" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="txtStartDateExtender"></ACT:TextBoxWatermarkExtender><asp:CompareValidator ID="cvStartDate" runat="server" ControlToValidate="txtStartDate" Operator="DataTypeCheck" Type="Date" ErrorMessage="Start date invalid" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"></asp:CompareValidator>    
    </td></tr>

    <tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">End Date :</td><td style="padding-top:10px;" align="left">
    <asp:TextBox Width="100" CssClass="textbox dates txtdate" ID="txtEndDate" runat="server" /><asp:Image ID="imgEndDate" ImageUrl="/images/icons/datepicker.gif" runat="server" /><ACT:CalendarExtender ID="CalendarExtender2" Format="yyyy-MM-dd" PopupPosition="BottomLeft" TargetControlID="txtEndDate" runat="server" PopupButtonID="imgEndDate" OnClientDateSelectionChanged="checkdates" /><ACT:TextBoxWatermarkExtender TargetControlID="txtEndDate" WatermarkText="yyyy-MM-dd" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="txtEndDateExtender"></ACT:TextBoxWatermarkExtender><asp:CompareValidator ID="cvEndDate" runat="server" ControlToValidate="txtEndDate" Operator="DataTypeCheck" Type="Date" ErrorMessage="End date invalid" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"></asp:CompareValidator>    
    </td></tr>

    <tr><td colspan="2" style="height:5px;"></td></tr>

        <tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Start Time :</td><td align="left"><asp:DropDownList ID="ddlStartTime" runat="server" Width="105"/></td></tr>
    <tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">End Time :</td><td align="left"><asp:DropDownList ID="ddlEndTime" runat="server" Width="105"/></td></tr>


    <tr class="trenable"><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Enabled :</td><td style="padding-top:10px;" align="left"><asp:CheckBox ID="cbEnabled" runat="server"/></td></tr>

	<tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Reviewed :</td><td style="padding-top:10px;" align="left"><asp:CheckBox ID="cbReviewed" runat="server"/></td></tr>
	</table></div>
    </td></tr>
    <tr><td colspan="2" style="height:20px;">&nbsp;</td></tr>
	</table>
        </div>
    
    </div>
</div>
	</nav>
<script type="text/javascript">
    $(function () {

        $('#sdt_menu > li').bind('mouseenter', function () {
            var $elem = $(this);
            $elem.find('img.js')
						 .stop(true)
						 .animate({
						     'width': '160px',
						     'height': '85px',
						     'left': '0px'
						 }, 400, 'easeOutBack')
						 .addBack()                /*andSelf()*/ /*Note: This API has been removed in jQuery 3.0; use .addBack() instead, which should work identically.*/
						 .find('.sdt_wrap')
					     .stop(true)
						 .animate({ 'top': '100px' }, 500, 'easeOutBack')
						 .addBack()
						 .find('.sdt_active')
					     .stop(true)
						 .animate({ 'height': '70px' }, 300, function () {
						     var $sub_menu = $elem.find('.sdt_box');
						     if ($sub_menu.length) {
						         var left = '160px';
						         if ($elem.parent().children().length == $elem.index() + 1)
						             left = '-245px';
						         $sub_menu.show().animate({ 'left': left }, 200);
						     }
						 });
        }).bind('mouseleave', function () {
            var $elem = $(this);
            var $sub_menu = $elem.find('.sdt_box');
            if ($sub_menu.length)
                $sub_menu.hide().css('left', '0px');

            $elem.find('.sdt_active')
						 .stop(true)
						 .animate({ 'height': '0px' }, 300)
						 .addBack().find('img.js')
						 .stop(true)
						 .animate({
						     'width': '0px',
						     'height': '0px',
						     'left': '85px'
						 }, 400)
						 .addBack()
						 .find('.sdt_wrap')
						 .stop(true)
						 .animate({ 'top': '25px' }, 500);
        });

        $('.sdt_box_item').mouseenter(function() {
            $(this).find('.sdt_boxsub').show().animate({ 'left': '225px' }, 200);
        }).mouseleave(function() {
            $(this).find('.sdt_boxsub').hide().css('left', '0px');
        });
    });
    
    
    
        </script>

        <script type="text/javascript">

            $(document).ready(function () {

 
                var EnableDatesValidators = function () {
                    var DatesRequired = ($('#<%= txtStartDate.ClientID %>').val() != '' && $('#<%= txtStartDate.ClientID %>').val() != 'yyyy-MM-dd') || ($('#<%= txtEndDate.ClientID %>').val().length != '' && $('#<%= txtEndDate.ClientID %>').val() != 'yyyy-MM-dd');

                }

                $('.dates').change(function () {
                    EnableDatesValidators();
                });

                EnableDatesValidators();

            });

        </script>