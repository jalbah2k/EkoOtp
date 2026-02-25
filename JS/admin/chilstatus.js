$(document).ready(function() {
    var pndng = $('img.pending').click(function() {
    var thisdiv = $(this).next().find(':first');
        if (thisdiv.is(':hidden'))
            pndng.next().find(':first:visible').hide('slow');
        thisdiv.toggle('slow');
    });
});