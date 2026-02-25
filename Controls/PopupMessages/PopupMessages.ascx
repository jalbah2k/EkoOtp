<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PopupMessages.ascx.cs" Inherits="Controls_PopupMessages_PopupMessages" %>
    <style type="text/css">
       /*#region Modal */

    /* The Modal (background) */
    .modal {
        display: none; /* Hidden by default */
        position: fixed; /* Stay in place */
        z-index: 9999; /* Sit on top */
        left: 0;
        top: 70px;      /*0;*/
        width: 100%; /* Full width */
        height: 100%; /* Full height */
        overflow: auto; /* Enable scroll if needed */
        background-color: rgb(0,0,0); /* Fallback color */
        background-color: rgba(30,30,30,.8); /* Black w/ opacity */
            backdrop-filter: blur(2px);
            cursor: pointer;
    }
    .modal h2
    {
            margin: 15px 0 5px;
    font-size: 22px;
    }
    .modal h2
    {
        font-size: 18px;
    }

    .modal p
    {
            font-size: 16px;
    margin-top: 5px;
    }
        


    /* Modal Content/Box */
    .modal-content {
        background-color: #fff;
           padding: 15px;
    border-radius: 15px;
        -webkit-box-shadow: 3px 3px 5px -1px rgba(0,0,0,0.3);
        -moz-box-shadow: 3px 3px 5px -1px rgba(0,0,0,0.3);
        box-shadow: 3px 3px 5px -1px rgba(0,0,0,0.3);
        border:1px solid #bbbbbb;
        margin: 15% auto; /* 5% from the top and centered */
        border: 1px solid #888;
        width:80vw;
        position: relative;
        cursor: initial!important;
        max-width:800px; /* Could be more or less, depending on screen size */
    }

    /*.modal-content video
    {
        width:100%;
        -webkit-box-shadow: 3px 3px 5px -1px rgba(0,0,0,0.3);
        -moz-box-shadow: 3px 3px 5px -1px rgba(0,0,0,0.3);
        box-shadow: 3px 3px 5px -1px rgba(0,0,0,0.3);
        border:1px solid #222;
        outline: none;
    }*/


    /* The Close Button */
    .close {
        color: #000;
        /* float: right; */
        position: absolute;
        top: 0px;
        right: 9px;
        font-size: 26px;
        font-weight: bold;
        font-family: "open sans";
        z-index: 9999;
        /*display:none;*/
    }

        .close:hover,
        .close:focus {
            text-decoration: none;
            cursor: pointer;
            font-size: 26px;
            /*top: -2.5px;
        right: 17.5px;*/
        }


    /*#endregion */
</style>

<asp:Panel ID="pnlPopupMessage" runat="server">
<div id="myModal" class="modal">

  <!-- Modal content -->
  <div class="modal-content popup">

    <asp:Literal ID="litContent" runat="server"></asp:Literal>
        <a href="javascript:void(0)" tabindex="<%=tabs %>" class="close">&times;</a>

    </div>
</div>
   
</asp:Panel>
<script>
        // Get the modal
        var modal = document.getElementById("myModal");

        <%--// Get the button that opens the modal
        var btn = document.getElementById('<%=btnSubmit.ClientID%>');--%>

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        ////// When the user clicks on the button, open the modal
        //btn.onclick = function () {
        //    if (typeof (Page_ClientValidate) == 'function') {
        //        if (Page_ClientValidate() == false) { return false; }
        //    }
        //    modal.style.display = "block";
        //}

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }

        $(document).ready(function () {
            modal.style.display = "block";

            try {
                $('.popup').fitVids();
            }
            catch (err) {

            }

        });
</script>
