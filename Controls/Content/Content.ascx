<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Content.ascx.cs" Inherits="Content" %>
<div class="outterContWrap slick" style="position:relative;">
	<div class="innerContWrap">
	    <asp:Literal ID="litContent" runat="server" />
		<asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>
	    <%--<div id="tdEdit" runat="server" style="float:right;"><div style="position:absolute;"><div style="position:relative; top:0px; left:-90px; z-index:2"><asp:Literal ID="litEdit" runat="server"/></div></div></div>--%>
	    <div id="tdEdit" runat="server" style="position:absolute; bottom:0px; right:0px; z-index:2"><asp:Literal ID="litEdit" runat="server"/></div>
	</div>
</div>