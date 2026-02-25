<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuNoBlueBox.ascx.cs" Inherits="MenuNoBlueBox" %>

<script type="text/javascript" src="/js/jquery.easing.1.3.js"></script>

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


</style>
<link href="/CSS/AdminOverlay.css" rel="stylesheet" type="text/css" />
<link href="/css/menstyle.css" rel="stylesheet" type="text/css" media="screen" />
<nav class="u-full-width admin noprint" aria-label="admin menu">
<div style="height:85px;">&nbsp;</div>
<div class="fixedtopcenter" style=" z-index:999999998;top:0px;"></div>
<div class="fixedtopcenter" style="z-index:999999999;top:0px;width:100%;">
<table class="noprint" cellpadding="0" border="0" cellspacing="0" style="width:100%;height:85px;background-image:url(/images/lemonaid/partials/adminbg.jpg); background-repeat:repeat-x; border-bottom:1px solid #0078d8;">
<tr>
	<td style="padding-left:20px; vertical-align:top; padding-top:4px;"><asp:Literal ID="litLeft" runat="server" /></td><td align="center"><div style=" width:100%;position:relative;"><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><div style="position:relative; z-index:99999;">
        <ul id="sdt_menu" class="sdt_menu"><asp:Repeater runat="server" OnItemDataBound="BindMenuItem" ID="repMenu"><ItemTemplate><li><a href="#"><img class="js" src="/images/adminmenu/<%# DataBinder.Eval(Container.DataItem, "id") %>.jpg" alt=""/><span class="sdt_active"></span><span class="sdt_wrap"><span class="sdt_link"><%# DataBinder.Eval(Container.DataItem, "text") %></span><span class="sdt_descr" style="visibility:hidden;"><%# DataBinder.Eval(Container.DataItem, "subtext") %></span></span></a>
        <asp:Literal ID="litLinks" runat="server" /></li></ItemTemplate></asp:Repeater></ul></div></td>
</tr>

</table>
</div></td></tr></table>


	
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
