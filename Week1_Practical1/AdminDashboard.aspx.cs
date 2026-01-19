using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Week1_Practical1
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;
        public string RevenueLabels = "[]";
        public string RevenueData = "[]";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadUserData();
                    LoadYears();
                    LoadRevenueChart();
                    LoadTopProducts();
                    LoadTopCategories();
                }
            }
            catch
            {
                ShowError("An error occurred while loading the dashboard.");
            }

        }
        protected void ShowError(string message)
        {
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "popupError",
                $"alert('{message.Replace("'", "\\'")}');",
                true
            );
        }

        private void LoadYears()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try {
                    SqlCommand cmd = new SqlCommand("SELECT DISTINCT YEAR(OrderDate) AS OrderYear FROM Orders ORDER BY OrderYear DESC", con);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    ddlYear.Items.Clear();
                    while (dr.Read())
                    {
                        ddlYear.Items.Add(dr["OrderYear"].ToString());
                    }
                }
                catch {
                    ShowError("Unable to load years for revenue chart.");
                }
            }
        }
        void LoadRevenueChart()
        {

            try {
                RevenueChart.Series["Revenue"].Points.Clear();
                int year = ddlYear.SelectedValue != "" ? Convert.ToInt32(ddlYear.SelectedValue) : DateTime.Now.Year;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT FORMAT(OrderDate,'MMM') AS Month,
                             SUM(TotalAmount) AS Revenue
                      FROM Orders
                      WHERE PaymentStatus='Paid'
                      GROUP BY FORMAT(OrderDate,'MMM'), MONTH(OrderDate)
                      ORDER BY MONTH(OrderDate)", con);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    StringBuilder labels = new StringBuilder("[");
                    StringBuilder data = new StringBuilder("[");

                    while (dr.Read())
                    {
                        string month = dr["Month"].ToString();
                        decimal revenue = Convert.ToDecimal(dr["Revenue"]);

                        // Add to Chart control
                        RevenueChart.Series["Revenue"].Points.AddXY(month, revenue);
                        RevenueChart.Series["Revenue"].Points[RevenueChart.Series["Revenue"].Points.Count - 1].ToolTip =
                   $"{month}: {revenue:0.00} SGD";
                        // Add for JS charts (optional)
                        labels.Append($"'{month}',");
                        data.Append($"{revenue},");
                    }

                    RevenueLabels = labels.Append("]").ToString();
                    RevenueData = data.Append("]").ToString();
                }
            }
            catch {
                ShowError("Unable to load revenue chart.");
            }
        }

        void LoadTopProducts()
        {
            try
            {
                int adminId = Convert.ToInt32(Session["AdminID"]);
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter(
                        @"SELECT TOP 5 P.ProductName,
                         SUM(OI.Quantity) AS TotalSold
                          FROM OrderItems OI
                          JOIN Products P ON OI.ProductID = P.ProductID
                          WHERE P.AdminID=@AdminID
                          GROUP BY P.ProductName
                          ORDER BY TotalSold DESC", con);
                    da.SelectCommand.Parameters.AddWithValue("@AdminID", adminId);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptTopProducts.DataSource = dt;
                    rptTopProducts.DataBind();
                }
            }
            catch
            {
                ShowError("Unable to load top products.");
            }
        }


        void LoadTopCategories()
        {
            try
            {
                int adminId = Convert.ToInt32(Session["AdminID"]);
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter(
                        @"SELECT TOP 5 C.CategoryName,
                         SUM(OI.Quantity * OI.UnitPrice) AS TotalSales
                          FROM OrderItems OI
                          JOIN Products P ON OI.ProductID = P.ProductID
                          JOIN Categories C ON P.CategoryID = C.CategoryID
                          WHERE P.AdminID=@AdminID
                          GROUP BY C.CategoryName
                          ORDER BY TotalSales DESC", con);
                    da.SelectCommand.Parameters.AddWithValue("@AdminID", adminId);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptTopCategories.DataSource = dt;
                    rptTopCategories.DataBind();
                }
            }
            catch
            {
                ShowError("Unable to load top categories.");
            }
        }
        protected void LoadUserData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();
                    int adminId = Convert.ToInt32(Session["AdminID"]);

                    // Total Users (leave global if you want)
                    SqlCommand cmdUsers = new SqlCommand("SELECT COUNT(*) FROM Users", conn);
                    lblUsers.Text = cmdUsers.ExecuteScalar().ToString();

                    // Total Orders for this admin
                    SqlCommand cmdOrders = new SqlCommand(
                        @"SELECT COUNT(*) 
                          FROM Orders O
                          JOIN OrderItems OI ON O.OrderID = OI.OrderID
                          JOIN Products P ON OI.ProductID = P.ProductID
                          WHERE P.AdminID=@AdminID", conn);
                    cmdOrders.Parameters.AddWithValue("@AdminID", adminId);
                    lblOrders.Text = cmdOrders.ExecuteScalar().ToString();

                    // Total Products for this admin
                    SqlCommand cmdProducts = new SqlCommand(
                        "SELECT COUNT(*) FROM Products WHERE AdminID=@AdminID", conn);
                    cmdProducts.Parameters.AddWithValue("@AdminID", adminId);
                    lblProducts.Text = cmdProducts.ExecuteScalar().ToString();

                    // Total Revenue for this admin
                    SqlCommand cmdRevenue = new SqlCommand(
                        @"SELECT ISNULL(SUM(OI.Quantity * OI.UnitPrice), 0)
                          FROM Orders O
                          JOIN OrderItems OI ON O.OrderID = OI.OrderID
                          JOIN Products P ON OI.ProductID = P.ProductID
                          WHERE P.AdminID=@AdminID AND O.PaymentStatus='Paid'", conn);
                            cmdRevenue.Parameters.AddWithValue("@AdminID", adminId);
                    lblRevenue.Text = Convert.ToDecimal(cmdRevenue.ExecuteScalar()).ToString("0.00");
                }
            }
            catch
            {
                ShowError("Failed to load dashboard data.");
            }

        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadRevenueChart();
            }
            catch
            {
                ShowError("Unable to update revenue chart for selected year.");
            }
        }
    }
}