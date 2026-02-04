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
    public partial class DeliveryTrackingAdmin : System.Web.UI.Page
    {
        Delivery delivery = new Delivery();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindDeliveryGrid();
            }

        }

        private void BindDeliveryGrid()
        {
            try
            {
                if (Session["AdminID"] == null)
                    throw new Exception("Session expired. Please log in again.");

                int adminID = Convert.ToInt32(Session["AdminID"]);
                gvDelivery.DataSource = delivery.GetDeliveryByAdmin(adminID);
                gvDelivery.DataBind();
            }
            catch
            {
                Response.Write("<script>alert('Failed to load delivery data.');</script>");
            }
        }

        protected void gvDelivery_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvDelivery.EditIndex = e.NewEditIndex;
                BindDeliveryGrid();
            }
            catch
            {
                Response.Write("<script>alert('Unable to edit delivery record.');</script>");
            }
        }

        protected void gvDelivery_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int trackingID = Convert.ToInt32(gvDelivery.DataKeys[e.RowIndex].Value);

                TextBox txtStatus =
                    (TextBox)gvDelivery.Rows[e.RowIndex].FindControl("txtStatus");

                TextBox txtLocation =
                    (TextBox)gvDelivery.Rows[e.RowIndex].FindControl("txtLocation");

                if (txtStatus == null || txtLocation == null)
                    throw new Exception("Input fields not found.");

                delivery.UpdateDelivery(
                    trackingID,
                    txtStatus.Text.Trim(),
                    txtLocation.Text.Trim()
                );

                gvDelivery.EditIndex = -1;
                BindDeliveryGrid();
            }
            catch
            {
                Response.Write("<script>alert('Failed to update delivery details.');</script>");
            }
        }

        protected void gvDelivery_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvDelivery.EditIndex = -1;
                BindDeliveryGrid();
            }
            catch
            {
                Response.Write("<script>alert('Unable to cancel edit mode.');</script>");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["AdminID"] == null)
                    throw new Exception("Session expired.");

                int adminID = Convert.ToInt32(Session["AdminID"]);

                gvDelivery.DataSource = delivery.SearchDeliveryByAdmin(
                    adminID,
                    txtShipmentID.Text.Trim(),
                    ddlStatus.SelectedValue
                );

                gvDelivery.DataBind();
            }
            catch 
            {
                Response.Write("<script>alert('Failed to search delivery records.');</script>");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtShipmentID.Text = "";
                ddlStatus.SelectedIndex = 0;

                gvDelivery.EditIndex = -1;
                BindDeliveryGrid();
            }
            catch
            {
                Response.Write("<script>alert('Failed to reset search filters.');</script>");
            }
        }

        protected void gvDelivery_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateStatus")
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                int trackingID = Convert.ToInt32(args[0]);
                string newStatus = args[1];

                delivery.UpdateDelivery(trackingID, newStatus, ""); // keep location unchanged

                BindDeliveryGrid();
            }
        }
        protected string GetStatusClass(string status)
        {
            return status switch
            {
                "Packed" => "processed",
                "Shipped" => "processed",
                "Out for Delivery" => "pending",
                "Delivered" => "approved",
                _ => "pending"
            };
        }

    }
}