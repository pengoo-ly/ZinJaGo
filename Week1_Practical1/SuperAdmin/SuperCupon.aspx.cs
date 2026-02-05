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

namespace Week1_Practical1.SuperAdmin
{
    public partial class SuperCupon : System.Web.UI.Page
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
                Cupon c = new Cupon();
                c.AutoExpireCoupons();
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
        protected string GetCouponStatus(DateTime expiryDate, string status)
        {
            if (expiryDate.Date < DateTime.Today)
                return "Expired";

            return status;
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Text = "";
                gvCoupons.EditIndex = -1;
                LoadCoupons();
            }
            catch 
            {
                Response.Write("<script>alert('An error occurred while clearing the search.');</script>");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                gvCoupons.EditIndex = -1; // 🔑 VERY IMPORTANT

                string q = txtSearch.Text.Trim().ToLower();
                int adminId = Convert.ToInt32(Session["AdminID"]);

                var coupons = new Cupon()
                    .GetCouponsByAdmin(adminId)
                    .Where(c => c.Code.ToLower().Contains(q))
                    .ToList();

                gvCoupons.DataSource = coupons;
                gvCoupons.DataBind();
            }
            catch (Exception ex)
            {
                pnlCreateAlert.Visible = true;
                lblCreateAlert.Text = ex.Message;
            }
        }

        protected void btnOpenAdd_Click(object sender, EventArgs e)
        {
            try
            {
                pnlCreateCoupon.CssClass = "modal-panel";   // show create panel
                pnlEditCoupon.CssClass = "modal-panel hidden"; // hide edit panel

                ScriptManager.RegisterStartupScript(
                    this, GetType(), "showModal",
                    "document.getElementById('couponModal').classList.add('show');",
                    true
                );
            }
            catch
            {
                Response.Write("<script>alert('An error occurred while opening the add coupon modal.');</script>");
            }
        }

        protected void gvCoupons_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                e.Cancel = true;               // 🔑 STOP GridView edit mode
                gvCoupons.EditIndex = -1;      // 🔑 ENSURE no inline edit

                int id = Convert.ToInt32(gvCoupons.DataKeys[e.NewEditIndex].Value);
                Cupon c = new Cupon().GetCoupon(id);
                if (c == null) return;

                hfEditVoucherID.Value = c.VoucherID.ToString();
                txtEditCode.Text = c.Code.ToUpper();
                ddlEditVoucherType.SelectedValue = c.VoucherType;
                txtEditDiscount.Text = c.DiscountValue.ToString();
                txtEditCoin.Text = c.CoinCost?.ToString() ?? "";
                txtEditExpiry.Text = c.ExpiryDate.ToString("yyyy-MM-dd");
                ddlEditStatus.SelectedValue = c.Status;

                pnlCreateCoupon.CssClass = "modal-panel hidden"; // hide create
                pnlEditCoupon.CssClass = "modal-panel"; // show edit

                ScriptManager.RegisterStartupScript(
                    this, GetType(), "editModal",
                    "document.getElementById('couponModal').classList.add('show');",
                    true
                );
            }
            catch (Exception ex)
            {
                pnlEditAlert.Visible = true;
                lblEditAlert.Text = ex.Message;

                // 🔑 SHOW MODAL EVEN ON ERROR
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "editModalErr",
                    "document.getElementById('couponModal').classList.add('show');",
                    true
                );
            }
        }

        protected void gvCoupons_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                gvCoupons.EditIndex = -1;

                int id = Convert.ToInt32(gvCoupons.DataKeys[e.RowIndex].Value);
                new Cupon { VoucherID = id }.Delete();

                LoadCoupons();
                LoadStatistics();
            }
            catch (Exception ex)
            {
                pnlCreateAlert.Visible = true;
                lblCreateAlert.Text = ex.Message;
            }
        }

        protected void gvCoupons_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvCoupons.EditIndex = -1;
                LoadCoupons();
            }
            catch
            {
                Response.Write("<script>alert('An error occurred while cancelling the changes.');</script>");
            }
        }

        protected void btnCreateCoupon_Click(object sender, EventArgs e)
        {
            try
            {
                Cupon c = new Cupon
                {
                    Code = txtCreateCode.Text.Trim().ToUpper(),
                    VoucherType = ddlCreateVoucherType.SelectedValue,
                    DiscountType = "Fixed", // or Percentage if you add later
                    DiscountValue = decimal.Parse(txtCreateDiscount.Text),
                    CoinCost = string.IsNullOrEmpty(txtCreateCoin.Text) ? (int?)null : int.Parse(txtCreateCoin.Text),
                    ExpiryDate = DateTime.Parse(txtCreateExpiry.Text),
                    Status = "Active",
                    CreatedBy = Convert.ToInt32(Session["AdminID"])
                };

                c.Create();

                LoadCoupons();
                LoadStatistics();
            }
            catch (Exception ex)
            {
                pnlCreateAlert.Visible = true;
                lblCreateAlert.Text = ex.Message;
            }
        }

        protected void btnUpdateCoupon_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(hfEditVoucherID.Value);

                Cupon c = new Cupon
                {
                    VoucherID = id,
                    Code = txtEditCode.Text.Trim().ToUpper(),
                    VoucherType = ddlEditVoucherType.SelectedValue,
                    DiscountType = "Fixed",
                    DiscountValue = decimal.Parse(txtEditDiscount.Text),
                    CoinCost = string.IsNullOrEmpty(txtEditCoin.Text)
                                ? (int?)null
                                : int.Parse(txtEditCoin.Text),
                    ExpiryDate = DateTime.Parse(txtEditExpiry.Text),
                    Status = ddlEditStatus.SelectedValue
                };

                c.Update();

                LoadCoupons();
                LoadStatistics();
            }
            catch (Exception ex)
            {
                pnlEditAlert.Visible = true;
                lblEditAlert.Text = ex.Message;
            }
        }
    }
}