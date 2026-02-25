<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListAndDetails.ascx.cs" Inherits="Admin_MemberDirectory_ListAndDetails" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>

<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
<style>
    .btnAddLocation, .btnDelLocation{
        font-weight: bold!important;
        font-size: 25px!important;
        padding: 0 10px!important;
        border-radius:50%;
    }
    .btnDelLocation {
        padding: 0px 14px!important;
        font-size: 24px!important;
    }
    
    .tdLocation {
        vertical-align: top;
        padding-top: 15px;
    }
</style>


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
                    <td style="padding-left: 25px;"><asp:LinkButton ID="btnFilter" runat="server" CssClass="admin-button-green mw150" Text="Filter" onclick="btnFilter_Click" />
                        <asp:LinkButton ID="btnClearFilter" runat="server" CssClass="adminHL-button-question" Text="clear filter?" CommandName="Clear" onclick="btnClearFilter_Click"/>
                    </td>
                </tr>
                <tr runat="server" id="trFeaturedFilter">
                    <td class="admin-prompt-right" style="padding-top:0;">Featured</td>
                    <td><asp:CheckBox runat="server" ID="cbxFeatureFilter" CssClass="cbxActive cb-enhanced nolabel" Text="&nbsp;" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbxFeatureFilter_CheckedChanged" /></td>
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
                        
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Image">
                            <ItemStyle CssClass="itemrow" Width="65" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbxImage" CssClass="cbxActive cb-enhanced nolabel" Text="&nbsp;" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Active">
                            <ItemStyle CssClass="itemrow" Width="65" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbxActive" CssClass="cbxActive cb-enhanced nolabel" Text="&nbsp;" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Featured">
                            <ItemStyle CssClass="itemrow" Width="65" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="cbxFeature" CssClass="cbxActive cb-enhanced nolabel" Text="&nbsp;" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit" CommandArgument='<%# Eval("id") %>' CommandName="Editing" CausesValidation="False" ToolTip="Edit" />
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
                $('.btnAddLocation').click(function () {
                    var index = $(this).val();
                    $(".trLocation[value='" + index + "']").show();
                });

                $('.btnDelLocation').click(function () {
                    var index = $(this).val();
                    $(".trLocation[value='" + index + "']").hide();
                    $(".trLocation[value='" + index + "'] textarea").val('');
                    $(".trLocation[value='" + index + "'] input").val('');
                });

                $('#<%=ddlOrganization.ClientID%>').change(function () {

                    if ($('#<%= ddlOrganization.ClientID%>').val() != '') {

                        var myvalue = {
                            id: <%=OrgId%>,
                            lib: $('#<%= ddlOrganization.ClientID%>').val()
                        };

                         $.ajax({
                            type: "POST",
                            url: "/api/organization",
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

                                 <tr>
                                    <td>SEO:</td>
                                    <td><asp:TextBox ID="tbSeo" runat="server" Width='330px' CssClass='textbox' MaxLength="100"></asp:TextBox>&nbsp;
                                        <asp:Button runat="server" ID="btnSeo" CssClass="admin-button-blue mw150" OnClick="btnSeo_Click" Text="Sugest me" />
                                        <br /><i>Only lower case letters and numbers</i>
                                        </td>
                                    <td>&nbsp;<asp:RequiredFieldValidator ForeColor="Red" 
                                            ID="RequiredFieldValidator2" runat="server" ErrorMessage="SEO Required" 
                                            SetFocusOnError="True" ValidationGroup="Organization" ControlToValidate="tbSeo">*</asp:RequiredFieldValidator>
                                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="tbSeo" ValidationGroup="Organization"
                                        ForeColor="Red" ErrorMessage="Invalid SEO" ValidationExpression="^[a-z][a-z0-9]*$" Text="*"></asp:RegularExpressionValidator>
                                    </td>
                                </tr> 

                                <tr>
                                    <td>Address:</td>
                                    <td><asp:TextBox ID="tbAddress" runat="server" Width='500px' CssClass='textbox' MaxLength="500" ></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr> 

                                 <tr>
                                    <td>City:</td>
                                    <td><asp:TextBox ID="tbCity" runat="server" Width='500px' CssClass='textbox' MaxLength="500" ></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr> 

                                <tr>
                                    <td>Postal Code:</td>
                                    <td> <asp:TextBox ID="tbPostal" runat="server" CausesValidation="True" CssClass="textbox" title="Postal code" MaxLength="7" ></asp:TextBox>              
                                    </td>
                                    <td>
                                       <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="tbPostal" ValidationGroup="Organization"
                                        ForeColor="Red" ErrorMessage="Invalid Postel Code" ValidationExpression="[a-zA-Z]\d[a-zA-Z](\s)?\d[a-zA-Z]\d" Text="*"></asp:RegularExpressionValidator>
                                    </td>
                                </tr> 
                                <tr>
                                    <td>Phone Number:</td>
                                    <td><asp:TextBox ID="tbPhone" runat="server" Width='500px' CssClass='textbox' MaxLength="250" ></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr> 

                                <tr>
                                    <td>Fax:</td>
                                    <td><asp:TextBox ID="tbFax" runat="server" Width='500px' CssClass='textbox' MaxLength="250" ></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr> 
                                 <tr>
                                    <td>Toll-free:</td>
                                    <td><asp:TextBox ID="tbTollFree" runat="server" Width='500px' CssClass='textbox' MaxLength="250" ></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr> 
                                 <tr><td>Email:</td>
                                    <td><asp:TextBox ID="tbEmail" runat="server" Width='500px' CssClass='textbox' ></asp:TextBox>
                                    </td>
                                    <td>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbEmail"
        ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
        ErrorMessage = "Invalid email address" ValidationGroup="Organization" Text="*" /></td>
                                </tr>
                           
                                <tr><td>URL:</td>
                                    <td><asp:TextBox ID="tbUrl" runat="server" Width='500px' CssClass='textbox' ></asp:TextBox>
                                    </td>
                                    <td>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="tbUrl"
        ForeColor="Red" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
        ErrorMessage = "Invalid URL" ValidationGroup="Organization" Text="*"/></td>
                                </tr>


                                <tr><td>Facebook:</td>
                                    <td><asp:TextBox ID="tbFacebook" runat="server" Width='500px' CssClass='textbox' ></asp:TextBox>
                                    </td>
                                    <td>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="tbFacebook"
        ForeColor="Red" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
        ErrorMessage = "Invalid Facebook" ValidationGroup="Organization" Text="*"/></td>
                                </tr>
                                <tr><td>Instagram:</td>
                                    <td><asp:TextBox ID="tbInstagram" runat="server" Width='500px' CssClass='textbox' ></asp:TextBox>
                                    </td>
                                    <td>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="tbInstagram"
        ForeColor="Red" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
        ErrorMessage = "Invalid Instagram" ValidationGroup="Organization" Text="*"/></td>
                                </tr>
                                <tr><td>Twitter:</td>
                                    <td><asp:TextBox ID="tbTwitter" runat="server" Width='500px' CssClass='textbox' ></asp:TextBox>
                                    </td>
                                    <td>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="tbTwitter"
        ForeColor="Red" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
        ErrorMessage = "Invalid Twitter" ValidationGroup="Organization" Text="*"/></td>
                                </tr>
                                <tr><td>LinkedIn:</td>
                                    <td><asp:TextBox ID="tbLinkedIn" runat="server" Width='500px' CssClass='textbox' ></asp:TextBox>
                                    </td>
                                    <td>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="tbLinkedIn"
        ForeColor="Red" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
        ErrorMessage = "Invalid LinkedIn" ValidationGroup="Organization" Text="*"/></td>
                                </tr>
                                <tr><td>YouTube:</td>
                                    <td><asp:TextBox ID="tbYouTube" runat="server" Width='500px' CssClass='textbox' ></asp:TextBox>
                                    </td>
                                    <td>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="tbYouTube"
        ForeColor="Red" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
        ErrorMessage = "Invalid YouTube" ValidationGroup="Organization" Text="*"/></td>
                                </tr>




                                <tr>
                                    <td style="vertical-align:top; padding-top:15px;">Mission statement:</td>
                                    <td>
                                        <CE:Editor id="tbMission" runat="server" width="500" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor>
                                    </td>
                                    <td></td>
                                </tr> 
                                <tr>
                                    <td style="vertical-align:top; padding-top:15px;">About Us:</td>
                                    <td>
                                        <CE:Editor id="tbAboutUs" runat="server" width="500" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor>
                                    </td>
                                    <td></td>
                                </tr> 
                                <tr>
                                    <td style="vertical-align:top; padding-top:15px;">Our Story:</td>
                                    <td>
                                        <CE:Editor id="tbOurStory" runat="server" width="500" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:top; padding-top:15px;">Services/Client demographics:</td>
                                    <td>
                                        <CE:Editor id="tbServices" runat="server" width="500" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:top; padding-top:15px;">Our People/Our Team:</td>
                                    <td>
                                        <CE:Editor id="tbOurTeam" runat="server" width="500" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:top; padding-top:15px;">Achievements/Awards and recognition :</td>
                                    <td>
                                        <CE:Editor id="tbAchievements" runat="server" width="500" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:top; padding-top:15px;">Our Partners:</td>
                                    <td>
                                        <CE:Editor id="tbOurPartners" runat="server" width="500" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:top; padding-top:15px;">Learn More about Us:</td>
                                    <td>
                                        <CE:Editor id="tbLearnMoreAboutUs" runat="server" width="500" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor>
                                    </td>
                                    <td></td>
                                </tr>
                               <asp:PlaceHolder runat="server" ID="plDetails">
                               <tr>
                                    <td class="tdLocation">Location 1:</td>
                                    <td colspan="2" style="vertical-align:top;">
                                         <asp:TextBox runat="server" ID="txtTitle1" CssClass='textbox' Width='400px' MaxLength="50"></asp:TextBox><i>Title(if applicable)</i><br />
                                         <asp:TextBox runat="server" ID="txtLoaction1" CssClass='textbox' TextMode="MultiLine" Rows="3" Width='400px' Height="70" MaxLength="200"></asp:TextBox>
                                         <button class="btnAddLocation admin-button-gray" value="1" type="button">+</button>
                                   </td>
                                </tr>

                               <tr class="trLocation" value="1" style="display:none;">
                                    <td class="tdLocation">Location 2:</td>
                                    <td colspan="2" style="vertical-align:top;">
                                         <asp:TextBox runat="server" ID="txtTitle2" CssClass='textbox' Width='400px' MaxLength="50"></asp:TextBox><i>Title(if applicable)</i><br />
                                        <asp:TextBox runat="server" ID="txtLoaction2" CssClass='textbox' TextMode="MultiLine" Rows="3" Width='400px' Height="70" MaxLength="200"></asp:TextBox>
                                        <button class="btnAddLocation admin-button-gray" value="2" type="button">+</button>
                                        <button class="btnDelLocation admin-button-gray" value="1" type="button">-</button>
                                    </td>
                                </tr>

                                <tr class="trLocation" value="2" style="display:none;">
                                    <td class="tdLocation">Location 3:</td>
                                    <td colspan="2" style="vertical-align:top;">
                                         <asp:TextBox runat="server" ID="txtTitle3" CssClass='textbox' Width='400px' MaxLength="50"></asp:TextBox><i>Title(if applicable)</i><br />
                                        <asp:TextBox runat="server" ID="txtLoaction3" CssClass='textbox' TextMode="MultiLine" Rows="3" Width='400px' Height="70" MaxLength="200"></asp:TextBox>
                                        <button class="btnAddLocation admin-button-gray" value="3" type="button">+</button>
                                        <button class="btnDelLocation admin-button-gray" value="2" type="button">-</button>
                                    </td>
                                </tr>
                               <tr class="trLocation" value="3" style="display:none;">
                                    <td class="tdLocation">Location 4:</td>
                                    <td colspan="2" style="vertical-align:top;">
                                         <asp:TextBox runat="server" ID="txtTitle4" CssClass='textbox' Width='400px' MaxLength="50"></asp:TextBox><i>Title(if applicable)</i><br />
                                        <asp:TextBox runat="server" ID="txtLoaction4" CssClass='textbox' TextMode="MultiLine" Rows="3" Width='400px' Height="70" MaxLength="200"></asp:TextBox>
                                        <button class="btnAddLocation admin-button-gray" value="4" type="button">+</button>
                                        <button class="btnDelLocation admin-button-gray" value="3" type="button">-</button>
                                    </td>
                                </tr>
                               <tr class="trLocation" value="4" style="display:none;">
                                    <td class="tdLocation">Location 5:</td>
                                    <td colspan="2" style="vertical-align:top;">
                                         <asp:TextBox runat="server" ID="txtTitle5" CssClass='textbox' Width='400px' MaxLength="50"></asp:TextBox><i>Title(if applicable)</i><br />
                                        <asp:TextBox runat="server" ID="txtLoaction5" CssClass='textbox' TextMode="MultiLine" Rows="3" Width='400px' Height="70" MaxLength="200"></asp:TextBox>
                                        <%--<button class="btnAddLocation admin-button-gray" value="5" type="button">+</button>--%>
                                        <button class="btnDelLocation admin-button-gray" value="4" type="button">-</button>
                                    </td>
                                </tr>
                                </asp:PlaceHolder>


                                <tr><td style="vertical-align:top; padding-top:25px;">Logo:</td>                    
                                    <td><asp:FileUpload runat="server" ID="fuLogo" Width='500px' CssClass='textbox' />
                                        <div>
                                            Alt:&nbsp&nbsp<asp:TextBox ID="tbAltText_Logo" runat="server" Width='470px' CssClass='textbox' MaxLength="200" ></asp:TextBox>
                                        </div>
                                        <div>
                                        <asp:Image runat="server" ID="imgLogo" style="width:100px; height:auto;"></asp:Image>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lnkDeleteLogo" onclick="lnkDeleteLogo_Click" Text="Delete" OnClientClick="return confirm('Are you sure to delete this logo?');"></asp:LinkButton></div> </td><td></td>
                                </tr>
                             
                                <tr><td style="vertical-align:top; padding-top:25px;">Image:</td>                    
                                    <td colspan="2"><asp:FileUpload runat="server" ID="fuImage" Width='500px' CssClass='textbox' />
                                        <div>Background Position:&nbsp;
                                                <asp:DropDownList ID="ddlBackgroundPosition" runat="server" CssClass="dropdownlist" >
                                                <asp:ListItem Text="Center" Value="center"></asp:ListItem>
                                                <asp:ListItem Text="Top" Value="top"></asp:ListItem>
                                                <asp:ListItem Text="Bottom" Value="bottom"></asp:ListItem>
                                            </asp:DropDownList>    
            
                                            <asp:DropDownList ID="ddlBackgroundPosition2" runat="server" CssClass="dropdownlist" >
                                                <asp:ListItem Text="Center" Value="center"></asp:ListItem>
                                                <asp:ListItem Text="Left" Value="left"></asp:ListItem>
                                                <asp:ListItem Text="Right" Value="right"></asp:ListItem>
                                            </asp:DropDownList> 
                                        </div>
                                        <div>
                                            Alt:&nbsp&nbsp<asp:TextBox ID="tbAltText_Img" runat="server" Width='470px' CssClass='textbox' MaxLength="200" ></asp:TextBox>
                                        </div>
                                        <div>
                                        <asp:Image runat="server" ID="imgImage" style="width:100px; height:auto;"></asp:Image>&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="lnkDeleteImage" onclick="lnkDeleteImage_Click" Text="Delete" OnClientClick="return confirm('Are you sure to delete this image?');"></asp:LinkButton></div>                                     
                                    </td>

                                </tr>
                           
                                <tr runat="server" id="trFeature"><td>Feature:</td><td><asp:CheckBox runat="server" ID="cbFeatured" CssClass="cbxActive cb-enhanced nolabel" Text="&nbsp;" Enabled="true" /></td><td></td>
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
</div>


