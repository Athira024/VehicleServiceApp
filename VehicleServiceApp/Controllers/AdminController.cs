using Microsoft.AspNetCore.Mvc;
using System.Data;
using VehicleServiceApp.Models;

namespace VehicleServiceApp.Controllers
{
    public class AdminController : Controller
    {
        VehicleServiceDB dbobj = new VehicleServiceDB();
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("Reg_Id") == null || HttpContext.Session.GetString("Usertype") != "Admin")
                return RedirectToAction("Login", "Account");

            return View();
        }
        // --- MANAGE & ADD SERVICES ---
        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddService(ServiceCls obcls)
        {
            try
            {
                string resp = dbobj.InsertService(obcls);
                TempData["msg"] = resp;
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return View();
        }
        // --- VIEW CUSTOMER BOOKINGS & STATUS ---
        public IActionResult ViewBookings()
        {
            DataTable dt = dbobj.GetAllCustomerBookings(); // Or your database method fetching all bookings
            return View(dt);
        }
        // --- CUSTOMER DIRECTORY ---
        public IActionResult CustomerDirectory()
        {
            DataTable dt = dbobj.GetAllCustomers(); // Calls your database method to fetch registered users
            return View(dt);
        }
        // --- VEHICLE DIRECTORY ---
        public IActionResult VehicleDirectory()
        {
            DataTable dt = dbobj.GetAllVehicles(); // Calls your database access method
            return View(dt);
        }
        // --- FEEDBACK MANAGEMENT & REPLY ---
        public IActionResult ViewFeedback()
        {
            DataTable dt = dbobj.GetAllFeedback();
            return View(dt);
        }
        [HttpPost]
        public IActionResult ReplyFeedback(int feedbackId, string reply)
        {
            try
            {
                dbobj.UpdateFeedbackReply(feedbackId, reply);
                TempData["msg"] = "Reply sent successfully.";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error saving reply: " + ex.Message;
            }

            return RedirectToAction("ViewFeedback");
        }
        [HttpGet]
        public IActionResult UpdateStatus(int bId, string status)
        {
            try
            {
                dbobj.UpdateBookingStatus(bId, status);
                TempData["msg"] = $"Booking #{bId} status updated to '{status}' successfully.";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error updating status: " + ex.Message;
            }

            return RedirectToAction("ViewBookings");
        }
        [HttpGet]
        public IActionResult EditService(int id)
        {
            ServiceCls model = dbobj.GetServiceById(id);
            if (model == null)
            {
                TempData["msg"] = "Service not found.";
                return RedirectToAction("ViewServices"); // Or your admin service list action
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult EditService(ServiceCls obj)
        {
            try
            {
                dbobj.UpdateService(obj);

                // SET THE SUCCESS MESSAGE HERE:
                TempData["msg"] = "Service details updated successfully!";

                return RedirectToAction("ManageServices");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error updating service: " + ex.Message;
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult ManageServices()
        {
            // Fetch all services from your database layer
            DataTable dt = dbobj.GetAllServices();
            return View(dt);
        }
    }
}

