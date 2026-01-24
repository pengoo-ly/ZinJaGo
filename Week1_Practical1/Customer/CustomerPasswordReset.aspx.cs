using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class Customer_PasswordReset : System.Web.UI.Page
    {
        private const int RESET_CODE_EXPIRY_MINUTES = 30;

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

        protected void btnVerifyEmail_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string email = txtEmail.Text.Trim();

                // Check if email exists
                int userId = GetUserIdByEmail(email);
                if (userId <= 0)
                {
                    ShowError("Email not found in our system. Please check and try again.");
                    return;
                }

                // Generate reset code
                string resetCode = GenerateResetCode();

                // Store reset code in database
                if (StoreResetCode(userId, resetCode))
                {
                    Session["ResetUserID"] = userId;
                    Session["ResetEmail"] = email;
                    Session["ResetCode"] = resetCode;
                    Session["ResetCodeTime"] = DateTime.Now;

                    // In production, send email with reset code
                    SendResetEmail(email, resetCode);

                    ShowSuccess("Verification code sent to your email. Please check your inbox.");
                    emailPanel.Visible = false;
                    codePanel.Visible = true;
                    infoPanel.Visible = false;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "updateStep",
                        "updateStepIndicator(2);", true);

                    DbLogger.Log("Password reset code generated for email: " + email);
                }
                else
                {
                    ShowError("Failed to generate reset code. Please try again.");
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error verifying email: " + ex.Message);
                ShowError("An unexpected error occurred. Please try again.");
            }
        }

        protected void btnVerifyCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string enteredCode = txtResetCode.Text.Trim();
                string storedCode = Session["ResetCode"]?.ToString();
                object resetCodeTime = Session["ResetCodeTime"];

                if (string.IsNullOrEmpty(storedCode))
                {
                    ShowError("Session expired. Please start over.");
                    ResetForm();
                    return;
                }

                // Check if code has expired (30 minutes)
                if (resetCodeTime != null && resetCodeTime is DateTime)
                {
                    DateTime codeTime = (DateTime)resetCodeTime;
                    if (DateTime.Now.Subtract(codeTime).TotalMinutes > RESET_CODE_EXPIRY_MINUTES)
                    {
                        ShowError("Verification code has expired. Please request a new one.");
                        ResetForm();
                        return;
                    }
                }

                if (enteredCode != storedCode)
                {
                    ShowError("Invalid verification code. Please try again.");
                    return;
                }

                codePanel.Visible = false;
                passwordPanel.Visible = true;
                infoPanel.Visible = false;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "updateStep",
                    "updateStepIndicator(3);", true);

                ShowSuccess("Code verified! Now set your new password.");
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error verifying code: " + ex.Message);
                ShowError("An unexpected error occurred. Please try again.");
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string newPassword = txtNewPassword.Text.Trim();
                string confirmPassword = txtConfirmPassword.Text.Trim();

                if (newPassword != confirmPassword)
                {
                    ShowError("Passwords do not match.");
                    return;
                }

                int userId = (int?)Session["ResetUserID"] ?? 0;
                if (userId <= 0)
                {
                    ShowError("Session expired. Please start over.");
                    ResetForm();
                    return;
                }

                // Update password in database
                if (UpdateUserPassword(userId, newPassword))
                {
                    // Clear reset code from database
                    ClearResetCode(userId);

                    // Clear session
                    Session["ResetUserID"] = null;
                    Session["ResetEmail"] = null;
                    Session["ResetCode"] = null;
                    Session["ResetCodeTime"] = null;

                    ShowSuccess("Password reset successful! Redirecting to login...");
                    DbLogger.Log("Password reset successful for UserID: " + userId);

                    System.Threading.Thread.Sleep(2000);
                    Response.Redirect("CustomerLogin.aspx");
                }
                else
                {
                    ShowError("Failed to reset password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error resetting password: " + ex.Message);
                ShowError("An unexpected error occurred. Please try again.");
            }
        }

        private int GetUserIdByEmail(string email)
        {
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"SELECT UserID FROM Users WHERE Email = @Email AND Status = 'Active'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        conn.Open();

                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error getting user by email: " + ex.Message);
                return 0;
            }
        }

        private string GenerateResetCode()
        {
            // Generate a 6-digit random code
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private bool StoreResetCode(int userId, string resetCode)
        {
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // Create a table for password reset requests if it doesn't exist
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PasswordResetRequests')
                        CREATE TABLE PasswordResetRequests
                        (
                            ResetID INT PRIMARY KEY IDENTITY(1,1),
                            UserID INT NOT NULL,
                            ResetCode NVARCHAR(10) NOT NULL,
                            CreatedAt DATETIME NOT NULL,
                            ExpiresAt DATETIME NOT NULL,
                            IsUsed BIT NOT NULL DEFAULT 0,
                            FOREIGN KEY (UserID) REFERENCES Users(UserID)
                        )";

                    // First, create the table if it doesn't exist
                    using (SqlCommand createCmd = new SqlCommand(createTableQuery, conn))
                    {
                        conn.Open();
                        createCmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    // Now insert the reset code
                    string insertQuery = @"INSERT INTO PasswordResetRequests 
                                         (UserID, ResetCode, CreatedAt, ExpiresAt, IsUsed)
                                         VALUES (@UserID, @ResetCode, GETDATE(), DATEADD(MINUTE, @ExpiryMinutes, GETDATE()), 0)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@ResetCode", resetCode);
                        cmd.Parameters.AddWithValue("@ExpiryMinutes", RESET_CODE_EXPIRY_MINUTES);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error storing reset code: " + ex.Message);
                return false;
            }
        }

        private bool UpdateUserPassword(int userId, string newPassword)
        {
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"UPDATE Users 
                                    SET PasswordHash = @PasswordHash,
                                        DateCreated = GETDATE()
                                    WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PasswordHash", newPassword); // Note: In production, hash the password
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error updating password: " + ex.Message);
                return false;
            }
        }

        private bool ClearResetCode(int userId)
        {
            try
            {
                string connStr = ConfigurationManager
                    .ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"UPDATE PasswordResetRequests 
                                    SET IsUsed = 1 
                                    WHERE UserID = @UserID AND IsUsed = 0";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        return result >= 0; // Success even if no rows updated
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error clearing reset code: " + ex.Message);
                return false;
            }
        }

        private void SendResetEmail(string email, string resetCode)
        {
            try
            {
                // In production, implement actual email sending
                // For now, just log it or store in debug output
                DbLogger.Log("Reset code for " + email + ": " + resetCode);
                System.Diagnostics.Debug.WriteLine("Reset code for " + email + ": " + resetCode);

                // Example implementation using System.Net.Mail
                // You would need to configure these settings in Web.config
                /*
                using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                {
                    smtp.Host = ConfigurationManager.AppSettings["SmtpHost"];
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                    smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
                    smtp.Credentials = new System.Net.NetworkCredential(
                        ConfigurationManager.AppSettings["SmtpUsername"],
                        ConfigurationManager.AppSettings["SmtpPassword"]);

                    using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                    {
                        mail.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["EmailFrom"]);
                        mail.To.Add(email);
                        mail.Subject = "ZinJaGO Password Reset Code";
                        mail.Body = $"Your password reset code is: {resetCode}\n\nThis code will expire in {RESET_CODE_EXPIRY_MINUTES} minutes.";
                        mail.IsBodyHtml = false;

                        smtp.Send(mail);
                    }
                }
                */
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error sending reset email: " + ex.Message);
                // Don't throw - email failure shouldn't prevent password reset
            }
        }

        private void ResetForm()
        {
            // Clear all panels and reset to step 1
            emailPanel.Visible = true;
            codePanel.Visible = false;
            passwordPanel.Visible = false;
            infoPanel.Visible = true;

            txtEmail.Text = "";
            txtResetCode.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";

            Session["ResetUserID"] = null;
            Session["ResetEmail"] = null;
            Session["ResetCode"] = null;
            Session["ResetCodeTime"] = null;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "updateStep",
                "updateStepIndicator(1);", true);
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            errorMessage.Attributes["class"] = "error-message show";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideError",
                "setTimeout(function() { " +
                "var errorDiv = document.getElementById('errorMessage'); " +
                "if (errorDiv) { errorDiv.classList.remove('show'); } " +
                "}, 5000);", true);
        }

        private void ShowSuccess(string message)
        {
            lblSuccess.Text = message;
            successMessage.Attributes["class"] = "success-message show";
        }
    }
}