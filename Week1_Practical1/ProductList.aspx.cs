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

namespace Week1_Practical1
{
    public partial class ProductList : System.Web.UI.Page
    {
        Product aProd = new Product();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                bind();
            }
        }
        protected void bind()
        {
            try
            {
                if (Session["AdminID"] == null)
                {
                    Response.Redirect("AdminLogin.aspx");
                    return;
                }

                int adminId = Convert.ToInt32(Session["AdminID"]);
                List<Product> prodList = aProd.GetProductsByAdmin(adminId);

                gvProducts.DataSource = prodList;
                gvProducts.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error loading products: " + ex.Message + "');</script>");
            }
        }


        protected void gvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gvProducts.SelectedRow;
                string prodID = row.Cells[0].Text;
                Response.Redirect("ProductList.aspx?ProdID="+prodID);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error selecting product: " + ex.Message + "');</script>");
            }

        }

        protected void btnShowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AddProducts.aspx");
            }
            catch 
            {
                Response.Write("<script>alert('Sorry this page do not exist')</script>");
            }
        }

        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int result = 0;
                Product prod = new Product();
                int productID = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value.ToString());
                result = prod.ProductDelete(productID);

                if (result > 0)
                {
                    Response.Write("<script>alert('Product Remove successfully');</script>");
                }
                else
                {
                    Response.Write("<script>alert('Product Removal NOT successfully');</script>");
                }

                bind();

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error deleting product: " + ex.Message + "');</script>");
            }
        }

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try 
            {
                gvProducts.EditIndex = e.NewEditIndex;
                bind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error editing product: " + ex.Message + "');</script>");
            }
        }

        protected void gvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try 
            {
                gvProducts.EditIndex = -1;
                bind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error canceling edit: " + ex.Message + "');</script>");
            }
        }

        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int result = 0;
                Product prod = new Product();
                GridViewRow row = gvProducts.Rows[e.RowIndex];

                int pid = Convert.ToInt32(((TextBox)row.Cells[0].Controls[0]).Text); // ProductID
                string pname = ((TextBox)row.Cells[1].Controls[0]).Text;
                int cid = Convert.ToInt32(((TextBox)row.Cells[2].Controls[0]).Text);
                string pdesc = ((TextBox)row.Cells[3].Controls[0]).Text;
                decimal pprice = decimal.Parse(((TextBox)row.Cells[4].Controls[0]).Text);
                int pstock = Convert.ToInt32(((TextBox)row.Cells[5].Controls[0]).Text);
                string pstatus = ((TextBox)row.Cells[7].Controls[0]).Text;

                // Handle FileUpload
                FileUpload fuEdit = (FileUpload)row.FindControl("fuEditImage");
                HiddenField hfOld = (HiddenField)row.FindControl("hfOldImage");
                string pimage = hfOld.Value; // fallback to old image

                if (fuEdit.HasFile)
                {
                    pimage = fuEdit.FileName;
                    string savePath = Server.MapPath("~/Images/") + pimage;
                    fuEdit.SaveAs(savePath);
                }

                result = prod.ProductUpdate(pid, pname, cid, pdesc, pprice, pstock, pimage, pstatus);

                if (result > 0)
                    Response.Write("<script>alert('Product updated successfully');</script>");
                else
                    Response.Write("<script>alert('Product NOT updated');</script>");

                gvProducts.EditIndex = -1;
                bind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error updating product: " + ex.Message + "');</script>");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Product prod = new Product();
                gvProducts.DataSource = prod.SearchProductByName(txtSearch.Text.Trim());
                gvProducts.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error searching product: " + ex.Message + "');</script>");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Text = "";
                bind(); // reload all products
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error clearing search: " + ex.Message + "');</script>");
            }
        }
    }
}