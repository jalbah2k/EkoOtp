function init() {
    gapi.client.setApiKey("AIzaSyASW5KXBQrGcJ5GWWQBSvGIGC8HcjtEj-o");
    gapi.client.load("youtube", "v3", function () {
        // yt api is ready
    });
}