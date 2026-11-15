using Microsoft.AspNetCore.SignalR.Protocol;
using System.Data;
using System.Data.SqlClient;
namespace VehicleServiceApp.Models
{
    public class VehicleServiceDB
    {
        SqlConnection con = new SqlConnection(@"server=LAPTOP-JV3FS7B9\SQLEXPRESS;database=VehicleServicingDB;Integrated Security=true");

        public string InsertCustomer(CustomerCls obj)
        {
            try
            {

                con.Open(); // <-- Open connection FIRST

                // Step B: Get the latest generated Customer ID
                SqlCommand cmdMaxId = new SqlCommand("sp_MaxIdLogin", con);
                cmdMaxId.CommandType = CommandType.StoredProcedure;
                object maxResult = cmdMaxId.ExecuteScalar();
                int nextId = (maxResult != DBNull.Value && maxResult != null) ? Convert.ToInt32(maxResult) + 1 : 1;

                // Step A: Insert into Customer table using sp_CustomerReg
                SqlCommand cmd = new SqlCommand("sp_CustomerReg", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cid", nextId);
                cmd.Parameters.AddWithValue("@cna", obj.Customer_Name);
                cmd.Parameters.AddWithValue("@cmail", obj.Customer_Email);
                cmd.Parameters.AddWithValue("@cphn", obj.Customer_Phone);
                cmd.Parameters.AddWithValue("@caddr", obj.Customer_Address);
                cmd.ExecuteNonQuery();

                // Step C: Insert into Login Table using sp_LogInsert
                SqlCommand cmdLog = new SqlCommand("sp_LogInsert", con);
                cmdLog.CommandType = CommandType.StoredProcedure;
                cmdLog.Parameters.AddWithValue("@rid", nextId);
                cmdLog.Parameters.AddWithValue("@una", obj.Username);
                cmdLog.Parameters.AddWithValue("@pw", obj.Password);
                cmdLog.Parameters.AddWithValue("@utype", "Customer");
                cmdLog.ExecuteNonQuery();

                con.Close();
                return "Customer Registered Successfully";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return ex.Message;
            }
        }
    
        

        public string InsertAdmin(AdminCls obj)
        {
            try
            {
                con.Open(); // <-- Open connection FIRST

                // Step B: Get the next/max Reg_Id using your sp_MaxIdLogin procedure
                SqlCommand cmdMaxId = new SqlCommand("sp_MaxIdLogin", con);
                cmdMaxId.CommandType = CommandType.StoredProcedure;
                object maxResult = cmdMaxId.ExecuteScalar();
                int nextId = (maxResult != DBNull.Value && maxResult != null) ? Convert.ToInt32(maxResult) + 1 : 1;

                // Step A: Insert into Admin table using sp_AdminReg
                SqlCommand cmd = new SqlCommand("sp_AdminReg", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@adid", nextId);
                cmd.Parameters.AddWithValue("@adna", obj.Admin_Name);
                cmd.Parameters.AddWithValue("@admail", obj.Admin_Email);
                cmd.Parameters.AddWithValue("@adphn", obj.Admin_Phone);
                cmd.ExecuteNonQuery();

                // Step C: Insert into Login Table using sp_LogInsert
                SqlCommand cmdLog = new SqlCommand("sp_LogInsert", con);
                cmdLog.CommandType = CommandType.StoredProcedure;
                cmdLog.Parameters.AddWithValue("@rid", nextId);
                cmdLog.Parameters.AddWithValue("@una", obj.Username);
                cmdLog.Parameters.AddWithValue("@pw", obj.Password);
                cmdLog.Parameters.AddWithValue("@utype", "Admin");
                cmdLog.ExecuteNonQuery();

                con.Close();
                return "Admin Registered Successfully";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return ex.Message;
            }
        }
        
      

        public string LoginCheck(string uname, string password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_LoginType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@una", uname);
                cmd.Parameters.AddWithValue("@pwd", password);
                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();
                return result != null ? result.ToString() : "Invalid";
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open) con.Close();
                throw;
            }
        }

        public int GetLogId(string uname, string password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_LoginId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@una", uname);
                cmd.Parameters.AddWithValue("@pwd", password);
                con.Open();
                int regId = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                return regId;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return 0;
            }
        }

        public string InsertService(ServiceCls obj)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ServiceIns", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@servicena", obj.Service_Name);
                cmd.Parameters.AddWithValue("@servicedesc", obj.Service_Desc);
                cmd.Parameters.AddWithValue("@price", obj.Service_Price);
                cmd.Parameters.AddWithValue("@status", "Available");
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return "Service Added Successfully";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return ex.Message;
            }
        }

        public string InsertVehicle(VehicleCls obj)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_VehicleReg", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cid", obj.Customer_Id);
                cmd.Parameters.AddWithValue("@brand", obj.Brand);
                cmd.Parameters.AddWithValue("@model", obj.Model);
                cmd.Parameters.AddWithValue("@fueltype", obj.FuelType);
                cmd.Parameters.AddWithValue("@regno", obj.RegNumber);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return "Vehicle Registered Successfully";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return ex.Message;
            }
        }

        public DataTable GetAllServices()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllServices", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(dt);
                con.Close();
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return dt;
        }

        public string InsertBooking(BookingCls obj)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertBooking", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cid", obj.Customer_Id);
                cmd.Parameters.AddWithValue("@vid", obj.Vehicle_Id);
                cmd.Parameters.AddWithValue("@sid", obj.Service_Id);

                // FIX: Ensure Booking_Date is not default/min value to avoid SqlDateTime overflow
                DateTime bookingDate = (obj.Booking_Date == default(DateTime)) ? DateTime.Now : obj.Booking_Date;
                cmd.Parameters.AddWithValue("@bkdate", bookingDate);

                // Handle Service_Date safely as well (if it can be optional/null)
                if (obj.Service_Date == default(DateTime))
                {
                    cmd.Parameters.AddWithValue("@servicedt", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@servicedt", obj.Service_Date);
                }

                cmd.Parameters.AddWithValue("@servicemode", string.IsNullOrEmpty(obj.Service_Mode) ? (object)DBNull.Value : obj.Service_Mode);
                cmd.Parameters.AddWithValue("@bkstatus", "Pending");
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return "Booking Placed Successfully";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return ex.Message;
            }
        }
            

        public string InsertFeedback(FeedbackCls obj)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertFeedback", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@cid", obj.Customer_Id);
                cmd.Parameters.AddWithValue("@feedback", obj.Feedback);
                string replyText = string.IsNullOrEmpty(obj.Reply) ? "Pending Admin Reply" : obj.Reply;
                cmd.Parameters.AddWithValue("@reply", replyText);
                // Prevent SqlDateTime overflow by forcing DateTime.Now if date is default/empty
                DateTime feedbackDate = (obj.F_Date == default(DateTime)) ? DateTime.Now : obj.F_Date;
                cmd.Parameters.AddWithValue("@dt", feedbackDate);

                cmd.Parameters.AddWithValue("@status", "Pending"); // Default status when customer submits feedback

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return "Feedback Submitted Successfully";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open) con.Close();
                return ex.Message;
            }
        }
        public DataTable SearchServices(string searchTerm)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("sp_SearchServices", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Pass the search text into the @SearchTerm parameter of your SP
                cmd.Parameters.AddWithValue("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? "" : searchTerm);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
        public DataTable GetServiceHistoryByCustomer(int customerId)
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("sp_GetServiceHistoryByCustomer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cid", customerId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
        public void CancelBooking(int bookingId, int customerId)
        {
            using (SqlCommand cmd = new SqlCommand("sp_CancelBooking", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bookingId", bookingId);
                cmd.Parameters.AddWithValue("@customerId", customerId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public DataTable GetAllCustomerBookings()
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("sp_GetAllCustomerBookings", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
        public void UpdateBookingStatus(int bookingId, string status)
        {
            using (SqlCommand cmd = new SqlCommand("sp_UpdateBookingStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@bookingId", bookingId);
                cmd.Parameters.AddWithValue("@status", status);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public DataTable GetAllCustomers()
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("sp_GetAllCustomers", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
        public DataTable GetAllVehicles()
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("sp_GetAllVehicles", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }
        public DataTable GetAllFeedback()
        {
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand("sp_GetAllFeedback", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public void UpdateFeedbackReply(int feedbackId, string reply)
        {
            using (SqlCommand cmd = new SqlCommand("sp_UpdateFeedbackReply", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@feedbackId", feedbackId);
                cmd.Parameters.AddWithValue("@reply", reply);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        // Fetch a single service by ID to populate the edit form
        public ServiceCls GetServiceById(int serviceId)
        {
            ServiceCls model = null;
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM ServiceTab WHERE Service_Id = @id", con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", serviceId);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        model = new ServiceCls
                        {
                            Service_Id = Convert.ToInt32(dr["Service_Id"]),
                            Service_Name = dr["Service_Name"].ToString(),
                            Service_Desc= dr["Service_Desc"].ToString(),
                            Service_Price = Convert.ToDecimal(dr["Service_Price"])
                        };
                    }
                }
                con.Close();
            }
            return model;
        }

        // Update the service details
        public void UpdateService(ServiceCls obj)
        {
            using (SqlCommand cmd = new SqlCommand("sp_UpdateService", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@serviceId", obj.Service_Id);
                cmd.Parameters.AddWithValue("@serviceName", obj.Service_Name);
                cmd.Parameters.AddWithValue("@description", obj.Service_Desc);
                cmd.Parameters.AddWithValue("@price", obj.Service_Price);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
