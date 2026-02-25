<%@ Control Language="C#" AutoEventWireup="true" CodeFile="eFormFields.ascx.cs" Inherits="Admin_eForms_eFormFields" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register Src="eFormDal.ascx" TagName="eFormDal" TagPrefix="uc1" %>
<style type="text/css">
    .eFormField
    {
        cursor: move;
        cursor:hand;
        cursor:grab;
        cursor:-moz-grab;
        cursor:-webkit-grab;
    }
    .eFormField.custom_cursor
    {
        cursor: url(/images/openhand.cur), move;
    }
    .gvFields .trField
    {
        cursor: move;
        cursor:hand;
        cursor:grab;
        cursor:-moz-grab;
        cursor:-webkit-grab;
    }
    .gvFields .trField.custom_cursor
    {
        cursor: url(/images/openhand.cur), move;
    }
    .ui-state-highlight
    {
        border:0px;
    }
</style>
<asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
<ContentTemplate>
    <%if (EditView == "Main"){ %><%=test %>
    <table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td style="vertical-align: top;">
            <div id="tabs">
                <ul class="tab_link">
                    <li><a id="li_Default" href="#tab-Default">Default</a></li>
                    <li><a id="li_Custom" href="#tab-Custom">Custom</a></li>
                </ul>
                <div id="tab-Default">
                    <table id="Table1" class="admin-tbl-eFormField" border="0" cellpadding="0" cellspacing="0">
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton13" runat="server" CssClass="lnk" CommandName="Salutation" CommandArgument="-1" OnCommand="LinkButton1_Click">Salutation</asp:LinkButton></div></td></tr>  
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="lnkName" runat="server" CssClass="lnk" CommandName="Name" CommandArgument="-1" OnCommand="LinkButton1_Click">Name</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="lnkFullName" runat="server" CssClass="lnk" CommandName="FullName" CommandArgument="-1" OnCommand="LinkButton1_Click">Full Name</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton1" runat="server" CssClass="lnk" CommandName="Address" CommandArgument="-1" OnCommand="LinkButton1_Click">Address</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton5" runat="server" CssClass="lnk" CommandName="City" CommandArgument="-1" OnCommand="LinkButton1_Click">City</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton6" runat="server" CssClass="lnk" CommandName="Province" CommandArgument="-1" OnCommand="LinkButton1_Click">Province</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton15" runat="server" CssClass="lnk" CommandName="State" CommandArgument="-1" OnCommand="LinkButton1_Click">State</asp:LinkButton></div></td></tr>                        
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton7" runat="server" CssClass="lnk" CommandName="Country" CommandArgument="-1" OnCommand="LinkButton1_Click">Country</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton2" runat="server" CssClass="lnk" CommandName="Phone" CommandArgument="-1" OnCommand="LinkButton1_Click">Phone</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton9" runat="server" CssClass="lnk" CommandName="Postal" CommandArgument="-1" OnCommand="LinkButton1_Click">Postal Code</asp:LinkButton></div></td></tr>  
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="lnkEmail" runat="server" CssClass="lnk" CommandName="Email" CommandArgument="-1" OnCommand="LinkButton1_Click">Email</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton3" runat="server" CssClass="lnk" CommandName="Website" CommandArgument="-1" OnCommand="LinkButton1_Click">Website</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton10" runat="server" CssClass="lnk" CommandName="AddressBlock" CommandArgument="-1" OnCommand="LinkButton1_Click">Address Block</asp:LinkButton></div></td></tr> 
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton4" runat="server" CssClass="lnk" CommandName="Date" CommandArgument="-1" OnCommand="LinkButton1_Click">Date</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton8" runat="server" CssClass="lnk" CommandName="Comment" CommandArgument="-1" OnCommand="LinkButton1_Click">Comment</asp:LinkButton></div></td></tr>                        
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="lbTerms" runat="server" CssClass="lnk" CommandName="Terms" CommandArgument="-1" OnCommand="LinkButton1_Click">Terms and Conditions</asp:LinkButton></div></td></tr>   
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton11" runat="server" CssClass="lnk" CommandName="Browse" CommandArgument="-1" OnCommand="LinkButton1_Click">File Uploader</asp:LinkButton></div></td></tr> 
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton12" runat="server" CssClass="lnk" CommandName="HorizontalLine" CommandArgument="-1" OnCommand="LinkButton1_Click">Horizontal Line</asp:LinkButton></div></td></tr>
                    <tr class="eFormField"><td><div class="admin-tbl-eFormField-item"><asp:LinkButton ID="LinkButton14" runat="server" CssClass="lnk" CommandName="BlankLine" CommandArgument="-1" OnCommand="LinkButton1_Click">Blank Line</asp:LinkButton></div></td></tr>
                    </table>
                </div>
                <div id="tab-Custom">
                    <table id="Table2" class="admin-tbl-eFormField" border="0" cellpadding="0" cellspacing="0">
                    <tr class="eFormField custom"><td><div class="admin-tbl-eFormField-item"><a id="aTextBox" href="javascript:void(0);" rel="text" title=""  class="lnk CustomLink">Text Input</a></div></td></tr>
                    <tr class="eFormField custom"><td><div class="admin-tbl-eFormField-item"><a id="aTextArea" href="javascript:void(0);" rel="text" title="" class="lnk CustomLink">Multiline Text Input</a></div></td></tr>
                    <tr class="eFormField custom"><td><div class="admin-tbl-eFormField-item"><a id="aTitle" href="javascript:void(0);" rel="text" title="" class="lnk CustomLink">Insert Title</a></div></td></tr>
                    <tr class="eFormField custom"><td><div class="admin-tbl-eFormField-item"><a id="aLabel" href="javascript:void(0);" rel="text" title="" class="lnk CustomLink">Insert Content</a></div></td></tr>
                    <tr class="eFormField custom"><td><div class="admin-tbl-eFormField-item"><a id="aCheckBox" href="javascript:void(0);" rel="text" title="" class="lnk CustomLink">Check Box</a></div></td></tr>
                    <tr class="eFormField custom"><td><div class="admin-tbl-eFormField-item"><a id="aCheckboxList" href="javascript:void(0);" rel="text" title="" class="lnk CustomLink">Checkbox List</a></div></td></tr>
                    <tr class="eFormField custom"><td><div class="admin-tbl-eFormField-item"><a id="aRadioButton" href="javascript:void(0);" rel="text" title="" class="lnk CustomLink">Radio Buttons</a></div></td></tr>
                    <tr class="eFormField custom"><td><div class="admin-tbl-eFormField-item"><a id="aDropDown" href="javascript:void(0);" rel="text" title="" class="lnk CustomLink">Drop Down</a></div></td></tr>
                    <tr class="eFormField custom"><td><div class="admin-tbl-eFormField-item"><a id="aNumeric" href="javascript:void(0);" rel="text" title="" class="lnk CustomLink">Numeric UpDown</a></div></td></tr>
                    <tr class="eFormField custom"><td><div class="admin-tbl-eFormField-item"><a id="aListBox" href="javascript:void(0);" rel="text" title="" class="lnk CustomLink">List Box</a></div></td></tr>
                    <tr class="eFormField custom" style="display: none"><td><div class="admin-tbl-eFormField-item"><a id="aPassword" href="javascript:void(0);" rel="text" title="" class="lnk CustomLink">Password</a></div></td></tr>
                    </table>
                    <asp:HiddenField ID="hFieldType" runat="server" />
                    <asp:HiddenField ID="hOptions" runat="server" />
                    <asp:HiddenField ID="hPriority" runat="server" Value="-1" />
                </div>
            </div>
        </td>
        <td style="vertical-align: top; padding-left: 25px;">
            <div class="admin-white-box">
                <div class="admin-white-box-header">Fields</div>
                <div class="admin-white-box-inner">
                    <asp:Panel ID='pnlFields' runat="server">
                        <asp:GridView ID="gvFields" CssClass="gvFields admin-grid" runat="server" CellPadding="0" DataKeyNames="id,prompt,required,direction,type,caption"
                            OnRowDataBound="gvFields_RowDataBound" AutoGenerateColumns="False" GridLines="None"
                            OnRowDeleting="gvFields_RowDeleting" OnRowEditing="gvFields_RowEditing" OnRowCommand="gvFields_RowCommand">
                            <HeaderStyle CssClass="admin-grid-header" />
                            <PagerSettings Visible="false" />
                            <RowStyle CssClass="trField" />
                            <EmptyDataTemplate>
                            <table border="0" cellpadding="0" cellspacing="0">
                            <%--<tr class="padbottom" style="height:30px; vertical-align:top; background-color:#0078d8;"><td>&nbsp;</td></tr>--%>
                            <tr><td class="admin-title" style="height:100px; padding:20px; text-align:center;">Add fields by dragging the fields from<br />the left sidebar to this area.</td></tr>
                            </table>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Caption" ShowHeader="False">
                                    <ItemStyle CssClass="itemrow" Width="350" />
                                    <ItemTemplate>
                                        <asp:Literal ID='lblCaption' runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>     
                                <%--<asp:TemplateField HeaderText="" ShowHeader="False" Visible="false"> 
                                    <HeaderStyle CssClass="TemplateGridHeaderFont" />
                                    <ItemStyle CssClass="itemrow" />
                                    <ItemTemplate>
                                        <asp:Literal ID='lblField' runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:BoundField ItemStyle-CssClass="itemrow" DataField="type" />--%>
                                <asp:TemplateField HeaderText="Logic" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"> 
                                    <ItemStyle CssClass="itemrow" />
                                    <ItemTemplate>
                                        <asp:Literal ID='litLogic' runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgUP" runat="server" CommandName="Up" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/move-up.png" AlternateText="Move Up" ToolTip="Move Up" />
                                        <asp:ImageButton ID="imgDown" runat="server" CommandName="Down" CommandArgument='<%#Eval("id") %>' ImageUrl="/images/lemonaid/buttonsNew/move-down.png" AlternateText="Move Down" ToolTip="Move Down" />
                                        <asp:ImageButton ID="imgEdit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" border="0" alt='edit' runat="server" CommandName="Edit" CommandArgument='<%#Eval("id") %>' ToolTip='Edit' style="cursor:pointer;" />
                                        <asp:ImageButton ID="LB_Delete" runat="server" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip='Delete' style="cursor:pointer;"/>
                                        <asp:HiddenField ID="hfId" runat="server" Visible="true" Value='<%# Eval("id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
            <br />
            <div class="admin-white-box">
                <div class="admin-white-box-header">Custom Thank You Message</div>
                <div class="admin-white-box-inner">
                    <table cellpadding="0" cellspacing="0" border="0">
                    <tr><td><asp:TextBox ID="tbThankyou" runat="server" CssClass="textbox" TextMode="MultiLine" Width="547" Height="150" /></td></tr>
                    </table>
                    <div class="ar"><asp:LinkButton ID="btBack2" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="imgBack_Click" CausesValidation="false" /></div>
                </div>
            </div>
        </td>
    </tr>
    </table>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#tabs").tabs();
        });
    </script>
    <%} %>
    <%if (EditView == "Edit") { %>
    <div class="admin-white-box">
        <div class="admin-white-box-header">Field Type:&nbsp;<asp:Literal ID="litFieldType" runat="server" /></div>
        <div class="admin-white-box-inner">
            <table id="Table3" border="0" cellpadding="0" cellspacing="0">
            <tr><td class="admin-prompt-right"><asp:Literal ID="lbCaption" runat="server">Caption</asp:Literal></td><td><asp:TextBox ID="tbTitle" runat="server" CssClass="textbox" Width="250px"></asp:TextBox></td></tr>
            <% if (RQ) { %>
            <tr><td class="admin-prompt-right">Required</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbMandatory" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
            <% } %>
            <% if (isNumeric){ %>
            <tr><td class="admin-prompt-right">Type</td><td style="padding-top: 15px;"><asp:RadioButtonList ID="rblNumericType" CssClass="rb-enhanced" runat="server" RepeatDirection="Horizontal"><asp:ListItem Text="Numeric" Value="0" Selected="True" /><asp:ListItem Text="Custom List" Value="1" /></asp:RadioButtonList></td></tr>
            <tr class="NumericType"><td class="admin-prompt-right">Minimum</td><td><asp:TextBox ID="tbMinimum" Text="0" runat="server" CssClass="textbox" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" TargetControlID="tbMinimum" /></td></tr>
            <tr class="NumericType"><td class="admin-prompt-right">Maximum</td><td><asp:TextBox ID="tbMaximum" Text="10" runat="server" CssClass="textbox" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" TargetControlID="tbMaximum" /></td></tr>
            <tr class="CustomType"><td class="admin-prompt-right">Items</td><td><asp:TextBox ID="tbNumericItems" Text="" runat="server" CssClass="textbox" Width="250px"></asp:TextBox><ACT:TextBoxWatermarkExtender TargetControlID="tbNumericItems" WatermarkText="list of items separated by semicolons" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="TextBoxWatermarkExtender1"></ACT:TextBoxWatermarkExtender></td></tr>
            <tr class="hide"><td class="admin-prompt-right">Step</td><td><asp:TextBox ID="tbStep" Text="1" runat="server" CssClass="textbox" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Numbers" TargetControlID="tbStep" /></td></tr>
            <tr><td class="admin-prompt-right">Width</td><td><asp:TextBox ID="tbWidth" Text="100" runat="server" CssClass="textbox" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Numbers" TargetControlID="tbWidth" /></td></tr>
            <% } %>
            <% if (MWL) { %>
            <tr><td class="admin-prompt-right">Word limit</td><td><asp:TextBox ID="tbMaxWords" Text="0" runat="server" CssClass="textbox" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Numbers" TargetControlID="tbMaxWords" /></td></tr>
            <% } %>
            <% if (EM) { %> <!-- Show id field is email -->
            <tr><td class="admin-prompt-right">Sign up?</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbSignUp" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
            <tr id="trMailGroups" class="hide"><td class="admin-prompt-right">Mailer Group</td><td><asp:DropDownList ID="ddlMailGroups" runat="server" Width="150" CssClass="dropdownlist"></asp:DropDownList></td></tr>
            <script type="text/javascript">
                $(document).ready(function () {
                    var cbSignUpOnChange = function () {
                        var isChecked = $('#<%= cbSignUp.ClientID %>').is(':checked');
                        $('#trMailGroups').toggleClass('hide', !isChecked);
                    }
                    $('#<%= cbSignUp.ClientID %>').change(function () {
                        cbSignUpOnChange();
                    });
                    cbSignUpOnChange();
                });
            </script>
            <% } %>
            <% if (RB) { %>
            <tr><td class="admin-prompt-right">Direction</td><td style="padding-top: 15px;"><asp:RadioButton ID="rbV2" runat="server" CssClass="rb-enhanced" Text="Vertical" GroupName="direction" /><asp:RadioButton ID="rbH2" runat="server" CssClass="rb-enhanced" Text="Horizontal" GroupName="direction"/></td></tr>
            <% } %>
            <% if (OPT) { %>
            <tr><td class="admin-prompt-right" style="vertical-align:top;">Options</td><td><asp:TextBox ID="tbOptions2" runat="server" CssClass="textbox" TextMode="MultiLine" style="width:250px; height:100px;"></asp:TextBox></td></tr>
            <% } %>
            <% if (isListBox) { %>
            <tr><td class="admin-prompt-right">Rows</td><td><asp:TextBox ID="txtRows" Text="10" runat="server" CssClass="textbox" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" FilterType="Numbers" TargetControlID="txtRows" /></td></tr>
            <tr><td class="admin-prompt-right">Multiple Selection</td><td style="padding-top: 15px;"><asp:CheckBox ID="cbMultiple" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td></tr>
            <% } %>
            <asp:HiddenField ID="hfType2" runat="server" />
            </table>
        </div>
    </div>
    <br />
    <div class="admin-white-box">
        <div class="admin-white-box-header">Logic</div>
        <div class="admin-white-box-inner">
            <table border="0" cellpadding="0" cellspacing="0">
            <% if (Logic) { %>
            <tr><td><asp:DropDownList ID="ddlShow" runat="server" CssClass="dropdownlist" Width="85"><asp:ListItem Text="Show" Value="show" /><asp:ListItem Text="Hide" Value="hide" /></asp:DropDownList><span class="textbox">this field when</span></td></tr>
            <tr><td><asp:DropDownList ID="ddlAndOr" runat="server" CssClass="dropdownlist" Width="85"><asp:ListItem Text="All" Value="and" /><asp:ListItem Text="Any" Value="or" /></asp:DropDownList><span class="textbox">of the following rules match</span></td></tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="ibAddLogic" runat="server" CssClass="admin-button-blue" Text="Add Rule" OnClick="ibAddLogic_Click" CausesValidation="false" />
                        <br /><br />
                        <asp:GridView ID="gvLogic" CssClass="gvLogic admin-grid" runat="server" CellPadding="0" DataKeyNames="id" 
                            OnRowDataBound="gvLogic_RowDataBound" AutoGenerateColumns="False" GridLines="None"
                            OnRowDeleting="gvLogic_RowDeleting" ShowHeader="False">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle CssClass="itemrow" Width="200" />
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlField" runat="server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlField_SelectedIndexChanged" Width="200px"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>     
                                <asp:TemplateField>
                                    <ItemStyle CssClass="itemrow" Width="170" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlOperator" runat="server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlOperator_SelectedIndexChanged" Width="160px"><%--<asp:ListItem Text="is" Value="=" /><asp:ListItem Text="is not" Value="!=" />--%></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>     
                                <asp:TemplateField>
                                    <ItemStyle CssClass="itemrow" Width="200" />
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlValue" runat="server" CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlValue_SelectedIndexChanged" Width="200px"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="LB_Delete" runat="server" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png" ToolTip="Delete" style="cursor:pointer;"/>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="itemrow" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemStyle Width="10"></ItemStyle></asp:TemplateField>
                                <asp:TemplateField ><ItemTemplate><img src="/images/lemonaid/partials/shadowr.png" height="49" width="4" /></ItemTemplate></asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <%--<tr><td class="admin-prompt-right">Name:</td><td><table border="0" cellpadding="0" cellspacing="0"><tr><td><img src="/images/lemonaid/partials/gridrow_left2.png" /></td><td style="background-image:url('/images/lemonaid/partials/gridrow_bg2.png');background-repeat:repeat-x;" valign="middle"><asp:TextBox Width="300" CssClass="textbox" ID="txtName" runat="server" /><asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name required" Display="None" SetFocusOnError="True" ValidationGroup="EditFrom"></asp:RequiredFieldValidator></td><td><img src="/images/lemonaid/partials/gridrow_right2.png" /></td></tr></table></td></tr>--%>
            <% }else{ %>
            <tr><td class="textbox">Conditional Logic can only be used when a<br />select list, checkbox, radio, or number field is on the form</td></tr>
            <% } %>
            </table>
        </div>
    </div>
    <br />
    <div>
        <asp:LinkButton ID="ibCancel" runat="server" CssClass="admin-button-gray mw150" Text="Back" OnClick="ibCancel_Click" CausesValidation="false" />
        <asp:LinkButton ID="ibSave" runat="server" CssClass="admin-button-green mw150" Text="Save" OnClick="ibSave_Click" CausesValidation="false" />
    </div>
    <script type="text/javascript">
        $('#divBack2').hide();
        $('#divBack3').hide();
    </script>        
    <%} %>

    <%--<div id="calendarPlaceHolder" style="position: absolute; top: 0; left: 0; z-index: 1000;
        visibility: hidden; background-color: white;">
    </div>--%>
    
    <div id="divFields" class="admin-modal">
        <div class="SemiTransparentBg">&nbsp;</div> <%-- Set the semitransparent background for the parent div in order to allow opaque content --%>
        <div style="position:absolute; top:50%; width: 100%; text-align: center;">
            <div class="admin-modal-inner" style="text-align: left;"> <%-- Set top = -height/2 to center the inner div --%>
                <div id='divClose' class="admin-modal-close"><img title="Close" alt="Close" src="/images/icons/close-icon-white.png" /></div>
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td class="admin-prompt-right" style="width: 30%;">Caption</td>
                        <td id="tdRegular">
                            <asp:TextBox ID="tbCaption" runat="server" CssClass="textbox" Width="250"></asp:TextBox>
                        </td>
                        <td id="tdBlurb"><asp:TextBox ID="tbCaption2" runat="server" CssClass="textbox" TextMode="MultiLine" style="width: 250px; height: 100px;"></asp:TextBox></td>
                    </tr>
                    <tr id="rwRequired">
                        <td class="admin-prompt-right" style="width: 30%;">Required</td>
                        <td style="padding-top: 15px;">
                            <asp:CheckBox ID="cbRequired" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" />
                        </td>
                    </tr>
                    <tr id="rwDirection" class="hide">
                        <td class="admin-prompt-right" style="width: 30%;">Direction</td>
                        <td style="padding-top: 15px;">
                            <asp:RadioButton ID="rbV" runat="server" CssClass="rb-enhanced" Text="Vertical" Checked="True" GroupName="rbDirection" />
                            <asp:RadioButton ID="rbH" runat="server" CssClass="rb-enhanced" Text="Horizontal" GroupName="rbDirection" />
                        </td>
                    </tr>
                </table>
                <table id="tbOptions" border="0" cellspacing="0" cellpadding="0" class="hide" style="width: 100%;">
                    <tr>
                        <td class="admin-prompt-right" style="width: 30%;"><span id='spOptios'>Items</span></td>
                        <td>
                            <%--<asp:ListBox ID="lbOptions" runat="server" Width="180px" style="display:none"></asp:ListBox>--%>
                            <asp:TextBox ID="txOptions" runat="server" CssClass="textbox" TextMode="MultiLine" style="width: 250px; height: 100px;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="admin-prompt-right" style="width: 30%;">item</td>
                        <td>
                            <input id="ipOptions" type="text" /><input id="btOptions" type="button" value="Add" />
                        </td>
                    </tr>
                </table>
                <table id="tbCbOptions" border="0" cellspacing="0" cellpadding="0" class="hide" style="width: 100%;">
                    <tr>
                        <td class="admin-prompt-right" style="width: 30%;"><span id='spCbOptions'>Items</span></td>
                        <td>
                            <%--<asp:ListBox ID="lbCbOptions" runat="server" Width="180px" style="display:none"></asp:ListBox>--%>
                            <asp:TextBox ID="txtCbOptions" runat="server" CssClass="textbox" TextMode="MultiLine" style="width: 250px; height: 100px;"></asp:TextBox>
                        </td>
                    </tr>                           
                </table>
                <table id="tblNumericOptions" border="0" cellspacing="0" cellpadding="0" class="hide" style="width: 100%;">
                <tr>
                    <td class="admin-prompt-right" style="width: 30%;">Type</td>
                    <td style="padding-top: 15px;"><asp:RadioButtonList ID="rblNumericType2" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal"><asp:ListItem Text="Numeric" Value="0" Selected="True" /><asp:ListItem Text="Custom List" Value="1" /></asp:RadioButtonList></td>
                </tr>
                <tr class="NumericType2">
                    <td class="admin-prompt-right" style="width: 30%;">Minimum</td>
                    <td><asp:TextBox ID="tbMinimum2" runat="server" CssClass="textbox" Text="0" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers" TargetControlID="tbMinimum2" /></td>
                </tr>
                <tr class="NumericType2">
                    <td class="admin-prompt-right" style="width: 30%;">Maximum</td>
                    <td><asp:TextBox ID="tbMaximum2" runat="server" CssClass="textbox" Text="10" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers" TargetControlID="tbMaximum2" /></td>
                </tr>
                <tr class="CustomType2">
                    <td class="admin-prompt-right" style="width: 30%;">Items</td>
                    <td><asp:TextBox ID="tbNumericItems2" CssClass="textbox" Text="" runat="server" Width="250px"></asp:TextBox><ACT:TextBoxWatermarkExtender TargetControlID="tbNumericItems2" WatermarkText="list of items separated by semicolons" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="TextBoxWatermarkExtender2"></ACT:TextBoxWatermarkExtender></td>
                </tr>
                <tr class="hide">
                    <td class="admin-prompt-right" style="width: 30%;">Step</td>
                    <td><asp:TextBox ID="tbStep2" runat="server" CssClass="textbox" Text="1" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers" TargetControlID="tbStep2" /></td>
                </tr>
                <tr>
                    <td class="admin-prompt-right" style="width: 30%;">Width</td>
                    <td><asp:TextBox ID="tbWidth2" runat="server" CssClass="textbox" Text="100" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Numbers" TargetControlID="tbWidth2" /></td>
                </tr>
                </table>
                <table id="tblListBoxOptions" border="0" cellspacing="0" cellpadding="0" class="hide" style="width: 100%;">
                <tr>
                    <td class="admin-prompt-right" style="width: 30%;">Rows</td>
                    <td><asp:TextBox ID="txtRows2" runat="server" CssClass="textbox" Text="10" Width="250px"></asp:TextBox><ACT:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterType="Numbers" TargetControlID="txtRows2" /></td>
                </tr>
                <tr>
                    <td class="admin-prompt-right" style="width: 30%;">Multiple Selection</td>
                    <td style="padding-top: 15px;"><asp:CheckBox ID="cbMultiple2" runat="server" CssClass="cb-enhanced nolabel" Text="&nbsp;" /></td>
                </tr>
                </table>
                <div class="ar" style="padding-top: 20px;"><asp:LinkButton ID="btSubmit" runat="server" CssClass="admin-button-green" Text="Add" OnClick="btSubmit_Click" /></div>
            </div>
        </div>
    </div>
</ContentTemplate>
</asp:UpdatePanel>

<uc1:eFormDal ID="eFormDAL" runat="server" />
<script type="text/javascript">
    var CancelSorting = false;

    function ie_ver() {
        var iev = 0;
        var ieold = (/MSIE (\d+\.\d+);/.test(navigator.userAgent));
        var trident = !!navigator.userAgent.match(/Trident\/7.0/);
        var rv = navigator.userAgent.indexOf("rv:11.0");

        if (ieold) iev = new Number(RegExp.$1);
        if (navigator.appVersion.indexOf("MSIE 10") != -1) iev = 10;
        if (trident && rv != -1) iev = 11;

        return iev;
    }
    var IEVersion = ie_ver();

    function BindEFFControlEvents() {
        $('#<%= gvFields.ClientID %>').find('tr').each(function () {
            var id = $(this).find("input[id$='hfId']").val();
            if (id != null && id != 'undefined')
                $(this).attr('id', 'tr_' + id);
        });

        $('.CustomLink').click(function () {
            var fieldType = $(this).attr('id').substr(1, this.id.length - 1);
            $('#<%= hFieldType.ClientID %>').val(fieldType);

            $('#tblListBoxOptions').hide();
            $('#tblNumericOptions').hide();
            $('#tbCbOptions').hide();
            $('#tbOptions').hide();
            $('#rwDirection').hide();
            $('#tdRegular').show();
            $('#tdBlurb').hide();
            $('#rwRequired').show();

            if (fieldType == "DropDown") {
                $('#tbOptions').show();
            }

            if (fieldType == "RadioButton") {
                $('#tbOptions').show();
                $('#rwDirection').show();
            }
            if (fieldType == "Label") {
                $('#tdRegular').hide();
                $('#tdBlurb').show();
                $('#rwRequired').hide();
            }
            if (fieldType == "Title") {
                $('#rwRequired').hide();
            }
            if (fieldType == "CheckboxList") {
                //$('#tbCbOptions').show();
                //$('#rwRequired').hide();
                 $('#tbOptions').show();
                $('#rwDirection').show();
            }
            if (fieldType == "Numeric") {
                $('#tblNumericOptions').show();
                $('#rwRequired').hide();
            }
            if (fieldType == "ListBox") {
                $('#tbOptions').show();
                $('#tblListBoxOptions').show();
            }
            
            //$('#divFields').css({ top: $(this).position().top + 20, left: $(this).position().left });
            //$('#divFields').css({ top: $(this).offset().top + 20, left: $(this).offset().left });
            $('#divFields').show();
            $('#divFields .admin-modal-inner').css({ top: -1 * $('#divFields .admin-modal-inner').height() / 2 });
            //$('#divFields').slideDown();
        });
        $('#divClose').click(function () {
            $('.gvFields tr.eFormField').remove();
            //$('#divFields').slideUp();
            $('#divFields').hide();
        });

        $('input[name="<%= rblNumericType.ClientID.Replace("_", "$") %>"]:radio').change(function () {
            rblNumericTypeOnChange();
        });
        rblNumericTypeOnChange();

        $('input[name="<%= rblNumericType2.ClientID.Replace("_", "$") %>"]:radio').change(function () {
            rblNumericType2OnChange();
        });
        rblNumericType2OnChange();
        /*$('#%= rblNumericType.ClientID %>').change(function () {
            //alert($(this).find('input[type="radio"]:checked').val());
            rblNumericTypeOnChange($(this));
        });
        rblNumericTypeOnChange($('#%= rblNumericType.ClientID %>'));*/
    }

    /*var rblNumericTypeOnChange = function (ctrl) {
        if (ctrl.find('input[type="radio"]:checked').val() == "0") {
            $('.NumericType').show();
            $('.CustomType').hide();
        }
        else {
            $('.NumericType').hide();
            $('.CustomType').show();
        }
    }*/

    var rblNumericTypeOnChange = function () {
        if ($('input[name="<%= rblNumericType.ClientID.Replace("_", "$") %>"]:checked').val() == "0") {
            $('.NumericType').show();
            $('.CustomType').hide();
        }
        else {
            $('.NumericType').hide();
            $('.CustomType').show();
        }
    }

    var rblNumericType2OnChange = function () {
        if ($('input[name="<%= rblNumericType2.ClientID.Replace("_", "$") %>"]:checked').val() == "0") {
            $('.NumericType2').show();
            $('.CustomType2').hide();
        }
        else {
            $('.NumericType2').hide();
            $('.CustomType2').show();
        }
    }

    //Initial bind
    $(document).ready(function () {
        BindEFFControlEvents();

        //Check if browser is IE
        if (IEVersion > 0) {
            $('.trField').addClass('custom_cursor');
            $('.eFormField').addClass('custom_cursor');
        }

        $('.eFormField').draggable({
            addClasses: false,
            cursor: "grabbing",
            cursorAt: { top: 10, left: 10 },
            helper: "clone",
            /*helper: function (e, ui) {
                //return $(this).clone();
                return $('<tr class="eFormField"><td align="right" style="background-color:#409AE2;width:15px;">&nbsp;</td><td align="right" style="background-color:#409AE2;"><img src="/images/lemonaid/partials/gridrow_left.png"></td><td valign="middle" align="left" style="width:1px;" class="itemrow">&nbsp;</td><td style="width:200px;" class="itemrow"><span>' + $(this).find('a').text() + '</span></td><td align="right" style="width:10px;" class="itemrow">&nbsp;</td><td style="background-color:#409AE2;"><img src="/images/lemonaid/partials/gridrow_right.png"></td><td align="right" style="background-color:#409AE2;width:10px;">&nbsp;</td><td align="center" style="background-color:#409AE2;">&nbsp;</td><td align="right" style="background-color:#409AE2;width:10px;">&nbsp;</td><td align="center" style="background-color:#409AE2;">&nbsp;</td><td align="right" style="background-color:#409AE2;width:10px;">&nbsp;</td><td><img width="4" height="49" src="/images/lemonaid/partials/shadowr.png"></td></tr>');
            },*/
            appendTo: "body",
            connectToSortable: ".gvFields > tbody",
            zIndex: 99999999999
            /*start: function (event, ui) {
            ui.helper.css("list-style", "none");
            $('.WidgetDD').find('ul').slideUp(400);
            }*/
        });

        var DraggingCursor = null;
        //Check if browser is IE
        if (IEVersion > 0) {
            DraggingCursor = "url(/images/closedhand.cur), move";
        }
        //Check if browser is Chrome or not
        else if (navigator.userAgent.search("Chrome") >= 0 || navigator.userAgent.search("Safari") >= 0) {
            DraggingCursor = "-webkit-grabbing";
        }
        //Check if browser is Opera or not
        else if (navigator.userAgent.search("Opera") >= 0) {
            DraggingCursor = "move";
        }
        if (DraggingCursor != null) {
            //$('#%= pnlWidgetToolbar.ClientID %>').draggable("option", "cursor", DraggingCursor);
            $('.eFormField').each(function () {
                $(this).draggable("option", "cursor", DraggingCursor);
            });
        }

        //$('.DroppableContent').sortable({
        $('.gvFields > tbody').sortable({
            items: "> .trField"
            , cursor: 'grabbing'
            //, handle: '.widget_handle'
            //, cancel: 'input,textarea,button,select,option'
            , revert: true
            , tolerance: 'pointer'
            , cursorAt: { top: 1 }
            //, placeholder: "ui-state-highlight"
            , placeholder: {
                element: function (currentItem) {
                    //return $('<tr class="ui-state-highlight" style="z-index:-1;"><td colspan="12">I am modifying the placeholder element!</td></tr>')[0];
                    //return $('<tr><td style="background-color:#409AE2;width:1px;">&nbsp;</td><td colspan="10" class="ui-state-highlight">I am modifying the placeholder element!</td><td align="right" style="background-color:#409AE2;"><img width="4" height="20" src="/images/lemonaid/partials/shadowr.png"></td></tr>')[0];
                    //return $('<tr><td colspan="11" class="ui-state-highlight">&nbsp;</td><td align="right" class="ui-state-highlight" style="background-color:#409AE2; width:4px;"><img width="4px" height="20px" src="/images/lemonaid/partials/shadowr.png"></td></tr>')[0];
                    return $('<tr><td colspan="3" class="ui-state-highlight">&nbsp;</td></tr>')[0];
                },
                update: function (container, p) {
                    return;
                }
            }
            , forcePlaceholderSize: true
            , zIndex: 99999999999
            , update: function (e, ui) {
                
                if (ui.item.attr('id') != null && ui.item.attr('id') != 'undefined' && ui.item.attr('id') != '' && !CancelSorting) {
                    var ZoneId = ui.item.attr('ZoneId');

                    var ReorderedIds = $(this).sortable("toArray").toString().replace(/tr_/gi, "");
                    //alert(ReorderedIds);

                    $.ajax({
                        type: "POST",
                        url: "admin.aspx/ReordereFormFields",
                        data: JSON.stringify({ 'Reorder': ReorderedIds }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d != "")
                                alert(response.d);
                        },
                        error: function (xhr, status, errorThrown) {
                            alert(status + " | " + xhr.responseText);
                        }
                    });
                }
            }
            , receive: function (event, ui) {
                $('body').css('cursor', 'auto');

                var eFormField = $(this).find('.eFormField');

                if (CancelSorting) {
                    CancelSorting = false;
                    $(this).sortable("cancel");
                    eFormField.remove();
                    return;
                }

                var newIndex = parseInt($(this).data("ui-sortable").currentItem.index());
                //alert(newIndex);

                if (eFormField.hasClass("custom")) {
                    $('#<%= hPriority.ClientID %>').val(newIndex);
                    ui.item.find('a').click();
                }
                else {
                    //WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(ui.item.find('a').attr('id').replace(/_/g, '$'), "", true, "", "", false, true));
                    //WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(ui.item.find('a').attr('id').replace(/_/g, '$'), newIndex, true, "", "", false, true));
                    __doPostBack(ui.item.find('a').attr('id').replace(/_/g, '$'), newIndex);
                }

            }
            /*, over: function (event, ui) {
                ui.placeholder.removeClass("hide-placeholder");
            }
            , out: function (event, ui) {
                ui.placeholder.addClass("hide-placeholder");
            }*/
            , start: function (event, ui) {
                //ui.placeholder.html('<tr><td colspan="12">I am modifying the placeholder element!</td></tr>');
                //ui.placeholder.html("I'm modifying the placeholder element!");
            }
            , stop: function (event, ui) {
                if (ui.item.hasClass("eFormField")) {
                    //ui.item.replaceWith('<tr><td colspan="11">Hello</td></tr>');
                    var theLink = ui.item.find('a');
                    //ui.item.replaceWith('<tr class="eFormField"><td align="right" style="background-color:#409AE2;width:15px;">&nbsp;</td><td align="right" style="background-color:#409AE2;"><img src="/images/lemonaid/partials/gridrow_left.png"></td><td valign="middle" align="left" style="width:1px;" class="itemrow">&nbsp;</td><td style="width:200px;" class="itemrow"><span>' + theLink.text() + '</span></td><td align="right" style="width:10px;" class="itemrow">&nbsp;</td><td style="background-color:#409AE2;"><img src="/images/lemonaid/partials/gridrow_right.png"></td><td align="right" style="background-color:#409AE2;width:10px;">&nbsp;</td><td align="center" style="background-color:#409AE2;">&nbsp;</td><td align="right" style="background-color:#409AE2;width:10px;">&nbsp;</td><td align="center" style="background-color:#409AE2;">&nbsp;</td><td align="right" style="background-color:#409AE2;width:10px;">&nbsp;</td><td align="right" style="background-color:#409AE2;"><img width="4" height="49" src="/images/lemonaid/partials/shadowr.png"></td></tr>');
                    ui.item.replaceWith('<tr class="eFormField"><td style="width:350px;" class="itemrow"><span>' + theLink.text() + '</span></td><td class="itemrow" style="width:75px;" align="center">&nbsp;</td><td class="itemrow" align="center">&nbsp;</td></tr>');
                }

                if (CancelSorting) {
                    CancelSorting = false;
                    $(this).sortable("cancel");
                    if (!$(".gvFields > tbody").sortable("option", "revert"))
                        $(".gvFields > tbody").sortable("option", "revert", true);
                }
            }
        });

        if (DraggingCursor != null) {
            $('.gvFields > tbody').sortable("option", "cursor", DraggingCursor);
        }

        $(document).keydown(function (e) {
            //e = (e) ? e : window.event;
            var charCode = (e.which) ? e.which : e.keyCode;
            if (charCode == 27) {
                CancelSorting = true;
                $('.gvFields > tbody').trigger('mouseup');
                $('.eFormField').trigger('mouseup');
            }
        });
    });

    //Re-bind for callbacks 
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    prm.add_endRequest(function () {
        BindEFFControlEvents();
    });
</script>

