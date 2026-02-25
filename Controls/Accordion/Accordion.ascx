<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Accordion.ascx.cs" Inherits="Accordion" %>
<%@ Reference Page="~/Default.aspx"  %>
<%@ Reference Control="~/Controls/Accordion/SubAccordion.ascx" %>

   <asp:Literal runat="server" ID="litAnchor"></asp:Literal>
<div class="accordion-wrapper">
    <asp:Literal runat="server" ID="litTitle"></asp:Literal>
    <div class="accordion">
        <div class="accordion-section">
            <asp:Repeater runat="server" ID="rep1" OnItemDataBound="rep1_ItemDataBound">
                <ItemTemplate>
                    <div>
                    <a class="offAnchor" name="<%# "accordion-" + Parameters + "-" + Eval("n").ToString() %>"   ></a>
                    <a class="accordion-section-title" href="<%# "#accordion-" + Parameters + "-" + Eval("n").ToString() %>" ><h2><%#Eval("Title") %></h2></a>
                    <div id="<%# "accordion-" + Parameters + "-" + Eval("n").ToString() %>B" class="accordion-section-content"  >
                        <%--<%#Eval("content").ToString() %>--%>
                        <asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>