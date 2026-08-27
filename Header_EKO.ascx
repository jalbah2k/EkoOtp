<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header_EKO.ascx.cs" Inherits="Header_EKO" %>
<%@ Reference Page="~/Default.aspx"  %>
<div id="greenBar"></div>
<style>
body.TYPE_PNCA #ekoPncaTips, body.TYPE_PNCA #ekoTips, body.TYPE_EKO #ekoPncaTips, body.TYPE_EKO #pncaTips, body.TYPE_BOTH #ekoTips, body.TYPE_BOTH #pncaTips
{
    display: none;
}

#headerSocial, #mobileSocial
{
    display: inline-flex;
    align-items: center;
    gap: 10px;
    margin: 0 6px;
    vertical-align: middle;
}

#headerSocial .header-icon, #mobileSocial .header-icon
{
    display: inline-flex;
    align-items: center;
    justify-content: center;
    width: 26px;
    height: 26px;
    border-radius: 50%;
    background: #6b3fa0;
    color: #fff;
    text-decoration: none;
}

#headerSocial .header-icon svg, #mobileSocial .header-icon svg
{
    width: 14px;
    height: 14px;
    fill: none;
    stroke: currentColor;
    stroke-width: 2;
    stroke-linecap: round;
    stroke-linejoin: round;
}

#headerSocial .header-icon:hover, #mobileSocial .header-icon:hover
{
    background: #5a3486;
}

#headerSocial .header-icon-bell,
#mobileSocial .header-icon-bell
{
    position: relative;
}

.header-notification-dot
{
    position: absolute;
    top: 1px;
    right: 1px;
    width: 7px;
    height: 7px;
    border: 2px solid #6b3fa0;
    border-radius: 50%;
    background: #c45de8;
    box-sizing: content-box;
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
            <%
            }else{ %>
            <asp:Literal runat="server" ID="litTopMenu"></asp:Literal>
            <div id="headerSocial">
                <a href="/Membership/MyMessages" class="header-icon header-icon-bell" title="My Messages" aria-label="My Messages">
                    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M18 8a6 6 0 1 0-12 0c0 7-3 9-3 9h18s-3-2-3-9"/><path d="M13.73 21a2 2 0 0 1-3.46 0"/></svg>
                    <% if (HasUnreadMessages) { %>
                        <span class="header-notification-dot" aria-hidden="true"></span>
                    <% } %>
                </a>
                <a href="/Membership/MyAccount" class="header-icon header-icon-user" title="My Account" aria-label="My Account">
                    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
                </a>
                <%--<a href="/Membership/MyAccount" class="header-icon header-icon-settings" title="Settings" aria-label="Settings">
                    <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><circle cx="12" cy="12" r="3"/><path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 1 1-2.83 2.83l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-4 0v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 1 1-2.83-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1 0-4h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 1 1 2.83-2.83l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 4 0v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 1 1 2.83 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 0 4h-.09a1.65 1.65 0 0 0-1.51 1z"/></svg>
                </a>--%>
            </div>
            <%--<a href="javascript:LogOutClick()" class="toplinks">
                <i class="fa fa-sign-out-alt fa-fw"></i>Logout
            </a>--%>
            <%} %>
        </div>
        
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


<div id="mainMenu" aria-label="main menu">
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
            <a href='/welcomebod' class='toplinks'>Board of Directors</a><br />
            <a href='/mleadershipcouncil' class='toplinks'>Leadership Council</a><br />
            <a href="javascript:LogOutClick()" class="toplinks">
                <%--<i class="fa fa-sign-out-alt fa-fw"></i>--%>Logout
            </a>
            <%} %>
        <% if (Session["LoggedInID"] != null)
            { %>
        <div id="mobileSocial">
            <a href="/Membership/MyMessages" class="header-icon header-icon-bell" title="My Messages" aria-label="My Messages">
                <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M18 8a6 6 0 1 0-12 0c0 7-3 9-3 9h18s-3-2-3-9"/><path d="M13.73 21a2 2 0 0 1-3.46 0"/></svg>
                <% if (HasUnreadMessages) { %>
                    <span class="header-notification-dot" aria-hidden="true"></span>
                <% } %>
            </a>
            <a href="/Membership/MyAccount" class="header-icon header-icon-user" title="My Account" aria-label="My Account">
                <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
            </a>
            <a href="/Membership/MyAccount" class="header-icon header-icon-settings" title="Settings" aria-label="Settings">
                <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><circle cx="12" cy="12" r="3"/><path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 1 1-2.83 2.83l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-4 0v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 1 1-2.83-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1 0-4h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 1 1 2.83-2.83l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 4 0v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 1 1 2.83 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 0 4h-.09a1.65 1.65 0 0 0-1.51 1z"/></svg>
            </a>
        </div>
        <%} %>
    </div>
</div>
<a name="content"></a>
