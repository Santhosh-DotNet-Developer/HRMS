using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Repository;
using HRMS.Models;

namespace HRMS.Controllers
{
    public class ManualAttendanceController : Controller
    {
        #region Declaration
        private RepoManualAttendance rp;
        #endregion

        #region Constructor
        public ManualAttendanceController()
        {
            this.rp = new RepoManualAttendance(new HRMSEntities());
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Show = TempData["message"];
            }
            if (TempData["content"] != null)
            {
                ViewBag.Content = TempData["content"];
            }
            return View(rp.GetList());
        }
        #endregion

        #region Add/Edit Manual Attendance
        public ActionResult AddEdit(int manualAttdId)
        {
            return PartialView(rp.GetDetails(manualAttdId));
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(MANUAL_ATTENDANCE manualAttd)
        {
            string result = rp.Save(manualAttd);
            if (result == "Insert")
            {
                TempData["message"] = "Insert";
                TempData["content"] = Constant.Insert;
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
        #region Check Attendance Exists
        public JsonResult CheckAttendanceExists(DateTime date)
        {
            bool result = rp.CheckAttendanceExists(date);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
	}
}