using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace Week1_Practical1.Helpers
{
    public class Category
    {
        string _connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;
        private int _catID = 0;
        private string _catName = string.Empty;
        private string _catDesc = "";

        public Category() 
        { 
        }

        public Category(int catID, string catName, string catDesc)
        {
            _catID = catID;
            _catName = catName;
            _catDesc = catDesc;
        }
        public Category(string catName, string catDesc)
            : this(0, catName, catDesc)
        {
        }

        public Category(int catID) 
            :this(catID,"","")
        { 
        }
        public int CategoryID
        {
            get { return _catID; }
            set { _catID = value; }
        }
        public string CategoryName
        {
            get { return _catName; }
            set { _catName = value; }
        }
        public string Description
        {
            get { return _catDesc; }
            set { _catDesc = value; }
        }
        public Category getCategory(int catID)
        {

            Category catDetail = null;

            string cat_Name, cat_Desc;
            try
            {
                string queryStr = "SELECT * FROM Categories WHERE CategoryID = @CatID";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@CatID", catID);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cat_Name = dr["CategoryName"].ToString();
                    cat_Desc = dr["Description"].ToString();

                    catDetail = new Category(catID, cat_Name, cat_Desc);
                }
                else
                {
                    catDetail = null;
                }

                conn.Close();
                dr.Close();
                dr.Dispose();
            }

            catch (Exception ex) 
            {
                return null;
            }

            return catDetail;
        }

        public List<Category> getCategoryAll()
        {
            List<Category> catList = new List<Category>();
            try 
            {
                string cat_Name, cat_Desc;
                int cat_ID;

                string queryStr = "SELECT * FROM Categories Order By CategoryID";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cat_ID = Convert.ToInt32(dr["CategoryID"]);
                    cat_Name = dr["CategoryName"].ToString();
                    cat_Desc = dr["Description"].ToString();
                    Category a = new Category(cat_ID, cat_Name, cat_Desc);
                    catList.Add(a);
                }

                conn.Close();
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex) 
            {
                return null;
            }

            return catList;
        }
        public int CategoryInsert()
        {

            // string msg = null;
            int result = 0;

            string queryStr = "INSERT INTO Categories(CategoryID,CategoryName, Description)"
                + " values (@Category_ID,@Category_Name, @Category_Desc)";
            try
            {
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@Category_ID", this.CategoryID);
                cmd.Parameters.AddWithValue("@Category_Name", this.CategoryName);
                cmd.Parameters.AddWithValue("@Category_Desc", this.Description);

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
        public int CategoryDelete(int catID)
        {
            try
            {
                string queryStr = "DELETE FROM Categories WHERE CategoryID=@catID";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@catID", catID);
                conn.Open();
                int nofRow = 0;
                nofRow = cmd.ExecuteNonQuery();
                //  Response.Write("<script>alert('Delete successful');</script>");
                conn.Close();
                return nofRow;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                return 0;
            }

        }
        public int CategoryUpdate(int cId, string cName, string cDesc)
        {
            try
            {
                string queryStr = "UPDATE Categories SET" +
                    //" CategoryID = @categoryID, " +
                    " CategoryName = @categoryName, " +
                    " Description = @categoryDesc " +
                    " WHERE CategoryID = @categoryID";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@categoryID", cId);
                cmd.Parameters.AddWithValue("@categoryName", cName);
                cmd.Parameters.AddWithValue("@categoryDesc", cDesc);

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

        public List<Category> SearchCategoryByName(string keyword)
        {
            List<Category> catList = new List<Category>();

            try
            {
                string queryStr = "SELECT * FROM Categories WHERE CategoryName LIKE @keyword";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Category p = new Category(
                        int.Parse(dr["CategoryID"].ToString()),
                        dr["CategoryName"].ToString(),
                        dr["Description"].ToString()
                    );

                    catList.Add(p);
                }

                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return catList;
        }
    }
}