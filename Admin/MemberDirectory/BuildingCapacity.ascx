<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BuildingCapacity.ascx.cs" Inherits="Admin_MemberDirectory_BuildingCapacity" %>
<%@ Register Src="~/Admin/MemberDirectory/ListAndDetails.ascx" TagPrefix="uc1" TagName="ListAndDetails" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<style>
    i{
        font-size:12px;
    }
</style>

<div class="admin-header-wrapper noprint">
    <div class="admin-header">Building Capacity Partners</div>
    <div class="admin-header-subtitle">This is a list of Building Capacity partner organizations, where you can edit or delete them.</div>
</div>


<uc1:ListAndDetails runat="server" ID="ListAndDetails" OrganizationType="2" />
