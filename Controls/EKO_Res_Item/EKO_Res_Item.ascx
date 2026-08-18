<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Res_Item.ascx.cs" Inherits="EKO_Res_Item" %>
<%@ Reference Page="~/Default.aspx"  %>
<%@ Register Src="~/Controls/EKO_Res_Libraries/EKO_Breadcrumbs.ascx" TagPrefix="uc1" TagName="EKO_Breadcrumbs" %>


<uc1:EKO_Breadcrumbs runat="server" ID="EKO_Breadcrumbs1" Visible="false" />
<div id="singleDetails" class="contained-width res-details">
    <script>
        document.addEventListener("contextmenu", function (e) {
            if (e.target && e.target.closest && e.target.closest("#adobe-dc-view"))
                e.preventDefault();
        });

        document.addEventListener("keydown", (e) => {
            if (e.ctrlKey && (e.key === 'p' || e.key === 's')) {
                e.preventDefault();
                alert("Printing and saving are disabled.");
            }
        });

        document.addEventListener("click", function (e) {
            var link = e.target && e.target.closest ? e.target.closest(".res-actions a.new-tab, .res-actions a.res-btn-primary") : null;
            if (!link) return;
            var url = link.getAttribute("href");
            if (!url || url === "#") return;
            e.stopPropagation();
            var win = window.open(url, "_blank");
            if (win) {
                e.preventDefault();
                try { win.opener = null; } catch (ex) { }
            }
        }, true);
    </script>

    <div class="res-details-layout">
        <div class="res-details-main">
            <asp:PlaceHolder runat="server" ID="plHeader"></asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="plMeta"></asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="plBody"></asp:PlaceHolder>

            <div class="res-actions">
                <asp:HyperLink runat="server" ID="btn_newtab" CssClass="button new-tab res-btn-primary" Target="_blank" Visible="false" Text="Open in a new tab"></asp:HyperLink>
                <asp:Button runat="server" ID="btnDownload" OnClick="btnDownload_Click" Text="Download resource" CssClass="button download res-btn-secondary" UseSubmitBehavior="false" />
                <asp:HyperLink runat="server" ID="hlkView" CssClass="button link res-btn-secondary" Visible="false" />

                <%--<asp:HiddenField runat="server" ID="hfFavourite" />
                <asp:Button runat="server" ID="btnFavourite" OnClick="btnFavourite_Click" Text="favourite resource" />--%>

                <button runat="server" ID="btnFavourite" type="button" class="favBtn button res-btn-secondary">Favourite resource</button>
            </div>

            <%if (IsPdf)
                {%>

            <div id="adobe-dc-view" title="File preview"></div>
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

        <aside class="res-sidebar" aria-label="Library and related resources">
            <asp:PlaceHolder runat="server" ID="plLibrary"></asp:PlaceHolder>

            <asp:Panel runat="server" ID="pnlAssociated" CssClass="res-card res-related-card" Visible="false">
                <script>
                $(document).ready(function () {

                    $("#result-items input[type='submit']").click(function () {
                        $("#<%=hfDownloadId.ClientID%>").val($(this).attr("id"));
                    });
                });
                </script>
                <asp:Literal runat="server" ID="litAssociated"></asp:Literal>
                <asp:HiddenField runat="server" ID="hfDownloadId" />
                <asp:Repeater runat="server" ID="repeaterResources" OnItemDataBound="repeaterResources_ItemDataBound">
                    <HeaderTemplate>
                        <ul class="res-related-list">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
            </asp:Panel>
        </aside>
    </div>
</div>
