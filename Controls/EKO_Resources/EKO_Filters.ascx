<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Filters.ascx.cs" Inherits="Filters" %>
<script>
    var helpers =
    {
        buildDropdown: function (result, dropdown, emptyMessage) {
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

    function searchResources (myvalue) {
        $.ajax({
            type: "POST",
            url: "/api/search",
            data: JSON.stringify(myvalue),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                helpers.updateSearchResults(response[0].items, $('#result-items'));
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

        $('#<%=ddlLib.ClientID%>').change(function () {

            var keywords = $('#<%=txtSearch.ClientID%>').val();

            if ($(this).val() == "") {
                var qstring = "";
                if (keywords != "")
                    qstring = '?search_term=' + keywords;

                helpers.changeQS(qstring);
            }

            helpers.clearDropdown($('#<%= ddlSubcateg.ClientID%>'), '<%=SelectWord%>');

            var myvalue = {
                u: <%=Session["LoggedInId"].ToString()%>,
                lib: $(this).val(),
                search: keywords,
                save: 1
            };
            $.ajax({
                type: "POST",
                url: "/api/category",
                data: JSON.stringify(myvalue),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    helpers.buildDropdown(
                        response,
                        $('#<%=ddlCateg.ClientID%>'),
                        '<%=SelectWord%>'
                    );

                    if (response != null && response.length > 0) {
                        var qstring = '?library=' + response[0].libseo;
                        if (keywords != "")
                            qstring += '&search_term=' + keywords;

                        helpers.changeQS(qstring);
                    }
                },
                error: function (xhr, status, errorThrown) {
                    alert(status + " | " + xhr.responseText);
                }
            });

            searchResources(myvalue);

        });

        $('#<%=ddlCateg.ClientID%>').change(function () {

            var keywords = $('#<%=txtSearch.ClientID%>').val();            

            helpers.clearDropdown($('#<%= ddlSubcateg.ClientID%>'), '<%=SelectWord%>');

            var myvalue = {
                u: <%=Session["LoggedInId"].ToString()%>,
                lib: $('#<%= ddlLib.ClientID%>').val(),
                cat: $(this).val(),
                search: keywords,
                save: 1
            };
            $.ajax({
                type: "POST",
                url: "/api/subcategory",
                data: JSON.stringify(myvalue),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    helpers.buildDropdown(
                        response,
                        $('#<%=ddlSubcateg.ClientID%>'),
                        '<%=SelectWord%>'
                    );

                    var newvalue = "unset";
                    if (response != null && response.length > 0) {
                        newvalue = response[0].catseo;
                    }
                   
                    var newurl = helpers.updateUrlParameter(window.location.href, "category", newvalue);
                    newurl = helpers.updateUrlParameter(newurl, "subcategory", "");
                    helpers.changeUrl(newurl);
                },
                error: function (xhr, status, errorThrown) {
                    alert(status + " | " + xhr.responseText);
                }
            });

            searchResources(myvalue);
        });

        $('#<%=ddlSubcateg.ClientID%>').change(function () {

            var keywords = $('#<%=txtSearch.ClientID%>').val();
            var myvalue = {
                u: <%=Session["LoggedInId"].ToString()%>,
                lib: $('#<%= ddlLib.ClientID%>').val(),
                cat: $('#<%= ddlCateg.ClientID%>').val(),
                sub: $(this).val(),
                search: keywords,
                save: 1
            };
            $.ajax({
                type: "POST",
                url: "/api/subcategory",
                data: JSON.stringify(myvalue),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                   
                    var newvalue = "unset";
                    if (response != null && response.length > 0) {
                        newvalue = response[0].subseo;
                    }

                    var newurl = helpers.updateUrlParameter(window.location.href, "subcategory", newvalue);
                    helpers.changeUrl(newurl);
                },
                error: function (xhr, status, errorThrown) {
                    alert(status + " | " + xhr.responseText);
                }
            });

            searchResources(myvalue);
        });

        $('#btnSearchRes, #mobileSearch').click(function () {

            var keywords = $('#<%=txtSearch.ClientID%>').val();
            var myvalue = {
                u: <%=Session["LoggedInId"].ToString()%>,
                lib: $('#<%= ddlLib.ClientID%>').val(),
                cat: $('#<%= ddlCateg.ClientID%>').val(),
                sub: $(this).val(),
                search: keywords,
                save: 1
            };

            searchResources(myvalue);
            if (this.id == "mobileSearch")
                $("#sub-filter, #cat-filter, #lib-filter, #mobBtnWrap").toggle();
        });

        $('#<%=txtSearch.ClientID%>').keypress(function (e) {

            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                $('#btnSearchRes').click(); return false;
            }
            else
                return true;
        });
    });
</script>

<div class="row-filter contained-width" >
    <div id="lib-filter"><div><span>Library:</span><asp:DropDownList runat="server" ID="ddlLib" DataTextField="name" DataValueField="id"></asp:DropDownList></div></div>
    <div id="cat-filter"><div><span>Category:</span><asp:DropDownList runat="server" ID="ddlCateg" DataTextField="name" DataValueField="id"></asp:DropDownList></div></div>
    <div id="sub-filter"><div><span>Sub category:</span><asp:DropDownList runat="server" ID="ddlSubcateg" DataTextField="name" DataValueField="id"></asp:DropDownList></div></div>
    <div id="search-filter"><div><label for="<%=txtSearch.ClientID %>">Search:</label>
        <div><asp:TextBox runat="server" ID="txtSearch"></asp:TextBox></div>
        <button type="button" id="btnSearchRes" >Go</button>
    </div></div>
    <div id="mobBtnWrap">
        <div id="closeMob">Close</div>
        <div class="button" id="mobileSearch">Search</div>
    </div>
</div>
