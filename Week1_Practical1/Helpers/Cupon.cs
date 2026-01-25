using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Week1_Practical1.Helpers
{
    public class Cupon
    {
        private static string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        public static DataTable GetAllCoupons()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        V.VoucherID,
                        V.Code,
                        V.VoucherType,
                        V.DiscountType,
                        V.DiscountValue,
                        V.CoinCost,
                        V.ExpiryDate,
                        V.Status,
                        A.AdminName AS CreatedBy
                    FROM Vouchers V
                    LEFT JOIN AuditTrail AT ON AT.Action = CONCAT('CREATE_VOUCHER_', V.VoucherID)
                    LEFT JOIN Admins A ON AT.AdminID = A.AdminID
                    ORDER BY V.VoucherID DESC", con))
                {
                    new SqlDataAdapter(cmd).Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        // 🔹 Get single coupon
        public static DataRow GetCoupon(int voucherId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT * FROM Vouchers WHERE VoucherID=@ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", voucherId);
                new SqlDataAdapter(cmd).Fill(dt);
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        // 🔹 Create coupon + audit
        public static bool CreateCoupon(
            int adminId,
            string code,
            string voucherType,
            string discountType,
            decimal discountValue,
            int coinCost,
            DateTime expiry,
            string status)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    SqlCommand insert = new SqlCommand(@"
                        INSERT INTO Vouchers
                        VALUES
                        ((SELECT ISNULL(MAX(VoucherID),0)+1 FROM Vouchers),
                         @Code,@VT,@CC,@DT,@DV,@ED,@Status)", con);

                    insert.Parameters.AddWithValue("@Code", code);
                    insert.Parameters.AddWithValue("@VT", voucherType);
                    insert.Parameters.AddWithValue("@CC", coinCost);
                    insert.Parameters.AddWithValue("@DT", discountType);
                    insert.Parameters.AddWithValue("@DV", discountValue);
                    insert.Parameters.AddWithValue("@ED", expiry);
                    insert.Parameters.AddWithValue("@Status", status);

                    insert.ExecuteNonQuery();

                    SqlCommand audit = new SqlCommand(@"
                        INSERT INTO AuditTrail
                        VALUES
                        ((SELECT ISNULL(MAX(AuditID),0)+1 FROM AuditTrail),
                         @AdminID,
                         CONCAT('CREATE_VOUCHER_', (SELECT MAX(VoucherID) FROM Vouchers)),
                         GETDATE())", con);

                    audit.Parameters.AddWithValue("@AdminID", adminId);
                    audit.ExecuteNonQuery();
                }
                return true;
            }
            catch { return false; }
        }

        // 🔹 Update coupon
        public static bool UpdateCoupon(
            int adminId,
            int voucherId,
            string code,
            string voucherType,
            string discountType,
            decimal discountValue,
            int coinCost,
            DateTime expiry,
            string status)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
                        UPDATE Vouchers SET
                            Code=@Code,
                            VoucherType=@VT,
                            DiscountType=@DT,
                            DiscountValue=@DV,
                            CoinCost=@CC,
                            ExpiryDate=@ED,
                            Status=@Status
                        WHERE VoucherID=@ID", con);

                    cmd.Parameters.AddWithValue("@ID", voucherId);
                    cmd.Parameters.AddWithValue("@Code", code);
                    cmd.Parameters.AddWithValue("@VT", voucherType);
                    cmd.Parameters.AddWithValue("@DT", discountType);
                    cmd.Parameters.AddWithValue("@DV", discountValue);
                    cmd.Parameters.AddWithValue("@CC", coinCost);
                    cmd.Parameters.AddWithValue("@ED", expiry);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.ExecuteNonQuery();

                    SqlCommand audit = new SqlCommand(@"
                        INSERT INTO AuditTrail
                        VALUES
                        ((SELECT ISNULL(MAX(AuditID),0)+1 FROM AuditTrail),
                         @AdminID,
                         CONCAT('UPDATE_VOUCHER_',@VID),
                         GETDATE())", con);

                    audit.Parameters.AddWithValue("@AdminID", adminId);
                    audit.Parameters.AddWithValue("@VID", voucherId);
                    audit.ExecuteNonQuery();
                }
                return true;
            }
            catch { return false; }
        }

        public static DataTable GetCouponsByAdmin(int adminId, string search = "")
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand(@"
                        SELECT V.*
                        FROM Vouchers V
                        INNER JOIN AuditTrail A 
                            ON A.Action = CONCAT('CREATE_VOUCHER_', V.VoucherID)
                        WHERE A.AdminID = @AdminID
                        AND (@Search = '' OR V.Code LIKE '%' + @Search + '%')
                        ORDER BY V.VoucherID DESC", con))
                {
                    cmd.Parameters.AddWithValue("@AdminID", adminId);
                    cmd.Parameters.AddWithValue("@Search", search);
                    new SqlDataAdapter(cmd).Fill(dt);
                }
            }
            catch { }
            return dt;
        }

        public static bool AddCoupon(
    int adminId, string code, string type,
    string discountType, decimal discount,
    int coinCost, DateTime expiry)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();

                    SqlCommand insert = new SqlCommand(@"
                        INSERT INTO Vouchers
                        VALUES ((SELECT ISNULL(MAX(VoucherID),0)+1 FROM Vouchers),
                        @Code,@Type,@Coin,@DT,@DV,@Exp,'Active')", con);

                    insert.Parameters.AddWithValue("@Code", code);
                    insert.Parameters.AddWithValue("@Type", type);
                    insert.Parameters.AddWithValue("@Coin", coinCost);
                    insert.Parameters.AddWithValue("@DT", discountType);
                    insert.Parameters.AddWithValue("@DV", discount);
                    insert.Parameters.AddWithValue("@Exp", expiry);

                    insert.ExecuteNonQuery();

                    SqlCommand audit = new SqlCommand(@"
                        INSERT INTO AuditTrail
                        VALUES ((SELECT ISNULL(MAX(AuditID),0)+1 FROM AuditTrail),
                        @Admin,'CREATE_VOUCHER_' + 
                        CAST((SELECT MAX(VoucherID) FROM Vouchers) AS NVARCHAR),
                        GETDATE())", con);

                    audit.Parameters.AddWithValue("@Admin", adminId);
                    audit.ExecuteNonQuery();
                }
                return true;
            }
            catch { return false; }
        }

    }
}