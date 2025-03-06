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
    public class EmployeeGradeSalaryGroupController : Controller
    {
        #region Declaration
        RepoEmployeeGradeSalaryGroup rp = new RepoEmployeeGradeSalaryGroup();
        #endregion

        #region Index
        public ActionResult Index()
        {
            ViewBag.Show = TempData["message"];
            ViewBag.Content = TempData["content"];
            ViewBag.CurTab = TempData["tab"];
            return View(rp.GetList());
        }
        #endregion

        #region Add/Edit for Grade
        public ActionResult AddEditEmpGrade(string empGradeCode)
        {
            return PartialView(rp.GetEmpGradeDetails(empGradeCode));
        }
        #endregion

        #region Add/Edit for Salary Group
        public ActionResult AddEditSalaryGrp(string salaryGrpCode)
        {
            return PartialView(rp.GetSalaryGrpDetails(salaryGrpCode));
        }
        #endregion

        #region Save for Employee Grade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEmployeeGrade(EMPLOYEE_GRADE empGrade)
        {
            string result = rp.SaveEmployeeGrade(empGrade);
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
            TempData["tab"] = "EmpGrade";
            return RedirectToAction("Index");
        }
        #endregion

        #region Save for Salary Group
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSalaryGroup(SALARY_GROUP salaryGrp)
        {
            string result = rp.SaveSalaryGroup(salaryGrp);
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
            TempData["tab"] = "SalaryGrp";
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete for Employee Grade
        public ActionResult DeleteEmployeeGrade(string empGradeCode)
        {
            string result = rp.DeleteEmployeeGrade(empGradeCode);
            if (result == "Delete")
            {
                TempData["message"] = "Delete";
                TempData["content"] = Constant.Delete;
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Bank";
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete for Salary Group
        public ActionResult DeleteSalaryGroup(string salaryGrpCode)
        {
            string result = rp.DeleteSalaryGroup(salaryGrpCode);
            if (result == "Delete")
            {
                TempData["message"] = "Delete";
                TempData["content"] = Constant.Delete;
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "SalaryGrp";
            return RedirectToAction("Index");
        }
        #endregion

        #region JSON
        #region Check Employee Grade Code Exists
        public JsonResult CheckEmployeeGradeCodeExists(string empGradeCode)
        {
            bool result = rp.CheckEmployeeGradeCodeExists(empGradeCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Salary Group Code Exists
        public JsonResult CheckSalaryGroupCodeExists(string salaryGrpCode)
        {
            bool result = rp.CheckSalaryGroupCodeExists(salaryGrpCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Employee Grade Exists
        public JsonResult CheckEmployeeGradeExists(string empGrade, string empGradeCode, string creBy)
        {
            bool result = rp.CheckEmployeeGradeExists(empGrade, empGradeCode, creBy);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Salary Group Exists
        public JsonResult CheckSalaryGroupExists(string salaryGrp, string salaryGrpCode, string creBy)
        {
            bool result = rp.CheckSalaryGroupExists(salaryGrp, salaryGrpCode, creBy);
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