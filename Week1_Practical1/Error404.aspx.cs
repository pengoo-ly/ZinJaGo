using System;

namespace YourProjectName
{
    public partial class Error404 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set the status code to 404 so search engines know it's a dead link
            Response.StatusCode = 404;
            Response.StatusDescription = "Page Not Found";
        }
    }
}