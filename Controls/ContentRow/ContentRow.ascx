<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContentRow.ascx.cs" Inherits="ContentRow" %>
<link href="/CSS/row-col.css?v=<%=ConfigurationManager.AppSettings["CSSVersion"] %>" rel="stylesheet" />
<div class="<%=ClassName %>" id="Content-Row-<%=parameter %>">
	<div id="Content-Row-<%=parameter %>-Sub" class="rowSub contained-width <%=SubrowClass %>">
	    <asp:PlaceHolder ID="MainRow" runat="server" />
	</div>
</div>