<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Breadcrumbs.ascx.cs" Inherits="Breadcrumbs" %>
<style type="text/css">
.breadcrumbs,  .breadcrumbs a, .breadcrumbs a:hover, .breadcrumbs td
{
	color: #000000;
}

.breadcrumbs a {
    padding-right: 30px;
}
.breadcrumbs img
{
	padding-left: 10px;
	padding-right: 10px;
}

.breadcrumbs a{
    position:relative;
}
.breadcrumbs a:after{
    font-family: "FontAwesome";
    content: "\f105";
    position: absolute;
    color: #AF1858;
    position: absolute;
    font-weight: bolder;
    color:    #000000;
    position: absolute;
    right: 11px;
    font-size: 16px;
}
.breadcrumbs tr td:last-child a{
}
    .breadcrumbs tr td:last-child a:after {
        display:none;
    }

.breadcrumbs .row div{
    display:table-cell;
}
</style>
<asp:Literal ID="litBreadCrumb" runat="server"></asp:Literal>
<asp:Literal ID="litBreadCrumb_Mobile" runat="server"></asp:Literal>
