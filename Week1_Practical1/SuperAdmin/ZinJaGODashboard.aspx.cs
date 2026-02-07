using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Week1_Practical1.Helpers;

namespace Week1_Practical1.SuperAdmin
{
    public partial class ZinJaGODashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        public string RevenueLabels = "[]";
        public string RevenueData = "[]";
        public string OrderStatusLabels = "[]";
        public string OrderStatusData = "[]";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsSuperAdmin()) return;
                if (!IsPostBack)
                {
                    BindYearDropdowns();
                    LoadStats();
                    LoadRevenueChart();
                    LoadOrderStatusChart();
                    LoadTopAdmins();
                    LoadAuditTrail();
                }
            }
            catch (Exception ex)
            {
                ShowError("Error in Page_Load of ZinJaGODashboard: " + ex.Message);
                Response.Redirect("~/Login.aspx");
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

        bool IsSuperAdmin()
        {
            if (Session["AdminRole"]?.ToString() != "SuperAdmin")
            {
                Response.Redirect("~/Login.aspx");
                return false;
            }
            return true;
        }

        void LoadStats()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                lblUsers.Text = ExecuteScalar(con, "SELECT COUNT(*) FROM Users");
                lblAdmins.Text = ExecuteScalar(con, "SELECT COUNT(*) FROM Admins");
                lblOrders.Text = ExecuteScalar(con, "SELECT COUNT(*) FROM Orders");

                lblRevenue.Text = ExecuteScalar(con,
                    @"SELECT ISNULL(SUM(OI.Quantity * OI.UnitPrice),0)
                      FROM Orders O
                      JOIN OrderItems OI ON O.OrderID = OI.OrderID
                      WHERE O.PaymentStatus='Paid'");
            }
        }

        void BindYearDropdowns()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT DISTINCT YEAR(OrderDate) Y FROM Orders ORDER BY Y DESC", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlRevenueYear.Items.Clear();
                ddlOrderYear.Items.Clear();

                while (dr.Read())
                {
                    string year = dr["Y"].ToString();
                    ddlRevenueYear.Items.Add(year);
                    ddlOrderYear.Items.Add(year);
                }
            }

            string currentYear = DateTime.Now.Year.ToString();
            ddlRevenueYear.SelectedValue = currentYear;
            ddlOrderYear.SelectedValue = currentYear;
        }


        void LoadRevenueChart()
        {
            int year = int.Parse(ddlRevenueYear.SelectedValue);

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT DATENAME(MONTH, O.OrderDate) M,
                       MONTH(O.OrderDate) MN,
                       SUM(OI.Quantity * OI.UnitPrice) Revenue
                FROM Orders O
                JOIN OrderItems OI ON O.OrderID = OI.OrderID
                WHERE O.PaymentStatus = 'Paid'
                  AND YEAR(O.OrderDate) = @Year
                GROUP BY DATENAME(MONTH, O.OrderDate), MONTH(O.OrderDate)
                ORDER BY MN", con))
            {
                cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                List<string> labels = new List<string>();
                List<decimal> data = new List<decimal>();

                while (dr.Read())
                {
                    labels.Add(dr["M"].ToString());
                    data.Add(Convert.ToDecimal(dr["Revenue"]));
                }

                RevenueLabels = "[" + string.Join(",", labels.Select(x => "'" + x + "'")) + "]";
                RevenueData = "[" + string.Join(",", data) + "]";
            }
        }



        void LoadOrderStatusChart()
        {
            int year = int.Parse(ddlOrderYear.SelectedValue);

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlDataAdapter da = new SqlDataAdapter(@"
                SELECT ShippingStatus, COUNT(*) Total
                FROM Orders
                WHERE YEAR(OrderDate) = @Year
                GROUP BY ShippingStatus", con))
            {
                da.SelectCommand.Parameters.Add("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();
                da.Fill(dt);

                OrderStatusLabels = "[" +
                    string.Join(",", dt.Rows.Cast<DataRow>()
                    .Select(r => "'" + r["ShippingStatus"].ToString() + "'")) + "]";

                OrderStatusData = "[" +
                    string.Join(",", dt.Rows.Cast<DataRow>()
                    .Select(r => r["Total"].ToString())) + "]";
            }
        }
        void LoadTopAdmins()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
            SELECT TOP 5 A.AdminName,
                   SUM(OI.Quantity * OI.UnitPrice) Revenue
            FROM Products P
            JOIN Admins A ON P.AdminID = A.AdminID
            JOIN OrderItems OI ON P.ProductID = OI.ProductID
            JOIN Orders O ON OI.OrderID = O.OrderID
            WHERE O.PaymentStatus='Paid'
            GROUP BY A.AdminName
            ORDER BY Revenue DESC", con);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptTopAdmins.DataSource = dt;
                rptTopAdmins.DataBind();
            }
        }


        void LoadAuditTrail()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
            SELECT TOP 10 A.AdminName, AT.Action, AT.DateTime
            FROM AuditTrail AT
            JOIN Admins A ON AT.AdminID = A.AdminID
            ORDER BY AT.DateTime DESC", con);

                DataTable dt = new DataTable();
                da.Fill(dt);
                rptAudit.DataSource = dt;
                rptAudit.DataBind();
            }
        }
        string ExecuteScalar(SqlConnection con, string q)
        {
            return Convert.ToString(new SqlCommand(q, con).ExecuteScalar());
        }

        protected void ddlYear_Changed(object sender, EventArgs e)
        {
            LoadRevenueChart();
            LoadOrderStatusChart();
        }
    }
}