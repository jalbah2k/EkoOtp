<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CareerOp.ascx.cs" Inherits="controls_AZ_az" %>

<style type="text/css">
.letter
{
	color:#000;
	font-size:22px;
	font-family:Arial;
	font-weight:bold;
	cursor:pointer;
}
.disabledletter
{
	color:#aaa;
	font-size:22px;
	font-family:Arial;
	font-weight:bold;
	cursor:default;
}
</style>

<a name="TOP" />
<table width="100%"><tr>
<asp:Repeater ID="repAZlinks" runat="server">
<ItemTemplate>
	<td align="center"><a href="#<%# Eval("letter") %>" class="<%# (Eval("enabled")=="0"?"disabledletter":"letter") %>"><%# Eval("letter") %></a></td>
</ItemTemplate>
</asp:Repeater>
</tr></table>
<asp:Panel ID="pnlAZ" runat="server" />