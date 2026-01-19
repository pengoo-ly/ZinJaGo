using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Week1_Practical1.Helpers
{
    public class Product
    {
        string _connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;
        private int _prodID = 0;
        private string _prodName = string.Empty;
        private int _catID = 0;
        private string _prodDesc = ""; // this is another way to specify empty string
        private decimal _unitPrice = 0;
        private int _stockLevel = 0;
        private string _prodImage = "";
        private string _status = "";

        public Product()
        {
        }

        public Product(int prodID, string prodName, int catID, string prodDesc,
                       decimal unitPrice, int stockLevel, string prodImage, string status)
        {
            _prodID = prodID;
            _prodName = prodName;
            _catID = catID;
            _prodDesc = prodDesc;
            _unitPrice = unitPrice;
            _stockLevel = stockLevel;
            _prodImage = prodImage;
            _status = status;
        }

        public Product(string prodName, int catID, string prodDesc,
                       decimal unitPrice, int stockLevel, string prodImage, string status)
            : this(0, prodName, 0, prodDesc, unitPrice, stockLevel, prodImage, "Active")
        {
        }

        public Product(int prodID)
            : this(prodID, "", 0, "", 0, 0, "", "")
        {
        }

        public int ProductID
        {
            get { return _prodID; }
            set { _prodID = value; }
        }
        public string ProductName
        {
            get { return _prodName; }
            set { _prodName = value; }
        }
        public int CategoryID
        {
            get { return _catID; }
            set { _catID = value; }
        }
        public string Description
        {
            get { return _prodDesc; }
            set { _prodDesc = value; }
        }
        public decimal Unit_Price
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }
        public int StockQuantity
        {
            get { return _stockLevel; }
            set { _stockLevel = value; }
        }
        public string Image
        {
            get { return _prodImage; }
            set { _prodImage = value; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Product getProduct(int prodID)
        {

            Product prodDetail = null;

            string prod_Name, prod_Desc, Prod_Image, status;
            decimal unit_Price;
            int stock_Level, catID;

            try
            {
                string queryStr = "SELECT * FROM Products WHERE ProductID = @prodID";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@prodID", prodID);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    prod_Name = dr["ProductName"].ToString();
                    catID = int.Parse(dr["CategoryID"].ToString());
                    prod_Desc = dr["Description"].ToString();
                    unit_Price = decimal.Parse(dr["Unit_Price"].ToString());
                    stock_Level = int.Parse(dr["StockQuantity"].ToString());
                    Prod_Image = dr["Image"].ToString();
                    status = dr["Status"].ToString();

                    prodDetail = new Product(prodID, prod_Name, catID, prod_Desc, unit_Price, stock_Level, Prod_Image, status);
                }
                else
                {
                    prodDetail = null;
                }

                conn.Close();
                dr.Close();
                dr.Dispose();
            }

            catch (Exception ex)
            {
                return null;
            }

            return prodDetail;
        }

        public List<Product> getProductAll()
        {
            List<Product> prodList = new List<Product>();
            try
            {
                string prod_Name, prod_Desc, Prod_Image, status;
                decimal unit_Price;
                int stock_Level, prod_ID, cat_ID;

                string queryStr = "SELECT * FROM Products Order By ProductID";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    prod_ID = int.Parse(dr["ProductID"].ToString());
                    prod_Name = dr["ProductName"].ToString();
                    cat_ID = int.Parse(dr["CategoryID"].ToString());
                    prod_Desc = dr["Description"].ToString();
                    unit_Price = decimal.Parse(dr["Unit_Price"].ToString());
                    stock_Level = int.Parse(dr["StockQuantity"].ToString());
                    Prod_Image = dr["Image"].ToString();
                    status = dr["Status"].ToString();
                    Product a = new Product(prod_ID, prod_Name, cat_ID, prod_Desc, unit_Price, stock_Level, Prod_Image, status);
                    prodList.Add(a);
                }

                conn.Close();
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                return null;
            }

            return prodList;
        }
        public int ProductInsert()
        {

            // string msg = null;
            int result = 0;

            string queryStr = "INSERT INTO Products(ProductID, ProductName, CategoryID, Description, Unit_Price, StockQuantity,Image, Status)"
                + " values (@Product_ID,@Product_Name,@Category_ID, @Product_Desc, @Unit_Price,@Stock_Level, @Product_Image, @Product_Status)";
            //+ "values (@Product_ID, @Product_Name, @Product_Desc, @Unit_Price, @Product_Image,@Stock_Level)";
            try
            {
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@Product_ID", this.ProductID);
                cmd.Parameters.AddWithValue("@Product_Name", this.ProductName);
                cmd.Parameters.AddWithValue("@Category_ID", this.CategoryID);
                cmd.Parameters.AddWithValue("@Product_Desc", this.Description);
                cmd.Parameters.AddWithValue("@Unit_Price", this.Unit_Price);
                cmd.Parameters.AddWithValue("@Stock_Level", this.StockQuantity);
                cmd.Parameters.AddWithValue("@Product_Image", this.Image);
                cmd.Parameters.AddWithValue("@Product_Status", this.Status);

                conn.Open();
                result += cmd.ExecuteNonQuery(); // Returns no. of rows affected. Must be > 0
                conn.Close();

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int ProductDelete(int ID)
        {
            try
            {
                string queryStr = "DELETE FROM Products WHERE ProductID=@ID";
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@ID", ID);
                conn.Open();
                int nofRow = 0;
                nofRow = cmd.ExecuteNonQuery();
                //  Response.Write("<script>alert('Delete successful');</script>");
                conn.Close();
                return nofRow;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return 0;

            }

        }
        public int ProductUpdate(int pId, string pName, int cId,string pdesc, decimal pUnitPrice,int pStock, string pImg, string pStatus)
        {
            try 
            {
                string queryStr = "UPDATE Products SET" +
                " ProductName = @productName, " +
                " CategoryID = @categoryID, " +
                " Description = @prodDesc, " +
                " Unit_Price = @unitPrice, " +
                " StockQuantity = @stockLevel, " +
                " Image = @productImage, " +
                " Status = @productStatus " +
                " WHERE ProductID = @productID";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@productID", pId);
                cmd.Parameters.AddWithValue("@productName", pName);
                cmd.Parameters.AddWithValue("@categoryID", cId);
                cmd.Parameters.AddWithValue("@prodDesc", pdesc);
                cmd.Parameters.AddWithValue("@unitPrice", pUnitPrice);
                cmd.Parameters.AddWithValue("@stockLevel", pStock);
                cmd.Parameters.AddWithValue("@productImage", pImg);
                cmd.Parameters.AddWithValue("@productStatus", pStatus);
                conn.Open();
                int nofRow = 0;
                nofRow = cmd.ExecuteNonQuery();

                conn.Close();

                return nofRow;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
        public int ProductDeactivate(int ID)
        {
            try
            {
                string queryStr = "UPDATE Products SET Status = 'Unavailable' WHERE ProductID = @ID";
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@ID", ID);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                return rows;
            }
            catch
            {
                return 0;
            }
        }
        public List<Product> SearchProductByName(string keyword)
        {
            List<Product> prodList = new List<Product>();

            try
            {
                string queryStr = "SELECT * FROM Products WHERE ProductName LIKE @keyword";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Product p = new Product(
                        int.Parse(dr["ProductID"].ToString()),
                        dr["ProductName"].ToString(),
                        int.Parse(dr["CategoryID"].ToString()),
                        dr["Description"].ToString(),
                        decimal.Parse(dr["Unit_Price"].ToString()),
                        int.Parse(dr["StockQuantity"].ToString()),
                        dr["Image"].ToString(),
                        dr["Status"].ToString()
                    );

                    prodList.Add(p);
                }

                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return prodList;
        }

        public bool ProductExists(int productId)
        {
            try
            {
                bool exists = false;
                string query = "SELECT COUNT(*) FROM Products WHERE ProductID = @ProductID";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    exists = count > 0;
                    conn.Close();
                }

                return exists;
            }
            catch (Exception ex)
            {
                // Optionally log the exception somewhere
                // Console.WriteLine(ex.Message);

                // Return false if there is an error
                return false;
            }
        }

        public int GetNextProductID()
        {
            int nextID = 1;
            string query = "SELECT MAX(ProductID) FROM Products";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                        nextID = Convert.ToInt32(result) + 1;
                    conn.Close();
                }
            }
            catch
            {
                nextID = 1; // fallback
            }

            return nextID;
        }

    }
}