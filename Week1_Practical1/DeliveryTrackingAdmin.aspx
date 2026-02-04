<%@ Page Title="Delivery Tracking" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="DeliveryTrackingAdmin.aspx.cs" Inherits="Week1_Practical1.DeliveryTrackingAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>Delivery Tracking Management</h3>
    <p>Manage your Deliveries and tracking here!</p>
    <br />
    <br />
    <div class="panel-style">
        <asp:Panel ID="pnlSearch" runat="server" CssClass="search-panel">

            Shipment ID:
            <asp:TextBox ID="txtShipmentID" runat="server" Width="120px" />

            Status:
            <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem Text="-- All --" Value="" />
                <asp:ListItem Text="Packed" Value="Packed" />
                <asp:ListItem Text="Shipped" Value="Shipped" />
                <asp:ListItem Text="Out for Delivery" Value="Out for Delivery" />
                <asp:ListItem Text="Delivered" Value="Delivered" />
            </asp:DropDownList>

            <asp:Button ID="btnSearch" runat="server"
                Text=" 🔍 Search"
                OnClick="btnSearch_Click" />

            <asp:Button ID="btnReset" runat="server"
                Text="Reset"
                OnClick="btnReset_Click" />

        </asp:Panel>

        <br />

        <asp:GridView ID="gvDelivery"
                runat="server"
                CssClass="gridview-style"
                HeaderStyle-CssClass="gv-header"
                AutoGenerateColumns="False"
                DataKeyNames="TrackingID"
                OnRowEditing="gvDelivery_RowEditing"
                OnRowCancelingEdit="gvDelivery_RowCancelingEdit"
                OnRowUpdating="gvDelivery_RowUpdating" OnRowCommand="gvDelivery_RowCommand">
                <Columns>

                    <asp:BoundField DataField="TrackingID" HeaderText="Tracking ID" ReadOnly="true" />
                    <asp:BoundField DataField="ShipmentID" HeaderText="Shipment ID" ReadOnly="true" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class="status-badge <%# Eval("StatusUpdate").ToString().Replace(" ", "").ToLower() %>">
                                <%# Eval("StatusUpdate") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <%# Eval("Location") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLocation" runat="server"
                                Text='<%# Bind("Location") %>' CssClass="textbox" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="UpdateTime" HeaderText="Last Updated" ReadOnly="true"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:TemplateField HeaderText="Update Status">
                    <ItemTemplate>
                        <asp:Button runat="server" Text="Packed"
                            CommandName="UpdateStatus"
                            CssClass="btn-add"
                            CommandArgument='<%# Eval("TrackingID") + "|Packed" %>' />

                        <asp:Button runat="server" Text="Shipped"
                            CommandName="UpdateStatus"
                            CssClass="btn-add"
                            CommandArgument='<%# Eval("TrackingID") + "|Shipped" %>' />

                        <asp:Button runat="server" Text="Delivered"
                            CommandName="UpdateStatus"
                            CssClass="btn-add"
                            CommandArgument='<%# Eval("TrackingID") + "|Delivered" %>' />
                    </ItemTemplate>
                </asp:TemplateField>


                </Columns>
            </asp:GridView>
        </div>  

</asp:Content>
