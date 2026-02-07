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

namespace Week1_Practical1.SuperAdmin
{
    public partial class ReportsAnalytics : System.Web.UI.Page
    {
        Report rpt = new Report();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboard();
                LoadCharts();
            }
        }
        private void LoadDashboard()
        {
            lblAOV.Text = "$" + rpt.GetAverageOrderValue().ToString("0.00");

            gvTopProducts.DataSource = rpt.GetTopProducts();
            gvTopProducts.DataBind();
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Parse(txtStart.Text);
            DateTime end = DateTime.Parse(txtEnd.Text);

            gvCategoryRevenue.DataSource =
                rpt.GetRevenueByCategory(start, end);
            gvCategoryRevenue.DataBind();
        }

        protected void btnExportCsv_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Parse(txtStart.Text);
            DateTime end = DateTime.Parse(txtEnd.Text);

            DataTable dt = rpt.GetOrdersByDateRange(start, end);

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
            // Revenue (current year)
            DataTable revenue = rpt.GetRevenueByMonth(DateTime.Now.Year);

            hfRevenueLabels.Value = string.Join(",", revenue.Rows
                .Cast<DataRow>().Select(r => "M" + r["Month"]));

            hfRevenueData.Value = string.Join(",", revenue.Rows
                .Cast<DataRow>().Select(r => r["Revenue"]));

            // Order status
            DataTable status = rpt.GetOrderStatusBreakdown();

            hfStatusLabels.Value = string.Join(",", status.Rows
                .Cast<DataRow>().Select(r => r["ShippingStatus"]));

            hfStatusData.Value = string.Join(",", status.Rows
                .Cast<DataRow>().Select(r => r["Total"]));
        }

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Parse(txtStart.Text);
            DateTime end = DateTime.Parse(txtEnd.Text);

            DataTable dt = rpt.GetOrdersByDateRange(start, end);

            Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(doc, ms);

            doc.Open();
            doc.Add(new Paragraph("Orders Report"));
            doc.Add(new Paragraph($"From {start:d} to {end:d}\n\n"));

            PdfPTable table = new PdfPTable(dt.Columns.Count);

            foreach (DataColumn col in dt.Columns)
                table.AddCell(new Phrase(col.ColumnName));

            foreach (DataRow row in dt.Rows)
                foreach (var item in row.ItemArray)
                    table.AddCell(new Phrase(item.ToString()));

            doc.Add(table);
            doc.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=report.pdf");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }
}