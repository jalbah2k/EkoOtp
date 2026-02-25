<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MemberDirectory.ascx.cs" Inherits="MemberDirectory" %>
<%@ Register Src="~/Admin/MemberDirectory_PNCA/ListAndDetails.ascx" TagPrefix="uc1" TagName="ListAndDetails" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<style>
    i{
        font-size:12px;
    }
</style>

<div class="admin-header-wrapper noprint">
    <div class="admin-header">Member Directory</div>
    <div class="admin-header-subtitle">This is a list of PNCA Mmembers of Directory, where you can edit or delete them.</div>
</div>

<uc1:ListAndDetails runat="server" ID="ListAndDetails" OrganizationType="1" />

