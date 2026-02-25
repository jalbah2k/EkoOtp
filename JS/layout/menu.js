

//Initial bind
$(document).ready(function () {

    //----------------------------------------
    //-------- MENU --------------------------
    $('#nav-icon').click(function () {
        $(this).toggleClass('open');
        $("#mainMenu").toggle();
    });

    // $(".list-group-item").click(function () {
    //     alert("clicked");
    // });
   

    //function checkWidth() {
    //    var windowSize = $(window).width();

    //    if (windowSize <= 767) {
    //        $("#mainMenu").hide();
    //        $("#nav-icon").removeClass('open');
    //    }
    //    else if (windowSize >= 768) {
    //        $("#mainMenu").show();
    //    }
    //}

    //// Execute on load
    //checkWidth();
    //// Bind event listener
    //$(window).resize(checkWidth);


  
    /*search for mobile*/
    $("#mobileSearchToggle").on("click", function () {
        //alert($("#mainMenu").attr("style"));
        if ($("#mainMenu").attr("style") == 'display: none;' || $("#mainMenu").attr("style") != 'display: block;')
            $("#nav-icon").click();

        $('#mobileSearch input[type=text]').focus();

    });
    
     $("#leftMenuDropWrap").click(function () {
        $("#dropWrap").toggleClass('dropOpen');
    });
});
