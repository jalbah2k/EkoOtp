<%@ Control Language="C#" AutoEventWireup="true" CodeFile="oTreeMenuCollection.ascx.cs" Inherits="oTreeMenuCollection" %>
<%@ Reference Control="oTreeMenu.ascx" %>


<asp:PlaceHolder runat="server" ID="plcTreeViews" />
<script type="text/javascript">
    function ob_OnNodeDrop(src, dst, copy) {

        // add client side code here	
        //alert("Node with id:" + src + " was " + (!copy ? "moved" : "copied") + " to node with id:" + dst);

        PageMethods.MoveNode(dst, src, Response, Failure, TimeOut);

    }

    function ob_OnNodeSelect(id) {
        window.location.href = "admin.aspx?c=menu&m=" + id;
    }
    function Response(var1) {
        // alert(var1);
    }
    function Failure() {
        alert("Failure");
    }
    function TimeOut() {
        alert("TimeOut");
    }
</script> 


