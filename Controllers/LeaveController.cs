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
    public class LeaveController : Controller
    {
        #region Declaration
        RepoLeave rp = new RepoLeave();
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
        public ActionResult AddEdit(string leaveCode)
        {
            return PartialView(rp.GetDetails(leaveCode));
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(LEAVE leave)
        {
            string result = rp.Save(leave);
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
        public ActionResult Delete(string leaveCode)
        {
            string result = rp.Delete(leaveCode);
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
            return RedirectToAction("Index");
        }
        #endregion

        #region JSON
        #region Check Code Exists
        public JsonResult CheckCodeExists(string leaveCode)
        {
            bool result = rp.CheckCodeExists(leaveCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Leave Exists
        public JsonResult CheckLeaveExists(string leave, string leaveCode, string creBy)
        {
            bool result = rp.CheckLeaveExists(leave, leaveCode, creBy);
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