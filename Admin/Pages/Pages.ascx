<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Pages.ascx.cs" Inherits="Admin_Pages_Pages" %>
<%@ Register TagPrefix="i386" Namespace="i386.UI" Assembly="i386.UI" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<link href="/CSS/DarkStyle.css" rel="stylesheet" type="text/css" />
<link href="/CSS/LemonAid.css" rel="stylesheet" type="text/css" />
<table cellpadding="0" cellspacing="0" border="0"><tr><td rowspan="2"><img src="/images/lemonaid/icons/pages.png" /></td><td valign="top"><i386:FontLabel ID="FontLabel2" Font-Size="35px" Font-Names="Century Gothic" Width="300px" Height="58px" BorderWidth="0" runat="server" BackColor="#0078D8" ForeColor="#FFFFFF">Pages</i386:FontLabel></td></tr>
        <tr><td style="padding-left:10px;color:#91caf7;font-size:16px;font-family:Arial; font-weight:bold;" valign="top" height="53">This is a list of pages on your website, where you can edit or delete them.</td></tr>
        </table><br /><br />
         <asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
<asp:UpdatePanel runat="server" ID="up1" UpdateMode="Always">
<Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtFilter" EventName="TextChanged"/>
      </Triggers>
<ContentTemplate>
        <table border="0" cellpadding="0" cellspacing="0"><tr><td><img src="/images/lemonaid/partials/filter_left.png" /></td><td style="background-image:url('/images/lemonaid/partials/filter_bg.png');background-repeat:repeat-x;font-family:Arial;">Filter :</td><td style="padding-left:5px;background-image:url('/images/lemonaid/partials/filter_bg.png');background-repeat:repeat-x;"><asp:TextBox ID="txtFilter" runat="server" Width="100" OnTextChanged="filter" AutoPostBack="true" BorderWidth="0" BackColor="Transparent" style="background-image:url('/images/lemonaid/partials/filtertext.jpg'); background-repeat:no-repeat; width:201px; height:33px; font-size:15px; font-family:Arial, Sans-Serif; color:#0078d8; font-weight:bold; padding-top:7px; vertical-align:top; padding-left:8px;"/></td><td style="padding-left:5px;background-image:url('/images/lemonaid/partials/filter_bg.png');background-repeat:repeat-x;"><i386:ImageOverButton ImageOverUrl="/images/lemonaid/buttons/runfilter_over.png" onclick="filter" ImageURL="/images/lemonaid/buttons/runfilter.png" runat="server" /></td><td><img src="/images/lemonaid/partials/filter_right.png" /></td></tr></table><br />
                <div id="tbl_noresults" runat="server" class="admin_bodytext_white">No results were found with the current filter.<asp:Label ID="lbl_msg" runat="server" CssClass="alert"></asp:Label></div>
        
		  <table id="tbl_Grid" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;"><tr><td style="vertical-align:top; line-height:22px; margin:0;"><div style="float:left;position:relative;top:47px;"><img src="/images/lemonaid/partials/corners_tl.png" /></div><div style="float:right;position:relative;top:47px;left:2px;"><img src="/images/lemonaid/partials/corners_tr.png" /></div></td></tr>
			<tr><td ><asp:GridView ID="GV_Main" runat="server" AllowPaging="True" Font-Names="Arial, sans-serif" 
                AllowSorting="False" AutoGenerateColumns="False" 
                DataKeyNames="id"
                ondatabound="GV_Main_DataBound" onpageindexchanging="GV_Main_PageIndexChanging" 
                OnRowDeleting="GV_Main_RowDeleting" 
                onsorting="GV_Main_Sorting" GridLines="None" CellPadding="0" CellSpacing="0" 
                    onrowdatabound="GV_Main_RowDataBound">
                <FooterStyle/>
                <PagerStyle CssClass="newpagerstyle" HorizontalAlign="Center" ForeColor="#ffbb51" Font-Bold="true" Height="30"   />
				<HeaderStyle ForeColor="White" Font-Underline="false" HorizontalAlign="Left" CssClass="padbottom" Height="30" VerticalAlign="Top"  />
                <Columns>
				<asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemStyle Width="15"></ItemStyle></asp:TemplateField>
				<asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemTemplate><img src="/images/lemonaid/partials/gridrow_left.png" /></ItemTemplate></asp:TemplateField>
                    <asp:BoundField DataField="name" HeaderText="Name" 
                        SortExpression="name">
                    <ItemStyle CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" Width="250"/>
                    </asp:BoundField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="itemrow"><ItemStyle Width="10"></ItemStyle></asp:TemplateField>
                     <asp:BoundField DataField="gname" HeaderText="Group" 
                        SortExpression="layout_name">
                    <ItemStyle CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                    
                    </asp:BoundField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="itemrow"><ItemStyle Width="10"></ItemStyle></asp:TemplateField>
                    <asp:BoundField DataField="language_name" HeaderText="Language" 
                        SortExpression="language_name">
                        <ItemStyle CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="itemrow"><ItemStyle Width="10"></ItemStyle></asp:TemplateField>
                    <asp:BoundField DataField="seo" HeaderText="SEO" SortExpression="seo">
                    <ItemStyle CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                   
                    </asp:BoundField>
                     
					<asp:TemplateField ItemStyle-BackColor="#409ae2"><ItemTemplate><img src="/images/lemonaid/partials/gridrow_right.png" /></ItemTemplate></asp:TemplateField>
					<asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemStyle Width="10"></ItemStyle></asp:TemplateField>
					<asp:TemplateField ItemStyle-BackColor="#409ae2">
							<HeaderTemplate></HeaderTemplate>
                            <ItemTemplate>
                                <a target="_blank" runat="server" id="lkSeo"><img src="/images/lemonaid/buttons/magnify.png" border="0" alt="View"/></a>
                            </ItemTemplate>
                            <ItemStyle  HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemStyle Width="10"></ItemStyle></asp:TemplateField>
                    <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-BackColor="#409ae2">
                        <ItemTemplate>
                            <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False"  AlternateText="Delete"
                                CommandName="Delete" ImageUrl="/images/lemonaid/buttons/ex.png"/>
                        </ItemTemplate>
                        <ItemStyle  HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemStyle Width="10"></ItemStyle></asp:TemplateField>
					<asp:TemplateField ><ItemTemplate><img src="/images/lemonaid/partials/shadowr.png" height="49" width="4" /></ItemTemplate></asp:TemplateField>
                </Columns>
            </asp:GridView></td></tr><tr><td style="background-image:url('/images/lemonaid/partials/shadow.png');background-repeat:repeat-x;vertical-align:top; line-height:22px; margin:0;"><div style="float:left;position:relative;top:-19px;margin:0;"><img src="/images/lemonaid/partials/corners_bl.png" /></div><div style="float:right;position:relative;top:-19px;left:2px;"><img src="/images/lemonaid/partials/corners_br.png" /></div></td></tr>
		  </table><cc:PagerV2_8 ID="pager1" runat="server" 
                OnCommand="pager_Command" 
                GenerateGoToSection="true" PageSize="10" Font-Names="Arial" 
                />
		  
	<asp:Repeater ID="repPages" runat="server" Visible="false">
		<HeaderTemplate>
			<table border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td></td><td>Name</td><td>Group</td><td>Language</td><td>SEO</td><td></td><td>View</td><td>Delete</td>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr style="background-color: #409ae2;">
				<td><img src="/images/lemonaid/partials/gridrow_left.png" /></td>
				<td class="itemrow"><%# Eval("name") %></td><td class="itemrow"><%# Eval("gname") %></td>
				<td class="itemrow"><%# Eval("language_name") %></td><td class="itemrow"><%# Eval("seo") %></td>
				<td><img src="/images/lemonaid/partials/gridrow_right.png" /></td><td></td><td></td>
			</tr>
		</ItemTemplate>
		<FooterTemplate></table></FooterTemplate>
	</asp:Repeater>
            
                           <br /><i386:ImageOverButton ID="bttn_Add_Question" runat="server" CssClass="button" 
                OnClick="bttn_Add_Question_Click" ImageOverUrl="/images/lemonaid/buttons/add_over.png" ImageUrl="/images/lemonaid/buttons/add.png" />
</ContentTemplate>
</asp:UpdatePanel>
<div runat="server" id="divpopup" visible="false" style="width: 400px; height:100px; background-color:Beige;">
    <span>You are about to delete a page which contain sub-items. Click OK to continue</span><br />
    <asp:Button runat="server" ID="btnOK" Text="OK" onclick="btnOK_Click" />&nbsp;&nbsp;<asp:Button 
        runat="server" ID="btnCancel" Text="Cancel" onclick="btnCancel_Click" />
</div>