<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Controls_Search_Search" %>
<%@ Register TagPrefix="Custom" Namespace="ASB" Assembly="AutoSuggestBox" %>
<%@ Reference Page="~/Default.aspx"  %>

<style type="text/css">
.pager, .pager a, .pager a:hover, .pager a:link
{
  font-weight:bold;
  font-family:Arial, Calibri;
  font-size:14px;
  color:#bfbfbf;
  text-decoration:none;
}
.pager 
{
  color:#4f3695;
}
#search-wrapper .asbMenu
{
    left: -6px;
    right: auto;
    top: 33px;
}
.searchPanel.inside{
    float:left;
}
</style>
<%if (!First)
  {%>
<div style="padding-bottom: 20px; padding-left: 0px;">
   <h1> <%=(Language == "en"?"Results":"Résultats")%> </h1>
</div>
<%}%>
<%if (First && advanced)
  {%>
<div style="padding-bottom: 20px; padding-left: 0px;display:none;">
   <h1> <%=(Language == "en" ? "Advanced Search" : "Recherche avancée")%></h1>
</div>
<%}%>

   <div><asp:Literal runat="server" ID="litSuggestion"></asp:Literal></div>

<%if ((bool)ViewState["first"] || (!(bool)ViewState["first"] && !searched))
  {%>

<asp:Panel ID="Panel1" CssClass="searchPanel inside" runat="server" DefaultButton="btSearch" role="search" Visible="false">
    <div role="presentation">
        <div class="row">
            <div style="display:table-cell; vertical-align:middle;position:relative;">
                <label for="<%=tbSearch.ClientID %>" class="nosize">Search:</label>   
                <Custom:AutoSuggestBox id="tbSearch" CssClass="tbSearch searchInput" DataType="Searches" runat="server" ResourcesDir="/asb_includes" style="border:none; color:#000000; vertical-align:middle; padding-left:5px; height:37px;" ValidationGroup="search" Text="" onfocus="if (this.value==search) this.value='';" onblur="if (this.value.length==0) this.value=search;"/>
            </div>
            <div style="display:table-cell; vertical-align:middle; width: 29px; padding-left: 2px;">
                <button type="button" onclick="javascript: $('#<%=btSearch.ClientID %>').click();"><i class="fa fa-search" aria-hidden="true"></i><span>Search</span></button>
                <div style="display:none;"><asp:ImageButton ID="btSearch" title="search button" ValidationGroup="search" runat="server" OnClick="Search" AlternateText="search button" /></div>
            </div>   
        </div>
    </div>
</asp:Panel>
<%--<br /><br />
    <%if(advanced) {%><asp:DropDownList ID="ddlGroups" runat="server"  Width="200px"> </asp:DropDownList><%}%> --%>



<%}%>
<%if (!First)
  {%>
<% if (searched)
   {%>
<table style="width: 100%;padding-left:15px; margin: 0 auto;">
    <tr>
        <td class="bodytext"><asp:Literal id="wordsA" runat="Server" Text="Your search for keyword(s) '"/><%=tbSearch.Text%><asp:Literal id="wordsB" runat="Server" Text="' produced: "/>
            <%--<%= TotalRecords.ToString() %><asp:Literal id="wordsC" runat="Server" Text=" results" />--%>

        </td>
        
    </tr>
    <tr id="ptitle" runat="server"><td style="padding-top:20px;"><h2>Pages</h2></td></tr>
    <tr id="pgrid" runat="server">
        <td>
            <asp:Literal runat="server" ID="litPageResults"></asp:Literal>
            <div style=" margin-left: 10px; width:100%">
                <asp:DataGrid ID="dbSearch" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                    Width="100%" CellSpacing="-1" GridLines="None" PageSize="1" 
                    AllowPaging="true" role="presentation">
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <div style="padding: 10px; border-bottom: dotted 1px #bfbfbf;">
                                <table style="width: 100%;">
                                <tr>
                                <td style="width:5%;padding-right:5px;font-family:Calibri;font-size:10px;color:#555555; display:none"><%# DataBinder.Eval(Container.DataItem,"id")%></td>
                                <td class="bodytext"><a href='<%=LanguagePrefix %><%#DataBinder.Eval(Container.DataItem,"seo")%>'><%#DataBinder.Eval(Container.DataItem,"Title")%></a></td>
                                </tr>
                                <%--<tr><td style="display:none">&nbsp;</td><td class="bodytext" ><%# DataBinder.Eval(Container.DataItem,"Description")%></td></tr>--%>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid></div>
        </td>
    </tr>
    <tr id="ntitle" runat="server"><td style="padding-top:20px;">
    <%if (Session["Language"].ToString() == "1")
      { %>
    <h2>News</h2>
    <%}
      else
      { %>
    <h2>Nouvelles</h2>
    <%} %>
    </td></tr>
    <tr id="ngrid" runat="server"><td>
        <asp:Literal runat="server" ID="litNewsResults"></asp:Literal>        
        <div style=" margin-left: 10px; width:100%">
                <asp:DataGrid ID="dbSearchNews" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                    Width="100%" CellSpacing="-1" GridLines="None" 
                    AllowPaging="true" role="presentation">
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <div style="padding: 10px; border-bottom: dashed 1px #000000;">
                                <table style="width: 100%;">
                                <tr>
                                <td style="width:5%;padding-right:5px;font-family:Calibri;font-size:10px;color:#555555; display:none"><%# DataBinder.Eval(Container.DataItem,"id")%></td>
                                <td class="bodytext"><a href='<%=LanguagePrefix %>newsroom?newsid=<%#DataBinder.Eval(Container.DataItem,"linkid")%>'><%#DataBinder.Eval(Container.DataItem,"Title")%></a></td>
                                </tr>
                                <tr style="display:none;"><td style="display:none">&nbsp;</td><td class="bodytext" ><%# DataBinder.Eval(Container.DataItem,"detailsshort")%></td></tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid></div></td></tr>
    
	<tr id="dtitle" runat="server" visible="false"><td style="padding-top:20px;"><h2>Directory</h2></td></tr>
    <tr id="dgrid" runat="server" visible="false">
        <td>
            <div style=" margin-left: 10px; width:100%; padding-top:20px;">
                <%--<uc1:StaffDirectory runat="server" ID="StaffDirectory1" />--%>
            </div>
        </td>
    </tr>

    <tr id="trTitleDirectory" runat="server"><td style="padding-top:20px;"><h2>Member Directory</h2></td></tr>
    <tr id="trGridDirectory" runat="server">
        <td>
            <asp:Literal runat="server" ID="litDirectoryResults"></asp:Literal>
            <div style=" margin-left: 10px; width:100%">
                <asp:DataGrid ID="dgDirectory" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                    Width="100%" CellSpacing="-1" GridLines="None" 
                    AllowPaging="false" role="presentation">
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <div style="padding: 10px; border-bottom: dotted 1px #bfbfbf;">
                                <table style="width: 100%;">
                                <tr>
                                <td style="width:5%;padding-right:5px;font-family:Calibri;font-size:10px;color:#555555; display:none"><%# DataBinder.Eval(Container.DataItem,"id")%></td>
                                <td class="bodytext"><a href='/memberdirectory/<%#DataBinder.Eval(Container.DataItem,"seo")%>'><%#DataBinder.Eval(Container.DataItem,"Title")%></a></td>
                                </tr>
                                <%--<tr><td style="display:none">&nbsp;</td><td class="bodytext" ><%# DataBinder.Eval(Container.DataItem,"Description")%></td></tr>--%>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid></div>
        </td>
    </tr>

</table>
<% }%>
<% if (!searched)
   {%>
        <div><asp:Literal runat="server" ID="litSuggestion2"></asp:Literal></div>

<table style="padding-left:15px; width: 100%;" runat="server" id="tbNoResults" visible="false">
    <tr>
        <td class="bodytext" style="padding-top: 10px">
            <asp:Literal ID="wordsD" runat="server" Text="Your search for '"/><strong><%=searchTerm%></strong><asp:Literal ID="wordsE" runat="server" Text="' produced no results." />
            <p style="margin-left: 55px;"><%=(Language == "en" ? "Suggestions:":"Suggestions:")%></p>
            <ul>
            <li style="margin-left: 95px;"><%=Language == "en" ? "Make sure all words are spelled correctly.":"Assurez-vous que tous les mots sont correctement orthographiés."%></li>
            <li style="margin-left: 95px;"><%=(Language == "en" ? "Try different keywords.":"Essayez d'autres mots.")%></li>
            <li style="margin-left: 95px;"><%=Language == "en" ? "Try more general keywords.":"Utilisez des mots plus généraux."%></li>
            <li style="margin-left: 95px;"><%=Language == "en" ? "Try fewer keywords.":"Essayez avec moins de mots-clés."%></li>
            </ul>
            <p />
        </td>
    </tr>
</table>
<%}%>
<%}%>
<%if(!advanced){ %>
<div class="bodytext" style='padding:10px 0 0 0px'><span style="display:none;"><a href='<%=LanguagePrefix %>searchAdvance'><%=(Language == "en" ? "Advanced Search" : "Recherche avancée")%></a></span></div><%} %>

