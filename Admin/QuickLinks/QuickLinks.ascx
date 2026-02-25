<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuickLinks.ascx.cs" Inherits="Admin_Groups_Groups" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="/js/jquery.tablednd.0.8.min.js"></script>

 <asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false"></asp:ScriptManager>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Quicklinks</div>
    <div class="admin-header-subtitle">Here you can create and modify Quicklinks to give users quick navigation around your site.</div>
</div>
<div class="admin-control-wrapper">
    <asp:UpdatePanel runat="server" ID="up1">
    <ContentTemplate>
        <div><asp:Label ID="lbl_msg" runat="server" CssClass="alert"></asp:Label></div>

        <asp:Panel ID="pnlGroups" runat="server">
                <div class="admin-white-box" style="min-width: 640px;">
                    <div class="admin-white-box-header">QuickLinks</div>
                    <div class="admin-white-box-inner">
                       <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			            <tr><td colspan="2">
                            <asp:GridView ID="GV_Group" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowSorting="False" AutoGenerateColumns="False"
                                CellPadding="0" GridLines="None" OnRowCommand="GV_Group_RowCommand" OnRowDataBound="GV_Group_RowDataBound" >
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" 
                                        HeaderStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle  CssClass="itemrow" Width="300" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Copy Embeded Code">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="litCopy"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
            <script>
                function myCopyFunction(id, name) {

                    var mytext = "<widget id='" + id + "' class='control-quicklinks'>Quicklinks: " + name + "</widget>";
                    navigator.clipboard.writeText(mytext);

                    /* Alert the copied text */

                    alert("Embed code for '" + name + "' has been copied. Paste into any accordion  to display.");
                }
            </script>
            </asp:Panel>

         <asp:Panel id="pnlEditGrp" runat="server" visible="false">
            <div class="admin-white-box">
                <div class="admin-white-box-inner">
                    <table cellspacing="0" cellpadding="0" border="0">
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox ID="txtNameGrp" CssClass="textbox" runat="server" Width="450"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNameGrp" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                    </table>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="btn_Cancel_Grp" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btn_Cancel_Grp_Click" CausesValidation="False" />
            <asp:LinkButton ID="btn_Submit_Grp" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btn_Submit_Grp_Click" ValidationGroup="EditForm" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="EditForm" ShowMessageBox="True" ShowSummary="False" />
        </asp:Panel>

        <asp:Panel ID="pnlList" runat="server" Visible="false">
            <div class="admin-white-box" style="min-width: 640px;">
                <div class="admin-white-box-header">
                    <asp:LinkButton ID="bttn_Add_Question" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="bttn_Add_Question_Click" />
                </div>
                <div class="admin-white-box-inner">
                    <div id="tbl_noresults" runat="server" style="padding: 20px 0;">No results were found.<asp:Label ID="Label1" runat="server" CssClass="alert"></asp:Label></div>
                    <table ID="tbl_Grid" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			        <tr><td colspan="2">
                        <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="True" 
                            AllowSorting="False" AutoGenerateColumns="False" CellPadding="0" 
                            onrowdatabound="gvMain_RowDataBound" OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" 
                            onsorting="GV_Main_Sorting" GridLines="None">
                            <HeaderStyle CssClass="admin-grid-header" />
                            <PagerSettings Visible="false" />
                            <FooterStyle/>
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Right" >
                                        <ItemStyle CssClass="itemrow" Width="1" HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" ID="litHfId"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:BoundField DataField="name" HeaderText="Link Name" SortExpression="name">
                                <ItemStyle CssClass="itemrow" Width="200"  HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle  HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="url" HeaderText="URL" SortExpression="url">
                                <ItemStyle CssClass="itemrow" Width="200" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:BoundField DataField="language" HeaderText="Language" SortExpression="language" Visible="false">
                                <ItemStyle CssClass="itemrow" Width="150" HorizontalAlign="Left" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Left"/>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="edit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item" ToolTip="Edit" />
                                        <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False"  AlternateText="Delete" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td></tr>
                    <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		            </table>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="btnBack" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btnBack_Click" CausesValidation="False" />

            <script type="text/javascript">

                var strorder;

                function reorder() {
                    strorder = "";

                    var totalid = $("#<%= GV_Main.ClientID %> tr td input[class='HfId']").length;
                    for (var i = 0; i < totalid; i++) {
                        strorder += $("#<%= GV_Main.ClientID %> tr td input[class='HfId']")[i].getAttribute("value") + "|";
                                }

                   // alert("totalid: " + totalid + " - strorder: " + strorder);
                }

                function BindControlEvents() {
                    $('#<%= GV_Main.ClientID %>').tableDnD(
                    {
                        onDragClass: "myDragClass",
                        onDrop: function (table, row) {
                            reorder();
                            $.ajax({
                                type: "POST",
                                url: "admin.aspx/QuickLinks_Reorders",
                                data: '{"Reorder":"' + strorder + '", "Group":"' + <%=Group%> + '"}',
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
                    $('#<%= GV_Main.ClientID %>').find('tr').each(function () {
                        var id = $(this).find("input[class='HfId']").val();


                        if (id != null && id != 'undefined')
                            $(this).attr('id', 'tr_' + id);
                    });

                    $("#<%= GV_Main.ClientID %> tr[id^='tr_']").hover(function () {
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

        <asp:Panel id="tbl_add_edit" runat="server">
            <div class="admin-white-box">
                <div class="admin-white-box-inner">
                    <table cellspacing="0" cellpadding="0" border="0">
                    <tr><td class="admin-prompt-right"><span class="required">*</span>Name</td><td><asp:TextBox ID="txt_Name" CssClass="textbox" runat="server" Width="450"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_Name" ErrorMessage="Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                    <tr runat="server" visible="false"><td class="admin-prompt-right">Language</td><td><asp:DropDownList ID="ddlLanguage" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="getpages" AutoPostBack="true" Width="450"></asp:DropDownList></td></tr>
                    <tr><td class="admin-prompt-right">Pages</td><td><asp:DropDownList ID="ddlPages" runat="server" CssClass="dropdownlist" DataTextField="name" DataValueField="id" Width="450" /></td></tr>
                    <tr><td class="admin-prompt-right">Url</td><td><asp:TextBox CssClass="textbox" ID="txtUrl" runat="server" Width="450" /></td></tr>
                                
                    <tr><td class="admin-prompt-right">New window</td><td style="padding-top: 15px;">
                        <asp:RadioButtonList ID="rbNewWindow" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal">
                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>

                        </asp:RadioButtonList></td></tr>
				    
                   <tr>
                <td class="admin-prompt-right">
                    Image:
                </td>
                <td>
                    <asp:FileUpload runat="server" ID="fuImage"  CssClass="textbox"/>
                    <%--&nbsp;<asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="false" onclick="UploadImage_Click" Width="60" />--%>
                     <br /><br /><div style="background-color:#00205b;max-width: 34px;"><asp:Image ID="imgCurrentImage" runat="server"  CssClass="curImage" style="max-width:200px; height:auto;"/></div>&nbsp;&nbsp;
                    <asp:LinkButton runat="server" ID="lnkDlete" Text="Delete Image" OnClick="lnkDlete_Click" ForeColor="#265892" Font-Underline="true" Font-Size="14px" ></asp:LinkButton>
                </td>

            </tr>
                        <tr><td colspan="2" style="height:20px;"></td></tr>
                        <tr>
                <td class="admin-prompt-right">
                    Image (hover):
                </td>
                <td>
                    <asp:FileUpload runat="server" ID="fuImage2"  CssClass="textbox"/>
                     <br /><br /><div style="background-color:#00205b;max-width: 34px;"><asp:Image ID="imgCurrentImage2" runat="server"  CssClass="curImage" style="max-width:200px; height:auto;"/></div>&nbsp;&nbsp;
                    <asp:LinkButton runat="server" ID="lnkDlete2" Text="Delete Image" OnClick="lnkDlete2_Click" ForeColor="#265892" Font-Underline="true" Font-Size="14px" ></asp:LinkButton>
                </td>

            </tr>
                        
                    </table>
                </div>
            </div>
            <br />
            <asp:LinkButton ID="btn_Cancel_step5" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btn_Cancel_step1_Click" CausesValidation="False" />
            <asp:LinkButton ID="btn_Submit" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btn_Submit_Click" ValidationGroup="EditForm" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="EditForm" ShowMessageBox="True" ShowSummary="False" />

            <script>
                $(document).ready(function () {
                    $('#<%=ddlPages.ClientID%>').change(function () {
                        $('#<%=txtUrl.ClientID%>').val('');
                    });
                });
            </script>
        </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
          <div class="updateprogress">Update in progress...</div>
        </ProgressTemplate>
        </asp:UpdateProgress>
</div>
