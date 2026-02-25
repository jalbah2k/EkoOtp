<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Resources.ascx.cs" Inherits="EKO_Resources" %>
<%@ Register Src="~/Controls/EKO_Resources/EKO_Filters.ascx" TagPrefix="uc1" TagName="EKO_Filters" %>
<script>
    $(document).ready(function () {

        $("#result-items input[type='submit']").click(function () {
            $("#<%=hfDownloadId.ClientID%>").val($(this).attr("id"));
        });
    });
</script>
<uc1:EKO_Filters runat="server" ID="EKO_Filters" />

<div id="resSearchResults">

    <div class="contained-width">

        <div id="div-plHeader">
        <asp:PlaceHolder runat="server" ID="plHeader"></asp:PlaceHolder>
        </div>

        <div class="div-res-content" id="result-items">
            <asp:HiddenField runat="server" ID="hfDownloadId" />    

            <asp:Repeater runat="server" ID="repeaterResources" OnItemDataBound="repeaterResources_ItemDataBound">
                <ItemTemplate>
                    <asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>
                </ItemTemplate>
            </asp:Repeater>
        </div>

    </div>
    
</div>
