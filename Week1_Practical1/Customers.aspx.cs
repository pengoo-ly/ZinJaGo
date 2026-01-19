using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Week1_Practical1
{
    public partial class Customers : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomerStatistics();
                LoadCustomers();
                LoadChartData();
            }
        }

        private void LoadCustomerStatistics()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();

                    // Total Customers (users who have placed orders)
                    SqlCommand cmdTotal = new SqlCommand(
                        "SELECT COUNT(DISTINCT UserID) FROM Orders", conn);
                    int totalCustomers = (int)cmdTotal.ExecuteScalar();
                    lblTotalCustomers.Text = totalCustomers.ToString("N0");

                    // New Customers (last 7 days)
                    SqlCommand cmdNew = new SqlCommand(
                        @"SELECT COUNT(DISTINCT o.UserID) 
                          FROM Orders o
                          WHERE o.OrderDate >= DATEADD(day, -7, CAST(GETDATE() AS DATE))
                          AND o.UserID NOT IN (
                              SELECT DISTINCT UserID FROM Orders 
                              WHERE OrderDate < DATEADD(day, -7, CAST(GETDATE() AS DATE))
                          )", conn);
                    int newCustomers = (int)cmdNew.ExecuteScalar();
                    lblNewCustomers.Text = newCustomers.ToString("N0");

                    // Calculate percentage change for new customers
                    SqlCommand cmdPrevNew = new SqlCommand(
                        @"SELECT COUNT(DISTINCT o.UserID) 
                          FROM Orders o
                          WHERE o.OrderDate >= DATEADD(day, -14, CAST(GETDATE() AS DATE))
                          AND o.OrderDate < DATEADD(day, -7, CAST(GETDATE() AS DATE))
                          AND o.UserID NOT IN (
                              SELECT DISTINCT UserID FROM Orders 
                              WHERE OrderDate < DATEADD(day, -14, CAST(GETDATE() AS DATE))
                          )", conn);
                    int prevNewCustomers = (int)cmdPrevNew.ExecuteScalar();
                    double newChange = prevNewCustomers > 0 ? ((double)(newCustomers - prevNewCustomers) / prevNewCustomers) * 100 : 0;
                    lblNewChange.Text = (newChange >= 0 ? "↑ " : "↓ ") + Math.Abs(newChange).ToString("0.0") + "%";

                    // Active Customers (users with Status = 'Active')
                    SqlCommand cmdActive = new SqlCommand(
                        @"SELECT COUNT(DISTINCT o.UserID) 
                          FROM Orders o
                          JOIN Users u ON o.UserID = u.UserID
                          WHERE u.Status = 'Active'", conn);
                    int activeCustomers = (int)cmdActive.ExecuteScalar();
                    lblActiveCustomers.Text = (activeCustomers / 1000.0).ToString("0.0") + "k";

                    // Inactive Customers
                    SqlCommand cmdInactive = new SqlCommand(
                        @"SELECT COUNT(DISTINCT o.UserID) 
                          FROM Orders o
                          JOIN Users u ON o.UserID = u.UserID
                          WHERE u.Status = 'Inactive'", conn);
                    int inactiveCustomers = (int)cmdInactive.ExecuteScalar();
                    lblInactiveCustomers.Text = (inactiveCustomers / 1000.0).ToString("0.0") + "k";

                    // Visitor count (unique users who placed orders in last 7 days)
                    SqlCommand cmdVisitors = new SqlCommand(
                        @"SELECT COUNT(DISTINCT UserID) FROM Orders 
                          WHERE OrderDate >= DATEADD(day, -7, CAST(GETDATE() AS DATE))", conn);
                    int visitors = (int)cmdVisitors.ExecuteScalar();
                    lblVisitors.Text = (visitors / 1000.0).ToString("0.0") + "k";

                    // Calculate visitor change
                    SqlCommand cmdPrevVisitors = new SqlCommand(
                        @"SELECT COUNT(DISTINCT UserID) FROM Orders 
                          WHERE OrderDate >= DATEADD(day, -14, CAST(GETDATE() AS DATE))
                          AND OrderDate < DATEADD(day, -7, CAST(GETDATE() AS DATE))", conn);
                    int prevVisitors = (int)cmdPrevVisitors.ExecuteScalar();
                    double visitorChange = prevVisitors > 0 ? ((double)(visitors - prevVisitors) / prevVisitors) * 100 : 0;
                    lblVisitorChange.Text = (visitorChange >= 0 ? "↑ " : "↓ ") + Math.Abs(visitorChange).ToString("0.0") + "%";

                    // Total Spend (last 7 days)
                    SqlCommand cmdSpend = new SqlCommand(
                        @"SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders 
                          WHERE OrderDate >= DATEADD(day, -7, CAST(GETDATE() AS DATE))", conn);
                    decimal totalSpend = (decimal)cmdSpend.ExecuteScalar();
                    lblTotalSpend.Text = (totalSpend / 1000).ToString("0.0") + "k";

                    // Conversion Rate
                    decimal conversionRate = totalCustomers > 0 ? (decimal)activeCustomers / totalCustomers * 100 : 0;
                    lblConversionRate.Text = conversionRate.ToString("0.0");

                    // Total change percentage
                    SqlCommand cmdPrevTotal = new SqlCommand(
                        "SELECT COUNT(DISTINCT UserID) FROM Orders WHERE OrderDate < DATEADD(day, -7, CAST(GETDATE() AS DATE))", conn);
                    int prevTotalCustomers = (int)cmdPrevTotal.ExecuteScalar();
                    double totalChange = prevTotalCustomers > 0 ? ((double)(totalCustomers - prevTotalCustomers) / prevTotalCustomers) * 100 : 0;
                    lblTotalChange.Text = (totalChange >= 0 ? "↑ " : "↓ ") + Math.Abs(totalChange).ToString("0.0") + "%";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading statistics: " + ex.Message);
            }
        }

        private void LoadCustomers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter(
                        @"SELECT 
                            u.UserID,
                            u.Username,
                            u.Email,
                            ISNULL(CONVERT(VARCHAR(20), u.CardNumber), 'N/A') AS Phone,
                            (SELECT COUNT(*) FROM Orders WHERE UserID = u.UserID) AS OrderCount,
                            (SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders WHERE UserID = u.UserID) AS TotalSpent,
                            ISNULL(u.Status, 'Active') AS Status
                        FROM Users u
                        WHERE EXISTS (SELECT 1 FROM Orders WHERE Orders.UserID = u.UserID)
                        ORDER BY (SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders WHERE UserID = u.UserID) DESC", conn);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptCustomers.DataSource = dt;
                    rptCustomers.DataBind();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading customers: " + ex.Message);
            }
        }

        private void LoadChartData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();

                    // Get daily customer data for the last 7 days
                    SqlCommand cmd = new SqlCommand(
                        @"
                        DECLARE @Today DATE = CAST(GETDATE() AS DATE);
                        
                        WITH DateRange AS (
                            SELECT DATEADD(day, -6, @Today) AS ChartDate
                            UNION ALL
                            SELECT DATEADD(day, 1, ChartDate) FROM DateRange WHERE DATEADD(day, 1, ChartDate) <= @Today
                        )
                        SELECT 
                            FORMAT(DateRange.ChartDate, 'ddd') AS DayName,
                            COUNT(DISTINCT o.UserID) AS CustomerCount
                        FROM DateRange
                        LEFT JOIN Orders o ON CAST(o.OrderDate AS DATE) = DateRange.ChartDate
                        GROUP BY DateRange.ChartDate
                        ORDER BY DateRange.ChartDate;
                        ", conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    var chartDataPoints = new List<string>();
                    var customerCounts = new List<int>();

                    while (reader.Read())
                    {
                        chartDataPoints.Add(reader["DayName"].ToString());
                        customerCounts.Add(Convert.ToInt32(reader["CustomerCount"]));
                    }

                    reader.Close();

                    // Pass data to client-side script
                    string chartDataJson = "[" + string.Join(",", customerCounts) + "]";
                    string labelDataJson = "[\"" + string.Join("\",\"", chartDataPoints) + "\"]";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "chartData",
                        $@"
                        var chartLabels = {labelDataJson};
                        var chartData = {chartDataJson};
                        renderChart(chartLabels, chartData);
                        ", true);

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading chart data: " + ex.Message);
            }
        }
    }
}