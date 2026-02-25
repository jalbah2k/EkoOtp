<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PagesNoUpdated.ascx.cs" Inherits="Admin_Reports_PagesNoUpdated" %>
<%@ Register src="PagesCore.ascx" tagname="PagesCore" tagprefix="uc1" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />

<div class="admin-header-wrapper noprint">
    <div class="admin-header">Pages</div>
    <div class="admin-header-subtitle">This is a list of not updated pages.</div>
</div>
<uc1:PagesCore ID="PagesCore1" runat="server" IsUpdated="false" />

