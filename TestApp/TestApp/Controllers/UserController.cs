using System;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;
using TestApp.Models;
using TestApp.Services;
using TestApp.ViewModels;

namespace TestApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            try
            {
                if (user != null && ModelState.IsValid && !_userService.IsUserExist(user))
                {
                    _userService.Register(user);
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("UserName", "User with this user name or Email exist, please chose another");
                    ModelState.AddModelError("Email", "User with this user name or Email exist, please chose another");
                    return View(user);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Login()
        {
            FormsAuthentication.SignOut();
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            try
            {
                if (user != null && ModelState.IsValid)
                {
                    var usr = _userService.Login(user);
                    if (usr != null)
                    {
                        FormsAuthentication.SetAuthCookie(usr.UserName, false);
                        return RedirectToAction("Index", "Index");
                    }
                }
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Error()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _userService.Dispose();

            base.Dispose(disposing);
        }
    }
}