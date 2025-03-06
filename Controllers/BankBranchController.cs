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
    public class BankBranchController : Controller
    {
        #region Declaration
        RepoBankBranch rp = new RepoBankBranch();
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

        #region Add/Edit for Bank
        public ActionResult AddEditBank(string bankCode)
        {
            return PartialView(rp.GetBankDetails(bankCode));
        }
        #endregion

        #region Add/Edit for Bank Branch
        public ActionResult AddEditBankBranch(int bankBranchId)
        {
            return PartialView(rp.GetBankBranchDetails(bankBranchId));
        }
        #endregion

        #region Save for Bank
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBank(BANK bank)
        {
            string result = rp.SaveBank(bank);
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
            TempData["tab"] = "Bank";
            return RedirectToAction("Index");
        }
        #endregion

        #region Save for Bank Branch
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBankBranch(BANK_BRANCHES bankBranch)
        {
            string result = rp.SaveBankBranch(bankBranch);
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
            TempData["tab"] = "Branch";
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete for Bank
        public ActionResult DeleteBank(string bankCode)
        {
            string result = rp.DeleteBank(bankCode);
            if (result == "Delete")
            {
                TempData["message"] = "Delete";
                TempData["content"] = Constant.Delete;
            }
            else if (result == "Child")
            {
                TempData["message"] = "Child";
                TempData["content"] = "Cannot delete this bank, as it has a dependency in the application.";
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

        #region Delete for Bank Branch
        public ActionResult DeleteBankBranch(int bankBranchId)
        {
            string result = rp.DeleteBankBranch(bankBranchId);
            if (result == "Delete")
            {
                TempData["message"] = "Delete";
                TempData["content"] = Constant.Delete;
            }
            else if (result == "Child")
            {
                TempData["message"] = "Child";
                TempData["content"] = "Cannot delete this bank branch, as it has a dependency in the application.";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Branch";
            return RedirectToAction("Index");
        }
        #endregion

        #region JSON
        #region Check Bank Code Exists
        public JsonResult CheckBankCodeExists(string bankCode)
        {
            bool result = rp.CheckBankCodeExists(bankCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Bank Name Exists
        public JsonResult CheckBankNameExists(string bankName, string bankCode, string creBy)
        {
            bool result = rp.CheckBankNameExists(bankName, bankCode, creBy);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Bank Branch Name Exists
        public JsonResult CheckBankBranchNameExists(string bankCode, string bankBranchName, int bankBranchId, string creBy)
        {
            bool result = rp.CheckBankBranchNameExists(bankCode, bankBranchName, bankBranchId, creBy);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check IFSC Code Exists
        public JsonResult CheckIFSCCodeExists(string IFSCCode, int bankBranchId, string creBy)
        {
            bool result = rp.CheckIFSCCodeExists(IFSCCode, bankBranchId, creBy);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get State Country
        public JsonResult GetStateCountry(int cityId)
        {
            var list = DropDownList.GetStateCountry(cityId);
            return Json(list, JsonRequestBehavior.AllowGet);
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