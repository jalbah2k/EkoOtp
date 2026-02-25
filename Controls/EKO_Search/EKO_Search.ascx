<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Search.ascx.cs" Inherits="EKO_Search" %>
<script>
  
    $(document).ready(function () {

        $('#<%=txtSearch.ClientID%>').keypress(function (e) {

            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                $('#btnSearchRes').click(); return false;
            }
            else
                return true;
        });
    });
</script>

<div class="row-filter" >
    <div id="search-filter"><div><label for="<%=txtSearch.ClientID %>">Find a Resource:</label><asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>
        <button type="button" id="btnSearchRes" onclick="javascript: $('#<%=btSearch.ClientID %>').click();" >Go</button>
        <div style="display:none;"><asp:ImageButton ID="btSearch" title="search button" ValidationGroup="search" runat="server" OnClick="btSearch_Click" AlternateText="search button" /></div>
    </div></div>
    <div id="latestSearches">
        <span>Latest Resource Searches</span>
        <br />
        <asp:Repeater runat="server" ID="repSearches" OnItemDataBound="repSearches_ItemDataBound">
            <ItemTemplate>
                <asp:Literal runat="server" ID="litItem"></asp:Literal>
            </ItemTemplate>
        </asp:Repeater>
        <br />
    </div>
    <a id="viewFullSearchLink" href="/mylatestsearches">View Full History</a>
    <p id="byline">* indicates search results filtered to a library / category</p>
    <a style="display: none;" href="/uploads/EKO-Board-binder-Leadership Council _2022.pdf">test</a>

</div>
