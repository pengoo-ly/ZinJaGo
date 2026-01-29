using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class OrderShipmentAdmin : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Check if admin is logged in
                if (Session["IsAdminLoggedIn"] == null || !(bool)Session["IsAdminLoggedIn"])
                {
                    Response.Redirect("~/Login.aspx");
                }

                if (!IsPostBack)
                {
                    LoadOrderData();
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error on OrderShipmentAdmin page load: " + ex.Message);
                ShowError("An error occurred while loading the page.");
            }
        }

        private void LoadOrderData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    // FIXED: Removed AdminID references and non-existent columns
                    // Using PaymentStatus and ShippingStatus instead of Status
                    string query = @"
                        SELECT 
                            o.OrderID,
                            o.OrderDate,
                            o.TotalAmount,
                            o.PaymentStatus,
                            o.ShippingStatus,
                            u.UserID,
                            u.Email,
                            os.ShipmentID,
                            os.ShippingMethodID,
                            os.TrackingNumber,
                            os.ShippedDate,
                            os.EstimatedDelivery,
                            os.DeliveredDate,
                            os.Carrier,
                            ISNULL((SELECT COUNT(*) FROM OrderItems WHERE OrderID = o.OrderID), 0) as ItemCount
                        FROM Orders o
                        JOIN Users u ON o.UserID = u.UserID
                        LEFT JOIN OrderShipments os ON o.OrderID = os.OrderID
                        ORDER BY o.OrderDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            gvOrders.DataSource = dt;
                            gvOrders.DataBind();

                            // Calculate statistics
                            CalculateStatistics();

                            errorMessage.Attributes["class"] = "error-message";
                        }
                        else
                        {
                            gvOrders.DataSource = null;
                            gvOrders.DataBind();
                            ShowError("No orders found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error loading order data: " + ex.Message);
                ShowError("Error loading orders: " + ex.Message);
            }
        }

        private void CalculateStatistics()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    // FIXED: Using ShippingStatus instead of Status
                    string query = @"
                        SELECT 
                            COUNT(*) as TotalOrders,
                            SUM(CASE WHEN ShippingStatus = 'Pending' THEN 1 ELSE 0 END) as PendingOrders,
                            SUM(CASE WHEN ShippingStatus = 'Processing' THEN 1 ELSE 0 END) as ProcessingOrders,
                            SUM(CASE WHEN ShippingStatus = 'Shipped' THEN 1 ELSE 0 END) as ShippedOrders,
                            SUM(CASE WHEN ShippingStatus = 'Delivered' THEN 1 ELSE 0 END) as DeliveredOrders,
                            SUM(TotalAmount) as TotalRevenue
                        FROM Orders";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int totalOrders = reader["TotalOrders"] != DBNull.Value ? Convert.ToInt32(reader["TotalOrders"]) : 0;
                                int pendingOrders = reader["PendingOrders"] != DBNull.Value ? Convert.ToInt32(reader["PendingOrders"]) : 0;
                                int shippedOrders = reader["ShippedOrders"] != DBNull.Value ? Convert.ToInt32(reader["ShippedOrders"]) : 0;
                                int deliveredOrders = reader["DeliveredOrders"] != DBNull.Value ? Convert.ToInt32(reader["DeliveredOrders"]) : 0;
                                decimal totalRevenue = reader["TotalRevenue"] != DBNull.Value ? Convert.ToDecimal(reader["TotalRevenue"]) : 0;

                                lblTotalOrders.Text = totalOrders.ToString();
                                lblPendingOrders.Text = pendingOrders.ToString();
                                lblShippedOrders.Text = shippedOrders.ToString();
                                lblDeliveredOrders.Text = deliveredOrders.ToString();
                                lblTotalRevenue.Text = "₱" + totalRevenue.ToString("N2");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error calculating statistics: " + ex.Message);
            }
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetails")
                {
                    int orderId = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("OrderDetails.aspx?OrderID=" + orderId);
                }
                else if (e.CommandName == "UpdateShipment")
                {
                    int orderId = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("UpdateShipment.aspx?OrderID=" + orderId);
                }
                else if (e.CommandName == "CancelOrder")
                {
                    int orderId = Convert.ToInt32(e.CommandArgument);
                    if (CancelOrder(orderId))
                    {
                        ShowSuccess("Order cancelled successfully!");
                        LoadOrderData();
                    }
                    else
                    {
                        ShowError("Unable to cancel this order. It may have already been shipped.");
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error in grid view command: " + ex.Message);
                ShowError("An error occurred while processing your request.");
            }
        }

        private bool CancelOrder(int orderId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    // FIXED: Using ShippingStatus instead of Status
                    string query = "UPDATE Orders SET ShippingStatus = 'Cancelled' WHERE OrderID = @OrderID AND ShippingStatus IN ('Pending', 'Processing')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", orderId);
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            DbLogger.Log("Order " + orderId + " cancelled successfully");
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error cancelling order: " + ex.Message);
            }
            return false;
        }

        protected void gvOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrders.PageIndex = e.NewPageIndex;
            LoadOrderData();
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            errorMessage.Attributes["class"] = "error-message show";
            successMessage.Attributes["class"] = "success-message";
        }

        private void ShowSuccess(string message)
        {
            lblSuccess.Text = message;
            successMessage.Attributes["class"] = "success-message show";
            errorMessage.Attributes["class"] = "error-message";
        }
    }
}