using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class Customer_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.Cookies["CustomerEmail"] != null)
                    {
                        txtEmail.Text = Request.Cookies["CustomerEmail"].Value;
                        chkRemember.Checked = true;
                    }
                    // Check if user is already logged in
                    if (Session["IsCustomerLoggedIn"] != null && (bool)Session["IsCustomerLoggedIn"])
                    {
                        RedirectToCustomerPanel();
                    }
                }
            }
            catch
            {
                ShowError("You have been logged out!");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                    return;

                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();

                // Remember email (optional)
                if (chkRemember.Checked)
                {
                    HttpCookie cookie = new HttpCookie("CustomerEmail", email);
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);
                }

                int userId;
                string userName;

                if (ValidateCredentials(email, password, out userId, out userName))
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
                    ShowError("Invalid email or password.");
                    txtPassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Customer login error: " + ex.Message);
                ShowError("An unexpected error occurred. Please try again.");
            }
        }

        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerPasswordReset.aspx");
        }

        private bool ValidateCredentials(string email, string password, out int userId, out string userName)
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

        private void ShowError(string message)
        {
            lblError.Text = message;
            errorMessage.Attributes["class"] = "error-message show";
        }

        private void RedirectToCustomerPanel()
        {
            try
            {
                Response.Redirect("~/Home.aspx");
            }
            catch (Exception)
            {
                ShowError("Please try reloading the page!");
            }
        }
    }
}