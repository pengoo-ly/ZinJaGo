using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.Script.Serialization;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class AdminCupon : System.Web.UI.Page
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Handle AJAX requests first (GET for fetch, POST for delete)
            string action = Request.QueryString["action"];

            // Handle GET request (fetch coupon data for edit)
            if (action == "get" && Request.HttpMethod == "GET")
            {
                HandleGetRequest();
                return;
            }

            // Handle DELETE request
            if (action == "delete" && Request.HttpMethod == "POST")
            {
                HandleDeleteRequest();
                return;
            }

            // Handle CREATE/UPDATE requests (FormData POST)
            if (Request.HttpMethod == "POST" && Request.Form["action"] != null)
            {
                string formAction = Request.Form["action"];

                if (formAction == "create")
                {
                    HandleCreateRequest();
                    return;
                }
                else if (formAction == "update")
                {
                    HandleUpdateRequest();
                    return;
                }
            }

            // Normal page load
            if (!IsPostBack)
            {
                LoadCoupons();
                UpdateStatistics();
            }
        }

        private void HandleGetRequest()
        {
            try
            {
                if (!int.TryParse(Request.QueryString["id"], out int voucherId))
                {
                    RespondJson(new { success = false, message = "Invalid ID" }, 400);
                    return;
                }

                DataTable dt = GetVoucherData(voucherId);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    var responseData = new
                    {
                        VoucherID = row["VoucherID"],
                        Code = row["Code"].ToString(),
                        VoucherType = row["VoucherType"].ToString(),
                        DiscountType = row["DiscountType"].ToString(),
                        DiscountValue = decimal.Parse(row["DiscountValue"].ToString()),
                        CoinCost = decimal.Parse(row["CoinCost"].ToString()),
                        ExpiryDate = ((DateTime)row["ExpiryDate"]).ToString("yyyy-MM-dd"),
                        Status = row["Status"].ToString()
                    };

                    RespondJson(responseData);
                }
                else
                {
                    RespondJson(new { success = false, message = "Coupon not found" }, 404);
                }
            }
            catch (Exception ex)
            {
                RespondJson(new { success = false, message = "Error: " + ex.Message }, 500);
            }
        }

        private void HandleCreateRequest()
        {
            try
            {
                string code = Request.Form["code"]?.Trim();
                string voucherType = Request.Form["voucherType"];
                string discountType = Request.Form["discountType"];
                string discountValueStr = Request.Form["discountValue"];
                string coinCostStr = Request.Form["coinCost"];
                string expiryDateStr = Request.Form["expiryDate"];
                string status = Request.Form["status"];

                // Validation
                if (string.IsNullOrEmpty(code))
                {
                    RespondJson(new { success = false, message = "Coupon code is required" }, 400);
                    return;
                }

                if (string.IsNullOrEmpty(voucherType) || voucherType == "")
                {
                    RespondJson(new { success = false, message = "Voucher type is required" }, 400);
                    return;
                }

                if (string.IsNullOrEmpty(discountType) || discountType == "")
                {
                    RespondJson(new { success = false, message = "Discount type is required" }, 400);
                    return;
                }

                if (!decimal.TryParse(discountValueStr, out decimal discountValue) || discountValue < 0)
                {
                    RespondJson(new { success = false, message = "Invalid discount value" }, 400);
                    return;
                }

                if (!decimal.TryParse(coinCostStr, out decimal coinCost) || coinCost < 0)
                {
                    RespondJson(new { success = false, message = "Invalid coin cost" }, 400);
                    return;
                }

                if (!DateTime.TryParse(expiryDateStr, out DateTime expiryDate))
                {
                    RespondJson(new { success = false, message = "Invalid expiry date" }, 400);
                    return;
                }

                if (string.IsNullOrEmpty(status) || status == "")
                {
                    RespondJson(new { success = false, message = "Status is required" }, 400);
                    return;
                }

                // Check if code already exists
                if (CodeExists(code))
                {
                    RespondJson(new { success = false, message = "This coupon code already exists" }, 400);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Vouchers (Code, VoucherType, DiscountType, DiscountValue, CoinCost, ExpiryDate, Status) " +
                        "VALUES (@Code, @VoucherType, @DiscountType, @DiscountValue, @CoinCost, @ExpiryDate, @Status)",
                        conn);

                    cmd.Parameters.AddWithValue("@Code", code);
                    cmd.Parameters.AddWithValue("@VoucherType", voucherType);
                    cmd.Parameters.AddWithValue("@DiscountType", discountType);
                    cmd.Parameters.AddWithValue("@DiscountValue", discountValue);
                    cmd.Parameters.AddWithValue("@CoinCost", coinCost);
                    cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                    cmd.Parameters.AddWithValue("@Status", status);

                    cmd.ExecuteNonQuery();
                }

                RespondJson(new { success = true, message = "Coupon created successfully" });
            }
            catch (Exception ex)
            {
                RespondJson(new { success = false, message = "Error: " + ex.Message }, 500);
            }
        }

        private void HandleUpdateRequest()
        {
            try
            {
                // Validate voucherId first
                string voucherIdStr = Request.Form["voucherId"];
                
                if (string.IsNullOrWhiteSpace(voucherIdStr))
                {
                    RespondJson(new { success = false, message = "Voucher ID is required" }, 400);
                    return;
                }

                if (!int.TryParse(voucherIdStr, out int voucherId) || voucherId <= 0)
                {
                    RespondJson(new { success = false, message = "Invalid voucher ID format" }, 400);
                    return;
                }

                string code = Request.Form["code"]?.Trim();
                string voucherType = Request.Form["voucherType"];
                string discountType = Request.Form["discountType"];
                string discountValueStr = Request.Form["discountValue"];
                string coinCostStr = Request.Form["coinCost"];
                string expiryDateStr = Request.Form["expiryDate"];
                string status = Request.Form["status"];

                // Validation
                if (string.IsNullOrEmpty(code))
                {
                    RespondJson(new { success = false, message = "Coupon code is required" }, 400);
                    return;
                }

                if (string.IsNullOrEmpty(voucherType) || voucherType == "")
                {
                    RespondJson(new { success = false, message = "Voucher type is required" }, 400);
                    return;
                }

                if (string.IsNullOrEmpty(discountType) || discountType == "")
                {
                    RespondJson(new { success = false, message = "Discount type is required" }, 400);
                    return;
                }

                if (!decimal.TryParse(discountValueStr, out decimal discountValue) || discountValue < 0)
                {
                    RespondJson(new { success = false, message = "Invalid discount value" }, 400);
                    return;
                }

                if (!decimal.TryParse(coinCostStr, out decimal coinCost) || coinCost < 0)
                {
                    RespondJson(new { success = false, message = "Invalid coin cost" }, 400);
                    return;
                }

                if (!DateTime.TryParse(expiryDateStr, out DateTime expiryDate))
                {
                    RespondJson(new { success = false, message = "Invalid expiry date" }, 400);
                    return;
                }

                if (string.IsNullOrEmpty(status) || status == "")
                {
                    RespondJson(new { success = false, message = "Status is required" }, 400);
                    return;
                }

                // Check if code already exists (excluding current voucher)
                if (CodeExistsExcept(code, voucherId))
                {
                    RespondJson(new { success = false, message = "This coupon code already exists" }, 400);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Vouchers SET Code = @Code, VoucherType = @VoucherType, DiscountType = @DiscountType, " +
                        "DiscountValue = @DiscountValue, CoinCost = @CoinCost, ExpiryDate = @ExpiryDate, Status = @Status " +
                        "WHERE VoucherID = @VoucherID",
                        conn);

                    cmd.Parameters.AddWithValue("@VoucherID", voucherId);
                    cmd.Parameters.AddWithValue("@Code", code);
                    cmd.Parameters.AddWithValue("@VoucherType", voucherType);
                    cmd.Parameters.AddWithValue("@DiscountType", discountType);
                    cmd.Parameters.AddWithValue("@DiscountValue", discountValue);
                    cmd.Parameters.AddWithValue("@CoinCost", coinCost);
                    cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                    cmd.Parameters.AddWithValue("@Status", status);

                    cmd.ExecuteNonQuery();
                }

                RespondJson(new { success = true, message = "Coupon updated successfully" });
            }
            catch (FormatException)
            {
                RespondJson(new { success = false, message = "Invalid data format provided" }, 400);
            }
            catch (Exception ex)
            {
                RespondJson(new { success = false, message = "Error: " + ex.Message }, 500);
            }
        }

        private void HandleDeleteRequest()
        {
            try
            {
                if (!int.TryParse(Request.QueryString["id"], out int voucherId) || voucherId <= 0)
                {
                    RespondJson(new { success = false, message = "Invalid ID" }, 400);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    try
                    {
                        SqlCommand cmd = new SqlCommand("DELETE FROM Vouchers WHERE VoucherID = @VoucherID", conn);
                        cmd.Parameters.AddWithValue("@VoucherID", voucherId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            RespondJson(new { success = false, message = "Coupon not found" }, 404);
                            return;
                        }

                        RespondJson(new { success = true, message = "Coupon deleted successfully" });
                    }
                    catch (SqlException sqlEx)
                    {
                        // Check for foreign key constraint violation
                        if (sqlEx.Number == 547)
                        {
                            RespondJson(new
                            {
                                success = false,
                                message = "Cannot delete this coupon because it is being used..."
                            }, 400);
                        }

                        else
                        {
                            RespondJson(new { success = false, message = "Database error: " + sqlEx.Message }, 500);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RespondJson(new { success = false, message = "Error: " + ex.Message }, 500);
            }
        }

        private void LoadCoupons()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT VoucherID, Code, VoucherType, DiscountType, DiscountValue, CoinCost, ExpiryDate, Status " +
                        "FROM Vouchers ORDER BY VoucherID DESC",
                        conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                rptCoupons.DataSource = dt;
                rptCoupons.DataBind();
            }
            catch (Exception ex)
            {
                // Log error silently
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Total Coupons
                    SqlCommand totalCmd = new SqlCommand("SELECT COUNT(*) FROM Vouchers", conn);
                    int totalCount = (int)totalCmd.ExecuteScalar();
                    lblTotalCoupons.Text = totalCount.ToString();

                    // Active Coupons
                    SqlCommand activeCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Vouchers WHERE Status = 'Active' AND ExpiryDate > GETDATE()",
                        conn);
                    int activeCount = (int)activeCmd.ExecuteScalar();
                    lblActiveCoupons.Text = activeCount.ToString();

                    // Total Discount Value
                    SqlCommand discountCmd = new SqlCommand(
                        "SELECT ISNULL(SUM(CAST(DiscountValue AS DECIMAL(10,2))), 0) FROM Vouchers WHERE Status = 'Active'",
                        conn);
                    object discountResult = discountCmd.ExecuteScalar();
                    decimal totalDiscount = 0;
                    if (discountResult != null && discountResult != DBNull.Value)
                    {
                        totalDiscount = Convert.ToDecimal(discountResult);
                    }
                    lblTotalDiscount.Text = totalDiscount.ToString("0.00");

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Log error silently
            }
        }

        private DataTable GetVoucherData(int voucherId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT VoucherID, Code, VoucherType, DiscountType, DiscountValue, CoinCost, ExpiryDate, Status " +
                        "FROM Vouchers WHERE VoucherID = @VoucherID",
                        conn);

                    cmd.Parameters.AddWithValue("@VoucherID", voucherId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                // Log error
            }

            return dt;
        }

        private bool CodeExists(string code)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Vouchers WHERE Code = @Code",
                        conn);
                    cmd.Parameters.AddWithValue("@Code", code);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool CodeExistsExcept(string code, int voucherId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Vouchers WHERE Code = @Code AND VoucherID != @VoucherID",
                        conn);
                    cmd.Parameters.AddWithValue("@Code", code);
                    cmd.Parameters.AddWithValue("@VoucherID", voucherId);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private void RespondJson(object data, int statusCode = 200)
        {
            Response.Clear();
            Response.StatusCode = statusCode;
            Response.ContentType = "application/json";
            Response.TrySkipIisCustomErrors = true;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            Response.Write(json);

            // ❌ REMOVE THIS
            // Response.End();

            // ✅ ADD THIS
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

    }
}