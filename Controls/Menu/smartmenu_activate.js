
$(mymenuid).smartmenus({
    subMenusSubOffsetX: 1,
    subMenusSubOffsetY: -8
});

$(document).ready(function () {
    try {
        if ($('ul.sm-vertical .current.has-submenu').siblings('ul').children('li').children('a.current').html().length > 0)
            $('ul.sm-vertical .current.has-submenu').siblings('ul').addClass("always-show");
    }
    catch (err) {
   
    }

    //To add the class 'current' to a top menu item (linkedmenuid)
    $(".menu-horizontal ul.navbar-nav > li > a.has-submenu").each(function (i) {
        try {
            if ($(this).siblings('ul').children('li').children('a.current').html().length > 0) {
                //alert($(this).html());
                $(this).removeClass("current");
                $(this).addClass("current");
            }
        }
        catch (err) {

        }
    });
});
