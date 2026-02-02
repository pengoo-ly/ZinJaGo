using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class ReturnManagement : System.Web.UI.Page
    {
        Return returnHelper = new Return();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadReturns();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        void LoadReturns()
        {
            try
            {
                if (Session["AdminID"] == null)
                    Response.Redirect("~/AdminLogin.aspx");

                int adminId = Convert.ToInt32(Session["AdminID"]);
                gvReturns.DataSource = returnHelper.GetAllReturns(adminId);
                gvReturns.DataBind();
            }
            catch (Exception ex)
            {
                // Log error if needed
                // Example: lblError.Text = "Failed to load return records."
                throw;
            }
        }

        protected void gvReturns_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (Session["AdminID"] == null)
                    throw new Exception("Admin session has expired. Please log in again.");

                int returnId = Convert.ToInt32(e.CommandArgument);
                int adminId = Convert.ToInt32(Session["AdminID"]);
                Return r = new Return();

                string statusToSet = "";

                switch (e.CommandName.ToUpper())
                {
                    case "APPROVE":
                        statusToSet = "Approved";
                        break;
                    case "REJECT":
                        statusToSet = "Rejected";
                        break;
                    case "PROCESSED":
                        statusToSet = "Processed";
                        break;
                    case "CANCELRETURN":
                        statusToSet = "Pending";
                        break;
                    default:
                        return; // unknown command
                }

                r.UpdateReturnWithAudit(returnId, statusToSet, adminId);
                LoadReturns(); // refresh table
            }
            catch (Exception ex)
            {
                // Log error if needed
                // Example: lblError.Text = ex.Message;
                throw;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["AdminID"] == null) return;

                int adminId = Convert.ToInt32(Session["AdminID"]);
                gvReturns.DataSource = returnHelper.SearchReturns(txtSearch.Text.Trim(), adminId);
                gvReturns.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error search: " + ex.Message + "');</script>");
            }
        }
    }
}