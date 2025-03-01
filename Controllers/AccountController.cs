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
        private HRMSEntities db = new HRMSEntities();
        #endregion

        #region Login View
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string type)
        {
            if (type == "Reset")
            {
                string inUserName = Constant.SessionUser;
                USER_CREATION inUserData = db.USER_CREATION.Where(w => w.USER_NAME == inUserName).SingleOrDefault();
                if(inUserData != null)
                {
                    EMPLOYEE inEmpData = db.EMPLOYEES.Where(w => w.EMP_CODE == inUserData.USER_EMP_CODE).SingleOrDefault();
                    ROLE inRoleDet = db.ROLEs.Where(w => w.ROLE_ID == inUserData.USER_ROLE_ID).SingleOrDefault();
                    this.ClearCookies();

                    HttpCookie BE = new HttpCookie(Constant.BE);
                    BE.Value = inUserData.USER_BE.ToString();
                    BE.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                    Response.SetCookie(BE);

                    HttpCookie BU = new HttpCookie(Constant.BU);
                    BU.Value = inUserData.USER_BU.ToString();
                    BU.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                    Response.SetCookie(BU);

                    HttpCookie Username = new HttpCookie(Constant.Username);
                    Username.Value = inUserData.USER_NAME;
                    Username.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                    Response.SetCookie(Username);

                    HttpCookie EmpCode = new HttpCookie(Constant.EmpCode);
                    EmpCode.Value = inEmpData.EMP_CODE;
                    EmpCode.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                    Response.SetCookie(EmpCode);

                    HttpCookie EmpName = new HttpCookie(Constant.EmpName);
                    EmpName.Value = inEmpData.EMP_FIRST_NAME;
                    EmpName.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                    Response.SetCookie(EmpName);

                    HttpCookie Role = new HttpCookie(Constant.Role);
                    Role.Value = inRoleDet.ROLE_DESC;
                    Role.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                    Response.SetCookie(Role);

                    HttpCookie RoleID = new HttpCookie(Constant.RoleID);
                    RoleID.Value = inRoleDet.ROLE_ID.ToString();
                    RoleID.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                    Response.SetCookie(RoleID);

                    Constant.SessionExpiryTime = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                    Constant.SessionUser = inUserData.USER_NAME;
                }
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
                if(App_Session.validTrail())
                {
                    try
                    {
                        EncryptDecrypt enc = new EncryptDecrypt();
                        model.Password = enc.Encrypt(model.Password);
                        USER_CREATION inUserData = db.USER_CREATION.Where(w => w.USER_NAME == model.UserName && w.USER_PSWD == model.Password && w.USER_STATUS == true).SingleOrDefault();
                        if (inUserData != null)
                        {
                            EMPLOYEE inEmpData = db.EMPLOYEES.Where(w => w.EMP_CODE == inUserData.USER_EMP_CODE).SingleOrDefault();
                            this.ClearCookies();

                            HttpCookie BE = new HttpCookie(Constant.BE);
                            BE.Value = inUserData.USER_BE.ToString();
                            BE.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                            Response.SetCookie(BE);

                            HttpCookie BU = new HttpCookie(Constant.BU);
                            BU.Value = inUserData.USER_BU.ToString();
                            BU.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                            Response.SetCookie(BU);

                            HttpCookie Username = new HttpCookie(Constant.Username);
                            Username.Value = inUserData.USER_NAME;
                            Username.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                            Response.SetCookie(Username);

                            HttpCookie EmpCode = new HttpCookie(Constant.EmpCode);
                            EmpCode.Value = inEmpData.EMP_CODE;
                            EmpCode.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                            Response.SetCookie(EmpCode);

                            HttpCookie EmpName = new HttpCookie(Constant.EmpName);
                            EmpName.Value = inEmpData.EMP_FIRST_NAME;
                            EmpName.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                            Response.SetCookie(EmpName);

                            HttpCookie Role = new HttpCookie(Constant.Role);
                            Role.Value = inUserData.ROLE.ROLE_DESC;
                            Role.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                            Response.SetCookie(Role);

                            HttpCookie RoleID = new HttpCookie(Constant.RoleID);
                            RoleID.Value = inUserData.USER_ROLE_ID.ToString();
                            RoleID.Expires = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                            Response.SetCookie(RoleID);

                            Constant.SessionExpiryTime = DateTime.Now.AddMinutes(Constant.cTimeOutMin);
                            Constant.SessionUser = inUserData.USER_NAME;
                            Constant.UserExist = true;

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
                        string errorMessage = null;
                        string msgs = "Message: " + ex.Message;
                        if (ex.InnerException != null)
                        {
                            msgs += "\r\nInnerException: " + ex.InnerException.Message;
                        }
                        errorMessage = msgs;
                        ViewBag.Error = errorMessage;
                    }
                }
                else
                {
                    string errMsg = "Validity Expired. Please contact Roadmap IT Solutions!";
                    ViewBag.Error = errMsg;
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region SessionOut
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SessionOut()
        {
            return View("SessionOut");
        }
        #endregion

        #region LogOff
        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session.Abandon();
            this.ClearCookies();
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region ClearCookies
        public void ClearCookies()
        {
            try
            {
                if (Request.Cookies[Constant.BE] != null)
                {
                    HttpCookie BECookies = new HttpCookie(Constant.BE);
                    BECookies.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(BECookies);
                }
                if (Request.Cookies[Constant.BU] != null)
                {
                    HttpCookie BUCookies = new HttpCookie(Constant.BU);
                    BUCookies.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(BUCookies);
                }
                if (Request.Cookies[Constant.Username] != null)
                {
                    HttpCookie UserNameCookies = new HttpCookie(Constant.Username);
                    UserNameCookies.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(UserNameCookies);
                }
                if (Request.Cookies[Constant.EmpCode] != null)
                {
                    HttpCookie EmpIdCookies = new HttpCookie(Constant.EmpCode);
                    EmpIdCookies.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(EmpIdCookies);
                }
                if (Request.Cookies[Constant.EmpName] != null)
                {
                    HttpCookie EmpNameCookies = new HttpCookie(Constant.EmpName);
                    EmpNameCookies.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(EmpNameCookies);
                }
                if (Request.Cookies[Constant.Role] != null)
                {
                    HttpCookie RoleNameCookies = new HttpCookie(Constant.Role);
                    RoleNameCookies.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(RoleNameCookies);
                }
                if (Request.Cookies[Constant.RoleID] != null)
                {
                    HttpCookie RoleIDCookies = new HttpCookie(Constant.RoleID);
                    RoleIDCookies.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(RoleIDCookies);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in AccountController - ClearCookies : " + ex.Message);
                string errorMessage = null;
                string msgs = "Message: " + ex.Message;
                if (ex.InnerException != null)
                    msgs += "\r\nInnerException: " + ex.InnerException.Message;

                errorMessage = msgs;
                ViewBag.Error = errorMessage;
            }
        }
        #endregion

        #region Helpers
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

        #region CheckSessionStatus
        [HttpGet]
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
    }
}