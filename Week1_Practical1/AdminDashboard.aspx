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
        <!-- Revenue Chart -->
        <div class="col-md-8">
            <div class="dashboard-panel">
                <h5>Revenue Overview</h5>
                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" CssClass="form-select">
                </asp:DropDownList>
                <br />
                <asp:Chart ID="RevenueChart" runat="server" Width="670px" BorderlineColor="Transparent" Height="329px">
                    <Series>
                        <asp:Series Name="Revenue" ChartType="Line" />
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="MainArea" BackColor="Transparent" BackGradientStyle="TopBottom" BorderColor="Transparent" >
                            <AxisY IsLabelAutoFit="False" Title="Revenue" TitleFont="Segoe UI, 10.2pt, style=Bold" LineColor="DarkGray">
                                <MajorGrid LineColor="DarkGray" />
                                <LabelStyle Font="Segoe UI, 7.8pt" ForeColor="DarkSlateGray" />
                            </AxisY>
                            <AxisX Title="Month" TitleFont="Segoe UI, 10.2pt, style=Bold" InterlacedColor="DarkGray" LineColor="DarkGray">
                                <MajorGrid LineColor="DarkGray" />
                                <MajorTickMark LineColor="DarkGray" />
                                <LabelStyle ForeColor="DarkSlateGray" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </div>
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
</asp:Content>
