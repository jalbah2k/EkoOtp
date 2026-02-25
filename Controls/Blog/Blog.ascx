<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Blog.ascx.cs" Inherits="Controls_Blog_Blog" %>
<%@ Register src="BlogList.ascx" tagname="BlogList" tagprefix="uc1" %>
<%@ Register src="BlogContent.ascx" tagname="BlogContent" tagprefix="uc2" %>
<%--<%@ Reference Control="~/Controls/Blog/BlogList.ascx" %>--%>
     
<table cellpadding="0" cellspacing="0" style="width:100%;">
    <tr>
        <td style=" vertical-align:top;">
            <uc2:BlogContent ID="BlogContent1" runat="server" />
        </td>
        <td style="width:200px;"><uc1:BlogList ID="BlogList1" runat="server" /></td>
    </tr>
</table>


