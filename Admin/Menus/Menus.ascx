<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menus.ascx.cs" Inherits="Admin_Menus_Menus" %>
<%@ Register TagPrefix="i386" Namespace="i386.UI" Assembly="i386.UI" %>
<%@ Register Src="../oTreeMenu/oTreeMenuCollection.ascx" TagName="oTreeMenuCollection" TagPrefix="otree" %>
<script type="text/javascript">
    function deleteNodes(id) {
         PageMethods.DeleteNodes(id, gomenu, Failure, TimeOut);
    }
    function gomenu() {
        window.location.href = "admin.aspx?c=menu";
    }
</script>
<asp:ScriptManager runat="server" ID="scamag1" EnablePageMethods="true" EnablePartialRendering="false">
</asp:ScriptManager>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Menus</div>
    <div class="admin-header-subtitle">Here you can manage the menus used on your website.</div>
</div>
<div class="admin-control-wrapper">
    <%--<asp:UpdatePanel runat="server" ID="up1">
    <ContentTemplate>--%>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" align="left" style="width: 420px;">
                <div id="thetree" runat="server" visible="true" style="width: 420px;">
                    <div class="admin-white-box" style="min-width: 400px;">
                        <div class="admin-white-box-inner">
                            <div style="position:relative;">
                                <div style="position:absolute; top:0px; left:10px; z-index: 11;"><asp:ImageButton ID="btn_NewMenu" runat="server" onclick="btn_NewMenu_Click" ImageUrl="/images/lemonaid/buttons/menu-new.png" ToolTip="New Menu" />&nbsp;&nbsp;<asp:ImageButton ID="btn_EditMenu" runat="server" Visible="false" onclick="btn_EditMenu_Click" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" ToolTip="Edit Menu" /></div>
                                <div style="position: relative; text-align:right; z-index: 10;">
                                    <asp:ImageButton ID="btn_Addnew" runat="server" ImageUrl="/images/lemonaid/buttons/plus.png" onclick="btn_Addnew_Click" style="height: 20px;" ToolTip="Add Menu Item" />&nbsp;&nbsp;
                                    <asp:ImageButton ID="btn_Del" runat="server" ImageUrl="/images/lemonaid/buttonsNew/delete-red.png" AlternateText="Delete" onclick="btn_Del_Click" ToolTip="Delete" />&nbsp;&nbsp;
                                    <asp:ImageButton ID="btn_Up" runat="server" ImageUrl="/images/lemonaid/buttonsNew/up.png" onclick="btn_Up_Click" style="height: 20px;" ToolTip="Move Up" />&nbsp;&nbsp;
                                    <asp:ImageButton ID="btn_Down" runat="server" ImageUrl="/images/lemonaid/buttonsNew/down.png" onclick="btn_Down_Click" style="height: 20px;" ToolTip="Move Down" />
                                </div>
                            </div>
                            <otree:oTreeMenuCollection ID="oTreeMenuCollection1" runat="server" />
                            <br />
                        </div>
                    </div>
                </div>
            </td>
            <td valign="top">
                <div id="tbl_Step3" runat="server" style="position: relative; z-index: 1">
                    <div class="admin-white-box">
                        <div class="admin-white-box-inner">
                            <table border="0" cellpadding="0" cellspacing="0">
                                 <tr>
                                    <td class="admin-prompt-right">
                                        Multi language
                                    </td>
                                    <td class="admin-prompt-right">
                                        <asp:RadioButtonList ID="rblMultiLang" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblMultiLang_SelectedIndexChanged">
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="admin-prompt-right">
                                        Minisite as Submenu
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLinkedMinisite" runat="server" CssClass="dropdownlist" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="admin-prompt-right">
                                        <span class="required">*</span>English Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Menu_EN" Width="200" CssClass="textbox" runat="server" ValidationGroup="step2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_Menu_EN" ErrorMessage="English Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="admin-prompt-right">
                                        English Title
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Title_EN" Width="200" CssClass="textbox" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="ff1" runat="server">
                                    <td class="admin-prompt-right">
                                        <span class="required">*</span>French Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Menu_FR" runat="server" Width="200" CssClass="textbox" ValidationGroup="step2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_Menu_FR" ErrorMessage="French Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="trTitleFR" runat="server">
                                    <td class="admin-prompt-right">
                                        French Title
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Title_FR" Width="200" CssClass="textbox" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="admin-prompt-right">
                                        External URL
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_URL" runat="server" Width="200" CssClass="textbox" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="frow1" runat="server">
                                    <td class="admin-prompt-right">
                                        External URL (fr)
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_URL_fr" runat="server" Width="200" CssClass="textbox" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="admin-prompt-right">
                                        Internal Page
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDL_Pages" runat="server" CssClass="dropdownlist" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr id="grprow" runat="server">
                                    <td class="admin-prompt-right">
                                        Group
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDL_Groups" runat="server" CssClass="dropdownlist" Width="200px" DataTextField="name" DataValueField="id">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="DDL_Groups" ErrorMessage="Group required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="step2"> *</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="admin-prompt-right">
                                        Target
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDL_Target" runat="server" CssClass="dropdownlist" Width="200px">
                                            <asp:ListItem Value="_self">Same Window</asp:ListItem>
                                            <asp:ListItem Value="_new">New Window</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <%--<tr id="Tr2" runat="server" visible="false">--%>
                                <tr id="Tr1" runat="server" visible="false">
                                    <td class="admin-prompt-right">
                                        Visible
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="RB_Visible" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1" Selected="True">Yes</asp:ListItem>
                                            <asp:ListItem Value="0">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="admin-prompt-right">
                                        Enabled
                                    </td>
                                    <td class="admin-prompt-right">
                                        <asp:RadioButtonList ID="RB_Enabled" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="0">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="admin-prompt-right">
                                        Show Lock
                                    </td>
                                    <td class="admin-prompt-right">
                                        <asp:RadioButtonList ID="rbShowLock" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="0">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="admin-prompt-right">
                                       Only on mobile
                                    </td>
                                    <td class="admin-prompt-left" style="vertical-align:bottom;">
                                        <asp:CheckBox ID="cbOnlyMobile" runat="server" ></asp:CheckBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div id="tbl_Navigation" runat="server">
                        <asp:LinkButton ID="btn_Save" runat="server" CssClass="admin-button-green mw150" Text="Save" ValidationGroup="step2" onclick="btn_Save_Click" />
                        <asp:LinkButton ID="btn_Cancel" runat="server" CssClass="admin-button-gray mw150" Text="Cancel" CausesValidation="False" Visible="false" />
                    </div>
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="step2" ShowMessageBox="True" ShowSummary="False" />
                </div>
            </td>
        </tr>
    </table>
    <div id="pnlNewMenu" runat="server" visible="false" style="position:fixed; left:0; top:0; z-index:99999; width:100%; height:100%; min-height:100%;">
        <div class="SemiTransparentBg">&nbsp;</div> <%-- Set the semitransparent background for the parent div in order to allow opaque content --%>
        <div style="position:absolute; top:50%; width: 100%; text-align: center;">
            <div id="inner" class="rounded-corners shadowed" style="position:relative; top:-95px; margin-left: auto; margin-right: auto; width:480px; padding:20px; background-color:#FFFFFF; text-align: center;"> <%-- Set top = -height/2 to center the inner div --%>
                <table border="0" cellpadding="0" cellspacing="0">
                <tr><td class="admin-prompt-right"><span class="required">*</span>English Name</td><td><asp:TextBox Width="300" CssClass="textbox" ID="txtMenuName_EN" runat="server" /><asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtMenuName_EN" ErrorMessage="Menu English Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                <tr id="trNewMenuFrenchName" runat="server"><td class="admin-prompt-right"><span class="required">*</span>French Name</td><td><asp:TextBox Width="300" CssClass="textbox" ID="txtMenuName_FR" runat="server" /><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMenuName_FR" ErrorMessage="Menu French Name required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator></td></tr>
                <tr><td class="admin-prompt-right">Orientation</td><td><asp:DropDownList ID="ddlOrientation" CssClass="dropdownlist" runat="server" Width="300"><asp:ListItem Text="Horizontal" Value="Horizontal" /><asp:ListItem Text="Vertical" Value="Vertical" /></asp:DropDownList><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlOrientation" ErrorMessage="Orientation required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator>--%></td></tr>
                <tr><td class="admin-prompt-right">Group</td><td><asp:DropDownList ID="ddlGroup" CssClass="dropdownlist" runat="server" Width="300"></asp:DropDownList><%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlGroup" ErrorMessage="Group required" Display="Dynamic" SetFocusOnError="True" ValidationGroup="EditForm"> *</asp:RequiredFieldValidator>--%></td></tr>
                <tr><td class="admin-prompt-right">Page</td><td><asp:DropDownList ID="ddlPage" CssClass="dropdownlist" runat="server" Width="300"></asp:DropDownList></td></tr>
                </table>
                <br />
                <asp:LinkButton ID="ibCancelNewMenu" runat="server" CssClass="admin-button-gray mw150" Text="Cancel" onclick="ibCancelNewMenu_Click" />
                <asp:LinkButton ID="ibSaveNewMenu" runat="server" CssClass="admin-button-green mw150" Text="Save" OnCommand="ibSaveNewMenu_Click" ValidationGroup="EditForm" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="EditForm" ShowMessageBox="True" ShowSummary="False" />
            </div>
        </div>
    </div>

    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <asp:HiddenField ID="hfMinisitesFR" runat="server" Value="" />
</div>
<script type="text/javascript">
    $(document).ready(function () {
        var dicMinisiteMenusFR = '';
        if ($('#<%= hfMinisitesFR.ClientID %>').val().length > 0)
            dicMinisiteMenusFR = JSON.parse($('#<%= hfMinisitesFR.ClientID %>').val());

        $('#<%= ddlLinkedMinisite.ClientID %>').change(function () {
            var id = $(this).val();
            if (id != '') {
                $('#<%= txt_Menu_EN.ClientID %>').val($('#<%= ddlLinkedMinisite.ClientID %> option:selected').text());
                if (dicMinisiteMenusFR.length > 0) {
                    $('#<%= txt_Menu_FR.ClientID %>').val(dicMinisiteMenusFR[id].name);
                }
            }
        });
    });
</script>
