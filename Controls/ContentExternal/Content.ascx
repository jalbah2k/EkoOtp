<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Content.ascx.cs" Inherits="Content" %>
<div style="position:relative;">
    <asp:Literal ID="litContent" runat="server" />
    <%--<div id="tdEdit" runat="server" style="float:right;"><div style="position:absolute;"><div style="position:relative; top:0px; left:-90px; z-index:2"><asp:Literal ID="litEdit" runat="server"/></div></div></div>--%>
    <div id="tdEdit" runat="server" style="position:absolute; bottom:-32px; right:0px; z-index:2"><asp:Literal ID="litEdit" runat="server"/></div>
</div>