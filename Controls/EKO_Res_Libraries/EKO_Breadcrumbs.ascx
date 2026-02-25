<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Breadcrumbs.ascx.cs" Inherits="Breadcrumbs" %>
<asp:Panel runat="server" DefaultButton="btnSearchRes">
<div id="resLibCatBread" class="bread-filter contained-width">
    <div><asp:Literal runat="server" ID="litContent"></asp:Literal></div>
    <div><label for="<%=txtSearch.ClientID %>">Search</label><br /><asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>
        <asp:Button runat="server" ID="btnSearchRes" Text="Go" OnClick="btnSearchRes_Click" />     
    </div>
</div>
</asp:Panel>