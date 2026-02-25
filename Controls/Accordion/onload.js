$(document).ready(function () {
    var anchorValue;
    var url = document.location;
    var strippedUrl = url.toString().split("#");
    if (strippedUrl.length > 1) {
        anchorvalue = strippedUrl[1];
        var elem = $('.accordion-section-title[href="#' + anchorvalue + '"]');
        elem.click();
    }
});