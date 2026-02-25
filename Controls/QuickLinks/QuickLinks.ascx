<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuickLinks.ascx.cs" Inherits="QuickLinks" %>
<link href="/Controls/QuickLinks/quicklinks.css?v=2" rel="stylesheet" />
<asp:Literal runat="server" ID="litStyles"></asp:Literal>

<div role="navigation" class="quickWrapper QuickLinkIcons">
    <asp:Repeater runat="server" ID="Repeater1" OnItemDataBound="Repeater1_ItemDataBound">
        <ItemTemplate><asp:Literal runat="server" ID="litLink"></asp:Literal></ItemTemplate>
    </asp:Repeater>
    <asp:Literal runat="server" ID="litDummy"></asp:Literal>
</div>