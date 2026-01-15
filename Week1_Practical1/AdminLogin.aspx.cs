using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Week1_Practical1.Helpers;
using Week1_Practical1.SuperAdmin;

namespace Week1_Practical1
{
    public partial class Admin_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.Cookies["AdminEmail"] != null)
                    {
                        txtEmail.Text = Request.Cookies["AdminEmail"].Value;
                        chkRemember.Checked = true;
                    }
                    // Check if user is already logged in
                    if (Session["IsAdminLoggedIn"] != null && (bool)Session["IsAdminLoggedIn"])
                    {
                        RedirectToAdminPanel();
                    }
                }
            }
            catch {
                ShowError("You are log out!");
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
                    HttpCookie cookie = new HttpCookie("AdminEmail", email);
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);
                }

                int adminId;
                string adminName;
                string role;

                if (ValidateCredentials(email, password, out adminId, out adminName, out role))
                {
                    Session["IsAdminLoggedIn"] = true;
                    Session["AdminID"] = adminId;
                    Session["AdminName"] = adminName;
                    Session["AdminRole"] = role;
                    Session["AdminEmail"] = email;

                    UpdateLastLogin(adminId);
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
                    ShowError("Invalid email or password.");
                    txtPassword.Text = "";
                }
            }
            catch(Exception ex)
            {
                DbLogger.Log("Admin login error: " + ex.Message);
                ShowError("An unexpected error occurred. Please try again.");
            }
        }

        private bool ValidateCredentials(string email, string password, out int adminId, out string adminName, out string role)
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
            catch
            {
                ShowError("You are log out!");
            }
            return false;
        }

        private void UpdateLastLogin(int adminId)
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
            catch {
                ShowError("You have been log out!");
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
            try
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
            catch (Exception) {
                ShowError("Please try reloading the page!");
            }
        }
    }
}