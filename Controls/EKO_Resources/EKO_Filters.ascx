<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_Filters.ascx.cs" Inherits="Filters" %>
<script>
    var helpers =
    {
        buildDropdown: function (result, dropdown, emptyMessage) {
            dropdown.html('');
            dropdown.append('<option value="">' + emptyMessage + '</option>');

            if (result == null)
                return;

            for (i = 0; i < result.length; i++) {
                if (result[i].id == 0)
                    break;

                dropdown.append('<option value="' + result[i].id + '">' + result[i].name + '</option>');
            }
        },

        clearDropdown: function (dropdown, emptyMessage) {
            dropdown.html('');
            dropdown.append('<option value="">' + emptyMessage + '</option>');
        },

        changeQS: function (qstring) {
            if (history.pushState) {
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

    function getFilterPayload() {
        return {
            u: <%=Session["LoggedInId"].ToString()%>,
            lib: $('#<%= ddlLib.ClientID%>').val(),
            cat: $('#<%= ddlCateg.ClientID%>').val(),
            format: $('#<%= ddlFormat.ClientID%>').val(),
            audience: $('#<%= ddlAudience.ClientID%>').val(),
            search: $('#<%=txtSearch.ClientID%>').val(),
            save: 1
        };
    }

    function searchResources(myvalue) {
        $.ajax({
            type: "POST",
            url: "/api/search",
            data: JSON.stringify(myvalue),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (!response || !response[0])
                    return;

                helpers.updateSearchResults(response[0].items, $('#result-items'));
                helpers.updateSearchResults(response[0].header, $('#div-plHeader'));

                var newurl = helpers.updateUrlParameter(window.location.href, "search_term", myvalue.search || "");
                newurl = helpers.updateUrlParameter(newurl, "format", myvalue.format || "");
                newurl = helpers.updateUrlParameter(newurl, "audience", myvalue.audience || "");
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

            var myvalue = getFilterPayload();
            myvalue.lib = $(this).val();

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
                        '<%=AllCategoriesWord%>'
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
        });

        $('#<%=ddlCateg.ClientID%>').change(function () {
            var newvalue = "unset";
            var selected = $(this).find('option:selected');
            if ($(this).val() != "")
                newvalue = selected.text();

            var newurl = helpers.updateUrlParameter(window.location.href, "category", newvalue);
            helpers.changeUrl(newurl);
        });

        $('#btnSearchRes, #btnApplyFilters, #mobileSearch').click(function () {
            searchResources(getFilterPayload());
            if (this.id == "mobileSearch")
                $("#format-filter, #audience-filter, #apply-filter, #cat-filter, #lib-filter, #mobBtnWrap").toggle();
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

<div class="res-search-filters">
    <div class="row-search contained-width">
        <div class="search-heading">Search all resources by keyword</div>
        <div id="search-filter">
            <label for="<%=txtSearch.ClientID %>" class="sr-only">Search</label>
            <asp:TextBox runat="server" ID="txtSearch" placeholder="Search resources..."></asp:TextBox>
            <button type="button" id="btnSearchRes">Search</button>
        </div>
    </div>

    <div class="row-filter contained-width">
        <div id="lib-filter"><div>
            <label for="<%=ddlLib.ClientID %>">Library</label>
            <asp:DropDownList runat="server" ID="ddlLib" DataTextField="name" DataValueField="id"></asp:DropDownList>
        </div></div>
        <div id="cat-filter"><div>
            <label for="<%=ddlCateg.ClientID %>">Category</label>
            <asp:DropDownList runat="server" ID="ddlCateg" DataTextField="name" DataValueField="id"></asp:DropDownList>
        </div></div>
        <div id="format-filter"><div>
            <label for="<%=ddlFormat.ClientID %>">Format</label>
            <asp:DropDownList runat="server" ID="ddlFormat" DataTextField="name" DataValueField="id"></asp:DropDownList>
        </div></div>
        <div id="audience-filter"><div>
            <label for="<%=ddlAudience.ClientID %>">Audience</label>
            <asp:DropDownList runat="server" ID="ddlAudience" DataTextField="name" DataValueField="id"></asp:DropDownList>
        </div></div>
        <div id="apply-filter"><div>
            <button type="button" id="btnApplyFilters">Apply filters</button>
        </div></div>
        <div id="mobBtnWrap">
            <div id="closeMob">Close</div>
            <div class="button" id="mobileSearch">Search</div>
        </div>
    </div>
</div>
