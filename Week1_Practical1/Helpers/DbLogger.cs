using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace Week1_Practical1.Helpers
{
    public class DbLogger
    {
        private const int MAX_ACTION_LENGTH = 255;

        public static void Log(string action, int? userId = null)
        {
            try
            {
                // Truncate action to fit the NVARCHAR(255) column
                string truncatedAction = action;
                if (action != null && action.Length > MAX_ACTION_LENGTH)
                {
                    truncatedAction = action.Substring(0, MAX_ACTION_LENGTH);
                    // Also log to debug output that truncation occurred
                    Debug.WriteLine($"[DbLogger] WARNING: Message truncated from {action.Length} to {MAX_ACTION_LENGTH} chars");
                    Debug.WriteLine($"[DbLogger] Original: {action}");
                    Debug.WriteLine($"[DbLogger] Truncated: {truncatedAction}");
                }

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
                        cmd.Parameters.AddWithValue("@Action", truncatedAction);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log to debug output instead of throwing exception
                // This prevents DbLogger errors from crashing the application
                Debug.WriteLine($"[DbLogger] Error logging to database: {ex.Message}");
                Debug.WriteLine($"[DbLogger] Stack Trace: {ex.StackTrace}");
                
                // Only throw if it's critical
                if (ex.Message.Contains("connection"))
                {
                    throw new ApplicationException("Error connecting to database for logging", ex);
                }
                // For other errors (like truncation), just log to debug and continue
            }
        }
    }
}
