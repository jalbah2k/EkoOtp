<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PhotoNanogallery.ascx.cs" Inherits="PhotoNanogallery" %>
<%@ Reference Page="~/Default.aspx"  %>



<div class="photo-container">
    <div id="btnPhotoAdd" class="btnPhotoAdd widget_edit_btn" runat="server" visible="false"><asp:Literal ID="litBtnAddPhoto" runat="server"></asp:Literal></div>


     <div id="my_nanogallery2_<%=ContentId %>"></div>


    <div class="photo-album-title"><asp:Literal ID="lblTitle" runat="server" ></asp:Literal></div>

</div>

  <%--<script>
      jQuery(document).ready(function () {
          jQuery("#my_nanogallery2").nanogallery2({
              items: [
                  //// album 1
                  //{
                  //    src: 'berlin1.jpg',		     // image url
                  //    srct: 'berlin1t.jpg',	     // thumbnail url
                  //    title: 'Berlin - album 1',   // item title
                  //    ID: 1,                       // item ID
                  //    kind: 'album'                // item kind
                  //},
                  //{ src: 'berlin1.jpg', srct: 'berlin1t.jpg', title: 'Berlin 1', ID: 10, albumID: 1 },
                  //{ src: 'berlin2.jpg', srct: 'berlin2t.jpg', title: 'Berlin 2', ID: 11, albumID: 1 },

                  // album 2
                  //{ src: 'berlin2.jpg', srct: 'berlin2t.jpg', title: 'Berlin - album 2', ID: 2, kind: 'album' },
                  { src: 'berlin2.jpg', srct: 'berlin2t.jpg', title: 'Berlin', description: '1. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.' },
                  { src: 'berlin1.jpg', srct: 'berlin1t.jpg', title: 'Paris', description: '2. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.' },
                  { src: 'berlin3.jpg', srct: 'berlin3t.jpg', title: 'Madrid', description: '3. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.' },

                  //// top level images
                  //{ src: 'berlin2.jpg', srct: 'berlin2t.jpg', title: 'Berlin 2' },
                  //{ src: 'berlin3.jpg', srct: 'berlin3t.jpg', title: 'Berlin 3' }
              ],
              thumbnailWidth: 'auto',
              thumbnailHeight: 170,
              itemsBaseURL: 'https://nanogallery2.nanostudio.org/samples/',
              locationHash: false,
              viewerToolbar: {
                  display: true,
                  standard: 'minimizeButton, label',
                  minimized: 'minimizeButton, label, fullscreenButton, downloadButton, infoButton'
              },
              viewerTools: {
                  topLeft: 'pageCounter',
                  topRight: 'playPauseButton, zoomButton, fullscreenButton, closeButton'
              },
              thumbnailLabel: {
                  display: false
                  //valign: "bottom", position: 'overImage', hideIcons: true, displayDescription: false
              },
          });
      });
  </script>--%>