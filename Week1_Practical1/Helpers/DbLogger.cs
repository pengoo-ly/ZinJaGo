using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace Week1_Practical1.Helpers
{
    public class DbLogger
    {
        public static void Log(string action, int? userId = null)
        {
            try
            {
                string cs = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"
                        INSERT INTO SystemLogs (UserID, Action, Timestamp)
                        VALUES (@UserID, @Action, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UserID",
                            (object)userId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Action", action);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log to a file or event viewer)
                // For simplicity, we are just rethrowing the exception here
                throw new ApplicationException("Error logging to database", ex);
            }
        }
    }
}