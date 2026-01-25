<%@ Page Title="Order Management" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="OrderManagement.aspx.cs" Inherits="Week1_Practical1.OrderManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .order-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }

        .order-actions {
            display: flex;
            gap: 12px;
            align-items: center;
        }

        .btn-primary-custom {
            background: var(--accent);
            color: #fff;
            border: none;
            padding: 10px 18px;
            border-radius: 8px;
            font-size: 14px;
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            gap: 6px;
            transition: opacity 0.2s ease;
        }

            .btn-primary-custom:hover {
                opacity: 0.9;
            }

        .btn-secondary-custom {
            background: transparent;
            color: var(--text);
            border: 1px solid rgba(0,0,0,0.1);
            padding: 10px 18px;
            border-radius: 8px;
            font-size: 14px;
            cursor: pointer;
            transition: all 0.2s ease;
            position: relative;
        }

            .btn-secondary-custom:hover {
                background: rgba(0,0,0,0.03);
                border-color: var(--accent);
            }

        .dark .btn-secondary-custom {
            border-color: rgba(255,255,255,0.1);
        }

        .order-stats {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 16px;
            margin-bottom: 28px;
        }

        .stat-card {
            background: var(--card);
            border-radius: 12px;
            padding: 18px;
            box-shadow: var(--shadow);
            text-align: center;
        }

        .stat-number {
            font-size: 28px;
            font-weight: 700;
            color: var(--text);
            margin: 8px 0;
        }

        .stat-label {
            font-size: 13px;
            color: var(--muted);
            margin-bottom: 8px;
        }

        .stat-change {
            font-size: 12px;
            padding: 4px 8px;
            border-radius: 4px;
            display: inline-block;
        }

        .change-positive {
            background: rgba(79, 163, 146, 0.1);
            color: var(--accent);
        }

        .change-negative {
            background: rgba(220, 77, 77, 0.1);
            color: #dc4d4d;
        }

        .table-wrapper {
            background: var(--card);
            border-radius: 14px;
            padding: 20px;
            box-shadow: var(--shadow);
            overflow-x: auto;
        }

        .table-toolbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
            gap: 12px;
            flex-wrap: wrap;
        }

        .filter-tabs {
            display: flex;
            gap: 12px;
            align-items: center;
        }

        .filter-btn {
            background: transparent;
            border: 1px solid rgba(0,0,0,0.1);
            color: var(--muted);
            padding: 6px 14px;
            border-radius: 6px;
            font-size: 13px;
            cursor: pointer;
            transition: all 0.2s ease;
        }

            .filter-btn.active {
                background: var(--accent);
                color: #fff;
                border-color: var(--accent);
            }

            .filter-btn:hover {
                border-color: var(--accent);
            }

        .dark .filter-btn {
            border-color: rgba(255,255,255,0.1);
        }

        .table-search {
            position: relative;
            flex: 1;
            min-width: 200px;
        }

        .table-search input {
            width: 100%;
            background: transparent;
            border: 1px solid rgba(0,0,0,0.1);
            padding: 8px 12px;
            border-radius: 6px;
            color: var(--text);
            font-size: 13px;
        }

        .dark .table-search input {
            border-color: rgba(255,255,255,0.1);
        }

        .table-search input::placeholder {
            color: var(--muted);
        }

        .table-action-icons {
            display: flex;
            gap: 8px;
            align-items: center;
            position: relative;
        }

        .icon-btn {
            background: transparent;
            border: none;
            color: var(--muted);
            cursor: pointer;
            font-size: 18px;
            padding: 6px;
            transition: color 0.2s ease;
        }

            .icon-btn:hover {
                color: var(--accent);
            }

        .dropdown-menu {
            display: none;
            position: absolute;
            top: 35px;
            right: 0;
            background: var(--card);
            border: 1px solid rgba(0,0,0,0.1);
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
            min-width: 200px;
            z-index: 1000;
        }

        .dark .dropdown-menu {
            border-color: rgba(255,255,255,0.1);
            box-shadow: 0 4px 12px rgba(0,0,0,0.3);
        }

        .dropdown-menu.show {
            display: block;
        }

        .dropdown-item {
            padding: 10px 16px;
            color: var(--text);
            cursor: pointer;
            border: none;
            background: transparent;
            width: 100%;
            text-align: left;
            font-size: 13px;
            transition: background 0.2s ease;
        }

            .dropdown-item:hover {
                background: rgba(79, 163, 146, 0.1);
                color: var(--accent);
            }

        .dropdown-divider {
            height: 1px;
            background: rgba(0,0,0,0.05);
            margin: 6px 0;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            font-size: 13px;
        }

        table thead {
            border-bottom: 2px solid rgba(0,0,0,0.08);
        }

        .dark table thead {
            border-bottom-color: rgba(255,255,255,0.08);
        }

        table th {
            text-align: left;
            padding: 12px 8px;
            font-weight: 600;
            color: var(--muted);
            font-size: 12px;
            text-transform: uppercase;
        }

        table td {
            padding: 14px 8px;
            border-bottom: 1px solid rgba(0,0,0,0.05);
            vertical-align: middle;
        }

        .dark table td {
            border-bottom-color: rgba(255,255,255,0.05);
        }

        table tbody tr:hover {
            background: rgba(79, 163, 146, 0.04);
        }

        .product-cell {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .product-icon {
            width: 32px;
            height: 32px;
            border-radius: 6px;
            background: rgba(0,0,0,0.05);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 16px;
            overflow: hidden;
        }

        .product-icon img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

        .product-name {
            font-weight: 500;
            color: var(--text);
        }

        .badge {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            padding: 4px 10px;
            border-radius: 4px;
            font-size: 12px;
            font-weight: 500;
        }

        .badge-paid {
            background: rgba(79, 163, 146, 0.1);
            color: var(--accent);
        }

        .badge-unpaid {
            background: rgba(220, 77, 77, 0.1);
            color: #dc4d4d;
        }

        .badge-delivered {
            background: rgba(79, 163, 146, 0.1);
            color: var(--accent);
        }

        .badge-pending {
            background: rgba(255, 159, 64, 0.1);
            color: #ff9f40;
        }

        .badge-shipped {
            background: rgba(100, 150, 200, 0.1);
            color: #6496c8;
        }

        .badge-cancelled {
            background: rgba(220, 77, 77, 0.1);
            color: #dc4d4d;
        }

        .pagination-wrapper {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 20px;
            padding-top: 20px;
            border-top: 1px solid rgba(0,0,0,0.05);
        }

        .dark .pagination-wrapper {
            border-top-color: rgba(255,255,255,0.05);
        }

        .pagination {
            display: flex;
            gap: 6px;
            list-style: none;
            padding: 0;
            margin: 0;
        }

        .pagination a, .pagination span {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 30px;
            height: 30px;
            border-radius: 6px;
            text-decoration: none;
            color: var(--muted);
            font-size: 13px;
            border: 1px solid rgba(0,0,0,0.1);
            transition: all 0.2s ease;
            cursor: pointer;
        }

            .pagination a:hover {
                background: rgba(79, 163, 146, 0.1);
                color: var(--accent);
                border-color: var(--accent);
            }

        .pagination .active {
            background: var(--accent);
            color: #fff;
            border-color: var(--accent);
        }

        .dark .pagination a, .dark .pagination span {
            border-color: rgba(255,255,255,0.1);
        }

        .pagination-nav {
            display: flex;
            gap: 12px;
            align-items: center;
        }

        .pagination-nav a {
            color: var(--accent);
            text-decoration: none;
            font-size: 13px;
            display: flex;
            align-items: center;
            gap: 4px;
        }

            .pagination-nav a:hover {
                text-decoration: underline;
            }

        .pagination-nav a.disabled {
            color: var(--muted);
            cursor: not-allowed;
            text-decoration: none;
        }

        @media (max-width: 768px) {
            .order-header {
                flex-direction: column;
                align-items: flex-start;
            }

            .order-stats {
                grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
            }

            .table-toolbar {
                flex-direction: column;
                align-items: stretch;
            }

            .filter-tabs {
                overflow-x: auto;
            }

            table {
                font-size: 12px;
            }

            table th, table td {
                padding: 10px 6px;
            }

            .product-cell {
                flex-direction: column;
                align-items: flex-start;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="order-header">
        <div>
            <h2>Order Management</h2>
            <p style="color: var(--muted); margin: 6px 0 0 0; font-size: 13px;">Manage all customer orders and their details</p>
        </div>
    </div>

    <!-- Order Statistics -->
    <div class="order-stats">
        <div class="stat-card">
            <div class="stat-label">Total Orders</div>
            <div class="stat-number"><asp:Label ID="lblTotalOrders" runat="server" /></div>
            <div class="stat-change change-positive">↑ 14% <span>Last 7 days</span></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">New Orders</div>
            <div class="stat-number"><asp:Label ID="lblNewOrders" runat="server" /></div>
            <div class="stat-change change-positive">↑ 20% <span>Last 7 days</span></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Completed Orders</div>
            <div class="stat-number"><asp:Label ID="lblCompletedOrders" runat="server" /></div>
            <div class="stat-change change-positive">↑ 4% <span>Last 7 days</span></div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Canceled Orders</div>
            <div class="stat-number"><asp:Label ID="lblCanceledOrders" runat="server" /></div>
            <div class="stat-change change-negative">↓ 4% <span>Last 7 days</span></div>
        </div>
    </div>

    <!-- Orders Table -->
    <div class="table-wrapper">
        <div class="table-toolbar">
            <div class="filter-tabs">
                <button type="button" class="filter-btn active" data-filter="all">All orders (<span class="count-all"><asp:Label ID="lblAllOrderCount" runat="server" /></span>)</button>
                <button type="button" class="filter-btn" data-filter="completed">Completed</button>
                <button type="button" class="filter-btn" data-filter="pending">Pending</button>
            </div>

            <div class="table-search">
                <input type="text" placeholder="Search order report" id="searchOrders" />
            </div>

            <div class="table-action-icons">
                <button type="button" class="icon-btn sort-btn" title="Sort">⇅</button>
                <div style="position: relative;">
                    <button type="button" class="icon-btn more-btn" title="More options">⋯</button>
                    <div class="dropdown-menu" id="dropdownMenu">
                        <button class="dropdown-item" data-action="price-asc">Sort by Price (Low to High)</button>
                        <button class="dropdown-item" data-action="price-desc">Sort by Price (High to Low)</button>
                        <div class="dropdown-divider"></div>
                        <button class="dropdown-item" data-action="date-newest">Date (Newest First)</button>
                        <button class="dropdown-item" data-action="date-oldest">Date (Oldest First)</button>
                        <div class="dropdown-divider"></div>
                        <button class="dropdown-item" data-action="paid">Only Paid Orders</button>
                        <button class="dropdown-item" data-action="unpaid">Only Unpaid Orders</button>
                    </div>
                </div>
            </div>
        </div>

        <table>
            <thead>
                <tr>
                    <th style="width: 30px;"><input type="checkbox" id="selectAll" /></th>
                    <th>No.</th>
                    <th>Order Id</th>
                    <th>Product</th>
                    <th>Date</th>
                    <th>Price</th>
                    <th>Payment</th>
                    <th>Status</th>
                    <th style="width: 40px;"></th>
                </tr>
            </thead>
            <tbody id="ordersTableBody">
                <asp:Repeater ID="rptOrders" runat="server">
                    <ItemTemplate>
                        <tr class="order-row" data-order-status="<%# Eval("OrderStatus").ToString().ToLower() %>" data-payment-status="<%# Eval("PaymentStatus").ToString().ToLower() %>" data-price="<%# Eval("Price") %>" data-date="<%# ((DateTime)Eval("OrderDate")).Ticks %>">
                            <td><input type="checkbox" class="row-checkbox" /></td>
                            <td class="row-number"><%# Container.ItemIndex + 1 %></td>
                            <td><strong><%# Eval("OrderID") %></strong></td>
                            <td>
                                <div class="product-cell">
                                    <div class="product-icon">
                                        <img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("ProductName") %>" alt="<%# Eval("ProductName") %>" />
                                    </div>
                                    <div class="product-name"><%# Eval("ProductName") %></div>
                                </div>
                            </td>
                            <td><%# Eval("OrderDate", "{0:dd-MM-yyyy}") %></td>
                            <td data-price-value="<%# Eval("Price") %>">$<%# Eval("Price", "{0:0.00}") %></td>
                            <td>
                                <span class="badge <%# Eval("PaymentStatus").ToString() == "Paid" ? "badge-paid" : "badge-unpaid" %>">
                                    ● <%# Eval("PaymentStatus") %>
                                </span>
                            </td>
                            <td>
                                <span class="badge <%# "badge-" + Eval("OrderStatus").ToString().ToLower() %>">
                                    <%# GetStatusIcon(Eval("OrderStatus").ToString()) %> <%# Eval("OrderStatus") %>
                                </span>
                            </td>
                            <td><button type="button" class="icon-btn">⋯</button></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>

        <!-- Pagination -->
        <div class="pagination-wrapper">
            <div class="pagination-nav">
                <a href="#" class="prev-btn">← Previous</a>
                <span style="color: var(--muted); font-size: 12px;">Page <span id="currentPage">1</span> of <span id="totalPages">1</span></span>
                <a href="#" class="next-btn">Next →</a>
            </div>

            <ul class="pagination" id="paginationList">
            </ul>
        </div>
    </div>

    <script>
        const ITEMS_PER_PAGE = 10;
        let allOrders = [];
        let filteredOrders = [];
        let currentPage = 1;
        let currentFilter = 'all';
        let sortOrder = 'desc'; // desc = newest first, asc = oldest first

            // Generate page numbers
        const paginationList = document.getElementById('paginationList');
            paginationList.innerHTML = '';

            for (let i = 1; i <= totalPages; i++) {
                const li = document.createElement('li');
                const a = document.createElement('a');
                a.href = '#';
                a.textContent = i;
                a.classList.add('pagination-btn');
                if (i === currentPage) a.classList.add('active');
                a.addEventListener('click', (e) => {
                    e.preventDefault();
                    currentPage = i;
                    renderTable();
                    updatePagination();
                });
                li.appendChild(a);
                paginationList.appendChild(li);
            }
        }

        // Search functionality
        function applySearch() {
            const query = document.getElementById('searchOrders').value.toLowerCase();
            const rows = document.querySelectorAll('#ordersTableBody tr');
            rows.forEach(row => {
                const text = row.textContent.toLowerCase();
                row.style.display = text.includes(query) ? '' : 'none';
            });
        }

        // Event Listeners
        document.querySelectorAll('.filter-btn').forEach(btn => {
            btn.addEventListener('click', function () {
                document.querySelectorAll('.filter-btn').forEach(b => b.classList.remove('active'));
                this.classList.add('active');
                filterOrders(this.dataset.filter);
            });
        });

        document.getElementById('searchOrders').addEventListener('input', applySearch);

        document.querySelector('.prev-btn').addEventListener('click', (e) => {
            e.preventDefault();
            if (currentPage > 1) {
                currentPage--;
                renderTable();
                updatePagination();
            }
        });

        document.querySelector('.next-btn').addEventListener('click', (e) => {
            e.preventDefault();
            const totalPages = Math.ceil(filteredOrders.length / ITEMS_PER_PAGE) || 1;
            if (currentPage < totalPages) {
                currentPage++;
                renderTable();
                updatePagination();
            }
        });

        // Sort button
        document.querySelector('.sort-btn').addEventListener('click', () => {
            if (sortOrder === 'desc') {
                sortOrders('date-oldest');
                sortOrder = 'asc';
            } else {
                sortOrders('date-newest');
                sortOrder = 'desc';
            }
        });

        // Dropdown menu
        document.querySelector('.more-btn').addEventListener('click', () => {
            document.getElementById('dropdownMenu').classList.toggle('show');
        });

        document.querySelectorAll('.dropdown-item').forEach(item => {
            item.addEventListener('click', () => {
                sortOrders(item.dataset.action);
                document.getElementById('dropdownMenu').classList.remove('show');
            });
        });

        // Close dropdown when clicking outside
        document.addEventListener('click', (e) => {
            if (!e.target.closest('.table-action-icons')) {
                document.getElementById('dropdownMenu').classList.remove('show');
            }
        });

        // Select all checkbox
        document.getElementById('selectAll').addEventListener('change', function () {
            document.querySelectorAll('.row-checkbox').forEach(cb => cb.checked = this.checked);
        });

        // Initialize on load
        initializeData();
    </script>
</asp:Content>