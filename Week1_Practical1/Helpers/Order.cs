using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Week1_Practical1.Helpers
{
    public class Order
    {
        private static string cs= ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        public static DataTable GetOrderStatistics(int adminId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT
                        COUNT(DISTINCT o.OrderID) AS TotalOrders,
                        SUM(CASE WHEN o.OrderDate >= DATEADD(DAY,-7,GETDATE()) THEN 1 ELSE 0 END) AS NewOrders,
                        SUM(CASE WHEN o.PaymentStatus = 'Paid' THEN 1 ELSE 0 END) AS CompletedOrders,
                        SUM(CASE WHEN o.PaymentStatus = 'Unpaid' THEN 1 ELSE 0 END) AS UnpaidOrders
                    FROM Orders o
                    JOIN OrderItems oi ON o.OrderID = oi.OrderID
                    JOIN Products p ON oi.ProductID = p.ProductID
                    WHERE p.AdminID = @AdminID
                    ", conn))
                {
                    cmd.Parameters.AddWithValue("@AdminID", adminId);
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }
        public static DataTable GetOrdersByAdmin(int adminId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        o.OrderID,
                        p.ProductName,
                        p.Image AS ImageUrl,
                        o.OrderDate,
                        oi.UnitPrice AS Price,
                        o.PaymentStatus,
                        CASE 
                            WHEN o.PaymentStatus = 'Paid' THEN 'Delivered'
                            ELSE 'Pending'
                        END AS OrderStatus
                    FROM Orders o
                    JOIN OrderItems oi ON o.OrderID = oi.OrderID
                    JOIN Products p ON oi.ProductID = p.ProductID
                    WHERE p.AdminID = @AdminID
                    ORDER BY o.OrderDate DESC
                    ", conn))
                {
                    cmd.Parameters.AddWithValue("@AdminID", adminId);
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }
    }
}