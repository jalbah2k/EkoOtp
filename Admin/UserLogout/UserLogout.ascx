<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserLogout.ascx.cs" Inherits="Admin_UserLogout_UserLogout" %>
<style type="text/css">
    @font-face{ 
      font-family: 'WebSymbolsLigaRegular';
          src: url('/fonts/WebSymbolsLigaRegular.eot');                                     /* IE9 Compat Modes */
          src: url('/fonts/WebSymbolsLigaRegular.eot?#iefix') format('embedded-opentype'),  /* IE6-IE8 */
               url('/fonts/WebSymbolsLigaRegular.woff') format('woff'),                     /* Pretty Modern Browsers */
               url('/fonts/WebSymbolsLigaRegular.ttf') format('truetype'),                  /* Safari, Android, iOS */
               url('/fonts/WebSymbolsLigaRegular.svg#WebSymbolsRegular') format('svg');     /* Legacy iOS */
    }
    .admin-user-logout-wrapper
    {
        position: absolute;
        right: 50px;
        top: 61px;
        z-index: 2;
    }
    .admin-username
    {
        color: #434343;
	    font-family: 'Lato', sans-serif;
        font-size: 14px;
        font-weight: 700;
        padding-left: 22px;
        padding-right: 5px;
        background: url(/images/lemonaid/menuicons/users.png) no-repeat left center;
    }
    .admin-user-logout
    {
        color:#0078d8;
        font-family: 'WebSymbolsLigaRegular', 'Open Sans', "Raleway", "HelveticaNeue", "Helvetica Neue", Helvetica, Arial, sans-serif;
        font-size: 16px;
        font-weight: normal;
        line-height: 16px;
        text-align: center;
        text-decoration: none;
    }
</style>
<div class="admin-user-logout-wrapper"><asp:Label ID="lblUser" runat="server" CssClass="admin-username" /><a href="/logout.aspx" title="Logout" class="admin-user-logout">&#0096;</a></div>
