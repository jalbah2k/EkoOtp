<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WeeklyRoundup.ascx.cs" Inherits="WeeklyRoundup" %>
<%@ Register TagPrefix="CE" Namespace="CuteEditor" Assembly="CuteEditor" %>
<style>
    .btn-enabled{
        opacity:1;
        cursor:pointer;
    }
     .btn-disabled{
        opacity:0.3;
        cursor:none;
    }
</style>
<div class="admin-header-wrapper noprint">
    <div class="admin-header">Weekly Roundup Intro</div>
    <div class="admin-header-subtitle">Here you can create and modify Weekly Roundup Intros.</div>
</div>
<div class="admin-control-wrapper">
    <div><asp:Label ID="lbl_msg" runat="server" CssClass="alert"></asp:Label></div>
    
    <table>
        <tr>
            <td>
                <div class="admin-white-box">
                    <div class="admin-white-box-header">Details</div>
                    <div class="admin-white-box-inner">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr><td class="admin-prompt-right" style="vertical-align:middle;">Select Week</td>
                                <td style="padding-top:18px;">
                                    <asp:DropDownList runat="server" ID="ddlWeeks" DataValueField="id" DataTextField="name" 
                                        CssClass="dropdownlist" AutoPostBack="true" OnSelectedIndexChanged="ddlWeeks_SelectedIndexChanged"></asp:DropDownList>
                                 </td>
                            </tr>
                            <tr><td colspan="2" style="height:20px;"></td></tr>
                            <tr><td class="admin-prompt-right" style="vertical-align:top;">Intro Text</td>
                                <td style="padding-top:5px;">
                                    <CE:Editor id="Editor1" runat="server" width="600" Text="<span style='font-family:Arial;font-size:11px'> </span>"></CE:Editor>
                                 </td>
                            </tr>
                            <tr><td colspan="2" style="padding-top:20px;">
                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="admin-button-green" Text="Save" OnClick="btnSave_Click" />
                            </td></tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>

    </table>
</div>
