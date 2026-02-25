<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FolderContent.aspx.cs" Inherits="FolderContent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Folder List</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:BulletedList 
            ID="blFolders" 
            runat="server" 
            BulletStyle="NotSet">
        </asp:BulletedList>
    </form>
</body>
</html>
