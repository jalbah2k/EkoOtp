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

    var filterNavigateToResources = <%= NavigateToResourcesOnApply ? "true" : "false" %>;
    var resourcesPageUrl = '<%=ResourcesPagePath%>';
    var libraryPageUrl = '<%=LibraryPagePath%>';

    function hasActiveFilters() {
        var v = getFilterPayload();
        return !!(v.lib || v.cat || v.format || v.audience || $.trim(v.search || ""));
    }

    function toggleClearButton() {
        if (hasActiveFilters())
            $('#btnClearFilters').show();
        else
            $('#btnClearFilters').hide();
    }

    function buildResourcesUrl(keywordOnly) {
        var v = getFilterPayload();
        var parts = [];
        if (keywordOnly) {
            if ($.trim(v.search || ""))
                parts.push('search_term=' + encodeURIComponent($.trim(v.search)));
            parts.push('save=1');
            return resourcesPageUrl + '?' + parts.join('&');
        }
        if (v.lib)
            parts.push('library=' + encodeURIComponent(v.lib));
        if (v.cat)
            parts.push('category=' + encodeURIComponent(v.cat));
        if (v.format)
            parts.push('format=' + encodeURIComponent(v.format));
        if (v.audience)
            parts.push('audience=' + encodeURIComponent(v.audience));
        if ($.trim(v.search || ""))
            parts.push('search_term=' + encodeURIComponent($.trim(v.search)));
        if (parts.length)
            parts.push('save=1');
        return resourcesPageUrl + (parts.length ? '?' + parts.join('&') : '');
    }

    function applyFilters(myvalue) {
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
                helpers.changeUrl(buildResourcesUrl(false));
            },
            error: function (xhr, status, errorThrown) {
                alert(status + " | " + xhr.responseText);
            }
        });
    }

    function searchResources(myvalue) {
        $.ajax({
            type: "POST",
            url: "/api/keywordsearch",
            data: JSON.stringify(myvalue),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (!response || !response[0])
                    return;

                helpers.updateSearchResults(response[0].items, $('#result-items'));
                helpers.updateSearchResults(response[0].header, $('#div-plHeader'));

                var newurl = helpers.updateUrlParameter(window.location.href, "search_term", myvalue.search || "");
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
            toggleClearButton();

            if ($(this).val() == "") {
                var qstring = "";
                if (keywords != "")
                    qstring = '?search_term=' + keywords;

                if (!filterNavigateToResources)
                    helpers.changeQS(qstring);
            }

            var myvalue = getFilterPayload();
            myvalue.lib = $(this).val();

            if (!myvalue.lib) {
                helpers.clearDropdown($('#<%=ddlCateg.ClientID%>'), '<%=AllCategoriesWord%>');
                return;
            }

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

                    if (!filterNavigateToResources && response != null && response.length > 0) {
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
            toggleClearButton();
            if (filterNavigateToResources)
                return;

            var newvalue = "unset";
            var selected = $(this).find('option:selected');
            if ($(this).val() != "")
                newvalue = selected.text();

            var newurl = helpers.updateUrlParameter(window.location.href, "category", newvalue);
            helpers.changeUrl(newurl);
        });

        $('#<%=ddlFormat.ClientID%>, #<%=ddlAudience.ClientID%>').change(function () {
            toggleClearButton();
        });

        $('#<%=txtSearch.ClientID%>').on('input', function () {
            toggleClearButton();
        });

        $('#btnApplyFilters').click(function () {
            var myvalue = getFilterPayload();
            if (!myvalue.lib && !myvalue.cat && !myvalue.format && !myvalue.audience) {
                if ($.trim(myvalue.search || "") === "")
                    return;
                if (filterNavigateToResources) {
                    window.location = buildResourcesUrl(true);
                    return;
                }
                myvalue.lib = "";
                myvalue.cat = "";
                myvalue.format = "";
                myvalue.audience = "";
                searchResources(myvalue);
                return;
            }
            if (filterNavigateToResources) {
                window.location = buildResourcesUrl(false);
                return;
            }
            applyFilters(myvalue);
        });

        $('#btnClearFilters').click(function () {
            if (filterNavigateToResources) {
                window.location = libraryPageUrl;
                return;
            }
            window.location = resourcesPageUrl;
        });

        $('#btnKeywordSearch, #mobileSearch').click(function () {
            var myvalue = getFilterPayload();
            if (filterNavigateToResources) {
                if (!$.trim(myvalue.search || ""))
                    return;
                window.location = buildResourcesUrl(true);
                return;
            }
            myvalue.lib = "";
            myvalue.cat = "";
            myvalue.format = "";
            myvalue.audience = "";
            searchResources(myvalue);
            if (this.id == "mobileSearch")
                $("#format-filter, #audience-filter, #apply-filter, #cat-filter, #lib-filter, #mobBtnWrap").toggle();
        });

        $('#<%=txtSearch.ClientID%>').keydown(function (e) {
            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                e.preventDefault();
                $('#btnKeywordSearch').click();
                return false;
            }
        });

        toggleClearButton();
    });
</script>

<div class="res-search-filters">
    <div class="row-search contained-width">
        <div class="search-heading">Search all resources by keyword</div>
        <div id="search-filter">
            <label for="<%=txtSearch.ClientID %>" class="sr-only">Search</label>
            <asp:TextBox runat="server" ID="txtSearch" placeholder="Search resources..."></asp:TextBox>
            <button type="button" id="btnKeywordSearch">Search</button>
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
        <div id="apply-filter"><div class="filter-actions">
            <button type="button" id="btnApplyFilters">Apply filters</button>
            <button type="button" id="btnClearFilters" class="btn-clear-filters"<%= ShowClearButton ? "" : " style=\"display:none\"" %>>Clear</button>
        </div></div>
        <div id="mobBtnWrap">
            <div id="closeMob">Close</div>
            <div class="button" id="mobileSearch">Search</div>
        </div>
    </div>
</div>
