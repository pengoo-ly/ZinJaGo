using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Week1_Practical1.Helpers;


namespace Week1_Practical1
{
    public partial class AddProducts : System.Web.UI.Page
    {
        Product prod = new Product();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                int defaultID = prod.GetNextProductID();
                txtProdID.Text = defaultID.ToString();
            }
        }
        private void LoadCategories()
        {
            try
            {
                // Assuming you have a Category class with getCategoryAll()
                Category cat = new Category();
                List<Category> categories = cat.getCategoryAll();
                ddlCategory.DataSource = categories;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", "0"));
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error loading categories: " + ex.Message + "');</script>");
            }
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtProdID.Text.Trim(), out int prodID))
                {
                    Response.Write("<script>alert('Invalid Product ID');</script>");
                    return;
                }

                // Check if ID already exists
                if (prod.ProductExists(prodID))
                {
                    Response.Write("<script>alert('This Product ID already exists!');</script>");
                    return;
                }

                string prodName = txtProdName.Text.Trim();
                if (string.IsNullOrWhiteSpace(prodName))
                {
                    Response.Write("<script>alert('Please enter a Product Name');</script>");
                    return;
                }

                int catID = int.Parse(ddlCategory.SelectedValue);
                if (catID == 0)
                {
                    Response.Write("<script>alert('Please select a category');</script>");
                    return;
                }

                string desc = txtDesc.Text.Trim();

                if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
                {
                    Response.Write("<script>alert('Invalid Unit Price');</script>");
                    return;
                }

                if (!int.TryParse(txtStock.Text.Trim(), out int stock))
                {
                    Response.Write("<script>alert('Invalid Stock Quantity');</script>");
                    return;
                }

                string status = ddlStatus.SelectedValue;
                string imageFileName = "";

                if (fuImage.HasFile)
                {
                    imageFileName = Path.GetFileName(fuImage.PostedFile.FileName);
                    string savePath = Server.MapPath("~/Images/") + imageFileName;
                    fuImage.SaveAs(savePath);

                    // Show preview
                    imgPreview.ImageUrl = "~/Images/" + imageFileName;
                    imgPreview.CssClass = "preview-img";
                }

                // Create Product object
                Product newProd = new Product(prodID, prodName, catID, desc, price, stock, imageFileName, status);
                int result = newProd.ProductInsert();

                if (result > 0)
                    Response.Write("<script>alert('Product added successfully');window.location='ProductList.aspx';</script>");
                else
                    Response.Write("<script>alert('Product NOT added');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('Error adding product: {ex.Message}');</script>");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProductList.aspx");
        }
    }
}