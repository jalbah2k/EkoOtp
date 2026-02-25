<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BlogList.ascx.cs" Inherits="Controls_Blog_BlogList" %>
<link rel="stylesheet" type="text/css" href="/css/accordion.css" />

<script type="text/javascript" src="/js/scriptaculous.js"></script>
<script type="text/javascript" src="/js/accordion.js"></script>

<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
        <div id="test-accordion" class="accordion">
        <asp:Repeater ID="Repeater1" runat="server" 
                onitemdatabound="Repeater1_ItemDataBound">
            <ItemTemplate>        
                <div class="accordion-toggle"><%#Eval ("MyYear") %><hr /></div>
	            <div class="accordion-content">
		            <p>
		                <asp:Repeater ID="Repeater2" runat="server" onitemdatabound="Repeater2_ItemDataBound" OnItemCommand="Repeater2_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton  runat="server" ID="hlkTitle" Text='<%# Eval("Title") %>' CommandName="link" CommandArgument='<%# Eval("id")%>' ToolTip='<%# "/BarbsBlog?id=" + Eval("id")%>'></asp:LinkButton>
                                <%--<asp:HyperLink runat="server" ID="hlkTitle" Text='<%# Eval("Title") %>' NavigateUrl='<%# "~/Default.aspx?id=" + Eval("id")%>'></asp:HyperLink>--%>
                                <hr style="border-bottom-style:dotted;border-top-style:none;" />
                            </ItemTemplate>
                        </asp:Repeater>
		            </p>
	            </div>
            </ItemTemplate>
        </asp:Repeater>
        </div>
<%--    </ContentTemplate>
</asp:UpdatePanel>
--%>
<script type="text/javascript">
    document.observe("dom:loaded", function () {
        accordion = new Accordion("test-accordion", <%=group %>);
    })
</script>


