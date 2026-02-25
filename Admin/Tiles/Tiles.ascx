<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tiles.ascx.cs" Inherits="Tiles" %>
<%@ Register Src="~/Admin/Tiles/TileImage.ascx" TagPrefix="uc1" TagName="TileImage" %>

<div class="admin-header-wrapper noprint">
    <div class="admin-header">Tiles</div>
    <div class="admin-header-subtitle">Manage Tiles content</div>
</div>
<uc1:TileImage runat="server" ID="TileImage1" Parameter="1" />
