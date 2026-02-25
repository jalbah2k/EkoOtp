<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Login.ascx.cs" Inherits="Controls_Login_Login" %>
 <style type="text/css">
        #box_login
        {
	        position:relative;
	        display:none;
        }
        #box_login input
        {
            margin: 0px;
        }
        body, html
        {
	        height:100%;
        }
        div#box_login > table{
            width: 100%; padding: 0px; margin:20px;

        }

        h1
        {
            color:var(--blue);
            margin:0 20px;
        }
        div#box_login > table tr:first-child td{
            padding-top: 13px;
        }
        .login-container{
            border:none;
            padding: 0px;
            width:440px;
            display:table;
        }
     
         div.logo{
             left:0;
         }
         .login-password input, .login-username input
         {
            width: 100%;
         }
         .login-username, .login-password
         {
             display:table-row;
         }
         .login-username > div, .login-password > div{
             display:table-cell;
         }

    .login-container h2{
        /*line-height: 1.4;*/
        font-size: 16px;
        color: var(--darkgrey);
        font-weight: 300;
    }

    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#box_login').slideDown('slow');
            setTimeout(function () { $('#<%= txtUsername.ClientID %>').focus(); }, 1000);
        });
    </script>

   <asp:Panel runat="server" ID="pnlLogin" DefaultButton="Button1">
    <div>
        <div id="box_login">
             
            <div>
               
                <div>
                    <div >
                            <a name="content"></a>
                            <div class="login-container">
                                <div class="login-username">
                                    <div class="text-right">
                                        <h2><asp:Label ID="Label1" runat="server" Text="User name" AssociatedControlID="txtUsername" /></h2></div>
                                    <div>
                                        <asp:TextBox ID="txtUsername" title="username" runat="server" /></div>
                                </div>
                                <br />
                                <div class="login-password">
                                    <div class="padding-top-20 text-right">
                                        <h2><asp:Label ID="Label2" runat="server" Text="Password" AssociatedControlID="txtPassword" /></h2></div>
                                    <div class="padding-top-20">
                                        <asp:TextBox ID="txtPassword" title="password" runat="server" TextMode="Password" /></div>
                                </div>
                                <div>
                                  
                                
                                        <asp:Button ID="Button1" runat="server" OnClick="ClickLogin" Text="Login" CssClass="button-primary" />
                                 
                                </div>
                            </div>
                     
                    </div>
                </div>
                <div class="login-error">
                    <div >
                        
                            <asp:Literal ID="literr" runat="server" />
                    </div>
                </div>
                <div style="display:none;">
                    <div>
                        <div class="forgot-change-pwd-container">
                            <a class="bodytext" href="/forgotpassword" target="_blank">
                                <span>Forgot password?</span>
                            </a>&nbsp;&nbsp;<a class="bodytext" href="/changepassword" target="_self">
                                <span>Change password</span></a>
                        </div>
                       <%-- <div class="remember-pwd-conatiner">
                            <span>
                                <label for="cbRem">Remember me</label>
                            </span>&nbsp;&nbsp;<asp:CheckBox ID="cbRem" runat="server" />
                        </div>--%>
                    </div>
                </div>
                <%--<tr><td><asp:CheckBox ID="cbRem" runat="server" /></td></tr>--%>
            </div>
        </div>
    </div>
</asp:Panel>