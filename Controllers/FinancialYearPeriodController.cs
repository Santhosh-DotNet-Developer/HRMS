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
    public class FinancialYearPeriodController : Controller
    {
        #region Declaration
        RepoFinancialYearPeriod rp = new RepoFinancialYearPeriod();
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
        public ActionResult AddEdit(int year)
        {
            return PartialView(rp.GetDetails(year));
        }
        #endregion

        #region Update
        public ActionResult Update([Bind(Prefix = "Item1")] FINANCIAL_YEAR finYear, [Bind(Prefix = "Item2")] List<FINANCIAL_PERIOD> finPeriodLst)
        {
            string result = rp.Update(finYear, finPeriodLst);
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
            return RedirectToAction("Index");
        }
        #endregion

        #region JSON
        #region Check Status
        public JsonResult CheckStatus(string status)
        {
            bool result = rp.CheckStatus(status);
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