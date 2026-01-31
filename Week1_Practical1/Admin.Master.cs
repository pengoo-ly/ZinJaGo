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
                // Protect admin pages - check both null and false
                if (Session["IsAdminLoggedIn"] == null || (bool)Session["IsAdminLoggedIn"] == false)
                {
                    Response.Redirect("~/Login.aspx", true);
                    return;
                }

                if (!IsPostBack)
                {
                    string adminName = Session["AdminName"]?.ToString();
                    string adminEmail = Session["AdminEmail"]?.ToString();

                    // Validate session data exists
                    if (string.IsNullOrEmpty(adminName) || string.IsNullOrEmpty(adminEmail))
                    {
                        Response.Redirect("~/Login.aspx", true);
                        return;
                    }

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
            catch (Exception ex)
            {
                // Log the exception and redirect to login
                DbLogger.Log("Admin.Master Page_Load error: " + ex.Message);
                Response.Redirect("~/Login.aspx", true);
            }
        }

        /// <summary>
        /// Handle logout button click from the profile dropdown
        /// </summary>
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                int adminId = 0;
                if (Session["AdminID"] != null && int.TryParse(Session["AdminID"].ToString(), out adminId))
                {
                    DbLogger.Log("Admin logout", adminId);
                }

                // Prevent page caching FIRST - set cache headers before any redirects
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate, private");
                Response.AddHeader("Pragma", "no-cache");
                Response.AddHeader("Expires", "0");

                // Clear admin email cookie BEFORE session abandon
                if (Request.Cookies["AdminEmail"] != null)
                {
                    HttpCookie cookie = new HttpCookie("AdminEmail");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                // Clear all session variables explicitly
                Session.Clear();

                // Then abandon the entire session
                Session.Abandon();

                // Force a new session by setting a dummy value (this ensures old session is completely gone)
                Session["LoggedOut"] = true;

                // Clear the cookie from browser by setting empty and negative expiry
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    HttpCookie sessionCookie = new HttpCookie("ASP.NET_SessionId");
                    sessionCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(sessionCookie);
                }

                // Redirect to login page with absolute path and end response
                Response.Redirect("~/Login.aspx", true);
            }
            catch (Exception ex)
            {
                DbLogger.Log("Admin logout error: " + ex.Message);
                try
                {
                    // Even on error, try to clear and redirect
                    Session.Clear();
                    Session.Abandon();
                    Response.Redirect("~/Login.aspx", true);
                }
                catch
                {
                    // Last resort - just redirect
                    Response.Redirect("~/Login.aspx", true);
                }
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