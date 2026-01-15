using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Week1_Practical1
{
    public partial class AdminSignUp : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is already logged in
                if (Session["IsAdminLoggedIn"] != null && (bool)Session["IsAdminLoggedIn"])
                {
                    Response.Redirect("AdminDashboard.aspx");
                }
            }
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // Validate inputs
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                ShowError("Please enter both first and last name.");
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                ShowError("Please enter an email address.");
                return;
            }

            if (!IsValidEmail(email))
            {
                ShowError("Please enter a valid email address.");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowError("Please enter a password.");
                return;
            }

            if (password.Length < 8)
            {
                ShowError("Password must be at least 8 characters long.");
                return;
            }

            if (!password.Equals(confirmPassword))
            {
                ShowError("Passwords do not match. Please try again.");
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                return;
            }

            // Check if email already exists
            if (EmailExists(email))
            {
                ShowError("This email address is already registered. Please login or use a different email.");
                return;
            }

            // Register admin
            if (RegisterAdmin(firstName, lastName, email, password))
            {
                ShowSuccess("Account created successfully! Redirecting to login...");
                ClearForm();
            }
        }

        private bool RegisterAdmin(string firstName, string lastName, string email, string password)
        {
            try
            {
                // Hash the password for security
                string passwordHash = HashPassword(password);

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    conn.Open();

                    // Get the next AdminID
                    int nextAdminId = GetNextAdminId(conn);

                    // Create admin name from first and last name
                    string adminName = firstName + " " + lastName;

                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Admins (AdminID, AdminName, Email, PasswordHash, Role, LastLogin) " +
                        "VALUES (@adminId, @adminName, @email, @passwordHash, @role, @lastLogin)", conn);

                    cmd.Parameters.AddWithValue("@adminId", nextAdminId);
                    cmd.Parameters.AddWithValue("@adminName", adminName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@role", "Admin");
                    cmd.Parameters.AddWithValue("@lastLogin", DateTime.Now);

                    int result = cmd.ExecuteNonQuery();
                    conn.Close();

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                ShowError("Error registering account: " + ex.Message);
                return false;
            }
        }

        private int GetNextAdminId(SqlConnection conn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(AdminID), 0) + 1 FROM Admins", conn);

                int nextId = (int)cmd.ExecuteScalar();
                return nextId;
            }
            catch
            {
                return 1;
            }
        }

        private bool EmailExists(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Admins WHERE Email = @email", conn);
                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    int result = (int)cmd.ExecuteScalar();
                    conn.Close();

                    return result > 0;
                }
            }
            catch
            {
                return false;
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

        // Simple password hashing using SHA256
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return System.Convert.ToBase64String(hashedBytes);
            }
        }

        private void ClearForm()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            errorMessage.Attributes["class"] = "error-message show";
            successMessage.Attributes["class"] = "success-message";
        }

        private void ShowSuccess(string message)
        {
            lblSuccess.Text = message;
            successMessage.Attributes["class"] = "success-message show";
            errorMessage.Attributes["class"] = "error-message";
        }
    }
}