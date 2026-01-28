using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class OrderManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["AdminID"] == null)
                    {
                        Response.Redirect("Login.aspx");
                        return;
                    }

                    int adminId = Convert.ToInt32(Session["AdminID"]);

                    LoadStatistics(adminId);
                    LoadOrders(adminId);
                }
            }
            catch(Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void LoadStatistics(int adminId)
        {
            try
            {
                DataTable dt = Order.GetOrderStatistics(adminId);

                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    lblTotalOrders.Text = r["TotalOrders"].ToString();
                    lblNewOrders.Text = r["NewOrders"].ToString();
                    lblCompletedOrders.Text = r["CompletedOrders"].ToString();
                    lblCanceledOrders.Text = r["UnpaidOrders"].ToString();
                    lblAllOrderCount.Text = r["TotalOrders"].ToString();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void LoadOrders(int adminId)
        {
            try
            {
                rptOrders.DataSource = Order.GetOrdersByAdmin(adminId);
                rptOrders.DataBind();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Resolves product image based on product name
        /// </summary>

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