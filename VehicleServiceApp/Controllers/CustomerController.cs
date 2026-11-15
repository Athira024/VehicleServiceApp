using Microsoft.AspNetCore.Mvc;
using System.Data;
using VehicleServiceApp.Models;

namespace VehicleServiceApp.Controllers
{
    public class CustomerController : Controller
    {
        VehicleServiceDB dbobj = new VehicleServiceDB();
        public IActionResult Dashboard(string searchQuery)
        {
            if (HttpContext.Session.GetInt32("Reg_Id") == null)
                return RedirectToAction("Login", "Account");
            DataTable dt;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                dt = dbobj.SearchServices(searchQuery); // Or filter the DataTable directly
            }
            else
            {
                dt = dbobj.GetAllServices();
            }
            return View(dt);
        }

        // --- REGISTER VEHICLE ---
        [HttpGet]
        public IActionResult RegisterVehicle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterVehicle(VehicleCls obcls)
        {
            try
            {
                obcls.Customer_Id = HttpContext.Session.GetInt32("Reg_Id") ?? 0;
                string resp = dbobj.InsertVehicle(obcls);
                TempData["msg"] = resp;
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return View();
        }

        // --- SEARCH AVAILABLE SERVICES ---
        public IActionResult SearchServices()
        {
            DataTable dt = dbobj.GetAllServices();
            return View(dt);
        }

        // --- BOOK SERVICE ---
        [HttpGet]
        public IActionResult BookService(int sId)
        {
            ViewBag.ServiceId = sId;
            return View();
        }

        [HttpPost]
        public IActionResult BookService(BookingCls obcls)
        {
            try
            {
                int? customerId = HttpContext.Session.GetInt32("Reg_Id");
                if (customerId == null)
                    return RedirectToAction("Login", "Account");
                obcls.Customer_Id = customerId.Value;
                string resp = dbobj.InsertBooking(obcls);
                TempData["msg"] = resp; 
                return RedirectToAction("ServiceHistory");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error: " + ex.Message;
                return View(obcls);
            }
        }

        // --- SERVICE HISTORY / LAST SERVICE HISTORY ---
        public IActionResult ServiceHistory()
        {
            int? regId = HttpContext.Session.GetInt32("Reg_Id");
            if (regId == null || regId.Value == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            DataTable dt = dbobj.GetServiceHistoryByCustomer(regId.Value);
            return View(dt);
        }
        // --- GIVE FEEDBACK ---
        [HttpGet]
        public IActionResult GiveFeedback()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GiveFeedback(FeedbackCls obcls)
        {
            try
            {
                int? regId = HttpContext.Session.GetInt32("Reg_Id"); 
                if (regId == null || regId.Value == 0)
                {
                    return RedirectToAction("Login", "Account");
                }
                obcls.Customer_Id = regId.Value;
                string resp = dbobj.InsertFeedback(obcls);
                TempData["msg"] = resp;
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error: " + ex.Message;
            }
            return View(); 
        }
        [HttpGet]
        public IActionResult CancelBooking(int bId)
        {
            int? regId = HttpContext.Session.GetInt32("Reg_Id");
            if (regId == null || regId.Value == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                dbobj.CancelBooking(bId, regId.Value);
                TempData["msg"] = "Booking has been cancelled successfully.";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error: " + ex.Message;
            }

            return RedirectToAction("ServiceHistory");
        }
    }
}
   
