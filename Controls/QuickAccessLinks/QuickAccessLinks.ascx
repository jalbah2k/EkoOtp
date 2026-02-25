<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuickAccessLinks.ascx.cs" Inherits="Controls_QuickAccessLinks_QuickAccessLinks" %>
<script type="text/javascript">
    $(document).ready(function () {
        var QuickAccessLinksTop;
        var SetQuickAccessLinksTop = function () {
            QuickAccessLinksTop = 100;
            /*try {
                QuickAccessLinksTop = parseInt($('#banner').offset().top) + $('#banner').height() + 10;
            }
            catch (err) {*/
                try {
                    QuickAccessLinksTop = parseInt($('#header').offset().top) + $('#header').height() + 10;
                }
                catch (err) { }
            //}
            if ($("#QuickAccessLinks").hasClass('absolute')) {
                $('#QuickAccessLinks').css({ 'top': QuickAccessLinksTop + 'px' });
            }
        }

        $(window).resize(function () {
            SetQuickAccessLinksTop();
            CloseQuickAccessLinks();
        });
        SetQuickAccessLinksTop();
        //if ($.browser.mozilla == true) {
        //if ($.browser.msie == false) {
        setTimeout(SetQuickAccessLinksTop, 2000);
        //}

        var CloseQuickAccessLinks = function () {
            if ($('#QuickAccessLinks').width() > 51 && !$('#QuickAccessLinks').hasClass('closed')) {
                $("#QuickAccessLinks").addClass('closed');
                $("#QuickAccessLinks a").stop().animate({ width: '15px' }, 500);
                $("#QuickAccessLinks").stop().animate({ width: '51px' }, 500);
                //$('#QuickAccessLinks').css({ 'width': '51px' });
            }
        }

        setTimeout(CloseQuickAccessLinks, 3000);

        $(window).scroll(function (e) {
            //var height = $(window).height();

            /*if ($('#QuickAccessLinks').width() > 51 && !$('#QuickAccessLinks').hasClass('closed')) {
                $("#QuickAccessLinks").addClass('closed');
                $("#QuickAccessLinks a").stop().animate({ width: '15px' }, 500);
            }*/
            CloseQuickAccessLinks();

            //if ((height >= 768)) {
            if ($(this).scrollTop() >= QuickAccessLinksTop && !$("#QuickAccessLinks").hasClass('fixed')) {
                $('#QuickAccessLinks').removeClass('absolute').addClass('fixed');
                //if ('<%= IsEditor() %>' == 'True')
                //    $('#QuickAccessLinks').removeClass('absolute').addClass('fixed100');
                //else
                //    $('#QuickAccessLinks').removeClass('absolute').addClass('fixed');
            }
            else if ($(this).scrollTop() < QuickAccessLinksTop && !$("#QuickAccessLinks").hasClass('absolute')) {
                $('#QuickAccessLinks').removeClass('fixed').addClass('absolute');
                $('#QuickAccessLinks').css({ 'top': QuickAccessLinksTop + 'px' });
            }
            //}
        });

        $('#QuickAccessLinks').mouseenter(function () {
            $("#QuickAccessLinks").removeClass('closed');
            $("#QuickAccessLinks a").stop().animate({ width: '240px' }, 500);
            $("#QuickAccessLinks").stop().animate({ width: '276px' }, 500);
            //$('#QuickAccessLinks').css({ 'width': '276px' });
        });
    });
</script>
<div id="QuickAccessLinks" class="absolute">
<%--    <div><a href="#" target="_self" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Download','','/images/rehab/qa-download-hover.png',1)" onblur="MM_swapImgRestore()" onfocus="MM_swapImage('Download','','/images/rehab/qa-download-hover.png',1)"><img src="/images/rehab/qa-download.png" name="Download" border="0" id="Download" alt="download" /></a></div>
    <div><a href="#" target="_self" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Location','','/images/rehab/qa-location-hover.png',1)" onblur="MM_swapImgRestore()" onfocus="MM_swapImage('Location','','/images/rehab/qa-location-hover.png',1)"><img src="/images/rehab/qa-location.png" name="Location" border="0" id="Location" alt="location" /></a></div>
    <div><a href="#" target="_self" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Staff','','/images/rehab/qa-staff-hover.png',1)" onblur="MM_swapImgRestore()" onfocus="MM_swapImage('Staff','','/images/rehab/qa-staff-hover.png',1)"><img src="/images/rehab/qa-staff.png" name="Staff" border="0" id="Staff" alt="Staff" /></a></div>
    <div><a href="#" target="_self" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Feedback','','/images/rehab/qa-feedback-hover.png',1)" onblur="MM_swapImgRestore()" onfocus="MM_swapImage('Feedback','','/images/rehab/qa-feedback-hover.png',1)"><img src="/images/rehab/qa-feedback.png" name="Feedback" border="0" id="Feedback" alt="Feedback" /></a></div>
    <div><a href="#" target="_self" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Contact','','/images/rehab/qa-contact-us-hover.png',1)" onblur="MM_swapImgRestore()" onfocus="MM_swapImage('Contact','','/images/rehab/qa-contact-us-hover.png',1)"><img src="/images/rehab/qa-contact-us.png" name="Contact" border="0" id="Contact" alt="Contact" /></a></div>--%>
    <a class="download" href="/brochuregallery" target="_self">Download Brochure<i class="fa fa-download"></i></a>
    <a class="location" href="/stafflist" target="_self">Contact Your Consultant<i class="fa fa-map-marker"></i></a>
    <a class="referral" href="/ReferralCentre" target="_self">Referral Consultation<i class="fa fa-users"></i></a>
    <a class="feedback" href="/feedback" target="_self">Client Feedback<i class="fa fa-comments"></i></a>
    <a class="contact" href="/contactus" target="_self">Contact Us<i class="fa fa-paper-plane"></i></a>
</div>
