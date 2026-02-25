<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchNew.ascx.cs" Inherits="SearchNew" %>
<%@ Register Src="~/Controls/SearchResults/SearchResults.ascx" TagPrefix="uc1" TagName="SearchResults" %>

<h1> Results </h1>
<asp:Literal runat="server" id="litSubtitle"></asp:Literal>

<div id="resultsPages" class="search-results-wrapper">
<uc1:SearchResults runat="server" ID="SearchResults_Page" Parameters="5,1,1" ></uc1:SearchResults>
</div>

<div id="resultsDirectory" class="search-results-wrapper">
<uc1:SearchResults runat="server" ID="SearchResults_Directory" Parameters="5,3,1" ></uc1:SearchResults>
</div>

<div id="resultsNews" class="search-results-wrapper">
<uc1:SearchResults runat="server" ID="SearchResults_News" Parameters="5,2,1" ></uc1:SearchResults>
</div>