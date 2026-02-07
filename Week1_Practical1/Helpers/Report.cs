using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Week1_Practical1.Helpers
{
    public class Report
    {
        string _connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;
        public DataTable GetRevenueByMonth(int year)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    MONTH(OrderDate) AS Month,
                    SUM(TotalAmount) AS Revenue
                FROM Orders
                WHERE YEAR(OrderDate) = @Year
                GROUP BY MONTH(OrderDate)
                ORDER BY Month";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Year", year);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public decimal GetRevenueForPeriod(DateTime start, DateTime end)
        {
            string query = @"SELECT ISNULL(SUM(TotalAmount),0) FROM Orders 
                             WHERE OrderDate BETWEEN @Start AND @End";

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@End", end);
                conn.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        public DataTable GetRevenueByCategory(DateTime start, DateTime end)
        {
            string sql = @"
                SELECT 
                    c.CategoryName,
                    SUM(oi.Quantity * oi.UnitPrice) AS Revenue
                FROM OrderItems oi
                INNER JOIN Products p ON oi.ProductID = p.ProductID
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                WHERE oi.PurchaseDate BETWEEN @Start AND @End
                GROUP BY c.CategoryName
                ORDER BY Revenue DESC";

            return ExecuteTable(sql,
                new SqlParameter("@Start", start),
                new SqlParameter("@End", end));
        }

        public DataTable GetTopProducts()
        {
            string sql = @"
                SELECT TOP 10
                    p.ProductName,
                    SUM(oi.Quantity) AS QuantitySold,
                    SUM(oi.Quantity * oi.UnitPrice) AS Revenue
                FROM OrderItems oi
                INNER JOIN Products p ON oi.ProductID = p.ProductID
                GROUP BY p.ProductName
                ORDER BY Revenue DESC";

            return ExecuteTable(sql);
        }
        public decimal GetAverageOrderValue()
        {
            string sql = "SELECT AVG(TotalAmount) FROM Orders";
            return ExecuteScalarDecimal(sql);
        }

        public DataTable GetNewVsRepeatCustomers()
        {
            string sql = @"
                SELECT 
                    CASE WHEN COUNT(o.OrderID) = 1 THEN 'New' ELSE 'Repeat' END AS CustomerType,
                    COUNT(*) AS Total
                FROM Orders o
                GROUP BY o.UserID";

            return ExecuteTable(sql);
        }

        public DataTable GetCustomerLifetimeValue()
        {
            string sql = @"
                SELECT 
                    u.Username,
                    SUM(o.TotalAmount) AS LifetimeValue
                FROM Orders o
                INNER JOIN Users u ON o.UserID = u.UserID
                GROUP BY u.Username
                ORDER BY LifetimeValue DESC";

            return ExecuteTable(sql);
        }
        public DataTable GetOrdersOverTime(string period)
        {
            string groupBy =
                period == "Day" ? "CONVERT(date, OrderDate)" :
                period == "Week" ? "DATEPART(week, OrderDate)" :
                "MONTH(OrderDate)";

            string sql = $@"
                SELECT {groupBy} AS Period, COUNT(*) AS TotalOrders
                FROM Orders
                GROUP BY {groupBy}
                ORDER BY Period";

            return ExecuteTable(sql);
        }

        public DataTable GetOrderStatusBreakdown()
        {
            string sql = @"
                SELECT ShippingStatus, COUNT(*) AS Total
                FROM Orders
                GROUP BY ShippingStatus";

            return ExecuteTable(sql);
        }
        public DataTable GetOrdersByDateRange(DateTime start, DateTime end)
        {
            string sql = @"
                SELECT *
                FROM Orders
                WHERE OrderDate BETWEEN @Start AND @End";

            return ExecuteTable(sql,
                new SqlParameter("@Start", start),
                new SqlParameter("@End", end));
        }
        private DataTable ExecuteTable(string sql, params SqlParameter[] param)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (param != null)
                    cmd.Parameters.AddRange(param);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

        private decimal ExecuteScalarDecimal(string sql, params SqlParameter[] param)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                if (param != null)
                    cmd.Parameters.AddRange(param);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
            }
        }

    }
}