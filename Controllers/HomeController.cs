using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JobPortal.Core;
using JobPortal.Core.Domain.User;
using JobPortal.Web.Models;
using JobPortal.Areas.Job.Models;
//using JobPortal.Web.Models;

namespace JobPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private SearchRepository _searchRepository;
        private User _user;
        private UserVM _userVm;
        private JobSearchVM _searchVm;

        public ActionResult Index()
        {
            _searchVm = new JobSearchVM();
            _searchRepository = new SearchRepository();
            ViewBag.what = "Jobtitle, Keyword, or Company";
            ViewBag.where = "Country, State, City, or ZipCode";
            ViewBag.pageIndex = 0;
            _searchVm.Jobs = _searchRepository.GetAllJob();
            _searchVm.BasicSearches = _searchRepository.Keyterms();
            return View(_searchVm);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login user, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _searchRepository = new SearchRepository();
                    _userVm = new UserVM();
                    DataTable dt = new DataTable();
                    dt = _searchRepository.IsAvailableUser(user);
                    _user = new User();
                    if (dt == null)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        user.IsAvailable = true;
                        foreach (DataRow dr in dt.Rows)
                        {
                            user = new Login(dr);
                        }
                        AuthorizeAttribute authorizeAttribute = new AuthorizeAttribute();
                        authorizeAttribute.Users = user.UserName;
                        TempData["User_Id"] = user.UserId;
                        TempData["User_Name"] = user.UserName;
                        Session["User"] = user.UserName;

                        int timeout = 525600; //it is for 365 days
                        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(user.UserId.ToString(),
                                                                                         user.IsAvailable, timeout);
                        string enctrypted = FormsAuthentication.Encrypt(Ticket);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, enctrypted);
                        cookie.Expires = System.DateTime.Now.AddMinutes(timeout);
                        //HttpContext.Current.Response.Cookies.Add(cookie);
                        HttpContext.Response.Cookies.Add(cookie);

                        Response.Cookies.Add(new HttpCookie("UserName", user.UserName.ToString()));
                        FormsAuthentication.SetAuthCookie(user.UserId.ToString(), user.IsAvailable);
                        //TempData["IsLogin"] = user.UserName;
                        return RedirectToAction("Index", "Home");
                    }
                    /*if (Membership.ValidateUser(user.UserName, user.Password))
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, user.isAvailable);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }*/
                }
                catch (Exception ex)
                {
                    clsTools.Log("Login Error", ex, true);
                }
            }
            return View();

        }
        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(RegisterModel register)
        {
            _searchRepository = new SearchRepository();

            //_searchRepository.UserRegistration(register);
            return View("Login");
        }

        public ActionResult Logout()
        {
            var user = new User();
            user.UserName = null;
            TempData["User_Id"] = null;
            TempData["User_Name"] = null;
            TempData.Clear();

            var cookie = new HttpCookie("UserName") { Expires = DateTime.Now.AddDays(-1d) };
            Response.Cookies.Add(cookie);

            Session.Clear();
            FormsAuthentication.SignOut();
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoServerCaching();
            HttpContext.Response.Cache.SetNoStore();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}
