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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace Week1_Practical1.SuperAdmin
{
    public partial class ReportsAnalytics : System.Web.UI.Page
    {
        Report rpt = new Report();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindYearMonth();
                LoadDashboard();
                LoadCharts();

            }
        }
        private void LoadDashboard()
        {
            if (!int.TryParse(ddlYear.SelectedValue, out int year))
                year = DateTime.Now.Year;

            DateTime start = new DateTime(year, 1, 1);
            DateTime end = new DateTime(year, 12, 31);

            DataTable topProducts = rpt.GetTopProductsByYear(year);
            decimal totalRevenue = topProducts.AsEnumerable()
                                              .Sum(r => r.Field<decimal>("Revenue"));

            lblTotalRevenue.Text = "$" + totalRevenue.ToString("N2");
            lblTotalOrders.Text = rpt.GetTotalOrdersByYear(year).ToString();
            lblCompletedOrders.Text = rpt.GetCompletedOrdersByYear(year).ToString();
            decimal averageOrderValue = totalRevenue / Math.Max(1, rpt.GetTotalOrdersByYear(year));
            lblAOV.Text = "$" + averageOrderValue.ToString("N2");
            int totalOrders = rpt.GetTotalOrdersByYear(year);
            int completedOrders = rpt.GetCompletedOrdersByYear(year);
            decimal completionRate = totalOrders > 0 ? (decimal)completedOrders / totalOrders * 100 : 0;

            lblCompletionRate.Text = completionRate.ToString("N2") + "%";
            int selectedMonth = int.TryParse(ddlMonth.SelectedValue, out int m) ? m : 0;
            DateTime startDate = selectedMonth > 0
                                 ? new DateTime(year, selectedMonth, 1)
                                 : new DateTime(year, 1, 1);

            DateTime endDate = selectedMonth > 0
                               ? new DateTime(year, selectedMonth, DateTime.DaysInMonth(year, selectedMonth))
                               : new DateTime(year, 12, 31);

            DataTable customerData = rpt.GetNewVsRepeatCustomersByPeriod(startDate, endDate);

            int newCustomers = customerData.AsEnumerable()
                                           .Count(r => r.Field<string>("CustomerType") == "New");
            int repeatCustomers = customerData.AsEnumerable()
                                              .Count(r => r.Field<string>("CustomerType") == "Repeat");

            lblNewCustomers.Text = newCustomers.ToString();
            lblRepeatCustomers.Text = repeatCustomers.ToString();
            hfCustomerLabels.Value = "New,Repeat";
            hfCustomerData.Value = newCustomers + "," + repeatCustomers;
            DataTable categoryRevenue = rpt.GetRevenueByCategory(start, end);
            gvCategoryRevenue.DataSource = categoryRevenue;
            gvCategoryRevenue.DataBind();
            gvTopProducts.DataSource = rpt.GetTopProductsByYear(year);
            gvTopProducts.DataBind();
        }
        private void BindYearMonth()
        {
            ddlYear.Items.Clear();
            ddlMonth.Items.Clear();

            int currentYear = DateTime.Now.Year;

            // ---- YEAR ----
            for (int y = currentYear; y >= currentYear - 5; y--)
            {
                ddlYear.Items.Add(new ListItem(y.ToString(), y.ToString()));
            }

            // ✅ Select current year by default
            ddlYear.SelectedValue = currentYear.ToString();

            // ---- MONTH ----
            ddlMonth.Items.Add(new ListItem("All Months", "0"));

            for (int m = 1; m <= 12; m++)
            {
                ddlMonth.Items.Add(
                    new ListItem(
                        new DateTime(2000, m, 1).ToString("MMMM"),
                        m.ToString()
                    )
                );
            }

            // ✅ Default = All Months
            ddlMonth.SelectedValue = "0";
        }


        protected void btnExportCsv_Click(object sender, EventArgs e)
        {
            int year = int.Parse(ddlYear.SelectedValue);

            DataTable dt = rpt.GetOrdersByDateRange(
                new DateTime(year, 1, 1),
                new DateTime(year, 12, 31)
            );

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=orders.csv");

            foreach (DataColumn col in dt.Columns)
                Response.Write(col.ColumnName + ",");

            Response.Write("\n");

            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                    Response.Write(item.ToString().Replace(",", " ") + ",");
                Response.Write("\n");
            }

            Response.End();
        }
        private void LoadCharts()
        {
            // --- Get year & month safely ---
            if (!int.TryParse(ddlYear.SelectedValue, out int year))
                year = DateTime.Now.Year;

            if (!int.TryParse(ddlMonth.SelectedValue, out int month))
                month = 0;

            // --- Revenue Chart ---
            DataTable revenue = rpt.GetRevenueByMonthFromItems(year);

            DataTable categoryRevenue = rpt.GetRevenueByCategory(
                new DateTime(year, 1, 1),
                new DateTime(year, 12, 31)
            );

            hfCategoryLabels.Value = string.Join(",",
                categoryRevenue.AsEnumerable().Select(r => r["CategoryName"].ToString())
            );
            hfCategoryData.Value = string.Join(",",
                categoryRevenue.AsEnumerable().Select(r => r["Revenue"].ToString())
            );


            // Filter by month if selected
            if (month > 0)
            {
                var rows = revenue.AsEnumerable()
                                  .Where(r => r.Field<int>("Month") == month);
                revenue = rows.Any() ? rows.CopyToDataTable() : revenue.Clone();
            }

            // Fill hidden fields
            hfRevenueLabels.Value = string.Join(",",
                revenue.AsEnumerable()
                       .Select(r => new DateTime(2000, r.Field<int>("Month"), 1).ToString("MMM"))
            );

            hfRevenueData.Value = string.Join(",",
                revenue.AsEnumerable()
                       .Select(r => r["Revenue"].ToString())
            );

            // --- Order Status Chart ---
            DataTable status = rpt.GetOrderStatusBreakdownByYear(year);

            hfStatusLabels.Value = string.Join(",",
                status.AsEnumerable().Select(r => r["ShippingStatus"].ToString())
            );

            hfStatusData.Value = string.Join(",",
                status.AsEnumerable().Select(r => r["Total"].ToString())
            );
        }

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            int year = DateTime.Now.Year;

            Document doc = new Document(PageSize.A4, 36, 36, 36, 36);
            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(doc, ms);

            doc.Open();

            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            doc.Add(new Paragraph("Annual Revenue Report", titleFont));
            doc.Add(new Paragraph($"Year: {year}\n\n", bodyFont));

            doc.Add(new Paragraph($"Total Revenue: ${rpt.GetTotalRevenueByYear(year):N2}", bodyFont));
            doc.Add(new Paragraph($"Total Orders: {rpt.GetTotalOrdersByYear(year)}", bodyFont));
            doc.Add(new Paragraph($"Average Order Value: ${rpt.GetTopProductsByYear(year):N2}\n\n", bodyFont));

            DataTable orders = rpt.GetOrdersByDateRange(
                new DateTime(year, 1, 1),
                new DateTime(year, 12, 31));

            PdfPTable table = new PdfPTable(orders.Columns.Count);
            table.WidthPercentage = 100;

            foreach (DataColumn col in orders.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(col.ColumnName, headerFont));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell);
            }

            foreach (DataRow row in orders.Rows)
                foreach (var item in row.ItemArray)
                    table.AddCell(new Phrase(item.ToString(), bodyFont));

            doc.Add(table);
            doc.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=AnnualReport.pdf");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }

        protected void ddlFilter_Changed(object sender, EventArgs e)
        {
            LoadDashboard();
            LoadCharts();
        }
    }
}