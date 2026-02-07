<%@ Page Title="" Language="C#" MasterPageFile="~/SuperAdmin/SuperAdmin.Master" AutoEventWireup="true" CodeBehind="ReportsAnalytics.aspx.cs" Inherits="Week1_Practical1.SuperAdmin.ReportsAnalytics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>Reports Analytics</h3>
    <p>Advanced analytics for your application with charts, KPIs, and top performers.</p>
    <br />
    <div class="main-content">

        <h2 class="page-title">Reports & Analytics</h2>

        <!-- YEAR / MONTH FILTERS -->
        <div class="dashboard-panel">
            <asp:Label Text="Year:" runat="server" />
            <asp:DropDownList ID="ddlYear" runat="server"
                CssClass="textbox"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlFilter_Changed" />

            <asp:Label Text="Month:" runat="server" Style="margin-left:15px;" />
            <asp:DropDownList ID="ddlMonth" runat="server"
                CssClass="textbox"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlFilter_Changed" />
        </div>

        <br />
        <!-- KPI CARDS -->
        <div class="dashboard-panel kpi-grid">

            <div class="card">
                <h6>Total Revenue (Year)</h6>
                <asp:Label ID="lblTotalRevenue" runat="server" />
            </div>

            <div class="card">
                <h6>Total Orders</h6>
                <asp:Label ID="lblTotalOrders" runat="server" />
            </div>

            <div class="card">
                <h6>Completed Orders</h6>
                <asp:Label ID="lblCompletedOrders" runat="server" />
            </div>

            <div class="card">
                <h6>Average Order Value</h6>
                <asp:Label ID="lblAOV" runat="server" />
            </div>

        </div>

        <br />
        <asp:HiddenField ID="hfRevenueLabels" runat="server" />
        <asp:HiddenField ID="hfRevenueData" runat="server" />

        <asp:HiddenField ID="hfStatusLabels" runat="server" />
        <asp:HiddenField ID="hfStatusData" runat="server" />
        <br />
        <!-- CHART PLACEHOLDERS -->
        <div class="dashboard-panel">
            <h3>Revenue Trends</h3>
            <canvas id="revenueChart"></canvas>
        </div>
        <br />
        <div class="dashboard-panel">
            <h3>Category Revenue</h3>
            <asp:GridView ID="gvCategoryRevenue"
                runat="server"
                CssClass="gridview-style"
                AutoGenerateColumns="true" />
        </div>
        <br />
        <div class="dashboard-panel">
            <h3>Top Products</h3>
            <asp:GridView ID="gvTopProducts"
                runat="server"
                CssClass="gridview-style"
                AutoGenerateColumns="true" />
        </div>
        <br />
        <div class="dashboard-panel">
            <h3>Order Status Breakdown</h3>
            <canvas id="orderStatusChart"></canvas>
        </div>
        <br />
        <!-- EXPORT -->
        <div class="dashboard-panel">
            <asp:Button ID="btnExportCsv" runat="server"
                Text="Export CSV"
                CssClass="btn-add"
                OnClick="btnExportCsv_Click" />
            <asp:Button ID="btnExportPdf" runat="server"
                Text="Export PDF"
                CssClass="btn-add"
                OnClick="btnExportPdf_Click" />
        </div>

    </div>

    <script>
        // ===== Revenue Chart (Year → Monthly) =====
        const revLabelsEl = document.getElementById('<%= hfRevenueLabels.ClientID %>');
        const revDataEl = document.getElementById('<%= hfRevenueData.ClientID %>');
        const revCanvas = document.getElementById('revenueChart');

        if (revLabelsEl && revDataEl && revCanvas &&
            revLabelsEl.value && revDataEl.value) {

            new Chart(revCanvas, {
                type: 'bar',   // 👈 better for monthly revenue
                data: {
                    labels: revLabelsEl.value.split(','),
                    datasets: [{
                        label: 'Revenue',
                        data: revDataEl.value.split(',')
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: { display: true }
                    }
                }
            });
        }

        // ===== Order Status Pie Chart (KEEP THIS) =====
        const statusLabelsEl = document.getElementById('<%= hfStatusLabels.ClientID %>');
        const statusDataEl = document.getElementById('<%= hfStatusData.ClientID %>');
            const statusCanvas = document.getElementById('orderStatusChart');

            if (statusLabelsEl && statusDataEl && statusCanvas &&
                statusLabelsEl.value && statusDataEl.value) {

                new Chart(statusCanvas, {
                    type: 'pie',
                    data: {
                        labels: statusLabelsEl.value.split(','),
                        datasets: [{
                            data: statusDataEl.value.split(',')
                        }]
                    }
                });
            }
    </script>


</asp:Content>
