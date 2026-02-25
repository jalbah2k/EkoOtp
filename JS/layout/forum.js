
var styles = ['h1', 'h2', 'h3', 'p', 'a', 'li', 'span', 'div'];
var changer = "span.changer";
var grow = 'spGrow';
var srhink = 'spShrink';
var growup = 3;
var grew = 0; //counter initial value

// initialize the jquery code
$(document).ready(function () {
    $("#startForum").click(function () {
        // alert("clicked!");
        $(".ekoWalk").hide();
        $("body").addClass("nextStepTwo");
        // setTimeout(function () {
            $("body").addClass("showPop");
        // }, 400);
    });

    $("#showAccount").click(function () {
        $(".ekoWalk").hide();
        $("body").addClass("bodyShowAccount");
    });

    $("#showMyAccountBody").click(function () {
        $(".ekoWalk").hide();
        $("body").addClass("myAccountBody");
    });

    $("#finishWalkthrough").click(function () {
        $(".ekoWalk").hide();
    });
    $(changer).click(function () {

        for (var i = 0; i < styles.length; i++) {
            var $mainText = $(styles[i]);
            var changerId = this.id;

            $mainText.each(function () {
                if ($(this).css('font-size')) {
                    var currentSize = $(this).css('font-size');
                    var num = parseFloat(currentSize, 10);

                    var unit = currentSize.slice(-2);

                    if (changerId == grow && grew < growup) {
                        num += 1;
                    }
                    else if (changerId == srhink && grew > 0) {
                        num -= 1;
                    }
                    $(this).css('font-size', num + unit);
                }
            });
        }

        if (this.id == grow && grew < growup)
            grew++;
        else if (this.id == srhink && grew > 0)
            grew--;

        return false;

    });

});

$(document).ready(function () {
    //$.ajax({
    //    url: '/twitterapp/',
    //    type: 'GET',
    //    success: function (data) {
    //        tweetTxt = data;

    //        if (tweetTxt.includes('&amp;')) {
    //            tweetTxt = tweetTxt.replace(/&amp;/g, '&');
    //        }
    //        tweetTxt = tweetTxt.replace(/&lt;/g, '<');
    //        tweetTxt = tweetTxt.replace(/&gt;/g, '>');
    //        tweetTxt = tweetTxt.replace(/&#x27;/g, '"');

    //        $('#myTwitterNew').html(tweetTxt);

    //    }
    //    //, error: function (xhr, status, errorThrown) {
    //    //    alert(status + " | " + xhr.responseText);
    //    //}
    //});

    $('.searchPanel button').click(function () {

        var search = $('.searchPanel input[type=text]').val();
        if (search != undefined && search != "") {
            search = encodeURIComponent(search);
            window.location = '/membersearch?q=' + search;
        }

    });

    function searxtText( page) {

        var search = $('.searchPanel input[type=text]').val();
        if (search != undefined && search != "") {
            ;
        }
        else {
            search = $('.searchPanel_mobile input[type=text]').val();
            if (search != undefined && search != "") {
                ;
            }
            else
                return;
        }

        search = encodeURIComponent(search);
        window.location = page + search;

    }

    $('.searchPanel-inside button').click(function () {

        searxtText('/membersearch?q=');
    });

    $('.searchPanel-login button').click(function () {

        searxtText('/search?q=');
    });


});