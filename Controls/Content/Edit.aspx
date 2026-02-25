<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="Controls_Content_Edit" ValidateRequest="false" %>

<%@ Register src="~/Controls/Content/Edit.ascx" tagname="Edit" tagprefix="uc1" %>


<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title></title>
    <asp:PlaceHolder runat="server">
	<link href="/CSS/Admin.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="/CSS/base.css?v=<%= "" + ConfigurationManager.AppSettings.Get("CSSVersion") %>" />
    <link rel="stylesheet" href="/CSS/site.css?v=<%= "" + ConfigurationManager.AppSettings.Get("CSSVersion") %>" />

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.2/themes/smoothness/jquery-ui.css" />
    </asp:PlaceHolder>

    	<!-- Favicon
    –––––––––––––––––––––––––––––––––––––––––––––––––– -->
	<!-- ****** faviconit.com favicons ****** -->
	<link rel="shortcut icon" href="/uploads/favicon/fav-icon.ico">

<%--    <script src="//code.jquery.com/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js" type="text/javascript"></script>--%>

    <script type="text/javascript" src="/Libraries/jquery/jquery-3.7.1.min.js"></script>
    <script type="text/javascript" src="/Libraries/jquery-ui-1.14.1/jquery-ui.min.js"></script>

    
    <link rel="stylesheet" href="//code.jquery.com/resources/demos/style.css" />
    <% if (1 == 0 /*(bool)Session["Multilingual"]*/) { %>
    <script type="text/javascript">

        $(function () {
            $("#tabs").tabs();
        });

        window.location.href = '#tabs-<%= Session["Language"].ToString()%>';

        $(document).ready(function () {
            $("#tabs").on("tabsactivate", function (event, ui) {
                $('iframe.CuteEditorFrame').css('height', '460px');
            });
        });

    </script>
    <% } %>
    <script type="text/javascript">

        $(document).ready(function () {
            
            <%if (Edit1.IsPending){ %>
                $('#li_en').css("color", "#ff0000");
            <%}
            if (Edit2.IsPending){ %>
                $('#li_fr').css("color", "#ff0000");
            <%} %>


             <% if (Session["LoggedInID"] == null){%>
                $('.tab_link').hide();
            <%} %>

        });       

    </script>


<%--<script type="text/javascript">
function RefreshParent()
{
	window.parent.parent.location.href = window.parent.parent.location.href;
	alert("test");
}

window.onbeforeunload = function (evt) {
var message = 'Are you sure you want to leave?';
if (typeof evt == ‘undefined’) {
evt = window.event;
}
if (evt) {
evt.returnValue = message;
}
return message;
}

</script>--%>

<style type="text/css">
    /*#CE_Edit1_Editor1_ID_Frame, #CE_Edit2_Editor1_ID_Frame
    {
        height:360px!important;     this patch is to fix the issue with Cuteeditor int IE11 and the secondary tab (the tab that is hiden when the page is rendered)
    }*/
    .button,
    button,
    input[type="submit"],
    input[type="reset"],
    input[type="button"] 
    {
        color: #000000;
        /*padding: 0px 0.7rem;*/
        border: 0px;
    }
    /*#btnDone
    {
        border: 2px solid #000000;
    }*/
   
    .CuteEditorBottomBarContainer img.CuteEditorButton:nth-child(3){
        display:none!important;
    }
    html {
            background-color: #f0f0ea!important;
        }

</style>

</head>
<body style="background-image:none; background-repeat:repeat-y; background-position:center; width:100%; background-color:#f0f0ea;position:relative;">
<form id="form1" runat="server"><center>
<asp:ScriptManager ID="sman" runat="server" EnablePartialRendering="False" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
<ContentTemplate><asp:Panel ID="Panel1" runat="server" Visible="True"><table border="0" cellpadding="0" cellspacing="0" style="width:953px; text-align:left; margin:0px;" width="953" class="inedit">
<tr><td style="padding-top:20px;">

<div id="tabs">
<% if (1 == 0 /*(bool)Session["Multilingual"]*/) { %>
  <ul class="tab_link">
    <li><a href="#tabs-1" id="li_en">English</a></li>
    <li><a href="#tabs-2" id="li_fr">French</a></li>
  </ul>
<% } %>
  <div id="tabs-1">
      <uc1:Edit ID="Edit1" runat="server" Language="1" />
  </div>
<% if (1 == 0 /*(bool)Session["Multilingual"]*/) { %>
  <div id="tabs-2"> 
      <uc1:Edit ID="Edit2" runat="server" Language="2" />
  </div>
<% } %>
</div>

<br /><asp:Button runat="server" Text="Done" id="btnDone" onclick="btnDone_Click" CssClass="button-primary" />

</td></tr></table></asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</center>
    </form>
    <script>
    var iframe = window.document.getElementById("CE_Edit1_Editor1_ID_Frame");
    iframe.onload = function () {
        var iframeDoc = iframe.contentDocument;
        var s = iframeDoc.createElement('script');
        s.type = 'text/javascript';
        s.src = '//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js';
        iframeDoc.head.appendChild(s);

        s = iframeDoc.createElement('script');
        s.type = 'text/javascript';
        s.src = '/js/CollapsiblePanel.js';
        iframeDoc.head.appendChild(s);
    }
</script>
    </body>
</html>

