(function () {
    document.documentElement.classList.add('eko-whoiswho-page');

    if (document.body) {
        document.body.classList.add('eko-whoiswho-page');
    }

    var root = document.getElementById('pnlDirectory');
    if (!root) return;

    var dataEl = document.getElementById('ekoWhoisWhoData');
    var members = [];

    try {
        members = JSON.parse(
            dataEl.textContent ||
            dataEl.innerText ||
            '[]'
        );
    } catch (e) {
        members = [];
    }

    var search = document.getElementById('ekoWhoSearch');
    var searchWrap = document.getElementById('ekoWhoSearchWrap');
    var clearBtn = document.getElementById('ekoWhoClear');
    var org = document.getElementById('ddlOrganization');
    var body = document.getElementById('ekoWhoBody');
    var empty = document.getElementById('ekoWhoEmpty');
    var status = document.getElementById('ekoWhoStatus');

    var backdrop = document.getElementById('ekoWhoBackdrop');
    var panel = document.getElementById('ekoWhoPanel');
    var panelInner = document.getElementById('ekoWhoPanelInner');
    var closeBtn = document.getElementById('ekoWhoClose');

    var sortKey = 'name';
    var sortDir = 'asc';
    var lastFocus = null;
    var timer = null;
    var pageSize = 5;
    var visibleCount = pageSize;

    var loadMoreWrap = document.getElementById('ekoWhoLoadMoreWrap');
    var loadMoreBtn = document.getElementById('ekoWhoLoadMore');

    /*
     * Move modal elements to body
     */
    if (backdrop && backdrop.parentNode !== document.body) {
        document.body.appendChild(backdrop);
    }

    if (panel && panel.parentNode !== document.body) {
        document.body.appendChild(panel);
    }

    /*
     * Helper functions
     */
    function text(v) {
        return (v || '').toString();
    }

    function lower(v) {
        return text(v).toLowerCase();
    }

    function esc(v) {
        return text(v).replace(/[&<>"']/g, function (c) {
            return ({
                '&': '&amp;',
                '<': '&lt;',
                '>': '&gt;',
                '"': '&quot;',
                "'": '&#39;'
            })[c];
        });
    }

    /*
     * Search fields
     */
    function searchBlob(m) {
        return [
            m.name,
            m.title,
            m.organization,
            m.institution,
            m.certification,
            m.email,
            m.phone,
            m.mobile,
            m.linkedIn,
            m.pronouns,
            m.yearOfGraduation,
            m.about,
            (m.committees || []).join(' ')
        ].join(' ').toLowerCase();
    }

    /*
     * Filter + Sort
     */
    function filtered() {

        var q = lower(search ? search.value : '').trim();

        var orgId = org ? org.value : '';

        var orgName =
            org &&
                org.options &&
                org.options[org.selectedIndex]
                ? org.options[org.selectedIndex].text
                : '';

        return members
            .filter(function (m) {

                /*
                 * Organization filter
                 */
                if (orgId) {

                    var matchOrg =
                        String(m.orgId) === String(orgId) ||
                        lower(m.organization) === lower(orgName);

                    if (!matchOrg) {
                        return false;
                    }
                }

                /*
                 * Search filter
                 */
                if (q && searchBlob(m).indexOf(q) === -1) {
                    return false;
                }

                return true;
            })
            .sort(function (a, b) {

                var av = lower(a[sortKey]);
                var bv = lower(b[sortKey]);

                if (av < bv) {
                    return sortDir === 'asc' ? -1 : 1;
                }

                if (av > bv) {
                    return sortDir === 'asc' ? 1 : -1;
                }

                return lower(a.name) < lower(b.name) ? -1 : 1;
            });
    }

    /*
     * Avatar
     */
    function avatarHtml(m, sizeClass) {

        var initials = esc(m.initials || '');

        if (m.photoUrl) {

            return '<span class="' + sizeClass + '" data-initials="' + initials + '">' +
                '<img src="' + esc(m.photoUrl) + '" alt="" width="100" height="100" ' +
                'style="display:block;width:100%;height:100%;object-fit:cover;object-position:center;" ' +
                'onerror="this.onerror=null;this.replaceWith(document.createTextNode(this.parentNode.getAttribute(\'data-initials\')));">' +
                '</span>';
        }

        return '<span class="' + sizeClass + '">' +
            initials +
            '</span>';
    }

    /*
     * Create Load More button
     */
    function createLoadMoreButton() {

        var existing = document.getElementById('ekoWhoLoadMoreWrap');

        if (existing) {
            existing.remove();
        }

        var rows = filtered();

        /*
         * No need to show Load More if
         * all records are already displayed.
         */
        if (visibleCount >= rows.length) {
            return;
        }

        var wrapper = document.createElement('div');

        wrapper.id = 'ekoWhoLoadMoreWrap';
        wrapper.className = 'eko-who-load-more-wrap';

        wrapper.innerHTML =
            '<button type="button" id="ekoWhoLoadMore" class="eko-who-load-more">' +
            'Load more' +
            '</button>';

        /*
         * Put button after table/card
         */
        var tableWrap = root.querySelector('.eko-who-table-wrap');

        if (tableWrap && tableWrap.parentNode) {
            tableWrap.parentNode.insertBefore(
                wrapper,
                tableWrap.nextSibling
            );
        } else {
            root.appendChild(wrapper);
        }

        var loadMoreBtn =
            document.getElementById('ekoWhoLoadMore');

        if (loadMoreBtn) {

            loadMoreBtn.addEventListener('click', function () {

                /*
                 * Add 5 more records
                 */
                visibleCount += pageSize;

                render();
            });
        }
    }

    /*
     * Render members
     */
    function render() {
        var rows = filtered();

        searchWrap.classList.toggle('has-text', search.value.length > 0);
        body.innerHTML = '';

        // No members at all
        if (!members.length) {
            empty.hidden = false;
            empty.textContent = 'No members have opted into the directory yet.';
            status.textContent = 'Showing 0 members';

            if (loadMoreWrap) {
                loadMoreWrap.hidden = true;
            }

            return;
        }

        // No records after filtering/search
        if (!rows.length) {
            empty.hidden = false;
            empty.textContent = 'No members match your search or filter.';
            status.textContent = 'Showing 0 members';

            if (loadMoreWrap) {
                loadMoreWrap.hidden = true;
            }

            return;
        }

        empty.hidden = true;

        // Show only the current number of records
        var visibleRows = rows.slice(0, visibleCount);

        status.textContent =
            'Showing ' + visibleRows.length +
            ' of ' + rows.length +
            ' member' + (rows.length === 1 ? '' : 's');

        visibleRows.forEach(function (m) {
            var tr = document.createElement('tr');

            tr.innerHTML =
                '<td>' + avatarHtml(m, 'eko-who-avatar') + '</td>' +
                '<td><div class="eko-who-name">' + esc(m.name) + '</div></td>' +
                '<td class="eko-who-muted">' + esc(m.organization) + '</td>' +
                '<td class="eko-who-muted eko-who-col-title">' + esc(m.title) + '</td>' +
                '<td><button type="button" class="eko-who-view" data-id="' +
                esc(m.id) +
                '" aria-label="View profile for ' +
                esc(m.name) +
                '">View profile →</button></td>';

            body.appendChild(tr);
        });

        // Show/hide Load More button
        if (loadMoreWrap) {
            loadMoreWrap.hidden = visibleCount >= rows.length;
        }

        // Update sorting arrows
        Array.prototype.forEach.call(
            root.querySelectorAll('.eko-who-sort'),
            function (btn) {
                var key = btn.getAttribute('data-sort');

                if (key === sortKey) {
                    btn.setAttribute(
                        'aria-sort',
                        sortDir === 'asc' ? 'ascending' : 'descending'
                    );

                    btn.querySelector('.caret').textContent =
                        sortDir === 'asc' ? '▲' : '▼';
                } else {
                    btn.setAttribute('aria-sort', 'none');
                    btn.querySelector('.caret').textContent = '▲';
                }
            }
        );
    }

    /*
     * Remove Load More button
     */
    function removeLoadMore() {

        var existing =
            document.getElementById('ekoWhoLoadMoreWrap');

        if (existing) {
            existing.remove();
        }
    }

    /*
     * Profile field
     */
    function field(label, value) {

        if (!text(value).trim()) {
            return '';
        }

        return '<div class="eko-who-field">' +
            '<span class="eko-who-label">' +
            esc(label) +
            '</span>' +
            '<span class="eko-who-value">' +
            esc(value) +
            '</span>' +
            '</div>';
    }

    /*
     * LinkedIn
     */
    function linkedInHref(v) {

        v = text(v).trim();

        if (!v) {
            return '';
        }

        if (/^https?:\/\//i.test(v)) {
            return v;
        }

        return 'https://' + v.replace(/^\/+/, '');
    }

    /*
     * Phone
     */
    function phoneText(m) {

        var p = text(m.phone).trim();

        if (!p) {
            p = text(m.mobile).trim();
        }

        if (!p) {
            return '';
        }

        if (text(m.extension).trim()) {
            p += ' ext. ' + m.extension;
        }

        return p;
    }

    /*
     * Open profile
     */
    function openProfile(id, trigger) {

        lastFocus =
            trigger ||
            document.activeElement;

        var member =
            members.filter(function (m) {
                return String(m.id) === String(id);
            })[0];

        panelInner.innerHTML =
            '<p class="eko-who-muted">Loading profile…</p>';

        document.body.classList.add(
            'eko-who-modal-open'
        );

        backdrop.classList.add('is-open');
        panel.classList.add('is-open');

        panel.focus();

        if (!member) {

            panelInner.innerHTML =
                '<p class="eko-who-error">' +
                'This profile is not available. ' +
                '<button type="button" id="ekoWhoRetry">Retry</button>' +
                '</p>';

            var retry =
                document.getElementById('ekoWhoRetry');

            if (retry) {
                retry.onclick = function () {
                    openProfile(id, lastFocus);
                };
            }

            return;
        }

        var details =
            field('Title', member.title) +
            field('Organization', member.organization) +
            field('Institution', member.institution) +
            field(
                'Certification / Degree',
                member.certification
            ) +
            field(
                'Year of Graduation',
                member.yearOfGraduation
            );

        var contact = '';

        if (text(member.email).trim()) {

            contact +=
                '<div class="eko-who-contact-row">' +
                '<i class="fa fa-envelope" aria-hidden="true"></i>' +
                '<a href="mailto:' +
                esc(member.email) +
                '">' +
                esc(member.email) +
                '</a>' +
                '</div>';
        }

        var phone = phoneText(member);

        if (phone) {

            contact +=
                '<div class="eko-who-contact-row">' +
                '<i class="fa fa-phone" aria-hidden="true"></i>' +
                '<span>' +
                esc(phone) +
                '</span>' +
                '</div>';
        }

        var li =
            linkedInHref(member.linkedIn);

        if (li) {

            var liLabel =
                text(member.linkedIn)
                    .replace(/^https?:\/\//i, '');

            contact +=
                '<div class="eko-who-contact-row">' +
                '<i class="fa fa-link" aria-hidden="true"></i>' +
                '<a href="' +
                esc(li) +
                '" target="_blank" rel="noopener noreferrer">' +
                esc(liLabel) +
                '</a>' +
                '</div>';
        }

        var chips =
            (member.committees || [])
                .map(function (c) {
                    return '<span class="eko-who-chip">' +
                        esc(c) +
                        '</span>';
                })
                .join('');

        var about =
            text(member.about).trim();

        panelInner.innerHTML =

            '<div class="eko-who-profile-head">' +

            avatarHtml(
                member,
                'eko-who-profile-avatar'
            ) +

            '<h2 id="ekoWhoPanelName">' +
            esc(member.name) +
            '</h2>' +

            (
                member.pronouns
                    ? '<p class="eko-who-pronouns">' +
                    esc(member.pronouns) +
                    '</p>'
                    : ''
            ) +

            '</div>' +

            '<a class="eko-who-pm" ' +
            'href="/Membership/PostPrivateMessage?m=' +
            encodeURIComponent(member.id) +
            '" target="_blank" ' +
            'rel="noopener noreferrer">' +
            'Send a private message' +
            '</a>' +

            (
                details
                    ? '<div class="eko-who-section">' +
                    '<h3>DETAILS</h3>' +
                    details +
                    '</div>'
                    : ''
            ) +

            (
                contact
                    ? '<div class="eko-who-section eko-who-contact">' +
                    '<h3>CONTACT</h3>' +
                    contact +
                    '</div>'
                    : ''
            ) +

            (
                chips
                    ? '<div class="eko-who-section">' +
                    '<h3>COMMITTEE MEMBERSHIPS</h3>' +
                    '<div class="eko-who-chips">' +
                    chips +
                    '</div>' +
                    '</div>'
                    : ''
            ) +

            (
                about
                    ? '<div class="eko-who-section">' +
                    '<h3>ABOUT</h3>' +
                    '<p class="eko-who-about">' +
                    esc(about).replace(/\n/g, '<br>') +
                    '</p>' +
                    '</div>'
                    : ''
            );
    }

    /*
     * Close profile
     */
    function closeProfile() {

        document.body.classList.remove(
            'eko-who-modal-open'
        );

        backdrop.classList.remove('is-open');
        panel.classList.remove('is-open');

        if (lastFocus && lastFocus.focus) {
            lastFocus.focus();
        }
    }

    /*
     * Focus trap
     */
    function trap(e) {

        if (!panel.classList.contains('is-open')) {
            return;
        }

        var nodes =
            panel.querySelectorAll(
                'a, button, input, select, textarea, [tabindex]:not([tabindex="-1"])'
            );

        if (!nodes.length) {
            return;
        }

        var first = nodes[0];
        var last = nodes[nodes.length - 1];

        if (
            e.shiftKey &&
            document.activeElement === first
        ) {

            e.preventDefault();
            last.focus();

        } else if (
            !e.shiftKey &&
            document.activeElement === last
        ) {

            e.preventDefault();
            first.focus();
        }
    }

    /*
     * Search
     */
    if (search) {

        search.addEventListener('input', function () {
            clearTimeout(timer);

            // Reset to first 5 records whenever search changes
            visibleCount = pageSize;

            timer = setTimeout(render, 120);
        });
    }

    /*
     * Clear search
     */
    if (clearBtn) {

        clearBtn.addEventListener('click', function () {
            search.value = '';

            // Reset pagination
            visibleCount = pageSize;

            render();
            search.focus();
        });
    }

    if (org) {
        org.addEventListener('change', function () {

            // Reset pagination when organization changes
            visibleCount = pageSize;

            render();
        });
    }
    // Load More
    if (loadMoreBtn) {
        loadMoreBtn.addEventListener('click', function () {

            // Add next 5 records
            visibleCount += pageSize;

            render();
        });
    }

    /*
     * View profile
     */
    body.addEventListener(
        'click',
        function (e) {

            var btn =
                e.target.closest('.eko-who-view');

            if (btn) {

                openProfile(
                    btn.getAttribute('data-id'),
                    btn
                );
            }
        }
    );

    /*
     * Sorting
     */
    root.querySelectorAll('.eko-who-sort').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var key = btn.getAttribute('data-sort');

            if (sortKey === key) {
                sortDir = sortDir === 'asc' ? 'desc' : 'asc';
            } else {
                sortKey = key;
                sortDir = 'asc';
            }

            // Reset pagination after sorting
            visibleCount = pageSize;

            render();
        });
    });

    /*
     * Close profile
     */
    if (closeBtn) {
        closeBtn.addEventListener(
            'click',
            closeProfile
        );
    }

    if (backdrop) {
        backdrop.addEventListener(
            'click',
            closeProfile
        );
    }

    /*
     * Keyboard
     */
    document.addEventListener(
        'keydown',
        function (e) {

            if (
                e.key === 'Escape' &&
                panel.classList.contains('is-open')
            ) {
                closeProfile();
            }

            if (
                e.key === 'Tab' &&
                panel.classList.contains('is-open')
            ) {
                trap(e);
            }
        }
    );

    /*
     * INITIAL RENDER
     *
     * This will display only 5 members.
     */
    visibleCount = pageSize;

    render();

})();