<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Users.ascx.cs" Inherits="Admin_Users_Users" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ACT" %>
<%@ Register Namespace="ASPnetControls" Assembly="ASPnetPagerV2_8" TagPrefix="cc" %>
<%--<%@ Register Assembly="UrisoftValidators" Namespace="UrisoftValidators" TagPrefix="USV" %>--%>
<link href="/CSS/pagerNew.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sm" runat="server" EnablePartialRendering="false"></asp:ScriptManager>
<asp:UpdatePanel runat="server" ID="up1">
<ContentTemplate>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Users</div>
    <div class="admin-header-subtitle">Here you can find information on the members of your website, as well as create new ones.<br />You can also control their access levels throughout the site.</div>
</div>
<div class="admin-control-wrapper">
<table border="0">
    <tr style="display:none;">
        <td align="left">
            <asp:Label ID="lbl_msg" runat="server" CssClass="alert"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <table id="tbl_Grid" class="style3" runat ="server"  >
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <div class="admin-white-box">
                                <div class="admin-white-box-header">Filter</div>
                                <div class="admin-white-box-inner">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="admin-prompt-right">Text</td>
                                        <td><asp:TextBox ID="txtFilter" runat="server" OnTextChanged="filter" AutoPostBack="true" CssClass="textbox" style="width:238px;"/><ACT:TextBoxWatermarkExtender TargetControlID="txtFilter" WatermarkText="username or name" WatermarkCssClass="watermarked" runat="server" Enabled="True" ID="TextBoxWatermarkExtender1" /></td>
                                        <td style="padding-left: 25px;"><asp:DropDownList ID="ddlGroups" runat="server" CssClass="dropdownlist" Width="200"></asp:DropDownList></td>
                                        <td style="padding-left: 25px;"><asp:LinkButton ID="ImageOverButton1" runat="server" CssClass="admin-button-green mw150" Text="Filter" onclick="filter" /></td>
                                    </tr>
                                    </table>
                                </div>
                            </div>
					    </td>
					</tr>
					<tr>
					    <td>
                            <div class="admin-white-box" style="min-width: 640px;">
                                <div class="admin-white-box-header">
                                    <asp:LinkButton ID="bttn_Add_Question" runat="server" CssClass="admin-button-blue" Text="Add" OnClick="bttn_Add_Question_Click" Visible="false" />
                                    <asp:LinkButton ID="lbExport" runat="server" CssClass="admin-button" Text="Export" OnClick="export" />
                                </div>
                                <div class="admin-white-box-inner">
                                    <div id="tbl_noresults" runat="server" style="padding: 20px 0;">No results were found with the current filter.<br /></div>
                                    <div id="tbl_Grids" runat="server">
                                        <table cellpadding="0" cellspacing="0" border="0" style="margin:0px; padding:0px; border-collapse:collapse;">
                                        <tr><td colspan="2">
                                            <asp:GridView ID="GV_Main" runat="server" CssClass="admin-grid" AllowPaging="True" 
                                                AllowSorting="True" AutoGenerateColumns="False" CellPadding="0" 
                                                DataKeyNames="id"
                                                OnRowDataBound="GV_Main_RowDataBound"
                                                OnRowDeleting="GV_Main_RowDeleting" OnRowEditing="GV_Main_RowEditing" 
                                                onsorting="GV_Main_Sorting" GridLines="None" onrowcommand="GV_Main_RowCommand">
                                                <HeaderStyle CssClass="admin-grid-header" />
                                                <PagerSettings Visible="false" />
                                                <Columns>
				                                <asp:BoundField DataField="username" HeaderText="Username" SortExpression="username">
                                                    <ItemStyle CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" Width="250"/>
					                                <HeaderStyle HorizontalAlign="Left"/>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="name" HeaderText="Full Name" SortExpression="name">
                                                    <ItemStyle CssClass="itemrow" HorizontalAlign="Left" VerticalAlign="Middle" Width="250"/>
					                                <HeaderStyle HorizontalAlign="Left"/>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="LB_Password" runat="server" CommandName="password" CausesValidation="False" Visible="false" CommandArgument='<%# Eval("id") %>' ImageUrl="/images/lemonaid/buttons/keys.png"  AlternateText="Send Password" ToolTip="Send Password" />
                                                        <%--<asp:ImageButton ID="ImageButtonP" runat="server" CausesValidation="False" CommandName="People" CommandArgument='< %# Eval("id") %>' ImageUrl="/images/lemonaid/buttons/editfields.png" AlternateText="Edit Item Details" />--%>
                                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="/images/lemonaid/buttonsNew/pencil.png" AlternateText="Edit Item" ToolTip="Edit"  />
                                                        <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="/images/lemonaid/buttonsNew/ex.png"  AlternateText="Delete" ToolTip="Delete" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                    <%--<asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-BackColor="#409ae2">
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="True" 
                                                                CommandName="Update" AlternateText="Update" />
                                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" 
                                                                CommandName="Cancel" AlternateText="Cancel" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Edit" ImageUrl="/images/lemonaid/buttons/pencil.png" AlternateText="Edit Item" title="Edit Item"  />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="40"/>
                                                        <ItemStyle HorizontalAlign="Center" Width="40"/>
                                                    </asp:TemplateField>--%>
                                                </Columns>
                                            </asp:GridView>
                                        </td></tr>
                                        <tr><td style="padding-top: 10px;"><asp:DropDownList ID="ddlPageSize" runat="server" CssClass="dropdownlist" OnSelectedIndexChanged="PageSizeChange" AutoPostBack="true"><%--<asp:ListItem Text="2 per page" Value="2" />--%><asp:ListItem Text="10 per page" Value="10" Selected="True" /><asp:ListItem Text="30 per page" Value="30" /><asp:ListItem Text="100 per page" Value="100" /></asp:DropDownList><span class="admin-pager-showing"><asp:Literal ID="litPagerShowing" runat="server" /></span></td><td style="text-align: right;"><cc:PagerV2_8 ID="pager1" runat="server" OnCommand="pager_Command" GenerateGoToSection="false" PageSize="10" Font-Names="Arial" PreviousClause="&#171;" NextClause="&#187;" GeneratePagerInfoSection="false" /></td></tr>
		                                </table>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td valign="top" style="padding-top:42px;padding-left:10px;">
                            <div id = "tbl_showfield" runat="server">
							    <table cellpadding="0" cellspacing="0" border="0" style="  margin:0px; padding:0px; border-collapse:collapse;"><tr><td colspan="3" style="vertical-align:top; line-height:22px;"><div style="float:left;position:relative;top:22px;"><img src="/images/lemonaid/partials/corners_tl.png" /></div><div style="float:right;position:relative;top:22px;left:2px;"><img src="/images/lemonaid/partials/corners_tr.png" /></div></td></tr>
			                    <tr><td colspan="3" style="height:8px; line-height:8px; background-color:#409ae2;">&nbsp;</td></tr>
			                    <tr>
                                    <td style="width:12px; background-color:#409ae2;">&nbsp;</td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0" style="background-color:#409ae2; padding:0px;">
                                        <tr>
                                            <td width="30%" class="admin_bodytext_blue" >
                                                Name</td>
                                            <td width="70%"><table border="0" cellpadding="0" cellspacing="0"><tr><td><img src="/images/lemonaid/partials/gridrow_left2.png" /></td><td style="background-image:url('/images/lemonaid/partials/gridrow_bg2.png');background-repeat:repeat-x;" valign="middle"><asp:Label ID="lbl_Name" runat="server"></asp:Label></td><td><img src="/images/lemonaid/partials/gridrow_right2.png" /></td></tr></table></td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td width="30%" style="background-color:#ffffff;">
                                                address1</td>
                                            <td width="70%" style="background-color:#ffffff;">
                                                <asp:Label ID="lbl_Address1" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td width="30%" style="background-color:#d3e2f3;">
                                                address2</td>
                                            <td width="70%" style="background-color:#d3e2f3;">
                                                <asp:Label ID="lbl_Address2" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td width="30%" style="background-color:#ffffff;">
                                                city</td>
                                            <td width="70%" style="background-color:#ffffff;">
                                                <asp:Label ID="lbl_City" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td width="30%" style="background-color:#d3e2f3;">
                                                country</td>
                                            <td width="70%" style="background-color:#d3e2f3;">
                                                <asp:Label ID="lbl_Country" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td width="30%" style="background-color:#ffffff;">
                                                postalcode</td>
                                            <td width="70%" style="background-color:#ffffff;">
                                                <asp:Label ID="lbl_Postel" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td width="30%" style="background-color:#d3e2f3;">
                                                homephone</td>
                                            <td width="70%" style="background-color:#d3e2f3;">
                                                <asp:Label ID="lbl_HomePhone" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%" class="admin_bodytext_blue" style="background-color:#ffffff;">
                                                E-mail</td>
                                            <td width="70%" style="background-color:#ffffff;">
                                                <asp:Label ID="lbl_Email" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%" class="admin_bodytext_blue" style="background-color:#d3e2f3;">
                                                Status :</td>
                                            <td width="70%" style="background-color:#d3e2f3;">
                                                <asp:Label ID="lbl_Status" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr><td colspan="2"></td></tr>
                                        </table>
                                    </td>
                                    <td style="width:12px; background-color:#409ae2;background-image:url('/images/lemonaid/partials/shadowr.png'); background-repeat:repeat-y; background-position:right;">&nbsp;</td>
			                    </tr>
                                <tr><td colspan="3" style="height:8px; line-height:8px; background-color:#409ae2;">&nbsp;</td></tr>
                                <tr><td colspan="3" style="background-image:url('/images/lemonaid/partials/shadow.png');background-repeat:repeat-x;vertical-align:top; line-height:22px; margin:0; "><div style="float:left;position:relative;top:-19px;margin:0;"><img src="/images/lemonaid/partials/corners_bl.png" /></div><div style="float:right;position:relative;top:-19px;left:2px;"><img src="/images/lemonaid/partials/corners_br.png" /></div></td></tr>
		                        </table>
                            </div>
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <div id="tbl_add_edit" runat="server">
                <asp:LinkButton ID="btn_Cancel_step5" runat="server" CssClass="admin-button-gray mw150" Text="Back" onclick="btn_Cancel_step1_Click" CausesValidation="False" />
                <asp:LinkButton ID="btn_Submit" runat="server" CssClass="admin-button-green mw150" Text="Save" onclick="btn_Submit_Click" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" />
                <br /><br />
                <table cellpadding="0" cellspacing="0" border="0" style="width:100%;">
				<tr>
                    <td style="vertical-align:top;">
                        <div class="admin-white-box">
                            <div class="admin-white-box-header">User Details</div>
                            <div class="admin-white-box-inner">
                                <table border="0" cellpadding="0" cellspacing="0">
						        <tr><td class="admin-prompt-right"><span class="required">*</span>Full Name</td><td colspan="2"><asp:TextBox ID="txt_Name" runat="server" ReadOnly="true" CssClass="textbox" Width="450"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_Name" ErrorMessage='Full Name required' Display="Dynamic" SetFocusOnError="True"> *</asp:RequiredFieldValidator></td></tr>
						        <tr>
                                    <td class="admin-prompt-right"><span class="required">*</span>User Name</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txt_UserName" runat="server" CssClass="textbox" Width="450" ReadOnly="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_UserName" ErrorMessage="User Name required" Display="Dynamic" SetFocusOnError="True"> *</asp:RequiredFieldValidator>
                                        <%--<USV:AjaxSyncValidator ID="asvUserName" runat="server" ControlToValidate="txt_UserName" SyncWebMethod="/ValidationWS.asmx/UserNameExists" ErrorMessage="User Name already exists" Display="Dynamic" SetFocusOnError="True" onservervalidate="asvUserName_ServerValidate"> *</USV:AjaxSyncValidator>--%>
                                    </td>
						        </tr>
<%if (Session["LoggedInID"].ToString() == "1")
    { %>
						        <tr><td class="admin-prompt-right"><span class="required">*</span>E-mail</td><td colspan="2"><asp:TextBox ID="txt_Email" runat="server" CssClass="textbox" Width="450" ReadOnly="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txt_Email" ErrorMessage="E-mail required" Display="Dynamic" SetFocusOnError="True"> *</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_Email" Display="Dynamic" ErrorMessage="E-mail invalid" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"> *</asp:RegularExpressionValidator></td></tr>
<%} %>						        
                                    <tr id="frow1" runat="server"><td class="admin-prompt-right"><span class="required">*</span>Group</td><td colspan="2" style="padding-top: 15px;"><asp:RadioButtonList ID="RB_Language" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal"><asp:ListItem Value="1" Selected="True">English</asp:ListItem><asp:ListItem Value="2">French</asp:ListItem></asp:RadioButtonList><asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="RB_Language" ErrorMessage="Group required" Display="Dynamic" SetFocusOnError="True"> *</asp:RequiredFieldValidator></td></tr>
						        <tr><td class="admin-prompt-right"><span class="required">*</span>Status</td><td colspan="2" style="padding-top: 15px;"><asp:RadioButtonList ID="RB_Status" runat="server" CssClass="rb-enhanced" RepeatDirection="Horizontal"><asp:ListItem Value="active" Selected="True">Active</asp:ListItem><asp:ListItem Value="inactive">Inactive</asp:ListItem></asp:RadioButtonList><asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="RB_Status" ErrorMessage="Status required" Display="Dynamic" SetFocusOnError="True"> *</asp:RequiredFieldValidator></td></tr>
						       
                                <tr><td class="admin-prompt-right">&nbsp;</td><td colspan="2"><asp:CheckBox ID="cbAD" runat="server" Text="Active Directory"  CssClass="cb-enhanced" /></td></tr>
                                <tr id="adminrow" runat="server" visible="false"><td class="admin-prompt-right">&nbsp;</td><td colspan="2"><asp:CheckBox ID="cbAdmin" runat="server" CssClass="cb-enhanced" Text="Make Administrator" /></td></tr>
                                <tr><td class="admin-prompt-right">&nbsp;</td><td colspan="2"><asp:CheckBox ID="cbReviewer" runat="server" CssClass="cb-enhanced" Text="Make Reviewer" /></td></tr>
                                <tr><td class="admin-prompt-right">&nbsp;</td><td colspan="2"><asp:CheckBox ID="cbManageArea" runat="server" CssClass="cb-enhanced" Text="Manage Area" /></td></tr>
                                <tr id="grprow_add" runat="server">
							        <td class="admin-prompt-right">Group</td>
							        <td><table border="0" cellpadding="2" cellspacing="2">
                                        <tr>
										    <td><asp:DropDownList ID="LB_Group2" runat="server"  CssClass="dropdownlist"></asp:DropDownList></td>
										    <td>
                                                <asp:DropDownList ID="RB_Permission2" runat="server" CssClass="dropdownlist">
													<asp:ListItem Value="0">Deny</asp:ListItem>
													<asp:ListItem Value="1">Viewer</asp:ListItem>
													<asp:ListItem Value="2" Selected="True">Editor</asp:ListItem>
													<asp:ListItem Value="3">Publisher</asp:ListItem>
													<asp:ListItem Value="4">Administrator</asp:ListItem>
												</asp:DropDownList>
									        </td>
									    </tr>
								    </table></td>                                
							    </tr>
                                    
                                    
                               <tr id="grprow" runat="server">
						            <td class="admin-prompt-right">Group</td>
						            <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
								        <tr>
									        <td><asp:DropDownList ID="LB_Group" runat="server" CssClass="dropdownlist" onselectedindexchanged="LB_Group_SelectedIndexChanged" Width="150px" AutoPostBack="True"></asp:DropDownList></td>
									        <td style="padding-left: 20px;">
                                                <table id="tbl_permission" runat="server" visible="False" border="0" cellpadding="0" cellspacing="0">
											    <tr>
												    <td>
													    <asp:DropDownList ID="RB_Permission" runat="server" CssClass="dropdownlist" >
                                                        <%--AutoPostBack="True" onselectedindexchanged="RB_Permission_SelectedIndexChanged">--%>
														    <asp:ListItem Value="-1">Select Access Level</asp:ListItem>
														    <asp:ListItem Value="0">Deny</asp:ListItem>
														    <asp:ListItem Value="1">Viewer</asp:ListItem>
														    <asp:ListItem Value="2">Editor</asp:ListItem>
														    <%--<asp:ListItem Value="3">Screener</asp:ListItem>--%>
														    <asp:ListItem Value="3">Publisher</asp:ListItem>
														    <asp:ListItem Value="4">Administrator</asp:ListItem>
													    </asp:DropDownList>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvAccess" ControlToValidate="RB_Permission" ErrorMessage="Access Level required" SetFocusOnError="True" Display="Dynamic" InitialValue="-1" ValidationGroup="AccessGroup"> *</asp:RequiredFieldValidator>
												    </td>
											    </tr>
										        </table>
									        </td>
								        </tr>
							            </table>
						            </td>
                                    <td><asp:ImageButton ID="LB_AddGroup" runat="server" ValidationGroup="AccessGroup" ImageUrl="/images/lemonaid/buttons/Plus.png"  AlternateText="Add" OnClick="mAddAccess" Visible="false" /></td>
						        </tr>
						        <tr><td colspan="3"><asp:Literal ID="litSetPermissions" runat="server" Visible="false"/></td></tr>
						        </table>
			
						        <table style="display:none;">
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Username</td>
							        <td><!-- Username --></td>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Name</td>
							        <td><!-- Full name --></td>
						        </tr>
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Date of Birth</td>
							        <td>
								        <asp:TextBox ID="txt_DOB" runat="server" Text="12/12/2012"></asp:TextBox>
								        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
									        ControlToValidate="txt_DOB" ErrorMessage="Required" 
									        SetFocusOnError="True"></asp:RequiredFieldValidator>
							            &nbsp;<asp:CompareValidator ID="CompareValidator2" runat="server" 
									        ControlToValidate="txt_DOB" CssClass="errorlabel" Display="Dynamic" 
									        ErrorMessage="Invalid Date" Operator="DataTypeCheck" Type="Date" 
									        ValidationGroup="step2"></asp:CompareValidator>
							        </td>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Sex</td>
							        <td>
								        <asp:RadioButtonList ID="RB_Sex" runat="server" RepeatDirection="Horizontal">
									        <asp:ListItem Value="male" Selected="True">Male</asp:ListItem>
									        <asp:ListItem Value="female">Female</asp:ListItem>
								        </asp:RadioButtonList>
								        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
									        ControlToValidate="RB_Sex" ErrorMessage="Required" 
									        SetFocusOnError="True"></asp:RequiredFieldValidator>
							        </td>
						        </tr>
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Address 1</td>
							        <td><asp:TextBox ID="txt_Address_1" runat="server"></asp:TextBox></td>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Address 2</td>
							        <td><asp:TextBox ID="txt_Address_2" runat="server"></asp:TextBox></td>
						        </tr>
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">City</td>
							        <td><asp:TextBox ID="txt_City" runat="server"></asp:TextBox></td>
						        </tr>
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Region</td>
							        <td><asp:TextBox ID="txt_Region" runat="server"></asp:TextBox></td>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Country</td>
							        <td><asp:TextBox ID="txt_Country" runat="server"></asp:TextBox></td>
						        </tr>
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Postal Code</td>
							        <td><asp:TextBox ID="txt_Postel" runat="server" MaxLength="10"></asp:TextBox></td>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Home Phone</td>
							        <td><asp:TextBox ID="txt_Home_Phone" runat="server"></asp:TextBox></td>
						        </tr>
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Work Phone</td>
							        <td><asp:TextBox ID="txt_Work_Phone" runat="server"></asp:TextBox></td>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Work Ext.</td>
							        <td><asp:TextBox ID="txt_Work_Ext" runat="server"></asp:TextBox></td>
						        </tr>
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Cell Phone</td>
							        <td><asp:TextBox ID="txt_Cell_Phone" runat="server"></asp:TextBox></td>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Fax Phone</td>
							        <td><asp:TextBox ID="txt_Fax_Phone" runat="server"></asp:TextBox></td>
						        </tr>
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Fax Ext.</td>
							        <td><asp:TextBox ID="txt_Fax_Ext" runat="server"></asp:TextBox></td>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Email</td>
							        <td><!-- email --></td>
						        </tr>
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">WebSite</td>
							        <td><asp:TextBox ID="txt_Website" runat="server"></asp:TextBox></td>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Time Zone</td>
							        <td>
								        <asp:TextBox ID="txt_TimeZone" runat="server"></asp:TextBox>
								        <asp:RangeValidator ID="Rangevalidator2" runat="server" 
									        ControlToValidate="txt_TimeZone" CssClass="errorlabel" Display="Dynamic" 
									        ErrorMessage="Numeric Only" MaximumValue="99999999" MinimumValue="-99999999" 
									        Type="Integer"></asp:RangeValidator>
							        </td>
						        </tr>
						        <tr>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Language</td>
							        <td><!--Language--></td>
							        <td width="15%">&nbsp;</td>
							        <td width="25%">Status</td>
							        <td><!-- Status --></td>
						        </tr>
						        </table>
                            </div>
                        </div>
                        <br />
                        <div id="dgPermissionstbl" runat="server" visible="false" class="admin-white-box" style="min-width: 583px;">
                            <div class="admin-white-box-header">Access Level</div>
                            <div class="admin-white-box-inner">
                                <asp:GridView ID="GridView1" runat="server" Visible="false">
                                </asp:GridView>
                
                                <asp:GridView ID="dgPermissions" runat="server" CssClass="admin-grid" AutoGenerateColumns="false" GridLines="None" CellPadding="0" CellSpacing="0" ShowHeader="true"
                                    onrowcommand="dgPermissions_RowCommand" OnRowDeleting="dgPermissions_RowDeleting" OnRowDataBound="dgPermissions_RowDataBound" 
                                    AllowPaging="false">
                                <HeaderStyle CssClass="admin-grid-header" />
                                <PagerSettings Visible="false" />
                                <Columns>
                                <asp:BoundField DataField="group" HeaderText="Group" ItemStyle-CssClass="itemrow" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="250"></asp:BoundField>
                                <asp:BoundField DataField="access" HeaderText="Access Level" ItemStyle-CssClass="itemrow" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150"></asp:BoundField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblId" Text='<%#Eval("id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>           
                                <asp:TemplateField HeaderText="" ShowHeader="False" ItemStyle-CssClass="itemrow" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="LB_Delete" runat="server" CausesValidation="False" CommandArgument='<%#Eval("id") %>' CommandName="delPermission" ImageUrl="/images/lemonaid/buttonsNew/delete-red.png" AlternateText="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                    <td style="padding-left:50px;  vertical-align:top;">
                        <asp:Panel ID="pnlAdmin" runat="server" Visible="false">
                            <div class="admin-white-box">
                                <div class="admin-white-box-header">User's Admin Menu</div>
                                <div class="admin-white-box-inner">
                                    <table cellpadding="0" cellspacing="0" border="0">
	                                <tr>
                                        <td>
                                            <asp:DataGrid ID="dgMenu" runat="server" AutoGenerateColumns="false" GridLines="None" ShowHeader="false" CellPadding="0" CellSpacing="0" Visible="false">
                                            <Columns>
		                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2"><ItemTemplate><img src="/images/lemonaid/partials/gridrow_left.png" /></ItemTemplate></asp:TemplateColumn>
                                            <asp:TemplateColumn ><ItemStyle CssClass="itemrow" />
			                                    <ItemTemplate><asp:CheckBox ID="cbVisible" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"visible")%>'/></ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="name">
			                                    <ItemStyle CssClass="itemrow" />
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn Visible="false">
                                            <ItemTemplate><asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"id")%>'/></ItemTemplate>
                                            </asp:TemplateColumn>
		                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="#409ae2">
                                                <ItemTemplate><img src="/images/lemonaid/partials/gridrow_right.png" /></ItemTemplate>
                                            </asp:TemplateColumn>
                                            </Columns>
                                            </asp:DataGrid>
        
                                            <asp:DataList ID="repMenu" RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Table" runat="server" CellPadding="5">
			                                    <ItemTemplate>
				                                    <asp:CheckBox ID="cbOn" runat="server" Checked='<%# Eval("visible") %>' Text='<%# Eval("name") %>' ToolTip='<%# Eval("id") %>' CssClass="cb-enhanced" />
			                                    </ItemTemplate>
		                                    </asp:DataList>
                                        </td>
                                    </tr>
                                    </table>
                                    <br /><%--<i386:ImageOverButton ID="ibSave" runat="server" ImageUrl="/images/lemonaid/buttons/save.png" ImageOverUrl="/images/lemonaid/buttons/save_over.png" OnClick="SAVE" Visible="false" />--%>
                                    <asp:Label ID="lblId" runat="server" Visible="false" />
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                </table>
            </div>
            <%--</div>--%>
        </td>
        
    </tr>
</table>
</div>
</ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
      <div class="updateprogress">Update in progress...</div>
    </ProgressTemplate>
    </asp:UpdateProgress>

    <%--<%if (ViewState["action"].ToString() == "add"){ %>
    <script>
        //Initial bind
        $(document).ready(function () {

            var txtUser = $('#<%=txt_UserName.ClientID %>');
            var txtEmail = $('#<%=txt_Email.ClientID %>');

            txtUser.blur(function () {
                if (txtUser.val() != "") {
                    $.ajax({
                        type: "POST",
                        url: "Admin.aspx/ValidateUsername",
                        data: JSON.stringify({ 'username': txtUser.val() }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d != "") {
                                alert(response.d);
                                txtUser.val('');
                            }
                        },
                        error: function (xhr, status, errorThrown) {
                            alert(status + " | " + xhr.responseText);
                        }
                    });
                }
            });


//            txtEmail.blur(function () {
//                if (txtEmail.val() != "") {
//                    $.ajax({
//                        type: "POST",
//                        url: "Admin.aspx/ValidateEmail",
//                        data: JSON.stringify({ 'email': txtEmail.val() }),
//                        contentType: "application/json; charset=utf-8",
//                        dataType: "json",
//                        success: function (response) {
//                            if (response.d != "") {
//                                alert(response.d);
//                                txtEmail.val('');
//                            }
//                        },
//                        error: function (xhr, status, errorThrown) {
//                            alert(status + " | " + xhr.responseText);
//                        }
//                    });
//                }
//            });

        });

    </script>
    <%} %>--%>