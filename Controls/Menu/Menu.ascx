<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Menu" %>
<%@ Reference Page="~/Default.aspx" %>

<ul id="Menu_<%=Parameters %>" class="<%= CssClass%> sm-blue nav navbar-nav">
    <asp:Literal runat="server" ID="litMenuItems"></asp:Literal>
</ul>
