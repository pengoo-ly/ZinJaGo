using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace Week1_Practical1
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string email = Request.QueryString["email"];

                if (string.IsNullOrEmpty(email))
                {
                    ShowError("Invalid reset link. Please request a new password reset.");
                    btnResetPassword.Enabled = false;
                    return;
                }

                // Validate email exists
                if (!EmailExists(email))
                {
                    ShowError("This email address is not registered in our system.");
                    btnResetPassword.Enabled = false;
                    return;
                }

                // Store email in view state for later use
                ViewState["AdminEmail"] = email;
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string email = ViewState["AdminEmail"] != null ? ViewState["AdminEmail"].ToString() : "";
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // Validate inputs
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                ShowError("Please enter both password fields.");
                return;
            }

            if (newPassword.Length < 8)
            {
                ShowError("Password must be at least 8 characters long.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                ShowError("Passwords do not match. Please try again.");
                txtNewPassword.Text = "";
                txtConfirmPassword.Text = "";
                return;
            }

            // Reset password
            if (ResetPasswordInDatabase(email, newPassword))
            {
                ShowSuccess("Your password has been reset successfully. Redirecting to login...");
                btnResetPassword.Enabled = false;
                
                // Redirect immediately (the success message will display briefly before redirect)
                Response.Redirect("AdminLogin.aspx");
            }
            else
            {
                ShowError("An error occurred while resetting your password. Please try again.");
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

        private bool ResetPasswordInDatabase(string email, string newPassword)
        {
            try
            {
                string passwordHash = HashPassword(newPassword);

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Admins SET PasswordHash = @passwordHash WHERE Email = @email", conn);

                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    conn.Close();

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error resetting password: " + ex.Message);
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return System.Convert.ToBase64String(hashedBytes);
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            errorMessage.Attributes["class"] = "message-alert error show";
            successMessage.Attributes["class"] = "message-alert success";
        }

        private void ShowSuccess(string message)
        {
            lblSuccess.Text = message;
            successMessage.Attributes["class"] = "message-alert success show";
            errorMessage.Attributes["class"] = "message-alert error";
        }
    }
}