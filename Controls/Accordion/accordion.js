$(document).ready(function () {

    $('.accordion-section-title').click(function (e) {
        var currentAttrvalue = $(this).attr('href');
        if ($(e.target).is('.active')) {
            $(this).removeClass('active');
            if (navigator.userAgent.indexOf("Firefox") !== -1 || navigator.userAgent.indexOf("Edg") !== -1 || navigator.userAgent.indexOf("Trident") !== -1) 
                $('.accordion-section-content:visible').hide(100); //FF
            else
                $('.accordion-section-content:visible').slideUp(300);
        } else {
            $('.accordion-section-title').removeClass('active').filter(this).addClass('active');
            if (navigator.userAgent.indexOf("Firefox") !== -1 || navigator.userAgent.indexOf("Edg") !== -1 || navigator.userAgent.indexOf("Trident") !== -1
                || navigator.userAgent.indexOf("Chrome") > 0            ) {
                $('.accordion-section-content').hide();
                $('.accordion-section-content').filter(currentAttrvalue + 'B').show();
            }
            else
                $('.accordion-section-content').slideUp(300).filter(currentAttrvalue+'B').slideDown(300);
        }

        var is_mobile = !!navigator.userAgent.match(/iphone|android|blackberry/ig) || false;
        if (is_mobile) {
            setTimeout(() => { location.href = currentAttrvalue; }, 1000); 

        }
    });


    $(".faq a.accordion-section-title").click(function (event) {
            event.preventDefault();
        });

         $(".medical a.accordion-section-title").click(function (event) {
           // $("html, body").animate({ scrollTop: $($(this).attr("href")).offset().top }, 500);
        });


    $('.accordion-section-content a').click(function (event) {

        
        var url = $(this).attr("href");

        if (url.toLowerCase().indexOf("#accordion-") >= 0 && url.toLowerCase().indexOf(window.location.pathname) >= 0) {
            //event.preventDefault();

            var strippedUrl = url.toString().split("#");
            var anchorvalue = strippedUrl[1];
            var elem = $('.accordion-section-title[href="#' + anchorvalue + '"]');
            elem.click();

        }
    });
    

    


});