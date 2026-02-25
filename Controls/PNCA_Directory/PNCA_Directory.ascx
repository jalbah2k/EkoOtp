<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PNCA_Directory.ascx.cs" Inherits="PNCA_Directory" %>
<%@ Register Src="~/Controls/PNCA_Directory/PNCA_Filters.ascx" TagPrefix="uc1" TagName="PNCA_Filters" %>
<h1>PNCA Member Directory</h1>
<uc1:PNCA_Filters runat="server" ID="PNCA_Filters" />

<div id="div-plHeader">
    <asp:PlaceHolder runat="server" ID="plHeader"></asp:PlaceHolder>
</div>

    <div id="dirPageWrap" CssClass="repDirectory contained-width">
        <asp:Panel runat="server" ID="pnlList">
            <asp:Repeater runat="server" ID="repDirectory" OnItemDataBound="repDirectory_ItemDataBound" >
                <ItemTemplate> 
                    <asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>                    
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
    </div>
