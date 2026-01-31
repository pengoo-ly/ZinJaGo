<%@ Page Title="Warehouse Management" Language="C#" AutoEventWireup="true" CodeBehind="WarehouseManagement.aspx.cs" Inherits="EcommerceWebsite.WarehouseManagement" MasterPageFile="~/Admin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f5f0e8;
            color: #333;
            line-height: 1.6;
        }

        .container {
            max-width: 1400px;
            margin: 0 auto;
            padding: 30px 20px;
        }

        /* Header Section */
        .header {
            margin-bottom: 40px;
            animation: slideDown 0.6s ease-out;
        }

        .header h1 {
            font-size: 2.5rem;
            color: #2c2c2c;
            margin-bottom: 8px;
            font-weight: 600;
        }

        .header p {
            font-size: 1rem;
            color: #666;
        }

        /* Key Metrics Cards */
        .metrics-grid {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 15px;
            margin-bottom: 40px;
            animation: fadeInUp 0.8s ease-out;
        }

        .metric-card {
            background: white;
            border-radius: 8px;
            padding: 18px 15px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
            transition: all 0.3s ease;
            border-left: 4px solid #4a9b8e;
        }

        .metric-card:hover {
            transform: translateY(-4px);
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.12);
        }

        .metric-card.warning {
            border-left-color: #d97706;
        }

        .metric-card.danger {
            border-left-color: #ef4444;
        }

        .metric-card.success {
            border-left-color: #10b981;
        }

        .metric-label {
            font-size: 0.75rem;
            color: #999;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            margin-bottom: 8px;
            font-weight: 600;
        }

        .metric-value {
            font-size: 1.8rem;
            color: #2c2c2c;
            font-weight: 700;
            margin-bottom: 8px;
        }

        .metric-change {
            font-size: 0.75rem;
            display: inline-block;
            padding: 3px 6px;
            border-radius: 4px;
            background-color: #f0f9f7;
            color: #4a9b8e;
        }

        .metric-change.negative {
            background-color: #fef2f2;
            color: #ef4444;
        }

        /* Controls Section */
        .controls {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 25px;
            flex-wrap: wrap;
            gap: 15px;
            animation: fadeInUp 1s ease-out;
        }

        .filter-tabs {
            display: flex;
            gap: 10px;
        }

        .filter-tabs button {
            background: white;
            border: 1px solid #e0ddd5;
            padding: 10px 18px;
            border-radius: 6px;
            cursor: pointer;
            font-size: 0.9rem;
            color: #666;
            transition: all 0.3s ease;
            font-weight: 500;
        }

        .filter-tabs button.active {
            background-color: #4a9b8e;
            color: white;
            border-color: #4a9b8e;
        }

        .filter-tabs button:hover:not(.active) {
            background-color: #f9f7f4;
            border-color: #d0ccc4;
        }

        .search-box {
            flex: 1;
            min-width: 250px;
            position: relative;
        }

        .search-box input {
            width: 100%;
            padding: 10px 15px;
            border: 1px solid #e0ddd5;
            border-radius: 6px;
            font-size: 0.9rem;
            transition: all 0.3s ease;
        }

        .search-box input:focus {
            outline: none;
            border-color: #4a9b8e;
            box-shadow: 0 0 0 3px rgba(74, 155, 142, 0.1);
        }

        .action-buttons {
            display: flex;
            gap: 10px;
        }

        .btn {
            padding: 10px 16px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 0.9rem;
            transition: all 0.3s ease;
            font-weight: 500;
            display: flex;
            align-items: center;
            gap: 6px;
        }

        .btn-sort {
            background: white;
            border: 1px solid #e0ddd5;
            color: #666;
        }

        .btn-sort:hover {
            background-color: #f9f7f4;
            border-color: #d0ccc4;
        }

        .btn-menu {
            background: white;
            border: 1px solid #e0ddd5;
            color: #666;
            padding: 10px 12px;
        }

        .btn-menu:hover {
            background-color: #f9f7f4;
            border-color: #d0ccc4;
        }

        /* Table Section */
        .table-wrapper {
            background: white;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
            animation: fadeInUp 1.2s ease-out;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        thead {
            background-color: #faf8f5;
            border-bottom: 2px solid #e0ddd5;
        }

        th {
            padding: 16px 18px;
            text-align: left;
            font-size: 0.85rem;
            color: #666;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        td {
            padding: 16px 18px;
            border-bottom: 1px solid #f0ede5;
            font-size: 0.95rem;
        }

        tbody tr {
            transition: background-color 0.2s ease;
        }

        tbody tr:hover {
            background-color: #faf8f5;
        }

        .checkbox {
            width: 18px;
            height: 18px;
            cursor: pointer;
            accent-color: #4a9b8e;
        }

        .status-badge {
            display: inline-block;
            padding: 6px 12px;
            border-radius: 20px;
            font-size: 0.8rem;
            font-weight: 600;
            text-transform: capitalize;
        }

        .status-in-stock {
            background-color: #d1fae5;
            color: #065f46;
        }

        .status-low-stock {
            background-color: #fed7aa;
            color: #92400e;
        }

        .status-out-of-stock {
            background-color: #fee2e2;
            color: #991b1b;
        }

        .status-pending {
            background-color: #dbeafe;
            color: #1e40af;
        }

        .sku {
            font-family: 'Courier New', monospace;
            font-size: 0.85rem;
            color: #999;
            font-weight: 500;
        }

        /* Pagination Section */
        .pagination {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 20px 18px;
            background-color: #faf8f5;
            border-top: 1px solid #e0ddd5;
        }

        .pagination-info {
            font-size: 0.9rem;
            color: #666;
        }

        .pagination-controls {
            display: flex;
            gap: 8px;
            align-items: center;
        }

        .pagination-controls a,
        .pagination-controls button {
            padding: 8px 12px;
            border: 1px solid #e0ddd5;
            background: white;
            color: #666;
            border-radius: 4px;
            cursor: pointer;
            font-size: 0.9rem;
            transition: all 0.2s ease;
            text-decoration: none;
        }

        .pagination-controls a:hover,
        .pagination-controls button:hover {
            border-color: #4a9b8e;
            color: #4a9b8e;
        }

        .pagination-number {
            background-color: #4a9b8e;
            color: white;
            border-color: #4a9b8e;
            pointer-events: none;
            min-width: 36px;
            text-align: center;
        }

        /* Empty State */
        .empty-state {
            text-align: center;
            padding: 60px 20px;
            color: #999;
        }

        .empty-state-icon {
            font-size: 3rem;
            margin-bottom: 20px;
            opacity: 0.5;
        }

        .empty-state h3 {
            color: #666;
            margin-bottom: 10px;
            font-size: 1.2rem;
        }

        /* Animations */
        @keyframes slideDown {
            from {
                opacity: 0;
                transform: translateY(-20px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(20px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        /* Responsive Design */
        @media (max-width: 1200px) {
            .metrics-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 768px) {
            .header h1 {
                font-size: 1.8rem;
            }

            .metrics-grid {
                grid-template-columns: 1fr;
            }

            .controls {
                flex-direction: column;
                align-items: stretch;
            }

            .filter-tabs {
                flex-wrap: wrap;
            }

            .search-box {
                min-width: unset;
            }

            .action-buttons {
                justify-content: flex-start;
            }

            table {
                font-size: 0.85rem;
            }

            th, td {
                padding: 12px 10px;
            }

            .pagination {
                flex-direction: column;
                gap: 15px;
            }

            .pagination-controls {
                width: 100%;
                justify-content: center;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="container">
            <!-- Header -->
            <div class="header">
                <h1>Warehouse Management</h1>
                <p>Manage inventory levels, stock movements, and warehouse operations</p>
            </div>

            <!-- Key Metrics -->
            <div class="metrics-grid">
                <div class="metric-card">
                    <div class="metric-label">Total Stock Items</div>
                    <div class="metric-value"><asp:Label ID="lblTotalItems" runat="server">0</asp:Label></div>
                    <div class="metric-change"><asp:Label ID="lblTotalItemsChange" runat="server">↑ 12% Last 7 days</asp:Label></div>
                </div>

                <div class="metric-card">
                    <div class="metric-label">In Stock</div>
                    <div class="metric-value"><asp:Label ID="lblInStock" runat="server">0</asp:Label></div>
                    <div class="metric-change"><asp:Label ID="lblInStockChange" runat="server">↑ 8% Last 7 days</asp:Label></div>
                </div>

                <div class="metric-card warning">
                    <div class="metric-label">Low Stock Items</div>
                    <div class="metric-value"><asp:Label ID="lblLowStock" runat="server">0</asp:Label></div>
                    <div class="metric-change negative"><asp:Label ID="lblLowStockChange" runat="server">↑ 2% Last 7 days</asp:Label></div>
                </div>

                <div class="metric-card danger">
                    <div class="metric-label">Out of Stock</div>
                    <div class="metric-value"><asp:Label ID="lblOutOfStock" runat="server">0</asp:Label></div>
                    <div class="metric-change negative"><asp:Label ID="lblOutOfStockChange" runat="server">↑ 1 Last 7 days</asp:Label></div>
                </div>
            </div>

            <!-- Controls -->
            <div class="controls">
                <div class="filter-tabs">
                    <asp:Button ID="btnAllWarehouses" runat="server" Text="All Warehouses (0)" CssClass="filter-tabs" OnClick="FilterWarehouses_Click" />
                    <asp:Button ID="btnLowStock" runat="server" Text="Low Stock" CssClass="filter-tabs" OnClick="FilterLowStock_Click" />
                    <asp:Button ID="btnMovements" runat="server" Text="Recent Movements" CssClass="filter-tabs" OnClick="FilterMovements_Click" />
                </div>

                <div class="search-box">
                    <asp:TextBox ID="txtSearch" runat="server" Placeholder="Search by SKU, product name, or warehouse..." />
                </div>

                <div class="action-buttons">
                    <asp:Button ID="btnSort" runat="server" Text="↑↓ Sort" CssClass="btn btn-sort" OnClick="SortTable_Click" />
                    <asp:Button ID="btnMenu" runat="server" Text="⋯" CssClass="btn btn-menu" OnClick="ShowMenu_Click" />
                </div>
            </div>

            <!-- Table -->
            <div class="table-wrapper">
                <asp:GridView ID="gvWarehouse" runat="server" AutoGenerateColumns="false" CssClass="warehouse-table" OnRowDataBound="GvWarehouse_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <input type="checkbox" class="checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SKU" HeaderText="SKU" />
                        <asp:BoundField DataField="ProductName" HeaderText="PRODUCT NAME" />
                        <asp:BoundField DataField="WarehouseName" HeaderText="WAREHOUSE" />
                        <asp:BoundField DataField="Quantity" HeaderText="QUANTITY" />
                        <asp:BoundField DataField="ReorderLevel" HeaderText="REORDER LEVEL" />
                        <asp:TemplateField HeaderText="STATUS">
                            <ItemTemplate>
                                <span class="status-badge" id="statusBadge" runat="server">In Stock</span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LastMovement" HeaderText="LAST MOVEMENT" />
                        <asp:BoundField DataField="Location" HeaderText="LOCATION" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="empty-state">
                            <div class="empty-state-icon">📦</div>
                            <h3>No Warehouse Data</h3>
                            <p>No inventory items to display at this time.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            <!-- Pagination -->
            <div class="pagination">
                <div class="pagination-info">
                    <asp:Label ID="lblPageInfo" runat="server">Page 1 of 1</asp:Label>
                </div>
                <div class="pagination-controls">
                    <asp:LinkButton ID="lbPrevious" runat="server" OnClick="PreviousPage_Click">← Previous</asp:LinkButton>
                    <span class="pagination-number">1</span>
                    <asp:LinkButton ID="lbNext" runat="server" OnClick="NextPage_Click">Next →</asp:LinkButton>
                </div>
            </div>
        </div>
    </asp:Content>

