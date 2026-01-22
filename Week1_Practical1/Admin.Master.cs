using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.Services;

namespace Week1_Practical1
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected bool IsDarkMode { get; private set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try {
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
                    lblAvatar.Text = adminName.Substring(0, 1).ToUpper();

                    // First letter for avatar
                    lblTopAvatar.Text = adminName.Substring(0, 1).ToUpper();
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
            catch { 
                return null;
            }
            
        }
    }
}