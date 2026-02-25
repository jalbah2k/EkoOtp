<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Favorites.ascx.cs" Inherits="EKO_Favorites" %>
<%@ Register Src="~/Controls/EKO_Res_Libraries/EKO_Breadcrumbs.ascx" TagPrefix="uc1" TagName="EKO_Breadcrumbs" %>

<script>
    $(document).ready(function () {

        $("#result-items-rows input[type='submit']").click(function () {
            $("#<%=hfDownloadId.ClientID%>").val($(this).attr("id"));
        });
    });
</script>

<uc1:EKO_Breadcrumbs runat="server" ID="EKO_Breadcrumbs1" />
<div id="div-plHeader">
<asp:PlaceHolder runat="server" ID="plHeader"></asp:PlaceHolder>
</div>

<div class="div-res-content" id="result-items-rows">
    <asp:HiddenField runat="server" ID="hfDownloadId" />    

    <asp:Repeater runat="server" ID="repeaterResources" OnItemDataBound="repeaterResources_ItemDataBound">
        <ItemTemplate>
            <asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>
        </ItemTemplate>
    </asp:Repeater>
</div>