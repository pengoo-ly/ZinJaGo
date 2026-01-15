<%@ Page Title="Customers" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="Week1_Practical1.Customers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.9.1/chart.min.js"></script>
    <style>
        .customers-container {
            display: grid;
            gap: 24px;
            max-width: 1400px;
            margin: 0 auto;
        }

        .page-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }

        .page-header h1 {
            font-size: 28px;
            font-weight: 700;
            color: var(--text);
            margin: 0;
        }

        .header-actions {
            display: flex;
            gap: 12px;
            align-items: center;
        }

        .btn-primary {
            background: var(--accent);
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 8px;
            font-size: 14px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.2s ease;
            display: inline-flex;
            align-items: center;
            gap: 6px;
        }

        .btn-primary:hover {
            opacity: 0.9;
            transform: translateY(-2px);
        }

        .stats-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
            margin-bottom: 28px;
        }

        .stat-card {
            background: var(--card);
            border-radius: 12px;
            padding: 20px;
            box-shadow: var(--shadow);
        }

        .stat-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 16px;
        }

        .stat-label {
            font-size: 13px;
            color: var(--muted);
            margin: 0;
        }

        .stat-options {
            background: none;
            border: none;
            color: var(--muted);
            cursor: pointer;
            font-size: 18px;
            padding: 0;
        }

        .stat-options:hover {
            color: var(--accent);
        }

        .stat-number {
            font-size: 32px;
            font-weight: 700;
            color: var(--text);
            margin: 8px 0;
        }

        .stat-change {
            font-size: 12px;
            color: var(--accent);
            font-weight: 600;
            margin-top: 8px;
        }

        .stat-subtext {
            font-size: 12px;
            color: var(--muted);
            margin-top: 4px;
        }

        .customer-overview {
            background: var(--card);
            border-radius: 12px;
            padding: 24px;
            box-shadow: var(--shadow);
            margin-bottom: 28px;
        }

        .overview-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }

        .overview-header h2 {
            font-size: 18px;
            font-weight: 600;
            color: var(--text);
            margin: 0;
        }

        .period-selector {
            display: flex;
            gap: 12px;
        }

        .period-btn {
            background: transparent;
            border: 1px solid rgba(0, 0, 0, 0.1);
            color: var(--muted);
            padding: 6px 12px;
            border-radius: 6px;
            font-size: 12px;
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .period-btn.active {
            background: var(--accent);
            color: white;
            border-color: var(--accent);
        }

        .period-btn:hover {
            border-color: var(--accent);
            color: var(--accent);
        }

        .overview-stats {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
            gap: 20px;
            margin-bottom: 24px;
        }

        .overview-stat {
            text-align: center;
        }

        .overview-stat-number {
            font-size: 24px;
            font-weight: 700;
            color: var(--text);
            margin-bottom: 4px;
        }

        .overview-stat-label {
            font-size: 12px;
            color: var(--muted);
        }

        .chart-container {
            height: 300px;
            position: relative;
            margin-bottom: 20px;
        }

        .customers-table-wrapper {
            background: var(--card);
            border-radius: 12px;
            padding: 24px;
            box-shadow: var(--shadow);
            overflow-x: auto;
        }

        .table-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
            flex-wrap: wrap;
            gap: 12px;
        }

        .table-search {
            flex: 1;
            min-width: 200px;
            position: relative;
        }

        .table-search input {
            width: 100%;
            padding: 10px 12px;
            border: 1px solid rgba(0, 0, 0, 0.1);
            border-radius: 6px;
            font-size: 13px;
            background: transparent;
            color: var(--text);
        }

        .table-search input::placeholder {
            color: var(--muted);
        }

        .table-search input:focus {
            outline: none;
            border-color: var(--accent);
            box-shadow: 0 0 0 2px rgba(28, 176, 116, 0.1);
        }

        table {
            width: 100%;
            border-collapse: collapse;
            font-size: 13px;
        }

        table thead {
            border-bottom: 2px solid rgba(0, 0, 0, 0.08);
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
            border-bottom: 1px solid rgba(0, 0, 0, 0.05);
            vertical-align: middle;
        }

        table tbody tr:hover {
            background: rgba(79, 163, 146, 0.04);
        }

        .customer-id {
            font-weight: 600;
            color: var(--text);
        }

        .customer-name {
            color: var(--text);
            font-weight: 500;
        }

        .badge {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: 4px 10px;
            border-radius: 4px;
            font-size: 12px;
            font-weight: 600;
        }

        .badge-active {
            background: rgba(79, 163, 146, 0.1);
            color: var(--accent);
        }

        .badge-inactive {
            background: rgba(220, 77, 77, 0.1);
            color: #dc4d4d;
        }

        .badge-vip {
            background: rgba(255, 193, 7, 0.1);
            color: #ffc107;
        }

        .status-dot {
            width: 8px;
            height: 8px;
            border-radius: 50%;
            display: inline-block;
        }

        .status-active {
            background: var(--accent);
        }

        .status-inactive {
            background: #dc4d4d;
        }

        .status-vip {
            background: #ffc107;
        }

        .action-buttons {
            display: flex;
            gap: 8px;
        }

        .action-btn {
            background: none;
            border: none;
            color: var(--muted);
            cursor: pointer;
            font-size: 16px;
            padding: 6px;
            transition: color 0.2s ease;
        }

        .action-btn:hover {
            color: var(--accent);
        }

        .pagination-wrapper {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 20px;
            padding-top: 20px;
            border-top: 1px solid rgba(0, 0, 0, 0.05);
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
            border: 1px solid rgba(0, 0, 0, 0.1);
            cursor: pointer;
            transition: all 0.2s ease;
        }

        .pagination a:hover {
            background: rgba(79, 163, 146, 0.1);
            color: var(--accent);
            border-color: var(--accent);
        }

        .pagination .active {
            background: var(--accent);
            color: white;
            border-color: var(--accent);
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

        .empty-state {
            text-align: center;
            padding: 40px 20px;
            color: var(--muted);
        }

        .empty-state-icon {
            font-size: 48px;
            margin-bottom: 16px;
        }

        @media (max-width: 768px) {
            .page-header {
                flex-direction: column;
                align-items: flex-start;
                gap: 16px;
            }

            .stats-grid {
                grid-template-columns: 1fr;
            }

            .overview-stats {
                grid-template-columns: repeat(2, 1fr);
            }

            .table-header {
                flex-direction: column;
                align-items: stretch;
            }

            table {
                font-size: 11px;
            }

            table th, table td {
                padding: 10px 6px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="customers-container">
        <!-- Page Header -->
        <div class="page-header">
            <div>
                <h1>Customers</h1>
            </div>
        </div>

        <!-- Statistics Cards -->
        <div class="stats-grid">
            <div class="stat-card">
                <div class="stat-header">
                    <p class="stat-label">Total Customers</p>
                    <button type="button" class="stat-options">⋯</button>
                </div>
                <div class="stat-number"><asp:Label ID="lblTotalCustomers" runat="server" Text="0"></asp:Label></div>
                <div class="stat-change"><asp:Label ID="lblTotalChange" runat="server" Text="↑ 0%"></asp:Label></div>
                <div class="stat-subtext">Last 7 days</div>
            </div>

            <div class="stat-card">
                <div class="stat-header">
                    <p class="stat-label">New Customers</p>
                    <button type="button" class="stat-options">⋯</button>
                </div>
                <div class="stat-number"><asp:Label ID="lblNewCustomers" runat="server" Text="0"></asp:Label></div>
                <div class="stat-change"><asp:Label ID="lblNewChange" runat="server" Text="↑ 0%"></asp:Label></div>
                <div class="stat-subtext">Last 7 days</div>
            </div>

            <div class="stat-card">
                <div class="stat-header">
                    <p class="stat-label">Visitor</p>
                    <button type="button" class="stat-options">⋯</button>
                </div>
                <div class="stat-number"><asp:Label ID="lblVisitors" runat="server" Text="0"></asp:Label></div>
                <div class="stat-change"><asp:Label ID="lblVisitorChange" runat="server" Text="↑ 0%"></asp:Label></div>
                <div class="stat-subtext">Last 7 days</div>
            </div>
        </div>

        <!-- Customer Overview Chart -->
        <div class="customer-overview">
            <div class="overview-header">
                <h2>Customer Overview</h2>
                <div class="period-selector">
                    <button type="button" class="period-btn active" data-period="week">This week</button>
                    <button type="button" class="period-btn" data-period="month">Last week</button>
                </div>
            </div>

            <div class="overview-stats">
                <div class="overview-stat">
                    <div class="overview-stat-number"><asp:Label ID="lblActiveCustomers" runat="server" Text="0"></asp:Label></div>
                    <div class="overview-stat-label">Active Customers</div>
                </div>
                <div class="overview-stat">
                    <div class="overview-stat-number"><asp:Label ID="lblInactiveCustomers" runat="server" Text="0"></asp:Label></div>
                    <div class="overview-stat-label">Inactive Customers</div>
                </div>
                <div class="overview-stat">
                    <div class="overview-stat-number"><asp:Label ID="lblTotalSpend" runat="server" Text="0"></asp:Label></div>
                    <div class="overview-stat-label">Total Spend</div>
                </div>
                <div class="overview-stat">
                    <div class="overview-stat-number"><asp:Label ID="lblConversionRate" runat="server" Text="0"></asp:Label></div>
                    <div class="overview-stat-label">Conversion Rate</div>
                </div>
            </div>

            <div class="chart-container">
                <canvas id="customerChart"></canvas>
            </div>
        </div>

        <!-- Customers Table -->
        <div class="customers-table-wrapper">
            <div class="table-header">
                <div class="table-search">
                    <input type="text" id="searchCustomers" placeholder="Search customers by name or email..." />
                </div>
            </div>

            <table id="customersTable">
                <thead>
                    <tr>
                        <th>Customer Id</th>
                        <th>Name</th>
                        <th>Phone</th>
                        <th>Order Count</th>
                        <th>Total Spent</th>
                        <th>Status</th>
                        <th style="width: 80px;">Action</th>
                    </tr>
                </thead>
                <tbody id="customersTableBody">
                    <asp:Repeater ID="rptCustomers" runat="server">
                        <ItemTemplate>
                            <tr class="customer-row" data-status="<%# Eval("Status").ToString().ToLower() %>">
                                <td class="customer-id">#<%# Eval("UserID") %></td>
                                <td class="customer-name"><%# Eval("Username") %></td>
                                <td><%# Eval("Phone") %></td>
                                <td><%# Eval("OrderCount") %></td>
                                <td>$<%# Eval("TotalSpent", "{0:0.00}") %></td>
                                <td>
                                    <span class="badge <%# Eval("Status").ToString() == "Active" ? "badge-active" : (Eval("Status").ToString() == "VIP" ? "badge-vip" : "badge-inactive") %>">
                                        <span class="status-dot <%# Eval("Status").ToString() == "Active" ? "status-active" : (Eval("Status").ToString() == "VIP" ? "status-vip" : "status-inactive") %>"></span>
                                        <%# Eval("Status") %>
                                    </span>
                                </td>
                                <td>
                                    <div class="action-buttons">
                                        <button type="button" class="action-btn" title="View">👁️</button>
                                        <button type="button" class="action-btn" title="Edit">✎</button>
                                        <button type="button" class="action-btn" title="Delete">🗑️</button>
                                    </div>
                                </td>
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
    </div>

    <script>
        let chartInstance = null;

        function renderChart(labels, data) {
            const ctx = document.getElementById('customerChart').getContext('2d');

            if (chartInstance) {
                chartInstance.destroy();
            }

            chartInstance = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Customers',
                        data: data,
                        borderColor: '#1cb074',
                        backgroundColor: 'rgba(28, 176, 116, 0.1)',
                        fill: true,
                        tension: 0.4,
                        pointBackgroundColor: '#1cb074',
                        pointBorderColor: '#fff',
                        pointBorderWidth: 2,
                        pointRadius: 5,
                        pointHoverRadius: 7
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: true,
                            labels: {
                                color: 'var(--text)',
                                font: {
                                    size: 12
                                }
                            }
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: {
                                color: 'var(--muted)'
                            },
                            grid: {
                                color: 'rgba(0, 0, 0, 0.05)'
                            }
                        },
                        x: {
                            ticks: {
                                color: 'var(--muted)'
                            },
                            grid: {
                                display: false
                            }
                        }
                    }
                }
            });
        }

        const ITEMS_PER_PAGE = 10;
        let allCustomers = [];
        let currentPage = 1;

        function initializeCustomers() {
            const rows = document.querySelectorAll('.customer-row');
            allCustomers = Array.from(rows).map((row, index) => ({
                index: index,
                element: row,
                status: row.dataset.status
            }));
            renderTable();
            updatePagination();
        }

        function renderTable() {
            const start = (currentPage - 1) * ITEMS_PER_PAGE;
            const end = start + ITEMS_PER_PAGE;
            const pageCustomers = allCustomers.slice(start, end);

            const tbody = document.getElementById('customersTableBody');
            tbody.innerHTML = '';

            if (pageCustomers.length === 0) {
                tbody.innerHTML = '<tr><td colspan="7"><div class="empty-state"><div class="empty-state-icon">📭</div><div>No customers found</div></div></td></tr>';
                return;
            }

            pageCustomers.forEach(customer => {
                tbody.appendChild(customer.element.cloneNode(true));
            });
        }

        function updatePagination() {
            const totalPages = Math.ceil(allCustomers.length / ITEMS_PER_PAGE) || 1;
            document.getElementById('currentPage').textContent = currentPage;
            document.getElementById('totalPages').textContent = totalPages;

            const prevBtn = document.querySelector('.prev-btn');
            const nextBtn = document.querySelector('.next-btn');

            if (currentPage === 1) {
                prevBtn.classList.add('disabled');
            } else {
                prevBtn.classList.remove('disabled');
            }

            if (currentPage >= totalPages) {
                nextBtn.classList.add('disabled');
            } else {
                nextBtn.classList.remove('disabled');
            }

            const paginationList = document.getElementById('paginationList');
            paginationList.innerHTML = '';

            for (let i = 1; i <= totalPages; i++) {
                const li = document.createElement('li');
                const a = document.createElement('a');
                a.href = '#';
                a.textContent = i;
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
            const totalPages = Math.ceil(allCustomers.length / ITEMS_PER_PAGE) || 1;
            if (currentPage < totalPages) {
                currentPage++;
                renderTable();
                updatePagination();
            }
        });

        document.getElementById('searchCustomers').addEventListener('input', function (e) {
            const query = e.target.value.toLowerCase();
            const rows = document.querySelectorAll('.customer-row');
            rows.forEach(row => {
                const text = row.textContent.toLowerCase();
                row.style.display = text.includes(query) ? '' : 'none';
            });
        });

        // Period selector
        document.querySelectorAll('.period-btn').forEach(btn => {
            btn.addEventListener('click', function () {
                document.querySelectorAll('.period-btn').forEach(b => b.classList.remove('active'));
                this.classList.add('active');
            });
        });

        // Initialize on page load
        document.addEventListener('DOMContentLoaded', initializeCustomers);
    </script>
</asp:Content>