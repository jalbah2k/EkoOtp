<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListAndDetails.ascx.cs" Inherits="Admin_MemberDirectory_ListAndDetails" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>
<style>
    .cb-enhanced{
        margin-bottom:5px;
    }
    .itemrow .cbxActive label{
        opacity:0.5;
        cursor:default;

    }

</style>

<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>

<div class="admin-control-wrapper">
    <asp:Panel ID="pnlList" runat="server">
        
        <div class="admin-white-box">
            <div class="admin-white-box-header">Filter</div>
            <div class="admin-white-box-inner">
                <asp:Panel runat="server" DefaultButton="btnFilter">
                <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="admin-prompt-right">Text</td>
                    <td><asp:TextBox ID="txtFilter" runat="server" CssClass="textbox" style="width:238px;" />
                        <ACT:TextBoxWatermarkExtender TargetControlID="txtFilter" WatermarkText="search" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="TextBoxWatermarkExtender1" /></td>
                    <td style="padding-left: 25px;">
                        <asp:LinkButton ID="btnFilter" runat="server" CssClass="admin-button-green mw150" Text="Filter" onclick="btnFilter_Click" />
                        <asp:LinkButton ID="btnClearFilter" runat="server" CssClass="adminHL-button-question" Text="clear filter?" CommandName="Clear" onclick="btnClearFilter_Click"/>
                    </td>
                </tr>
                <tr">
                    <td class="admin-prompt-right">Region</td>
                    <td><asp:DropDownList runat="server" ID="ddlRegionFilter" CssClass="dropdownlist" DataValueField="id" DataTextField="name" AutoPostBack="true" OnSelectedIndexChanged="ddlRegionFilter_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                </table>
                </asp:Panel>
            </div>
        </div>
        <br />
        <div class="admin-white-box" style="min-width: 640px;">
            <div class="admin-white-box-header">
                <asp:LinkButton ID="bttn_Add" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="bttn_Add_Click" />
            </div>
            <div class="admin-white-box-inner">
                 <table id="tbl_Grid" runat="server" cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			    <tr><td colspan="2">
                    <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                        onrowdatabound="GV_Main_RowDataBound" OnRowDeleting="GV_Main_RowDeleting" OnRowCommand="GV_Main_RowCommand"
                        onsorting="GV_Main_Sorting" GridLines="None" CellPadding="0" CellSpacing="0">
                    <HeaderStyle CssClass="admin-grid-header" />
                    <PagerSettings Visible="false" />
                    <FooterStyle/>
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" SortExpression="Name">
                            <ItemStyle CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" Width="250"/>
                        </asp:BoundField>
                        

                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Active">
                            <ItemStyle CssClass="itemrow" Width="65" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbxActive" CssClass="cbxActive cb-enhanced nolabel" Text="&nbsp;" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit" CommandArgument='<%# Eval("id") %>' CommandName="Editing" CausesValidation="False" ToolTip="Edit" />
                                <asp:ImageButton ID="ImageButton2" runat="server" CommandName="Services" ImageUrl="/images/lemonaid/buttonsNew/editfields.png" AlternateText="Edit Items" CommandArgument='<%# Eval("id") %>' ToolTip="Edit Items" />
                                <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False"  AlternateText="Delete" CommandName="Delete" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
			    </td></tr>
                <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" AutoPostBack="true"><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td>
                    <td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager1_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		        </table>
            </div>
        </div>
    </asp:Panel>

<asp:Panel id="pnlDetails"  runat="server" CssClass="admin_title_blue pnlDetails" Visible="false" >
    <script>
        $(document).ready(function () {

            $('#<%=ddlOrganization.ClientID%>').change(function () {

                if ($('#<%= ddlOrganization.ClientID%>').val() != '') {

                    var myvalue = {
                        id: <%=OrgId%>,
                        lib: $('#<%= ddlOrganization.ClientID%>').val()
                    };

                    $.ajax({
                    type: "POST",
                        url: "/api/PncaValidation",
                    data: JSON.stringify(myvalue),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response != '') {
                            alert(response);
                            $('#<%= ddlOrganization.ClientID%>').val('');
                            }
                        },
                        error: function (xhr, status, errorThrown) {
                            alert(status + " | " + xhr.responseText);
                        }
                    });
                }
            });
        });
    </script>
        
    <table border='0' cellpadding='0' cellspacing='0' class='currentwidth'>
        <tr>
            
            <td style='text-align:right'>
                <asp:Button ID="btnBackToList" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btnBackToList_Click" CausesValidation="false" />&nbsp;&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btnSave_Click"  ValidationGroup="Organization" />
                <asp:Button ID="btnDone" runat="server" CssClass="admin-button-blue mw150" Text="Done" onclick="btnDone_Click"  CausesValidation="false" />
            </td>
        </tr>
    </table>
     
    <br />
    <asp:Literal runat="server" ID="litErr"></asp:Literal>

    
    <table cellpadding="0" cellspacing="0" border="0" style="width:100%;">
        <tr><td style="vertical-align:top;">

            <div class="admin-white-box" >
                <div class="admin-white-box-header">Details</div>
                <div class="admin-white-box-header"> 
                    <div style='padding: 10px 10px 10px 30px;width:unset!important;' class="currentwidth">
                       <table border="0" cellpadding="4" cellspacing="4"  style=" padding:0px; width:640px;">

                           <tr>
                                <td>Organization:</td>
                                <td>
                                    <%--<asp:TextBox ID="tbTitle" runat="server" Width='500px' CssClass='textbox' MaxLength="100" Enabled="false" style="opacity:0.5;"></asp:TextBox>--%>
                                    <asp:Label runat="server" id="litTitle" style="opacity:0.5;"></asp:Label>
                                    </td>
                                <td>&nbsp;<%--<asp:RequiredFieldValidator ForeColor="Red" 
                                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="Organization Required" 
                                        SetFocusOnError="True" ValidationGroup="Organization" ControlToValidate="tbTitle">*</asp:RequiredFieldValidator>--%></td>
                            </tr> 

							<tr>
                                <td>Organization:</td>
                                <td><asp:DropDownList runat="server" ID="ddlOrganization" DataTextField="name" DataValueField="id" CssClass="dropdownlist"></asp:DropDownList>
                                    </td>
                                <td>&nbsp;<asp:RequiredFieldValidator ForeColor="Red" 
                                        ID="RequiredFieldValidator3" runat="server" ErrorMessage="Organization Required" 
                                        SetFocusOnError="True" ValidationGroup="Organization" ControlToValidate="ddlOrganization">*</asp:RequiredFieldValidator></td>
                            </tr> 
                             <tr><td style="vertical-align:top; padding-top:25px;">Logo:</td>                    
                                <td><asp:FileUpload runat="server" ID="fuLogo" Width='500px' CssClass='textbox' />
                                    <div>
                                    <asp:Image runat="server" ID="imgLogo" style="width:100px; height:auto;"></asp:Image>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton runat="server" ID="lnkDeleteLogo" onclick="lnkDeleteLogo_Click" Text="Delete" OnClientClick="return confirm('Are you sure to delete this logo?');"></asp:LinkButton></div> </td><td></td>
                            </tr>
                      
                            <tr><td>Website:</td>
                                <td><asp:TextBox ID="tbUrl" runat="server" Width='500px' CssClass='textbox' ></asp:TextBox>
                                </td>
                                <td>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbUrl"
    ForeColor="Red" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
    ErrorMessage = "Invalid URL" ValidationGroup="Organization" Text="*"/></td>
                            </tr>


                            <tr>
                                <td>Admin Contact:</td>
                                <td><asp:TextBox ID="tbAdminContact" runat="server" Width='500px' CssClass='textbox' MaxLength="500" ></asp:TextBox>
                                </td>
                                <td></td>
                            </tr> 

                                <tr><td>Admin Email:</td>
                                <td><asp:TextBox ID="tbEmail" runat="server" Width='500px' CssClass='textbox' ></asp:TextBox>
                                    <br /><i>Enter the email(s) separated by semicolon</i>
                                </td>
                                <td><%--&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbEmail"
    ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
    ErrorMessage = "Invalid email address" ValidationGroup="Organization" Text="*" />--%>
                                  
                                </td>
                            </tr>

                            <tr>
                                <td>Region:</td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlRegion" CssClass="dropdownlist" DataValueField="id" DataTextField="name" ></asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;<asp:RequiredFieldValidator ForeColor="Red" 
                                        ID="RequiredFieldValidator2" runat="server" ErrorMessage="Region Required" 
                                        SetFocusOnError="True" ValidationGroup="Organization" ControlToValidate="ddlRegion">*</asp:RequiredFieldValidator>
                                </td>
                            </tr> 

                            <tr><td>Active:</td><td><asp:CheckBox runat="server" ID="cbActive" CssClass="cbxActive cb-enhanced nolabel" Text="&nbsp;" Enabled="true" /></td><td></td>
                            </tr>   
          
                             </table>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                            ShowMessageBox="True" ShowSummary="False" ValidationGroup="Organization" />
                   </div>
                </div>
            </div>

        </td></tr>
    </table>                

    <br />

    <table border='0' cellpadding='0' cellspacing='0' class='currentwidth'>
        <tr>
            <td style='text-align:right'>
                <asp:Button ID="btnBackToList2" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btnBackToList_Click" CausesValidation="false" />&nbsp;&nbsp;
                <asp:Button ID="btnSave2" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btnSave_Click"   ValidationGroup="Organization"  />
                <asp:Button ID="btnDone2" runat="server" CssClass="admin-button-blue mw150" Text="Done" onclick="btnDone_Click"  CausesValidation="false" />
            </td>
        </tr>
        
    </table>
</asp:Panel>

    <asp:Panel runat="server" ID="pnlServices">
        <table border='0' cellpadding='0' cellspacing='0' class='currentwidth'>
        <tr>
            
            <td style='text-align:right'>
                <asp:Button ID="btnBackToList3" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btnBackToList_Click" CausesValidation="false" />&nbsp;&nbsp;
                <asp:Button ID="btnDone3" runat="server" CssClass="admin-button-blue mw150" Text="Done" onclick="btnDone_Click"  CausesValidation="false" />
            </td>
        </tr>
        
    </table>
     
    <br />
    <asp:Literal runat="server" ID="litMessageService"></asp:Literal>

        <div class="admin-white-box-inner">
            <asp:Label runat="server" ID="litOrganizationName" CssClass="admin-header-subtitle"></asp:Label><br /><br />
                 <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
			    <tr><td colspan="2">
                    <asp:GridView ID="GV_Services" runat="server" CssClass="admin-grid" DataKeyNames="id" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                        GridLines="None" CellPadding="0" CellSpacing="0"
                        OnRowDataBound="GV_Services_RowDataBound">
                    <HeaderStyle CssClass="admin-grid-header" />
                    <PagerSettings Visible="false" />
                    <FooterStyle/>
                    <Columns>
                         <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Availabilty">
                            <ItemStyle CssClass="itemrow" Width="65" HorizontalAlign="Center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbxActive" CssClass="cbxActive cb-enhanced nolabel" Text="&nbsp;" Enabled="false"  />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Service Offering">
                            <ItemStyle CssClass="itemrow"  HorizontalAlign="left" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:Literal runat="server" ID="litService"></asp:Literal>&nbsp;&nbsp;&nbsp;&nbsp;
                                <img src="/Images/Icons/more-collapse.png" alt="expand" title="expand" class="img-expand-collapse" style="float:right;cursor:pointer;" />
                                <br /><br />
                                <div style="display:none;">
                                    <asp:CheckBoxList runat="server" ID="cblCities" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="cblCities_SelectedIndexChanged"></asp:CheckBoxList>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
		        </table>
            </div>
        <script>
            $(document).ready(function () {
                $('.img-expand-collapse').click(function () {
                    //alert($(this).attr("src"));
                    $(this).siblings('div').toggle();
                    if ($(this).attr("src") == "/Images/Icons/more-collapse.png") {
                        $(this).attr("src", "/Images/Icons/less-collapse.png");
                        $(this).attr("title", "collapse");
                    }
                    else {
                        $(this).attr("src", "/Images/Icons/more-collapse.png");
                        $(this).attr("title", "expand");

                    }
                });
            });
        </script>

        <asp:Literal runat="server" ID="litScript"></asp:Literal>
    </asp:Panel>
</div>


