<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Directory_Featured.ascx.cs" Inherits="EKO_Directory_Featured" %>
<%@ Reference Page="~/Default.aspx" %>



<div class="featuredLeft" style="background: url('<%=BackgroundImage%>'); <%=BackgroundPosition%>">
   <%-- <span style="display: none;">

        We need logic to switch from full container width to half if image or none and also switch classes

        <asp:Literal runat="server" ID="litImage" Visible="true"></asp:Literal></span>--%>
</div>
<div class="featuredRight">
    <asp:Literal runat="server" ID="litContent"></asp:Literal>
</div>
