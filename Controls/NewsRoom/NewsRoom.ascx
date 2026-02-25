<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsRoom.ascx.cs" Inherits="Controls_NewsRoom_NewsRoom" %>
<%@ Reference Page="~/Default.aspx"  %>
<style>
.round_corners
    {
    -moz-border-radius: 5px 5px 5px 5px;
    -webkit-border-radius: 5px 5px 5px 5px;
    -khtml-border-radius: 5px 5px 5px 5px;
    border-radius: 5px 5px 5px 5px;
    
    behavior: url(/js/PIE-2.0beta1/PIE.htc);
    }
.TemplateGrid hr{
    margin:20px 0 5px 0;
    border:dotted 1px #E1E1E1;
}
.TemplateGrid img{
     width: 100%;
     height: auto;
}

.TemplateGrid img.likeit{
     width:unset;
     height: unset;
}
/* .news-item-web{
            display:none;
        }*/
 .news-item-web p{
     margin-top:0;
 }
    .news-item-web h2 {
        margin-bottom:1px;
    }
  .news-item-mobile{
            display:block;
        }

    @media (min-width: 767px) {
        .news-item-web{
            display:table;
        }
        .news-item-mobile{
            display:none;
        }
    }

    @media (max-width: 1200px) {
        div.row.filter .columns,
        div.row.filter {
            width: 100%;
            margin-bottom:10px;
        }
    }

@media (min-width: 1200px) {
    div.row.filter {
        width: 78%;
    }
}

#div-wrapper-news{
    position:relative;
}
#div-wrapper-news > div:first-child{
    padding: 15px 0;
}
#div-wrapper-new #trDates h2{
    margin:0px;
}
   /* #div-wrapper-news h3 {
        color:#696969 ;
        font-size:15px;
    }*/
#div-select-year div{
    display:table-cell;
}
#div-select-year div:last-child{
    padding-left:5px;
}
#Content_ctl00_ddlDateSelect, #Content_ctl00_ddlCategories{
    margin:0px;
}
.news_subtitle{
    padding-top:0px; width: 150px;
}
.TemplateGrid{
    width:100%;
}

.TemplateGrid .news_subtitle td{
    vertical-align: top;
}
.TemplateGrid .news-item-web, .TemplateGrid .news-item-mobile{
    padding-top:0px; width:100%;
    margin-top:15px;
}
    .TemplateGrid table tr:first-child td:first-child{
        vertical-align:top; padding-top:15px; width: 135px;
    }
    .TemplateGrid table tr:first-child td:last-child{
        vertical-align:top; padding-top:8px; padding-left: 20px;
    }
    .TemplateGrid table tr:last-child td{
        text-align:right;
    }

    .TemplateGrid div.newsitem{
        overflow:hidden; padding:0px; width:100%; cursor:pointer;
    }
    /*.span-posted{
        color:#707070;font-family:Arial;font-size:13px;
    }*/
 input.nosize{
    
     display:none!important;
 }
 #divLikeit  button{
     background:transparent!important;
     border:none!important;
 }
 #div-select-year label{
     font-family: 'PT Sans Regular','PT Sans', Merriweather, 'Muli', sans-serif;
    color: #333333;
    font-size: 16px;
    font-weight: bold;
    text-transform: none;
 }
 a.read_more{
    margin-top: 0px;
    color: #a23193 !important;
    font-size: 14px !important;
    float:right;
    font-weight:normal!important;
}

    a.read_more:hover {
        color: #a23193 !important;
    }

.news-item-web .row div,
.news-item-mobile .row div{
    display:table-cell;
    vertical-align:top;
}
.div-photo{
    width:90px;
    padding-right: 20px;
}
</style>

<script>
$( document ).ready(function() {
    $("a.open_new_tab").click(function () {
        //alert($(this).attr("filename"));
        window.open($(this).attr("filename"), null, 'status=no, toolbar=no, menubar=no, location=no, scrollbars=yes, resizable');
        return false;
    });
});
    var category = <%= category%>;
    var records = <%=records%>;
    var publish = <%=publish%>;
</script>
<div id="div-wrapper-news">
    <div class="row filter" role="presentation">
        <div class="twelve columns" runat="server" id="trDates">
            <%--<div class="three columns">
                <h2><%if (Language == "1"){%>Now viewing:<%}else{%>Affichage des nouvelles de:<%} %></h2>
            </div>--%>
            <%--<div class="six columns" style="display:none;">
                <asp:DropDownList ID="ddlCategories" runat="server" AutoPostBack="True" DataTextField="name" DataValueField="id" ToolTip="Category"
                        onselectedindexchanged="ddlCategories_SelectedIndexChanged">
                 </asp:DropDownList>
                     <input type="submit" value="Search" class="nosize" />
            </div>--%>
            <div class="three columns" id="div-select-year">
                <div><label for="<%=ddlDateSelect.ClientID %>">from:&nbsp;&nbsp;</label></div>
                <div>
                <asp:DropDownList ID="ddlDateSelect" runat="server" AutoPostBack="True" ToolTip="Year" 
                        onselectedindexchanged="ddlDateSelect_SelectedIndexChanged">
                 </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>

    <div class="TemplateGrid">
    <asp:Repeater runat="server" ID="DataList1" OnItemDataBound="DataList1_ItemDataBound">
        <ItemTemplate>
           
                   <div role="presentation" class="news-item-web">
                    <div class="row">
                        <div class="div-photo">
                            <asp:Image runat="server" ID="imgPhoto" AlternateText='<%# Eval("PhotoAltText") %>' GenerateEmptyAlternateText="True" CssClass="round_corners" /></div>

                        <div>
                            <h2>
                                <%#((DateTime)Eval("NewsDate")).ToString("MMMM dd, yyyy")%></h2>
                            <h3><asp:Label ID="lblTitle" runat="server" Text='<%#Eval("Title")%>'></asp:Label></h3>
                            
                                <asp:Literal runat="server" ID="litLikeIt"></asp:Literal>
                               <%-- <asp:Label ID="lblCategories" runat="server" CssClass="news-item-category" />--%>
                                    <p><%#Eval("DetailsShort")%></p>
                                </div>
                    </div>

                    <div class="row">
                        <a id="theLink" runat="server" class="newsitem read_more">Read more...</a>
                    </div>
                </div>

            <hr />
        </ItemTemplate>
    </asp:Repeater>
    </div>
            <%--<asp:LinkButton runat="server" ID="lnbMore" OnClick="lnbMore_Click" Text="load more"></asp:LinkButton>--%>
            <%if (bLoadMore)
                { %>
            
            <div class="div_load_more TemplateGrid"><div class="grid3col contained-width"></div></div>
            <%--<span id="span_load_more" class="load-more">load more</span><br />--%>
           <%-- <span class="load-less">load less</span>--%>
            <%} %>
       

    <div id="singleitem" runat="server" visible="false">
       <div class="view-all-news"><a href='/<%= newspage %>' style="float:right;">View All News</a></div>

     
                    <h2><asp:Literal ID="litTitle" runat="server"></asp:Literal></h2>
             
                    <h3><asp:Literal ID="litDate" runat="server"></asp:Literal></h3>
             
                    <asp:Image runat="server" ID="imgPhoto1" Visible="false" />
                    <%--<p><asp:Literal ID="litDetails" runat="server"></asp:Literal></p>--%>
                            <p><asp:PlaceHolder runat="server" ID="plContent"></asp:PlaceHolder></p>
                    
            
            <%--<div runat="server" id="trLikeit">
                    <%if (Request.QueryString["newsid"] != null)
                        { %>
                    <div style="float:right;" id="divLikeit"><button type="button" id="<%=Request.QueryString["newsid"] %>_btnLikeIt" class="LikeIt"><img src="/Images/Icons/thumb-up-64.png" alt="I like it" style="" /></button></div>
                    <%} %>
            </div>
<asp:Literal runat="server" ID="litjsLikeIt"></asp:Literal>--%>
    </div>
</div>

            <div class="mike-gray">            
                <asp:PlaceHolder runat="server" ID="phBreakingNews"></asp:PlaceHolder>
            </div>

