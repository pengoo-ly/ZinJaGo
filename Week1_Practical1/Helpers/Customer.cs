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
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }

        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        public List<Customer> GetCustomersByAdmin(int adminId)
        {
            List<Customer> list = new List<Customer>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(@"
                SELECT DISTINCT
                    U.UserID, U.Username, U.Email, U.Phone,
                    COUNT(DISTINCT O.OrderID) AS TotalOrders,
                    SUM(OI.Quantity * OI.UnitPrice) AS TotalSpent
                FROM Users U
                JOIN Orders O ON U.UserID = O.UserID
                JOIN OrderItems OI ON O.OrderID = OI.OrderID
                JOIN Products P ON OI.ProductID = P.ProductID
                WHERE P.AdminID = @AdminID
                  AND O.PaymentStatus = 'Paid'
                GROUP BY U.UserID, U.Username, U.Email, U.Phone", con);

                cmd.Parameters.AddWithValue("@AdminID", adminId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Customer
                    {
                        UserID = Convert.ToInt32(dr["UserID"]),
                        Username = dr["Username"].ToString(),
                        Email = dr["Email"].ToString(),
                        Phone = dr["Phone"].ToString(),
                        TotalOrders = Convert.ToInt32(dr["TotalOrders"]),
                        TotalSpent = Convert.ToDecimal(dr["TotalSpent"])
                    });
                }
            }
            return list;
        }
    }
}