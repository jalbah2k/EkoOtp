$(document).ready(function () {
    //$('.Toggle-Header').click(function () {
    $('body').on('click', '.Toggle-Header, .Toggle-More', function () {
        var header = $(this).closest('h1, h2, h3');
        //var CollapsiblePanel = header.nextAll('.Collapsible-Panel, div[style="position: inherit;"], div[style="position: inherit; display: none;"], div[style="position: inherit; display: block;"]').first();
        //if (CollapsiblePanel == null || CollapsiblePanel == 'undefined' || CollapsiblePanel.length == 0)
        //    CollapsiblePanel = header.next("p");
        /*var CollapsiblePanel = header.next("p").filter(function () {
        return $.trim($(this).html()) != '&nbsp;';
        });*/
        CollapsiblePanel = header.next("p");
        if (CollapsiblePanel != null && CollapsiblePanel != 'undefined')
            if ($.trim(CollapsiblePanel.html()) == '&nbsp;')
                CollapsiblePanel = CollapsiblePanel.next("p");
        if (CollapsiblePanel == null || CollapsiblePanel == 'undefined' || CollapsiblePanel.length == 0)
            CollapsiblePanel = header.nextAll('.Collapsible-Panel, div[style="position: inherit;"], div[style="position: inherit; display: none;"], div[style="position: inherit; display: block;"], div[style="display: none; position: inherit;"], div[style="overflow: hidden; display: block; position: inherit;"], div[style="overflow: hidden; display: none; position: inherit;"], div[style="position: inherit; display: block; overflow: hidden;"], div[style="position: inherit; display: none; overflow: hidden;"], div[style="border-image: none; position: inherit;"], div[ style="border-image: none; display: none; position: inherit;"]').first();
        if (CollapsiblePanel != null && CollapsiblePanel != 'undefined' && CollapsiblePanel.length > 0) {
            if (CollapsiblePanel.is(':visible')) {
                $(this).addClass('collapsed');
                if ($(this).hasClass('Toggle-More'))
                    $(this).text($(this).text().replace(/less/gi, "More"));
            }
            else {
                $(this).removeClass('collapsed');
                if ($(this).hasClass('Toggle-More'))
                    $(this).text($(this).text().replace(/more/gi, "Less"));
            }
            CollapsiblePanel.slideToggle(200, "swing", function () {
                //What to do on toggle compelte...
                MyMatchContentHeight();
            })
        }
    });
    $('body').on('keypress', '.Toggle-Header, .Toggle-More', function (e) {
        var charCode = (window.event) ? event.keyCode : e.which;
        if (charCode == 13)
            $(this).click();
    });
    $('.Toggle-Header, .Toggle-More').each(function () {
        var header = $(this).closest('h1, h2, h3');
        $(this).attr('tabindex', 0);
        //        var CollapsiblePanel = header.nextAll('.Collapsible-Panel, div[style="position: inherit;"], div[style="position: inherit; display: none;"], div[style="position: inherit; display: block;"]').first();
        //        if (CollapsiblePanel == null || CollapsiblePanel == 'undefined' || CollapsiblePanel.length == 0)
        //            CollapsiblePanel = header.next("p");
        /*var CollapsiblePanel = header.next("p").filter(function () {
        return $.trim($(this).html()) != '&nbsp;';
        });*/
        CollapsiblePanel = header.next("p");
        if (CollapsiblePanel != null && CollapsiblePanel != 'undefined')
            if ($.trim(CollapsiblePanel.html()) == '&nbsp;')
                CollapsiblePanel = CollapsiblePanel.next("p");
        if (CollapsiblePanel == null || CollapsiblePanel == 'undefined' || CollapsiblePanel.length == 0) {
//////            CollapsiblePanel = header.nextAll('.Collapsible-Panel, div[style="position: inherit;"], div[style="position: inherit; display: none;"], div[style="position: inherit; display: block;"]').first();
            CollapsiblePanel = header.nextAll('.Collapsible-Panel, div[style="position: inherit;"], div[style="position: inherit; display: none;"], div[style="position: inherit; display: block;"], div[style="display: none; position: inherit;"], div[style="overflow: hidden; display: block; position: inherit;"], div[style="overflow: hidden; display: none; position: inherit;"], div[style="position: inherit; display: block; overflow: hidden;"], div[style="position: inherit; display: none; overflow: hidden;"], div[style="border-image: none; position: inherit;"], div[ style="border-image: none; display: none; position: inherit;"]').first();
}
        if (CollapsiblePanel != null && CollapsiblePanel != 'undefined' && CollapsiblePanel.length > 0) {
            $(this).addClass('collapsed');
            CollapsiblePanel.hide();
        }
    });
});

function MyMatchContentHeight() {
    var ContentHeight = $('.insideContent').height();
    var LeftMenuHeight = $('.leftmenu').height();

    $(".leftmenu").css({ 'height': ContentHeight + 'px' });
}
