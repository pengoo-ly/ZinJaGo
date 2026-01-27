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
        private string _connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        // Properties
        public int VoucherID { get; set; }
        public string Code { get; set; }
        public string VoucherType { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public int? CoinCost { get; set; } // Nullable as per DB
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }

        // Optional: Joined info
        public string AdminName { get; set; } // Only if using join

        // Constructors
        public Cupon() { }

        public Cupon(int voucherID, string code, string voucherType, string discountType,
                     decimal discountValue, int? coinCost, DateTime expiryDate, string status, int createdBy,
                     string adminName = "")
        {
            VoucherID = voucherID;
            Code = code;
            VoucherType = voucherType;
            DiscountType = discountType;
            DiscountValue = discountValue;
            CoinCost = coinCost;
            ExpiryDate = expiryDate;
            Status = status;
            CreatedBy = createdBy;
            AdminName = adminName;
        }

        // --- CRUD METHODS ---

        public Cupon GetCoupon(int voucherID)
        {
            Cupon coupon = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Vouchers WHERE VoucherID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", voucherID);
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            coupon = new Cupon(
                                Convert.ToInt32(dr["VoucherID"]),
                                dr["Code"].ToString(),
                                dr["VoucherType"].ToString(),
                                dr["DiscountType"].ToString(),
                                Convert.ToDecimal(dr["DiscountValue"]),
                                dr["CoinCost"] != DBNull.Value ? (int?)Convert.ToInt32(dr["CoinCost"]) : null,
                                Convert.ToDateTime(dr["ExpiryDate"]),
                                dr["Status"].ToString(),
                                dr["CreatedBy"] != DBNull.Value ? Convert.ToInt32(dr["CreatedBy"]) : 0
                            );
                        }
                    }
                }
            }
            catch { }
            return coupon;
        }

        public List<Cupon> GetCouponsByAdmin(int adminId)
        {
            List<Cupon> list = new List<Cupon>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT * FROM Vouchers WHERE CreatedBy=@AdminID ORDER BY VoucherID DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@AdminID", adminId);
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new Cupon(
                                Convert.ToInt32(dr["VoucherID"]),
                                dr["Code"].ToString(),
                                dr["VoucherType"].ToString(),
                                dr["DiscountType"].ToString(),
                                Convert.ToDecimal(dr["DiscountValue"]),
                                dr["CoinCost"] != DBNull.Value ? (int?)Convert.ToInt32(dr["CoinCost"]) : null,
                                Convert.ToDateTime(dr["ExpiryDate"]),
                                dr["Status"].ToString(),
                                dr["CreatedBy"] != DBNull.Value ? Convert.ToInt32(dr["CreatedBy"]) : 0
                            ));
                        }
                    }
                }
            }
            catch { }
            return list;
        }

        public bool Create()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    int nextID = GetNextVoucherID(conn);

                    using (SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO Vouchers
                        (VoucherID, Code, VoucherType, DiscountType, DiscountValue, CoinCost, ExpiryDate, Status, CreatedBy)
                        VALUES
                        (@ID, @Code, @VT, @DT, @DV, @CC, @ED, @Status, @AdminID)", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", nextID);
                        cmd.Parameters.AddWithValue("@Code", Code);
                        cmd.Parameters.AddWithValue("@VT", VoucherType);
                        cmd.Parameters.AddWithValue("@DT", DiscountType);
                        cmd.Parameters.AddWithValue("@DV", DiscountValue);
                        cmd.Parameters.AddWithValue("@CC",CoinCost.HasValue ? (object)CoinCost.Value : DBNull.Value);
                        // Nullable
                        cmd.Parameters.AddWithValue("@ED", ExpiryDate);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@AdminID", CreatedBy);
                        cmd.ExecuteNonQuery();
                    }

                    VoucherID = nextID;
                }
                return true;
            }
            catch { return false; }
        }

        public bool Update()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(@"
                    UPDATE Vouchers SET
                        Code=@Code,
                        VoucherType=@VT,
                        DiscountType=@DT,
                        DiscountValue=@DV,
                        CoinCost=@CC,
                        ExpiryDate=@ED,
                        Status=@Status
                    WHERE VoucherID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", VoucherID);
                    cmd.Parameters.AddWithValue("@Code", Code);
                    cmd.Parameters.AddWithValue("@VT", VoucherType);
                    cmd.Parameters.AddWithValue("@DT", DiscountType);
                    cmd.Parameters.AddWithValue("@DV", DiscountValue);
                    cmd.Parameters.AddWithValue("@CC", (object)CoinCost ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ED", ExpiryDate);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch { return false; }
        }

        public bool Delete()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Vouchers WHERE VoucherID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", VoucherID);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch { return false; }
        }

        // --- JOINED METHOD ---
        public List<Cupon> GetCouponsWithAdmin()
        {
            List<Cupon> list = new List<Cupon>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT v.*, a.AdminName
                    FROM Vouchers v
                    LEFT JOIN Admins a ON v.CreatedBy = a.AdminID
                    ORDER BY v.VoucherID DESC", conn))
                {
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new Cupon(
                                Convert.ToInt32(dr["VoucherID"]),
                                dr["Code"].ToString(),
                                dr["VoucherType"].ToString(),
                                dr["DiscountType"].ToString(),
                                Convert.ToDecimal(dr["DiscountValue"]),
                                dr["CoinCost"] != DBNull.Value ? (int?)Convert.ToInt32(dr["CoinCost"]) : null,
                                Convert.ToDateTime(dr["ExpiryDate"]),
                                dr["Status"].ToString(),
                                dr["CreatedBy"] != DBNull.Value ? Convert.ToInt32(dr["CreatedBy"]) : 0,
                                dr["AdminName"] != DBNull.Value ? dr["AdminName"].ToString() : ""
                            ));
                        }
                    }
                }
            }
            catch { }
            return list;
        }

        // --- HELPER ---
        private int GetNextVoucherID(SqlConnection conn)
        {
            int nextID = 1;
            using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(VoucherID),0)+1 FROM Vouchers", conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != null)
                    nextID = Convert.ToInt32(result);
            }
            return nextID;
        }
    }
}