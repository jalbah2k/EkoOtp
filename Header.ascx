<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Header" %>
<%@ Reference Page="~/Default.aspx"  %>
<%@ Register tagprefix="Hardcode" src="Controls/TextSize/TextSize.ascx" tagname="TextSize" %>
<%@ Register tagprefix="Hardcode" src="Controls/SearchText/SearchText.ascx" tagname="SearchText" %>
<%@ Register Src="~/Controls/SearchText/SearchText_Mobile.ascx" TagPrefix="Hardcode" TagName="SearchText_Mobile" %>

<%--<%@ Register Src="~/Controls/LangSelect/Selector.ascx" TagPrefix="Hardcode" TagName="Selector" %>--%>
<div id="greenBar"></div>
<style>
body.TYPE_PNCA #ekoPncaTips, body.TYPE_PNCA #ekoTips, body.TYPE_EKO #ekoPncaTips, body.TYPE_EKO #pncaTips, body.TYPE_BOTH #ekoTips, body.TYPE_BOTH #pncaTips
{
    display: none;
}
</style>
<header class="contained-width" aria-label="header">
    <div id="logo">
        <a href="/<%=_language %>home"><img src="/images/logo.jpg" alt="Placeholder Logo" class="img-logo" style="width: 300px; height:auto;" /></a>
    </div>
    <div id="utility">
        <div id="utilityLinks">
            <% if(Session["LoggedInID"]==null){ %>
            <a href="/Membership/Account/Login" class="toplinks" title="Log in to our Members" >member login</a>
            <%--<a href="/Membership/Account/Register" class="toplinks" title="Registration" >become a member</a>--%>
            <%
            }else{ %>
            <asp:Literal runat="server" ID="litTopMenu"></asp:Literal>
            <a href="javascript:LogOutClick()" class="toplinks">
                <i class="fa fa-sign-out-alt fa-fw"></i>Logout
            </a>
            <%} %>
            <%--<Hardcode:Selector runat="server" ID="Selector" />--%>
            <div id="headerSocial">
                <a href="https://www.facebook.com/EmpoweredKidsON/" class="facebook" title="EKO Facebook" target="_blank"></a>
                <a href="https://twitter.com/empoweredkidson?lang=en" class="twitter" title="EKO Twitter" target="_blank"></a>
                <a href="https://www.linkedin.com/company/empowered-kids-ontario/" class="linkedin" title="EKO Linkedin" target="_blank"></a>
                <a href="https://www.instagram.com/empoweredkidson/" class="instagram" title="EKO Instagram" target="_blank"></a>
                <a href="https://bsky.app/profile/empoweredkidson.bsky.social" class="bluesky" title="EKO Bluesky" target="_blank"></a>
            </div>
        </div>
        <Hardcode:SearchText ID="SearchText1" runat="server" />
        <Hardcode:TextSize ID="TextSize1" runat="server" />
        
        
    </div>
    <div id="mobileSearchToggle"></div>
    <div id="mobileMenu"> 
        <div id="nav-icon">
          <span></span>
          <span></span>
          <span></span>
          <span></span>
        </div>
    </div>
</header>
<div id="greenBarMobile"></div>
<!-- header -->


<div id="mainMenu" aria-label="main menu">
    <div id="mobileSearch">
        <Hardcode:SearchText_Mobile runat="server" ID="SearchText_Mobile" />
    </div> 
    <nav class="contained-width">
        <div class="mainmenu menu-horizontal">
            <asp:PlaceHolder ID="MainMenu" runat="server" />
        </div>
    </nav>
    <div id="mobileSubMenu">
            <% if (Session["LoggedInID"] == null)
                { %>

        <a id="mobileMenuLogin" href="/Membership/Account/Login" class="button1"><span style="text-transform: uppercase">EKO</span> Member Login</a>
        <p>Don’t have an account with EKO? <a id="mobileMenuSignup" href="/Membership/Account/Register">Request access</a> </p>
        <%
            }else{ %>
            <a href='/EKOMembers' class='toplinks'>My Dashboard</a><br />
                                   <%-- <a href='/Membership/MyMessages' class='toplinks'>Inbox</a><br />
                                    <a href='/Membership/MyAccount' class='toplinks'>Account</a><br />--%>
            <a href='/welcomebod' class='toplinks'>Board of Directors</a><br />
            <a href='/mleadershipcouncil' class='toplinks'>Leadership Council</a><br />
            <a href="javascript:LogOutClick()" class="toplinks">
                <%--<i class="fa fa-sign-out-alt fa-fw"></i>--%>Logout
            </a>
            <%} %>
        <div id="mobileSocial">
            <a href="https://www.facebook.com/EmpoweredKidsON/" class="facebook" title="EKO Facebook" target="_blank"></a>
            <a href="https://twitter.com/empoweredkidson?lang=en" class="twitter" title="EKO Twitter" target="_blank"></a>
            <a href="https://www.linkedin.com/in/eko-canada-8189b1108/" class="linkedin" title="EKO Linkedin" target="_blank"></a>
        </div>
    </div>
</div><!-- mainmenu -->
<a name="content"></a>