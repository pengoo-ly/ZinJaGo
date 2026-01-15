using System;
using System.IO;

namespace Week1_Practical1
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Prefer Page.Title (set in content pages). If not set, derive from file name.
            string title = Page.Title;
            if (string.IsNullOrWhiteSpace(title))
            {
                string file = Path.GetFileName(Request.Path);
                title = !string.IsNullOrEmpty(file) ? Path.GetFileNameWithoutExtension(file) : "Admin";
            }

            // Set the label so the master shows the page name at top
            lblPageTitle.Text = title;
        }
    }
}