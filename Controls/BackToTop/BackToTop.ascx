<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BackToTop.ascx.cs" Inherits="Controls_BackToTop_BackToTop" %>
<%--<script type="text/javascript" src="/js/fontdetect.min.js"></script>
<script type="text/javascript" src="/js/jquery.topLink.js"></script>--%>
    <%--<script type="text/javascript">
        jQl.loadjQdep('/js/jquery.topLink.js');
    </script>--%>
<script type="text/javascript">
    $(document).ready(function () {
        //usage w/ smoothscroll
        //set the link
        $('#top-link').topLink({
            min: 500,
            fadeSpeed: 100,
            scrollSpeed: 300
        });

        FontDetect.onFontLoaded('WebSymbolsLigaRegular', onWebSymbolsLoaded, onWebSymbolsDidntLoad, { msTimeout: 3000 });
    });

    function onWebSymbolsLoaded(p_fontName) {
        //alert(p_fontName + ' looks good!');
    }
    function onWebSymbolsDidntLoad(p_fontName) {
        //alert(p_fontName + ' didn\'t load within 3 seconds');
        $('#top-link').text('Back to Top');
        $('#top-link').css({'font-size':'12px', 'font-weight':'bold'});
    }
</script>
<style type="text/css">
    @font-face{ 
      font-family: 'WebSymbolsLigaRegular';
          src: url('/fonts/WebSymbolsLigaRegular.eot');                                     /* IE9 Compat Modes */
          src: url('/fonts/WebSymbolsLigaRegular.eot?#iefix') format('embedded-opentype'),  /* IE6-IE8 */
               url('/fonts/WebSymbolsLigaRegular.woff') format('woff'),                     /* Pretty Modern Browsers */
               url('/fonts/WebSymbolsLigaRegular.ttf') format('truetype'),                  /* Safari, Android, iOS */
               url('/fonts/WebSymbolsLigaRegular.svg#WebSymbolsRegular') format('svg');     /* Legacy iOS */
    }
    #top-link
    {
        display:none;
        position:fixed;
        right:80px;
        bottom:22px;
        /*width:100px;*/
        padding:5px 10px;
        border:1px solid #CCC;
        background:#222222;
        color:#ffffff;
        
        font-family: 'WebSymbolsLigaRegular', 'Open Sans', "Raleway", "HelveticaNeue", "Helvetica Neue", Helvetica, Arial, sans-serif;
        font-size: 24px;
        /*font-weight:bold;*/
        font-weight:normal;
        text-align:center;
        text-decoration:none;
        z-index:2147483647; /* max value */
        
        filter: alpha(opacity=30);
        -moz-opacity: 0.3;
        -khtml-opacity: 0.3;
        opacity: 0.3;
        
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        -khtml-border-radius: 5px;
        border-radius: 5px;
    
        behavior: url(/js/PIE-2.0beta1/PIE.htc);
    }
    #top-link:hover
    {
        filter: alpha(opacity=100);
        -moz-opacity: 1;
        -khtml-opacity: 1;
        opacity: 1;
    }
</style>
<%--<a href="javascript:void(0)" id="top-link" title="Back to Top">Back to Top</a>--%>
<span class="noprint" role="navigation" aria-label="Back to Top"><a href="javascript:void(0)" id="top-link" title="Back to Top">&#198;</a></span>
