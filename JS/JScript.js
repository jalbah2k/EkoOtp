    function pop_up(href) {
        var param = 'height=%1px,width=%2px,left=%3;top=%4;status=no,resizable=no,toolbar=no,menubar=no,location=no,scrollbars=no';
        var _hght = ((arguments[1]) ? arguments[1] : 600);
        var _wdth = ((arguments[2]) ? arguments[2] : 900);
        var _left = ((arguments[3]) ? arguments[3] : 100);
        var _top = ((arguments[4]) ? arguments[4] : 100);
        _left = (screen.width) ? (screen.width - _left) / 2 : _left;
        _top = (screen.height) ? (screen.height - _top) / 2 : _top;
        param = param.replace(/%1/, _hght).replace(/%2/, _wdth).replace(/%3/, _left).replace(/%4/, _top);
        var name = null;
        window.open(href, 'popup', param);
        document.getElementById
    }    
