<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin" EnableEventValidation="false" ValidateRequest="false"%>
<%@ Register TagPrefix="cc1" Namespace="skmMenu" Assembly="skmMenu" %>
<%@ Register tagprefix="Hardcode" src="~/Controls/BackToTop/BackToTop.ascx" tagname="BackToTop" %>
<%@ Register tagprefix="Hardcode" src="~//Controls/Session/Session.ascx" tagname="Session" %>
<%@ Register TagPrefix="Hardcode" Src="~/Admin/UserLogout/UserLogout.ascx" TagName="UserLogout" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title></title>
     <script type="text/javascript">
    	var GB_ROOT_DIR = "/js/greybox/";
	</script>
    <asp:PlaceHolder runat="server">

    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@100;200;400;500;600&display=swap" rel="stylesheet" />

	<%--<link rel="Stylesheet" type="text/css" href="/CSS/uploadify.css" /> --%>
   <%-- <link href="/CSS/noprint.css" rel="stylesheet" type="text/css" media="print" />--%>
    <link rel="stylesheet" href="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/themes/smoothness/jquery-ui.css" />

    <link href="/CSS/Admin.css?v=<%= "" + ConfigurationManager.AppSettings.Get("CSSVersion") %>" rel="stylesheet" type="text/css" />
    <link href="/CSS/LemonAid.css" rel="stylesheet" type="text/css" />
	<link href="/CSS/AdminNew.css" rel="stylesheet" type="text/css" />


    <!-- Favicon
    –––––––––––––––––––––––––––––––––––––––––––––––––– -->
     <!-- ****** faviconit.com favicons ****** -->
		<link rel="shortcut icon" href="/favicon.ico" />

	<!-- ****** faviconit.com favicons ****** -->


	<%--<link rel="Stylesheet" type="text/css" href="/CSS/uploadify.css" /> --%>
    <%--<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>--%>
   <%-- <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>--%>
 <%--    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js"></script>--%>
   <script type="text/javascript" src="/Libraries/jquery/jquery-3.7.1.min.js"></script>
    <script type="text/javascript" src="/Libraries/jquery-ui-1.14.1/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/JS/jquery.regex.js"></script>
    <script type="text/javascript" src="/js/jquery.easing.1.3.js"></script>
    <script src="/JS/jquery.corner.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/greybox/AJS.js"></script>
    <script type="text/javascript" src="/js/greybox/AJS_fx.js"></script>
    <script type="text/javascript" src="/js/greybox/gb_scripts.js"></script>
    <script type="text/javascript" src="/js/CalendarPopup.js"></script>
    <script type="text/javascript" src="/js/jquery.tablesorter.min.js"></script>
    <script type="text/javascript" src="/js/scripts-pack.js"></script>  
    <link href="/js/greybox/gb_styles.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/css/menstyle.css" type="text/css" media="screen"/>
    <link rel="stylesheet" href="/fonts/font-awesome-4.5.0/css/font-awesome.min.css" />

   <%: System.Web.Optimization.Scripts.Render("~/bundles/backtotop") %>

    </asp:PlaceHolder>

 
    <script type="text/javascript">

	    /*$(document).ready(function() {*/
	    function pageLoad(sender, args) {
		    $(".WhiteTable").corner("top 15px");

		    $(".NewTable").corner("15px");
	    }
	    /*});*/

    </script>
    <!--[if lt IE 7]>
        <script type="text/javascript" src="/png_fix/unitpngfix.js"></script>
	<![endif]-->

    <script type="text/javascript">
    <!--
        function silentErrorHandler() {return true;}
        window.onerror=silentErrorHandler;
    //-->
    </script>

    <script type="text/javascript">
    <!--
        function MM_swapImgRestore() { //v3.0
            var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
        }
        function MM_preloadImages() { //v3.0
            var d = document; if (d.images) {
                if (!d.MM_p) d.MM_p = new Array();
                var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
                    if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; }
            }
        }

        function MM_findObj(n, d) { //v4.01
            var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
                d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
            }
            if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
            for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
            if (!x && d.getElementById) x = d.getElementById(n); return x;
        }

        function MM_swapImage() { //v3.0
            var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
                if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
        }
    //-->
    </script>

    <style type="text/css">
        .admin-user-logout-wrapper
        {
            top: 54px !important;
        }
    </style>
</head>
<body onload="MM_preloadImages('/images/lemonaid/buttons/runfilter_over.png')">
    <form id="form1" runat="server">
        <div style="height:85px;">&nbsp;</div>
        <div class="fixedtopcenter" style=" z-index:999999998;top:0px;"></div>
        <div class="fixedtopcenter" style="z-index:999999999;top:0px;width:100%;">
            <Hardcode:UserLogout id="UserLogout" runat="server" />
        <%--<div style="position:relative; z-index:1000;">--%>
            <table class="adminmenu-wrapper noprint" cellpadding="0" border="0" cellspacing="0" style="width:100%;height:85px;background-image:url(/images/lemonaid/partials/adminbg.jpg); background-position:bottom; background-color:#ffffff; background-repeat:repeat-x;">
            <tr><td style="padding-left:20px; vertical-align:top; padding-top:4px;"><a href="/"><img src="/images/lemonaid/partials/lemonaid_logo.jpg" border="0" /></a></td><td style="text-align:center;" align="center"><div style="position:relative;"><div style="position:relative; margin:0 auto; width:100%; z-index:1000;"><%--<div style="position:absolute; top:24px; left:800px;" ><a href="/logout.aspx" style="font-weight:bold;font-size:24px; text-decoration:none;font-family:Myriad Pro,Trebuchet MS,sans-serif;">LOGOUT</a></div>--%><ul id="sdt_menu" class="sdt_menu"><asp:Repeater runat="server" OnItemDataBound="BindMenuItem" ID="repMenu"><ItemTemplate><li><a href="#"><img class="js" src="images/adminmenu/<%# DataBinder.Eval(Container.DataItem, "id") %>.jpg" alt=""/><span class="sdt_active"></span><span class="sdt_wrap"><span class="sdt_link"><%# DataBinder.Eval(Container.DataItem, "text") %></span><span class="sdt_descr" style="visibility:hidden;"><%# DataBinder.Eval(Container.DataItem, "subtext") %></span></span></a><asp:Literal ID="litLinks" runat="server" /></li></ItemTemplate></asp:Repeater></ul></div><div style="position:relative; margin:0 auto; width:100%; z-index:1000;display:none;"><cc1:Menu ID="TheMenu" runat="server"/></div></div></td><td style="padding-right:20px; vertical-align:top; padding-top:4px;display:none;"><a href="" onclick="window.open('https://www.bluelemonmedia.com/help/', null, 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, scrolling=auto');return false;"><img src="/images/lemonaid/partials/help_logo.jpg" border="0"/></a></td></tr>
            </table>
        </div>
        <div style="position:absolute; left:50%; top:85px; z-index:999999;"><Hardcode:Session runat="server" id="sessioncontrol" visible="true"></Hardcode:Session></div>
        <Hardcode:BackToTop ID="BackToTop1" runat="server" />

        <table class="tbl-Control" cellpadding="0" border="0" cellSpacing="0">
        <tr><td class="td-Control"><asp:Panel ID="pnlControl" runat="server" /></td></tr>
        </table>
    </form>
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
						                 left = '-260px';
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
                $(this).find('.sdt_boxsub').show().animate({ 'left': '245px' }, 200);
            }).mouseleave(function() {
                $(this).find('.sdt_boxsub').hide().css('left', '0px');
            });
        });
    </script>
</body>
</html>
