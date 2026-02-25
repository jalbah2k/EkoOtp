<%@ Control Language="C#" AutoEventWireup="true" CodeFile="azfront.ascx.cs" Inherits="controls_AZfront_azfront" %>
<asp:Literal ID="litJavahead" runat="server" />
<script type="text/javascript">

    var imgloc = "/images/cheo2/letters/";

    $(document).ready(function() {

        $(".letter").hover(function() {

            document.getElementById(this.id).src = imgloc + this.id + "over.png";

        }, function() {

            document.getElementById(this.id).src = imgloc + this.id + "normal.png";

        });

        $(".letter").click(function() {

            window.location.replace("/" + lang + "/HealthBitsAZ#az" + this.id);

        });

    });


    
    
</script><br />
<div style="width:300px;"><img src="/images/cheo2/letters/anormal.png" class="letter" id="a" /><img src="/images/cheo2/letters/bnormal.png" class="letter" id="b" /><img src="/images/cheo2/letters/cnormal.png" class="letter" id="c" /><img src="/images/cheo2/letters/dnormal.png" class="letter" id="d" /><img src="/images/cheo2/letters/enormal.png" class="letter" id="e" /><img src="/images/cheo2/letters/fnormal.png" class="letter" id="f" /><img src="/images/cheo2/letters/gnormal.png" class="letter" id="g" /><img src="/images/cheo2/letters/hnormal.png" class="letter" id="h" /><img src="/images/cheo2/letters/inormal.png" class="letter" id="i" /><img src="/images/cheo2/letters/jnormal.png" class="letter" id="j" /><img src="/images/cheo2/letters/knormal.png" class="letter" id="k" /><img src="/images/cheo2/letters/lnormal.png" class="letter" id="l" /><img src="/images/cheo2/letters/mnormal.png" class="letter" id="m" /><img src="/images/cheo2/letters/nnormal.png" class="letter" id="n" /><img src="/images/cheo2/letters/onormal.png" class="letter" id="o" /><img src="/images/cheo2/letters/pnormal.png" class="letter" id="p" /><img src="/images/cheo2/letters/qnormal.png" class="letter" id="q" /><img src="/images/cheo2/letters/rnormal.png" class="letter" id="r" /><img src="/images/cheo2/letters/snormal.png" class="letter" id="s" /><img src="/images/cheo2/letters/tnormal.png" class="letter" id="t" /><img src="/images/cheo2/letters/unormal.png" class="letter" id="u" /><img src="/images/cheo2/letters/vnormal.png" class="letter" id="v" /><img src="/images/cheo2/letters/wnormal.png" class="letter" id="w" /><img src="/images/cheo2/letters/xnormal.png" class="letter" id="x" /><img src="/images/cheo2/letters/ynormal.png" class="letter" id="y" /><img src="/images/cheo2/letters/znormal.png" class="letter" id="z" /></div>
<div style="display:none;"><img src="/images/cheo2/letters/aover.png" class="letter" id="Img1" /><img src="/images/cheo2/letters/bover.png" class="letter" id="Img2" /><img src="/images/cheo2/letters/cover.png" class="letter" id="Img3" /><img src="/images/cheo2/letters/dover.png" class="letter" id="Img4" /><img src="/images/cheo2/letters/eover.png" class="letter" id="Img5" /><img src="/images/cheo2/letters/fover.png" class="letter" id="Img6" /><img src="/images/cheo2/letters/gover.png" class="letter" id="Img7" /><img src="/images/cheo2/letters/hover.png" class="letter" id="Img8" /><img src="/images/cheo2/letters/iover.png" class="letter" id="Img9" /><img src="/images/cheo2/letters/jover.png" class="letter" id="Img10" /><img src="/images/cheo2/letters/kover.png" class="letter" id="Img11" /><img src="/images/cheo2/letters/lover.png" class="letter" id="Img12" /><img src="/images/cheo2/letters/mover.png" class="letter" id="Img13" /><img src="/images/cheo2/letters/nover.png" class="letter" id="Img14" /><img src="/images/cheo2/letters/oover.png" class="letter" id="Img15" /><img src="/images/cheo2/letters/pover.png" class="letter" id="Img16" /><img src="/images/cheo2/letters/qover.png" class="letter" id="Img17" /><img src="/images/cheo2/letters/rover.png" class="letter" id="Img18" /><img src="/images/cheo2/letters/sover.png" class="letter" id="Img19" /><img src="/images/cheo2/letters/tover.png" class="letter" id="Img20" /><img src="/images/cheo2/letters/uover.png" class="letter" id="Img21" /><img src="/images/cheo2/letters/vover.png" class="letter" id="Img22" /><img src="/images/cheo2/letters/wover.png" class="letter" id="Img23" /><img src="/images/cheo2/letters/xover.png" class="letter" id="Img24" /><img src="/images/cheo2/letters/yover.png" class="letter" id="Img25" /><img src="/images/cheo2/letters/zover.png" class="letter" id="Img26" /></div><asp:Literal id="litJava" runat="server" />