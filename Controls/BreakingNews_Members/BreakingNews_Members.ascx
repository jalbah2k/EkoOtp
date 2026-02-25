<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BreakingNews_Members.ascx.cs" Inherits="BreakingNews" %>
<%@ Reference Page="~/Default.aspx"  %>

<script>
//$( document ).ready(function() {
//    $("a.open_new_tab").click(function () {
//        window.open($(this).attr("filename"), null, 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, resizable');
//        return false;
//    });
//});
    var category = <%= category%>;
    var records = <%=records%>;
    var publish = <%=publish%>;

</script>
<div class="newsWrap breaking-news">
   
        <%if (ShowHeader)
            { %>
        <h1>Member News</h1>
        <h2>News & Events</h2>
        <%} %>
         <div class="all-news">
            <asp:HyperLink runat="server" ID="lnkAllNews" Text="view all"></asp:HyperLink>
        </div>
        <div id="Breaking-New-<%=categoryid %>" class="Breaking-News-0">        
            <asp:Repeater ID="Repeater1" runat="server" onitemdatabound="Repeater1_ItemDataBound">
            <ItemTemplate>
                <a id="theLink" runat="server" class="three jnewssc">

                    <div class="div_table news_description">
                        <div class="div_cell image_news">
                            <asp:Image runat="server" ID="imgPhoto" AlternateText='<%# Eval("PhotoAltText") %>' GenerateEmptyAlternateText="True" />
                        </div>
                        <div class="div_cell home-news-desc">
                            <h3><asp:Literal ID="litDate" runat="server"></asp:Literal></h3><br />
                            <h4><asp:Literal ID="litTitle" runat="server"></asp:Literal></h4>
                        </div>                      
                    </div>
                </a>
            </ItemTemplate>
            </asp:Repeater>

        </div>
        <%if (bLoadMore)
        { %>
            
        <center><span id="span_load_more" class="load-more">load more news</span></center><br />
        <%-- <span class="load-less">show less new</span>--%>
        <%} %>
</div>


