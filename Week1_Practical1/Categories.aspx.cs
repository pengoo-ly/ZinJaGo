using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Week1_Practical1.Helpers;


namespace Week1_Practical1
{
    public partial class Categories : System.Web.UI.Page
    {
        Category aCategory = new Category();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                bind();
            }
        }
        protected void bind() 
        {
            try 
            {
                List<Category> catList = new List<Category>();
                catList=aCategory.getCategoryAll();
                gvCategories.DataSource = catList;
                gvCategories.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error loading categories: " + ex.Message + "');</script>");
            }

        }

        protected void gvCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                GridViewRow row = gvCategories.SelectedRow;
                string catID = row.Cells[0].Text;
                Response.Redirect("Categories.aspx?CatID="+catID);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error selecting category: " + ex.Message + "');</script>");
            }


        }

        protected void gvCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try 
            {
                int result = 0;
                Category cat = new Category();
                int CategoryID = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
                result = cat.CategoryDelete(CategoryID);

                if (result > 0)
                {
                    Response.Write("<script>alert('Category Remove successfully');</script>");
                }
                else
                {
                    Response.Write("<script>alert('Category Removal NOT successfully');</script>");
                }

                bind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error deleting category: " + ex.Message + "');</script>");
            }

        }

        protected void gvCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try 
            {
                gvCategories.EditIndex = e.NewEditIndex;
                bind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error editing category: " + ex.Message + "');</script>");
            }
        }

        protected void gvCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try 
            {
                gvCategories.EditIndex = -1;
                bind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error canceling edit: " + ex.Message + "');</script>");
            }

        }

        protected void gvCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try 
            {
                int result = 0;
                Category cat = new Category();
                GridViewRow row = (GridViewRow)gvCategories.Rows[e.RowIndex];
                int id = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
                int cid = Convert.ToInt32(((TextBox)row.Cells[0].Controls[0]).Text);
                string cname = ((TextBox)row.Cells[1].Controls[0]).Text;
                string cdesc = ((TextBox)row.Cells[2].Controls[0]).Text;

                result = cat.CategoryUpdate(cid, cname, cdesc);
                if (result > 0)
                {
                    Response.Write("<script>alert('Category updated successfully');</script>");
                }
                else
                {
                    Response.Write("<script>alert('Category NOT updated');</script>");
                }
                gvCategories.EditIndex = -1;
                bind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error updating category: " + ex.Message + "');</script>");
            }

        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            try 
            {
                Category cat = new Category();
                cat.CategoryID = Convert.ToInt32(txtCatID.Text);
                cat.CategoryName = txtCatName.Text;
                cat.Description = txtCatDesc.Text;
                int result = cat.CategoryInsert();
                if (result > 0)
                {
                    Response.Write("<script>alert('Category added successfully');</script>");
                    txtCatID.Text = "";
                    txtCatName.Text = "";
                    txtCatDesc.Text = "";
                    pnlAddCategory.Visible = false;
                    bind();
                }
                else
                {
                    Response.Write("<script>alert('Category NOT added');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error adding category: " + ex.Message + "');</script>");
            }
        }

        protected void btnShowAdd_Click(object sender, EventArgs e)
        {
            pnlAddCategory.Visible = true;
        }

        protected void btnCancelAdd_Click(object sender, EventArgs e)
        {
            pnlAddCategory.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Category cat = new Category();
                gvCategories.DataSource = cat.SearchCategoryByName(txtSearch.Text.Trim());
                gvCategories.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error searching category: " + ex.Message + "');</script>");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Text = "";
                bind(); 
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error clearing search: " + ex.Message + "');</script>");
            }
        }
    }
}