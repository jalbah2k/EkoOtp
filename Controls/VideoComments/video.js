
$('#div_whole_comments_parameter').ready(function () {

    var prev_action = "";

    $('#div_whole_comments_parameter').on("click", ".read-more", function () {

        var obj = $(this);
        var href = $(this).attr("href");

        var id = href.substring(4);
        $.ajax({
            type: "GET",
            url: "/api/Comment/" + id,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                $(obj).parent().parent().html(response);
            },
            error: function (xhr, status, errorThrown) {
                ;//alert(status + " | " + xhr.responseText);
            }
        });
    });

    $('#div_whole_comments_parameter').on("click", ".sp-link", function () {

        var href = $(this).attr("href");
        var video_id = $(this).attr("videoid");
        var parent_id = href.substring(4);

        var div_comment = $(this).parent().parent().parent();

        if (prev_action == "") {
            if ($(this).html().indexOf("↑") != -1) {
                div_comment.children(".div-replies").remove();
                var mylink = $("a.sp-link[href='#sp_" + parent_id + "']");
                mylink.html(mylink.html().replace("↑", "↓"));
                return;
            }
        }


        var myvalue = {
            parentid: parent_id,
            front: true,
            videoid: video_id
        };

        $.ajax({
            type: "POST",
            url: "/api/replies/",
            data: JSON.stringify(myvalue),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                var str = div_comment.html();
                if (prev_action == "add_reply") {
                    //remove replies to be refreshed 
                    var pos = str.indexOf('<div class="div-replies">');
                    if (pos > 1)
                        str = str.substring(0, pos);

                }

                div_comment.html(str + response);
                var mylink = $("a.sp-link[href='#sp_" + parent_id + "']");
                if (prev_action == "") {
                    mylink.html(mylink.html().replace("↓", "↑"));
                }
                else {
                    //alert(mylink.html());
                    var text = mylink.html();
                    const myArray = text.split(" ");
                    var tot = myArray[0];
                    var num = parseInt(tot, 10) + 1;
                    text = text.replace(tot, num);
                    if (num > 1)
                        text = text.replace('reply', 'replies');

                    mylink.html(text);
                }
                prev_action = "";

            },
            error: function (xhr, status, errorThrown) {
                prev_action = "";
                ;//alert(status + " | " + xhr.responseText);
            }
        });
    });

    $('#div_whole_comments_parameter').on("click", ".icon_comment", function () {

        var href = $(this).attr("href");
        var video_id = $(this).attr("videoid");
        var parent_id = href.substring(12);

        var myvalue = {
            parentid: parent_id,
            front: true,
            videoid: video_id
        };

        $.ajax({
            type: "POST",
            url: "/api/replies/",
            data: JSON.stringify(myvalue),
            contentType: "application/json; charset=utf-8",
            success: function (response) {

                $('#txtVideoId').val(video_id);
                $('#txtParentId').val(parent_id);
                $('#txtNewComment').val('');

                $('#myModal_Comment').show();
            },
            error: function (xhr, status, errorThrown) {
                ;//alert(status + " | " + xhr.responseText);
            }
        });

    });

    $('#div_whole_comments_parameter').on("click", "#btnAddComment_parameter", function () {

        $('#txtVideoId').val($(this).attr("videoid"));
        $('#txtParentId').val($(this).attr("parentid"));
        $('#txtNewComment').val('');

        $('#myModal_Comment').show();
    });

    $(document).on("click", "#btnSaveComment", function () {

        var video_id = $(this).parent().siblings('#txtVideoId').val();
        if (video_id != _parameter)
            return;

        var parent_id = $(this).parent().siblings('#txtParentId').val();
        var user_id = $(this).parent().siblings('#txtUId').val();
        var comments = $(this).parent().siblings('#txtNewComment').val();

        var mynewvalue = {
            parentid: parent_id,
            videoid: video_id,
            userid: user_id,
            comment: comments
        };

        $.ajax({
            type: "POST",
            url: "/api/Comment/",
            data: JSON.stringify(mynewvalue),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (response == true) {
                    if (parent_id > 0) {
                        //then it's a reply
                        if ($("a.sp-link[href='#sp_" + parent_id + "']").length == 0) {
                            var obj = $('[href="#addcomment_' + parent_id + '"]');
                            obj.parent().siblings('div').html("<a href='#sp_" + parent_id + "' class='sp-link' videoid='" + video_id + "' title='view replies'>0 reply ↑</a>");
                        }

                        prev_action = "add_reply";
                        $("a.sp-link[href='#sp_" + parent_id + "']").click();
                        //program it when $("a.sp-link[href='#sp_" + parent_id + "']")= null
                    }
                    else {
                        //location.reload();

                        var myvideo = {
                            front: true,
                            videoid: video_id
                        }

                        $.ajax({
                            type: "POST",
                            url: "/api/GetAllComments/",
                            data: JSON.stringify(myvideo),
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                $('#div_whole_comments_parameter').html(response);
                            },
                            error: function (xhr, status, errorThrown) {
                                ;//alert(status + " | " + xhr.responseText);
                            }
                        });
                    }
                }
            },
            error: function (xhr, status, errorThrown) {
                ;//alert(status + " | " + xhr.responseText);
            }
        });

        $('#myModal_Comment').hide();

    });

    $(document).on("click", "#btnCancelComment", function () {
        $('#myModal_Comment').hide();

    });

});