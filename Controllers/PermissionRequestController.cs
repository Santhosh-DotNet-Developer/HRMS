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
    public class PermissionRequestController : Controller
    {
        #region Declaration
        RepoPermissionRequest rp = new RepoPermissionRequest();
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
            if(TempData["tab"] != null)
            {
                ViewBag.CurTab = TempData["tab"];
            }
            return View(rp.GetList());
        }
        #endregion

        #region Add/Edit
        public ActionResult AddEdit(int perReqId)
        {
            return PartialView(rp.GetDetails(perReqId));
        }
        #endregion

        #region Cancel Reject View
        public ActionResult CancelReject(int perReqId, string type)
        {
            PERMISSION_REQUEST perReqDet = new PERMISSION_REQUEST();
            perReqDet.PR_ID = perReqId;
            ViewBag.Type = type;
            return PartialView(perReqDet);
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(PERMISSION_REQUEST perReq)
        {
            string result = rp.Save(perReq);
            if (result == "Success")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "Permission Request Raised Successfully.";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Index";
            return RedirectToAction("Index");
        }
        #endregion

        #region Cancel/Reject
        public ActionResult CancelReject(PERMISSION_REQUEST perReq, string type)
        {
            string result = rp.CancelReject(perReq, type);
            if (result == "Cancel")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "Permission Request Canceled Successfully.";
            }
            else if (result == "Reject")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "Permission Request Rejected Successfully.";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "History";
            return RedirectToAction("Index");
        }
        #endregion

        #region Approve
        public ActionResult Approve(int perReqId)
        {
            string result = rp.Approve(perReqId);
            if (result == "Approve")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "Permission Request Approved Successfully.";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "History";
            return RedirectToAction("Index");
        }
        #endregion

        #region JSON
        #region Check Permission Exists
        public JsonResult CheckPermissionExists(string empCode, string perTime)
        {
            bool result = rp.CheckPermissionExists(empCode, perTime);
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