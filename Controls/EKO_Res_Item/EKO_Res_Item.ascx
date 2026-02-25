<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Res_Item.ascx.cs" Inherits="EKO_Res_Item" %>
<%@ Reference Page="~/Default.aspx"  %>
<%@ Register Src="~/Controls/EKO_Res_Libraries/EKO_Breadcrumbs.ascx" TagPrefix="uc1" TagName="EKO_Breadcrumbs" %>


<uc1:EKO_Breadcrumbs runat="server" ID="EKO_Breadcrumbs1" />
<div id="singleDetails" class="contained-width">
    <script>
        // Disable right-click
        document.addEventListener("contextmenu", (e) => e.preventDefault());

        // Disable keyboard shortcuts (e.g., Ctrl+P, Ctrl+S)
        document.addEventListener("keydown", (e) => {
            if (e.ctrlKey && (e.key === 'p' || e.key === 's')) {
                e.preventDefault();
                alert("Printing and saving are disabled.");
            }
        });
    </script>


    <asp:PlaceHolder runat="server" ID="plBody"></asp:PlaceHolder>
    <a runat="server" id="btn_newtab" class="button new-tab" style="font-size:16px;" visible="false">view in a new tab</a>
    <asp:Button runat="server" ID="btnDownload" OnClick="btnDownload_Click" Text="download resource" CssClass="button download" />
    <asp:HyperLink runat="server" ID="hlkView" CssClass="button link" Visible="false" />

    <%--<asp:HiddenField runat="server" ID="hfFavourite" />
    <asp:Button runat="server" ID="btnFavourite" OnClick="btnFavourite_Click" Text="favourite resource" />--%>

    <button runat="server" ID="btnFavourite" type="button" >favourite resource</button>

    <%if (IsPdf)
        {%>

    <div id="adobe-dc-view"></div>
    <%}
        else if (IsVideo)
        { %>
    <asp:Literal runat="server" ID="litVideo"></asp:Literal>
    <%}
        else if (IsImage)
        {%>
            <asp:Image runat="server" ID="imgPhoto" GenerateEmptyAlternateText="True" />
        
    <%} %>
</div>





<div id="resSearchResults">
    <div class="contained-width">
        <asp:Panel runat="server" ID="pnlAssociated">
            <script>
            $(document).ready(function () {

                $("#result-items input[type='submit']").click(function () {
                    $("#<%=hfDownloadId.ClientID%>").val($(this).attr("id"));
                });
            });
            </script>
            <asp:Literal runat="server" ID="litAssociated"></asp:Literal>
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
