<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Res_Home.ascx.cs" Inherits="EKO_Res_Home" %>

<div id="resSearchResults">
    <div class="contained-width">
        <asp:Panel runat="server" ID="pnlResources">
            <script>
            $(document).ready(function () {

                $("#result-items input[type='submit']").click(function () {
                    $("#<%=hfDownloadId.ClientID%>").val($(this).attr("id"));
                });
            });
            </script>
            <asp:Literal runat="server" ID="litLatestRes"></asp:Literal>
            <div class="div-res-content" id="result-items">
                <asp:HiddenField runat="server" ID="hfDownloadId" />    
                <asp:Repeater runat="server" ID="repeaterResources" OnItemDataBound="repeaterResources_ItemDataBound">
                    <ItemTemplate>
                        <asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </asp:Panel>
    </div>
</div>