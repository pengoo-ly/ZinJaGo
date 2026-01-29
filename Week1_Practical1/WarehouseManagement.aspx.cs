using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace EcommerceWebsite
{
    public partial class WarehouseManagement : Page
    {
        private List<WarehouseItem> _warehouseData;
        private int _currentPage = 1;
        private const int PAGE_SIZE = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWarehouseData();
                BindMetrics();
                BindGridView();
                SetActiveFilterButton(btnAllWarehouses);
            }
        }

        /// <summary>
        /// Load sample warehouse data - Replace with actual database query
        /// </summary>
        private void LoadWarehouseData()
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
                },
                new WarehouseItem
                {
                    SKU = "SKU-006",
                    ProductName = "Tablet Stand",
                    WarehouseName = "Regional Warehouse",
                    Quantity = 89,
                    ReorderLevel = 30,
                    Status = "In Stock",
                    LastMovement = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
                    Location = "Aisle C-14"
                },
                new WarehouseItem
                {
                    SKU = "SKU-007",
                    ProductName = "Wireless Mouse",
                    WarehouseName = "Main Warehouse",
                    Quantity = 156,
                    ReorderLevel = 60,
                    Status = "In Stock",
                    LastMovement = DateTime.Now.AddHours(-12).ToString("yyyy-MM-dd"),
                    Location = "Aisle A-22"
                },
                new WarehouseItem
                {
                    SKU = "SKU-008",
                    ProductName = "Mechanical Keyboard",
                    WarehouseName = "Distribution Center",
                    Quantity = 0,
                    ReorderLevel = 40,
                    Status = "Out of Stock",
                    LastMovement = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"),
                    Location = "Aisle B-11"
                },
                new WarehouseItem
                {
                    SKU = "SKU-009",
                    ProductName = "HDMI Cable 6ft",
                    WarehouseName = "Regional Warehouse",
                    Quantity = 320,
                    ReorderLevel = 100,
                    Status = "In Stock",
                    LastMovement = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"),
                    Location = "Aisle C-03"
                },
                new WarehouseItem
                {
                    SKU = "SKU-010",
                    ProductName = "Desk Lamp LED",
                    WarehouseName = "Main Warehouse",
                    Quantity = 25,
                    ReorderLevel = 20,
                    Status = "Low Stock",
                    LastMovement = DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd"),
                    Location = "Aisle A-09"
                },
                new WarehouseItem
                {
                    SKU = "SKU-011",
                    ProductName = "Phone Tripod",
                    WarehouseName = "Distribution Center",
                    Quantity = 87,
                    ReorderLevel = 35,
                    Status = "In Stock",
                    LastMovement = DateTime.Now.AddHours(-6).ToString("yyyy-MM-dd"),
                    Location = "Aisle B-16"
                },
                new WarehouseItem
                {
                    SKU = "SKU-012",
                    ProductName = "Camera Lens Cloth",
                    WarehouseName = "Regional Warehouse",
                    Quantity = 5,
                    ReorderLevel = 25,
                    Status = "Low Stock",
                    LastMovement = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd"),
                    Location = "Aisle C-20"
                }
            };
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

            // Calculate changes (sample data - replace with actual calculation)
            lblTotalItemsChange.Text = "↑ 12% Last 7 days";
            lblInStockChange.Text = "↑ 8% Last 7 days";
            lblLowStockChange.Text = "↑ 2% Last 7 days";
            lblOutOfStockChange.Text = "↑ 1 Last 7 days";

            // Update filter button with warehouse count
            int uniqueWarehouses = _warehouseData.Select(x => x.WarehouseName).Distinct().Count();
            btnAllWarehouses.Text = $"All Warehouses ({uniqueWarehouses})";
        }

        /// <summary>
        /// Bind GridView with warehouse data
        /// </summary>
        private void BindGridView()
        {
            var data = _warehouseData
                .Skip((_currentPage - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();

            gvWarehouse.DataSource = data;
            gvWarehouse.DataBind();

            // Update pagination info
            int totalPages = (int)Math.Ceiling((double)_warehouseData.Count / PAGE_SIZE);
            lblPageInfo.Text = $"Page {_currentPage} of {totalPages}";

            // Enable/disable pagination buttons
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
            _currentPage = 1;
            SetActiveFilterButton(btnAllWarehouses);
            BindGridView();
        }

        /// <summary>
        /// Filter by low stock items
        /// </summary>
        protected void FilterLowStock_Click(object sender, EventArgs e)
        {
            _warehouseData = _warehouseData.Where(x => x.Status == "Low Stock" || x.Status == "Out of Stock").ToList();
            _currentPage = 1;
            SetActiveFilterButton(btnLowStock);
            BindGridView();
            LoadWarehouseData(); // Reload for next filter
        }

        /// <summary>
        /// Filter by recent movements
        /// </summary>
        protected void FilterMovements_Click(object sender, EventArgs e)
        {
            _currentPage = 1;
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
            // Implementation for sort functionality
            // Could sort by quantity, SKU, product name, etc.
        }

        /// <summary>
        /// Show additional menu options
        /// </summary>
        protected void ShowMenu_Click(object sender, EventArgs e)
        {
            // Implementation for menu dropdown
            // Could include export, bulk actions, etc.
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
            int totalPages = (int)Math.Ceiling((double)_warehouseData.Count / PAGE_SIZE);
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