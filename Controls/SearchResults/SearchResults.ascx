<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchResults.ascx.cs" Inherits="SearchResults" %>

<%if (!_partial)
    { %>
<h1> Results </h1>
<asp:Literal runat="server" id="litSubtitle"></asp:Literal>
<%} %>

<asp:Literal runat="server" ID="litTitle"></asp:Literal>
<asp:Literal runat="server" ID="litMessage"></asp:Literal>
<asp:Literal runat="server" ID="litContent"></asp:Literal>
