<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Zone.aspx.cs" Inherits="Controls_Zone_Zone" %>
<%@ Register TagPrefix="i386" Namespace="i386.UI" Assembly="i386.UI" %>
<link href="/CSS/LemonAid.css" rel="stylesheet" type="text/css" />


<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title></title>
    <link type="text/css" href="/css/admin.css" rel="stylesheet" />
    <link href="/CSS/LemonAid.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            background-color: #0078d8;
            
        }
        
    </style>
<%--    <script src="/JS/jquery-1.3.1.min.js" type="text/javascript"></script>--%>
    <script src="/Libraries/jquery/jquery-3.7.1.min.js" type="text/javascript"></script>
	<script src="/JS/jquery.corner.js" type="text/javascript"></script>
	<script type="text/javascript">
	$(document).ready(function()
	{
		$(".WhiteTable").corner("top 15px");
	});
	</script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-top:50px; padding-left:100px;">
    <asp:ScriptManager ID="sman" runat="server" EnablePartialRendering="False" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button runat="server" Text="Live" CssClass="editor_button_top_on2" style="color:#ffffff; display:none;" ID="btnLive"
                OnClick="clickedLive" />&nbsp;<asp:Button ID="btnPending" runat="server" Text="Pending"
                    CssClass="editor_button_top_off2" style="color:#ffffff; display:none;" OnClick="clickedPending" /><br />
            <div >
            <i386:ImageOverButton ID="ImageOverButton1" runat="server" CssClass="button" 
                OnClientClick="window.parent.parent.location.reload()" ImageOverUrl="/images/lemonaid/buttons/back_over.png" ImageUrl="/images/lemonaid/buttons/back.png" /><br />
            <table cellpadding="0" cellspacing="0" border="0" style="  margin:0px; padding:0px; border-collapse:collapse;"><tr><td colspan="3" style="vertical-align:top; line-height:22px;"><div style="float:left;position:relative;top:22px;"><img src="/images/lemonaid/partials/corners_tl.png" /></div><div style="float:right;position:relative;top:22px;left:2px;"><img src="/images/lemonaid/partials/corners_tr.png" /></div></td></tr>
			<tr><td colspan="3" style="height:8px; line-height:8px; background-color:#409ae2;">&nbsp;</td></tr>
			<tr><td style="width:12px; background-color:#409ae2;">&nbsp;</td><td>
<table border="0" cellpadding="0" cellspacing="0"  style="background-color:#409ae2; padding:0px;">
            <tr><td class="admin_bodytext_blue">New Content :</td><td><table border="0" cellpadding="0" cellspacing="0"><tr><td><img src="/images/lemonaid/partials/gridrow_left2.png" /></td><td style="background-image:url('/images/lemonaid/partials/gridrow_bg2.png');background-repeat:repeat-x;width:150px;"><asp:TextBox ID="tbHTML" runat="server" Width="150" CssClass="textbox"></asp:TextBox></td><td><img src="/images/lemonaid/partials/gridrow_right2.png" /></td></tr></table>
                        </td>
                        <td style="padding-left: 10px"><i386:ImageOverButton ID="ibNewHTML" runat="server" CssClass="button" 
                OnClick="AddNewHTML" ImageOverUrl="/images/lemonaid/buttons/create_over.png" ImageUrl="/images/lemonaid/buttons/create.png" />
                        </td>
                    </tr>
                    </table></td><td style="width:12px; background-color:#409ae2;background-image:url('/images/lemonaid/partials/shadowr.png'); background-repeat:repeat-y; background-position:right;">&nbsp;</td></tr><tr><td colspan="3" style="height:8px; line-height:8px; background-color:#409ae2;">&nbsp;</td></tr><tr><td colspan="3" style="background-image:url('/images/lemonaid/partials/shadow.png');background-repeat:repeat-x;vertical-align:top; line-height:22px; margin:0; "><div style="float:left;position:relative;top:-19px;margin:0;"><img src="/images/lemonaid/partials/corners_bl.png" /></div><div style="float:right;position:relative;top:-19px;left:2px;"><img src="/images/lemonaid/partials/corners_br.png" /></div></td></tr>
		  </table>
            
                <table cellpadding="0" cellspacing="0" border="0" style="  margin:0px; padding:0px; border-collapse:collapse;"><tr><td colspan="3" style="vertical-align:top; line-height:22px;"><div style="float:left;position:relative;top:22px;"><img src="/images/lemonaid/partials/corners_tl.png" /></div><div style="float:right;position:relative;top:22px;left:2px;"><img src="/images/lemonaid/partials/corners_tr.png" /></div></td></tr>
			<tr><td colspan="3" style="height:8px; line-height:8px; background-color:#409ae2;">&nbsp;</td></tr>
			<tr><td style="width:12px; background-color:#409ae2;">&nbsp;</td><td>
<table border="0" cellpadding="0" cellspacing="0"  style="background-color:#409ae2; padding:0px;">
                    <tr>
                        <td style="width:110px;">
                            <span class='admin_bodytext_blue'>Add Content :</span>
                        </td>
                        <td><table border="0" cellpadding="0" cellspacing="0"><tr><td><img src="/images/lemonaid/partials/gridrow_left2.png" /></td><td style="background-image:url('/images/lemonaid/partials/gridrow_bg2.png');background-repeat:repeat-x;width:205px;"><asp:DropDownList ID="ddlContentHTML" runat="server" Width="270"/></td><td><img src="/images/lemonaid/partials/gridrow_right2.png" /></td></tr></table>
                        </td>
                        <td style="padding-left: 10px">
                        
                            <asp:ImageButton ID="ibAddHTML" runat="server" OnClick="AddToZone" ImageUrl="/images/lemonaid/buttons/add.png" CommandArgument="ddlContentHTML" />
                        </td>
                    </tr>
					
                    <tr>
						<td colspan="3"><br /></td>
                    </tr>
                    <tr>
                        <td>
                            <span class='admin_bodytext_blue'>Add Widget :</span>
                        </td>
                        <td><table border="0" cellpadding="0" cellspacing="0"><tr><td><img src="/images/lemonaid/partials/gridrow_left2.png" /></td><td style="background-image:url('/images/lemonaid/partials/gridrow_bg2.png');background-repeat:repeat-x;width:205px;"><asp:DropDownList ID="ddlContentControl" runat="server"  Width="270" /></td><td><img src="/images/lemonaid/partials/gridrow_right2.png" /></td></tr></table>
                        </td>
                        <td style="padding-left: 10px">
                            <asp:ImageButton ID="ibAddControl" runat="server" OnClick="AddToZone" ImageUrl="/images/lemonaid/buttons/add.png"  CommandArgument="ddlContentControl"/>
                        </td>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                </table></td><td style="width:12px; background-color:#409ae2;background-image:url('/images/lemonaid/partials/shadowr.png'); background-repeat:repeat-y; background-position:right;">&nbsp;</td></tr><tr><td colspan="3" style="height:8px; line-height:8px; background-color:#409ae2;">&nbsp;</td></tr><tr><td colspan="3" style="background-image:url('/images/lemonaid/partials/shadow.png');background-repeat:repeat-x;vertical-align:top; line-height:22px; margin:0; "><div style="float:left;position:relative;top:-19px;margin:0;"><img src="/images/lemonaid/partials/corners_bl.png" /></div><div style="float:right;position:relative;top:-19px;left:2px;"><img src="/images/lemonaid/partials/corners_br.png" /></div></td></tr>
		  </table><br />
                <div style="padding:6px;"><table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;"><tr><td style="vertical-align:top; line-height:22px; margin:0;"><div style="float:left;position:relative;top:47px;"><img src="/images/lemonaid/partials/corners_tl.png" /></div><div style="float:right;position:relative;top:47px;left:2px;"><img src="/images/lemonaid/partials/corners_tr.png" /></div></td></tr>
			<tr><td >
                    <asp:GridView ID="gvContent" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        CellPadding="0" DataKeyNames="Priority" GridLines="None" Width="100%" OnRowCommand="gvContent_RowCommand"
                        OnRowDataBound="gvContent_RowDataBound" OnRowDeleting="gvContent_RowDeleting"
                        OnSorting="gvContent_Sorting" OnDataBound="gvContent_DataBound">
                        <HeaderStyle ForeColor="White" Font-Underline="false" HorizontalAlign="Left" CssClass="padbottom" Height="30" VerticalAlign="Top"  />
                <Columns>
               <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemStyle Width="15"></ItemStyle></asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemTemplate><img src="/images/lemonaid/partials/gridrow_left.png" /></ItemTemplate></asp:TemplateField>
                            <asp:BoundField DataField="Content_Name" 
                                HeaderStyle-HorizontalAlign="Left" HeaderText="On This Page" SortExpression="Content_Name"  ItemStyle-CssClass="itemrow">
                                <HeaderStyle HorizontalAlign="Left" Width="400" />
                            </asp:BoundField>
                            <asp:BoundField DataField="priority" HeaderStyle-CssClass="TemplateGridHeaderFont" Visible="false"
                                HeaderStyle-HorizontalAlign="Left" HeaderText="Priority" SortExpression="Priority">
                                <HeaderStyle CssClass="TemplateGridHeaderFont" HorizontalAlign="Left" Width="20%" />
                            </asp:BoundField>
                             <asp:TemplateField ItemStyle-BackColor="#409ae2"><ItemTemplate><img src="/images/lemonaid/partials/gridrow_right.png" /></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemStyle Width="10"></ItemStyle></asp:TemplateField>
                            <asp:TemplateField HeaderText="" ShowHeader="False"  ItemStyle-BackColor="#409ae2">
                                <ItemTemplate>
                                    <%if (CurrentTable.Rows.Count>1)
                                      { %>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:ImageButton ID="ibUP" runat="server" AlternateText="Move Up" CommandName="Up"
                                                    ImageUrl="/images/lemonaid/buttons/up.png" CommandArgument='<%# Eval("priority") %>' />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ibDown" runat="server" CommandArgument='<%#Eval("priority") %>' CommandName="Down"
                                                    ImageUrl="/images/lemonaid/buttons/down.png" />
                                            </td>
                                            <td style="padding-left:10px">
                                                <asp:ImageButton ID="ibDelete" runat="server" CommandName="Delete" ImageUrl="/images/lemonaid/buttons/ex.png" />
                                            </td>
                                        </tr>
                                    </table>
                                    <%} %>
                                </ItemTemplate>
                                <HeaderStyle CssClass="TemplateGridHeaderFont" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                             <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemStyle Width="10"></ItemStyle></asp:TemplateField>
                         <asp:TemplateField ><ItemTemplate><img src="/images/lemonaid/partials/shadowr.png" height="49" width="4" /></ItemTemplate></asp:TemplateField>
                        </Columns>
                    </asp:GridView></td></tr><tr><td style="background-image:url('/images/lemonaid/partials/shadow.png');background-repeat:repeat-x;vertical-align:top; line-height:22px; margin:0;"><div style="float:left;position:relative;top:-19px;margin:0;"><img src="/images/lemonaid/partials/corners_bl.png" /></div><div style="float:right;position:relative;top:-19px;left:2px;"><img src="/images/lemonaid/partials/corners_br.png" /></div><br /></td></tr><tr><td><i386:ImageOverButton ID="btnSave" runat="server" CssClass="button" 
                OnClick="btnSave_Click" ImageOverUrl="/images/lemonaid/buttons/save_over.png" ImageUrl="/images/lemonaid/buttons/save.png" />&nbsp;&nbsp;&nbsp;<i386:ImageOverButton ID="btnApprove" runat="server" CssClass="button" 
                OnClick="clickedApprove" ImageOverUrl="/images/lemonaid/buttons/approve_over.png" ImageUrl="/images/lemonaid/buttons/approve.png" /></td></tr>
		  </table>
                </div>
            </div>
            
                
          
            
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
