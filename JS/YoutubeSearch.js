$(function () {
    //$("form").on("submit", function (e) {
    $('.btnYoutubeSearch').on("click", function (e) {
        //alert("Form Submitted");
        //e.preventDefault();
        // prepare the request
        var request = gapi.client.youtube.search.list({
            part: "snippet",
            type: "video",
            q: encodeURIComponent($('.txtYoutubeSearch').val()).replace(/%20/g, "+"),
            maxResults: 5,
            order: "viewCount",
            publishedAfter: "2016-01-01T00:00:00Z"
        });
        // execute the request
        request.execute(function (response) {
            //console.log(response);
            //var itemTemplate = '<div class="draggable-item"><h2><#title#></h2><iframe class="video w100" width="640" height="360" src="//www.youtube.com/embed/<#videoid#>" frameborder="0" allowfullscreen></iframe></div>';
            //var itemTemplate = '<div class="draggable-item"><h2><#title#></h2><iframe class="video w100" width="480" height="270" src="//www.youtube.com/embed/<#videoid#>" frameborder="0" allowfullscreen></iframe></div>';
            var itemTemplate = '<div class="draggable-item"><h2><#title#></h2><iframe class="video u-full-width" width="320" height="180" src="//www.youtube.com/embed/<#videoid#>" frameborder="0" allowfullscreen></iframe></div>';
            //var itemTemplate = '<div class="draggable-item"><iframe class="video w100" width="640" height="360" src="//www.youtube.com/embed/<#videoid#>" frameborder="0" allowfullscreen></iframe></div>';
            var results = response.result;
            $('#YoutubeResults').html("");
            $.each(results.items, function (index, item) {
                //console.log(item);
                //$('#YoutubeResults').append(item.id.videoId + " " + item.snippet.title + "<br />");
                $('#YoutubeResults').append(itemTemplate.replace("<#title#>", item.snippet.title).replace("<#videoid#>", item.id.videoId));
            });
            //resetVideoHeight();
        });
        return false;
    });

    //$(window).on("resize", resetVideoHeight);
});

//function resetVideoHeight() {
//    $('.video').css("height", $('#YoutubeResults').width() * 9 / 16);
//}

function init() {
    gapi.client.setApiKey("AIzaSyASW5KXBQrGcJ5GWWQBSvGIGC8HcjtEj-o");
    gapi.client.load("youtube", "v3", function () {
        // yt api is ready
    });
}