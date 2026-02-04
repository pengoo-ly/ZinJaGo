<%@ Page Title="Delivery Tracking" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="DeliveryTrackingAdmin.aspx.cs" Inherits="Week1_Practical1.DeliveryTrackingAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>Delivery Tracking Management</h3>
    <p>Manage your Deliveries and tracking here!</p>
    <br />
    <br />
    <div class="panel-style">
        <asp:GridView ID="gvDelivery"
                runat="server"
                CssClass="gridview-style"
                HeaderStyle-CssClass="gv-header"
                AutoGenerateColumns="False"
                DataKeyNames="TrackingID"
                OnRowEditing="gvDelivery_RowEditing"
                OnRowCancelingEdit="gvDelivery_RowCancelingEdit"
                OnRowUpdating="gvDelivery_RowUpdating">
                <Columns>

                    <asp:BoundField DataField="TrackingID" HeaderText="Tracking ID" ReadOnly="true" />
                    <asp:BoundField DataField="ShipmentID" HeaderText="Shipment ID" ReadOnly="true" />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# Eval("StatusUpdate") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtStatus" runat="server"
                                Text='<%# Bind("StatusUpdate") %>' CssClass="textbox" />
                        </EditItemTemplate>
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

                    <asp:CommandField ShowEditButton="true"
                        EditText="Edit"
                        UpdateText="Save"
                        CancelText="Cancel"
                        ControlStyle-CssClass="gv-btn" />

                </Columns>
            </asp:GridView>
        </div>  

</asp:Content>
