<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Admin_Menu_Menu" %>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register TagPrefix="i386" Namespace="i386.UI" Assembly="i386.UI" %>

<script type="text/javascript">
	$(document).ready(function()
	{
	
		$(".spacer").hide();
		//$("#AdminMenu").corner("bottom 5px");
		
		$(".togglebutton").click(function()
		{
			$('.spacer').slideToggle('medium');
			
		});

		$(".togglebutton2").click(function() {
			$('.spacer').hide();
			return true;
		});

		
	});

	function change(img_name, img_src) {
		document[img_name].src = img_src;
	}
	
</script>
<style type="text/css">
    
    .admintopmenu, a .admintopmenu, .admintopmenu a
{
   
font-family: Arial,Sans Serif;
font-size: 20px;
color: #0078e5;
cursor: pointer;
font-weight: bold;

}
.admintopmenu:hover, a .admintopmenu:hover, .admintopmenu:hover a
{
   
font-family: Arial,Sans Serif;
font-size: 20px;
color: #ff9c00;
cursor: pointer;
font-weight: bold;

}
</style>

<link href="/CSS/AdminOverlay.css?v=<%= "" + ConfigurationManager.AppSettings.Get("CSSVersion") %>" rel="stylesheet" type="text/css" />
<div class="fixedtopcenter" style="z-index:400;top:0px;">
	<div id="AdminMenu" class="admin_overlay"><div style="height:28px;"></div><div class="spacer" style="height:350px;display:none;" height="350"></div></div>
</div>
<div class="fixedtopcenter" style="z-index:401;top:0px;width:100%;">

<table class="noprint" cellpadding="0" border="0" cellspacing="0" style="width:100%;height:65px;background-image:url(/images/lemonaid/partials/adminbg.jpg); background-repeat:repeat-x;">
<tr>
	<td style="padding-left:20px; vertical-align:top; padding-top:4px;"><asp:Literal ID="litLeft" runat="server" /></td><td align="center"><div style=" width:100%;position:relative;"><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td style="width:33%;" class="admintopmenu"><a href="/admin.aspx?c=dash">Dashboard</a></td><td style="width:33%;" class="admintopmenu"><a href="/admin.aspx?c=wizard">Page Wizard</a></td><td style="width:33%;" class="admintopmenu"><a href="/logout.aspx">Logout</a></td></tr></table></div></td><td style="padding-right:20px; vertical-align:top; padding-top:4px;"><a href="" onclick="window.open('https://www.bluelemonmedia.com/help/', null, 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, scrolling=auto');return false;"><img src="/images/lemonaid/partials/help_logo.jpg" border="0"/></a></td>
</tr>

</table>

<table cellpadding="0" cellspacing="0" border="0" class="admintable" width="900" style="position:fixed; display:none; bottom:50px;; right:50px;">
		<tr>
			<td></td><td class="admin_bodytext_white"><img src="/images/icons/vv_orange_20.gif" class="togglebutton" id="tb1" runat="server"/></td><td class="admin_bodytext_white" style="display:none;"><asp:Literal ID="litMessage" runat="server" /></td><td><a href="/admin.aspx?c=dash" id="adminareabutton" class="admin_bodytext_white" Visible="false" runat="Server">Control Panel</a></td><td><a href="/admin.aspx?c=wizard" class="admin_bodytext_white" id="wizardbutton" runat="server" visible="false">The Wizard</a></td><td><asp:LinkButton class="admin_bodytext_white" ID="lbLogout" runat="server" Text="Logout" OnClick="Logout"/></td><td class="admin_bodytext_white" align="right"><img src="/images/icons/vv_orange_20.gif" width="80" height="80" /></td><td align="right"></td>
		</tr>
	</table>
	<div class="spacer" style="display:none; text-align:left;"><table border="0" cellpadding="0" cellspacing="0" width="502" height="385" class="admin_bodytext_white" id="tbl" runat="server" style="background-image:url('/images/lemonaid/partials/topmenbg.png'); background-repeat:no-repeat;"><tr><td><div style="position:relative; top:29px; left:33px;"><table border="0" cellpadding="0" cellspacing="0">
	<tr><td align="left" style="width:160px;  height:25px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Internal Name : </td><td align="left"><asp:TextBox ID="txtName" BorderWidth="0"  runat="server" Width="242" BackColor="Transparent"  style="width:242px;  background:#fff; padding-left:10px; font-family:Arial, Sans-Serif; font-size:15px; color:#0078d8; vertical-align:top; padding-top:5px;"/></td></tr>
	<tr><td align="left" style="  height:25px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Title :</td><td align="left"><asp:TextBox ID="txtTitle" BorderWidth="0"  runat="server"  Width="350"  BackColor="Transparent" style="width:242px;  background:#fff; padding-left:10px; font-family:Arial, Sans-Serif; font-size:15px; color:#0078d8; vertical-align:top; padding-top:5px;"/></td></tr>
	<tr><td align="left" style="  height:25px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Keywords :</td><td align="left"><asp:TextBox ID="txtKeywords" BorderWidth="0"  runat="server"  Width="242" BackColor="Transparent" style="width:242px;  background:#fff; padding-left:10px; font-family:Arial, Sans-Serif; font-size:15px; color:#0078d8; vertical-align:top; padding-top:5px;"/></td></tr>
	<tr><td align="left" style="  height:25px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Description :</td><td align="left"><asp:TextBox ID="txtDescription" BorderWidth="0"  runat="server"  Width="242" BackColor="Transparent" style="width:242px;  background:#fff; padding-left:10px; font-family:Arial, Sans-Serif; font-size:15px; color:#0078d8; vertical-align:top; padding-top:5px;"/></td></tr>
	<tr style="display:none;"><td align="left">URL Name</td><td align="left"><asp:TextBox ID="txtSeo" runat="server"  Width="350"  BackColor="Transparent" BorderWidth="0" style="width:230px; height:20px; background:#fff;"/></td></tr>
	<tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Group :</td><td align="left"><asp:DropDownList ID="ddlGroup" runat="server" Width="230"/></td></tr>
	<tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Layout :</td><td style="padding-top:10px;" align="left"><asp:DropDownList ID="ddlLayout" runat="server" Width="230"/></td></tr>
	<tr><td align="left"  style=" height:20px; text-align:right; font-family:Arial, Sans Serif; font-size:16px; color:#004075; vertical-align:top; padding-top:6px; padding-right:10px;">Enabled :</td><td style="padding-top:10px;" align="left"><asp:CheckBox ID="cbEnabled" runat="server"/><div style="position:relative; top:-310px; left:190px;"><i386:ImageOverButton ID="bttn_Add_Question" runat="server" CssClass="togglebutton2" 
                OnClick="ClickedSave" ImageOverUrl="/images/lemonaid/buttons/save_over.png" ImageUrl="/images/lemonaid/buttons/save.png" /></div></td></tr>
	</table></div></td></tr>
	</table></div>
</div>