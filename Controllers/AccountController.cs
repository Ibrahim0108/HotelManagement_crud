using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HotelManagement.Models;
using HotelManagement.Data;

namespace HotelManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDAL userDAL = new UserDAL();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username already exists.
                if (userDAL.UsernameExists(model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(model);
                }

                if (model.RequestedRole == "Admin")
                {
                    model.IsPendingAdmin = true;
                }
                else
                {
                    model.IsPendingAdmin = false;
                }
                userDAL.InsertUser(model);

                return RedirectToAction("Login");
            }
            return View(model);
        }


        public ActionResult Login(string returnUrl)
        {
            // Ignore ReturnUrl if it points to the Login page (avoid loop)
            if (!string.IsNullOrEmpty(returnUrl) && (returnUrl.Contains("/Account/Login") || returnUrl.Contains("/Account/Register")))
            {
                returnUrl = null;
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


         // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userDAL.GetUser(model.Username, model.Password);
                if (user != null)
                {
                    if (user.IsPendingAdmin)
                    {
                        ModelState.AddModelError("", "Your admin request is pending approval. Please wait for an administrator to review your account.");
                        return View(model);
                    }
                    var ticket = new FormsAuthenticationTicket(
    1,
    user.Username,
    DateTime.Now,
    DateTime.Now.AddMinutes(30),
    false,
    user.Role  // <- This is critical!
);

                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) && !returnUrl.StartsWith("/Account/Login", StringComparison.OrdinalIgnoreCase) && !returnUrl.StartsWith("/Account/Register", StringComparison.OrdinalIgnoreCase))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Rooms");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }



    }
}