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
            try
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
            catch (Exception ex)
            {
                throw new Exception("Failed to get revenue by month.", ex);
            }
        }

        public decimal GetRevenueForPeriod(DateTime start, DateTime end)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Failed to get revenue for the given period.", ex);
            }
        }

        public DataTable GetRevenueByCategory(DateTime start, DateTime end)
        {
            try
            {
                string sql = @"
            SELECT 
                c.CategoryName,
                SUM(oi.Quantity * oi.UnitPrice) AS Revenue
            FROM OrderItems oi
            INNER JOIN Products p ON oi.ProductID = p.ProductID
            INNER JOIN Categories c ON p.CategoryID = c.CategoryID
            INNER JOIN Orders o ON oi.OrderID = o.OrderID
            WHERE o.OrderDate BETWEEN @Start AND @End
            GROUP BY c.CategoryName
            ORDER BY Revenue DESC";

                return ExecuteTable(sql,
                    new SqlParameter("@Start", start),
                    new SqlParameter("@End", end));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get revenue by category.", ex);
            }
        }


        public DataTable GetTopProductsByYear(int year)
        {
            try
            {
                string sql = @"
            SELECT TOP 10
                p.ProductName,
                SUM(oi.Quantity) AS QuantitySold,
                SUM(oi.Quantity * oi.UnitPrice) AS Revenue
            FROM OrderItems oi
            INNER JOIN Products p ON oi.ProductID = p.ProductID
            INNER JOIN Orders o ON oi.OrderID = o.OrderID
            WHERE YEAR(o.OrderDate) = @Year
            GROUP BY p.ProductName
            ORDER BY Revenue DESC";

                return ExecuteTable(sql, new SqlParameter("@Year", year));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get top products by year.", ex);
            }
        }

        public decimal GetAverageOrderValueByYear(int year)
        {
            try
            {
                string sql = @"SELECT AVG(TotalAmount)
                       FROM Orders
                       WHERE YEAR(OrderDate) = @Year";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Year", year);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();

                    return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get average order value by year.", ex);
            }
        }

        public DataTable GetNewVsRepeatCustomers()
        {
            try
            {
                string sql = @"
                SELECT 
                    CASE WHEN COUNT(o.OrderID) = 1 THEN 'New' ELSE 'Repeat' END AS CustomerType,
                    COUNT(*) AS Total
                FROM Orders o
                GROUP BY o.UserID";

                return ExecuteTable(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get new vs repeat customers.", ex);
            }
        }

        public DataTable GetCustomerLifetimeValue()
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Failed to get customer lifetime value.", ex);
            }
        }
        public DataTable GetOrdersOverTime(string period)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Failed to get orders over time.", ex);
            }
        }

        public DataTable GetOrderStatusBreakdownByYear(int year)
        {
            try
            {
                string sql = @"
                SELECT ShippingStatus, COUNT(*) AS Total
                FROM Orders
                WHERE YEAR(OrderDate) = @Year
                GROUP BY ShippingStatus";

                return ExecuteTable(sql, new SqlParameter("@Year", year));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get order status breakdown by year.", ex);
            }
        }

        public DataTable GetOrdersByDateRange(DateTime start, DateTime end)
        {
            try
            {
                string sql = @"
                SELECT *
                FROM Orders
                WHERE OrderDate BETWEEN @Start AND @End";

                return ExecuteTable(sql,
                    new SqlParameter("@Start", start),
                    new SqlParameter("@End", end));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get orders by date range.", ex);
            }
        }
        private DataTable ExecuteTable(string sql, params SqlParameter[] param)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Failed to execute SQL query.", ex);
            }
        }

        public DataTable GetRevenueByAdmin(int adminId)
        {
            try
            {
                string sql = @"
                SELECT 
                    a.AdminName,
                    SUM(oi.Quantity * oi.UnitPrice) AS Revenue
                FROM Products p
                INNER JOIN OrderItems oi ON p.ProductID = oi.ProductID
                INNER JOIN Admins a ON p.AdminID = a.AdminID
                WHERE a.AdminID = @AdminID
                GROUP BY a.AdminName";

                return ExecuteTable(sql, new SqlParameter("@AdminID", adminId));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get revenue by admin.", ex);
            }
        }
        public DataTable CompareAdminsRevenue(DateTime start, DateTime end)
        {
            try
            {
                string sql = @"
                SELECT 
                    a.AdminName,
                    SUM(oi.Quantity * oi.UnitPrice) AS Revenue
                FROM OrderItems oi
                INNER JOIN Products p ON oi.ProductID = p.ProductID
                INNER JOIN Admins a ON p.AdminID = a.AdminID
                WHERE oi.PurchaseDate BETWEEN @Start AND @End
                GROUP BY a.AdminName
                ORDER BY Revenue DESC";

                return ExecuteTable(sql,
                    new SqlParameter("@Start", start),
                    new SqlParameter("@End", end));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to compare admins revenue.", ex);
            }
        }
        public decimal GetTotalRevenueByYear(int year)
        {
            try
            {
                string sql = @"SELECT ISNULL(SUM(TotalAmount),0)
                           FROM Orders
                           WHERE YEAR(OrderDate) = @Year";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Year", year);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get total revenue by year.", ex);
            }
        }


        public int GetTotalOrdersByYear(int year)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM Orders
                           WHERE YEAR(OrderDate) = @Year";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Year", year);
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get total orders by year.", ex);
            }
        }

        public int GetCompletedOrdersByYear(int year)
        {
            try
            {
                string sql = @"SELECT COUNT(*) FROM Orders
                           WHERE YEAR(OrderDate) = @Year
                           AND ShippingStatus = 'Completed'";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Year", year);
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get completed orders by year.", ex);
            }
        }
        public DataTable GetRevenueByMonthFromItems(int year)
        {
            try
            {
                string sql = @"
                SELECT MONTH(o.OrderDate) AS Month,
                       SUM(oi.Quantity * oi.UnitPrice) AS Revenue
                FROM Orders o
                INNER JOIN OrderItems oi ON o.OrderID = oi.OrderID
                WHERE YEAR(o.OrderDate) = @Year
                GROUP BY MONTH(o.OrderDate)
                ORDER BY Month";

                return ExecuteTable(sql, new SqlParameter("@Year", year));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get revenue by month from items.", ex);
            }
        }

        public DataTable GetNewVsRepeatCustomersByPeriod(DateTime start, DateTime end)
        {
            try
            {
                string sql = @"
                SELECT 
                    CASE WHEN COUNT(o.OrderID) = 1 THEN 'New' ELSE 'Repeat' END AS CustomerType,
                    COUNT(*) AS Total
                FROM Orders o
                WHERE o.OrderDate BETWEEN @Start AND @End
                GROUP BY o.UserID";

                return ExecuteTable(sql,
                    new SqlParameter("@Start", start),
                    new SqlParameter("@End", end));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get new vs repeat customers by period.", ex);
            }
        }

    }
}