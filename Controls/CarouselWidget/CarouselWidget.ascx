<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CarouselWidget.ascx.cs" Inherits="CarouselWidget" %>

<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>


<style>
/*.container_carousel{
    width:100%;
}*/
.col-centered {
    float: none;
    margin: 0 auto;
}

.carousel-control { 
    width: 8%;
    width: 0px;
}
.carousel-control.left,
.carousel-control.right { 
    margin-right: 40px;
    margin-left: 32px; 
    background-image: none;
    opacity: 1;
}
.carousel-control > a > span {
    color: white;
	  font-size: 29px !important;
}

.carousel-col { 
    position: relative; 
    min-height: 1px; 
    padding: 5px; 
    float: left;
 }

 .active > div { display:none; }
 .active > div:first-child { display:block; }

/*xs*/
@media (max-width: 600px) {
  .carousel-inner .active.left { left: -100%; }
  .carousel-inner .active.right { left: 100%; }
	.carousel-inner .next        { left:  100%; }
	.carousel-inner .prev		     { left: -100%; }
  .carousel-col                { width: 100%; }
	.active > div:first-child + div { display:block; }
	.HRtestimonials .container_carousel .item .carousel-col:nth-child(2)
	{
		display: none!important;
	}
}

/*sm*/
@media (min-width: 601px) and (max-width: 991px) {
  .carousel-inner .active.left { left: -46.5%; } /*-3.5% for spacing*/
  .carousel-inner .active.right { left: 46.5%; }
	.carousel-inner .next        { left:  46.5%; }
	.carousel-inner .prev		     { left: -46.5%; }
  .carousel-col                { width: 46.5%; }
	.active > div:first-child + div { display:block; }
}

/*md*/
@media (min-width: 992px) and (max-width: 1199px) {
  .carousel-inner .active.left { left: -30%; }/*-3% for spacing*/
  .carousel-inner .active.right { left: 30%; }
	.carousel-inner .next        { left:  30%; }
	.carousel-inner .prev		     { left: -30%; }
  .carousel-col                { width: 30%; }
	.active > div:first-child + div { display:block; }
  .active > div:first-child + div + div { display:block; }
}

/*lg*/
@media (min-width: 1200px) {
  .carousel-inner .active.left { left: -30%; }/*-3% for spacing*/
  .carousel-inner .active.right{ left:  30%; }
	.carousel-inner .next        { left:  30%; }
	.carousel-inner .prev		     { left: -30%; }
  .carousel-col                { width: 30%; }
	.active > div:first-child + div { display:block; }
  .active > div:first-child + div + div { display:block; }
	.active > div:first-child + div + div + div { display:block; }
}

.block {
	width: 306px;
	/*height: 230px;*/
}

.red {background: red;}

.blue {background: blue;}

.green {background: green;}

.yellow {background: yellow;}


.glyphicon-chevron-right:before, .glyphicon-chevron-left:before
{
	content: "";
	height: 22px;
	width: 12px;
	display: block;

}

.glyphicon-chevron-right:before
{
	background:url('/Images/arrow-right.png')!important;
	background-size:12px 22px!important;
}

.glyphicon-chevron-left:before
{
	background:url('/Images/arrow-left.png')!important;
	background-size:12px 22px!important;
}

.carousel-control.right {
    right: -60px;
}

.carousel-control.left {
    left: -38px;
}

</style>
<div class="HRtestimonials">

<div class="container container_carousel">
	<div class="row">
		<div class="col-xs-11 col-md-10 col-centered">

			<div id="carousel" class="carousel slide" data-ride="carousel" data-type="multi" data-interval="5000">
				<div class="carousel-inner">
					<%--<div class="item active">
						<div class="carousel-col">
							<div class="block red img-responsive"></div>
						</div>
					</div>
					<div class="item">
						<div class="carousel-col">
							<div class="block green img-responsive"></div>
						</div>
					</div>
					<div class="item">
						<div class="carousel-col">
							<div class="block blue img-responsive"></div>
						</div>
					</div>
					<div class="item">
						<div class="carousel-col">
							<div class="block yellow img-responsive"></div>
						</div>
					</div>--%>

                    <asp:Repeater runat="server" ID="repItems" OnItemDataBound="repItems_ItemDataBound">
                        <ItemTemplate>
                            <div runat="server" id="div_item">
						        <div class="carousel-col">
							        <div class="block img-responsive">
                                        <img src="/ImagesWidget/<%#Eval("image") %>" alt="<%#Eval("alt") %>" style="width:100%;" >
                                        <div class="slideInner">
	                                        <h3><%#Eval("name")%></h3>
	                                        <span><%#Eval("title")%></span>
	                                        <br />
	                                        <asp:Literal runat="server" ID="litText"></asp:Literal>
	                                        <br />
	                                        <a href="<%#Eval("link").ToString() + "#item-" + Eval("id").ToString() %>"><asp:Literal runat="server" ID="litReadmore"></asp:Literal></a>
	                                    </div>
							        </div>
						        </div>
					        </div>                      
                        </ItemTemplate>
                    </asp:Repeater>
				</div>

				<!-- Controls -->
				<div class="left carousel-control">
					<a href="#carousel" role="button" data-slide="prev">
						<span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
						<span class="sr-only">Previous</span>
					</a>
				</div>
				<div class="right carousel-control">
					<a href="#carousel" role="button" data-slide="next">
						<span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
						<span class="sr-only">Next</span>
					</a>
				</div>
			</div>

		</div>
	</div>
</div>
                        <br><br>
<div>
                       
    <center><asp:Literal runat="server" ID="litViewAll"></asp:Literal></center>
</div>
</div>
<script>
    $('.carousel[data-type="multi"] .item').each(function() {
	var next = $(this).next();
	if (!next.length) {
		next = $(this).siblings(':first');
	}
	next.children(':first-child').clone().appendTo($(this));

	for (var i = 0; i < 1; i++) { // 2 for 4 items 1 for 3 items
		next = next.next();
		if (!next.length) {
			next = $(this).siblings(':first');
		}

		next.children(':first-child').clone().appendTo($(this));
	}
});
</script>
