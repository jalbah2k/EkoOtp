$(document).ready(function () {

    $('.sub-accordion-section-title').click(function (e) {
        var currentAttrvalue = $(this).attr('href');
        if ($(e.target).is('.active')) {
            $(this).removeClass('active');
            if (navigator.userAgent.indexOf("Firefox") !== -1 || navigator.userAgent.indexOf("Edg") !== -1 || navigator.userAgent.indexOf("Trident") !== -1)
                $('.sub-accordion-section-content:visible').hide(100); //FF
            else
                $('.sub-accordion-section-content:visible').slideUp(300);
        } else {
            $('.sub-accordion-section-title').removeClass('active').filter(this).addClass('active');
            if (navigator.userAgent.indexOf("Firefox") !== -1 || navigator.userAgent.indexOf("Edg") !== -1 || navigator.userAgent.indexOf("Trident") !== -1
                || navigator.userAgent.indexOf("Chrome") > 0) {
                $('.sub-accordion-section-content').hide();
                $('.sub-accordion-section-content').filter(currentAttrvalue + 'B').show();
            }
            else
                $('.sub-accordion-section-content').slideUp(300).filter(currentAttrvalue+'B').slideDown(300);
        }

        var is_mobile = !!navigator.userAgent.match(/iphone|android|blackberry/ig) || false;
        if (is_mobile) {
            setTimeout(() => { location.href = currentAttrvalue; }, 1000); 

        }
    });


    $('.sub-accordion-section-content a').click(function (event) {

        
        var url = $(this).attr("href");

        if (url.toLowerCase().indexOf("#sub-accordion-") >= 0 && url.toLowerCase().indexOf(window.location.pathname) >= 0) {
            //event.preventDefault();

            var strippedUrl = url.toString().split("#");
            var anchorvalue = strippedUrl[1];
            var elem = $('.sub-accordion-section-title[href="#' + anchorvalue + '"]');
            elem.click();

        }
    });

    if (window.location.href.split('#').length > 1) {
        var anchor = window.location.href.split('#')[1];
        if (anchor.indexOf("sub-accordion") == 0) {
            $("a[href$='" + anchor + "']").click();
            $("a[href$='" + anchor + "']").parents('.accordion-section-content').siblings('a').click();
        }
    }

});