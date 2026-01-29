using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class AdminCupon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCoupons();
                LoadStatistics();
            }
        }

        private void LoadCoupons()
        {
            int adminId = Convert.ToInt32(Session["AdminID"]);
            gvCoupons.DataSource = new Cupon().GetCouponsByAdmin(adminId);
            gvCoupons.DataBind();
        }


        private void LoadStatistics()
        {
            try
            {
                int adminId = Convert.ToInt32(Session["AdminID"]);
                List<Cupon> coupons = new Cupon().GetCouponsByAdmin(adminId);

                lblTotalCoupons.Text = coupons.Count.ToString();
                lblActiveCoupons.Text = coupons.FindAll(c => c.Status == "Active").Count.ToString();
                decimal totalDiscount = 0;
                foreach (var c in coupons)
                    totalDiscount += c.DiscountValue;
                lblTotalDiscount.Text = totalDiscount.ToString("0.00");
                lblTimesUsed.Text = "0"; // placeholder if you have a usage table
            }
            catch { }
        }

        protected void btnSaveCoupon_Click(object sender, EventArgs e)
        {
            try
            {
                Cupon c = new Cupon
                {
                    Code = txtCode.Text,
                    VoucherType = ddlVoucherType.SelectedValue,
                    DiscountType = ddlDiscountType.SelectedValue,
                    DiscountValue = decimal.Parse(txtDiscountValue.Text),
                    CoinCost = string.IsNullOrEmpty(txtCoinCost.Text) ? (int?)null : int.Parse(txtCoinCost.Text),
                    Status = ddlStatus.SelectedValue
                };

                if (string.IsNullOrEmpty(hfVoucherID.Value))
                {
                    // Create
                    c.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                    c.Create();
                }
                else
                {
                    // Update
                    c.VoucherID = int.Parse(hfVoucherID.Value);
                    c.Update();
                }

                LoadCoupons();
                LoadStatistics();
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                lblAlert.Text = "Error: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "openCouponModal();", true);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                 txtSearch.Text = "";
                LoadCoupons();
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                lblAlert.Text = "Error: " + ex.Message;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string q = txtSearch.Text.Trim().ToLower();
                int adminId = Convert.ToInt32(Session["AdminID"]);

                var coupons = new Cupon().GetCouponsByAdmin(adminId)
                    .Where(c => c.Code.ToLower().Contains(q))
                    .ToList();

                gvCoupons.DataSource = coupons;
                gvCoupons.DataBind();
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                lblAlert.Text = "Search error: " + ex.Message;
            }
        }


        protected void btnOpenAdd_Click(object sender, EventArgs e)
        {
            hfVoucherID.Value = "";
            txtCode.Text = "";
            ddlVoucherType.SelectedIndex = 0;
            ddlDiscountType.SelectedIndex = 0;
            txtDiscountValue.Text = "";
            txtCoinCost.Text = "";
            ddlStatus.SelectedIndex = 0;

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "openCouponModal();", true);
        }

        protected void gvCoupons_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int voucherId = Convert.ToInt32(gvCoupons.DataKeys[e.NewEditIndex].Value);
                Cupon c = new Cupon().GetCoupon(voucherId);

                if (c == null) return;

                hfVoucherID.Value = c.VoucherID.ToString();
                txtCode.Text = c.Code;
                ddlVoucherType.SelectedValue = c.VoucherType;
                ddlDiscountType.SelectedValue = c.DiscountType;
                txtDiscountValue.Text = c.DiscountValue.ToString();
                txtCoinCost.Text = c.CoinCost.HasValue ? c.CoinCost.Value.ToString() : "";
                ddlStatus.SelectedValue = c.Status;

                ScriptManager.RegisterStartupScript(
                    this, GetType(), "ShowModal", "openCouponModal();", true
                );
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                lblAlert.Text = "Edit failed: " + ex.Message;
            }
        }

        protected void gvCoupons_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int voucherId = Convert.ToInt32(gvCoupons.DataKeys[e.RowIndex].Value);

                Cupon c = new Cupon { VoucherID = voucherId };
                c.Delete();

                LoadCoupons();
                LoadStatistics();
            }
            catch (Exception ex)
            {
                pnlAlert.Visible = true;
                lblAlert.Text = "Delete failed: " + ex.Message;
            }
        }

        protected void gvCoupons_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvCoupons.EditIndex = -1;
                LoadCoupons();
            }
            catch (Exception ex) 
            { 
                pnlAlert.Visible = true;
            }
        }
    }
}