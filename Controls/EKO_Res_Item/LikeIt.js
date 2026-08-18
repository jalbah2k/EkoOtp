$(document).ready(function () {
    $('button.favBtn').click(function(){
        $('body').addClass("itsFaved");
        var arrayOfStrings = $(this).attr("values").split("&");
        var customer = { id: arrayOfStrings[0], ln: arrayOfStrings[1] };
       // alert(arrayOfStrings[0] );
        $.ajax({
            type: "POST",
            url: "/api/FavouriteResource",
            data: JSON.stringify(customer),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {                
               // alert(response);
                if (response == true) {
                    $('button.favBtn').addClass("favourite").attr("aria-pressed", "true");
                    $('#resLibCatBread').addClass("favourite");
                }
                else {
                    $('button.favBtn').removeClass("favourite").attr("aria-pressed", "false");
                    $('#resLibCatBread').removeClass("favourite");
                }
            },
            error: function (xhr, status, errorThrown) {
                alert(status + " | " + xhr.responseText);
            }
        });
    });
});

