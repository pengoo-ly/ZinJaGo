using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class Customers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["AdminID"] == null)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                int adminId = Convert.ToInt32(Session["AdminID"]);
                Customer cust = new Customer();
                LoadStats(cust, adminId);
                LoadTable(cust, adminId);
                LoadChartByMonth(cust, adminId, DateTime.Now.Year, DateTime.Now.Month);
                BindYearMonth();
            }
        }
        protected void FilterChart(object sender, EventArgs e)
        {
            int adminId = Convert.ToInt32(Session["AdminID"]);
            int year = int.Parse(ddlYear.SelectedValue);
            int month = int.Parse(ddlMonth.SelectedValue);

            Customer cust = new Customer();
            LoadChartByMonth(cust, adminId, year, month);
        }
        private void BindYearMonth()
        {
            ddlYear.Items.Clear();
            ddlMonth.Items.Clear();

            int currentYear = DateTime.Now.Year;

            // Year dropdown (last 5 years)
            for (int y = currentYear; y >= currentYear - 4; y--)
            {
                ddlYear.Items.Add(new ListItem(y.ToString(), y.ToString()));
            }

            // Month dropdown
            for (int m = 1; m <= 12; m++)
            {
                ddlMonth.Items.Add(new ListItem(
                    new DateTime(2000, m, 1).ToString("MMMM"),
                    m.ToString()
                ));
            }

            ddlYear.SelectedValue = currentYear.ToString();
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
        }

        private void LoadStats(Customer cust, int adminId)
        {
            try
            {
                var stats = cust.GetCustomerStats(adminId);

                lblTotalCustomers.Text = stats.total.ToString();
                lblActiveCustomers.Text = stats.active.ToString();
                lblInactiveCustomers.Text = stats.inactive.ToString();
                lblTotalSpend.Text = stats.spend.ToString("0.00");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadStats ERROR: " + ex.Message);
            }
        }

        private void LoadTable(Customer cust, int adminId)
        {
            try
            {
                rptCustomers.DataSource = cust.GetCustomersByAdmin(adminId);
                rptCustomers.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadTable ERROR: " + ex.Message);
            }
        }

        private void LoadChart(Customer cust, int adminId)
        {
            try
            {
                var data = cust.GetCustomerChart(adminId);

                string labels = "['" + string.Join("','", data.Select(x => x.Day)) + "']";
                string values = "[" + string.Join(",", data.Select(x => x.Count)) + "]";

                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "custChart",
                    $"renderChart({labels}, {values});",
                    true
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadChart ERROR: " + ex.Message);
            }
        }

        private void LoadChartByMonth(Customer cust, int adminId, int year, int month)
        {
            try
            {
                var data = cust.GetCustomerChartByMonth(adminId, year, month);

                string labels = "['" + string.Join("','", data.Select(x => x.Day)) + "']";
                string values = "[" + string.Join(",", data.Select(x => x.Count)) + "]";

                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "custChart",
                    $"renderChart({labels}, {values});",
                    true
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadChartByMonth ERROR: " + ex.Message);
            }
        }

    }
}
