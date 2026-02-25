<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PNCA_Res_Libraries.ascx.cs" Inherits="PNCA_Res_Libraries" %>
<%@ Register Src="~/Controls/PNCA_Res_Libraries/PNCA_Breadcrumbs.ascx" TagPrefix="uc1" TagName="PNCA_Breadcrumbs" %>

<uc1:PNCA_Breadcrumbs runat="server" ID="EKO_Breadcrumbs1" />
<div id="resLibCatSelect">

    <asp:Panel runat="server" ID="pnlLibrary" Visible="false">
        <asp:Panel runat="server" Visible="false">
             <div class="div-res-content contained-width">
                <asp:Literal runat="server" ID="litEKOTitle"></asp:Literal>
            </div>
            <div class="div-res-content contained-width">
            
                <asp:PlaceHolder runat="server" ID="plMy"></asp:PlaceHolder>

                <asp:Repeater runat="server" ID="repeaterLibrary" OnItemDataBound="repeaterLibrary_ItemDataBound">
                    <ItemTemplate>
                        <asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>
                    </ItemTemplate>
                </asp:Repeater>
       

            </div>
            <br /><hr class="contained-width" /><br />
        </asp:Panel>

        <div class="div-res-content contained-width">
            <asp:Literal runat="server" ID="litPNCATitle"></asp:Literal>
        </div>

        <div class="div-res-content contained-width">

             <asp:Repeater runat="server" ID="repeaterLibrary_PNCA" OnItemDataBound="repeaterLibrary_ItemDataBound">
                <ItemTemplate>
                    <asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>
                </ItemTemplate>
            </asp:Repeater>


        </div>
    </asp:Panel>



    <asp:Panel runat="server" ID="pnlCategory" Visible="false">
    <div class="div-res-content contained-width">
            
        <asp:Repeater runat="server" ID="repeaterCategory" OnItemDataBound="repeaterCategory_ItemDataBound">
            <ItemTemplate>
                <asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </asp:Panel>
</div>

