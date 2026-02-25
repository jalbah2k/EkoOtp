<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SubAccordion.ascx.cs" Inherits="SubAccordion" %>

   <asp:Literal runat="server" ID="litAnchor"></asp:Literal>
<div class="sub-accordion-wrapper">
  <%--  <asp:Literal runat="server" ID="litTitle"></asp:Literal>--%>
    <div class="sub-accordion">
        <div class="sub-accordion-section" >
            <asp:Repeater runat="server" ID="rep1" OnItemDataBound="rep1_ItemDataBound">
                <ItemTemplate>
                    <div>
                        <a class="offAnchor" name="<%# "sub-accordion-" + Parameters + "-" + Eval("n").ToString() %>"   ></a>
                        <a class="sub-accordion-section-title" href="<%# "#sub-accordion-" + Parameters + "-" + Eval("n").ToString() %>" ><h4><%#Eval("Title") %></h4></a>
                        <div id="<%# "sub-accordion-" + Parameters + "-" + Eval("n").ToString() %>B" class="sub-accordion-section-content"  >
                            <asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>