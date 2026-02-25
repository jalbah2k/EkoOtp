<%@ Page Title="" Language="C#" MasterPageFile="~/InsidePreview.master" AutoEventWireup="true" CodeFile="Preview.aspx.cs" Inherits="Admin_eForms_Preview" %>
<%@ Register src="~/Controls/Menu/Menu.ascx" tagname="Menu" tagprefix="Hardcode" %>
<%@ Register src="~/Controls/Content/Content.ascx" tagname="Content" tagprefix="Hardcode" %>
<%@ Register src="~/Controls/eForm/eForm.ascx" tagname="eForm" tagprefix="Hardcode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<%--<asp:Content ID="Content4" ContentPlaceHolderID="MainMenu" Runat="Server">
    <Hardcode:Menu ID="Menu" runat="server" Parameter="1" />
</asp:Content>--%>
<asp:Content ContentPlaceHolderID="Content" Runat="Server">
    <Hardcode:eForm ID="eForm1" runat="server" />
</asp:Content>
