<%@ Control Language="C#" AutoEventWireup="true" CodeFile="eForm.ascx.cs" Inherits="Controls_eForm_eForm" %>
<script src="/JS/jquery.regex.js" type="text/javascript"></script>
<style type="text/css">
/*    .efTable, cusomise eFrom table*/
    /*td.rightTD input:not(.numeric), td.rightTD select*/
    /*td.rightTD input:not(.efNumeric), td.rightTD select*/
    td.rightTD input, td.rightTD select
    {
        font-family: Arial,Sans-Serif;
        font-size: 13px;
        color: #343434;
    }
    td.rightTD
    {
        padding-left: 10px;
    }
    .eforms-wrapper input[type="text"], .eforms-wrapper textarea, .eforms-wrapper select
    {
        width: 100%;
        box-sizing: border-box;
    }
    .eforms-wrapper .red-star
    {
        color: #E60000;
        padding-right: 5px;
    }
    .efCB
    {
        margin-bottom: 0px;
    }
    .efCB input
    {
        display: inline-block;
        margin-top: 5px;
        margin-bottom: 0px;
        /*margin-right: 10px;*/
        margin-right: -100px;
        vertical-align: top;
    }
    .efCB label
    {
        display: inline-block;
        margin-left: 110px;
        margin-right: -100px;
    }
    .efTable input 
    {
        display: inline-block;
        margin-top: 5px;
        margin-bottom: 0px;
        /*margin-right: 10px;*/
        margin-right: 10px;
        vertical-align: top;
    }
    div.efNumericWrapper
    {
        position:relative;
        display:inline-block;
        height: 22px;
        margin-right:23px;
        margin-bottom: 15px;
    }
    input.efNumeric
    {
        /*font-family:Tahoma, Arial, Helvetica, sans-serif;
        font-size:15px;
        line-height:16px;
        font-weight:bold;*/
        text-align:center;
        padding: 0px;
        margin: 0px;
    }
    input.NumericUpDown_BtnUp
    {
        position:absolute;
        top:0px;
        right:-23px;
        margin: 0px;
    }
    input.NumericUpDown_BtnDown
    {
        position:absolute;
        bottom:0px;
        right:-23px;
        margin: 0px;
    }
    input.NumericUpDown_BtnUp:hover, input.NumericUpDown_BtnDown:hover
    {
        right:-24px;
    }
    .efTitle
    {
        font-size: 18px;
        font-weight: bold;
    }
    .efFooter
    {
        padding-top: 25px;
    }
    .switcher
    {
        float: left;
        border: black thin solid;
        cursor: pointer;
        width: 18px;
        height: 18px;
        vertical-align: middle;
        text-align: center;
    }
    .efWatermarked
    {
	    /*background-color:Transparent;
	    border:0;*/
	    font-family:Arial, Verdana, Tahoma, Sans-Serif;
	    font-size : 12px;
        font-style: italic;
        color: Gray;
	    vertical-align:middle;
    }
    .ef-horizontal-line
    {
        border-bottom: 1px solid #000;
    }
    .centered
    {
        text-align: center;
    }
    .ajax__calendar_container
    {
        z-index:99999999999999;
    }
    .hide { display:none; }
    .error { color: #f00!important; }
    .sbar 
    {
        background-color: #CCCCCC;
        font-family:  Arial, Helvetica, sans-serif;
        font-size: 10px;
        text-align: right;
        padding-right: 5px;
    }
    .wl 
    {
        font-family:  Arial, Helvetica, sans-serif;
        font-size: 11px;
        /*text-align: right;
        padding-right: 5px;*/
    }
    .no-width{
        width:unset!important;
    }
    .eforms-wrapper label{
        font-weight: 400;
    }
    span ~ label.span-label{
        display:contents;
    }
    .label-captcha{
        display:none;
    }
    .efPrompt{
        margin-top:20px;
    }
    .sp-hidden{
        display:none;
    }
</style>
<script type="text/javascript">
//       var termClose = document.getElementById('termClose'); 
        
    function openTerms(id) {
    var tbTerms = document.getElementById('tbTerms'+id);
    var tdTerms = document.getElementById('tdTerms'+id);
        var _zindex = 20;
        var _width = parseInt(tdTerms.style.width, 10);
        var _height = parseInt(tdTerms.style.height, 10);
        var _left = _termLeft; //-_width;
        var _top = _termTop - _height;

        if (_left < 0)
            _left = 0;
        if (_top < 0)
            _top = 0;

        tbTerms.style.display = "block";
        tbTerms.style.position = "relative";
        tbTerms.style.left = "0px";
        tbTerms.style.top = "0px";
        tbTerms.style.zindex = "10000";
    }
    function clsTerms(id) {
        document.getElementById('tbTerms' + id).style.display = "none";
    }

    function getXY(e) {
        _termLeft = (window.event) ? event.clientX + document.body.scrollLeft : e.pageX;
        _termTop = (window.event) ? event.clientY + document.body.scrollTop : e.pageY;
    }
//       termClose.onclick = function() { return clsTerms() };
</script><%=test %>
<script type="text/javascript">
    $(document).ready(function () {
        if (typeof window.FileReader != 'undefined') {
            $('.fu_eForms').change(function () {
                var fileSize = document.getElementById($(this).attr('id')).files[0].size;
                $(this).next().val(fileSize);
                var hfFolderSize = $(this).next().next();
                $.ajax({
                    type: "POST",
                    url: "/Default.aspx/GetDirectorySize",
                    data: JSON.stringify({ 'path': '<%= string.Format("{0}{1}/", eFormsUploadsPath, param) %>' }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var folderSize = 0;
                        try {
                            folderSize = parseInt(response.d);
                        }
                        catch (err) {
                            folderSize = 0;
                        }
                        hfFolderSize.val(fileSize + folderSize);
                    },
                    error: function (xhr, status, errorThrown) {
                        alert(status + " | " + xhr.responseText);
                    }
                });
            });
        }
    });
</script>
<script>

    jQuery(window).on('load', function () {
        $("textarea.ng-hide").attr("aria-hidden", "true");
        $("textarea.ng-hide").attr("aria-label", "do not use");
        $("textarea.ng-hide").attr("aria-readonly", "true");

        $("textarea.ng-hide ~ input[type=text]").attr("aria-hidden", "true");
        $("textarea.ng-hide ~ input[type=text]").attr("aria-label", "do not use");
        $("textarea.ng-hide ~ input[type=text]").attr("aria-readonly", "true");
    });
</script>

<%if (captcha)
    { %>
<script>

    jQuery(window).on('load', function () {
        var textarea = document.getElementById("g-recaptcha-response");
        textarea.setAttribute("aria-hidden", "true");
        textarea.setAttribute("aria-label", "do not use");
        textarea.setAttribute("aria-readonly", "true");

    });
</script>
<%} %>
<div class="eforms-wrapper" id="eform_<%=param %>"> 
    <!-- JUAN TO ADD UNIQUE ID -->
    <%--<div id="ng-app" ng-app="textAngularDemo" ng-controller="demoController" class="ng-scope">--%>
    <div class="row"><asp:PlaceHolder ID="pnEForm" runat="server"></asp:PlaceHolder></div>
    </div>
    <div id="divSubmitted" class="hide" runat="server" style="position:fixed; left:0; top:0; z-index:99999; width:100%; height:100%; min-height:100%;">
        <div class="SemiTransparentBg">&nbsp;</div> <%-- Set the semitransparent background for the parent div in order to allow opaque content --%>
        <div style="position:absolute; top:50%; width: 100%; text-align: center;">
            <div id="inner" style="position:relative; top:-95px; margin-left: auto; margin-right: auto;"> <%-- Set top = -height/2 to center the inner div --%>
                <h2 style="color:#ff0000; font-weight:bold; vertical-align:middle;">We are processing your submission.<br />Please do not press back or refresh.</h2><br /><br />
                <asp:Image ID="imgSubmitted" runat="server" ImageUrl="/images/ajax_loader_large.gif" AlternateText="spinner" style="vertical-align: middle;" />
            </div>
        </div>
    </div>
<%--</div>--%>
<script type="text/javascript">
    var WordCountValidator = function (ctv, v) {
        var tb = $("textarea:regex([class,/.* c" + ctv + "/])");
        var elClass = tb.attr('class');

        var number = 0;
        var minWords = 0;
        var maxWords = 0;
        var countControl = elClass.substring((elClass.indexOf('[')) + 1, elClass.lastIndexOf(']')).split(',');

        if (countControl.length > 1) {
            minWords = parseInt(countControl[0]);
            maxWords = parseInt(countControl[1]);
        }
        else {
            maxWords = parseInt(countControl[0]);
        }

        //var numWords = v.match(/\b/g);
        var numWords = v.match(/\s*[\w+\S]+\s*/g);

        if (numWords) {
            //number = numWords.length / 2;
            number = numWords.length;
        }

        if (minWords == 0)
            tb.siblings("div:regex([class,/" + ctv + " .*/])").children('strong').text(number + " / " + maxWords);
        else
            tb.siblings("div:regex([class,/" + ctv + " .*/])").children('strong').text(number + " / [" + minWords + "-" + maxWords + "]");

        if (number < minWords || (number > maxWords && maxWords != 0)) {
            tb.siblings("div:regex([class,/" + ctv + " .*/])").addClass('error');
            return false;
        }
        else {
            tb.siblings("div:regex([class,/" + ctv + " .*/])").removeClass('error');
            return true;
        }
    }

    $(document).ready(function () {

        $('.efNumeric').each(function () {
            var id = $(this).attr('id');
            $('[id^="' + id + '"]').wrapAll('<div class="efNumericWrapper"></div>');
        });

        $("textarea[class^='count[']").each(function () {

            $(this).on('keyup click blur focus change paste', function () {
                WordCountValidator($(this).attr('id'), $(this).val());
            });

            WordCountValidator($(this).attr('id'), $(this).val());
        });

    });

    function CheckWordLimit(sender, args) {
        args.IsValid = WordCountValidator(sender.controltovalidate, args.Value);
    }
</script>
