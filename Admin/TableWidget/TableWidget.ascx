<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TableWidget.ascx.cs" Inherits="TableWidget" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>
<%--<%@ Register TagPrefix="CuteWebUI" Namespace="CuteWebUI" Assembly="CuteWebUI.AjaxUploader" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<script type="text/javascript" src="/js/jquery.tablednd.0.8.min.js"></script>


<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false">
</asp:ScriptManager>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Table Generator</div>
    <div class="admin-header-subtitle">Generate Accessible and Responsive data tables, anywhere on your site</div>
</div>

<div class="admin-control-wrapper">
    <asp:UpdatePanel runat="server" ID="up1" >
        <ContentTemplate>
            <!-- Main Page - Galleries -->
            <asp:Panel ID="pnlGroups" runat="server">
                <div class="admin-white-box" style="min-width: 640px;">
                    <div class="admin-white-box-header">Tables</div>
                    <div class="admin-white-box-inner">
                       <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			            <tr><td colspan="2">
                            <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="False" AutoGenerateColumns="False"
                                CellPadding="0" GridLines="None" OnRowCommand="GV_Main_RowCommand" OnRowDataBound="GV_Main_RowDataBound" >
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" 
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditGroup" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Table" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Table/Columns" />
                                            <asp:ImageButton ID="btnItems" runat="server" CommandName="EditRows" ImageUrl="/images/lemonaid/buttonsNew/editfields.png" AlternateText="Rows" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Rows" />
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteGroup" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete" CommandArgument='<%# Eval("id") %>' ToolTip="Delete Table" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
			            </td></tr>
		                </table>
                    </div>
                </div>
                <div class="admin-white-box-header">
                        <asp:LinkButton ID="btnAddGallery" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="btnAddGallery_Click"  />
                    </div>
            </asp:Panel>
			
			<asp:Panel ID="pnlEditGrp" runat="server" Visible="false">
                <%if (Table == 0)
                    { %>
                <style>
                    #ctl34_rbHeaders td{
                       border: 1px solid #dadada;
                       border-radius: 4px;
                       padding:3px 10px;
                    }                    
                </style>
                <%} %>
                <div class="admin-white-box">
                    <div class="admin-white-box-header">Table Setup</div>
                    <div class="admin-white-box-inner">
                        <table cellpadding="0" cellspacing="0" border="0">
                        <tr><td class="admin-prompt-right"><span class="required">*</span>Internal Name</td><td><asp:TextBox ID="txtNameGrp" runat="server" CssClass="textbox" Width="450" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNameGrp" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="Group" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator></td></tr>
                        <tr><td class="admin-prompt-right"><span class="required">*</span>Table Title</td><td><asp:TextBox ID="txtTitle" runat="server" CssClass="textbox" Width="450" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTitle" ErrorMessage="Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="Group" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator></td></tr>
                        <tr><td style="vertical-align:top;"><span class="required">*</span>Header Format</td>
                            <td runat="server" id="tdRbHeaders"><asp:RadioButtonList runat="server" ID="rbHeaders" AutoPostBack="true" OnSelectedIndexChanged="rbHeaders_SelectedIndexChanged" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="1 Header <br><img src='/admin/TableWidget/header1.png' />" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2 Headers<br><img src='/admin/TableWidget/header2.png' />" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator style="float:right;" ID="rfvHeaders" runat="server" ControlToValidate="rbHeaders" ErrorMessage="Headers required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="Group" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                          <div class="admin-white-box-header">Column Setup</div>

                            </td>
                        </tr>
                        <tr><td></td>
                            <td>
                                <br />

                              <asp:GridView ID="GV_Columns" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="False" AutoGenerateColumns="False"
                                CellPadding="0" GridLines="None" OnRowCommand="GV_Columns_RowCommand" OnRowDataBound="GV_Columns_RowDataBound">
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" >
                                        <ItemStyle CssClass="itemrow" Width="1" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="litHfId"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Caption" HeaderText="Top Headers Columns" 
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" OnClick="EditColumn" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText='<%# Eval("id") %>' ToolTip="Edit Column" />
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DelColumn" ImageUrl="/images/lemonaid/buttonsNew/ex.png" AlternateText="Delete Column Button" CommandArgument='<%# Eval("id") %>' ToolTip="Delete Columns" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            </td>
                        </tr>
                        </table>
                    </div>

                    <div class="admin-white-box-header" runat="server" id="divColumnName">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr><td class="admin-prompt-right"><span class="required">*</span>Column Header (Top)</td><td><asp:TextBox ID="txtColumnName" runat="server" CssClass="textbox" Width="350" />
                                <asp:LinkButton ID="btnAddColumn" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="btnAddColumn_Click" ValidationGroup="Column" />
                            </td></tr>
                            <tr><td></td>
                                <td>
                                <asp:LinkButton ID="btnCancelColumn" runat="server" CssClass="admin-button-gray" Text="Cancel" OnClick="btnCancelColumn_Click" Visible="false" CausesValidation="false"/>
                                <asp:LinkButton ID="btnSaveColumn" runat="server" CssClass="admin-button-blue" Text="Save" OnClick="btnSaveColumn_Click" Visible="false" ValidationGroup="Column" />
                                <asp:RequiredFieldValidator ID="rfvColumnName" runat="server" ControlToValidate="txtColumnName" ErrorMessage="Title required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="Column" ForeColor="Red"> &nbsp;Required</asp:RequiredFieldValidator>
                                </td>

                            </tr>
                        </table>

                    </div>

                </div>
                <br />
                <asp:LinkButton ID="btnCancelGrp" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="btnCancelGrp_Click" CausesValidation="false" />
                <asp:LinkButton ID="btnSaveGrp" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="btnSaveGrp_Click" ValidationGroup="Group" />

<script type="text/javascript">

    var strorder;

    function reorder() {
        strorder = "";

        var totalid = $("#<%= GV_Columns.ClientID %> tr td input[class='HfId']").length;
        for (var i = 0; i < totalid; i++) {
            strorder += $("#<%= GV_Columns.ClientID %> tr td input[class='HfId']")[i].getAttribute("value") + "|";
        }

        //alert("totalid: " + totalid + " - strorder: " + strorder);
    }

    function BindControlEvents() {
        $('#<%= GV_Columns.ClientID %>').tableDnD(
        {
            onDragClass: "myDragClass",
            onDrop: function (table, row) {
                reorder();
                $.ajax({
                    type: "POST",
                    url: "admin.aspx/Table_ColumnsReorders",
                    data: '{"Reorder":"' + strorder + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (msg) {
                        //alert("Successfully Save ReOrder");
                    }
                })
            }
        });

        //var i = 0;
        $('#<%= GV_Columns.ClientID %>').find('tr').each(function () {
            var id = $(this).find("input[class='HfId']").val();


            if (id != null && id != 'undefined')
                $(this).attr('id', 'tr_' + id);
        });

        $("#<%= GV_Columns.ClientID %> tr[id^='tr_']").hover(function () {
            $(this.cells[0]).addClass('showDragHandle');
            }, function () {
                $(this.cells[0]).removeClass('showDragHandle');
        });
    }

    //Initial bind
    $(document).ready(function () {
        BindControlEvents();
    });

    //Re-bind for callbacks 
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        BindControlEvents();
    });

</script>

            </asp:Panel>

            <asp:Panel ID="pnlRows" runat="server" Visible="false">
                <style>
                    .headercell,
                    .new-row-header-cell{
                        color:#fff!important;
                        background-color:#434343!important;
                        padding: 0 20px;
                    }
                </style>
                <div class="admin-white-box" style="min-width: 840px;">
                    <div class="admin-white-box-header"><asp:Literal runat="server" id="litTableNameRows"></asp:Literal></div>
                    <div class="admin-white-box-inner">
                       <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			            <tr><td colspan="2">
                            <div class="admin-white-box-header"><asp:Literal runat="server" id="litTableTitleRows"></asp:Literal></div>
                            <asp:GridView ID="GV_Rows" runat="server" CssClass="admin-grid admin-rows" DataKeyNames="id" AllowSorting="False" AutoGenerateColumns="False" 
                                CellPadding="0" GridLines="None" OnRowDataBound="GV_Rows_RowDataBound" >
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                   
                                </Columns>
                            </asp:GridView>
			            </td></tr>
		                </table>
                    </div>

                    <div class="admin-white-box-header">
                        <table cellpadding="0" cellspacing="0" border="0" style="width:100%;">
                             <tr><td></td>
                                <td>
                                <asp:LinkButton ID="btnCancelRow" runat="server" CssClass="admin-button-gray btnActionRow" Text="Cancel" OnClick="btnCancelRow_Click" CausesValidation="false"/>
                                <asp:LinkButton ID="btnSaveRow" runat="server" CssClass="admin-button-blue btnActionRow" Text="Save Row" OnClick="btnSaveRow_Click" ValidationGroup="RowGrp" />
                                 <div style="float:right;">
                                    <asp:LinkButton ID="btnDelRow" runat="server" CssClass="admin-button-green btnActionRow_" Text="Delete Row" OnClick="btnDelRow_Click" CausesValidation="false" />
                                </div>
                                </td>                              
                            </tr>
                        </table>

                    </div>
                </div>
                <div class="admin-white-box-header">
                        <asp:LinkButton ID="btnBackTables" runat="server" CssClass="admin-button-gray" Text="Back" OnClick="btnBackTables_Click" />
                        <asp:LinkButton ID="btAddRow" runat="server" CssClass="admin-button-blue" Text="Add New Row" OnClick="btAddRow_Click"  />
                    <asp:HiddenField runat="server" ID="hdfRow" />
                </div>
                
<script type="text/javascript">

    var strorder;

    function reorder() {
        strorder = "";

        var totalid = $("#<%= GV_Rows.ClientID %> tr td input[class='HfId']").length;
        for (var i = 0; i < totalid; i++) {
            strorder += $("#<%= GV_Rows.ClientID %> tr td input[class='HfId']")[i].getAttribute("value") + "|";
        }

        //alert("totalid: " + totalid + " - strorder: " + strorder);
    }

    function BindControlEvents() {
        $('#<%= GV_Rows.ClientID %>').tableDnD(
        {
            onDragClass: "myDragClass",
            onDrop: function (table, row) {
                reorder();
                $.ajax({
                    type: "POST",
                    url: "admin.aspx/Table_RowsReorders",
                    data: '{"Reorder":"' + strorder + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (msg) {
                        //alert("Successfully Save ReOrder");
                    }
                })
            }
        });

        //var i = 0;
        $('#<%= GV_Rows.ClientID %>').find('tr').each(function () {
            var id = $(this).find("input[class='HfId']").val();


            if (id != null && id != 'undefined')
                $(this).attr('id', 'tr_' + id);
        });

        $("#<%= GV_Rows.ClientID %> tr[id^='tr_']").hover(function () {
            $(this.cells[0]).addClass('showDragHandle');
            }, function () {
                $(this.cells[0]).removeClass('showDragHandle');
        });
    }

    //Initial bind
    $(document).ready(function () {
        BindControlEvents();
    });

    //Re-bind for callbacks 
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        BindControlEvents();
    });

</script>

            </asp:Panel>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

