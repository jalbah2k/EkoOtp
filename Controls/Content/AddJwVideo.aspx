<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddJwVideo.aspx.cs" Inherits="Controls_Content_AddJwVideo" %>

<%@ Register Assembly="Obout.Ajax.UI" Namespace="Obout.Ajax.UI.TreeView" TagPrefix="obout" %>
<%@ Register TagPrefix="ogrid" Namespace="Obout.Grid" Assembly="obout_Grid_NET" %>
<%@ Register TagPrefix="oajax" Namespace="OboutInc" Assembly="obout_AJAXPage" %>
<%@ Register TagPrefix="spl2" Namespace="OboutInc.Splitter2" Assembly="obout_Splitter2_Net" %>
<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
<base target= "_self" />
    <title>Insert JW Video</title>
    <script type="text/javascript" language="JavaScript">
        function SelectDir(dirID) {
            ob_post.AddParam("dirID", dirID);
            ob_post.post(null, "SelectDir", function () { });
        }
					
    </script>
    <style type="text/css">
        .tdText
        {
            font: 11px Verdana;
            color: #333333;
        }
        .option2
        {
            font: 11px Verdana;
            color: #0033cc;
            padding-left: 4px;
            padding-right: 4px;
        }
        a
        {
            font: 11px Verdana;
            color: #315686;
            text-decoration: underline;
        }
        
        a:hover
        {
            color: crimson;
        }
        
        .ob_spl_rightpanelcontent
        {
            position: relative;
        }
    </style>
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
    <script type="text/javascript">
        function ob_OnNodeSelect(sender, args) {
            var value = args.node.id;
            var parent = sender.getParentNode(args.node);
            while (parent != null) {
                value = parent.id + '/' + value;
                parent = sender.getParentNode(parent);
            }
            SelectDir(value);
            document.getElementById("hfPath").value = "/Uploads" + value;
            document.getElementById("lblCurrentDir").innerHTML = "/Uploads" + value;
            //alert(value);
        }

        function onClientSelect(arrSelectedRecords) {
            var sMessage="";
            for (var i = 0; i < arrSelectedRecords.length; i++) {
                var record = arrSelectedRecords[i];
                if (record.Type != "File Folder") {
                    //sMessage += (document.getElementById("hfPath").value + "/" + record.Name);
                    sMessage += (document.getElementById("lblCurrentDir").innerHTML + "/" + record.Name);
                    if (i < arrSelectedRecords.length - 1)
                        sMessage += ";";
                }
            }
            //alert(ObTree_selected_path);
            document.getElementById("txtPath").value = sMessage;
        }

    </script>
    <table border="0">
        <tr>
            <td valign="top" class="h5">
                <div style="font-family: Arial; color: #000000; font-size: 12px; font-weight: bold; margin-top: 3px; margin-bottom: 3px;"><asp:Label ID="lblCurrentDir" runat="server" Text="Label"></asp:Label></div>
                <div style="border: 1px solid gray; width: 700px; height: 370px;">
                    <div style="width: 700px; height: 370px;">
                        <spl2:Splitter ID="sp1" runat="server" StyleFolder="obout/Splitter/styles/default">
                            <LeftPanel WidthDefault="169" WidthMin="169" WidthMax="350">
                                <Header Height="30">
                                    <div style="padding-left: 10px; padding-top: 5px; padding-bottom: 5px; background-color: #C0C0C0"
                                        class="tdText">
                                        <b style="font-size: 12px">Folders</b>
                                    </div>
                                </Header>
                                <Content>
                                    <div style="padding-top: 7px; padding-left: 10px; border-top: 1px solid gray">
                                        <obout:Tree ID="ObTree" runat="server" OnNodeSelect="ob_OnNodeSelect">
                                        </obout:Tree>
                                    </div>
                                </Content>
                            </LeftPanel>
                            <RightPanel>
                                <Content>
                                    <div style="padding-top: 0px; padding-left: 0px;">
                                        <oajax:CallbackPanel ID="cpDir" runat="server">
                                            <Content>
                                                <ogrid:Grid ID="gridDir" runat="server" AllowRecordSelection="true" ShowFooter="true"
                                                    AllowPaging="false" AllowPageSizeSelection="false" KeepSelectedRecords="false"
                                                    AllowAddingRecords="false" CallbackMode="true" Serialize="true" AllowColumnResizing="true"
                                                    ShowHeader="true" PageSize="100" FolderStyle="obout/Grid/styles/premiere_blue" AutoGenerateColumns="false">
                                                    <ClientSideEvents OnClientSelect="onClientSelect" />
                                                    <Columns>
                                                        <ogrid:Column ID="Column1" DataField="imageType" HeaderText="" Align="center" Width="75"
                                                            runat="server">
                                                            <TemplateSettings TemplateId="tplImageType" />
                                                        </ogrid:Column>
                                                        <ogrid:Column ID="Column2" DataField="Name" HeaderText="Name" Width="120" runat="server" />
                                                        <ogrid:Column ID="Column3" DataField="Size" HeaderText="Size" Width="80" runat="server">
                                                            <TemplateSettings TemplateId="tplSize" />
                                                        </ogrid:Column>
                                                        <ogrid:Column ID="Column4" DataField="Type" HeaderText="Type" Width="83" runat="server" />
                                                        <ogrid:Column ID="Column5" DataField="DateModified" HeaderText="Date Modified" Width="167"
                                                            runat="server" />
                                                    </Columns>
                                                    <Templates>
                                                        <ogrid:GridTemplate runat="server" ID="tplImageType">
                                                            <Template>
                                                                <img src="obout/Grid/resources/images/filebrowser/<%# Container.Value %>.gif" />
                                                            </Template>
                                                        </ogrid:GridTemplate>
                                                        <ogrid:GridTemplate runat="server" ID="tplSize">
                                                            <Template>
                                                                <div style="width: 100%; height: 100%; padding-left: 10px; padding-top: 6px">
                                                                    <%# Container.Value == "0" ? "" : Container.Value + " KB" %>
                                                                </div>
                                                            </Template>
                                                        </ogrid:GridTemplate>
                                                    </Templates>
                                                </ogrid:Grid>
                                            </Content>
                                            <Loading>
                                                <table width="100%" height="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="center">
                                                            <img src="obout/Grid/resources/loading_icons/1.gif">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </Loading>
                                        </oajax:CallbackPanel>
                                        <oajax:CallbackPanel ID="cpLabel" runat="server">
                                            <Content>
                                                <asp:Label ID="lblDir" runat="server" />
                                            </Content>
                                        </oajax:CallbackPanel>
                                    </div>
                                </Content>
                            </RightPanel>
                        </spl2:Splitter>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <br />
    <span style="font-family: Arial; color: #000000; font-size: 12px;">Path: </span><asp:TextBox ID="txtPath" runat="server" Width="650px"></asp:TextBox><asp:HiddenField ID="hfPath" runat="server" Value="" />
    <br /><br />
    <table>
        <tr><td>Width:</td><td><asp:TextBox ID="txtWidth" runat="server" Text="650"></asp:TextBox></td><td>&nbsp;</td>
            <td rowspan="4" valign="top"><CuteWebUI:Uploader runat="server" ID="Uploader1" InsertText="Upload Multiple Documents" MultipleFilesUpload="true" OnFileUploaded="fileup" OnUploadCompleted="refresh"><ValidateOption AllowedFileExtensions="flv,swf,jpg,mp4" MaxSizeKB="20480" /></CuteWebUI:Uploader></td></tr>
        <tr><td>Height:</td><td><asp:TextBox ID="txtHeight" runat="server" Text="359"></asp:TextBox></td></tr>
        <tr><td>&nbsp;</td><td><asp:CheckBox ID="cbAutoplay" runat="server" Text="Autoplay" /></td></tr>
        <tr><td>&nbsp;</td><td><asp:CheckBox ID="cbFullScreen" runat="server" Text="Allow Fullscreen" Checked="True" /></td><%--<td>&nbsp;</td><td></td>--%></tr>
   </table>
   <br />
	<button onclick="button_click()" id="Button1">Insert</button>
	<%--<button onclick="window.top.close();" id="Button2">Close</button>--%>
    <br />
    </form>
	<script type="text/javascript" language="JavaScript">
	    function button_click() {
	        var editor = Window_GetDialogArguments(window);
	        //var ta = document.getElementById("ta");
	        var file = document.getElementById("txtPath");
	        if (file.value != "") {
	            var width = document.getElementById("txtWidth");
	            var height = document.getElementById("txtHeight");
	            var autoplay = document.getElementById("cbAutoplay");
	            var fullscreen = document.getElementById("cbFullScreen");
	            var content = "<div id=\"" + file.value + "\">Loading the player ...</div>";
	            content += "<div>";
	            content += "<script type=\"text/javascript\">";
	            content += "jwplayer(\"" + file.value + "\").setup({";
	            content += "flashplayer: \"/jwplayer/player.swf\",";
	            //content += "file: \"http://www.edencanada.ca/uploads/videos/Health_in_the_Workplace/CWGHR_1_EN_Final-Clip1.flv\",";
	            content += "file: \"" + file.value + "\",";
	            content += "width: " + width.value + ",";
	            content += "height: " + height.value + ",";
	            content += "autostart: " + autoplay.checked + ",";
	            content += "allowfullscreen: " + fullscreen.checked + ",";
	            content += "image: \"" + file.value.substring(0, file.value.length - 4) + ".jpg\"";
	            content += " });";
	            content += "<\/script>";
	            content += "<\/div>";

	            //editor.PasteHTML(ta.value);
	            editor.PasteHTML(content);
	        }
	    }
	    //Window_GetDialogArguments.js
	    function Window_GetDialogArguments(win) {
	        var top = win.top;
	        try {
	            var opener = top.opener;
	            if (opener && opener.document._dialog_arguments)
	                return opener.document._dialog_arguments;
	        }
	        catch (x) {
	        }
	        if (top.document._dialog_arguments)
	            return top.document._dialog_arguments;
	        if (top.dialogArguments)
	            return top.dialogArguments;
	        return top.document._dialog_arguments;
	    }
	</script>
</body>
</html>
