<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BoardLogin.ascx.cs" Inherits="Controls_BoardLogin_BoardLogin" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<style>
    #footer input[type=text],#footer input[type=password]{
        padding:0;
        border-radius:0;
        height:26px;
        width:100px;
        background-color:#F1F1F1;
        border:none;
        padding-left:5px!important;
        color:#333333;
        font-family:Lato;
        font-size:12px;
        font-weight:normal;
        opacity:.8;
    }
    #footer button{
        background-color: transparent !important;
        padding: 0 !important;
        border: none !important;
        vertical-align:sub;
    }
    #footer .fa.fa-angle-right{
        color:#AF1858;
        font-size:22px;
    }
    #footer .fa.fa-angle-right ~ span{
        display:none;
    }
    @media (min-width: 1000px) {
        #footer input[type=text],#footer input[type=password]{
        padding:0;
        border-radius:0;
        height:26px;
        width:140px;
        background-color:#F1F1F1;
        border:none;
    }
    }
</style>
<script>
     <%--$(document).ready(function () {
         $('#<%=txtUsername.ClientID%>').focus(function () {
             if ($('#< %=txtUsername.ClientID%>').val() == "USERNAME")
                 $('#< %=txtUsername.ClientID%>').val('');
         });      
         $('#<%=txtUsername.ClientID%>').blur(function () {
             if ($('#< %=txtUsername.ClientID%>').val() == "")
                 $('#< %=txtUsername.ClientID%>').val('USERNAME');
         });
         $('#<%=txtPassword.ClientID%>').focus(function () {
             if ($('#< %=txtPassword.ClientID%>').val() == "PASSWORD")
                 $('#< %=txtPassword.ClientID%>').val('');
         });      
         $('#<%=txtPassword.ClientID%>').blur(function () {
             if ($('#< %=txtPassword.ClientID%>').val() == "")
                 $('#< %=txtPassword.ClientID%>').val('PASSWORD');
         });
     });--%>
</script>
<asp:Panel runat="server" DefaultButton="btnSubmit">
<a href="/boardportal">BOARD LOGIN</a>
&nbsp;&nbsp;&nbsp;
<label for="<%=txtUsername.ClientID %>" class="nosize">Username</label>
<asp:TextBox runat="server" ID="txtUsername"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtUsername" WatermarkText="USERNAME" WatermarkCssClass="watermarked" runat="server" ID="TitleWMExtender" ></ajaxToolkit:TextBoxWatermarkExtender>&nbsp;
<label for="<%=txtPassword.ClientID %>" class="nosize">Password</label>
<asp:TextBox runat="server" ID="txtPassword" TextMode="Password" ToolTip="Password"></asp:TextBox><ajaxToolkit:TextBoxWatermarkExtender TargetControlID="txtPassword" WatermarkText="PASSWORD" WatermarkCssClass="watermarked" runat="server" ID="TextBoxWatermarkExtender1" ></ajaxToolkit:TextBoxWatermarkExtender>&nbsp;
<button type="button" onclick="javascript: $('#<%=btnSubmit.ClientID %>').click();"><i class="fa fa-angle-right" aria-hidden="true"></i><span>Login</span></button>
<div style="display:none;"><asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Login" /></div>
</asp:Panel>