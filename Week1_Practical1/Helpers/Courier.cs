using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Week1_Practical1.Helpers
{
    public class Courier
    {
        string _connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        private int _courierID = 0;
        private string _courierName = "";
        private string _contactNumber = "";
        private string _email = "";
        private string _trackingURL = "";
        private bool _isPartnered = false;

        public Courier() { }

        public Courier(int courierID, string courierName, string contactNumber,
                       string email, string trackingURL, bool isPartnered)
        {
            _courierID = courierID;
            _courierName = courierName;
            _contactNumber = contactNumber;
            _email = email;
            _trackingURL = trackingURL;
            _isPartnered = isPartnered;
        }

        public int CourierID
        {
            get { return _courierID; }
            set { _courierID = value; }
        }

        public string CourierName
        {
            get { return _courierName; }
            set { _courierName = value; }
        }

        public string ContactNumber
        {
            get { return _contactNumber; }
            set { _contactNumber = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string TrackingURL
        {
            get { return _trackingURL; }
            set { _trackingURL = value; }
        }

        public bool IsPartnered
        {
            get { return _isPartnered; }
            set { _isPartnered = value; }
        }

        public List<Courier> GetAllCouriers()
        {
            List<Courier> list = new List<Courier>();

            try
            {
                string query = "SELECT * FROM Couriers ORDER BY CourierID";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Courier c = new Courier(
                                Convert.ToInt32(dr["CourierID"]),
                                dr["CourierName"].ToString(),
                                dr["ContactNumber"].ToString(),
                                dr["Email"].ToString(),
                                dr["TrackingURL"].ToString(),
                                Convert.ToBoolean(dr["IsPartnered"])
                            );

                            list.Add(c);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Database-related error
                throw new Exception("An error occurred while retrieving courier data.", ex);
            }
            catch (Exception ex)
            {
                // Unexpected error
                throw new Exception("An unexpected error occurred while retrieving courier data.", ex);
            }

            return list;
        }

        public int InsertCourier()
        {
            try
            {
                string query = @"INSERT INTO Couriers
                         (CourierID, CourierName, ContactNumber, Email, TrackingURL, IsPartnered)
                         VALUES (@id, @name, @contact, @email, @url, @partnered)";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", CourierID);
                    cmd.Parameters.AddWithValue("@name", CourierName);
                    cmd.Parameters.AddWithValue("@contact", ContactNumber);
                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@url", TrackingURL);
                    cmd.Parameters.AddWithValue("@partnered", IsPartnered);

                    conn.Open();
                    return cmd.ExecuteNonQuery(); // rows affected
                }
            }
            catch (SqlException ex)
            {
                // Database-related error
                throw new Exception("An error occurred while inserting the courier.", ex);
            }
            catch (Exception ex)
            {
                // Unexpected error
                throw new Exception("An unexpected error occurred while inserting the courier.", ex);
            }
        }
        public int UpdateCourier()
        {
            try
            {
                string query = @"UPDATE Couriers SET
                         CourierName=@name,
                         ContactNumber=@contact,
                         Email=@email,
                         TrackingURL=@url,
                         IsPartnered=@partnered
                         WHERE CourierID=@id";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", CourierID);
                    cmd.Parameters.AddWithValue("@name", CourierName);
                    cmd.Parameters.AddWithValue("@contact", ContactNumber);
                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@url", TrackingURL);
                    cmd.Parameters.AddWithValue("@partnered", IsPartnered);

                    conn.Open();
                    return cmd.ExecuteNonQuery(); // number of rows affected
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("An error occurred while updating the courier.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the courier.", ex);
            }
        }
        public int DeleteCourier(int id)
        {
            try
            {
                string query = "DELETE FROM Couriers WHERE CourierID=@id";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery(); // rows affected
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write("<script>alert('Failed to delete courier.');</script>");
                return 0; // indicate failure
            }
        }
        public int GetNextCourierID()
        {
            int nextID = 1;

            try
            {
                string query = "SELECT MAX(CourierID) FROM Couriers";

                using (SqlConnection conn = new SqlConnection(_connStr))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                        nextID = Convert.ToInt32(result) + 1;
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write("<script>alert('Failed to get next Courier ID.');</script>");
                nextID = 1; // safe fallback
            }

            return nextID;
        }


    }
}