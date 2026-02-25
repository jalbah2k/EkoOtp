<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Video.ascx.cs" Inherits="Video" %>
<%@ Reference Control="~/Controls/VideoComments/VideoComments.ascx" %>
<link href="/controls/VideoComments/video.css" rel="stylesheet" />

<%--<script src="/controls/VideoComments/video.js"></script>--%>
<asp:Literal runat="server" ID="litScript"></asp:Literal>

<asp:Literal runat="server" ID="litTiltle"></asp:Literal>
<br />
<asp:Literal runat="server" ID="litVideo"></asp:Literal>
<%if (Session["LoggedInId"] != null)
    { %>
<br />
  <div id="myModal_Comment" class="modal myModal_Comment">

      <!-- Modal content -->
      <div class="modal-content">
          Comment / Reply <%--(<%=ConfigurationManager.AppSettings["Videos.Comment.MaxLength"] %> characters max)--%>:
          <input type="text"   id="txtNewComment" name="txtNewComment" class="textbox" maxlength="200" />
          <input type="hidden" id="txtVideoId"    />
          <input type="hidden" id="txtParentId"   />
          <input type="hidden" id="txtUId" value='<%=Session["LoggedInId"].ToString() %>' />
          <br />
          <div>
              <input type="button" value="Submit" id="btnSaveComment" class="button" />
              <input type="button" value="Cancel" id="btnCancelComment" class="button" />
          </div>
      </div>
    </div>
<%} %>

<asp:PlaceHolder ID="placeHolder1" runat="server"></asp:PlaceHolder>

