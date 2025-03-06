using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Repository;
using HRMS.Models;

namespace HRMS.Controllers
{
    [CheckSessionTimeOut]
    public class DepartmentController : Controller
    {
        #region Declaration
        IRepository<DEPARTMENT> rp = new RepoDepartment();
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
        public ActionResult AddEdit(string deptCode)
        {
            return PartialView(rp.GetDetails(deptCode));
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(DEPARTMENT department)
        {
            string result = rp.Save(department);
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
        public ActionResult Delete(string deptCode)
        {
            string result = rp.Delete(deptCode);
            if (result == "Delete")
            {
                TempData["message"] = "Delete";
                TempData["content"] = Constant.Delete;
            }
            else if (result == "Child")
            {
                TempData["message"] = "Child";
                TempData["content"] = "Cannot delete this department, as it has a dependency in the application.";
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
        #region Check Department Code Exists
        public JsonResult CheckDeptCodeExists(string deptCode)
        {
            bool result = rp.CheckDeptCodeExists(deptCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Department Name Exists
        public JsonResult CheckDeptNameExists(string deptName, string deptCode, string creBy)
        {
            bool result = rp.CheckDeptNameExists(deptName, deptCode, creBy);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
	}
}