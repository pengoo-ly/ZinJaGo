using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Week1_Practical1
{
    public partial class OrderManagement : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrderStatistics();
                LoadOrders();
            }
        }

        private void LoadOrderStatistics()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();

                    // Total Orders
                    SqlCommand cmdTotalOrders = new SqlCommand("SELECT COUNT(*) FROM Orders", conn);
                    int totalOrders = (int)cmdTotalOrders.ExecuteScalar();
                    lblTotalOrders.Text = totalOrders.ToString("N0");

                    // New Orders (last 7 days)
                    SqlCommand cmdNewOrders = new SqlCommand(
                        "SELECT COUNT(*) FROM Orders WHERE OrderDate >= DATEADD(day, -7, CAST(GETDATE() AS DATE))", conn);
                    int newOrders = (int)cmdNewOrders.ExecuteScalar();
                    lblNewOrders.Text = newOrders.ToString("N0");
                    lblAllOrderCount.Text = totalOrders.ToString();

                    // Completed Orders (Paid orders)
                    SqlCommand cmdCompleted = new SqlCommand(
                        "SELECT COUNT(*) FROM Orders WHERE PaymentStatus = 'Paid'", conn);
                    int completedOrders = (int)cmdCompleted.ExecuteScalar();
                    lblCompletedOrders.Text = completedOrders.ToString("N0");

                    // Unpaid Orders
                    SqlCommand cmdCanceled = new SqlCommand(
                        "SELECT COUNT(*) FROM Orders WHERE PaymentStatus = 'Unpaid'", conn);
                    int unpaidOrders = (int)cmdCanceled.ExecuteScalar();
                    lblCanceledOrders.Text = unpaidOrders.ToString("N0");
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading order statistics: " + ex.Message);
            }
        }

        private void LoadOrders()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter(
                        @"SELECT 
                            o.OrderID,
                            p.ProductName,
                            o.OrderDate,
                            oi.UnitPrice AS Price,
                            o.PaymentStatus,
                            CASE 
                                WHEN o.PaymentStatus = 'Paid' THEN 'Delivered'
                                WHEN o.PaymentStatus = 'Unpaid' THEN 'Pending'
                                ELSE 'Pending'
                            END AS OrderStatus,
                            '📦' AS ProductIcon
                        FROM Orders o
                        JOIN OrderItems oi ON o.OrderID = oi.OrderID
                        JOIN Products p ON oi.ProductID = p.ProductID
                        ORDER BY o.OrderDate DESC", conn);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptOrders.DataSource = dt;
                    rptOrders.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowError("Error loading orders: " + ex.Message);
            }
        }

        /// <summary>
        /// Resolves product image based on product name
        /// </summary>
        public string ResolveProductImage(string productName)
        {
            if (string.IsNullOrEmpty(productName))
                return "Images/default.png";

            string product = productName.ToLower().Trim();

            if (product.Contains("vitamin"))
                return "Images/Vitamin.png";
            else if (product.Contains("mouse") || product.Contains("wireless"))
                return "Images/Wireless_Mouse.png";
            else
                return "Images/default.png";
        }

        public string GetStatusIcon(string status)
        {
            switch (status)
            {
                case "Delivered":
                    return "📦";
                case "Pending":
                    return "⏳";
                case "Shipped":
                    return "🚚";
                case "Cancelled":
                    return "❌";
                default:
                    return "●";
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
    }
}