$(document).ready(function () {
    $('button.LikeIt').click(function(){
        //alert($(this).attr("id"));

        var arrayOfStrings = $(this).attr("id").split("_");
        var customer = { id: arrayOfStrings[0] };
 //       alert(arrayOfStrings[1] );
        $.ajax({
            type: "POST",
            url: "/api/LikeNews",
            data: JSON.stringify(customer),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {                
                //alert(response.d);
                $('#divLikeit button').attr('disabled', 'disabled');
                $('#divLikeit button img').attr('src', '/Images/Icons/thumb-up-64_blue.png');
            },
            error: function (xhr, status, errorThrown) {
                alert(status + " | " + xhr.responseText);
            }
        });
    });
});

