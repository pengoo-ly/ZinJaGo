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

        <!-- FILTERS -->
        <div class="dashboard-panel">
            <asp:Label Text="Date Range:" runat="server" />
            <asp:TextBox ID="txtStart" CssClass="textbox" runat="server" TextMode="Date" />
            <asp:TextBox ID="txtEnd" CssClass="textbox" runat="server" TextMode="Date" />
            <asp:Button ID="btnApply" runat="server" CssClass="btn-add"
                Text="Apply Filters" OnClick="btnApply_Click" />
        </div>
        <br />
        <!-- KPI CARDS -->
        <div class="dashboard-panel">
            <div class="card">
                <h6>Average Order Value</h6>
                <asp:Label ID="lblAOV" runat="server" />
            </div>
        </div>
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
        </div>

    </div>
    

</asp:Content>
