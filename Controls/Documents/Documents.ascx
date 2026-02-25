<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Documents.ascx.cs" Inherits="Admin_RelatedLinks_RelatedLinks" %>
<asp:Panel ID="pnlDocument" runat="server">
    <style type="text/css">
        .DocumentsWrapper
        {
            position:relative;
            float:left;
            /*padding-bottom:50px;*/
        }
        .DocumentsWrapper .btnDocAdd
        {
            position:absolute;
            bottom:-10px;
            right:-20px;
            z-index:100;
        }
        br.clear
        {
            display:block;
            clear:both;
        }
        
    </style>
    <div class="DocumentsWrapper" style="position:relative;">
    <div id="btnDocAdd" class="btnDocAdd" runat="server" visible="false"><asp:Literal ID="litBtnAddDoc" runat="server"></asp:Literal></div>
    <div style="padding-left:0px; padding-top:15px; width: 100%;display:table" id="thetable" runat="server" class="bodytext" role="presentation">
        <div class="row">
            <div><h2><asp:Literal ID="listname" runat="server" /></h2></div>
        </div>
        <div class="row">
            <div>
                <ul>
                <asp:DataList ID="GV_Main" runat="server" AutoGenerateColumns="False" RepeatLayout="Flow"
                    DataKeyNames="id" GridLines="None" AllowPaging="false" CellSpacing="-1" OnItemDataBound="GV_Main_ItemDataBound" OnDataBound="GV_Main_DataBound" ShowHeader="False" role="presentation">
                    
                            <ItemTemplate><asp:Image ID="imgStatus" runat="server" ImageUrl="/images/icons/types/doc.png" AlternateText='<%# Eval("name") %>' ToolTip='<%# Eval("mime") %>' ImageAlign="Top" style="height:20px;"/></ItemTemplate>
                    
                            <ItemTemplate>
                                <li><a class="bodytext" href="/data/documents/<%# Eval("groupid") %>/<%# Eval("filename") %>" title='<%# Eval("name") %>' target="_blank" onclick="ga('send', 'event', 'Download', '<%# Eval("mime").ToString().Replace(".", "") %>', '<%# Eval("filename").ToString() %>');"><%# Eval("name") %></a>
                                </li>
                            </ItemTemplate>
                 </asp:DataList>
                </ul>
            </div>
        </div>
    </div>
    </div><br class="clear" />
</asp:Panel>
