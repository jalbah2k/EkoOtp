<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="ReindexResources.aspx.cs" Inherits="Admin_ReindexResources" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Reindex Resource Embeddings</title></head>
<body>
  <form id="form1" runat="server">
    <asp:Button ID="btnRun" runat="server" Text="Run Resource Indexer"
        OnClick="btnRun_Click" />
    <asp:Literal ID="litResult" runat="server" />
  </form>
</body>
</html>