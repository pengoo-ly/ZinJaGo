<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="Week1_Practical1.AdminDashboard" %>
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Admin Dashboard</h2> 
    <p>Welcome back! Here’s a quick look at how things are running today. If you need to make any changes or check on the team, you’ll find everything in the sidebar.</p>
             <div class="row g-3 mb-4">

                <!-- Total Users -->
                <div class="col-md-3">
                    <div id="cardUsers" class="card">
                        <h6>Total Users</h6>
                        <h3><asp:Label ID="lblUsers" runat="server" /></h3>
                    </div>
                </div>

                <div class="col-md-3">
                    <div id="cardOrders" class="card">
                        <h6>Total Orders</h6>
                        <h3><asp:Label ID="lblOrders" runat="server" /></h3>
                    </div>
                </div>

                <div class="col-md-3">
                    <div id="cardProducts" class="card">
                        <h6>Total Products</h6>
                        <h3><asp:Label ID="lblProducts" runat="server" /></h3>
                    </div>
                </div>

                <div class="col-md-3">
                    <div id="cardRevenue" class="card">
                        <h6>Total Revenue (SGD)</h6>
                        <h3>$<asp:Label ID="lblRevenue" runat="server" /></h3>
                    </div>
                </div>

            </div>
    <br />

        <!-- Tabs -->
        <div class="dashboard-tabs mb-3">
            <button type="button" class="tab-btn active" data-tab="revenue">Revenue</button>
            <button type="button" class="tab-btn" data-tab="logistics">Logistics</button>
        </div>

        <!-- Revenue tab -->
        <div id="revenue-tab" class="tab-content">
            <label for="ddlYear">Select Year:</label>
            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" CssClass="form-select"></asp:DropDownList>
            <canvas id="revenueChart"></canvas>
        </div>

    <br />

    <!-- Logistics tab -->
    <div id="logistics-tab" class="tab-content" style="display:none">
        <div class="list-row list-header">
            <span><strong>Shipment ID</strong></span>
            <span><strong>Status</strong></span>
            <span><strong>Shipped Date</strong></span>
        </div>

        <asp:Repeater ID="rptLogistics" runat="server">
            <ItemTemplate>
                <div class="list-row">
                    <span><%# Eval("ShipmentID") %></span>
                    <span><%# Eval("Status") != DBNull.Value ? Eval("Status") : "-" %></span>
                    <span><%# Eval("ShippedDate") != DBNull.Value ? string.Format("{0:dd MMM yyyy}", Eval("ShippedDate")) : "-" %></span>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <br />
    <!-- Top Selling Products -->
        <div class="col-md-4">
            <div class="dashboard-panel">
                <h5>Top Selling Products</h5>
                <div class="list-row list-header">
                    <span><strong>Product Name</strong></span>
                    <strong>Total Sold</strong>
                </div>
                <asp:Repeater ID="rptTopProducts" runat="server">
                    <ItemTemplate>
                        <div class="list-row">
                            <span><%# Eval("ProductName") %></span>
                            <strong><%# Eval("TotalSold") %></strong>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>
    <br />
    <!-- TOP CATEGORIES -->
    <div class="row g-3">

        <div class="col-md-12">
            <div class="dashboard-panel">

                <div class="panel-header">
                    <h5>Top Categories</h5>

                    <asp:Button ID="btnAddProduct" runat="server" Text="+ Add Product" CssClass="btn-add" PostBackUrl="~/AddProducts.aspx" />


                </div>
                <div class="list-row list-header">
                    <span><strong>Category Name</strong></span>
                    <strong>Total Sales</strong>
                </div>

                <asp:Repeater ID="rptTopCategories" runat="server">
                    <ItemTemplate>
                        <div class="list-row">
                            <span><%# Eval("CategoryName") %></span>
                            <strong><%# Eval("TotalSales") %></strong>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>

    </div>

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>
    // Revenue Chart
    const labels = <%= RevenueLabels %>;
    const data = <%= RevenueData %>;

    const ctx = document.getElementById('revenueChart').getContext('2d');
    const revenueChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Revenue (SGD)',
                data: data,
                borderColor: 'var(--accent)',
                backgroundColor: 'rgba(79, 163, 146, 0.2)',
                tension: 0.3,
                fill: true
            }]
        },
        options: {
            responsive: true,
            plugins: {
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return '$' + parseFloat(context.raw).toFixed(2);
                        }
                    }
                }
            },
            scales: {
                y: { beginAtZero: true },
                x: { title: { display: true, text: 'Month' } }
            }
        }
    });

    // Tabs JS
    document.querySelectorAll('.tab-btn').forEach(btn => {
        btn.addEventListener('click', function() {
            document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
            btn.classList.add('active');

            let tab = btn.getAttribute('data-tab');
            document.getElementById('revenue-tab').style.display = (tab==='revenue') ? 'block' : 'none';
            document.getElementById('logistics-tab').style.display = (tab==='logistics') ? 'block' : 'none';
        });
    });
</script>
</asp:Content>
