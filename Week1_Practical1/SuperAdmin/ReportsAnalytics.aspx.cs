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
    public partial class ReportsAnalytics : System.Web.UI.Page
    {
        Report rpt = new Report();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboard();
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
    }
}