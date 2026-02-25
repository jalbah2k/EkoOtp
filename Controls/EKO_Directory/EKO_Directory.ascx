<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Directory.ascx.cs" Inherits="EKO_Directory" %>
<div id="dirPageWrap">
    <asp:Panel runat="server" ID="pnlList" CssClass="repDirectory">
        <h2>Empowered Kids Ontario General Members</h2><br />
        <asp:Repeater runat="server" ID="repDirectory" OnItemDataBound="repDirectory_ItemDataBound" >
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hlkOrg">
                    <div class="dirTopWrapList">
                        <div>
                            <div>
                                <asp:Literal runat="server" ID="litLogo"></asp:Literal>
                            </div>
                            <div class="dirListDetails">
                                <asp:Literal runat="server" ID="litContent"></asp:Literal>
                             </div>
                         </div>
                    </div>
                </asp:HyperLink>
            </ItemTemplate>

        </asp:Repeater>

        <asp:Panel runat="server" ID="pnlAffiliate" CssClass="repDirectory">
            <h2>Empowered Kids Ontario Associate Members</h2><br />
            <asp:Repeater runat="server" ID="repAffiliate" OnItemDataBound="repDirectory_ItemDataBound" >
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="hlkOrg">
                        <div class="dirTopWrapList">
                            <div>
                                <div>
                                    <asp:Literal runat="server" ID="litLogo"></asp:Literal>
                                </div>
                                <div class="dirListDetails">
                                    <asp:Literal runat="server" ID="litContent"></asp:Literal>
                                 </div>
                             </div>
                        </div>
                    </asp:HyperLink>
                </ItemTemplate>

            </asp:Repeater>
        </asp:Panel>

    </asp:Panel>    
</div>
<asp:Panel runat="server" ID="pnlDetails" Visible="false">
<!--     <asp:HyperLink runat="server" ID="hlkViewAll" Text="View All" ></asp:HyperLink> -->
    <asp:Literal runat="server" ID="litTitle"></asp:Literal>
    <asp:Literal runat="server" ID="litLogo"></asp:Literal>
    <asp:Literal runat="server" ID="litContent"></asp:Literal>

</asp:Panel>