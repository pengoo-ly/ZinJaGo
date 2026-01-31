using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class CustomerSignup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Check if user is already logged in
                    if (Session["IsCustomerLoggedIn"] != null && (bool)Session["IsCustomerLoggedIn"])
                    {
                        Response.Redirect("~/Home.aspx");
                    }
                }
            }
            catch
            {
                ShowError("An error occurred. Please try again.");
            }
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();
                string confirmPassword = txtConfirmPassword.Text.Trim();

                DbLogger.Log($"[SIGNUP] Attempting to create account - Email: {email}, Name: {firstName} {lastName}");

                // Validate passwords match
                if (password != confirmPassword)
                {
                    ShowError("Passwords do not match.");
                    return;
                }

                // Check if email already exists
                if (EmailExists(email))
                {
                    ShowError("This email is already registered. Please use a different email or try logging in.");
                    return;
                }

                // Hash password
                string hashedPassword = HashPassword(password);

                // Create user account
                if (CreateUserAccount(firstName, lastName, email, hashedPassword))
                {
                    ShowSuccess("Account created successfully! Redirecting to login page...");
                    DbLogger.Log("New customer account created for email: " + email);
                    System.Threading.Thread.Sleep(2000);
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    ShowError("Failed to create account. Please check the logs for details.");
                }
            }
            catch (SqlException sqlEx)
            {
                DbLogger.Log("SIGNUP SQL Exception: " + sqlEx.Message);
                DbLogger.Log("SIGNUP SQL Error Number: " + sqlEx.Number);
                ShowError("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                DbLogger.Log("SIGNUP General Exception: " + ex.Message);
                DbLogger.Log("SIGNUP Stack Trace: " + ex.StackTrace);
                ShowError("An unexpected error occurred: " + ex.Message);
            }
        }

        private bool EmailExists(string email)
        {
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"SELECT COUNT(*) FROM Users WHERE Email = @Email";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        conn.Open();

                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error checking email existence: " + ex.Message);
                return false;
            }
        }

        private bool CreateUserAccount(string firstName, string lastName, string email, string hashedPassword)
        {
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // Use only core columns that should exist in Users table
                    string query = @"INSERT INTO Users 
                                    (Username, Email, PasswordHash, Status)
                                    VALUES (@Username, @Email, @PasswordHash, @Status)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        string username = firstName + " " + lastName;
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        cmd.Parameters.AddWithValue("@Status", "Active");

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (result > 0)
                        {
                            DbLogger.Log($"[SIGNUP] Account created: {email}");
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                DbLogger.Log($"[SIGNUP] SQL Error #{sqlEx.Number}: {sqlEx.Message.Substring(0, Math.Min(200, sqlEx.Message.Length))}");
                return false;
            }
            catch (Exception ex)
            {
                DbLogger.Log($"[SIGNUP] Error: {ex.GetType().Name} - {ex.Message.Substring(0, Math.Min(200, ex.Message.Length))}");
                return false;
            }
        }

        /// <summary>
        /// Hashes password using SHA256 in Base64 format (same as Login.aspx)
        /// </summary>
        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashedBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            errorMessage.Attributes["class"] = "error-message show";
        }

        private void ShowSuccess(string message)
        {
            lblSuccess.Text = message;
            successMessage.Attributes["class"] = "success-message show";
        }
    }
}