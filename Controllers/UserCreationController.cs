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
        RepoUserCreation rp = new RepoUserCreation();
        #endregion

        #region Index
        public ActionResult Index()
        {
            ViewBag.Show = TempData["message"];
            ViewBag.Content = TempData["content"];
            return View(rp.GetList());
        }
        #endregion

        #region Add/Edit
        public ActionResult AddEdit(string empCode)
        {
            return PartialView(rp.GetDetails(empCode));
        }
        #endregion

        #region Update Password View
        public ActionResult UpdatePassword(string empCode)
        {
            USER_CREATION userCreDet = new USER_CREATION();
            userCreDet.USER_EMP_CODE = empCode;
            return PartialView(userCreDet);
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(USER_CREATION userCreation)
        {
            string result = rp.Save(userCreation);
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

        #region Update Password
        public ActionResult UpdatePassword(string empCode, string newPassword, string screen)
        {
            string result = rp.UpdatePassword(empCode, newPassword);
            if (result == "Update")
            {
                TempData["message"] = "Update";
                TempData["content"] = Constant.Update;
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }

            if (screen == "Dashboard")
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region JSON
        #region Check Username Exists
        public JsonResult CheckUsernameExists(string username)
        {
            bool result = rp.CheckUsernameExists(username);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Employee Exists
        public JsonResult CheckEmployeeExists(string empCode)
        {
            bool result = rp.CheckEmployeeExists(empCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
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