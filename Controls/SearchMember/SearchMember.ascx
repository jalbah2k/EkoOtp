<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchMember.ascx.cs" Inherits="SearchMember" %>
<%@ Register Src="~/Controls/SearchResults/SearchResults.ascx" TagPrefix="uc1" TagName="SearchResults" %>
<%@ Register Src="~/Controls/SearchResultsForum/SearchResultsForum.ascx" TagPrefix="uc1" TagName="SearchResultsForum" %>

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

<div id="resultsResources" class="search-results-wrapper">
<uc1:SearchResults runat="server" ID="SearchResults_Resources" Parameters="5,4,1" ></uc1:SearchResults>
</div>

<div id="resultsForum" class="search-results-wrapper">
<uc1:SearchResultsForum runat="server" ID="SearchResultsForum" Parameters="5,1"/>
</div>
