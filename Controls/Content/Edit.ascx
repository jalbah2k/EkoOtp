<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Editor" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>
    <script language="JavaScript" type="text/javascript">
        /*var editor1 = document.getElementById("%=Editor1.ClientID%>");

        function CuteEditor_OnCommand(editor, command, ui, value) {
            //handle the command by yourself
            alert(command);
            if (command == "MyCmd") {
                //editor.ExecCommand("youtube");
                //ShowMyDialog(this);
                alert("Test");
                return true;
            }
        }*/


        function ShowMyDialog(button) {
            //use CuteEditor_GetEditor(elementinsidetheEditor) to get the cute editor instance
            var editor = CuteEditor_GetEditor(button);
            //show the dialog page , and pass the editor as newwin.dialogArguments
            //(handler,url,args,feature)
            /*var newwin = editor.ShowDialog(null, "My_Custom_Text.html?_rand=" + new Date().getTime()
            , editor, "dialogWidth:400px;dialogHeight:240px");*/
            var newwin = editor.ShowDialog(null, "AddHtml5Video.aspx?_rand=" + new Date().getTime()
					, editor, "dialogWidth:750px;dialogHeight:600px");
        }
    </script>
<style>
.editor_button_top_off:hover 
{
    padding-top: 0!important;
}

</style>
<asp:Panel runat="server" ID="pnlEdit">
<asp:Button runat="server" Text="Live" cssclass="editor_button_top_on" ID="btnLive" OnClick="clickedLive" Font-Size="20px"/>&nbsp;<asp:Button ID="btnPending" runat="server" Text="Pending" cssclass="editor_button_top_off" OnClick="clickedPending" Font-Size="20px"/>&nbsp;<asp:Button ID="btnMessages" runat="server" Text="Messages" cssclass="editor_button_top_off" OnClick="clickedMessages" Font-Size="20px"/><br />
    <CE:Editor  CssClass="bodytext" id="Editor1" AutoParseClasses="false" runat="server" Width="950" height="500"/><br />
    <asp:Button ID="btnSave" runat="server" OnClick="clickSave" Text="Save" style="border:1px #7e9db9 solid;"/>&nbsp;&nbsp;<asp:Button ID="btnNotice" runat="server" OnClick="clickedNotice" Text="Send Request for Approval" style="border:1px #7e9db9 solid;background-color:#d7e3f2;"/>&nbsp;&nbsp;<asp:Button ID="btnApprove" runat="server" OnClick="clickedApprove" Text="Approve" style="border:1px #7e9db9 solid;"/>&nbsp;&nbsp;&nbsp;
    <%--<input type="button" value="Preview" runat="server" id="btnPreview" style="border:1px #7e9db9 solid;background-color:#d7e3f2;"  />--%>
    <asp:Button ID="btnPreview" runat="server" Text="Preview" 
        style="border:1px #7e9db9 solid;" 
        onclick="btnPreview_Click"/>
    <div id="messagearea" runat="server" visible="false"><asp:TextBox ID="txtMessages" ReadOnly="true" TextMode="MultiLine" runat="server" Width="99%" Height="300" style="font-family:Arial;font-size:12px;" ValidationGroup="msg"/><br /><br />Message:&nbsp;<asp:TextBox ID="txtMessage" runat="server" Width="400"  ValidationGroup="msg"/>&nbsp;<asp:Button Text="Send Message" ID="btnSendMessage" runat="server" Onclick="clickedSendMessage" ValidationGroup="msg" style="border:1px #7e9db9 solid;background-color:#d7e3f2;"/><br /><br /><asp:Label ID="lblSub" runat="server"  style="font-size:12px;font-family:arial;"/>&nbsp;<asp:Button ID="btnSubscribe" Text="Un/Subscribe" runat="server" onclick="clickedSub" style="font-size:12px;font-family:arial;border:1px #7e9db9 solid;background-color:#d7e3f2;"/></div></asp:Panel>
<asp:Panel ID="pnlLogin" runat="server" Visible="false"><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td style="width:100%; padding-top:100px;" align="center"><center><div style="text-align:left; margin:0 auto; width:420px;"><table border="0" cellpadding="0" cellspacing="0"><tr><td style="font-family:Arial; font-size:16px; color:#4097ff;"><span style="display:none;"><img src="/images/kids/kids_logo.jpg" /></span><br /><br /><span style="font-size:20px;">Oops!</span> Looks like you have been logged out.<br /><span style="color:#949494;">Please sign in below to save your changes.</span><br /> <br /></td></tr><tr><td><table cellpadding="4" border="0"><tr><td valign="middle"><asp:Label ID="lblUsername" runat="server" Text="Username:" AssociatedControlID="txtUsername" /></td><td><asp:TextBox ID="txtUsername" runat="server" style="margin:0px; width: 200px;" /></td></tr><tr><td valign="middle"><asp:Label ID="Label1" runat="server" Text="Password:" AssociatedControlID="txtPassword" /></td><td><asp:TextBox ID="txtPassword" TextMode="Password" runat="server" style="margin:0px; width: 200px;" /></td></tr><tr><td></td><td><asp:Button ID="btnSubmit" runat="server" CssClass="button-primary" OnClick="login" Text="Login" /></td></tr></table></td></tr><tr><td align="right"></td></tr></table></div></center></td></tr></table></asp:Panel>
<%--<script>
    $(document).ready(function () {

        $("#< %=btnPreview.ClientID %>").click(function () {
        <%if (Language == "1") {%>
            window.open('/en/< %=myseo%>?pv=1');
        <%}else{ %>
            window.open('/fr/< %=myseo%>?pv=1');
        <%} %>
        });
    });
    </script>--%>