using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace Week1_Practical1
{
    public partial class Admin_Login : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is already logged in
                if (Session["IsAdminLoggedIn"] != null && (bool)Session["IsAdminLoggedIn"])
                {
                    RedirectToAdminPanel();
                }

                // Check for remember me cookie
                if (Request.Cookies["AdminEmail"] != null)
                {
                    txtEmail.Text = Request.Cookies["AdminEmail"].Value;
                    chkRemember.Checked = true;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            // Validate inputs
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowError("Please enter both email and password.");
                return;
            }

            // Validate email format
            if (!IsValidEmail(email))
            {
                ShowError("Please enter a valid email address.");
                return;
            }

            // Check credentials against database
            if (ValidateCredentialsFromDatabase(email, password))
            {
                // Get admin details
                AdminDetails adminDetails = GetAdminDetails(email);

                if (adminDetails != null)
                {
                    // Update LastLogin timestamp
                    UpdateLastLogin(email);

                    // Set session variables
                    Session["IsAdminLoggedIn"] = true;
                    Session["AdminEmail"] = email;
                    Session["AdminName"] = adminDetails.AdminName;
                    Session["AdminInitial"] = GetInitials(adminDetails.AdminName);

                    // Set remember me cookie if checked
                    if (chkRemember.Checked)
                    {
                        HttpCookie cookie = new HttpCookie("AdminEmail");
                        cookie.Value = email;
                        cookie.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        // Clear remember me cookie if unchecked
                        if (Request.Cookies["AdminEmail"] != null)
                        {
                            HttpCookie cookie = new HttpCookie("AdminEmail");
                            cookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(cookie);
                        }
                    }

                    // Redirect to admin dashboard
                    RedirectToAdminPanel();
                }
            }
            else
            {
                ShowError("Invalid email or password. Please try again.");
                txtPassword.Text = "";
            }
        }

        private bool ValidateCredentialsFromDatabase(string email, string password)
        {
            try
            {
                // Hash the password to compare
                string passwordHash = HashPassword(password);
                ShowError("DEBUG HASH: " + passwordHash);

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    // Query to check if admin exists with matching email and password hash
                    SqlCommand cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Admins WHERE Email = @email AND PasswordHash = @passwordHash", conn);

                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

                    conn.Open();
                    int result = (int)cmd.ExecuteScalar();
                    conn.Close();

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                ShowError("Database error: " + ex.Message);
                return false;
            }
        }

        private AdminDetails GetAdminDetails(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT AdminID, AdminName, Email FROM Admins WHERE Email = @email", conn);

                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new AdminDetails
                        {
                            AdminID = reader["AdminID"].ToString(),
                            AdminName = reader["AdminName"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                    }

                    conn.Close();
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "A";

            string[] parts = name.Split(' ');
            if (parts.Length >= 2)
            {
                return (parts[0][0].ToString() + parts[parts.Length - 1][0].ToString()).ToUpper();
            }
            else if (parts.Length == 1 && parts[0].Length > 0)
            {
                return parts[0][0].ToString().ToUpper();
            }

            return "A";
        }

        private void UpdateLastLogin(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Admins SET LastLogin = @lastLogin WHERE Email = @email", conn);

                    cmd.Parameters.AddWithValue("@lastLogin", DateTime.Now);
                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch
            {
                // Silently fail if update fails
            }
        }

        // Simple password hashing using SHA256 (must match the hashing in signup)
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return System.Convert.ToBase64String(hashedBytes);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            errorMessage.Attributes["class"] = "error-message show";
        }

        private void RedirectToAdminPanel()
        {
            Response.Redirect("AdminDashboard.aspx");
        }

    }

    // Helper class for admin details
    public class AdminDetails
    {
        public string AdminID { get; set; }
        public string AdminName { get; set; }
        public string Email { get; set; }
    }
}