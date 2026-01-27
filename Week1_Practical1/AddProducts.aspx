<%@ Page Title="Add Products" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AddProducts.aspx.cs" Inherits="Week1_Practical1.AddProducts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 30px;
        }
        .preview-img {
            object-fit: cover;
            border-radius: 8px;
            margin-top: 8px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Add New Products</h2>
    <br />
    <asp:Panel ID="pnlImagePreview" runat="server" CssClass="panel-style" style="margin-top:20px;">
        <h4>Image Preview</h4>
        <asp:Image ID="imgPreview" CssClass="preview-img" runat="server" Height="253px" Width="239px" />
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlAddProduct" CssClass="panel-style" runat="server">
        <table class="w-100">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Product ID:" AssociatedControlID="txtProdID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtProdID" CssClass="textbox" runat="server" OnTextChanged="txtProdID_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" Text="Product Name:" AssociatedControlID="txtProdName"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtProdName" CssClass="textbox" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" Text="Category:" AssociatedControlID="ddlCategory"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategory" CssClass="textbox" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" Text="Description:" AssociatedControlID="txtDesc"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDesc" runat="server" CssClass="textbox" Height="108px" TextMode="MultiLine" Width="362px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" Text="Unit Price:" AssociatedControlID="txtPrice"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPrice" CssClass="textbox" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" Text="Stock Quantity:" AssociatedControlID="txtStock"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtStock" CssClass="textbox" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" Text="Upload Image:" AssociatedControlID="fuImage"></asp:Label>
                </td>
                <td>
                    <asp:FileUpload ID="fuImage" CssClass="textbox" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" Text="Status:" AssociatedControlID="ddlStatus"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" CssClass="textbox" runat="server">
                        <asp:ListItem Value="Available"></asp:ListItem>
                        <asp:ListItem Value="Unavailable"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btnAddProduct" runat="server" Text=" ➕ Add Product" CssClass="btn-add" OnClick="btnAddProduct_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel" OnClick="btnCancel_Click"  />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <script>
        const fuImage = document.getElementById('<%= fuImage.ClientID %>');
        const imgPreview = document.getElementById('<%= imgPreview.ClientID %>');

        fuImage.addEventListener('change', function() {
            if (fuImage.files && fuImage.files[0]) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    imgPreview.src = e.target.result;
                }
                reader.readAsDataURL(fuImage.files[0]);
            }
        });
    </script>

</asp:Content>
