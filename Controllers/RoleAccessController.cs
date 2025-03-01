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
    public class RoleAccessController : Controller
    {
        #region Declaration
        private RepoRoleAccess rp;
        #endregion

        #region Constructor
        public RoleAccessController()
        {
            this.rp = new RepoRoleAccess(new HRMSEntities());
        }
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
            return View(rp.GetList());
        }
        #endregion

        #region Add/Edit Role Access
        public ActionResult AddEdit(int inRoleId)
        {
            return PartialView(rp.GetSingle(inRoleId));
        }
        #endregion

        #region Save
        [ValidateAntiForgeryToken]
        public ActionResult Save(RoleAccessModel inData)
        {
            string result = rp.Save(inData);
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
        public ActionResult Delete(int inRoleId)
        {
            string result = rp.Delete(inRoleId);
            if (result == "Delete")
            {
                TempData["message"] = "Delete";
                TempData["content"] = Constant.Delete;
            }
            else if (result == "Child")
            {
                TempData["message"] = "Child";
                TempData["content"] = "Cannot delete this role, as it has a dependency in the application.";
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
        #region Check Role Exists
        public JsonResult CheckRoleExists(string inRoleDesc)
        {
            bool result = rp.CheckRoleExists(inRoleDesc);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Destructor
        ~RoleAccessController()
        {
            Dispose(false);
        }
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