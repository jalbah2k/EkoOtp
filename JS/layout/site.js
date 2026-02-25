

//Initial bind
$(document).ready(function () {

    $('a[href*="pdf"]').click(function(e) {
        e.preventDefault(); // stop the existing link from firing
        var documentUrl = $(this).attr("href"); // get the url of the pdf
        window.open(documentUrl, '_blank'); // open the pdf in a new window/tab
      });

    $('.infoToggle').click(function (){
      // $(this).siblings(".infoPopup").toggleClass("showme");
      $(this).siblings(".infoPopup").toggle();
    });

    $( "#search-filter input" ).focusin(function() {
      $("#sub-filter, #cat-filter, #lib-filter, #mobBtnWrap, #mobBtnWrapmainMenu").show();
    });

    $( "#btnSearchRes, #closeMob" ).click(function() {
      $("#sub-filter, #cat-filter, #lib-filter, #mobBtnWrap").toggle();
    });

    $( "#mobileSearch" ).click(function() {
      $( "#btnSearchRes" ).click();
    });

    $("#mobileSearchDir").click(function () {
        $("#btnSearchDir").click();
    });

    

    // $( "#mobileSearchToggle, #mobileMenu" ).click(function() {
    //   $(".row-filter").toggle();
    // });

    $( "#popWelcome .button1" ).click(function() {
        // alert("efse");
      $( "#popWelcome" ).addClass("hidePop");
      $( "body" ).addClass("bodyPopResources");
      $( "#popResources" ).removeClass("hidePop");
    });

    $( "#selResStep" ).click(function() {
      $( "#popResSelect" ).addClass("hidePop");
      $( "#popResSelectTwo" ).removeClass("hidePop");
    });

    $( "#selResStepTwo" ).click(function() {
      $( "#popResSelectTwo" ).addClass("hidePop");
      setTimeout(function() { 
            $("body").addClass("showResSelect");
        }, 600);
    });

    $( "#favResBtn" ).click(function() {
        // alert("ewrwer");
      $( "#popFavNext" ).addClass("hidePop");
      setTimeout(function() { 
            $("#resViewStep").hide();
            $(".favBtn").addClass("favBtnPseudo");
        }, 400);
    });

    $( ".favBtn" ).click(function() {
      //  alert("werewr");
      $("body").removeClass("showFavSelect");
      $("#resViewStepTwo").show();
    });


// $( "h2" ).simulate( "click" );

var $aDashboard = $('#aDashboard');
var aDashboard = $aDashboard[0];
var $ttDashboard = $('#ttDashboard');

$ttDashboard.hover(function(){
    aDashboard.currentTime = 0;
    aDashboard.play();
}, function(){
    aDashboard.pause();
});


var $aWatercoolers = $('#aWatercoolers');
var aWatercoolers = $aWatercoolers[0];
var $ttWaterCooler = $('#ttWaterCooler');

$ttWaterCooler.hover(function(){
    aWatercoolers.currentTime = 0;
    aWatercoolers.play();
}, function(){
    aWatercoolers.pause();
});


var $aNotifications = $('#aNotifications');
var aNotifications = $aNotifications[0];
var $ttNotifications = $('#ttNotifications');

$ttNotifications.hover(function(){
    aNotifications.currentTime = 0;
    aNotifications.play();
}, function(){
    aNotifications.pause();
});

var $aResources = $('#aResources');
var aResources = $aResources[0];
var $ttResources = $('#ttResources');

$ttResources.hover(function(){
    aResources.currentTime = 0;
    aResources.play();
}, function(){
    aResources.pause();
});




    

    





    
    

    
    

   
    function checkWidth() {
        
        var windowSize = $(window).width();

        if (windowSize >= 550) {
            $(".slicker").find("div.slick-track").contents().unwrap();  //slick slifer ****
            $(".slicker").find("div.slick-list").contents().unwrap();  //slick slifer ****
            // $('.slicker>a, #Content-Row-20-Sub>div').removeAttr('style');
        }
    }

    function checkWidthDelayed() {
        setTimeout(checkWidth, 100);
    }


    //if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
    //    // true for mobile device
    //    $(window).on('orientationchange', function () {
    //        $(window).resize(checkWidthDelayed);
    //    });
    //} 


    // Execute on load
    checkWidth();
    // Bind event listener
    $(window).resize(checkWidthDelayed);


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
});


