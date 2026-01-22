using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Services;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected bool IsDarkMode { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Protect admin pages
                if (Session["IsAdminLoggedIn"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    string adminName = Session["AdminName"].ToString();
                    string adminEmail = Session["AdminEmail"].ToString();

                    lblAdminName.Text = adminName;
                    lblAdminEmail.Text = adminEmail;
                    lblAvatar.Text = GetInitials(adminName);
                    lblTopAvatar.Text = GetInitials(adminName);
                }

                // Prefer Page.Title (set in content pages). If not set, derive from file name.
                string title = Page.Title;
                if (string.IsNullOrWhiteSpace(title))
                {
                    string file = Path.GetFileName(Request.Path);
                    title = !string.IsNullOrEmpty(file) ? Path.GetFileNameWithoutExtension(file) : "Admin";
                }

                // Set the label so the master shows the page name at top
                lblPageTitle.Text = title;
                string themeCookie = Request.Cookies["adminThemeDark"]?.Value;
                IsDarkMode = themeCookie == "1";
            }
            catch (Exception)
            {
                // Log the exception (not shown here for brevity)
                // Optionally, display a user-friendly message or redirect to an error page
                lblPageTitle.Text = "Error loading page";
            }
        }

        /// <summary>
        /// Handle logout - called from JavaScript
        /// </summary>
        public void LogoutUser()
        {
            try
            {
                int adminId = 0;
                if (Session["AdminID"] != null && int.TryParse(Session["AdminID"].ToString(), out adminId))
                {
                    DbLogger.Log("Admin logout", adminId);
                }

                // Clear all session variables
                Session["IsAdminLoggedIn"] = null;
                Session["AdminID"] = null;
                Session["AdminName"] = null;
                Session["AdminRole"] = null;
                Session["AdminEmail"] = null;
                Session["AdminInitial"] = null;
                Session["AdminProfileImage"] = null;

                // Clear admin email cookie
                if (Request.Cookies["AdminEmail"] != null)
                {
                    HttpCookie cookie = new HttpCookie("AdminEmail");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                // Abandon session
                Session.Abandon();

                // Redirect to login page
                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            {
                DbLogger.Log("Admin logout error: " + ex.Message);
                Response.Redirect("Login.aspx");
            }
        }

        /// <summary>
        /// Get initials from admin name for avatar
        /// </summary>
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

        [WebMethod]
        public static List<string> SearchProducts(string query)
        {
            try
            {
                List<string> results = new List<string>();

                if (string.IsNullOrWhiteSpace(query))
                    return results;

                string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT TOP 5 ProductName FROM Products WHERE ProductName LIKE @q", con);
                    cmd.Parameters.AddWithValue("@q", "%" + query + "%");

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        results.Add(dr["ProductName"].ToString());
                    }
                }
                return results;
            }
            catch
            {
                return null;
            }
        }
    }
}