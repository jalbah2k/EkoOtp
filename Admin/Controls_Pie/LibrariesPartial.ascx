<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LibrariesPartial.ascx.cs" Inherits="Admin_Controls_Pie_LibrariesPartial" %>

 <div class="admin-white-box" >
    <div class="admin-white-box-header">Libraries - Categories</div>
    <div class="admin-white-box-header" style="padding:20px;">
        <div class="row">                        
            <asp:Repeater runat="server" ID="repLibraries" OnItemDataBound="repLibraries_ItemDataBound">
                <ItemTemplate>
                    <div class="row div-Rep gvLib">
                        <asp:PlaceHolder runat="server" ID="plLibrary"></asp:PlaceHolder>
                        <asp:Repeater runat="server" ID="repCategories" OnItemDataBound="repCategories_ItemDataBound" >
                            <ItemTemplate>
                                <div class="row div-Rep gvCateg">
                                    <asp:PlaceHolder runat="server" ID="plCategory"></asp:PlaceHolder>
                                    <asp:Repeater runat="server" ID="repSubcategories" OnItemDataBound="repSubcategories_ItemDataBound" >
                                        <ItemTemplate>
                                            <div class="row div-Rep gvSubCateg" >
                                                <asp:PlaceHolder runat="server" ID="plSubCategory"></asp:PlaceHolder>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
