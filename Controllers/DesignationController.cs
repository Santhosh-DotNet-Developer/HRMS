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
    public class DesignationController : Controller
    {
        #region Declaration
        RepoDesignation rp = new RepoDesignation();
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
        public ActionResult AddEdit(string desgCode)
        {
            return PartialView(rp.GetDetails(desgCode));
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(DESIGNATION designation)
        {
            string result = rp.Save(designation);
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

        #region Delete
        public ActionResult Delete(string desgCode)
        {
            string result = rp.Delete(desgCode);
            if (result == "Delete")
            {
                TempData["message"] = "Delete";
                TempData["content"] = Constant.Delete;
            }
            else if (result == "Child")
            {
                TempData["message"] = "Child";
                TempData["content"] = "Cannot delete this designation, as it has a dependency in the application.";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region JSON
        #region Check Designation Code Exists
        public JsonResult CheckDesgCodeExists(string desgCode)
        {
            bool result = rp.CheckDesgCodeExists(desgCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Designation Name Exists
        public JsonResult CheckDesgNameExists(string desgName, string desgCode, string creBy)
        {
            bool result = rp.CheckDesgNameExists(desgName, desgCode, creBy);
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