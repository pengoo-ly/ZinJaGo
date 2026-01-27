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

            string action = Request["action"];
            if (!string.IsNullOrEmpty(action))
            {
                HandleAjaxAction(action);
                return;
            }

            if (!IsPostBack)
            {
                LoadCoupons();
                LoadStatistics();
            }
        }

        private void HandleAjaxAction(string action)
        {
            Response.Clear();
            Response.Buffer = true;

            try
            {
                switch (action)
                {
                    case "get":
                        HandleGet();
                        break;
                    case "create":
                        HandleCreate();
                        break;
                    case "update":
                        HandleUpdate();
                        break;
                    case "delete":
                        HandleDelete();
                        break;
                }
            }
            finally
            {
                // ✅ Complete request safely
                HttpContext.Current.ApplicationInstance.CompleteRequest();
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

                DataRow row = Cupon.GetCoupon(id);

                if (row == null)
                {
                    RespondJson(new { success = false, message = "Not found" }, 404);
                    return;
                }

                RespondJson(new
                {
                    VoucherID = row["VoucherID"],
                    Code = row["Code"],
                    VoucherType = row["VoucherType"],
                    DiscountType = row["DiscountType"],
                    DiscountValue = row["DiscountValue"],
                    CoinCost = row["CoinCost"],
                    ExpiryDate = ((DateTime)row["ExpiryDate"]).ToString("yyyy-MM-dd"),
                    Status = row["Status"]
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

                bool ok = Cupon.CreateCoupon(
                    adminId,
                    Request.Form["code"],
                    Request.Form["voucherType"],
                    Request.Form["discountType"],
                    decimal.Parse(Request.Form["discountValue"]),
                    int.Parse(Request.Form["coinCost"]),
                    DateTime.Parse(Request.Form["expiryDate"]),
                    Request.Form["status"]
                );

                RespondJson(new
                {
                    success = ok,
                    message = ok ? "Coupon created" : "Create failed"
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

                bool ok = Cupon.UpdateCoupon(
                    adminId,
                    int.Parse(Request.Form["voucherId"]),
                    Request.Form["code"],
                    Request.Form["voucherType"],
                    Request.Form["discountType"],
                    decimal.Parse(Request.Form["discountValue"]),
                    int.Parse(Request.Form["coinCost"]),
                    DateTime.Parse(Request.Form["expiryDate"]),
                    Request.Form["status"]
                );

                RespondJson(new
                {
                    success = ok,
                    message = ok ? "Coupon updated" : "Update failed"
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
                int id = int.Parse(Request["id"]);
                bool ok = Cupon.DeleteCoupon(id, Convert.ToInt32(Session["AdminID"]));

                RespondJson(new
                {
                    success = ok,
                    message = ok ? "Deleted" : "Delete failed"
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
            DataTable dt = Cupon.GetCouponsByAdmin(adminId);
            rptCoupons.DataSource = dt;
            rptCoupons.DataBind();
        }


        private void LoadStatistics()
        {
            try
            {
                int adminId = Convert.ToInt32(Session["AdminID"]);
                DataRow r = Cupon.GetCouponStatisticsByAdmin(adminId);

                if (r != null)
                {
                    lblTotalCoupons.Text = r["TotalCoupons"].ToString();
                    lblActiveCoupons.Text = r["ActiveCoupons"].ToString();
                    lblTotalDiscount.Text = Convert.ToDecimal(r["TotalDiscount"]).ToString("0.00");
                    lblTimesUsed.Text = "0"; // placeholder unless you add usage table
                }
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
            string json = serializer.Serialize(data);
            Response.Write(json);

            // ❌ REMOVE THIS
            // Response.End();

            // ✅ ADD THIS
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

       

    }
}