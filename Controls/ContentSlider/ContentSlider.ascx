<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContentSlider.ascx.cs" Inherits="Controls_ContentSlider_ContentSlider" %>

<%--<script src="/js/jquery.fitvids.js" type="text/javascript"></script>--%>
<%--<script src="/js/jquery.bxSlider.min.js" type="text/javascript"></script>--%>
<link href="/CSS/jquery.bxslider.content.css" rel="stylesheet" type="text/css" />
<link href="/CSS/timelineTweet.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    $(document).ready(function () {
        $('#<%= ContentSlider.ClientID %>').bxSlider({
            video: false,
            useCSS: true,
            pager: false,
            captions: false,
            mode: '<%= mode %>',
            auto: ('<%= auto %>'.toLowerCase() == 'true') ? true : false,
            autoStart: ('<%= autoStart %>'.toLowerCase() == 'true') ? true : false,
            autoControls: ('<%= autoControls %>'.toLowerCase() == 'true') ? true : false,
            autoControlsCombine: true,
            autoHover: ('<%= autoHover %>'.toLowerCase() == 'true') ? true : false,
            autoDirection: '<%= autoDirection %>',
            speed: <%= speed %>,
            minSlides: <%= minSlides %>,
            maxSlides: <%= maxSlides %>,
            moveSlides: <%= moveSlides %>,
            slideWidth: <%= Width %>,
            controls: ('<%= controls %>'.toLowerCase() == 'true') ? true : false,
            nextSelector: ('<%= customControls %>'.toLowerCase() == 'true') ? '#<%= contentSliderNext.ClientID %>' : '',
            prevSelector: ('<%= customControls %>'.toLowerCase() == 'true') ? '#<%= contentSliderPrev.ClientID %>' : '',
            nextText: ('<%= customControls %>'.toLowerCase() == 'true') ? '<img src="<%= nextImage %>" alt="next"/>' : 'Next',
            prevText: ('<%= customControls %>'.toLowerCase() == 'true') ? '<img src="<%= prevImage %>" alt="prev"/>' : 'Prev',
            <%--startText: '<img src="<%= startImage %>" alt="start"/>',
            stopText: '<img src="<%= stopImage %>" alt="stop"/>',--%>
            pause: '<%= pause %>',
            ticker: ('<%= ticker %>'.toLowerCase() == 'true') ? true : false,
            tickerHover: ('<%= tickerHover %>'.toLowerCase() == 'true') ? true : false,
            randomStart: ('<%= shuffle %>'.toLowerCase() == 'true') ? true : false,
            adaptiveHeight: ('<%= adaptiveHeight %>'.toLowerCase() == 'true') ? true : false,
            adaptiveHeightSpeed: 500
        });
    });
</script>
<div class="content-slider-wrapper content-gallery-<%= GalleryId %>">
    <div class="content-slider-title"><asp:Literal ID="litTitle" runat="server" /></div>
    <div id="ContentSlider" runat="server" class="content-slider">
        <asp:Repeater ID="repContentSlider" runat="server" onitemdatabound="repContentSlider_ItemDataBound">
        <ItemTemplate>
            <%--<div class="content-slider-item" style="width:<%= Width %>px; height:<%= Height %>px;">--%>
                <asp:Literal ID="litContent" runat="server" />
            <%--</div>--%>
        </ItemTemplate>
        </asp:Repeater>
    </div>
    <% if (customControls) { %>
    <div id="contentSliderPrev" runat="server" class="content-slider-prev"></div>
    <div id="contentSliderNext" runat="server" class="content-slider-next"></div>
    <% } %>
</div>
