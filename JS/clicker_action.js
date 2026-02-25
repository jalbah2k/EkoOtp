/**
 * actionn
 *
 */

(function($, undefined){
jQuery(document).ready(function(){
//////////////////////////////////////////////////

    //$("ul:not(:has( > li))").hide();?
    //$('ul').hide();


    $('ul').each(function(){
        var blankCheck = $(this).html().trim();
        if(0== blankCheck.length) {
            $(this).hide();
        }
    });


//////////////////////////////////////////////////
});
} )( jQuery );