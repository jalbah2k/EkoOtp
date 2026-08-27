<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BannerGallery.ascx.cs" Inherits="BannerGallery" %>

<asp:Panel ID="pnlGallery" runat="server" CssClass="bg-gallery">

    <div id="<%= this.ClientID %>_root"
         class="bg-root"
         data-autoplay="<%= this.Autoplay ? "1" : "0" %>"
         data-interval="<%= this.AutoplayInterval %>"
         role="region"
         aria-roledescription="carousel"
         aria-label="Featured announcements">

        <div class="bg-viewport">
            <asp:Repeater ID="rptBanners" runat="server" OnItemDataBound="rptBanners_ItemDataBound">
                <ItemTemplate>
                    <div id="divSlide" runat="server" class="bg-slide"
                         role="group" aria-roledescription="slide">

                        <div class="bg-slide-media">
                            <asp:HyperLink ID="lnkImage" runat="server" CssClass="bg-media-link">
                                <asp:Image ID="imgBanner" runat="server" CssClass="bg-image" />
                            </asp:HyperLink>
                        </div>

                        <div class="bg-slide-body">
                            <asp:Literal ID="litEyebrow" runat="server" />
                            <asp:Literal ID="litTitle" runat="server" />
                            <asp:Literal ID="litBody" runat="server" />

                            <asp:PlaceHolder ID="phActions" runat="server">
                                <div class="bg-actions">
                                    <asp:HyperLink ID="lnkPrimary" runat="server" CssClass="bg-btn bg-btn-primary" />
                                    <asp:HyperLink ID="lnkSecondary" runat="server" CssClass="bg-btn bg-btn-secondary" />
                                </div>
                            </asp:PlaceHolder>
                        </div>

                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <asp:PlaceHolder ID="phControls" runat="server">
            <button type="button" class="bg-nav bg-nav-prev" aria-label="Previous banner">
                <span aria-hidden="true">&#8249;</span>
            </button>
            <button type="button" class="bg-nav bg-nav-next" aria-label="Next banner">
                <span aria-hidden="true">&#8250;</span>
            </button>

            <div class="bg-dots">
                <asp:Repeater ID="rptDots" runat="server">
                    <ItemTemplate>
                        <button type="button" class="bg-dot"
                                data-index="<%# Container.ItemIndex %>"
                                aria-label='<%# "Show banner " + (Container.ItemIndex + 1) %>'></button>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </asp:PlaceHolder>

        <div class="bg-live" aria-live="polite" aria-atomic="true"></div>
    </div>

    <script type="text/javascript">
        (function () {
            var root = document.getElementById('<%= this.ClientID %>_root');
            if (!root) { return; }

            var slides = root.querySelectorAll('.bg-slide');
            if (slides.length === 0) { return; }

            var dots = root.querySelectorAll('.bg-dot');
            var live = root.querySelector('.bg-live');
            var prev = root.querySelector('.bg-nav-prev');
            var next = root.querySelector('.bg-nav-next');

            var current = 0;
            var timer = null;

            var interval = parseInt(root.getAttribute('data-interval'), 10);
            if (isNaN(interval) || interval < 2000) { interval = 6000; }

            // Never auto-rotate for someone who has asked for reduced motion.
            var reduceMotion = window.matchMedia &&
                window.matchMedia('(prefers-reduced-motion: reduce)').matches;

            var autoplay = root.getAttribute('data-autoplay') === '1' &&
                slides.length > 1 && !reduceMotion;

            function show(index) {
                if (index < 0) { index = slides.length - 1; }
                if (index >= slides.length) { index = 0; }

                var i;
                for (i = 0; i < slides.length; i++) {
                    if (i === index) {
                        slides[i].classList.add('is-active');
                        slides[i].removeAttribute('aria-hidden');
                    } else {
                        slides[i].classList.remove('is-active');
                        slides[i].setAttribute('aria-hidden', 'true');
                    }
                }

                for (i = 0; i < dots.length; i++) {
                    if (i === index) {
                        dots[i].classList.add('is-active');
                        dots[i].setAttribute('aria-current', 'true');
                    } else {
                        dots[i].classList.remove('is-active');
                        dots[i].removeAttribute('aria-current');
                    }
                }

                current = index;

                if (live) {
                    live.innerHTML = 'Banner ' + (index + 1) + ' of ' + slides.length;
                }
            }

            function start() {
                if (!autoplay || timer) { return; }
                timer = window.setInterval(function () { show(current + 1); }, interval);
            }

            function stop() {
                if (timer) { window.clearInterval(timer); timer = null; }
            }

            function goTo(index) {
                stop();
                show(index);
                start();
            }

            if (prev) {
                prev.addEventListener('click', function () { goTo(current - 1); });
            }
            if (next) {
                next.addEventListener('click', function () { goTo(current + 1); });
            }

            for (var d = 0; d < dots.length; d++) {
                (function (btn) {
                    btn.addEventListener('click', function () {
                        goTo(parseInt(btn.getAttribute('data-index'), 10));
                    });
                })(dots[d]);
            }

            // Pause while the member is reading or tabbing through the banner.
            root.addEventListener('mouseenter', stop);
            root.addEventListener('mouseleave', start);
            root.addEventListener('focus', stop, true);
            root.addEventListener('blur', start, true);

            root.addEventListener('keydown', function (e) {
                var key = e.which || e.keyCode;
                if (key === 37) { goTo(current - 1); }
                if (key === 39) { goTo(current + 1); }
            });

            show(0);
            start();
        })();
    </script>

</asp:Panel>
