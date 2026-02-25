<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TextSize.ascx.cs" Inherits="Controls_TextSize_TextSize" %>

<script type="text/javascript">

	var styles = ['h1', 'h2', 'h3', 'p', 'a', 'li', 'span', 'div'];
	var changer = "span.changer";
	var grow = 'spGrow';
	var srhink = 'spShrink';
	var growup = 3;
	var grew = 0; //counter initial value

	// initialize the jquery code
	$(document).ready(function () {
	    $(changer).click(function () {

	        for (var i = 0; i < styles.length; i++) {
	            var $mainText = $(styles[i]);
	            var changerId = this.id;

	            $mainText.each(function () {
	                if ($(this).css('font-size')) {
	                    var currentSize = $(this).css('font-size');
	                    var num = parseFloat(currentSize, 10);

	                    var unit = currentSize.slice(-2);

	                    if (changerId == grow && grew < growup) {
	                        num += 1;
	                    }
	                    else if (changerId == srhink && grew > 0) {
	                        num -= 1;
	                    }
	                    $(this).css('font-size', num + unit);
	                }
	            });
	        }

	        if (this.id == grow && grew < growup)
	            grew++;
	        else if (this.id == srhink && grew > 0)
	            grew--;

	        return false;

	    });

	});
</script>
<div class="textSize"><span class="txtLabel"><% if (Language == "1"){ %>Font Size:<% }else{ %>Taille du texte:<% } %></span>
	<span id="spShrink" class="changer"><a href="/minus" title="Decrease Text Size"></a></span>
    <span id="spGrow" class="changer"><a href="/plus" title="Increase Text Size"></a></span>
</div>
