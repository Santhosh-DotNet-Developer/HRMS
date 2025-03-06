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
    public class LeaveRequestController : Controller
    {
        #region Declaration
        RepoLeaveRequest rp = new RepoLeaveRequest();
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

        #region Add/Edit
        public ActionResult AddEdit(int levReqId)
        {
            return PartialView(rp.GetDetails(levReqId));
        }
        #endregion

        #region Cancel/Reject View
        public ActionResult CancelReject(int levReqId, string type)
        {
            LEAVE_REQUEST levReqDet = new LEAVE_REQUEST();
            levReqDet.LR_ID = levReqId;
            ViewBag.Type = type;
            return PartialView(levReqDet);
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(LEAVE_REQUEST levReq)
        {
            string result = rp.Save(levReq);
            if (result == "Success")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "Leave Request Raised Successfully.";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelReject(LEAVE_REQUEST levReq, string type)
        {
            string result = rp.CancelReject(levReq, type);
            if (result == "Cancel")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "Leave Request Canceled Successfully.";
            }
            else if (result == "Reject")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "Leave Request Rejected Successfully.";
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

        #region Approve Leave Request
        public ActionResult Approve(int levReqId)
        {
            string result = rp.Approve(levReqId);
            if (result == "Approve")
            {
                TempData["message"] = "CustomS";
                TempData["content"] = "Leave Request Approved Successfully.";
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
        #region Check Leave Exists
        public JsonResult CheckLeaveExists(string empCode, DateTime leaveDate)
        {
            bool result = rp.CheckLeaveExists(empCode, leaveDate);
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