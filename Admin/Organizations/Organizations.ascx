<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Organizations.ascx.cs" Inherits="Organizations" %>
<%@ Register Src="~/Admin/Organizations/ListAndDetails.ascx" TagPrefix="uc1" TagName="ListAndDetails" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<style>
    i{
        font-size:12px;
    }
</style>

<div class="admin-header-wrapper noprint">
    <div class="admin-header">Organizations</div>
    <div class="admin-header-subtitle">This is a list of Organizations, where you can edit or delete them.</div>
</div>

<uc1:ListAndDetails runat="server" ID="ListAndDetails" OrganizationType="1" />

