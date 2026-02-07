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
    }
}