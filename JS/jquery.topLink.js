jQuery.fn.topLink = function (settings) {
    settings = jQuery.extend({
        min: 1,
        fadeSpeed: 100,
        scrollSpeed: 300
    }, settings);
    return this.each(function () {
        var el = jQuery(this);
        el.hide(); //in case the user forgot
        //listen for scroll
        jQuery(window).scroll(function () {
            if (jQuery(window).scrollTop() >= settings.min) {
                el.fadeIn(settings.fadeSpeed);
            }
            else {
                el.fadeOut(settings.fadeSpeed);
            }
        });
        el.click(function (r) {
            jQuery("html, body").animate({ scrollTop: 0 }, settings.scrollSpeed);
        });
    });
};
