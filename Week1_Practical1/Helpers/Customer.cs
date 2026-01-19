using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Week1_Practical1.Helpers
{
    public class Customer
    {
        private string _connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;
        private int _userID;
        private string _username;
        private string _email;
        private string _phone;
        private string _status;

        public Customer() { }

        public Customer(int userID, string username, string email, string phone, string status)
        {
            _userID = userID;
            _username = username;
            _email = email;
            _phone = phone;
            _status = status;
        }

        public Customer(int userID)
            :this(userID, "", "", "", "Active") { }

        public int UserID { get { return _userID; } set { _userID = value; } }
        public string Username { get { return _username; } set { _username = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public string Phone { get { return _phone; } set { _phone = value; } }
        public string Status { get { return _status; } set { _status = value; } }

        public Customer GetCustomer(int userID)
        {
            Customer custDetail = null;

            string cust_Email,cust_Name, cust_Status;
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
                    cust_Name = dr["CategoryName"].ToString();
                    cust_Status = dr["Description"].ToString();

                    catDetail = new Category(catID, cat_Name, cat_Desc);
                }
                else
                {
                    custDetail = null;
                }

                conn.Close();
                dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in GetCustomer: " + ex.Message);
                return null;
            }
            return custDetail;
        }
    }
}