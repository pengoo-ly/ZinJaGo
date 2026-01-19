using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

                // Create user account
                if (CreateUserAccount(firstName, lastName, email, password))
                {
                    ShowSuccess("Account created successfully! Redirecting to login page...");
                    DbLogger.Log("New customer account created for email: " + email);
                    System.Threading.Thread.Sleep(2000);
                    Response.Redirect("CustomerLogin.aspx");
                }
                else
                {
                    ShowError("Failed to create account. Please try again.");
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Customer signup error: " + ex.Message);
                ShowError("An unexpected error occurred. Please try again.");
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

        private bool CreateUserAccount(string firstName, string lastName, string email, string password)
        {
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"INSERT INTO Users 
                                    (Username, Email, PasswordHash, Role, DateCreated, Status)
                                    VALUES (@Username, @Email, @PasswordHash, @Role, GETDATE(), @Status)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        string username = firstName + " " + lastName;
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", password); // Note: In production, hash the password
                        cmd.Parameters.AddWithValue("@Role", "Customer");
                        cmd.Parameters.AddWithValue("@Status", "Active");

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error creating user account: " + ex.Message);
                return false;
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