<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EKO_WhoisWho.ascx.cs" Inherits="EKO_WhoisWho" %>
<script>document.documentElement.classList.add('eko-whoiswho-page');</script>
<% if (Page.Header == null) { %>
<link rel="stylesheet" href="/Controls/EKO_WhoisWho/EKO_WhoisWho.css?v=<%= System.Configuration.ConfigurationManager.AppSettings["CSSVersion"] %>" />
<% } %>

<asp:Panel runat="server" ID="pnlSignedOut" Visible="false" CssClass="eko-who-signedout">
    Please sign in to view the Member Directory.
</asp:Panel>

<asp:Panel runat="server" ID="pnlDirectory" CssClass="eko-who" ClientIDMode="Static">
    <div class="eko-who-card eko-who-filters">
        <div class="eko-who-search" id="ekoWhoSearchWrap">
            <label for="ekoWhoSearch" class="nosize" style="position:absolute;left:-9999px;">Search members</label>
            <input id="ekoWhoSearch" type="search" autocomplete="off"
                   placeholder="Search by name, title, organization, committee, email..." />
            <button type="button" class="eko-who-clear" id="ekoWhoClear" aria-label="Clear search">Clear</button>
        </div>
        <div class="eko-who-org">
            <label for="<%= ddlOrganization.ClientID %>" class="nosize" style="position:absolute;left:-9999px;">Organization</label>
            <asp:DropDownList runat="server" ID="ddlOrganization" ClientIDMode="Static" CssClass="eko-who-org-select" />
        </div>
    </div>

    <p class="eko-who-status" id="ekoWhoStatus" aria-live="polite"></p>

    <div class="eko-who-card">
        <div class="eko-who-table-wrap">
            <table class="eko-who-table" id="ekoWhoTable">
                <thead>
                    <tr>
                        <th scope="col"><span class="nosize">Avatar</span></th>
                        <th scope="col">
                            <button type="button" class="eko-who-sort" data-sort="name" aria-sort="ascending">
                                Name <span class="caret" aria-hidden="true">▲</span>
                            </button>
                        </th>
                        <th scope="col">
                            <button type="button" class="eko-who-sort" data-sort="organization" aria-sort="none">
                                Organization <span class="caret" aria-hidden="true">▲</span>
                            </button>
                        </th>
                        <th scope="col" class="eko-who-col-title">
                            <button type="button" class="eko-who-sort" data-sort="title" aria-sort="none">
                                Title <span class="caret" aria-hidden="true">▲</span>
                            </button>
                        </th>
                        <th scope="col"><span class="nosize">Action</span></th>
                    </tr>
                </thead>
                <tbody id="ekoWhoBody"></tbody>
            </table>
        </div>
        <div class="eko-who-empty" id="ekoWhoEmpty" hidden></div>
    </div>

    <div class="eko-who-backdrop" id="ekoWhoBackdrop"></div>
    <div class="eko-who-panel" id="ekoWhoPanel" role="dialog" aria-modal="true" aria-labelledby="ekoWhoPanelName" tabindex="-1">
        <button type="button" class="eko-who-close" id="ekoWhoClose" aria-label="Close profile">&times;</button>
        <div class="eko-who-panel-inner" id="ekoWhoPanelInner"></div>
    </div>
</asp:Panel>

<script type="application/json" id="ekoWhoisWhoData"><asp:Literal runat="server" ID="litMembersJson" Mode="PassThrough" /></script>
<script src="/Controls/EKO_WhoisWho/EKO_WhoisWho.js?v=<%= System.Configuration.ConfigurationManager.AppSettings["CSSVersion"] %>"></script>
