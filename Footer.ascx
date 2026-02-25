<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.ascx.cs" Inherits="Footer" %>
<%@ Register Src="~/Controls/Content/Content.ascx" TagPrefix="Hardcode" TagName="Content" %>
<%@ Reference Page="~/Default.aspx"  %>

<footer class="full-width">
    <div id="topFooter" class="full-width">
        <div class="contained-width">
            <div id="footerLogo">
                <img src="/Images/logo-footer.png" alt="EKO Logo footer">
                <div id="footerSocial">
                    <a href="https://www.facebook.com/EmpoweredKidsON/" class="facebook" title="EKO Facebook" target="_blank"></a>
                    <a href="https://twitter.com/empoweredkidson?lang=en" class="twitter" title="EKO Twitter" target="_blank"></a>
                    <a href="https://www.linkedin.com/company/empowered-kids-ontario/" class="linkedin" title="EKO Linkedin" target="_blank"></a>
                    <a href="https://www.instagram.com/empoweredkidson/" class="instagram" title="EKO Instagram" target="_blank"></a>
                    <a href="https://bsky.app/profile/empoweredkidson.bsky.social" class="bluesky" title="EKO Bluesky" target="_blank"></a>
                </div>
            </div>
            <%--<div id="">
                <h3>Latest Tweet</h3>
                <span style="display:none;"><iframe id="twitterFrame" src="/twitterapp" scrolling="no" borde="no" frameBorder="0" ></iframe></span>
                <div id="myTwitterNew"></div>
            </div>--%>
            <div>
                <div id="needHelp">
                    <h3>Have a Question?</h3>
                    <p style="color:#fff;">Contact us at:<br><a class="footerEmail" href="mailto:info@empoweredkidsontario.ca">info@empoweredkidsontario.ca</a></p>
                </div>
                <div id="needHelpPNCA">
                    <h3>Have a Question?</h3>
                    <p style="color:#fff;">For PNCA inquiries email Margaret at:<br><a class="footerEmail" href="mailto:margaret@margarethoward.ca">margaret@margarethoward.ca</a></p>
                    
                </div>
            </div>
            <div id="miniLogin">
                <% if (Session["LoggedInID"] == null)
                    { %>
                <iframe src="/membership/Account/MiniLogin" frameBorder="0" scrolling="no" style="overflow:hidden; height:315px;"></iframe>
                <%} else { %>
                <h2>Member Quicklinks</h2>
                <asp:Literal runat="server" ID="litBottomMenu"></asp:Literal>
               
                
                <%} %>
            </div>
        </div>
    </div>
  

    <div id="btmFooter" class="full-width">
        <div class="contained-width">
            <button type="button" class="button" onclick="window.location.href='/support';">Donate now</button>
            <div id="copyrightContent"><Hardcode:Content runat="server" id="FooterContent" Parameters="5585"/></div>
        </div>
    </div>
    
</footer>


