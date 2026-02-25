<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BlogContent.ascx.cs" Inherits="Controls_Blog_BlogContent" %>

<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>--%>
        <table cellpadding="2" cellspacing="2" style="width:100%;">
            <tr>
                <td><asp:Label runat="server" ID="litTitle" CssClass="header" ></asp:Label></td>
            </tr>
            <tr><td><asp:Label runat="server" ID="litDate" CssClass="title" ></asp:Label></td></tr>
            <tr><td><asp:Label runat="server" ID="litContent" CssClass="bodytext" ></asp:Label></td></tr>
        </table>
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
