$(document).ready(function () {
    var pageNumber = 2;
    var pageSize = typeof records !== "undefined" ? records : 5;
    var loading = false;

    $('#span_load_more_whatisnew').click(function () {
        if (loading) {
            return;
        }

        loading = true;

        $.ajax({
            type: "GET",
            url: "/api/LoadWhatisNew/" + pageNumber + "/" + pageSize,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null && response !== "") {
                    $('.Whats-New-0').append(response);
                    pageNumber++;

                    $.ajax({
                        type: "GET",
                        url: "/api/LoadWhatisNew/" + pageNumber + "/" + pageSize,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (nextResponse) {
                            if (nextResponse == null || nextResponse === "") {
                                $('#span_load_more_whatisnew').hide();
                            }
                            loading = false;
                        },
                        error: function () {
                            loading = false;
                        }
                    });
                }
                else {
                    $('#span_load_more_whatisnew').hide();
                    loading = false;
                }
            },
            error: function (xhr, status, errorThrown) {
                loading = false;
                alert(status + " | " + xhr.responseText);
            }
        });
    });
});
