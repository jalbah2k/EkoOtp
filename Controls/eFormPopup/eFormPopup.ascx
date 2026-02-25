<%@ Control Language="C#" AutoEventWireup="true" CodeFile="eFormPopup.ascx.cs" Inherits="eFormPopup" %>
<%@ Reference Control="~/Controls/eForm/eForm.ascx"  %>
<asp:Panel ID="pnlPopupMessage" runat="server">
    <style type="text/css">
    #myModal {
        display: none; /* Hidden by default */
        position: fixed; /* Stay in place */
        z-index: 999999999999; /* Sit on top */
        left: 0;
        top: 0;      /*0;*/
        width: 100%; /* Full width */
        height: 100%; /* Full height */
        overflow: auto; /* Enable scroll if needed */
        background-color: rgb(255,255,255); /* Fallback color */
        background-color: rgba(255,255,255,.8); /* Black w/ opacity */
            backdrop-filter: blur(2px);
            cursor:not-allowed;
    }

    .efTable
    {
        position: relative;
    }
    .efTable>input
    {
        position: absolute!important;
        left: -30px;
    }


    input, /*.efTable input[type="radio"], .efTable input[type="checkbox"],*/ label, body .eforms-wrapper label, p, li, a, .eforms-wrapper label, .efPrompt, .efPrompt span, body .eforms-wrapper label
{
  position: relative!important;
}
    .efTable input[type="radio"]{
        margin-left:3px!important;
    }

    .efTable input[type="radio"], efTable input[type="checkbox"]
    {margin-left: 30px}
    .efPrompt>span, body .eforms-wrapper label[for="eForm1_ctl00_fld4354"] {
        color: var(--purple);
        font-size: 20px;
        font-weight: 600!important;
        line-height: 1;
    }

    .eforms-wrapper h2
    {
        text-align: center!important;
        color:var(--pink);
        margin-bottom:12px;
    }

/*    .eforms-wrapper h2:before, .eforms-wrapper h2:after, */
    .efPrompt>span:first-of-type:before, label[for="eForm1_ctl00_fld4354"]:before
    {
        content: "";
        display: inline-block;
        position: absolute;
        height: 20px;
        width: 30px;
        left:-30px;
    }

    .efPrompt>span[id*='rfv']
    {
        color: var(--pink)!important;
    }

    .efPrompt>span[id*='rfv']:after{
        color: #444;
        content: "Please provide an answer below:";
        font-size: 16px;
        margin-left: 6px;
    }
    

    .eforms-wrapper h2:before, .efPrompt>span:before, label[for="eForm1_ctl00_fld4354"]:before
    {
        background: url('/Images/darrow-right.svg') center no-repeat;
    }

    .eforms-wrapper h2:after
    {
        background: url('/Images/darrow-left.svg') center no-repeat;
    }

    .efPrompt>span, body .eforms-wrapper label[for="eForm1_ctl00_fld4354"]
    {
        display: block;
        margin-left:30px;
        position: relative;
    }

    .eforms-wrapper textarea
    {
        margin-top:20px!important;
        width: calc(100% - 48px)!important;
        margin-left: 24px;
    }
    body input[type="submit"]
    {
        width: calc(100% - 48px);
        margin-left: 24px;
    }

    body input[type="submit"]:hover
    {
        background-color: var(--pink)!important;
        border-color: var(--pink)!important;
        cursor: pointer;
    }

     /* Modal Content/Box */
    .modal-content.popup {
        background-color: #fff;
           padding: 20px 32px;
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
/*        top:-80px;*/
    }
    .eforms-wrapper
    {
        position: relative;
    }
    .eforms-wrapper:before
    {
        content:"";
        background:url('/Images/equals.png') center center no-repeat;
        height:50px;
        width:100px;
        position: absolute;
        left:-80px;
        top:-20px;
        display: block;
        background-size: contain;
    }
    fieldset {
        padding-left: 50px;
        padding-right: 50px;
        padding-top:14px;
    }

    #eform_1119 .bigP
        {
            font-size: 22px;
            text-align: center;
            line-height: 1;
            max-width: 700px;
            margin: 0 auto 60px auto;
        }
    </style>
    
    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div id="myinner" class="modal-content popup">
              <div>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#myModal').show();////.animate({ 'width': '100%', 'height': '100%' }, 300, 'easeOutBack');

            /*$('#myinner').css({ top: 1 * $('#myinner').height() / 2 });*/

        });
    </script>
   
</asp:Panel>

