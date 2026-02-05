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
    public partial class CourierManagement : System.Web.UI.Page
    {
        Courier c = new Courier();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindGrid();
        }
        void BindGrid()
        {
            gvCourier.DataSource = c.GetAllCouriers();
            gvCourier.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Courier newCourier = new Courier();
                newCourier.CourierID = newCourier.GetNextCourierID();
                newCourier.CourierName = txtName.Text;
                newCourier.ContactNumber = txtContact.Text;
                newCourier.Email = txtEmail.Text;
                newCourier.TrackingURL = txtURL.Text;
                newCourier.IsPartnered = chkPartnered.Checked;

                newCourier.InsertCourier();
                BindGrid();
            }
            catch
            {
                Response.Write("<script>alert('Failed to add new courier.');</script>");
            }
        }

        protected void gvCourier_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvCourier.EditIndex = e.NewEditIndex;
                BindGrid();
            }
            catch 
            {
                Response.Write("<script>alert('Failed to enter edit mode.');</script>");
            }
        }

        protected void gvCourier_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvCourier.EditIndex = -1;
                BindGrid();
            }
            catch
            {
                Response.Write("<script>alert('Failed to cancel edit.');</script>");
            }
        }

        protected void gvCourier_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvCourier.DataKeys[e.RowIndex].Value);

                Courier update = new Courier();
                update.CourierID = id;
                update.CourierName = ((TextBox)gvCourier.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
                update.ContactNumber = ((TextBox)gvCourier.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
                update.Email = ((TextBox)gvCourier.Rows[e.RowIndex].Cells[3].Controls[0]).Text;
                update.TrackingURL = ((TextBox)gvCourier.Rows[e.RowIndex].Cells[4].Controls[0]).Text;
                update.IsPartnered = ((CheckBox)gvCourier.Rows[e.RowIndex].Cells[5].Controls[0]).Checked;

                update.UpdateCourier();
                gvCourier.EditIndex = -1;
                BindGrid();
            }
            catch 
            {
                Response.Write("<script>alert('Failed to update courier details.');</script>");
            }
        }

        protected void gvCourier_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvCourier.DataKeys[e.RowIndex].Value);
                c.DeleteCourier(id);
                BindGrid();
            }
            catch 
            {
                Response.Write("<script>alert('Failed to delete courier.');</script>");
            }
        }
    }
}