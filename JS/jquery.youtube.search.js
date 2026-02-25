function init() {
    gapi.client.setApiKey("AIzaSyASW5KXBQrGcJ5GWWQBSvGIGC8HcjtEj-o");
    gapi.client.load("youtube", "v3", function () {
        // yt api is ready
    });
}
jQuery.fn.YoutubeSearch = function (settings) {
    settings = jQuery.extend({
        maxResults: 5,
        order: 'viewCount',     //relevance
        publishedAfter: '',
        inputSelector: '',
        resultsSelector: '',
        prevSelector: '',
        nextSelector: ''
    }, settings);
    var input = '';
    var sendRequest = function (input, pageToken) {
        // prepare the request
        var request = gapi.client.youtube.search.list({
            part: "snippet",
            type: "video",
            q: input,
            maxResults: settings.maxResults,    // 1 to 50
            order: settings.order,
            pageToken: pageToken,
            publishedAfter: settings.publishedAfter
        });
        // execute the request
        request.execute(function (response) {
            //console.log(response);
            //var itemTemplate = '<div class="draggable-item"><h2><#title#></h2><iframe class="video w100" width="640" height="360" src="//www.youtube.com/embed/<#videoid#>" frameborder="0" allowfullscreen></iframe></div>';
            //var itemTemplate = '<div class="draggable-item"><h2><#title#></h2><iframe class="video w100" width="480" height="270" src="//www.youtube.com/embed/<#videoid#>" frameborder="0" allowfullscreen></iframe></div>';
            var itemTemplate = '<div class="draggable-item"><h2><#title#></h2><iframe class="video Responsive-Video" width="320" height="180" src="//www.youtube.com/embed/<#videoid#>" frameborder="0" allowfullscreen></iframe></div>';
            var results = response.result;
            if (results.prevPageToken != null && results.prevPageToken != 'undefined') {
                jQuery(settings.prevSelector).removeClass('hide');
                jQuery(settings.prevSelector).attr('pageToken', results.prevPageToken);
            }
            else {
                jQuery(settings.prevSelector).addClass('hide');
                jQuery(settings.prevSelector).removeAttr('pageToken');
            }
            if (results.nextPageToken != null && results.nextPageToken != 'undefined') {
                jQuery(settings.nextSelector).removeClass('hide');
                jQuery(settings.nextSelector).attr('pageToken', results.nextPageToken);
            }
            else {
                jQuery(settings.nextSelector).addClass('hide');
                jQuery(settings.nextSelector).removeAttr('pageToken');
            }
            jQuery(settings.resultsSelector).html("");
            jQuery.each(results.items, function (index, item) {
                //console.log(item);
                jQuery(settings.resultsSelector).append(itemTemplate.replace("<#title#>", item.snippet.title).replace("<#videoid#>", item.id.videoId));
            });
            //resetVideoHeight();
        });
    };
    jQuery(settings.nextSelector + ',' + settings.prevSelector).click(function (e) {
        var pageToken = '';
        try {
            pageToken = jQuery(this).attr('pageToken');
        }
        catch (err) {}
        sendRequest(input, pageToken);
        return false;
    });
    return this.each(function (e) {
        var el = jQuery(this);
        el.click(function (e) {
            //e.preventDefault();
            input = jQuery(settings.inputSelector).val();
            try {
                input = input.trim();
            }
            catch (err) { }
            if (input.length == 0)
                return false;
            input = encodeURIComponent(input).replace(/%20/g, "+");

            sendRequest(input, '');
            //// prepare the request
            //var request = gapi.client.youtube.search.list({
            //    part: "snippet",
            //    type: "video",
            //    q: input,
            //    maxResults: settings.maxResults,    // 1 to 50
            //    order: settings.order,
            //    publishedAfter: settings.publishedAfter
            //});
            //// execute the request
            //request.execute(function (response) {
            //    //console.log(response);
            //    //var itemTemplate = '<div class="draggable-item"><h2><#title#></h2><iframe class="video w100" width="640" height="360" src="//www.youtube.com/embed/<#videoid#>" frameborder="0" allowfullscreen></iframe></div>';
            //    //var itemTemplate = '<div class="draggable-item"><h2><#title#></h2><iframe class="video w100" width="480" height="270" src="//www.youtube.com/embed/<#videoid#>" frameborder="0" allowfullscreen></iframe></div>';
            //    var itemTemplate = '<div class="draggable-item"><h2><#title#></h2><iframe class="video u-full-width" width="320" height="180" src="//www.youtube.com/embed/<#videoid#>" frameborder="0" allowfullscreen></iframe></div>';
            //    var results = response.result;
            //    if (results.prevPageToken != null && results.prevPageToken != 'undefined') {
            //        jQuery(settings.prevSelector).removeClass('hide');
            //    }
            //    else {
            //        jQuery(settings.prevSelector).addClass('hide');
            //    }
            //    if (results.nextPageToken != null && results.nextPageToken != 'undefined') {
            //        jQuery(settings.nextSelector).removeClass('hide');
            //    }
            //    else {
            //        jQuery(settings.nextSelector).addClass('hide');
            //    }
            //    jQuery(settings.resultsSelector).html("");
            //    jQuery.each(results.items, function (index, item) {
            //        //console.log(item);
            //        jQuery(settings.resultsSelector).append(itemTemplate.replace("<#title#>", item.snippet.title).replace("<#videoid#>", item.id.videoId));
            //    });
            //    //resetVideoHeight();
            //});
            return false;
        });
    });
};
