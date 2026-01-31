<%@ Page Title="Order Shipment" Language="C#" AutoEventWireup="true" CodeBehind="OrderShipmentAdmin.aspx.cs" 
    Inherits="Week1_Practical1.OrderShipmentAdmin" MasterPageFile="~/Admin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Order Shipment Management</title>
    <style>
        .container {
            max-width: 1400px;
            margin: 0 auto;
            background-color: #f8f0e3;
            border-radius: 8px;
            padding: 30px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
        }

        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 30px;
            border-bottom: 2px solid #e8ddd2;
            padding-bottom: 20px;
        }

        .header h1 {
            font-size: 28px;
            color: #2C2C2C;
            font-weight: 600;
        }

        .stats-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
            margin-bottom: 40px;
        }

        .stat-card {
            background: #f8f0e3;
            color: #2C2C2C;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
            border: 1px solid #e8ddd2;
        }

        .stat-card.total {
            background: linear-gradient(135deg, #4FA392 0%, #5DB5A4 100%);
            color: white;
        }

        .stat-card.pending {
            background: #f8f0e3;
            border-color: #e8ddd2;
        }

        .stat-card.shipped {
            background: #f8f0e3;
            border-color: #e8ddd2;
        }

        .stat-card.delivered {
            background: #f8f0e3;
            border-color: #e8ddd2;
        }

        .stat-card.revenue {
            background: #f8f0e3;
            border-color: #e8ddd2;
        }

        .stat-label {
            font-size: 12px;
            opacity: 0.75;
            text-transform: uppercase;
            letter-spacing: 1px;
            margin-bottom: 8px;
            font-weight: 600;
        }

        .stat-value {
            font-size: 32px;
            font-weight: bold;
        }

        .error-message {
            display: none;
            background-color: #FDD8D8;
            border: 1px solid #F5C6CB;
            color: #721c24;
            padding: 12px;
            border-radius: 4px;
            margin-bottom: 20px;
        }

        .error-message.show {
            display: block;
        }

        .success-message {
            display: none;
            background-color: #D4EDDA;
            border: 1px solid #C3E6CB;
            color: #155724;
            padding: 12px;
            border-radius: 4px;
            margin-bottom: 20px;
        }

        .success-message.show {
            display: block;
        }

        .table-container {
            overflow-x: auto;
            margin-top: 20px;
            background: #f8f0e3;
            border-radius: 8px;
            border: 1px solid #e8ddd2;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            background-color: #f8f0e3;
        }

        th {
            background-color: #4FA392;
            padding: 15px;
            text-align: left;
            font-weight: 600;
            color: white;
            border-bottom: 2px solid #3D8577;
            font-size: 14px;
        }

        td {
            padding: 15px;
            border-bottom: 1px solid #e8ddd2;
            color: #2C2C2C;
        }

        tr:hover {
            background-color: #f0e8dc;
        }

        .status-badge {
            display: inline-block;
            padding: 6px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 600;
            text-transform: capitalize;
        }

        .status-pending {
            background-color: #f8f0e3;
            color: #8B7355;
            border: 1px solid #e8ddd2;
        }

        .status-shipped {
            background-color: #f8f0e3;
            color: #8B7355;
            border: 1px solid #e8ddd2;
        }

        .status-delivered {
            background-color: #e8d9c8;
            color: #5C4A3D;
            font-weight: 700;
        }

        .status-cancelled {
            background-color: #FDD8D8;
            color: #721c24;
            border: 1px solid #F5C6CB;
        }

        .status-processing {
            background-color: #f8f0e3;
            color: #666666;
            border: 1px solid #e8ddd2;
        }

        .action-buttons {
            display: flex;
            gap: 8px;
            flex-wrap: wrap;
        }

        .btn {
            padding: 6px 12px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 12px;
            font-weight: 600;
            transition: all 0.3s ease;
            text-decoration: none;
            display: inline-block;
        }

        .btn-view {
            background-color: #4FA392;
            color: white;
        }

        .btn-view:hover {
            background-color: #3D8577;
            transform: translateY(-1px);
            box-shadow: 0 4px 8px rgba(79, 163, 146, 0.3);
        }

        .btn-update {
            background-color: #d4c4a8;
            color: #2C2C2C;
            border: 1px solid #c4b498;
        }

        .btn-update:hover {
            background-color: #c4b498;
            transform: translateY(-1px);
        }

        .btn-cancel {
            background-color: #e8a8a8;
            color: #2C2C2C;
            border: 1px solid #d89898;
        }

        .btn-cancel:hover {
            background-color: #d89898;
            transform: translateY(-1px);
        }

        .pagination {
            display: flex;
            gap: 5px;
            justify-content: center;
            margin-top: 30px;
        }

        .pagination a, .pagination span {
            padding: 8px 12px;
            border: 1px solid #d4c4a8;
            border-radius: 4px;
            cursor: pointer;
            color: #4FA392;
            text-decoration: none;
            background-color: #f8f0e3;
        }

        .pagination a:hover {
            background-color: #4FA392;
            color: white;
            border-color: #4FA392;
        }

        .pagination .active {
            background-color: #4FA392;
            color: white;
            border-color: #4FA392;
        }

        .no-data {
            text-align: center;
            padding: 40px;
            color: #999;
            font-size: 16px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="header">
            <h1>📦 Order & Shipment Management</h1>
        </div>

        <div id="errorMessage" runat="server" class="error-message">
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </div>

        <div id="successMessage" runat="server" class="success-message">
            <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
        </div>

        <!-- Statistics Cards -->
        <div class="stats-grid">
            <div class="stat-card total">
                <div class="stat-label">Total Orders</div>
                <div class="stat-value"><asp:Label ID="lblTotalOrders" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="stat-card pending">
                <div class="stat-label">Pending Orders</div>
                <div class="stat-value" style="color: #8B7355;"><asp:Label ID="lblPendingOrders" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="stat-card shipped">
                <div class="stat-label">Shipped Orders</div>
                <div class="stat-value" style="color: #8B7355;"><asp:Label ID="lblShippedOrders" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="stat-card delivered">
                <div class="stat-label">Delivered Orders</div>
                <div class="stat-value"><asp:Label ID="lblDeliveredOrders" runat="server" Text="0"></asp:Label></div>
            </div>
            <div class="stat-card revenue">
                <div class="stat-label">Total Revenue</div>
                <div class="stat-value" style="color: #8B7355;"><asp:Label ID="lblTotalRevenue" runat="server" Text="$0.00"></asp:Label></div>
            </div>
        </div>

        <!-- Orders Table -->
        <div class="table-container">
            <asp:GridView ID="gvOrders" runat="server" 
                AutoGenerateColumns="False" 
                AllowPaging="True" 
                PageSize="10"
                OnPageIndexChanging="gvOrders_PageIndexChanging"
                OnRowCommand="gvOrders_RowCommand"
                CssClass="orders-table"
                EmptyDataText="No orders found.">
                <Columns>
                    <asp:BoundField DataField="OrderID" HeaderText="Order ID" ItemStyle-Width="80px" />
                    <asp:BoundField DataField="Username" HeaderText="Customer Name" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="ItemCount" HeaderText="Items" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="{0:C}" ItemStyle-Width="100px" />
                    <asp:TemplateField HeaderText="Order Date" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <%# Convert.ToDateTime(Eval("OrderDate")).ToString("MM/dd/yyyy") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <span class="status-badge status-<%# Eval("Status").ToString().ToLower() %>">
                                <%# Eval("Status") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tracking" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <%# string.IsNullOrEmpty(Eval("TrackingNumber").ToString()) ? "N/A" : Eval("TrackingNumber") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="180px">
                        <ItemTemplate>
                            <div class="action-buttons">
                                <asp:LinkButton ID="btnView" runat="server" 
                                    CommandName="ViewDetails" 
                                    CommandArgument='<%# Eval("OrderID") %>'
                                    CssClass="btn btn-view"
                                    Text="View" />
                                
                                <asp:LinkButton ID="btnUpdate" runat="server" 
                                    CommandName="UpdateShipment" 
                                    CommandArgument='<%# Eval("OrderID") %>'
                                    CssClass="btn btn-update"
                                    Text="Ship" />
                                
                                <asp:LinkButton ID="btnCancel" runat="server" 
                                    CommandName="CancelOrder" 
                                    CommandArgument='<%# Eval("OrderID") %>'
                                    CssClass="btn btn-cancel"
                                    Text="Cancel"
                                    OnClientClick="return confirm('Are you sure you want to cancel this order?');" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerSettings PageButtonCount="5" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
