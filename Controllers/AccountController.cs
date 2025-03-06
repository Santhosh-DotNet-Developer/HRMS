using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using HRMS.Models;

namespace HRMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Declaration
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string type)
        {
            if (type == "Reset")
            {
                string empCode = Constant.SessionUserEmpCode;
                USER_CREATION userCreDet = db.USER_CREATION.Where(w => w.USER_EMP_CODE == empCode).SingleOrDefault();
                this.SetCookies(userCreDet);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }
        #endregion

        #region Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (CheckAplicationSession() == true)
                {
                    try
                    {
                        EncryptDecrypt encrypt = new EncryptDecrypt();
                        string password = encrypt.Encrypt(model.Password);
                        USER_CREATION userCreDet = db.USER_CREATION.Where(w => w.USER_NAME == model.UserName && w.USER_PSWD == password && w.USER_STATUS == true).SingleOrDefault();
                        if (userCreDet != null)
                        {
                            this.SetCookies(userCreDet);
                            if (returnUrl == "/")
                            {
                                return RedirectToAction("Index", "Dashboard");
                            }
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            ViewBag.Error = "Invalid Username/Password";
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception in AccountController - Login : " + ex.Message);
                    }
                }
                else
                {
                    ViewBag.Error = "Validity Expired. Please contact Roadmap IT Solutions!";
                }
            }
            return View(model);
        }
        #endregion

        #region SessionOut
        [AllowAnonymous]
        public ActionResult SessionOut()
        {
            return View();
        }
        #endregion

        #region Log Out
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Session.Abandon();
            this.ClearCookies();
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region Clear Cookies
        public void ClearCookies()
        {
            try
            {
                if (Request.Cookies[Constant.BE] != null)
                {
                    HttpCookie BE = new HttpCookie(Constant.BE);
                    BE.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(BE);
                }
                if (Request.Cookies[Constant.BU] != null)
                {
                    HttpCookie BU = new HttpCookie(Constant.BU);
                    BU.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(BU);
                }
                if (Request.Cookies[Constant.Username] != null)
                {
                    HttpCookie Username = new HttpCookie(Constant.Username);
                    Username.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(Username);
                }
                if (Request.Cookies[Constant.EmpCode] != null)
                {
                    HttpCookie EmpCode = new HttpCookie(Constant.EmpCode);
                    EmpCode.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(EmpCode);
                }
                if (Request.Cookies[Constant.EmpName] != null)
                {
                    HttpCookie EmpName = new HttpCookie(Constant.EmpName);
                    EmpName.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(EmpName);
                }
                if (Request.Cookies[Constant.Role] != null)
                {
                    HttpCookie Role = new HttpCookie(Constant.Role);
                    Role.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(Role);
                }
                if (Request.Cookies[Constant.RoleID] != null)
                {
                    HttpCookie RoleID = new HttpCookie(Constant.RoleID);
                    RoleID.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(RoleID);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in AccountController - ClearCookies : " + ex.Message);
            }
        }
        #endregion

        #region Set Cookies
        public void SetCookies(USER_CREATION userCreation)
        {
            try
            {
                this.ClearCookies();
                EMPLOYEE empDet = db.EMPLOYEES.Where(w => w.EMP_CODE == userCreation.USER_EMP_CODE).SingleOrDefault();

                HttpCookie BE = new HttpCookie(Constant.BE);
                BE.Value = userCreation.USER_BE.ToString();
                BE.Expires = DateTime.Now.AddMinutes(Constant.SessionTime);
                Response.SetCookie(BE);

                HttpCookie BU = new HttpCookie(Constant.BU);
                BU.Value = userCreation.USER_BU.ToString();
                BU.Expires = DateTime.Now.AddMinutes(Constant.SessionTime);
                Response.SetCookie(BU);

                HttpCookie Username = new HttpCookie(Constant.Username);
                Username.Value = userCreation.USER_NAME;
                Username.Expires = DateTime.Now.AddMinutes(Constant.SessionTime);
                Response.SetCookie(Username);

                HttpCookie EmpCode = new HttpCookie(Constant.EmpCode);
                EmpCode.Value = empDet.EMP_CODE;
                EmpCode.Expires = DateTime.Now.AddMinutes(Constant.SessionTime);
                Response.SetCookie(EmpCode);

                HttpCookie EmpName = new HttpCookie(Constant.EmpName);
                EmpName.Value = empDet.EMP_FIRST_NAME;
                EmpName.Expires = DateTime.Now.AddMinutes(Constant.SessionTime);
                Response.SetCookie(EmpName);

                HttpCookie Role = new HttpCookie(Constant.Role);
                Role.Value = userCreation.ROLE.ROLE_DESC;
                Role.Expires = DateTime.Now.AddMinutes(Constant.SessionTime);
                Response.SetCookie(Role);

                HttpCookie RoleID = new HttpCookie(Constant.RoleID);
                RoleID.Value = userCreation.USER_ROLE_ID.ToString();
                RoleID.Expires = DateTime.Now.AddMinutes(Constant.SessionTime);
                Response.SetCookie(RoleID);
            }
            catch (Exception ex)
            {
                log.Debug("Exception in AccountController - SetCookies : " + ex.Message);
            }
        }
        #endregion

        #region Check Session Status
        [AllowAnonymous]
        public JsonResult CheckSessionStatus()
        {
            bool result = false;
            var username = HttpContext.Request.Cookies[Constant.Username];
            if (username == null)
            {
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RedirectToLocal
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }
        #endregion

        #region Check Application Session
        public bool CheckAplicationSession()
        {
            bool result = false;
            try
            {
                var curDate = Constant.Date().Date;
                var appEndDate = Convert.ToDateTime("2025-12-31").Date;
                if (appEndDate >= curDate)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in AccountController - CheckAplicationSession : " + ex.Message);
            }
            return result;
        }
        #endregion
    }
}