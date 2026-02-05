<%@ Page Title="Courier Management" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="CourierManagement.aspx.cs" Inherits="Week1_Practical1.CourierManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>Courier Management</h3>
    <p>Organising tracking, and controlling parcel deliveries from pickup to final destination</p>
    <br />
        <asp:Panel ID="pnlAdd" runat="server" CssClass="panel-style">

                <asp:Label Text="Courier Name" runat="server" />
                <asp:TextBox ID="txtName" runat="server" CssClass="textbox" /><br /><br />

                <asp:Label Text="Contact Number" runat="server" />
                <asp:TextBox ID="txtContact" runat="server" CssClass="textbox" /><br /><br />

                <asp:Label Text="Email" runat="server" />
                <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" /><br /><br />

                <asp:Label Text="Tracking URL" runat="server" />
                <asp:TextBox ID="txtURL" runat="server" CssClass="textbox" /><br /><br />

                <asp:CheckBox ID="chkPartnered" runat="server" Text=" Partnered Courier" /><br /><br />

                <asp:Button ID="btnAdd" runat="server"
                    Text="Add Courier"
                    CssClass="btn-add"
                    OnClick="btnAdd_Click" />

            </asp:Panel>
        <br />

        <asp:GridView ID="gvCourier" runat="server"
                CssClass="gridview-style"
                AutoGenerateColumns="False"
                DataKeyNames="CourierID"
                OnRowEditing="gvCourier_RowEditing"
                OnRowCancelingEdit="gvCourier_RowCancelingEdit"
                OnRowUpdating="gvCourier_RowUpdating"
                OnRowDeleting="gvCourier_RowDeleting">

                <Columns>
                    <asp:BoundField DataField="CourierID" HeaderText="ID" ReadOnly="true" />
                    <asp:BoundField DataField="CourierName" HeaderText="Courier Name" />
                    <asp:BoundField DataField="ContactNumber" HeaderText="Contact" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="TrackingURL" HeaderText="Tracking URL" />
                    <asp:CheckBoxField DataField="IsPartnered" HeaderText="Partnered" />

                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                </Columns>
            <HeaderStyle CssClass="gv-header"></HeaderStyle>
            </asp:GridView>
</asp:Content>
