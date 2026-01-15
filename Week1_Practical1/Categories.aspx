<%@ Page Title="Category" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="Week1_Practical1.Categories" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="w-100">
        <tr>
            <td><h2>Category</h2></td>
            <td>
                <asp:Button ID="btnShowAdd" runat="server" CssClass="btn-add" Text="➕ Add Category" OnClick="btnShowAdd_Click" />
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlAddCategory" runat="server" BorderStyle="Solid" Visible="False" Height="302px" CssClass="panel-style">
        <h4>Add New Category</h4>
        
        <table class="w-100">
            <tr>
                <td>Category ID:</td>
                <td>
                    <asp:TextBox ID="txtCatID" CssClass="textbox" runat="server" Width="258px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Category Name:</td>
                <td>
                    <asp:TextBox ID="txtCatName" CssClass="textbox" runat="server" Width="257px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Description:</td>
                <td>
                    <asp:TextBox ID="txtCatDesc"  CssClass="textbox" runat="server" Height="96px" TextMode="MultiLine" Width="354px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btnAddCategory" runat="server" Text="Add" CssClass="btn-add" OnClick="btnAddCategory_Click" />
                    <asp:Button ID="btnCancelAdd" runat="server" Text="Cancel" CssClass="btn-cancel" OnClick="btnCancelAdd_Click" />
                </td>
            </tr>
        </table>
        
    </asp:Panel>
    <br />
    <asp:TextBox ID="txtSearch" CssClass="textbox" runat="server" Placeholder="Search category name..." />
    <asp:Button ID="btnSearch" CssClass="btn-add" runat="server" Text=" 🔍 Search" OnClick="btnSearch_Click"  />
    <asp:Button ID="btnClear" runat="server" CssClass="btn-cancel" Text="Clear" OnClick="btnClear_Click" />
    <br />
    <br />
    <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" CssClass="gridview-style" HeaderStyle-CssClass="gv-header" DataKeyNames="CategoryID" Height="221px" OnRowCancelingEdit="gvCategories_RowCancelingEdit" OnRowDeleting="gvCategories_RowDeleting" OnRowEditing="gvCategories_RowEditing" OnRowUpdating="gvCategories_RowUpdating" OnSelectedIndexChanged="gvCategories_SelectedIndexChanged" Width="965px" CellPadding="10">
        <Columns>
            <asp:BoundField DataField="CategoryID" HeaderText="No." />
            <asp:BoundField DataField="CategoryName" HeaderText="Category Name" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:CommandField AccessibleHeaderText="Action" ItemStyle-CssClass="gv-btn" HeaderText="Action" ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" >
<ItemStyle CssClass="gv-btn"></ItemStyle>
            </asp:CommandField>
        </Columns>

<HeaderStyle CssClass="gv-header"></HeaderStyle>
    </asp:GridView>
    
</asp:Content>
