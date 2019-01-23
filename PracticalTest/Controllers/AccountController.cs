using PracticalTest.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static PracticalTest.Models.AccountViewModels;
using PracticalTest.Utils;

namespace PracticalTest.Controllers
{
    public class AccountController : Controller
    {
        private PracticalTest.DAL.PracticalTest db = PracticalTest.DAL.PracticalTest.GetInstance();

        [AllowAnonymous]

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        // POST Login data to database
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            
            if (ModelState.IsValid)
            {
                var user = db.UserSys
                    .Where(u => u.Email == model.Email).SingleOrDefault();

                // check user password
                if (user != null && PasswordSecurity.IsPasswordMatch(user.Password, user.Salt, model.Password))
                {                                 
                      UserRole userRole = db.UserRole.SingleOrDefault(r => r.Id == user.UserRoleId);
                      this.Session.Add("UserId", user.Id);
                      if (userRole.IsAdmin)
                          this.Session.Add("IsUserAdmin", 1);
                      else
                        this.Session["IsUserAdmin"] = null;

                    FormsAuthentication.SetAuthCookie(model.Email, false);
                      return RedirectToAction("List", "Customer");                   
                 }
                 else
                 {
                     ModelState.AddModelError("", "The e-mail and/or password entered is invalid. Please Try again.");
                 }
             }
            
            return View(model);
        }
    }
}