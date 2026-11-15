using Microsoft.AspNetCore.Mvc;
using VehicleServiceApp.Models;

namespace VehicleServiceApp.Controllers
{
    public class AccountController : Controller
    {
        VehicleServiceDB dbobj = new VehicleServiceDB();
        // --- CUSTOMER REGISTRATION ---
        public IActionResult RegisterCustomer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterCustomer(CustomerCls obcls)
        {
            try
            {
                dbobj.InsertCustomer(obcls);
                ViewBag.Msg = "Registration successful! You can now log in.";
                ModelState.Clear();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "Error during registration: " + ex.Message;
                return View(obcls);
            }
        }
        // --- ADMIN REGISTRATION ---
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterAdmin(AdminCls obcls)
        {
            try
            {
                string resp = dbobj.InsertAdmin(obcls);
                TempData["msg"] = resp;
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                return View();
            }
        }
        // --- LOGIN ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string uname, string password)
        {
            try
            {
                string userType = dbobj.LoginCheck(uname, password);

                if (userType == "Customer" || userType == "Admin")
                {
                    int regId = dbobj.GetLogId(uname, password);
                    HttpContext.Session.SetInt32("Reg_Id", regId);
                    HttpContext.Session.SetString("Usertype", userType);

                    if (userType == "Admin")
                        return RedirectToAction("Dashboard", "Admin");
                    else
                        return RedirectToAction("Dashboard", "Customer");
                }
                else
                {
                    TempData["msg"] = "Invalid Username or Password";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                return View();
            }

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
    

