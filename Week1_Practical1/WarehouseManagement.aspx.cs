using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Week1_Practical1.Helpers;

namespace EcommerceWebsite
{
    public partial class WarehouseManagement : Page
    {
        private List<WarehouseItem> _warehouseData;
        private List<WarehouseItem> _filteredData;
        private int _currentPage = 1;
        private const int PAGE_SIZE = 10;
        private string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        // Store current filter state in ViewState
        private string CurrentFilter
        {
            get { return ViewState["CurrentFilter"] as string ?? "All"; }
            set { ViewState["CurrentFilter"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Check if admin is logged in
                if (Session["IsAdminLoggedIn"] == null || !(bool)Session["IsAdminLoggedIn"])
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                if (!IsPostBack)
                {
                    if (Session["AdminID"] == null)
                    {
                        Response.Redirect("~/Login.aspx");
                        return;
                    }

                    int adminId = Convert.ToInt32(Session["AdminID"]);
                    LoadWarehouseData(adminId);
                    BindMetrics();
                    BindGridView();
                    SetActiveFilterButton(btnAllWarehouses);
                }
                else
                {
                    // On postback, restore the filtered data from session
                    if (Session["WarehouseData"] != null)
                    {
                        _warehouseData = (List<WarehouseItem>)Session["WarehouseData"];
                        _filteredData = (List<WarehouseItem>)Session["FilteredData"];
                        _currentPage = int.Parse(ViewState["CurrentPage"]?.ToString() ?? "1");
                        BindMetrics();
                        BindGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error on WarehouseManagement page load: " + ex.Message);
            }
        }

        /// <summary>
        /// Load warehouse data from database filtered by admin's products
        /// </summary>
        private void LoadWarehouseData(int adminId)
        {
            try
            {
                _warehouseData = new List<WarehouseItem>();

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    // Load inventory from database - Products owned by this admin
                    // Also get warehouse info from InventoryTransactions and OrderShipments
                    string query = @"
                        SELECT DISTINCT
                            p.ProductID AS SKU,
                            p.ProductName,
                            ISNULL(w.WarehouseName, 'Main Warehouse') AS WarehouseName,
                            ISNULL(p.StockQuantity, 0) AS Quantity,
                            50 AS ReorderLevel,
                            CASE 
                                WHEN p.StockQuantity <= 0 THEN 'Out of Stock'
                                WHEN p.StockQuantity <= 50 THEN 'Low Stock'
                                ELSE 'In Stock'
                            END AS Status,
                            ISNULL(FORMAT(MAX(it.TransactionDate), 'yyyy-MM-dd'), CAST(GETDATE() AS DATE)) AS LastMovement,
                            ISNULL(w.Address, 'Warehouse A') AS Location
                        FROM Products p
                        LEFT JOIN InventoryTransactions it ON p.ProductID = it.ProductID
                        LEFT JOIN Warehouses w ON it.WarehouseID = w.WarehouseID
                        WHERE p.AdminID = @AdminID
                        GROUP BY 
                            p.ProductID,
                            p.ProductName,
                            p.StockQuantity,
                            w.WarehouseName,
                            w.Address
                        ORDER BY p.ProductName ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AdminID", adminId);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _warehouseData.Add(new WarehouseItem
                                {
                                    SKU = reader["SKU"].ToString(),
                                    ProductName = reader["ProductName"].ToString(),
                                    WarehouseName = reader["WarehouseName"] != DBNull.Value ? reader["WarehouseName"].ToString() : "Main Warehouse",
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    ReorderLevel = Convert.ToInt32(reader["ReorderLevel"]),
                                    Status = reader["Status"].ToString(),
                                    LastMovement = reader["LastMovement"] != DBNull.Value ? reader["LastMovement"].ToString() : DateTime.Now.ToString("yyyy-MM-dd"),
                                    Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : "Warehouse A"
                                });
                            }
                        }
                    }
                }

                // Initialize filtered data with all data (empty list if no products for this admin)
                _filteredData = new List<WarehouseItem>(_warehouseData);
                CurrentFilter = "All";

                // Store data in session for postback operations
                Session["WarehouseData"] = _warehouseData;
                Session["FilteredData"] = _filteredData;
                ViewState["CurrentPage"] = 1;
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error loading warehouse data: " + ex.Message);
                // Don't load sample data on error - keep warehouse empty for this admin
                _warehouseData = new List<WarehouseItem>();
                _filteredData = new List<WarehouseItem>();
                Session["WarehouseData"] = _warehouseData;
                Session["FilteredData"] = _filteredData;
            }
        }

        /// <summary>
        /// Bind metrics cards with warehouse statistics
        /// </summary>
        private void BindMetrics()
        {
            int totalItems = _warehouseData.Count;
            int inStock = _warehouseData.Count(x => x.Status == "In Stock");
            int lowStock = _warehouseData.Count(x => x.Status == "Low Stock");
            int outOfStock = _warehouseData.Count(x => x.Status == "Out of Stock");

            lblTotalItems.Text = totalItems.ToString();
            lblInStock.Text = inStock.ToString();
            lblLowStock.Text = lowStock.ToString();
            lblOutOfStock.Text = outOfStock.ToString();

            lblTotalItemsChange.Text = "↑ 12% Last 7 days";
            lblInStockChange.Text = "↑ 8% Last 7 days";
            lblLowStockChange.Text = lowStock > 0 ? $"↑ {lowStock} items" : "No low stock";
            lblOutOfStockChange.Text = outOfStock > 0 ? $"↑ {outOfStock} items" : "No out of stock";

            int uniqueWarehouses = _warehouseData.Select(x => x.WarehouseName).Distinct().Count();
            btnAllWarehouses.Text = $"All Warehouses ({uniqueWarehouses})";
        }

        /// <summary>
        /// Bind GridView with filtered warehouse data
        /// </summary>
        private void BindGridView()
        {
            var data = _filteredData
                .Skip((_currentPage - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();

            gvWarehouse.DataSource = data;
            gvWarehouse.DataBind();

            int totalPages = _filteredData.Count > 0 ? (int)Math.Ceiling((double)_filteredData.Count / PAGE_SIZE) : 1;
            lblPageInfo.Text = $"Page {_currentPage} of {totalPages} ({_filteredData.Count} items)";

            lbPrevious.Enabled = _currentPage > 1;
            lbNext.Enabled = _currentPage < totalPages;
        }

        /// <summary>
        /// GridView RowDataBound event - Set status badge styling
        /// </summary>
        protected void GvWarehouse_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = (WarehouseItem)e.Row.DataItem;
                HtmlGenericControl statusBadge = (HtmlGenericControl)e.Row.FindControl("statusBadge");

                if (statusBadge != null)
                {
                    statusBadge.InnerText = dataItem.Status;
                    statusBadge.Attributes["class"] = GetStatusBadgeClass(dataItem.Status);
                }
            }
        }

        /// <summary>
        /// Get CSS class for status badge based on status value
        /// </summary>
        private string GetStatusBadgeClass(string status)
        {
            switch (status)
            {
                case "In Stock":
                    return "status-badge status-in-stock";
                case "Low Stock":
                    return "status-badge status-low-stock";
                case "Out of Stock":
                    return "status-badge status-out-of-stock";
                default:
                    return "status-badge";
            }
        }

        /// <summary>
        /// Filter by all warehouses
        /// </summary>
        protected void FilterWarehouses_Click(object sender, EventArgs e)
        {
            try
            {
                _filteredData = new List<WarehouseItem>(_warehouseData);
                _currentPage = 1;
                CurrentFilter = "All";
                Session["FilteredData"] = _filteredData;
                ViewState["CurrentPage"] = _currentPage;
                SetActiveFilterButton(btnAllWarehouses);
                BindMetrics();
                BindGridView();
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error filtering warehouses: " + ex.Message);
            }
        }

        /// <summary>
        /// Filter by low stock and out of stock items
        /// </summary>
        protected void FilterLowStock_Click(object sender, EventArgs e)
        {
            try
            {
                if (_warehouseData != null && _warehouseData.Count > 0)
                {
                    _filteredData = _warehouseData.Where(x => x.Status == "Low Stock" || x.Status == "Out of Stock").ToList();
                }
                else
                {
                    _filteredData = new List<WarehouseItem>();
                }
                _currentPage = 1;
                CurrentFilter = "LowStock";
                Session["FilteredData"] = _filteredData;
                ViewState["CurrentPage"] = _currentPage;
                SetActiveFilterButton(btnLowStock);
                BindMetrics();
                BindGridView();
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error filtering low stock: " + ex.Message);
            }
        }

        /// <summary>
        /// Filter by recent movements (last 3 days)
        /// </summary>
        protected void FilterMovements_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime threeDaysAgo = DateTime.Now.AddDays(-3);
                if (_warehouseData != null && _warehouseData.Count > 0)
                {
                    _filteredData = _warehouseData.Where(x =>
                    {
                        if (DateTime.TryParse(x.LastMovement, out DateTime movementDate))
                        {
                            return movementDate >= threeDaysAgo;
                        }
                        return false;
                    }).ToList();
                }
                else
                {
                    _filteredData = new List<WarehouseItem>();
                }
                _currentPage = 1;
                CurrentFilter = "Movements";
                Session["FilteredData"] = _filteredData;
                ViewState["CurrentPage"] = _currentPage;
                SetActiveFilterButton(btnMovements);
                BindMetrics();
                BindGridView();
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error filtering movements: " + ex.Message);
            }
        }

        /// <summary>
        /// Set active state for filter buttons
        /// </summary>
        private void SetActiveFilterButton(Button activeButton)
        {
            btnAllWarehouses.CssClass = "filter-tabs";
            btnLowStock.CssClass = "filter-tabs";
            btnMovements.CssClass = "filter-tabs";
            activeButton.CssClass = "filter-tabs active";
        }

        /// <summary>
        /// Handle sorting
        /// </summary>
        protected void SortTable_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if currently sorted ascending or descending
                bool currentlySortedAsc = ViewState["SortAsc"] != null && (bool)ViewState["SortAsc"];
                
                if (currentlySortedAsc)
                {
                    // Sort descending
                    _filteredData = _filteredData.OrderByDescending(x => x.Quantity).ToList();
                    ViewState["SortAsc"] = false;
                }
                else
                {
                    // Sort ascending
                    _filteredData = _filteredData.OrderBy(x => x.Quantity).ToList();
                    ViewState["SortAsc"] = true;
                }

                _currentPage = 1;
                Session["FilteredData"] = _filteredData;
                ViewState["CurrentPage"] = _currentPage;
                BindMetrics();
                BindGridView();
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error sorting table: " + ex.Message);
            }
        }

        /// <summary>
        /// Show additional menu options
        /// </summary>
        protected void ShowMenu_Click(object sender, EventArgs e)
        {
            // Implementation for menu dropdown
            ScriptManager.RegisterStartupScript(this, this.GetType(), "menuClick",
                "alert('Export and additional features coming soon');", true);
        }

        /// <summary>
        /// Navigate to previous page
        /// </summary>
        protected void PreviousPage_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    ViewState["CurrentPage"] = _currentPage;
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error navigating to previous page: " + ex.Message);
            }
        }

        /// <summary>
        /// Navigate to next page
        /// </summary>
        protected void NextPage_Click(object sender, EventArgs e)
        {
            try
            {
                int totalPages = _filteredData.Count > 0 ? (int)Math.Ceiling((double)_filteredData.Count / PAGE_SIZE) : 1;
                if (_currentPage < totalPages)
                {
                    _currentPage++;
                    ViewState["CurrentPage"] = _currentPage;
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error navigating to next page: " + ex.Message);
            }
        }
    }

    /// <summary>
    /// Model class for warehouse inventory items
    /// </summary>
    [Serializable]
    public class WarehouseItem
    {
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public string Status { get; set; }
        public string LastMovement { get; set; }
        public string Location { get; set; }
    }
}