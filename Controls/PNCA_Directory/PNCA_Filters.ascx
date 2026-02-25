<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PNCA_Filters.ascx.cs" Inherits="Filters" %>

<script>
    var helpers =
    {
        buildDropdown: function (result, dropdown, emptyMessage, type) {
            // Remove current options
            dropdown.html('');

            // Add the empty option with the empty message
            dropdown.append('<option value="">' + emptyMessage + '</option>');

            if (result == null)
                return;

            // Loop through each of the results and append the option to the dropdown
            for (i = 0; i < result.length; i++) {
                if (result[i].id == 0)
                    break;

                if (result[i].type == type)
                    dropdown.append('<option value="' + result[i].id + '">' + result[i].name + '</option>');
            }
        },

        clearDropdown: function ( dropdown, emptyMessage) {
            // Remove current options
            dropdown.html('');

            // Add the empty option with the empty message
            dropdown.append('<option value="">' + emptyMessage + '</option>');
        },

        changeQS: function (qstring) {
            if (history.pushState) {

              //e.g. var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?myNewUrlQuery=1';
                var newurl = '<%=MyUrl%>' + qstring;
                window.history.pushState({ path: newurl }, '', newurl);
            }
        },

        changeUrl: function (newurl) {
            if (history.pushState) {
                window.history.pushState({ path: newurl }, '', newurl);
            }
        },

        updateUrlParameter: function (url, param, value) {

            if ((url.indexOf(param + "=" + value) != -1) && value != "") {
                return url;
            }

            var regex = new RegExp('(?<=[?|&])(' + param + '=)[^\&]+', 'i');
            if (value == undefined)
                value = "unset";

            var newurl = url.replace(regex, param + '=' + value);
            if (url == newurl) {
                if (newurl.indexOf('?') >= 0)
                    newurl += "&" + param + '=' + value;
                else
                    newurl += "?" + param + '=' + value;
            }

            if (value == "" || value == "unset")
                newurl = newurl.replace("&" + param + '=' + value, "");

            return newurl;
        },

        updateSearchResults: function (result, div) {
            div.html('');
            div.html(result);
        }       
    }

    function searchMembers (myvalue) {
        $.ajax({
            type: "POST",
            url: "/api/SearchPNCA",
            data: JSON.stringify(myvalue),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                helpers.updateSearchResults(response[0].items, $('#dirPageWrap'));
                helpers.updateSearchResults(response[0].header, $('#div-plHeader'));

                var newurl = helpers.updateUrlParameter(window.location.href, "search_term", myvalue.search);
                helpers.changeUrl(newurl);
            },
            error: function (xhr, status, errorThrown) {
                alert(status + " | " + xhr.responseText);
            }
        });
    }

    $(document).ready(function () {

        $('#<%=ddlService.ClientID%>').change(function () {

            var keywords = $('#<%=txtSearch.ClientID%>').val();

            if ($(this).val() == "") {
                var qstring = "";
                if (keywords != "")
                    qstring = '?search_term=' + keywords;

                helpers.changeQS(qstring);
            }

            helpers.clearDropdown($('#<%= ddlCities.ClientID%>'), '<%=SelectWordCity%>');

            var myvalue = {
                serv: $(this).val(),
                search: keywords
            };
            $.ajax({
                type: "POST",
                url: "/api/RegionsPNCA",
                data: JSON.stringify(myvalue),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    helpers.buildDropdown(
                        response,
                        $('#<%=ddlRegions.ClientID%>'),
                        '<%=SelectWordReg%>',
                        'region'
                    );

                    helpers.buildDropdown(
                        response,
                        $('#<%=ddlCities.ClientID%>'),
                        '<%=SelectWordCity%>',
                        'city'
                    );

                    if (response != null && response.length > 0) {
                        var qstring = '?service=' + response[0].service;
                        if (keywords != "")
                            qstring += '&search_term=' + keywords;

                        helpers.changeQS(qstring);
                    }
                },
                error: function (xhr, status, errorThrown) {
                    alert(status + " | " + xhr.responseText);
                }
            });

            searchMembers(myvalue);

        });

        $('#<%=ddlRegions.ClientID%>').change(function () {

            var keywords = $('#<%=txtSearch.ClientID%>').val();            

            helpers.clearDropdown($('#<%= ddlCities.ClientID%>'), '<%=SelectWordCity%>');

            var myvalue = {
                serv: $('#<%= ddlService.ClientID%>').val(),
                reg: $(this).val(),
                search: keywords
            };
            $.ajax({
                type: "POST",
                url: "/api/CitiesPNCA",
                data: JSON.stringify(myvalue),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    helpers.buildDropdown(
                        response,
                        $('#<%=ddlCities.ClientID%>'),
                        '<%=SelectWordCity%>',
                        'city'
                    );

                    var newvalue = "unset";
                    if (response != null && response.length > 0) {
                        newvalue = response[0].region;
                    }
                   
                    var newurl = helpers.updateUrlParameter(window.location.href, "region", newvalue);
                    newurl = helpers.updateUrlParameter(newurl, "city", "");
                    helpers.changeUrl(newurl);
                },
                error: function (xhr, status, errorThrown) {
                    alert(status + " | " + xhr.responseText);
                }
            });

            searchMembers(myvalue);
        });

        $('#<%=ddlCities.ClientID%>').change(function () {

            var keywords = $('#<%=txtSearch.ClientID%>').val();
            var myvalue = {
                serv: $('#<%= ddlService.ClientID%>').val(),
                reg: $('#<%= ddlRegions.ClientID%>').val(),
                city: $(this).val(),
                search: keywords
            };
            $.ajax({
                type: "POST",
                url: "/api/CitiesPNCA",
                data: JSON.stringify(myvalue),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                   
                    var newvalue = "unset";
                    if (response != null && response.length > 0) {
                        newvalue = response[0].city;
                    }

                    var newurl = helpers.updateUrlParameter(window.location.href, "city", newvalue);
                    helpers.changeUrl(newurl);
                },
                error: function (xhr, status, errorThrown) {
                    alert(status + " | " + xhr.responseText);
                }
            });

            searchMembers(myvalue);
        });

        $('#btnSearchDir, #mobileSearchDir').click(function () {

            var keywords = $('#<%=txtSearch.ClientID%>').val();
            var myvalue = {
                serv: $('#<%= ddlService.ClientID%>').val(),
                reg: $('#<%= ddlRegions.ClientID%>').val(),
                city: $('#<%= ddlCities.ClientID%>').val(),
                search: keywords
            };

            searchMembers(myvalue);
            if (this.id == "mobileSearchDir")
                $("#sub-filter, #cat-filter, #lib-filter, #mobBtnWrap").toggle();
        });

        $('#<%=txtSearch.ClientID%>').keypress(function (e) {

            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                $('#btnSearchDir').click(); return false;
            }
            else
                return true;
        });

        $('#clear-filter').click(function () {
            $('#<%=txtSearch.ClientID%>').val('');
            $('#<%=ddlService.ClientID%>').val('');
            $('#<%=ddlService.ClientID%>').change();
        });
    });
</script>

<div class="row-filter contained-width" >
    <div id="lib-filter"><div><span>Service Type:</span><asp:DropDownList runat="server" ID="ddlService" DataTextField="name" DataValueField="id"></asp:DropDownList></div></div>
    <div id="cat-filter"><div><span>Region:</span><asp:DropDownList runat="server" ID="ddlRegions" DataTextField="name" DataValueField="id"></asp:DropDownList></div></div>
    <div id="sub-filter"><div><span>Catchment Area:</span><asp:DropDownList runat="server" ID="ddlCities" DataTextField="name" DataValueField="id"></asp:DropDownList></div></div>
    <div id="search-filter"><div><span>Search:</span><asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>
        <button type="button" id="btnSearchDir" >Go</button>
    </div></div>

        <a id="clear-filter" href="#" style="border-bottom:0!important;"><i>clear all filters</i></a>
    <div id="mobBtnWrap">
        <div id="closeMob">Close</div>
        <div class="button" id="mobileSearchDir">Search</div>
    </div>
</div>


