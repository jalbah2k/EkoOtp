<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Sitemaps.ascx.cs" Inherits="Controls_Sitemaps_Sitemaps" %>
<%@ Reference Control="~/Controls/Sitemap/Sitemap.ascx" %>
<style type="text/css">
#sitemap-wrapper
{
    position: relative;
}
#sitemap-wrapper ul:not(:empty)
{
    margin-left: 10px;
}
</style>
<div id="sitemap-wrapper"><asp:PlaceHolder runat="server" ID="placeHolder"></asp:PlaceHolder></div>