using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Week1_Practical1.Helpers;

namespace Week1_Practical1.SuperAdmin
{
    public partial class ZinJaGODashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["IsAdminLoggedIn"] == null ||
                    Session["AdminRole"] == null ||
                    Session["AdminRole"].ToString() != "SuperAdmin")
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowError("Error in Page_Load of ZinJaGODashboard: " + ex.Message);
                Response.Redirect("~/Login.aspx");
            }
        }
        protected void ShowError(string message)
        {
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "popupError",
                $"alert('{message.Replace("'", "\\'")}');",
                true
            );
        }
    }
}