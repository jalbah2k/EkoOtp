(function () {
    document.documentElement.classList.add('eko-whoiswho-page');
    if (document.body) {
        document.body.classList.add('eko-whoiswho-page');
    }

    var root = document.getElementById('pnlDirectory');
    if (!root) return;
    var dataEl = document.getElementById('ekoWhoisWhoData');
    var members = [];
    try { members = JSON.parse(dataEl.textContent || dataEl.innerText || '[]'); } catch (e) { members = []; }

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

    if (backdrop && backdrop.parentNode !== document.body) {
        document.body.appendChild(backdrop);
    }
    if (panel && panel.parentNode !== document.body) {
        document.body.appendChild(panel);
    }

    function text(v) { return (v || '').toString(); }
    function lower(v) { return text(v).toLowerCase(); }
    function esc(v) {
        return text(v).replace(/[&<>"']/g, function (c) {
            return ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' })[c];
        });
    }

    function searchBlob(m) {
        return [
            m.name, m.title, m.organization, m.institution, m.certification,
            m.email, m.phone, m.mobile, m.linkedIn, m.pronouns, m.yearOfGraduation,
            m.about, (m.committees || []).join(' ')
        ].join(' ').toLowerCase();
    }

    function filtered() {
        var q = lower(search.value).trim();
        var orgId = org ? org.value : '';
        var orgName = org && org.options[org.selectedIndex] ? org.options[org.selectedIndex].text : '';
        return members.filter(function (m) {
            if (orgId) {
                var matchOrg = m.orgId === orgId || lower(m.organization) === lower(orgName);
                if (!matchOrg) return false;
            }
            if (q && searchBlob(m).indexOf(q) === -1) return false;
            return true;
        }).sort(function (a, b) {
            var av = lower(a[sortKey]);
            var bv = lower(b[sortKey]);
            if (av < bv) return sortDir === 'asc' ? -1 : 1;
            if (av > bv) return sortDir === 'asc' ? 1 : -1;
            return lower(a.name) < lower(b.name) ? -1 : 1;
        });
    }

    function avatarHtml(m, sizeClass) {
        if (m.photoUrl) {
            return '<span class="' + sizeClass + '"><img src="' + esc(m.photoUrl) + '" alt=""></span>';
        }
        return '<span class="' + sizeClass + '">' + esc(m.initials || '') + '</span>';
    }

    function render() {
        var rows = filtered();
        searchWrap.classList.toggle('has-text', search.value.length > 0);
        body.innerHTML = '';
        if (!members.length) {
            empty.hidden = false;
            empty.textContent = 'No members have opted into the directory yet.';
            status.textContent = 'Showing 0 members';
            return;
        }
        if (!rows.length) {
            empty.hidden = false;
            empty.textContent = 'No members match your search or filter.';
            status.textContent = 'Showing 0 members';
            return;
        }
        empty.hidden = true;
        status.textContent = 'Showing ' + rows.length + ' member' + (rows.length === 1 ? '' : 's');
        rows.forEach(function (m) {
            var tr = document.createElement('tr');
            tr.innerHTML =
                '<td>' + avatarHtml(m, 'eko-who-avatar') + '</td>' +
                '<td><div class="eko-who-name">' + esc(m.name) + '</div></td>' +
                '<td class="eko-who-muted">' + esc(m.organization) + '</td>' +
                '<td class="eko-who-muted eko-who-col-title">' + esc(m.title) + '</td>' +
                '<td><button type="button" class="eko-who-view" data-id="' + esc(m.id) + '" aria-label="View profile for ' + esc(m.name) + '">View profile →</button></td>';
            body.appendChild(tr);
        });
        Array.prototype.forEach.call(root.querySelectorAll('.eko-who-sort'), function (btn) {
            var key = btn.getAttribute('data-sort');
            if (key === sortKey) {
                btn.setAttribute('aria-sort', sortDir === 'asc' ? 'ascending' : 'descending');
                btn.querySelector('.caret').textContent = sortDir === 'asc' ? '▲' : '▼';
            } else {
                btn.setAttribute('aria-sort', 'none');
                btn.querySelector('.caret').textContent = '▲';
            }
        });
    }

    function field(label, value) {
        if (!text(value).trim()) return '';
        return '<div class="eko-who-field"><span class="eko-who-label">' + esc(label) + '</span><span class="eko-who-value">' + esc(value) + '</span></div>';
    }

    function linkedInHref(v) {
        v = text(v).trim();
        if (!v) return '';
        if (/^https?:\/\//i.test(v)) return v;
        return 'https://' + v.replace(/^\/+/, '');
    }

    function phoneText(m) {
        var p = text(m.phone).trim();
        if (!p) p = text(m.mobile).trim();
        if (!p) return '';
        if (text(m.extension).trim()) p += ' ext. ' + m.extension;
        return p;
    }

    function openProfile(id, trigger) {
        lastFocus = trigger || document.activeElement;
        var member = members.filter(function (m) { return String(m.id) === String(id); })[0];
        panelInner.innerHTML = '<p class="eko-who-muted">Loading profile…</p>';
        document.body.classList.add('eko-who-modal-open');
        backdrop.classList.add('is-open');
        panel.classList.add('is-open');
        panel.focus();
        if (!member) {
            panelInner.innerHTML = '<p class="eko-who-error">This profile is not available. <button type="button" id="ekoWhoRetry">Retry</button></p>';
            var retry = document.getElementById('ekoWhoRetry');
            if (retry) retry.onclick = function () { openProfile(id, lastFocus); };
            return;
        }

        var details = field('Title', member.title) +
            field('Organization', member.organization) +
            field('Institution', member.institution) +
            field('Certification / Degree', member.certification) +
            field('Year of Graduation', member.yearOfGraduation);

        var contact = '';
        if (text(member.email).trim()) {
            contact += '<div class="eko-who-contact-row"><i class="fa fa-envelope" aria-hidden="true"></i><a href="mailto:' + esc(member.email) + '">' + esc(member.email) + '</a></div>';
        }
        var phone = phoneText(member);
        if (phone) contact += '<div class="eko-who-contact-row"><i class="fa fa-phone" aria-hidden="true"></i><span>' + esc(phone) + '</span></div>';
        var li = linkedInHref(member.linkedIn);
        if (li) {
            var liLabel = text(member.linkedIn).replace(/^https?:\/\//i, '');
            contact += '<div class="eko-who-contact-row"><i class="fa fa-link" aria-hidden="true"></i><a href="' + esc(li) + '" target="_blank" rel="noopener noreferrer">' + esc(liLabel) + '</a></div>';
        }

        var chips = (member.committees || []).map(function (c) {
            return '<span class="eko-who-chip">' + esc(c) + '</span>';
        }).join('');
        var about = text(member.about).trim();

        panelInner.innerHTML =
            '<div class="eko-who-profile-head">' +
                avatarHtml(member, 'eko-who-profile-avatar') +
                '<h2 id="ekoWhoPanelName">' + esc(member.name) + '</h2>' +
                (member.pronouns ? '<p class="eko-who-pronouns">' + esc(member.pronouns) + '</p>' : '') +
            '</div>' +
            '<a class="eko-who-pm" href="/Membership/PostPrivateMessage?m=' + encodeURIComponent(member.id) + '" target="_blank" rel="noopener noreferrer">Send a private message</a>' +
            (details ? '<div class="eko-who-section"><h3>DETAILS</h3>' + details + '</div>' : '') +
            (contact ? '<div class="eko-who-section eko-who-contact"><h3>CONTACT</h3>' + contact + '</div>' : '') +
            (chips ? '<div class="eko-who-section"><h3>COMMITTEE MEMBERSHIPS</h3><div class="eko-who-chips">' + chips + '</div></div>' : '') +
            (about ? '<div class="eko-who-section"><h3>ABOUT</h3><p class="eko-who-about">' + esc(about).replace(/\n/g, '<br>') + '</p></div>' : '');
    }

    function closeProfile() {
        document.body.classList.remove('eko-who-modal-open');
        backdrop.classList.remove('is-open');
        panel.classList.remove('is-open');
        if (lastFocus && lastFocus.focus) lastFocus.focus();
    }

    function trap(e) {
        if (!panel.classList.contains('is-open')) return;
        var nodes = panel.querySelectorAll('a, button, input, select, textarea, [tabindex]:not([tabindex="-1"])');
        if (!nodes.length) return;
        var first = nodes[0];
        var last = nodes[nodes.length - 1];
        if (e.shiftKey && document.activeElement === first) { e.preventDefault(); last.focus(); }
        else if (!e.shiftKey && document.activeElement === last) { e.preventDefault(); first.focus(); }
    }

    search.addEventListener('input', function () {
        clearTimeout(timer);
        timer = setTimeout(render, 120);
    });
    clearBtn.addEventListener('click', function () { search.value = ''; render(); search.focus(); });
    if (org) org.addEventListener('change', render);
    body.addEventListener('click', function (e) {
        var btn = e.target.closest('.eko-who-view');
        if (btn) openProfile(btn.getAttribute('data-id'), btn);
    });
    root.querySelectorAll('.eko-who-sort').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var key = btn.getAttribute('data-sort');
            if (sortKey === key) sortDir = sortDir === 'asc' ? 'desc' : 'asc';
            else { sortKey = key; sortDir = 'asc'; }
            render();
        });
    });
    closeBtn.addEventListener('click', closeProfile);
    backdrop.addEventListener('click', closeProfile);
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && panel.classList.contains('is-open')) closeProfile();
        if (e.key === 'Tab' && panel.classList.contains('is-open')) trap(e);
    });

    render();
})();
