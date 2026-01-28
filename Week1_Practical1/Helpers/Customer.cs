using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Week1_Practical1.Helpers
{
    public class Customer
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalSpent { get; set; }
        public string Status { get; set; }

        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        public List<Customer> GetCustomersByAdmin(int adminId)
        {
            List<Customer> list = new List<Customer>();

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        U.UserID,
                        U.Username,
                        ISNULL(UP.Phone, 'N/A') AS Phone,
                        COUNT(DISTINCT O.OrderID) AS OrderCount,
                        SUM(OI.Quantity * OI.UnitPrice) AS TotalSpent,
                        ISNULL(U.Status, 'Active') AS Status
                    FROM Users U
                    JOIN Orders O ON U.UserID = O.UserID
                    JOIN OrderItems OI ON O.OrderID = OI.OrderID
                    JOIN Products P ON OI.ProductID = P.ProductID
                    LEFT JOIN UserProfiles UP ON U.UserID = UP.UserID
                    WHERE P.AdminID = @AdminID
                      AND O.PaymentStatus = 'Paid'
                    GROUP BY U.UserID, U.Username, UP.Phone, U.Status
                    ORDER BY TotalSpent DESC", con))
                {
                    cmd.Parameters.AddWithValue("@AdminID", adminId);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new Customer
                        {
                            UserID = Convert.ToInt32(dr["UserID"]),
                            Username = dr["Username"].ToString(),
                            Phone = dr["Phone"].ToString(),
                            OrderCount = Convert.ToInt32(dr["OrderCount"]),
                            TotalSpent = Convert.ToDecimal(dr["TotalSpent"]),
                            Status = dr["Status"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Customer.GetCustomersByAdmin ERROR: " + ex.Message);
            }

            return list;
        }
        public (int total, int active, int inactive, decimal spend) GetCustomerStats(int adminId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT
                        COUNT(DISTINCT O.UserID) AS TotalCustomers,
                        COUNT(DISTINCT CASE WHEN U.Status='Active' THEN U.UserID END) AS ActiveCustomers,
                        COUNT(DISTINCT CASE WHEN U.Status='Inactive' THEN U.UserID END) AS InactiveCustomers,
                        SUM(OI.Quantity * OI.UnitPrice) AS TotalSpend
                    FROM Orders O
                    JOIN Users U ON O.UserID = U.UserID
                    JOIN OrderItems OI ON O.OrderID = OI.OrderID
                    JOIN Products P ON OI.ProductID = P.ProductID
                    WHERE P.AdminID=@AdminID
                      AND O.PaymentStatus='Paid'
                    ", con))
                {
                    cmd.Parameters.AddWithValue("@AdminID", adminId);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        return (
                            Convert.ToInt32(dr["TotalCustomers"]),
                            Convert.ToInt32(dr["ActiveCustomers"]),
                            Convert.ToInt32(dr["InactiveCustomers"]),
                            Convert.ToDecimal(dr["TotalSpend"])
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Customer.GetCustomerStats ERROR: " + ex.Message);
            }

            return (0, 0, 0, 0);
        }
        public List<(string Day, int Count)> GetCustomerChart(int adminId)
        {
            List<(string, int)> data = new List<(string, int)>();

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        FORMAT(O.OrderDate,'ddd') AS DayName,
                        COUNT(DISTINCT O.UserID) AS Total
                    FROM Orders O
                    JOIN OrderItems OI ON O.OrderID = OI.OrderID
                    JOIN Products P ON OI.ProductID = P.ProductID
                    WHERE P.AdminID=@AdminID
                      AND O.PaymentStatus='Paid'
                      AND O.OrderDate >= DATEADD(day, -6, GETDATE())
                    GROUP BY FORMAT(O.OrderDate,'ddd'), DATEPART(WEEKDAY,O.OrderDate)
                    ORDER BY DATEPART(WEEKDAY,O.OrderDate)", con))
                {
                    cmd.Parameters.AddWithValue("@AdminID", adminId);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        data.Add((dr["DayName"].ToString(), Convert.ToInt32(dr["Total"])));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Customer.GetCustomerChart ERROR: " + ex.Message);
            }

            return data;
        }

        public List<(string Day, int Count)> GetCustomerChartByMonth(int adminId, int year, int month)
        {
            List<(string, int)> data = new List<(string, int)>();

            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        DAY(O.OrderDate) AS DayNum,
                        COUNT(DISTINCT O.UserID) AS Total
                    FROM Orders O
                    JOIN OrderItems OI ON O.OrderID = OI.OrderID
                    JOIN Products P ON OI.ProductID = P.ProductID
                    WHERE P.AdminID=@AdminID
                      AND O.PaymentStatus='Paid'
                      AND YEAR(O.OrderDate)=@Year
                      AND MONTH(O.OrderDate)=@Month
                    GROUP BY DAY(O.OrderDate)
                    ORDER BY DAY(O.OrderDate)", con))
                {
                    cmd.Parameters.AddWithValue("@AdminID", adminId);
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Month", month);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        data.Add((
                            dr["DayNum"].ToString(),
                            Convert.ToInt32(dr["Total"])
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Customer.GetCustomerChartByMonth ERROR: " + ex.Message);
            }

            return data;
        }

    }
}