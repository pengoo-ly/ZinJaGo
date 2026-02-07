<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="ZinJaGODashboard.aspx.cs" Inherits="Week1_Practical1.SuperAdmin.ZinJaGODashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>ZinJaGoDashboard</h3>
    <p>Welcome to the ZinJaGo Dashboard. Here you can manage all aspects of the ZinJaGo application.</p>
    <br />
    <!-- KPI CARDS -->
    <div class="row g-3 mb-4">

        <div class="col-md-3">
            <div class="card">
                <h6>Total Users</h6>
                <h3><asp:Label ID="lblUsers" runat="server" /></h3>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card">
                <h6>Total Admins</h6>
                <h3><asp:Label ID="lblAdmins" runat="server" /></h3>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card">
                <h6>Total Orders</h6>
                <h3><asp:Label ID="lblOrders" runat="server" /></h3>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card">
                <h6>Total Revenue (SGD)</h6>
                <h3>$<asp:Label ID="lblRevenue" runat="server" /></h3>
            </div>
        </div>

    </div>

    <!-- CHARTS -->
    <div class="row g-3">

        <div class="col-md-6">
            <div class="dashboard-panel">
                <div class="panel-header">
                    <h5>Monthly Revenue (All Admins)</h5>
                    <div>
                        <span style="margin-right:6px;font-weight:600;">Year</span>
                        <asp:DropDownList ID="ddlRevenueYear"
                            runat="server"
                            AutoPostBack="true"
                            CssClass="status-dropdown"
                            OnSelectedIndexChanged="ddlYear_Changed" />
                    </div>
                </div>
                <canvas id="revenueChart"></canvas>
            </div>
        </div>

        <div class="col-md-6">
            <div class="dashboard-panel">
               <div class="panel-header">
                    <h5>Orders by Status</h5>
                    <div>
                        <span style="margin-right:6px;font-weight:600;">Year</span>
                        <asp:DropDownList ID="ddlOrderYear"
                            runat="server"
                            AutoPostBack="true"
                            CssClass="status-dropdown"
                            OnSelectedIndexChanged="ddlYear_Changed" />
                    </div>
                </div>
                <canvas id="orderStatusChart"></canvas>
            </div>
        </div>

    </div>

    <br />

    <!-- TOP ADMINS -->
    <div class="dashboard-panel">
        <h5>Top Admins by Revenue</h5>

        <div class="list-row list-header">
            <strong>Admin</strong>
            <strong>Revenue (SGD)</strong>
        </div>

        <asp:Repeater ID="rptTopAdmins" runat="server">
            <ItemTemplate>
                <div class="list-row">
                    <span><%# Eval("AdminName") %></span>
                    <strong>$<%# Eval("Revenue","{0:0.00}") %></strong></div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <br />

    <div class="dashboard-panel">
        <h5>Top Categories by Revenue</h5>

        <div class="list-row list-header">
            <strong>Category</strong>
            <strong>Revenue (SGD)</strong>
        </div>

        <asp:Repeater ID="rptTopCategories" runat="server">
            <ItemTemplate>
                <div class="list-row">
                    <span><%# Eval("CategoryName") %></span>
                    <strong>$<%# Eval("Revenue","{0:0.00}") %></strong></div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <br />
    <div class="dashboard-panel">
        <h5>Top Products</h5>

        <div class="list-row list-header">
            <strong>Product</strong>
            <strong>Revenue</strong>
        </div>

        <asp:Repeater ID="rptTopProducts" runat="server">
            <ItemTemplate>
                <div class="list-row">
                    <span><%# Eval("ProductName") %></span>
                    <strong>$<%# Eval("Revenue","{0:0.00}") %></strong></div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <br />
    <div class="dashboard-panel">
        <h5>Top Customers</h5>

        <div class="list-row list-header">
            <strong>Customer</strong>
            <strong>Total Spent (SGD)</strong>
        </div>

        <asp:Repeater ID="rptTopCustomers" runat="server">
            <ItemTemplate>
                <div class="list-row">
                    <span><%# Eval("Username") %></span>
                    <strong>$<%# Eval("TotalSpent","{0:0.00}") %></strong></div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <br />
    <!-- AUDIT TRAIL -->
    <div class="dashboard-panel">
        <h5>Recent Admin Activity</h5>

        <div class="list-row list-header">
            <strong>Admin</strong>
            <strong>Action</strong>
            <strong>Date</strong>
        </div>

        <asp:Repeater ID="rptAudit" runat="server">
            <ItemTemplate>
                <div class="list-row">
                    <span><%# Eval("AdminName") %></span>
                    <span><%# Eval("Action") %></span>
                    <span><%# Eval("DateTime","{0:dd MMM yyyy HH:mm}") %></span>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

   <script>
       function cssVar(name) {
           return getComputedStyle(document.documentElement).getPropertyValue(name).trim();
       }

       const revenueLabels = <%= RevenueLabels %>;
       const revenueData = <%= RevenueData %>;

       const orderLabels = <%= OrderStatusLabels %>;
       const orderData = <%= OrderStatusData %>;

       // For order chart, automatically pick a color based on the status text
       const orderColors = orderLabels.map(status => {
           switch (status.toLowerCase()) {
               case 'shipped': return cssVar('--status-processed');
               case 'packed': return cssVar('--status-processed');
               case 'delivery': return cssVar('--status-approved');
               case 'out for delivery': return cssVar('--status-approved');
               case 'pending': return cssVar('--status-pending');
               case 'rejected': return cssVar('--status-rejected');
               default: return cssVar('--muted');
           }
       });

       // Revenue chart
       new Chart(document.getElementById('revenueChart'), {
           type: 'line',
           data: {
               labels: revenueLabels,
               datasets: [{
                   label: 'Revenue',
                   data: revenueData,
                   fill: true,
                   borderColor: cssVar('--accent'),
                   backgroundColor: 'rgba(79,163,146,0.2)',
                   tension: 0.3
               }]
           },
           options: { responsive: true }
       });

       // Order Status chart
       new Chart(document.getElementById('orderStatusChart'), {
           type: 'bar',
           data: {
               labels: orderLabels,
               datasets: [{
                   label: 'Orders',
                   data: orderData,
                   // Remove fixed colors, just use CSS variable if needed
                   backgroundColor: cssVar('--accent'),
                   borderColor: cssVar('--accent'),
                   borderWidth: 1
               }]
           },
           options: {
               responsive: true,
               plugins: {
                   legend: {
                       position: 'bottom',
                       labels: { font: { weight: '600' } }
                   },
                   tooltip: {}
               },
               scales: {
                   x: {
                       ticks: {},  // remove color
                       grid: {}    // remove color
                   },
                   y: {
                       ticks: {},  // remove color
                       grid: {},   // remove color
                       beginAtZero: true
                   }
               }
           }
       });
   </script>
</asp:Content>
