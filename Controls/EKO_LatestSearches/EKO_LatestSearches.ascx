<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_LatestSearches.ascx.cs" Inherits="EKO_LatestSearches" %>
<%@ Reference Page="~/Default.aspx"  %>
<script>
    var category = <%= Session["LoggedInID"].ToString()%>;
    var records = <%=records%>;

</script>


<h1>My Latest Searches</h1>
<div>
<asp:Repeater runat="server" ID="repSearches" OnItemDataBound="repSearches_ItemDataBound">
        <ItemTemplate>
            <asp:Literal runat="server" ID="litItem"></asp:Literal>
        </ItemTemplate>
    </asp:Repeater>
</div>

 <%if (bLoadMore)
                { %>
            
            <div class="div_load_more TemplateGrid"><div class="grid3col contained-width"></div></div>
            <span id="span_load_more" class="load-more">load more</span><br />
            <%} %>


