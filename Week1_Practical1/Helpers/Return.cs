using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Week1_Practical1.Helpers
{
    public class Return
    {
        string _connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        public int ReturnID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string Reason { get; set; }
        public string ReturnStatus { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal? RefundAmount { get; set; }
        public int? ProcessedBy { get; set; }

        public Return() { }

        public List<Return> GetAllReturns()
        {
            List<Return> list = new List<Return>();

            try
            {
                string sql = "SELECT * FROM Returns ORDER BY ReturnID DESC";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Return r = new Return
                            {
                                ReturnID = Convert.ToInt32(dr["ReturnID"]),
                                OrderID = Convert.ToInt32(dr["OrderID"]),
                                ProductID = Convert.ToInt32(dr["ProductID"]),
                                Reason = dr["Reason"].ToString(),
                                ReturnStatus = dr["ReturnStatus"].ToString(),
                                RefundAmount = dr["RefundAmount"] == DBNull.Value ? null : (decimal?)dr["RefundAmount"],
                                ReturnDate = dr["ReturnDate"] == DBNull.Value ? null : (DateTime?)dr["ReturnDate"],
                                ProcessedBy = dr["ProcessedBy"] == DBNull.Value ? null : (int?)dr["ProcessedBy"]
                            };

                            list.Add(r);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle database-related errors (log if needed)
                throw new Exception("An error occurred while retrieving return records from the database.", ex);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                throw new Exception("An unexpected error occurred while retrieving return records.", ex);
            }

            return list;
        }

        public int UpdateStatus(int returnId, string status, int adminId)
        {
            try
            {
                string sql = @"
            UPDATE Returns
            SET ReturnStatus = @status,
                ProcessedBy = @adminId,
                ReturnDate = GETDATE()
            WHERE ReturnID = @returnId";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@adminId", adminId);
                    cmd.Parameters.AddWithValue("@returnId", returnId);

                    conn.Open();
                    return cmd.ExecuteNonQuery(); // number of rows affected
                }
            }
            catch (SqlException ex)
            {
                // Database-related error
                throw new Exception("An error occurred while updating the return status.", ex);
            }
            catch (Exception ex)
            {
                // Any other unexpected error
                throw new Exception("An unexpected error occurred while updating the return status.", ex);
            }
        }
        public decimal CalculateRefund(int orderId, int productId)
        {
            decimal refund = 0;

            try
            {
                string sql = @"
                    SELECT Quantity * UnitPrice 
                    FROM OrderItems
                    WHERE OrderID = @orderId AND ProductID = @productId";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Parameters.AddWithValue("@productId", productId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        refund = Convert.ToDecimal(result);
                    }
                }
            }
            catch (SqlException ex)
            {
                // Database-related error
                throw new Exception("An error occurred while calculating the refund.", ex);
            }
            catch (Exception ex)
            {
                // Unexpected error
                throw new Exception("An unexpected error occurred while calculating the refund.", ex);
            }

            return refund;
        }

        public int UpdateReturnWithAudit(int returnId, string status, int adminId)
        {
            try
            {
                string sql = @"
                    UPDATE Returns
                    SET ReturnStatus = @status,
                        ProcessedBy = @adminId,
                        ReturnDate = GETDATE(),
                        RefundAmount = 
                            CASE 
                                WHEN @status IN ('Approved','Processed') 
                                THEN (
                                    SELECT TOP 1 Quantity * UnitPrice
                                    FROM OrderItems oi
                                    JOIN Returns r ON r.OrderID = oi.OrderID
                                    WHERE r.ReturnID = @returnId AND oi.ProductID = r.ProductID
                                )
                                ELSE RefundAmount
                            END
                    WHERE ReturnID = @returnId";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@adminId", adminId);
                    cmd.Parameters.AddWithValue("@returnId", returnId);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        InsertAudit(adminId, $"Return {returnId} set to {status}");
                    }

                    return rows;
                }
            }
            catch (SqlException ex)
            {
                // Database-related error
                throw new Exception("An error occurred while updating the return and audit record.", ex);
            }
            catch (Exception ex)
            {
                // Unexpected error
                throw new Exception("An unexpected error occurred while processing the return update.", ex);
            }
        }

        public void InsertAudit(int adminId, string action)
        {
            try
            {
                string sql = @"
                    INSERT INTO AuditTrail (AuditID, AdminID, Action, DateTime)
                    VALUES (
                        (SELECT ISNULL(MAX(AuditID),0)+1 FROM AuditTrail),
                        @adminId,
                        @action,
                        GETDATE()
                    )";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@adminId", adminId);
                    cmd.Parameters.AddWithValue("@action", action);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // Database-related error
                throw new Exception("An error occurred while inserting the audit trail.", ex);
            }
            catch (Exception ex)
            {
                // Unexpected error
                throw new Exception("An unexpected error occurred while inserting the audit trail.", ex);
            }
        }

        public List<Return> SearchReturns(string keyword)
        {
            try
            {
                string sql = @"
                    SELECT * FROM Returns
                    WHERE 
                        CAST(ReturnID AS NVARCHAR) LIKE @kw OR
                        CAST(OrderID AS NVARCHAR) LIKE @kw OR
                        UPPER(ReturnStatus) LIKE UPPER(@kw)
                    ORDER BY ReturnID DESC";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        List<Return> list = new List<Return>();
                        while (dr.Read())
                        {
                            list.Add(new Return
                            {
                                ReturnID = Convert.ToInt32(dr["ReturnID"]),
                                OrderID = Convert.ToInt32(dr["OrderID"]),
                                ProductID = Convert.ToInt32(dr["ProductID"]),
                                Reason = dr["Reason"].ToString(),
                                ReturnStatus = dr["ReturnStatus"].ToString(),
                                RefundAmount = dr["RefundAmount"] == DBNull.Value ? null : (decimal?)dr["RefundAmount"]
                            });
                        }
                        return list;
                    }
                }
            }
            catch
            {
                return new List<Return>();
            }
        }




    }
}