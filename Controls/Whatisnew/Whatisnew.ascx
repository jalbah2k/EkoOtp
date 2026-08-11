<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Whatisnew.ascx.cs" Inherits="Whatisnew" %>
<%@ Reference Page="~/Default.aspx" %>

<link rel="stylesheet" href="/Controls/Whatisnew/Whatisnew.css" />

<script>
    var records = <%= records %>;
</script>

<div class="whats-new">
    <h2>What's New Since Your Last Visit</h2>

    <div class="whats-new-list Whats-New-0">
        <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
            <ItemTemplate>
                <div class="whats-new-row">
                    <span class="whats-new-type">
                        <asp:Literal ID="litType" runat="server" />
                    </span>
                    <span class="whats-new-title">
                        <asp:Literal ID="litTitle" runat="server" />
                    </span>
                    <span class="whats-new-date">
                        <asp:Literal ID="litDate" runat="server" />
                    </span>
                    <a id="theLink" runat="server" class="whats-new-action">
                        <asp:Literal ID="litAction" runat="server" />
                    </a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <% if (bLoadMore)
       { %>
    <center><span id="span_load_more_whatisnew" class="load-more">load more</span></center><br />
    <% } %>
</div>
