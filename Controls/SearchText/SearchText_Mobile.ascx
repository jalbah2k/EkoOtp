<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchText.ascx.cs" Inherits="Controls_SearchText_SearchText" %>
<%@ Reference Page="~/Default.aspx"  %>
<%@ Register TagPrefix="Custom" Namespace="ASB" Assembly="AutoSuggestBox" %>
<style>
    .searchPanel button{
        background-color:transparent!important;
        padding:0!important;
        border:none!important;
    }
   
</style>
<asp:Panel ID="Panel1" CssClass="searchPanel" runat="server" DefaultButton="btSearch" role="search">
<div role="presentation">
<div class="row">
    <div style="display:table-cell; vertical-align:middle;width:100%;">
        <label for="<%=tbSearch.ClientID %>" class="nosize">Search:</label>        
        <Custom:AutoSuggestBox id="tbSearch" CssClass="tbSearch searchInput" DataType="Searches" runat="server" ResourcesDir="/asb_includes" ValidationGroup="search" Text="" onfocus="if (this.value==search) this.value='';" onblur="if (this.value.length==0) this.value=search;"/></div>
    <div style="display:table-cell; vertical-align:middle; width: 29px; padding-left: 2px;">
        <button type="button" onclick="javascript: $('#<%=btSearch.ClientID %>').click();"><i class="fa fa-search" aria-hidden="true"></i><span>Search</span></button>
        <div style="display:none;"><asp:ImageButton ID="btSearch" title="search button" ValidationGroup="search" runat="server" OnClick="Search" AlternateText="search button" /></div>
    </div>
</div>
</div>
<script type="text/javascript">    var search = '<%= enSearch %>';</script>
</asp:Panel>

