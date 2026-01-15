using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Week1_Practical1
{
    public partial class ForgotPassword : System.Web.UI.Page
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

        protected void btnSendReset_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            // Validate email
            if (string.IsNullOrEmpty(email))
            {
                ShowError("Please enter your email address.");
                return;
            }

            if (!IsValidEmail(email))
            {
                ShowError("Please enter a valid email address.");
                return;
            }

            // Check if email exists in database
            if (!EmailExists(email))
            {
                ShowError("No account found with this email address.");
                return;
            }

            // Send reset email directly with a generic reset link
            if (SendResetEmail(email))
            {
                ShowSuccess("Password reset instructions have been sent to your email address. Please check your inbox.");
                txtEmail.Text = "";
            }
            else
            {
                ShowError("Failed to send reset email. Please check your email settings and try again later.");
            }
        }

        private bool SendResetEmail(string email)
        {
            try
            {
                // Get admin name for personalized email
                string adminName = GetAdminName(email);

                // Create reset link
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                string resetLink = baseUrl + "/ResetPassword.aspx?email=" + System.Web.HttpUtility.UrlEncode(email);

                // Create email message with new color scheme
                string subject = "Password Reset Request - ZinJaGO Admin";
                string body = @"
                    <html>
                    <head>
                        <style>
                            body { font-family: 'Segoe UI', Arial, sans-serif; line-height: 1.6; color: #333; }
                            .container { max-width: 600px; margin: 0 auto; padding: 20px; }
                            .header { background: linear-gradient(135deg, #4FA392 0%, #76B29F 100%); color: white; padding: 30px 20px; text-align: center; border-radius: 8px 8px 0 0; }
                            .header h1 { margin: 0; font-size: 28px; }
                            .content { background: #F8F5F0; padding: 30px 20px; border: 1px solid #E0D5C7; border-radius: 0 0 8px 8px; }
                            .content p { margin-bottom: 15px; color: #555; }
                            .button { display: inline-block; background: linear-gradient(135deg, #4FA392 0%, #76B29F 100%); color: white; padding: 14px 32px; text-decoration: none; border-radius: 6px; margin: 20px 0; font-weight: 600; }
                            .button:hover { opacity: 0.9; }
                            .footer { color: #999; font-size: 12px; margin-top: 20px; text-align: center; border-top: 1px solid #E0D5C7; padding-top: 15px; }
                            .warning { background: rgba(79, 163, 146, 0.1); color: #2d6a5c; font-size: 13px; padding: 12px; border-radius: 4px; margin-top: 15px; border-left: 4px solid #4FA392; }
                            .link-text { color: #4FA392; word-break: break-all; font-size: 12px; margin: 15px 0; padding: 10px; background: white; border-radius: 4px; }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>🔐 Password Reset Request</h1>
                            </div>
                            <div class='content'>
                                <p>Hello <strong>" + adminName + @"</strong>,</p>
                                <p>We received a request to reset your password for your ZinJaGO Admin account. If you didn't make this request, you can safely ignore this email.</p>
                                <p style='text-align: center;'>
                                    <a href='" + resetLink + @"' class='button'>🔑 Reset Your Password</a>
                                </p>
                                <p>Or copy and paste this link in your browser:</p>
                                <div class='link-text'>" + resetLink + @"</div>
                                <div class='warning'>
                                    <strong>⏰ Note:</strong> This link will take you directly to the password reset page. You can use it immediately to create a new password.
                                </div>
                                <p style='margin-top: 20px;'><strong>For your security:</strong></p>
                                <ul style='color: #555;'>
                                    <li>Never share your password reset link with anyone</li>
                                    <li>Make sure your new password is strong and unique</li>
                                    <li>If you didn't request this, please contact support immediately</li>
                                </ul>
                                <div class='footer'>
                                    <p>If you did not request a password reset, please ignore this email or contact our support team.</p>
                                    <p>&copy; 2025 ZinJaGO Admin. All rights reserved.</p>
                                    <p style='color: #bbb;'>This is an automated message, please do not reply to this email.</p>
                                </div>
                            </div>
                        </div>
                    </body>
                    </html>
                ";

                // Get email settings from Web.config
                string emailSender = ConfigurationManager.AppSettings["EmailSender"];
                string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
                string smtpPortStr = ConfigurationManager.AppSettings["SmtpPort"] ?? "587";

                // Validate configuration
                System.Diagnostics.Debug.WriteLine("=== Email Configuration Check ===");
                System.Diagnostics.Debug.WriteLine($"EmailSender: {(string.IsNullOrEmpty(emailSender) ? "❌ NOT SET" : "✓ " + emailSender)}");
                System.Diagnostics.Debug.WriteLine($"EmailPassword: {(string.IsNullOrEmpty(emailPassword) ? "❌ NOT SET" : "✓ SET (length: " + emailPassword.Length + ")")}");

                if (string.IsNullOrEmpty(emailSender) || string.IsNullOrEmpty(emailPassword))
                {
                    System.Diagnostics.Debug.WriteLine("❌ ERROR: Email settings not configured in Web.config");
                    return false;
                }

                if (!int.TryParse(smtpPortStr, out int smtpPort))
                {
                    smtpPort = 587;
                }

                System.Diagnostics.Debug.WriteLine($"Attempting to send email to: {email}");

                // Send email using SMTP
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.Host = smtpHost;
                    smtpClient.Port = smtpPort;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(emailSender, emailPassword);
                    smtpClient.Timeout = 20000;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(emailSender);
                        mailMessage.To.Add(email);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;

                        System.Diagnostics.Debug.WriteLine("Sending email...");
                        smtpClient.Send(mailMessage);
                        System.Diagnostics.Debug.WriteLine("✓ Email sent successfully!");
                    }
                }

                return true;
            }
            catch (SmtpException smtpEx)
            {
                System.Diagnostics.Debug.WriteLine("❌ SMTP Error: " + smtpEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("❌ Error sending email: " + ex.Message);
                return false;
            }
        }

        private string GetAdminName(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("SELECT AdminName FROM Admins WHERE Email = @email", conn);
                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();

                    return result != null ? result.ToString() : "Admin";
                }
            }
            catch
            {
                return "Admin";
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error checking email: " + ex.Message);
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