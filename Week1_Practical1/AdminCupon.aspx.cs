using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
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


        private void HandleGet()
        {
            try
            {
                if (!int.TryParse(Request["id"], out int id))
                {
                    RespondJson(new { success = false, message = "Invalid ID" }, 400);
                    return;
                }

                Cupon coupon = new Cupon().GetCoupon(id);

                if (coupon == null)
                {
                    RespondJson(new { success = false, message = "Coupon not found" }, 404);
                    return;
                }

                RespondJson(new
                {
                    VoucherID = coupon.VoucherID,
                    Code = coupon.Code,
                    VoucherType = coupon.VoucherType,
                    DiscountType = coupon.DiscountType,
                    DiscountValue = coupon.DiscountValue,
                    CoinCost = coupon.CoinCost,
                    ExpiryDate = coupon.ExpiryDate.ToString("yyyy-MM-dd"),
                    Status = coupon.Status
                });
            }
            catch (Exception ex)
            {
                RespondJson(new { success = false, message = ex.Message }, 500);
            }
        }


        private void HandleCreate()
        {
            try
            {
                int adminId = Convert.ToInt32(Session["AdminID"]);

                Cupon coupon = new Cupon
                {
                    Code = Request.Form["code"],
                    VoucherType = Request.Form["voucherType"],
                    DiscountType = Request.Form["discountType"],
                    DiscountValue = decimal.Parse(Request.Form["discountValue"]),
                    CoinCost = string.IsNullOrEmpty(Request.Form["coinCost"]) ? (int?)null: int.Parse(Request.Form["coinCost"]),
                    ExpiryDate = DateTime.Parse(Request.Form["expiryDate"]),
                    Status = Request.Form["status"],
                    CreatedBy = adminId
                };

                bool ok = coupon.Create();

                RespondJson(new
                {
                    success = ok,
                    message = ok ? "Coupon created successfully" : "Create failed"
                });
            }
            catch (Exception ex)
            {
                RespondJson(new { success = false, message = ex.Message }, 500);
            }
        }


        private void HandleUpdate()
        {
            try
            {
                int adminId = Convert.ToInt32(Session["AdminID"]);

                Cupon coupon = new Cupon
                {
                    VoucherID = int.Parse(Request.Form["voucherId"]),
                    Code = Request.Form["code"],
                    VoucherType = Request.Form["voucherType"],
                    DiscountType = Request.Form["discountType"],
                    DiscountValue = decimal.Parse(Request.Form["discountValue"]),
                    CoinCost = string.IsNullOrEmpty(Request.Form["coinCost"]) ? (int?)null : int.Parse(Request.Form["coinCost"]),
                    ExpiryDate = DateTime.Parse(Request.Form["expiryDate"]),
                    Status = Request.Form["status"]
                };

                bool ok = coupon.Update();

                RespondJson(new
                {
                    success = ok,
                    message = ok ? "Coupon updated successfully" : "Update failed"
                });
            }
            catch (Exception ex)
            {
                RespondJson(new { success = false, message = ex.Message }, 500);
            }
        }


        private void HandleDelete()
        {
            try
            {
                if (!int.TryParse(Request["id"], out int id))
                {
                    RespondJson(new { success = false, message = "Invalid ID" }, 400);
                    return;
                }

                Cupon coupon = new Cupon { VoucherID = id };
                bool ok = coupon.Delete();

                RespondJson(new
                {
                    success = ok,
                    message = ok ? "Coupon deleted successfully" : "Delete failed"
                });
            }
            catch (Exception ex)
            {
                RespondJson(new { success = false, message = ex.Message }, 500);
            }
        }


        private void LoadCoupons()
        {
            int adminId = Convert.ToInt32(Session["AdminID"]);
            List<Cupon> coupons = new Cupon().GetCouponsByAdmin(adminId);

            rptCoupons.DataSource = coupons;
            rptCoupons.DataBind();
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


        private void RespondJson(object data, int statusCode = 200)
        {
            Response.Clear();
            Response.StatusCode = statusCode;
            Response.ContentType = "application/json";
            Response.TrySkipIisCustomErrors = true;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Response.Write(serializer.Serialize(data));
        }




    }
}