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
                    string query = @"
                        SELECT 
                            p.ProductID AS SKU,
                            p.ProductName,
                            w.WarehouseName,
                            ISNULL(p.StockQuantity, 0) AS Quantity,
                            50 AS ReorderLevel,  -- Default reorder level
                            CASE 
                                WHEN p.StockQuantity <= 0 THEN 'Out of Stock'
                                WHEN p.StockQuantity <= 50 THEN 'Low Stock'
                                ELSE 'In Stock'
                            END AS Status,
                            ISNULL(FORMAT(p.DateCreated, 'yyyy-MM-dd'), CAST(GETDATE() AS DATE)) AS LastMovement,
                            'Warehouse A' AS Location
                        FROM Products p
                        LEFT JOIN Warehouses w ON 1=1
                        WHERE p.AdminID = @AdminID
                        ORDER BY p.ProductID";

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
                                    LastMovement = reader["LastMovement"].ToString(),
                                    Location = reader["Location"].ToString()
                                });
                            }
                        }
                    }
                }

                // If no data from database, use sample data for demonstration
                if (_warehouseData.Count == 0)
                {
                    LoadSampleData();
                }

                // Initialize filtered data with all data
                _filteredData = new List<WarehouseItem>(_warehouseData);
                CurrentFilter = "All";
            }
            catch (Exception ex)
            {
                DbLogger.Log("Error loading warehouse data: " + ex.Message);
                LoadSampleData();
            }
        }

        /// <summary>
        /// Load sample warehouse data for demonstration
        /// </summary>
        private void LoadSampleData()
        {
            _warehouseData = new List<WarehouseItem>
            {
                new WarehouseItem
                {
                    SKU = "SKU-001",
                    ProductName = "Wireless Bluetooth Headphones",
                    WarehouseName = "Main Warehouse",
                    Quantity = 245,
                    ReorderLevel = 50,
                    Status = "In Stock",
                    LastMovement = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
                    Location = "Aisle A-12"
                },
                new WarehouseItem
                {
                    SKU = "SKU-002",
                    ProductName = "USB-C Charging Cable",
                    WarehouseName = "Distribution Center",
                    Quantity = 15,
                    ReorderLevel = 100,
                    Status = "Low Stock",
                    LastMovement = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"),
                    Location = "Aisle B-05"
                },
                new WarehouseItem
                {
                    SKU = "SKU-003",
                    ProductName = "Mobile Phone Case",
                    WarehouseName = "Regional Warehouse",
                    Quantity = 0,
                    ReorderLevel = 75,
                    Status = "Out of Stock",
                    LastMovement = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd"),
                    Location = "Aisle C-08"
                },
                new WarehouseItem
                {
                    SKU = "SKU-004",
                    ProductName = "Portable Power Bank",
                    WarehouseName = "Main Warehouse",
                    Quantity = 120,
                    ReorderLevel = 40,
                    Status = "In Stock",
                    LastMovement = DateTime.Now.ToString("yyyy-MM-dd"),
                    Location = "Aisle A-18"
                },
                new WarehouseItem
                {
                    SKU = "SKU-005",
                    ProductName = "Screen Protector Set",
                    WarehouseName = "Distribution Center",
                    Quantity = 38,
                    ReorderLevel = 50,
                    Status = "Low Stock",
                    LastMovement = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd"),
                    Location = "Aisle B-02"
                }
            };

            _filteredData = new List<WarehouseItem>(_warehouseData);
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
            _filteredData = new List<WarehouseItem>(_warehouseData);
            _currentPage = 1;
            CurrentFilter = "All";
            SetActiveFilterButton(btnAllWarehouses);
            BindGridView();
        }

        /// <summary>
        /// Filter by low stock and out of stock items
        /// </summary>
        protected void FilterLowStock_Click(object sender, EventArgs e)
        {
            _filteredData = _warehouseData.Where(x => x.Status == "Low Stock" || x.Status == "Out of Stock").ToList();
            _currentPage = 1;
            CurrentFilter = "LowStock";
            SetActiveFilterButton(btnLowStock);
            BindGridView();
        }

        /// <summary>
        /// Filter by recent movements (last 3 days)
        /// </summary>
        protected void FilterMovements_Click(object sender, EventArgs e)
        {
            DateTime threeDaysAgo = DateTime.Now.AddDays(-3);
            _filteredData = _warehouseData.Where(x =>
            {
                if (DateTime.TryParse(x.LastMovement, out DateTime movementDate))
                {
                    return movementDate >= threeDaysAgo;
                }
                return false;
            }).ToList();
            _currentPage = 1;
            CurrentFilter = "Movements";
            SetActiveFilterButton(btnMovements);
            BindGridView();
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
            // Sort by quantity descending
            _filteredData = _filteredData.OrderByDescending(x => x.Quantity).ToList();
            _currentPage = 1;
            BindGridView();
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
            if (_currentPage > 1)
            {
                _currentPage--;
                BindGridView();
            }
        }

        /// <summary>
        /// Navigate to next page
        /// </summary>
        protected void NextPage_Click(object sender, EventArgs e)
        {
            int totalPages = _filteredData.Count > 0 ? (int)Math.Ceiling((double)_filteredData.Count / PAGE_SIZE) : 1;
            if (_currentPage < totalPages)
            {
                _currentPage++;
                BindGridView();
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