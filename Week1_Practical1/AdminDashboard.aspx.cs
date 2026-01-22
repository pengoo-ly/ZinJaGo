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
                    LoadLogistics();
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
            try
            {
                ddlYear.Items.Clear();

                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT DISTINCT YEAR(OrderDate) AS OrderYear
                          FROM Orders
                          WHERE PaymentStatus = 'Paid'
                          ORDER BY OrderYear DESC", con);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        ddlYear.Items.Add(dr["OrderYear"].ToString());
                    }
                }
            }
            catch
            {
                ShowError("Unable to load years for revenue chart.");
            }
        }

        void LoadRevenueChart()
        {
            try
            {
                int adminId = Convert.ToInt32(Session["AdminID"]);
                int year = ddlYear.SelectedValue != ""
                    ? Convert.ToInt32(ddlYear.SelectedValue)
                    : DateTime.Now.Year;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT DATENAME(MONTH, O.OrderDate) AS MonthName,
                         MONTH(O.OrderDate) AS MonthNo,
                         SUM(OI.Quantity * OI.UnitPrice) AS Revenue
                          FROM Orders O
                          JOIN OrderItems OI ON O.OrderID = OI.OrderID
                          JOIN Products P ON OI.ProductID = P.ProductID
                          WHERE O.PaymentStatus = 'Paid'
                            AND YEAR(O.OrderDate) = @Year
                            AND P.AdminID = @AdminID
                          GROUP BY DATENAME(MONTH, O.OrderDate), MONTH(O.OrderDate)
                          ORDER BY MonthNo", con);

                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@AdminID", adminId);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    List<string> labels = new List<string>();
                    List<decimal> values = new List<decimal>();

                    while (dr.Read())
                    {
                        labels.Add(dr["MonthName"].ToString());
                        values.Add(Convert.ToDecimal(dr["Revenue"]));
                    }

                    RevenueLabels = "[" + string.Join(",", labels.Select(m => $"'{m}'")) + "]";
                    RevenueData = "[" + string.Join(",", values) + "]";
                }
            }
            catch
            {
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
                    // Total Users
                    SqlCommand cmdUsers = new SqlCommand(
                        @"SELECT COUNT(DISTINCT O.UserID)
                            FROM Orders O
                            JOIN OrderItems OI ON O.OrderID = OI.OrderID
                            JOIN Products P ON OI.ProductID = P.ProductID
                            WHERE P.AdminID = @AdminID", conn);
                    cmdUsers.Parameters.AddWithValue("@AdminID", adminId);
                    lblUsers.Text = cmdUsers.ExecuteScalar().ToString();

                    // Total Orders
                    SqlCommand cmdOrders = new SqlCommand(
                        @"SELECT COUNT(DISTINCT O.OrderID)
                            FROM Orders O
                            JOIN OrderItems OI ON O.OrderID = OI.OrderID
                            JOIN Products P ON OI.ProductID = P.ProductID
                            WHERE P.AdminID=@AdminID", conn);
                    cmdOrders.Parameters.AddWithValue("@AdminID", adminId);
                    lblOrders.Text = cmdOrders.ExecuteScalar().ToString();

                    // Total Products
                    SqlCommand cmdProducts = new SqlCommand(
                        "SELECT COUNT(*) FROM Products WHERE AdminID=@AdminID", conn);
                    cmdProducts.Parameters.AddWithValue("@AdminID", adminId);
                    lblProducts.Text = cmdProducts.ExecuteScalar().ToString();

                    // Total Revenue
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
        void LoadLogistics()
        {
            try
            {
                int adminId = Convert.ToInt32(Session["AdminID"]);
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter(@"
                SELECT OS.ShipmentID, OS.Status, OS.ShippedDate
                FROM OrderShipments OS
                JOIN OrderItems OI ON OS.OrderID = OI.OrderID
                JOIN Products P ON OI.ProductID = P.ProductID
                WHERE P.AdminID=@AdminID
                ORDER BY OS.ShippedDate DESC", con);
                    da.SelectCommand.Parameters.AddWithValue("@AdminID", adminId);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptLogistics.DataSource = dt;
                    rptLogistics.DataBind();
                }
            }
            catch
            {
                ShowError("Unable to load logistics.");
            }
        }

    }
}