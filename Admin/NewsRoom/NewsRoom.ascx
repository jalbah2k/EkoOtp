<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsRoom.ascx.cs" Inherits="Admin_NewsRoom_NewsRoom" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register src="EditPanel.ascx" tagname="EditPanel" tagprefix="uc1" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
<script type="text/javascript" src="/js/JScript.js"></script>

<div class="admin-header-wrapper noprint">
    <div class="admin-header">Newsroom</div>
    <div class="admin-header-subtitle">This area allows you to manage the news articles in the News Room.</div>
</div>
<div class="admin-control-wrapper">
    <asp:Panel ID="pnlList" runat="server">
        <div class="admin-white-box" style="min-width: 640px;">
            <div class="admin-white-box-header">
                <asp:LinkButton ID="ImageButton3" runat="server" CssClass="admin-button-blue" Text="Add" onclick="clickadd" />
            </div>
            <div class="admin-white-box-inner">
                <table id="tbl_grid" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			    <tr><td colspan="2">
                    <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="True" AutoGenerateColumns="False" AllowPaging="true"
                        GridLines="None"
                        OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" OnSorting="GV_Main_Sorting"
                        OnRowCommand="GV_Main_RowCommand" OnRowDataBound="GV_Main_RowDataBound">
                        <HeaderStyle CssClass="admin-grid-header" />
                        <PagerSettings Visible="false" />
                        <Columns>
                            <asp:BoundField DataField="title" HeaderText="Title" SortExpression="Title"
                                HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="language" HeaderText="Language" SortExpression="language"
                                HeaderStyle-HorizontalAlign="Left" Visible="false">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle  CssClass="itemrow" Width="100" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NewsDate" HeaderText="Date" SortExpression="NewsDate"
                                DataFormatString="{0:MMM d, yyyy}">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle  CssClass="itemrow" Width="100" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="likeit" HeaderText="Likes" SortExpression="likeit"
                                 HeaderStyle-HorizontalAlign="Left" Visible="false">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle  CssClass="itemrow" Width="100" HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CreatedDate" HeaderText="Submit Date" SortExpression="CreatedDate" Visible="false">
                                <ItemStyle CssClass="itemrow" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Author" HeaderText="Author" SortExpression="Author"
                                HeaderStyle-HorizontalAlign="Left" Visible="false">
                                <ItemStyle CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <ItemStyle CssClass="itemrow" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="litImgStatus"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" ImageURL="/images/lemonaid/buttonsNew/pencil.png" alt='edit' runat="server" onclick="clickedit" CommandArgument='<%#Eval("id") %>' ToolTip='Edit' />
                                    <asp:ImageButton ID="lbDelete" runat="server" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
			    </td></tr>
                <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		        </table>
                <div id="noResults" runat="server" style="padding: 20px 0;">There are currently no news.<br /></div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlAdd" runat="server" Visible="false">
        <div class="admin-white-box">
            <div class="admin-white-box-header">Details</div>
            <div class="admin-white-box-inner">
                <div id="tabs_dummy">
                   <%-- <% if ((bool)Session["Multilingual"]) { %>
                    <ul class="tab_link">
                        <li><a href="#tabs-1" id="li_en">English</a></li>
                        <li><a href="#tabs-2" id="li_fr">French</a></li>
                    </ul>
                    <% } %>--%>
                    <div id="tabs-1">
                        <uc1:EditPanel ID="EditPanel1" runat="server" Language="1" />
                    </div>
                   <%-- <% if ((bool)Session["Multilingual"]) { %>
                    <div id="tabs-2"> 
                        <uc1:EditPanel ID="EditPanel2" runat="server" Language="2" />
                    </div>
                    <% } %>--%>
                </div>
            </div>
        </div>
        <br />
        <asp:LinkButton ID="ImageButton2" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="clickcancel" CausesValidation="False" />
        <asp:LinkButton ID="ImageButton4" runat="server" CssClass="admin-button-green mw150" Text="Done" onclick="clickcancel" CausesValidation="False" />
        <% if ((bool)Session["Multilingual"]) { %>
        <script type="text/javascript">

            $(document).ready(function () {
                $("#tabs").tabs();

                $("#tabs").on("tabsactivate", function (event, ui) {
                    $('iframe.CuteEditorFrame').css('height', '200px');
                });
            });

        </script>
        <% } %>
        <script type="text/javascript">

            var CEWLValidator = function (ctv) {
                var ce = $("table:regex([id,/.*" + ctv + ".*/])");
                var ceClass = ce.attr('class');

                var number = 0;
                var minWords = 0;
                var maxWords = 0;
                var countControl = ceClass.substring((ceClass.indexOf('[')) + 1, ceClass.lastIndexOf(']')).split(',');

                if (countControl.length > 1) {
                    minWords = parseInt(countControl[0]);
                    maxWords = parseInt(countControl[1]);
                }
                else {
                    maxWords = parseInt(countControl[0]);
                }

                //number = $("table:regex([id,/.*" + ctv + ".*/])").find('.WordCount').text().match(/[0-9 -()+]+$/);
                //number = parseInt(ce.find('.WordCount').text().match(/[0-9 -()+]+$/)); // Words counter
                number = parseInt(ce.find('.CharCount').text().match(/[0-9 -()+]+$/));  // Characters counter
                //alert(maxWords);
                if (number < minWords || (number > maxWords && maxWords != 0))
                    return false;
                else
                    return true;

            }

            function CheckWordLimit(sender, args) {
                if (sender.controltovalidate.match(/CE_/g) == "CE_")
                    args.IsValid = CEWLValidator(sender.controltovalidate);
                /*else
                args.IsValid = WordCountValidator(sender.controltovalidate, args.Value);*/
            }

        </script>
    </asp:Panel>
</div>
