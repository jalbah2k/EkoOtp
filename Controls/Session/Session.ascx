<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Session.ascx.cs" Inherits="Controls_Session_Session" %>
<%--<script src="../../JS/jquery-1.3.1.min-vsdoc.js" type="text/javascript"></script>--%>
<style type="text/css">
.greenbox
{
    /*position:absolute;top:0px;right:50%;width:200px;background-color:green;padding:10px;*/
    position:relative;top:0px;left:-145px;width:200px;background-image:url('/images/lemonaid/partials/session_bg_on.png'); background-repeat:no-repeat; width:289px; height:93px; color:#c61414; text-align:center; padding-top:10px;
}

.redbox
{
	position:relative;top:0px;left:-145px;width:200px;background-image:url('/images/lemonaid/partials/session_bg.png'); background-repeat:no-repeat; width:289px; height:93px; color:#c61414; text-align:center; padding-top:10px;
}

.blackbox
{
	/*position:absolute;top:0px;right:50%;width:200px;background-color:#555555;padding:10px;*/
    position:relative;top:0px;left:-145px;width:200px;background-image:url('/images/lemonaid/partials/session_bg_off.png'); background-repeat:no-repeat; width:289px; height:93px; color:#c61414; text-align:center; padding-top:10px;
}

</style>
<script type="text/javascript">
    var diff = 2;
    var milisec = 0;
    var seconds = 0;
    var minutes = parseInt('<%= Session.Timeout %>') - diff;
    var myVar;


    $(document).ready(function () {

        //$(".hoverarea").hover(function () { $("#time").slideDown("100") }, function () { $("#time").slideUp("100") });
        $("#refresher").click(function () {

            milisec = 0;
            seconds = 0;
            minutes = parseInt('<%= Session.Timeout %>') - diff;

            $("#warning").slideUp("200");

            $("#box").addClass("greenbox");
            $("#box").removeClass("redbox");

            document.getElementById('test').innerHTML = "Session Active";
            try { clearTimeout(myVar); }
            catch (err) { }

            display();

        });
    });

    function display() {

        milisec -= 1

        if (milisec < 0) {
            milisec = 9
            seconds -= 1
        }
        if (seconds < 0) {
            seconds = 59
            minutes -= 1

        }
        if (minutes < 0) {

            document.getElementById('test').innerHTML = "<span style=\"color:#c61414;\">Session Expires Soon</span>";
            $("#box").removeClass("greenbox");
            $("#box").addClass("redbox");
            $("#box").slideDown("300");
           // $("#warning").slideDown("300");
            document.getElementById("refresher").focus();

            milisec = 0;
            seconds = 59;
            minutes = diff;

            displayFinal();
        }
        else {
            //document.getElementById('time').innerHTML = minutes + "." + seconds + "." + milisec
            //document.getElementById('test').innerHTML = "Active";
            setTimeout("display()", 100);
        }
    }

    function displayFinal() {

        milisec -= 1

        if (milisec < 0) {
            milisec = 9
            seconds -= 1
        }
        if (seconds < 0) {
            seconds = 59
            minutes -= 1

        }
        if (minutes < 0) {

            document.getElementById('test').innerHTML = "Session Lost";
            $("#box").removeClass("greenbox");
            $("#box").removeClass("redbox");
            $("#box").addClass("blackbox");

            $("#box").slideUp("500");

            window.location = "https://" + '<%= ConfigurationManager.AppSettings["SiteUrl"] %>';
        }
        else {
            myVar = setTimeout("displayFinal()", 100);
        }
    }
 
</script>
<div id="box" class="greenbox" style="display:none;">
<div id="test" style="font-size:16px;font-family:Arial, Verdana, Sans-Serif;color:#41af2c;">Session Active</div><div id="time"></div>
<div id="warning" style="font-family:Arial, Verdana, Sans-Serif;font-size:20px;color:#ffffff;padding-top:15px;"><a id="refresher" href="refresh.aspx" target="iF" style="color:#c61414;">Stay Logged In</a></div><div style="display:none;"><iframe name="iF" id="iF" runat="server" /></div>
</div>
<script>display()</script>