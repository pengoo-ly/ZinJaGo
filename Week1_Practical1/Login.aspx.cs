using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
                string password = txtCustomerPassword.Text.Trim();

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

                    UpdateLastLogin(userId);
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
            Response.Redirect("CustomerPasswordReset.aspx");
        }

        private bool ValidateCustomerCredentials(string email, string password, out int userId, out string userName)
        {
            userId = 0;
            userName = "";
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"SELECT UserID, Username
                             FROM Users
                             WHERE Email = @Email
                             AND PasswordHash = @Password
                             AND Status = 'Active'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = Convert.ToInt32(reader["UserID"]);
                                userName = reader["Username"].ToString();
                                return true;
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
                string password = txtSellerPassword.Text.Trim();

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

                    //UpdateAdminLastLogin(adminId);
                    DbLogger.Log("Admin login successful", adminId);

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
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"SELECT AdminID, AdminName, Role
                         FROM Admins
                         WHERE Email = @Email
                         AND PasswordHash = @Password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                adminId = Convert.ToInt32(reader["AdminID"]);
                                adminName = reader["AdminName"].ToString();
                                role = reader["Role"].ToString();
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error validating seller credentials: " + ex.Message);
            }
            return false;
        }

        private void ShowSellerError(string message)
        {
            lblSellerError.Text = message;
            sellerErrorMessage.Attributes["class"] = "error-message show";
        }

        #endregion

        #region Helper Methods

        private void UpdateLastLogin(int userId)
        {
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"UPDATE Users
                             SET DateCreated = GETDATE()
                             WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // Log but don't fail the login
            }
        }

        private void UpdateAdminLastLogin(int adminId)
        {
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

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
            catch
            {
                // Log but don't fail the login
            }
        }

        #endregion
    }
}