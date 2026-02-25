<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchResultsForum.ascx.cs" Inherits="SearchResultsForum" %>
<%@ Reference Page="~/Default.aspx"  %>

<%if (!_partial)
    { %>
<h1> Results </h1>
<asp:Literal runat="server" id="litSubtitle"></asp:Literal>
<%} %>
<asp:Literal runat="server" ID="litTitle"></asp:Literal>

<div id="TotalForumRecords" class="mt-3"></div>

<input name="txtSearchStringFromWho" type="text" id="txtSearchStringFromWho" class="form-control searchUserInput hide" data-display="False" />
<input name="SearchStringTag" type="text" id="SearchStringTag" class="form-control searchTagInput hide" />
<input name="SearchWhat" id="SearchWhat" class="form-control searchWhat hide" value="0" />
<input name="TitleOnly" id="TitleOnly" class="form-control titleOnly hide" value="0" />
<input name="listForum" id="listForum" class="form-control searchForum hide" value="0" />
<input name="listResInPage" id="listResInPage" class="form-control resultsPage hide" value="<%=_records %>" />
<input name="IsPartial" id="IsPartial" class="form-control IsPartial hide" value="0"  data-display="<%= _partial ? "true" : "false" %>" />
<input name="ViewAllUrl" id="ViewAllUrl" class="form-control ViewAllUrl hide" value="forumresults?q=" />
 <div id="SearchResultsPlaceholder"
         data-url="/Membership/"
         data-minimum="4"
         data-notext="&lt;h3>No results&lt;/h3>"
         data-posted="Posted"
         data-by="by"
         data-lastpost="Got to the newest Posting in the topic'"
         data-topic="View topic"
         style="clear: both;">
    </div>
    <%--<div id="SearchResultsPagerBottom" class="mt-3"></div>--%>