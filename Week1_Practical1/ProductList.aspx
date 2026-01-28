<%@ Page Title="Product List" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="Week1_Practical1.ProductList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="w-100">
    <tr>
        <td><h2>Products List</h2></td>
        <td>
            <asp:Button ID="btnShowAdd" runat="server" CssClass="btn-add" Text="➕ Add Product" OnClick="btnShowAdd_Click" />
        </td>
    </tr>
</table>
    <br />
    <asp:TextBox ID="txtSearch" CssClass="textbox" runat="server" Placeholder="Search product name..." />
    <asp:Button ID="btnSearch" CssClass="btn-add" runat="server" Text=" 🔍 Search" OnClick="btnSearch_Click"  />
    <asp:Button ID="btnClear" runat="server" CssClass="btn-cancel" Text="Clear" OnClick="btnClear_Click" />
    <br />
    <br />
    <asp:GridView ID="gvProducts" CssClass="gridview-style" HeaderStyle-CssClass="gv-header" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvProducts_SelectedIndexChanged" DataKeyNames="ProductID" OnRowCancelingEdit="gvProducts_RowCancelingEdit" OnRowDeleting="gvProducts_RowDeleting" OnRowEditing="gvProducts_RowEditing" OnRowUpdating="gvProducts_RowUpdating">

        <Columns>
            <asp:BoundField DataField="ProductID" HeaderText="No." />
            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
            <asp:BoundField DataField="CategoryID" HeaderText="Category No." />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Unit_Price" HeaderText="Unit Price" DataFormatString="{0:c}" />
            <asp:BoundField DataField="StockQuantity" HeaderText="Stock Level" />
            <asp:TemplateField HeaderText="Image">
                <ItemTemplate>
                    <asp:Image ID="img_Product" runat="server" Height="100px" Width="100px"
                        ImageUrl='<%# ResolveUrl("~/Images/") + Eval("Image") %>' CssClass="preview-img" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:FileUpload ID="fuEditImage" runat="server" CssClass="textbox" />
                    <br />
                    <asp:Image ID="imgEditPreview" runat="server" Height="100px" Width="100px"
                        ImageUrl='<%# ResolveUrl("~/Images/") + Eval("Image") %>' CssClass="preview-img" />
                    <asp:HiddenField ID="hfOldImage" runat="server" Value='<%# Eval("Image") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:CommandField AccessibleHeaderText="Action" ItemStyle-CssClass="gv-btn" HeaderText="Action" ShowSelectButton="True" ShowDeleteButton="True" ShowEditButton="True" CancelText="🚫" DeleteText="❌" EditText="✏️" SelectText="👆" UpdateText="✔️" >
<ItemStyle CssClass="gv-btn"></ItemStyle>
            </asp:CommandField>
        </Columns>
<HeaderStyle CssClass="gv-header"></HeaderStyle>

    </asp:GridView>
</asp:Content>
