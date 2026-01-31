using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Prevent caching
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetNoStore();
                    Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                    Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
                    Response.AddHeader("Pragma", "no-cache");
                    Response.AddHeader("Expires", "0");

                    // Check if customer is already logged in
                    if (Session["IsCustomerLoggedIn"] != null && (bool)Session["IsCustomerLoggedIn"])
                    {
                        Response.Redirect("~/Home.aspx");
                    }

                    // Check if admin/seller is already logged in
                    if (Session["IsAdminLoggedIn"] != null && (bool)Session["IsAdminLoggedIn"])
                    {
                        string role = Session["AdminRole"]?.ToString();
                        if (role == "SuperAdmin")
                        {
                            Response.Redirect("~/SuperAdmin/ZinJaGODashboard.aspx");
                        }
                        else
                        {
                            Response.Redirect("AdminDashboard.aspx");
                        }
                    }

                    // Load saved email for customer if remember me was checked
                    if (Request.Cookies["CustomerEmail"] != null)
                    {
                        txtCustomerEmail.Text = Request.Cookies["CustomerEmail"].Value;
                        chkCustomerRemember.Checked = true;
                    }

                    // Load saved email for admin if remember me was checked
                    if (Request.Cookies["AdminEmail"] != null)
                    {
                        txtSellerEmail.Text = Request.Cookies["AdminEmail"].Value;
                        chkSellerRemember.Checked = true;
                    }
                }
            }
            catch
            {
                // Silently handle errors on page load
            }
        }

        #region Customer Login

        protected void btnCustomerLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string email = txtCustomerEmail.Text.Trim();
                string password = txtCustomerPassword.Text;

                // Remember email (optional)
                if (chkCustomerRemember.Checked)
                {
                    HttpCookie cookie = new HttpCookie("CustomerEmail", email);
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);
                }

                int userId;
                string userName;

                if (ValidateCustomerCredentials(email, password, out userId, out userName))
                {
                    Session["IsCustomerLoggedIn"] = true;
                    Session["UserID"] = userId;
                    Session["UserName"] = userName;
                    Session["UserEmail"] = email;

                    UpdateCustomerLastLogin(userId);
                    DbLogger.Log("Customer login successful", userId);
                    Response.Redirect("~/Home.aspx");
                }
                else
                {
                    DbLogger.Log("Failed customer login attempt for email: " + email);
                    ShowCustomerError("Invalid email or password.");
                    txtCustomerPassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Customer login error: " + ex.Message);
                ShowCustomerError("An unexpected error occurred. Please try again.");
            }
        }

        protected void lnkCustomerForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Customer/CustomerPasswordReset.aspx");
        }

        private bool ValidateCustomerCredentials(string email, string password, out int userId, out string userName)
        {
            userId = 0;
            userName = "";
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"SELECT UserID, Username, PasswordHash, Status
                             FROM Users
                             WHERE Email = @Email
                             AND Status = 'Active'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader["PasswordHash"].ToString();
                                string status = reader["Status"].ToString();

                                // Verify the password against the stored hash
                                if (VerifyPassword(password, storedHash, userId))
                                {
                                    userId = Convert.ToInt32(reader["UserID"]);
                                    userName = reader["Username"].ToString();
                                    return true;
                                }
                                else
                                {
                                    DbLogger.Log($"Invalid password for customer: {email}");
                                    return false;
                                }
                            }
                            else
                            {
                                DbLogger.Log($"Customer not found: {email}");
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error validating customer credentials: " + ex.Message);
            }
            return false;
        }

        private void ShowCustomerError(string message)
        {
            lblCustomerError.Text = message;
            customerErrorMessage.Attributes["class"] = "error-message show";
        }

        #endregion

        #region Seller/Admin Login

        protected void btnSellerLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string email = txtSellerEmail.Text.Trim();
                string password = txtSellerPassword.Text;

                DbLogger.Log($"[LOGIN ATTEMPT] Email: {email}, Password length: {password.Length}");

                // Remember email (optional)
                if (chkSellerRemember.Checked)
                {
                    HttpCookie cookie = new HttpCookie("AdminEmail", email);
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);
                }

                int adminId;
                string adminName;
                string role;

                if (ValidateSellerCredentials(email, password, out adminId, out adminName, out role))
                {
                    Session["IsAdminLoggedIn"] = true;
                    Session["AdminID"] = adminId;
                    Session["AdminName"] = adminName;
                    Session["AdminRole"] = role;
                    Session["AdminEmail"] = email;

                    UpdateAdminLastLogin(adminId);
                    DbLogger.Log("Admin login successful");

                    if (role == "SuperAdmin")
                    {
                        Response.Redirect("~/SuperAdmin/ZinJaGODashboard.aspx");
                    }
                    else
                    {
                        Response.Redirect("AdminDashboard.aspx");
                    }
                }
                else
                {
                    DbLogger.Log("Failed admin login attempt for email: " + email);
                    ShowSellerError("Invalid email or password.");
                    txtSellerPassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Admin login error: " + ex.Message);
                ShowSellerError("An unexpected error occurred. Please try again.");
            }
        }

        protected void lnkSellerForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPasswordReset.aspx");
        }

        private bool ValidateSellerCredentials(string email, string password, out int adminId, out string adminName, out string role)
        {
            adminId = 0;
            adminName = "";
            role = "";
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                DbLogger.Log($"[DEBUG] Starting ValidateSellerCredentials for email: {email}");

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"SELECT AdminID, AdminName, Role, PasswordHash
                         FROM Admins
                         WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        try
                        {
                            conn.Open();
                            DbLogger.Log($"[DEBUG] Database connection opened successfully");
                        }
                        catch (Exception connEx)
                        {
                            DbLogger.Log($"[ERROR] Failed to open connection: {connEx.Message}");
                            return false;
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                DbLogger.Log($"[DEBUG] Query returned rows for email: {email}");

                                if (reader.Read())
                                {
                                    DbLogger.Log($"[DEBUG] reader.Read() returned TRUE - Admin found!");

                                    string storedHash = reader["PasswordHash"].ToString();
                                    adminId = Convert.ToInt32(reader["AdminID"]);
                                    adminName = reader["AdminName"].ToString();
                                    role = reader["Role"].ToString();

                                    DbLogger.Log($"[DEBUG] Admin Details - ID: {adminId}, Name: {adminName}, Role: {role}");
                                    DbLogger.Log($"[DEBUG] Stored Hash: {storedHash}");
                                    DbLogger.Log($"[DEBUG] Password to verify: {password}");
                                    DbLogger.Log($"[DEBUG] About to call VerifyPassword()");

                                    // Verify the password against the stored hash
                                    if (VerifyPassword(password, storedHash, adminId))
                                    {
                                        DbLogger.Log($"[SUCCESS] VerifyPassword returned TRUE - Login successful!");
                                        return true;
                                    }
                                    else
                                    {
                                        DbLogger.Log($"[FAIL] VerifyPassword returned FALSE - Password does not match!");
                                        return false;
                                    }
                                }
                                else
                                {
                                    DbLogger.Log($"[ERROR] reader.Read() returned FALSE - This should not happen!");
                                    return false;
                                }
                            }
                            else
                            {
                                DbLogger.Log($"[ERROR] Query returned NO rows for email: {email}");
                                DbLogger.Log($"[ERROR] Admin account not found in database!");
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log($"[EXCEPTION] Error validating seller credentials: {ex.Message}");
                DbLogger.Log($"[EXCEPTION] Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        private void ShowSellerError(string message)
        {
            lblSellerError.Text = message;
            sellerErrorMessage.Attributes["class"] = "error-message show";
        }

        #endregion

        #region Helper Methods

        private void UpdateCustomerLastLogin(int userId)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"UPDATE Users
                             SET LastLogin = GETDATE()
                             WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error updating customer last login: " + ex.Message);
            }
        }

        private void UpdateAdminLastLogin(int adminId)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"UPDATE Admins
                         SET LastLogin = GETDATE()
                         WHERE AdminID = @AdminID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AdminID", adminId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error updating admin last login: " + ex.Message);
            }
        }

        #endregion

        #region Password Hashing & Verification

        /// <summary>
        /// Hashes password using SHA256 in Base64 format
        /// </summary>
        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        private string HashPasswordHex(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Verifies a password against stored hash
        /// </summary>
        private bool VerifyPassword(string enteredPassword, string storedHash, int adminId)
        {
            try
            {
                // 1️⃣ Base64 SHA256 (NEW accounts)
                string base64Hash = HashPassword(enteredPassword);
                if (base64Hash.Equals(storedHash, StringComparison.Ordinal))
                {
                    return true;
                }

                // 2️⃣ HEX SHA256 (OLD accounts)
                string hexHash = HashPasswordHex(enteredPassword);
                if (hexHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase))
                {
                    // 🔄 Upgrade old HEX hash → Base64
                    UpdateAdminPasswordHash(adminId, base64Hash);
                    DbLogger.Log($"[MIGRATION] AdminID {adminId} password upgraded from HEX to Base64");

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                DbLogger.Log($"[VERIFY ERROR] {ex.Message}");
                return false;
            }
        }

        private void UpdateAdminPasswordHash(int adminId, string newHash)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"UPDATE Admins
                         SET PasswordHash = @PasswordHash
                         WHERE AdminID = @AdminID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PasswordHash", newHash);
                    cmd.Parameters.AddWithValue("@AdminID", adminId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        #endregion
    }
}