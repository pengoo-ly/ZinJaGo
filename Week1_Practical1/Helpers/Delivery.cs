using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Week1_Practical1.Helpers
{
    public class Delivery
    {
        string _connStr = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        private int _trackingID;
        private int _shipmentID;
        private string _status;
        private string _location;
        private DateTime _updateTime;

        public Delivery() { }

        public Delivery(int trackingID, int shipmentID, string status, string location, DateTime updateTime)
        {
            _trackingID = trackingID;
            _shipmentID = shipmentID;
            _status = status;
            _location = location;
            _updateTime = updateTime;
        }

        public int TrackingID
        {
            get { return _trackingID; }
            set { _trackingID = value; }
        }

        public int ShipmentID
        {
            get { return _shipmentID; }
            set { _shipmentID = value; }
        }

        public string StatusUpdate
        {
            get { return _status; }
            set { _status = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public DateTime UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }
        public List<Delivery> GetDeliveryByAdmin(int adminID)
        {
            List<Delivery> list = new List<Delivery>();

            string queryStr = @"
                SELECT DISTINCT DT.TrackingID, DT.ShipmentID, DT.StatusUpdate, DT.Location, DT.UpdateTime
                FROM DeliveryTracking DT
                INNER JOIN OrderShipments OS ON DT.ShipmentID = OS.ShipmentID
                INNER JOIN Orders O ON OS.OrderID = O.OrderID
                INNER JOIN OrderItems OI ON O.OrderID = OI.OrderID
                INNER JOIN Products P ON OI.ProductID = P.ProductID
                WHERE P.AdminID = @AdminID
                ORDER BY DT.UpdateTime DESC";

            try
            {
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@AdminID", adminID);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Delivery d = new Delivery(
                        Convert.ToInt32(dr["TrackingID"]),
                        Convert.ToInt32(dr["ShipmentID"]),
                        dr["StatusUpdate"].ToString(),
                        dr["Location"].ToString(),
                        Convert.ToDateTime(dr["UpdateTime"])
                    );

                    list.Add(d);
                }

                dr.Close();
                conn.Close();
            }
            catch
            {
                return null;
            }

            return list;
        }


        public int UpdateDelivery(int trackingID, string status, string location)
        {
            string queryStr = @"
                UPDATE DeliveryTracking
                SET StatusUpdate = @StatusUpdate,
                    Location = @Location,
                    UpdateTime = GETDATE()
                WHERE TrackingID = @TrackingID";

            try
            {
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);

                cmd.Parameters.AddWithValue("@TrackingID", trackingID);
                cmd.Parameters.AddWithValue("@StatusUpdate", status);
                cmd.Parameters.AddWithValue("@Location", location);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                return rowsAffected;
            }
            catch
            {
                return 0;
            }
        }
    }
}