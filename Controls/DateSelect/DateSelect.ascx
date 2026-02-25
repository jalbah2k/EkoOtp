<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DateSelect.ascx.cs" Inherits="Controls_DateSelect" %>
<script type="text/javascript">   
function PopulateDays() {
    var ddlMonth = document.getElementById("<%=ddlMonth.ClientID%>");
    var ddlYear = document.getElementById("<%=ddlYear.ClientID%>");
    var ddlDay = document.getElementById("<%=ddlDay.ClientID%>");
    var y = ddlYear.options[ddlYear.selectedIndex].value;
    var m = ddlMonth.options[ddlMonth.selectedIndex].value != 0;
    if (ddlMonth.options[ddlMonth.selectedIndex].value != 0 && ddlYear.options[ddlYear.selectedIndex].value != 0) {
        var dayCount = 32 - new Date(ddlYear.options[ddlYear.selectedIndex].value, ddlMonth.options[ddlMonth.selectedIndex].value - 1, 32).getDate();
        ddlDay.options.length = 0;
        AddOption(ddlDay, "DD", "0");
        for (var i = 1; i <= dayCount; i++) {
            AddOption(ddlDay, i, i);
        }
    }
}
 
function AddOption(ddl, text, value) {
    var opt = document.createElement("OPTION");
    opt.text = text;
    opt.value = value;
    ddl.options.add(opt);
}
 
function Validate(sender, args) {
    var ddlMonth = document.getElementById("<%=ddlMonth.ClientID%>");
    var ddlYear = document.getElementById("<%=ddlYear.ClientID%>");
    var ddlDay = document.getElementById("<%=ddlDay.ClientID%>");
    args.IsValid = (ddlDay.selectedIndex != 0 && ddlMonth.selectedIndex != 0 && ddlYear.selectedIndex != 0)
}
</script>

<asp:DropDownList ID="ddlYear" runat="server" onchange = "PopulateDays()" />&nbsp
                                <asp:DropDownList ID="ddlMonth" runat="server" onchange = "PopulateDays()" />&nbsp
                                <asp:DropDownList ID="ddlDay" runat="server" />
                                <asp:CustomValidator ID="Validator1" runat="server" ClientValidationFunction = "Validate" />