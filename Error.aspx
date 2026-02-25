<%@ Page Title="" Language="C#" MasterPageFile="~/Error.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<%@ Register Src="~/Controls/Content/Content.ascx" TagPrefix="uc1" TagName="Content" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Content" Runat="Server">
    <uc1:Content runat="server" ID="Content" Parameters="5606" />
</asp:Content>

