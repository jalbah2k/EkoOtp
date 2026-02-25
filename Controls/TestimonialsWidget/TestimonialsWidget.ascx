<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TestimonialsWidget.ascx.cs" Inherits="TestimonialsWidget" %>
<div class="testimonials-2-columns">
 <asp:Repeater runat="server" ID="repItems" OnItemDataBound="repItems_ItemDataBound">
    <ItemTemplate>
        <div><asp:Image runat="server" ID="imgPhoto" /></div>
        <div><asp:Literal runat="server" ID="litContent"></asp:Literal></div>
    </ItemTemplate>
</asp:Repeater>
</div>