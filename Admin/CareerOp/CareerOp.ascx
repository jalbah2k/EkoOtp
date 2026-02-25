<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CareerOp.ascx.cs" Inherits="Admin_Banner" %>
<script type="text/javascript">

    function openfiles() {

        window.open('/admin/sqlfiles/sqlfiles.aspx', '_blank');

    }
    
    </script>
    
<table cellpadding="0" cellspacing="0" border="0"><tr><td rowspan="3"><img src="/images/icons/book.png" /></td><td class="admin_header_white" valign="top" style="padding-left:10px;">Career Opportunities</td></tr><tr><td class="admin_title_blue" style="height:30px;padding-left:10px;" valign="top">Career Opportunities</td></tr>
        <tr><td class="admin_bodytext_white" style="padding-left:10px;">Manage the Career Opportunities on the website.</td></tr>
        </table><br />
         <asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false"></asp:ScriptManager>
<asp:UpdatePanel runat="server" ID="up1">
<ContentTemplate>
<asp:Panel ID="pnlList" runat="server">
<div class="WhiteTable" style="padding:6px;width:300px;" id="Div1" runat="server">
            <asp:GridView ID="GV_Main" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                DataKeyNames="id"
                ondatabound="GV_Main_DataBound" onpageindexchanging="GV_Main_PageIndexChanging" 
                OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" 
                GridLines="None" Width="300">
                <HeaderStyle CssClass="TemplateGridHeader" />
                <RowStyle CssClass="TemplateGridItem" />
                <AlternatingRowStyle CssClass="TemplateGridAltItem" />
                <FooterStyle/>
                <Columns>
                    <asp:BoundField DataField="name" HeaderText="Listing">
                    <ItemStyle CssClass="TemplateGridItemFont" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="TemplateGridHeaderFont"  HorizontalAlign="Left"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="groupname" HeaderText="Group">
                    <ItemStyle CssClass="TemplateGridItemFont" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="TemplateGridHeaderFont"  HorizontalAlign="Left"/>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" 
                                CommandName="Edit" ImageUrl="/images/icons/Edit.png" AlternateText="Edit" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="TemplateGridHeaderFont" />
                        <ItemStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False" 
                                CommandName="Delete" ImageUrl="/images/icons/delete.png" 
                                Text="" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle CssClass="TemplateGridHeaderFont" />
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="pagerstyle" HorizontalAlign="Center"/>
            </asp:GridView>
</div>
<div id="Div2" runat="server" visible="false">
</div><br /><asp:ImageButton ID="ImageButton5" runat="server" OnClick="newlist" ImageURL="/images/buttons/add.gif"/>
</asp:Panel>
<asp:Panel ID="pnlListGroups" runat="server" Visible="false">
<div class="WhiteTable" style="padding:6px;width:300px;" id="list" runat="server">
            <asp:GridView ID="GV_Main2" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                DataKeyNames="id"
                ondatabound="GV_Main_DataBound2" onpageindexchanging="GV_Main_PageIndexChanging2" 
                OnRowDeleting="GV_Main_RowDeleting2" OnRowEditing="GV_Main_RowEditing2" 
                GridLines="None" Width="300">
                <HeaderStyle CssClass="TemplateGridHeader" />
                <RowStyle CssClass="TemplateGridItem" />
                <AlternatingRowStyle CssClass="TemplateGridAltItem" />
                <FooterStyle/>
                <Columns>
                    <asp:BoundField DataField="name" HeaderText="Group Name">
                    <ItemStyle CssClass="TemplateGridItemFont" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="TemplateGridHeaderFont"  HorizontalAlign="Left"/>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" 
                                CommandName="Edit" ImageUrl="/images/icons/Edit.png" AlternateText="Edit" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="TemplateGridHeaderFont" />
                        <ItemStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False" 
                                CommandName="Delete" ImageUrl="/images/icons/delete.png" 
                                Text="" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle CssClass="TemplateGridHeaderFont" />
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="pagerstyle" HorizontalAlign="Center"/>
            </asp:GridView>
</div>
<div id="nolist" runat="server" visible="false">
</div><br /><asp:ImageButton ID="btnMake" runat="server" OnClick="newgroup" ImageURL="/images/buttons/add.gif"/>
</asp:Panel>
<asp:Panel ID="pnlView" runat="server" Visible="false">
<div class="WhiteTable" style="padding:6px;width:600px;" id="doctbl" runat="server">
<table border="0" cellpadding="3" cellspacing="0">
<tr><td style="color:#ffffff;" valign="top">Group Name:</td><td><asp:Textbox ID="txtGroupNameEdit" runat="server" width="300"/></td><td><asp:ImageButton runat="server" ImageUrl="/images/buttons/save_dark.gif" ID="btnSaveGroupName" OnClick="SaveGroupName" /></td></tr></table><br />
<table border="0" cellpadding="3" cellspacing="0" width="600">
<tr><td valign="top">
<asp:GridView ID="GV_Docs" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                DataKeyNames="id"
                ondatabound="GV_Docs_DataBound" onpageindexchanging="GV_Docs_PageIndexChanging" 
                OnRowDeleting="GV_Docs_RowDeleting" OnRowEditing="GV_Docs_RowEditing" 
                GridLines="None" Width="600">
                <HeaderStyle CssClass="TemplateGridHeader" />
                <RowStyle CssClass="TemplateGridItem" />
                <AlternatingRowStyle CssClass="TemplateGridAltItem" />
                <FooterStyle/>
                <Columns>
                    <asp:BoundField DataField="name" HeaderText="Name">
                    <ItemStyle CssClass="TemplateGridItemFont" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="TemplateGridHeaderFont"  HorizontalAlign="Left"/>
                    </asp:BoundField>
                     <asp:BoundField DataField="url" HeaderText="URL">
                    <ItemStyle CssClass="TemplateGridItemFont" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <HeaderStyle CssClass="TemplateGridHeaderFont"  HorizontalAlign="Left"/>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Edit" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" 
                                CommandName="Edit" ImageUrl="/images/icons/Edit.png" AlternateText="View Message" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="TemplateGridHeaderFont" />
                        <ItemStyle HorizontalAlign="Center"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False" 
                                CommandName="Delete" ImageUrl="/images/icons/delete.png" 
                                Text="" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle CssClass="TemplateGridHeaderFont" />
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="pagerstyle" HorizontalAlign="Center"/>
            </asp:GridView>
</td></tr>
</table></div><br /><asp:ImageButton ID="btnBack" runat="server" ImageURL="/images/buttons/done.gif" OnClick="backtolist" Visible="true"/>&nbsp;<asp:ImageButton ID="btnSend" runat="server" ImageURL="/images/buttons/add.gif" OnClick="addnewlink"/><br /><div id="nopic" runat="server" class="admin_title_blue">No Links in this Category</div>
</asp:Panel>
<asp:Panel ID="pnlMakeList" runat="server" Visible="false">
<div class="WhiteTable" style="padding:6px;width:600px;">
<table border="0" cellpadding="3" cellspacing="0" width="600">
<tr><td colspan="2" class="admin_bodytext_white">New List</td></tr>
<tr><td style="background-color:#ffffff;" valign="top">Name:</td><td style="background-color:#ffffff;"><asp:Textbox ID="Textbox3" runat="server" width="300"/></td></tr><tr><td style="background-color:#ffffff;">Group</td><td style="background-color:#ffffff;"><asp:DropDownList ID="ddlGroupsA" runat="server" /></td></tr>
</table></div><br /><asp:ImageButton ID="ImageButton6" runat="server" ImageURL="/images/buttons/cancel.gif" OnClick="backtolist"/>&nbsp;<asp:ImageButton ID="ImageButton7" runat="server" ImageURL="/images/buttons/submit.gif" OnClick="addnewlist"/>
</asp:Panel>
<asp:Panel ID="pnlSend" runat="server" Visible="false">
<div class="WhiteTable" style="padding:6px;width:600px;">
<table border="0" cellpadding="3" cellspacing="0" width="600">
<tr><td colspan="2" class="admin_bodytext_white">New Group</td></tr>
<tr><td style="background-color:#ffffff;" valign="top">Name:</td><td style="background-color:#ffffff;"><asp:Textbox ID="txtName" runat="server" width="300"/></td></tr><tr style="display:none;"><td style="background-color:#ffffff;">Group</td><td style="background-color:#ffffff;"><asp:DropDownList ID="ddlUseless" runat="server" /></td></tr>
</table></div><br /><asp:ImageButton ID="btnBack2" runat="server" ImageURL="/images/buttons/cancel.gif" OnClick="backtolist"/>&nbsp;<asp:ImageButton ID="btnSend2" runat="server" ImageURL="/images/buttons/submit.gif" OnClick="addnewgroup"/>
</asp:Panel>
<asp:Panel ID="pnlEditDoc" runat="server" Visible="false">
<div class="WhiteTable" style="padding:6px;width:600px;">
<table border="0" cellpadding="3" cellspacing="0" width="600">
<tr><td colspan="2" class="admin_bodytext_white">Edit Link Info</td></tr>
<tr><td class="admin_bodytext_blue" style="background-color:#c2e1f5;" valign="top">Name:</td><td style="background-color:#c2e1f5;"><asp:Textbox ID="txtDocName" runat="server" width="300"/></td></tr>
<tr><td class="admin_bodytext_blue" style="background-color:#c2e1f5;" valign="top">Full URL:</td><td style="background-color:#c2e1f5;"><asp:Textbox ID="txtUrl" runat="server" width="300"/></td></tr>
<tr><td class="admin_bodytext_blue" style="background-color:#c2e1f5;" valign="top">Description:</td><td style="background-color:#c2e1f5;"><asp:Textbox ID="txtDescr" runat="server" width="300"/></td></tr>
<tr><td class="admin_bodytext_blue" style="background-color:#c2e1f5;" valign="top">Target:</td><td style="background-color:#c2e1f5;"><asp:DropDownList ID="ddlTarget" runat="server" width="300"><asp:ListItem Text="New Window" Value="_new" /><asp:ListItem Text="Same Window" Value="_self" /></asp:DropDownList></td></tr>
</table></div><br /><asp:ImageButton ID="ImageButton1" runat="server" ImageURL="/images/buttons/cancel.gif" OnClick="backtolist" Visible="false"/>&nbsp;<asp:ImageButton ID="ImageButton2" runat="server" ImageURL="/images/buttons/submit.gif" OnClick="savedocname"/>
</asp:Panel>
<asp:Panel ID="pnlAddDoc" runat="server" Visible="false">
<div class="WhiteTable" style="padding:6px;width:600px;">
<table border="0" cellpadding="3" cellspacing="0" width="600">
<tr><td colspan="2" class="admin_bodytext_white">Add Link Info</td></tr>
<tr><td class="admin_bodytext_blue" style="background-color:#c2e1f5;" valign="top">Name:</td><td style="background-color:#c2e1f5;"><asp:Textbox ID="Textbox1" runat="server" width="300"/></td></tr>
<tr><td class="admin_bodytext_blue" style="background-color:#c2e1f5;" valign="top">Full URL:</td><td style="background-color:#c2e1f5;"><asp:Textbox ID="Textbox2" runat="server" width="300"/></td></tr>
<tr><td class="admin_bodytext_blue" style="background-color:#c2e1f5;" valign="top">Page:</td><td style="background-color:#c2e1f5;"><asp:DropDownList ID="ddlPages" runat="server" width="300" onchange="updateloc()" /></td></tr>
<tr><td class="admin_bodytext_blue" style="background-color:#c2e1f5;" valign="top">Description:</td><td style="background-color:#c2e1f5;"><asp:Textbox ID="txtDescrAdd" runat="server" width="300"/></td></tr>
<tr><td class="admin_bodytext_blue" style="background-color:#c2e1f5;" valign="top">Target:</td><td style="background-color:#c2e1f5;"><asp:DropDownList ID="ddlTargetAdd" runat="server" width="300"><asp:ListItem Text="Same Window" Value="_self" Selected="True" /><asp:ListItem Text="New Window" Value="_new" /></asp:DropDownList></td></tr>
<tr><td colspan="2" class="admin_bodytext_white">OR</td></tr>
<tr><td style="background-color:#c2e1f5;" class="admin_bodytext_blue">Choose a file:</td><td style="background-color:#c2e1f5;"><a href="#" onclick="openfiles();">[Browse]</a></td></tr>
<tr><td style="background-color:#c2e1f5;" class="admin_bodytext_blue">File chosen:</td><td style="background-color:#c2e1f5;"><span id="filename">N/A</span></td></tr>
<tr style="display:none;"><td colspan="2"><asp:TextBox ID="fileid" runat="server" /></td></tr>
</table></div><br /><asp:ImageButton ID="ImageButton3" runat="server" ImageURL="/images/buttons/cancel.gif" OnClick="backtolist" Visible="false"/>&nbsp;<asp:ImageButton ID="ImageButton4" runat="server" ImageURL="/images/buttons/submit.gif" OnClick="savenewlink" />

<script type="text/javascript">

    function updateloc() {
        var ddl = document.getElementById("ctl05_ddlPages");
        var loc = document.getElementById("ctl05_Textbox2");
        loc.value = ddl.options[ddl.selectedIndex].value;
    }

</script>
</asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
      <div class="updateprogress">Update in progress...</div>
    </ProgressTemplate>
    </asp:UpdateProgress>