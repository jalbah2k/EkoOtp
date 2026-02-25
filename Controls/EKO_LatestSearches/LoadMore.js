$(document).ready(function () {
    var offset = 1;
    $('#span_load_more').click(function(){

        $.ajax({
            type: "GET",
            url: "/api/LoadSearches/" + offset + "/" + records + "/" + category,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {                
                //alert(response);
                if (response != "") {
                    $('.div_load_more .grid3col.contained-width').html($('.div_load_more .grid3col.contained-width').html() + response);
                    offset++;

                    $.ajax({
                        type: "GET",
                        url: "/api/LoadSearches/" + offset + "/" + records + "/" + category,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            //alert(response);
                            if (response == "") {
                                $('#span_load_more').hide();
                            }
                        },
                        error: function (xhr, status, errorThrown) {
                            alert(status + " | " + xhr.responseText);
                        }
                    });

                }
                else
                    $('#span_load_more').hide();
            },
            error: function (xhr, status, errorThrown) {
                alert(status + " | " + xhr.responseText);
            }
        });
    });
});

