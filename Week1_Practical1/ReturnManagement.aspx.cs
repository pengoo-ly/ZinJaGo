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
                gvReturns.DataSource = returnHelper.GetAllReturns();
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
                int returnId = Convert.ToInt32(e.CommandArgument);

                // Safely get AdminID from Session
                if (Session["AdminID"] == null)
                {
                    throw new Exception("Admin session has expired. Please log in again.");
                }

                int adminId = Convert.ToInt32(Session["AdminID"]);

                if (e.CommandName == "Approve")
                {
                    returnHelper.UpdateStatus(returnId, "Approved", adminId);
                }
                else if (e.CommandName == "Reject")
                {
                    returnHelper.UpdateStatus(returnId, "Rejected", adminId);
                }
                else if (e.CommandName == "Processed")
                {
                    returnHelper.UpdateStatus(returnId, "Processed", adminId);
                }

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
                Return r = new Return();
                gvReturns.DataSource = r.SearchReturns(txtSearch.Text.Trim());
                gvReturns.DataBind();
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}