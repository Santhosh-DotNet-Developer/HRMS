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
    public class WorkDayCalendarController : Controller
    {
        #region Declaration
        RepoWorkDayCalendar Rp = new RepoWorkDayCalendar();
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
            return View(Rp.GetList());
        }
        #endregion

        #region Add/Edit Calendar
        public ActionResult AddEditCalendar(int docNo)
        {
            if (TempData["message"] != null)
            {
                ViewBag.Show = TempData["message"];
                TempData["message"] = null;
                ViewBag.Content = TempData["content"];
                TempData["content"] = null;
            }
            if (TempData["tab"] != null)
            {
                ViewBag.CurTab = TempData["tab"];
            }
            return View(Rp.GetSingle(docNo));
        }
        #endregion

        #region Add/Edit Holiday
        public ActionResult AddEditHoliday(int docNo, int seqNo)
        {
            return PartialView(Rp.GetHolidaySingle(docNo, seqNo));
        }
        #endregion

        #region Save Header
        [ValidateAntiForgeryToken]
        public ActionResult SaveHeader(RepoWorkDayCalendar Data)
        {
            string result = Rp.SaveHeader(Data);
            int docNo = 0;
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

            using(HRMSEntities db = new HRMSEntities())
            {
                docNo = db.WORK_DAY_CALENDAR_HD.Where(w => w.WDCH_YEAR == Data.hdSingle.WDCH_YEAR).Select(s => s.WDCH_DOC_NO).Single();
            }
            return RedirectToAction("AddEditCalendar", "WorkDayCalendar", new { docNo = docNo });
        }
        #endregion

        #region Generate Calendar
        public ActionResult GenerateCalendar(int docNo)
        {
            string result = Rp.GenerateCalendar(docNo);
            if (result == "Generated")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "Generated Successfully";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Calendar";
            return RedirectToAction("AddEditCalendar", "WorkDayCalendar", new { docNo = docNo });
        }
        #endregion

        #region Save Holiday
        [ValidateAntiForgeryToken]
        public ActionResult SaveHoliday(WORK_DAY_CALENDAR_HOLIDAYS Data)
        {
            string result = Rp.SaveHoliday(Data);
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
            TempData["tab"] = "Holiday";
            return RedirectToAction("AddEditCalendar", "WorkDayCalendar", new { docNo = Data.WDCHO_DOC_NO });
        }
        #endregion

        #region UpdateWeekOff
        [ValidateAntiForgeryToken]
        public ActionResult UpdateWeekOff(RepoWorkDayCalendar Data)
        {
            string result = Rp.UpdateWeekOff(Data);
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
            TempData["tab"] = "Holiday";
            return RedirectToAction("AddEditCalendar", "WorkDayCalendar", new { docNo = Data.hdSingle.WDCH_DOC_NO });
        }
        #endregion

        #region Activate Calendar
        public ActionResult ActivateCalendar(int docNo)
        {
            string result = Rp.ActivateCalendar(docNo);
            if (result == "Activate")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "Activated Successfully";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Calendar";
            return RedirectToAction("AddEditCalendar", "WorkDayCalendar", new { docNo = docNo });
        }
        #endregion

        #region DeActivate Calendar
        public ActionResult DeActivateCalendar(int docNo)
        {
            string result = Rp.DeActivateCalendar(docNo);
            if (result == "DeActivate")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "DeActivated Successfully";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Calendar";
            return RedirectToAction("AddEditCalendar", "WorkDayCalendar", new { docNo = docNo });
        }
        #endregion

        #region Delete Holiday
        public ActionResult DeleteHoliday(int docNo, int seqNo)
        {
            string message = Rp.DeleteHoliday(docNo, seqNo);
            if (message == "Delete")
            {
                TempData["message"] = "Delete";
                TempData["content"] = Constant.Delete;
            }
            else
            {
                TempData["content"] = message;
                TempData["message"] = "Error";
            }
            TempData["tab"] = "Holiday";
            return RedirectToAction("AddEditCalendar", "WorkDayCalendar", new { docNo = docNo });
        }
        #endregion

        #region Json
        #region Check Calendar Exists
        public JsonResult CheckExists(Int16 unit, int year)
        {
            bool result = Rp.CheckExists(unit, year);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Holiday Exists
        public JsonResult CheckHolidayExists(int docNo, DateTime date)
        {
            bool result = Rp.CheckHolidayExists(docNo, date);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Active Calendar
        public JsonResult CheckActiveCalendar()
        {
            bool result = Rp.CheckActiveCalendar();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
	}
}