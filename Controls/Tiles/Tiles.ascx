<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tiles.ascx.cs" Inherits="Tiles" %>
<script>
    $(document).ready(function () {
        $('.lbl-bio').click(function () {
            $(this).parent().siblings('div').toggle();
        });
    });
</script>

<style>
    .div-bio{
        background-color:#F2F2F2;
        padding:10px 20px;
        display:flex;
        justify-content:flex-start;

    }
    .lbl-bio{
        position:relative;
        font-size:15px;
        margin-left:10px;
    }
    .lbl-bio:before{
        content: '+';
        font-size:20px;
        position:absolute;
        left:-18px;
        top:-5px;
    }
    .btn-square{
        border-radius:0;
        /*text-transform:capitalize;*/
    }
</style>
<div class="testimonials-4-columns">
 <asp:Repeater runat="server" ID="repItems" OnItemDataBound="repItems_ItemDataBound">
    <ItemTemplate>
        <div>
            <asp:Image runat="server" ID="imgPhoto" />
            <asp:Literal runat="server" ID="litContent"></asp:Literal>
        </div>
    </ItemTemplate>
</asp:Repeater>
</div>