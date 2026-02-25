<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WidgetsToolbar.ascx.cs" Inherits="Controls_WidgetsToolbar_WidgetsToolbar" %>
<%@ Register tagprefix="Hardcode" src="BannerGalleryManager.ascx" tagname="BannerGalleryManager" %>
<%@ Register tagprefix="Hardcode" src="PhotoGalleryAddDnD.ascx" tagname="PhotoGalleryAdd" %>
<%@ Register tagprefix="Hardcode" src="DocumentsManager.ascx" tagname="DocumentsManager" %>
<%@ Register tagprefix="Hardcode" src="PopupMessagesManager.ascx" tagname="PopupMessagesManager" %>
<%@ Register tagprefix="Hardcode" src="ContentAdd.ascx" tagname="ContentAdd" %>
<style type="text/css">
    .pnlWidgetToolbar
    {
        /*position:relative;*/
        /*width:75px;*/
        position:fixed!important;
        top:95px;
        right:10px;
        width:40px;
        border:2px solid #000000;
        background-color:#d7e3f2;
        min-height:370px;
        z-index:9999999999;
        height:370px;
    }
    .pnlWidgetToolbar .pnlCaption
    {
        position:relative;
        height:18px;
        /*padding-top:2px;
        padding-bottom:2px;*/
        padding:2px;
        border-bottom:2px solid #000000;
        background-color:#0078d8;
	    /*color: #004075;*/
	    color: #FFFFFF;
	    font-size: 12px;
	    font-weight: bold;
	    font-family:Arial;
	    vertical-align:middle;
        cursor: move;
        cursor:hand;
        cursor:grab;
        cursor:-moz-grab;
        cursor:-webkit-grab;
    }
    .pnlWidgetToolbar .pnlCaption.custom_cursor
    {
        /*cursor: url(https://mail.google.com/mail/images/2/openhand.cur), move;*/
        cursor: url(/images/openhand.cur), move;
    }
    .pnlWidgetToolbar .WidgetToolbarContent
    {
        position:relative;
        /*padding:5px;
        overflow:auto;*/
    }
    .pnlWidgetToolbar .WidgetToolbarContent div.WidgetDD
    {
        position:relative;
        width:36px;
        height:20px;
        padding:2px;
        margin:0px auto;
        margin-top:5px;
        background: #d7e3f2 url('../../images/lemonaid/icons/dd-arrow.png') no-repeat right center;
        -pie-background: url('../../images/lemonaid/icons/dd-arrow.png') no-repeat right center, #d7e3f2 linear-gradient(to bottom, #d7e3f2, #d7e3f2); /*IE 6-9 via PIE*/
        cursor:pointer;
        behavior: url(/js/PIE-2.0beta1/PIE.htc);
    }
    .pnlWidgetToolbar .WidgetToolbarContent div.WidgetDD:hover
    {
        background: #d7e3f2; /* Old browsers */
        background: url('../../images/lemonaid/icons/dd-arrow.png') no-repeat right center, #d7e3f2 -moz-linear-gradient(top, rgba(255,255,255,.3) 0%, rgba(0,0,0,.3) 100%); /* FF3.6+ */
        background: url('../../images/lemonaid/icons/dd-arrow.png') no-repeat right center, #d7e3f2 -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(255,255,255,.3)), color-stop(100%,rgba(0,0,0,.3))); /* Chrome,Safari4+ */
        background: url('../../images/lemonaid/icons/dd-arrow.png') no-repeat right center, #d7e3f2 -webkit-linear-gradient(top, rgba(255,255,255,.3) 0%,rgba(0,0,0,.3) 100%); /* Chrome10+,Safari5.1+ */
        background: url('../../images/lemonaid/icons/dd-arrow.png') no-repeat right center, #d7e3f2 -o-linear-gradient(top, rgba(255,255,255,.3) 0%,rgba(0,0,0,.3) 100%); /* Opera11.10+ */
        background: url('../../images/lemonaid/icons/dd-arrow.png') no-repeat right center, #d7e3f2 -ms-linear-gradient(top, rgba(255,255,255,.3) 0%,rgba(0,0,0,.3) 100%); /* IE10+ */
        background: url('../../images/lemonaid/icons/dd-arrow.png') no-repeat right center, #d7e3f2 linear-gradient(top, rgba(255,255,255,.3) 0%,rgba(0,0,0,.3) 100%); /* W3C */
        -pie-background: url('../../images/lemonaid/icons/dd-arrow.png') no-repeat right center, #d7e3f2 linear-gradient(to bottom, rgba(255,255,255,.3), rgba(0,0,0,.3)); /*IE 6-9 via PIE*/
	}
    .pnlWidgetToolbar .WidgetToolbarContent div.WidgetDD ul
    {
        width: 245px;
        /*height:300px;
        min-height:100px;*/
        max-height:300px;
        overflow:auto!important;
        display: none;
        position: absolute;
        right: 0px;
        top: 24px;
        z-index: 9999999999;
        padding: 0px 0px 0px 0px;
        margin: 0px 0px 0px 0px;
        list-style: none;
        border: 1px solid #aaa;
        /*border-top: 0px;*/
        background-color: #ffffff;
    }
    .pnlWidgetToolbar .WidgetToolbarContent div.WidgetDD ul li
    {
        margin: 0px;
        padding: 2px 5px 2px 5px;
        background: none;
    }

    .pnlWidgetToolbar .WidgetToolbarContent div.WidgetDD ul li.WidgetItem:hover{
	    background-color: #0078d8!important;
	    color: #ffffff;
    }	
    .pnlWidgetToolbar .WidgetToolbarContent div.WidgetDD .WidgetItem
    {
        cursor: move;
        cursor:hand;
        cursor:grab;
        cursor:-moz-grab;
        cursor:-webkit-grab;
        display:flex;
    }
    .pnlWidgetToolbar .WidgetToolbarContent div.WidgetDD .WidgetItem.custom_cursor
    {
        /*cursor: url(https://mail.google.com/mail/images/2/openhand.cur), move;*/
        cursor: url(/images/openhand.cur), move;
    }
    .pnlWidgetToolbar .WidgetToolbarContent div.WidgetDD div.WidgetItem
    {
        width:20px;
    }
    .pnlWidgetToolbar .pnlRemoveWidget
    {
        position:absolute;
        left:0px;
        bottom:4px;
        text-align:center;
        width:100%;
        cursor:not-allowed;
    }
    .pnlWidgetToolbar .pnlRemoveWidget #remove_widget
    {
        filter: alpha(opacity=50);
        -moz-opacity: 0.5;
        -khtml-opacity: 0.5;
        opacity: 0.5;
    }
    .pnlWidgetToolbar .pnlRemoveWidget #remove_widget.active
    {
        filter: alpha(opacity=100);
        -moz-opacity: 1;
        -khtml-opacity: 1;
        opacity: 1;
    }
    .remove_widget_hover
    {
        background-color:#d7e3f2;
        -webkit-box-shadow:0px 0px 2px #000000; 
        -moz-box-shadow:0px 0px 2px #000000;
        box-shadow:0px 0px 2px #000000;
        behavior: url(/js/PIE-2.0beta1/PIE.htc);
        border-collapse:separate;   /* For IE 9 */
    }
    .DroppableContent
    {
        position:relative;
        min-width:25px;
        min-height:20px;
        z-index:inherit;
    }
    .DroppableContent .ui-state-highlight
    {
        list-style:none;
		background-color:#ff0000;
		display:block;
    }
    .DroppableContent img.ui-state-highlight
    {
        display:block;
    }
    .hide-placeholder
    {
        /*display:none;*/
        width:0px !important;
        height:0px !important;
    }
    .Draggable_Widget
    {
        position:relative;
    }
    [id^='widget_photos'].Draggable_Widget
    {
        display:inline-block;
    }
    .Draggable_Widget .widget_handle
    {
        position:absolute;
        top:0px;
        left:-20px;
        /*width:70px;
        height:32px;*/
        width:84px;
        height:75px;

        background:transparent url(../../images/lemonaid/partials/drag-handle-blue.png) no-repeat left center;

        /* IE hack */
        background:none\9; /* Targets IE only */
        filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src="/images/lemonaid/partials/drag-handle-blue.png", sizingMethod="crop"), alpha(opacity=50);

        /*filter: alpha(opacity=50);*/
        -moz-opacity: 0.5;
        -khtml-opacity: 0.5;
        opacity: 0.5;
        
        cursor: move;
        cursor:hand;
        cursor:grab;
        cursor:-moz-grab;
        cursor:-webkit-grab;
        
        display:block !important;
        z-index:101 !important;
    }
    .Draggable_Widget .widget_handle:hover
    {
        filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src="/images/lemonaid/partials/drag-handle-blue.png", sizingMethod="crop"), alpha(opacity=100);
        /*filter: alpha(opacity=100);*/
        -moz-opacity: 1;
        -khtml-opacity: 1;
        opacity: 1;
    }
    .Draggable_Widget .widget_handle img
    {
        position:absolute;
        left:50px;
        top:10px;
        width: 18px;
    }
    .Draggable_Widget .widget_handle.custom_cursor
    {
        cursor: url(/images/openhand.cur), move;
        /*cursor: url(https://mail.google.com/mail/images/2/openhand.cur), move;*/
    }
    .WidgetItem
    {
        list-style:none;
    }
    .widget_edit_btn
    {
        display:block !important;
    }
    .goBehind
    {
        z-index:1 !important;
    }
</style>
<Hardcode:BannerGalleryManager ID="BannerGalleryManager1" runat="server" />
<Hardcode:PhotoGalleryAdd ID="PhotoGalleryAdd1" runat="server" />
<Hardcode:DocumentsManager ID="DocumentsManager1" runat="server" />
<Hardcode:ContentAdd ID="ContentAdd1" runat="server" />
<Hardcode:PopupMessagesManager ID="PopupMessagesManager1" runat="server" />
<asp:Panel ID="pnlWidgetToolbar" CssClass="pnlWidgetToolbar" runat="server">
    <asp:Panel ID="pnlCaption" CssClass="pnlCaption" runat="server">
        <div class="dragMe">
            <img src="/images/lemonaid/menuicons/widgets_18x18.png" alt="Widgets" title="Widgets" align="middle" />
        </div>
    </asp:Panel>  
    <div class="WidgetToolbarContent">  
        <div class="WidgetDD">
            <div id="content_new" class="WidgetItem"><img src="/images/lemonaid/menuicons/pages.png" class="widget_handle_icon" alt="content" title="Add Content" /></div>
            <ul>
                <asp:Repeater ID="repContent" runat="server" onitemdatabound="WidgetItemDataBound">
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div class="WidgetDD">
            <div id="banners_new" class="WidgetItem"><img src="/images/lemonaid/menuicons/bannergallery.png" class="widget_handle_icon" alt="bannergallery" title="Add Banner Gallery" /></div>
            <ul>
                <asp:Repeater ID="repBannerGallery" runat="server" onitemdatabound="WidgetItemDataBound">
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <asp:Panel ID="pnlPhotoGallery" class="WidgetDD" runat="server">
            <div id="photos_new" class="WidgetItem"><img src="/images/lemonaid/menuicons/photogallery.png" class="widget_handle_icon" alt="photogallery" title="Add Photo Gallery" /></div>
            <ul>
                <asp:Repeater ID="repPhotoGallery" runat="server" onitemdatabound="WidgetItemDataBound">
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:Repeater>
            </ul>
        </asp:Panel>
        <asp:Panel ID="pnlDocuments" class="WidgetDD" runat="server">
            <div id="documents_new" class="WidgetItem"><img src="/images/lemonaid/menuicons/documents.png" class="widget_handle_icon" alt="documents" title="Add Documents" /></div>
            <ul>
                <asp:Repeater ID="repDocuments" runat="server" onitemdatabound="WidgetItemDataBound">
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:Repeater>
            </ul>
        </asp:Panel>
        <asp:Panel ID="pnleForm" class="WidgetDD" runat="server">
            <div id="eform_new"><img src="/images/lemonaid/menuicons/eforms.png" class="widget_handle_icon" alt="eform icon" title="Add eForm" /></div>
            <ul>
                <asp:Repeater ID="repeForm" runat="server" onitemdatabound="WidgetItemDataBound">
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:Repeater>
            </ul>
        </asp:Panel>
        <asp:Panel ID="pnlPopupMessages" class="WidgetDD" runat="server">
            <div id="popupmessages_new" class="WidgetItem"><img src="/images/lemonaid/menuicons/sharepage.png" class="widget_handle_icon" alt="popupmessages" title="Add Popup Message" /></div>
            <ul>
                <asp:Repeater ID="repPopupMessages" runat="server" onitemdatabound="WidgetItemDataBound">
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:Repeater>
            </ul>
        </asp:Panel>
        <asp:Panel ID="pnlMenu" class="WidgetDD" runat="server">
            <div id="menu_new"><img src="/images/lemonaid/menuicons/menu.png" class="widget_handle_icon" alt="menu icon" title="Add menu" /></div>
            <ul>
                <li style="font-weight:bold; cursor:default;">Horizontal Menus</li>
                <asp:Repeater ID="repMenuH" runat="server" onitemdatabound="WidgetItemDataBound">
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:Repeater>
                <li style="font-weight:bold; cursor:default;">Vertical Menus</li>
                <asp:Repeater ID="repMenuV" runat="server" onitemdatabound="WidgetItemDataBound">
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:Repeater>
            </ul>
        </asp:Panel>
        <asp:Panel ID="pnlNews" class="WidgetDD" runat="server">
            <div id="newsroom" class="WidgetItem"><img src="/images/lemonaid/menuicons/newsroom.png" class="widget_handle_icon" alt="news icon" title="Add News" /></div>
            <ul>
                <asp:Repeater ID="repNews" runat="server" onitemdatabound="WidgetItemDataBound">
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:Repeater>
            </ul>
        </asp:Panel>
         <div class="WidgetDD">
            <div id="template_new" class="WidgetItem"><img src="/images/lemonaid/menuicons/pages.png" class="widget_handle_icon" alt="content" title="Add Content Row" /></div>
            <ul>
                <asp:Repeater ID="repTemplate" runat="server" onitemdatabound="WidgetItemDataBound">
                <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server"></asp:Literal>
                </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>  
    <div class="pnlRemoveWidget">
        <hr />
        <div><img id="remove_widget" src="/images/lemonaid/icons/delete.png" alt="remove widget" title="Remove Widget" align="middle" /></div>
    </div>
</asp:Panel>
<script type="text/javascript">
(function ($) {
    // Fix for overlapping sortables
    $.widget("ui.sortable", $.extend({},
        $.ui.sortable.prototype, {

            _createOrig: $.ui.sortable.prototype._create,
            _create: function () {
                var result = this._createOrig();
                this.containerCache.sortable = this;
                return result;
            },
            
            _intersectsWithPointerOrig: $.ui.sortable.prototype._intersectsWithPointer,
            _intersectsWithPointer: function (item) {
                //This line....
                if (item.instance.element.hasClass("DC_Banner") && this.positionAbs.top + this.offset.click.top > 380) {
                    return false;
                }
                return this._intersectsWithPointerOrig(item);
            },
            
            _intersectsWithOrig: $.ui.sortable.prototype._intersectsWith,
            _intersectsWith: function (containerCache) {
                //Also this line....
                if (containerCache.sortable.element.hasClass("DC_Banner") && this.positionAbs.top + this.offset.click.top > 380) {
                    return false;
                }
                return this._intersectsWithOrig(containerCache);
            }

        }));

})(jQuery);
</script>
<script type="text/javascript">
    var CancelSorting = false;

    function ie_ver() {
        var iev = 0;
        var ieold = (/MSIE (\d+\.\d+);/.test(navigator.userAgent));
        var trident = !!navigator.userAgent.match(/Trident\/7.0/);
        var rv = navigator.userAgent.indexOf("rv:11.0");

        if (ieold) iev = new Number(RegExp.$1);
        if (navigator.appVersion.indexOf("MSIE 10") != -1) iev = 10;
        if (trident && rv != -1) iev = 11;

        return iev;
    }
    var IEVersion = ie_ver();

    function WidgetToolbarOnDrop() {
        var WindowsWidth = $(window).width();
        var TBPos = parseInt($('.pnlWidgetToolbar').css('left').replace('px', ''));

        if (WindowsWidth - TBPos < 265) {
            $('.WidgetDD ul').css({ left: '', right: '0px' });
        }
        else {
            $('.WidgetDD ul').css({ left: '0px', right: '' });
        }
    }

    function BindWTBControlEvents() {
        $(window).resize(function () {
            WidgetToolbarOnDrop();
        });

    }

    //Initial bind
    $(document).ready(function () {
        BindWTBControlEvents();

        $('#<%= pnlWidgetToolbar.ClientID %>').draggable({
            addClasses: false,
            handle: ".pnlCaption",
            cursor: "grabbing",
            cursorAt: { top: -5 },
            zIndex: 9999999999,
            stop: function (event, ui) {
                WidgetToolbarOnDrop();
            }
        });

        //Check if browser is IE
        if (IEVersion > 0) {
            $('.pnlCaption').addClass('custom_cursor');
            $('.WidgetItem').addClass('custom_cursor');
            $('.widget_handle').addClass('custom_cursor');
        }

        $('.WidgetDD').each(function () {
            var icon = $(this).find('img:first-child').attr('src');
            $(this).find('li').each(function () {
                $(this).find('img').attr('src', icon);
            });
        });

        $('.WidgetDD').click(function () {
            $('.WidgetDD').not(this).children('ul:visible').each(function () {
                $(this).hide();
            });

            //$(this).find('ul').slideToggle(400);
            var ul = $(this).find('ul');
            if (ul.children('li').length > 0)
                ul.slideToggle(400);

            return false;
        });

        $('.WidgetDD .WidgetItem').draggable({
            addClasses: false,
            cursor: "grabbing",
            cursorAt: { top: 10, left: 10 },
            helper: "clone",
            appendTo: "body",
            connectToSortable: ".DroppableContent",
            zIndex: 99999999999,
            start: function (event, ui) {
                ui.helper.css("list-style", "none");
                $('.WidgetDD').find('ul').slideUp(400);
            }
        });

        $('.WidgetDD [id^="menuh"].WidgetItem').draggable("option", "connectToSortable", ".DC_MainMenu");
        $('.WidgetDD [id^="menuv"].WidgetItem').draggable("option", "connectToSortable", ".DC_LeftMenu");

        var DraggingCursor = null;
        //Check if browser is IE
        if (IEVersion > 0) {
            //DraggingCursor = "url(https://mail.google.com/mail/images/2/closedhand.cur), move";
            DraggingCursor = "url(/images/closedhand.cur), move";
        }
        //Check if browser is Chrome or not
        else if (navigator.userAgent.search("Chrome") >= 0 || navigator.userAgent.search("Safari") >= 0) {
            DraggingCursor = "-webkit-grabbing";
        }
        //Check if browser is Opera or not
        else if (navigator.userAgent.search("Opera") >= 0) {
            DraggingCursor = "move";
        }
        if (DraggingCursor != null) {
            $('#<%= pnlWidgetToolbar.ClientID %>').draggable("option", "cursor", DraggingCursor);
            $('.WidgetDD .WidgetItem').each(function () {
                $(this).draggable("option", "cursor", DraggingCursor);
            });
        }

        $('#remove_widget').droppable({
            accept: ".Draggable_Widget, .WidgetItem",
            hoverClass: "remove_widget_hover",
            tolerance: "pointer",
            greedy: true,
            drop: function (event, ui) {
                if (ui.draggable.hasClass('Draggable_Widget')) {
                    var item = ui.draggable.attr('id').replace(/#widget_/gi, "").split("_");
                    var widget = item[1];
                    var id = item[2];

                    var ZoneId = ui.draggable.attr('ZoneId');

                    if (confirm('Are you sure you want to remove this widget?')) {
                        $(".DroppableContent").sortable("option", "revert", false);

                        ui.draggable.stop().effect("explode", { pieces: 9 }, 1000);
                        ui.draggable.hide();
                        $.ajax({
                            type: "POST",
                            url: "/Default.aspx/RemoveWidgetFromPage",
                            data: JSON.stringify({ 'PageId': '<%= Session["PageID"] %>', 'ZoneId': ZoneId, 'Control': widget, 'WidgetId': id }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                if (response.d != "")
                                    alert(response.d);
                                ui.draggable.remove();
                            },
                            error: function (xhr, status, errorThrown) {
                                ui.draggable.show();
                                alert(status + " | " + xhr.responseText);
                            }
                        });
                    }
                }

                CancelSorting = true;

                //return false;
            },
            over: function (event, ui) {
                if (ui.draggable.hasClass('WidgetItem')) {
                    ui.helper.css({ "cursor": "not-allowed" });
                }
            },
            out: function (event, ui) {
                if (ui.draggable.hasClass('WidgetItem')) {
                    ui.helper.css({ "cursor": "inherit" });
                }
            }
        });

        $('.DroppableContent').sortable({
            items: "> .Draggable_Widget"
            , cursor: 'grabbing'
            , handle: '.widget_handle'
            //, cancel: 'input,textarea,button,select,option'
            , revert: true
            , tolerance: 'pointer'
            , cursorAt: { top: 1 }
            , placeholder: "ui-state-highlight"
            , forcePlaceholderSize: true
            , zIndex: 99999999999
            , update: function (e, ui) {
                if (ui.item.attr('id') != null && ui.item.attr('id') != 'undefined' && ui.item.attr('id') != '' && !CancelSorting) {
                    var ZoneId = ui.item.attr('ZoneId');

                    //var ReorderedIds = $('.DroppableContent').sortable("toArray").toString().replace(/widget_/gi, "");
                    var ReorderedIds = $(this).sortable("toArray").toString().replace(/widget_/gi, "");
                    //alert(ReorderedIds);

                    $.ajax({
                        type: "POST",
                        url: "/Default.aspx/ReorderWidgetsInPage",
                        //data: JSON.stringify({ 'PageId': '<%= Session["PageID"] %>', 'Reorder': ReorderedIds }),
                        data: JSON.stringify({ 'PageId': '<%= Session["PageID"] %>', 'ZoneId': ZoneId, 'Reorder': ReorderedIds }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d != "")
                                alert(response.d);
                        },
                        error: function (xhr, status, errorThrown) {
                            alert(status + " | " + xhr.responseText);
                        }
                    });
                }
            }
            , receive: function (event, ui) {
                $('body').css('cursor', 'auto');

                var WidgetItem = $(this).find('.WidgetItem');

                if (CancelSorting) {
                    CancelSorting = false;
                    /*$('.DroppableContent').sortable("cancel");
                    $('.DroppableContent .WidgetItem').remove();*/
                    $(this).sortable("cancel");
                    WidgetItem.remove();
                    return;
                }

                var ZoneId = WidgetItem.parent().attr('ZoneId');

                var newIndex = parseInt($(this).data("ui-sortable").currentItem.index()) + 1;

                var item = ui.item.attr('id').split("_");
                var widget = item[0];
                var id = item[1];

                if (id == "new") {
                    WidgetItem.remove();
                    switch (widget) {
                        case "banners":
                            $('.hfBannerParameters').val(ZoneId + ',' + newIndex);
                            $('.BtnAddBannerGallery').click();
                            break;
                        case "photos":
                            $('.hfPhotoParameters').val(ZoneId + ',' + newIndex);
                            $('.BtnAddAlbum').click();
                            break;
                        case "documents":
                            $('.hfDocParameters').val(ZoneId + ',' + newIndex);
                            $('.BtnAddDocuments').click();
                            break;
                        case "eform":
                            break;
                        case "popupmessages":
                            $('.BtnAddPopupMessages').click();
                            break;
                        case "content":
                            $('.hfContentParameters').val(ZoneId + ',' + newIndex);
                            $('.BtnAddContent').click();
                            break;
                        default:
                    }
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: "/Default.aspx/AddWidgetToPage",
                        data: JSON.stringify({ 'PageId': '<%= Session["PageID"] %>', 'ContentId': id, 'ZoneId': ZoneId, 'Priority': newIndex }),
                        //data: JSON.stringify({ 'PageId': '<%= Session["PageID"] %>', 'ContentId': id, 'Priority': newIndex }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d != "")
                                alert(response.d);
                            window.location.reload();
                        },
                        error: function (xhr, status, errorThrown) {
                            alert(status + " | " + xhr.responseText);
                        }
                    });
                }
            }
            , over: function (event, ui) {
                ui.placeholder.removeClass("hide-placeholder");
            }
            , out: function (event, ui) {
                ui.placeholder.addClass("hide-placeholder");
            }
            , start: function (event, ui) {
                if (ui.item.attr('id') != null && ui.item.attr('id') != 'undefined' && ui.item.attr('id') != '') {
                    $('#remove_widget').addClass("active");
                    $('#remove_widget').parent().effect("shake", { direction: 'up', distance: 3, times: 2 }, 500);
                    if (ui.item.parent().hasClass('DC_Banner'))
                        $('#page').addClass('goBehind');
                }
            }
            , stop: function (event, ui) {
                if (ui.item.attr('id') != null && ui.item.attr('id') != 'undefined' && ui.item.attr('id') != '') {
                    $('#remove_widget').removeClass("active");
                    $('#page').removeClass('goBehind');
                }

                if (CancelSorting) {
                    CancelSorting = false;
                    $(this).sortable("cancel");
                    if (!$(".DroppableContent").sortable("option", "revert"))
                        $(".DroppableContent").sortable("option", "revert", true);
                }
            }
        });

        if (DraggingCursor != null) {
            $('.DroppableContent').each(function () {
                $(this).sortable("option", "cursor", DraggingCursor);
            });
        }

        $(document).keydown(function (e) {
            //e = (e) ? e : window.event;
            var charCode = (e.which) ? e.which : e.keyCode;
            if (charCode == 27) {
                CancelSorting = true;
                $('.DroppableContent').trigger('mouseup');
                $('.WidgetDD .WidgetItem').trigger('mouseup');
            }
        });
    });

    //Re-bind for callbacks 
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        BindWTBControlEvents();
    });
</script>

