using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
//using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebMD.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            using (Models.OurDbContext db = new Models.OurDbContext())
            {
                return View(db.userAccount.ToList());
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (Models.OurDbContext db = new Models.OurDbContext())
                {
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.PhoneNumber + " successfully registered.";
            }
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.UserAccount user)
        {
            using (Models.OurDbContext db = new Models.OurDbContext())
            {
                var usr = db.userAccount.Single(u => u.PhoneNumber ==
                user.PhoneNumber && u.Password == user.Password);
                if (usr != null)
                {
                    HttpContext.Session.SetString("UserID",usr.UserID.ToString());
                    HttpContext.Session.SetString("PhoneNumber", usr.PhoneNumber.ToString());
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "PhoneNumber or Password is wrong. ");
                }
            }
            return View();
        }

        // LoggedIn.cshtml zem Views/Account kaut kas neiet..
        public ActionResult LoggedIn()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}