<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Logos.ascx.cs" Inherits="Controls_Logos_Logos" %>

<%--<script src="/js/jquery.fitvids.js" type="text/javascript"></script>--%>
<%--<script src="/js/jquery.bxSlider.min.js" type="text/javascript"></script>--%>
<link href="/CSS/jquery.bxslider.logos.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    $(document).ready(function () {
        $('#<%= LogoSlider.ClientID %>').bxSlider({
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
            slideWidth: <%= MaxWidth %>,
            //slideWidth: '230',
            controls: ('<%= controls %>'.toLowerCase() == 'true') ? true : false,
            nextSelector: ('<%= customControls %>'.toLowerCase() == 'true') ? '#<%= logoSliderNext.ClientID %>' : '',
            prevSelector: ('<%= customControls %>'.toLowerCase() == 'true') ? '#<%= logoSliderPrev.ClientID %>' : '',
            nextText: ('<%= customControls %>'.toLowerCase() == 'true') ? '<img src="<%= nextImage %>" alt="next"/>' : 'Next',
            prevText: ('<%= customControls %>'.toLowerCase() == 'true') ? '<img src="<%= prevImage %>" alt="prev"/>' : 'Prev',
            <%--startText: '<img src="<%= startImage %>" alt="start"/>',
            stopText: '<img src="<%= stopImage %>" alt="stop"/>',--%>
            pause: '<%= pause %>',
            ticker: ('<%= ticker %>'.toLowerCase() == 'true') ? true : false,
            tickerHover: ('<%= tickerHover %>'.toLowerCase() == 'true') ? true : false,
            randomStart: ('<%= shuffle %>'.toLowerCase() == 'true') ? true : false,
        });

        var SwapImgSrcOnHover = function (element) {
            var ImgHover = element.attr('hoverImage');
            var ImgSrc = element.attr('src');
            element.attr('src', ImgHover);
            element.attr('hoverImage', ImgSrc);
        }
        $('.hoverable').mouseenter(function () {
            SwapImgSrcOnHover($(this));
        });
        $('.hoverable').mouseleave(function () {
            SwapImgSrcOnHover($(this));
        });
    });
</script>
<div class="logo-slider-wrapper logo-gallery-<%= GalleryId %>">
    <div id="LogoSlider" runat="server" class="logo-slider">
        <asp:Repeater ID="Repeater1" runat="server" onitemdatabound="Repeater1_ItemDataBound">
        <ItemTemplate>
            <div class="logo-slider-item" style="width:<%= MaxWidth %>px; height:<%= MaxHeight %>px;">
                <asp:Literal ID="litImage" runat="server"></asp:Literal>
            </div>
            <div id="Desc" runat="server" class="logo-slider-desc"><%# Eval("Description") %></div>
        </ItemTemplate>
        </asp:Repeater>
    </div>
    <% if (customControls) { %>
    <div id="logoSliderPrev" runat="server" class="logo-slider-prev"></div>
    <div id="logoSliderNext" runat="server" class="logo-slider-next"></div>
    <% } %>
</div>
