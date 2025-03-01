using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Models;
using HRMS.Repository;

namespace HRMS.Controllers
{
    [CheckSessionTimeOut]
    public class UserCreationController : Controller
    {
        #region Declaration
        private RepoUserCreation rp;
        #endregion

        #region Constructor
        public UserCreationController()
        {
            this.rp = new RepoUserCreation(new HRMSEntities());
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Show = TempData["message"];
                TempData["message"] = null;
                ViewBag.Content = TempData["content"];
                TempData["content"] = null;
            }
            return View(rp.GetList());
        }
        #endregion

        #region Add/Edit User
        public ActionResult AddEditUser(int inUserId)
        {
            return PartialView(rp.GetSingle(inUserId));
        }
        #endregion

        #region Save
        [ValidateAntiForgeryToken]
        public ActionResult Save(USER_CREATION inData)
        {
            string result = rp.Save(inData);
            if (result == "Insert")
            {
                TempData["message"] = "Insert";
                TempData["content"] = Constant.Insert;
            }
            else if (result == "Update")
            {
                TempData["message"] = "Update";
                TempData["content"] = Constant.Update;
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Json
        #region Check Username Exists
        public JsonResult CheckUsernameExists(string inUsername)
        {
            bool result = rp.CheckUsernameExists(inUsername);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Employee Exists
        public JsonResult CheckEmployeeExists(string inEmpCode)
        {
            bool result = rp.CheckEmployeeExists(inEmpCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update Password
        public JsonResult UpdatePassword(int inUserId, string inNewPassword)
        {
            bool result = rp.UpdatePassword(inUserId, inNewPassword);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Destructor
        ~UserCreationController()
        {
            Dispose(false);
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rp.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}