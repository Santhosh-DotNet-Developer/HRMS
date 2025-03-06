using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Repository;
using System.IO;
using HRMS.Models;

namespace HRMS.Controllers
{
    [CheckSessionTimeOut]
    public class BusEntitiesController : Controller
    {
        #region Declaration
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        RepoBusEntities rp = new RepoBusEntities();
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
            if (TempData["tab"] != null)
            {
                ViewBag.CurTab = TempData["tab"];
            }
            var test = TempData["message"];
            return View(rp.GetList());
        }
        #endregion

        #region Add/Edit for Unit
        public ActionResult AddEditUnit(Int16 unitId)
        {
            return PartialView(rp.GetUnitDetails(unitId));
        }
        #endregion

        #region Add/Edit for Branch
        public ActionResult AddEditBranch(Int16 branchId)
        {
            return PartialView(rp.GetBranchDetails(branchId));
        }
        #endregion

        #region Save Entity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEntity([Bind(Prefix = "Item1")]BUS_ENTITIES entity, HttpPostedFileBase entityLogo)
        {
            #region Entity Logo for Insert/Update
            try
            {
                #region Delete Previous Entity Logo
                if (entity.BE_ID > 0)
                {
                    try
                    {
                        BUS_ENTITIES entityDet = db.BUS_ENTITIES.Where(m => m.BE_ID == entity.BE_ID).SingleOrDefault();
                        if (entityDet != null)
                        {
                            string logoPath = entityDet.BE_LOGO_URL;
                            if (string.IsNullOrEmpty(logoPath) == false && (entityLogo != null || string.IsNullOrEmpty(entity.BE_LOGO_URL) == true))
                            {
                                string subFolder = logoPath.Substring(logoPath.LastIndexOf("Upload/"));
                                string path = Path.Combine(Server.MapPath("~\\"), subFolder);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception in BusEntitiesController - SaveEntity : " + ex.Message);
                    }
                }
                #endregion

                if (entityLogo != null)
                {
                    string subFolder = "~/Upload/Business Entities/" + entity.BE_ID + "/";
                    string fileName = Path.GetFileName(entityLogo.FileName);
                    var path = Path.Combine(Server.MapPath(subFolder), fileName);
                    if (!Directory.Exists(Server.MapPath(subFolder)))
                    {
                        Directory.CreateDirectory(Server.MapPath(subFolder));
                    }
                    entity.BE_LOGO_URL = "../Upload/Business Entities/" + entity.BE_ID + "/" + fileName;
                    entityLogo.SaveAs(path);
                    entity.BE_LOGO = ConvertToByte(entityLogo);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in BusEntitiesController - SaveEntity : " + ex.Message);
            }
            #endregion

            string result = rp.SaveEntity(entity);
            if (result == "Update")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Update;
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Entity";
            return RedirectToAction("Index");
        }
        #endregion

        #region Save Unit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUnit(BUS_UNITS unit, HttpPostedFileBase unitLogo)
        {
            #region Unit Logo Insert/Update
            try
            {
                #region Delete Previous Unit Logo
                if (unit.BU_ID > 0)
                {
                    try
                    {
                        BUS_UNITS unitDet = db.BUS_UNITS.Where(m => m.BU_ID == unit.BU_ID).SingleOrDefault();
                        if (unitDet != null)
                        {
                            string logoPath = unitDet.BU_LOGO_URL;
                            if (string.IsNullOrEmpty(logoPath) == false && (unitLogo != null || string.IsNullOrEmpty(unit.BU_LOGO_URL) == true))
                            {
                                string subFolder = logoPath.Substring(logoPath.LastIndexOf("Upload/"));
                                string path = Path.Combine(Server.MapPath("~\\"), subFolder);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception in BusEntitiesController - SaveUnit : " + ex.Message);
                    }
                }
                #endregion

                if (unitLogo != null)
                {
                    string subFolder = "~/Upload/Business Units/" + unit.BU_ID + "/";
                    string fileName = Path.GetFileName(unitLogo.FileName);
                    var path = Path.Combine(Server.MapPath(subFolder), fileName);
                    if (!Directory.Exists(Server.MapPath(subFolder)))
                    {
                        Directory.CreateDirectory(Server.MapPath(subFolder));
                    }
                    unit.BU_LOGO_URL = "../Upload/Business Units/" + unit.BU_ID + "/" + fileName;
                    unitLogo.SaveAs(path);
                    unit.BU_LOGO = ConvertToByte(unitLogo);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in BusEntitiesController - SaveUnit : " + ex.Message);
            }
            #endregion

            string result = rp.SaveUnit(unit);
            if (result == "Insert")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Insert;
            }
            else if (result == "Update")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Update;
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Unit";
            return RedirectToAction("Index");
        }
        #endregion

        #region Save Branch
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBranch(BUS_BRANCH branch, HttpPostedFileBase branchLogo)
        {
            #region Branch Logo Insert/Update
            try
            {
                #region Delete Previous Branch Logo
                if (branch.BR_ID > 0)
                {
                    try
                    {
                        Int16 BE = Convert.ToInt16(HttpContext.Request.Cookies[Constant.BE].Value);
                        Int16 BU = Convert.ToInt16(HttpContext.Request.Cookies[Constant.BU].Value);
                        BUS_BRANCH branchDet = db.BUS_BRANCH.Where(w => w.BR_BE == BE && w.BR_BU == BU && w.BR_ID == branch.BR_ID).SingleOrDefault();
                        if (branchDet != null)
                        {
                            string logoPath = branchDet.BR_LOGO_URL;
                            if (string.IsNullOrEmpty(logoPath) == false && (branchLogo != null || string.IsNullOrEmpty(branch.BR_LOGO_URL) == true))
                            {
                                string subFolder = logoPath.Substring(logoPath.LastIndexOf("Upload/"));
                                string path = Path.Combine(Server.MapPath("~\\"), subFolder);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception in BusEntitiesController - SaveBranch : " + ex.Message);
                    }
                }
                #endregion

                if (branchLogo != null)
                {
                    string subFolder = "~/Upload/Business Branches/" + branch.BR_ID + "/";
                    string fileName = Path.GetFileName(branchLogo.FileName);
                    var path = Path.Combine(Server.MapPath(subFolder), fileName);
                    if (!Directory.Exists(Server.MapPath(subFolder)))
                    {
                        Directory.CreateDirectory(Server.MapPath(subFolder));
                    }
                    branch.BR_LOGO_URL = "../Upload/Business Branches/" + branch.BR_ID + "/" + fileName;
                    branchLogo.SaveAs(path);
                    branch.BR_LOGO = ConvertToByte(branchLogo);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in BusEntitiesController - SaveBranch : " + ex.Message);
            }
            #endregion

            string result = rp.SaveBranch(branch);
            if (result == "Insert")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Insert;
            }
            else if (result == "Update")
            {
                TempData["message"] = result;
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

        #region Delete Unit
        public ActionResult DeleteUnit(Int16 unitId)
        {
            string result = rp.DeleteUnit(unitId);
            if (result == "Delete")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Delete;
            }
            else if (result == "Child")
            {
                TempData["message"] = result;
                TempData["content"] = "Cannot delete this unit, as it has dependencies in this application";
            }
            else
            {
                TempData["content"] = result;
                TempData["message"] = "Error";
            }
            TempData["tab"] = "Unit";
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete Branch
        public ActionResult DeleteBranch(Int16 branchId)
        {
            string result = rp.DeleteBranch(branchId);
            if (result == "Delete")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Delete;
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
        #region Check Unit Name Exists
        public JsonResult CheckUnitNameExists(string unitName, Int16 unitId)
        {
            bool isExist = rp.CheckUnitNameExists(unitName, unitId);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Branch Name Exists
        public JsonResult CheckBranchNameExists(Int16 unitId, string branchName, int branchId)
        {
            bool isExist = rp.CheckBranchNameExists(unitId, branchName, branchId);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get State Country
        public JsonResult GetStateCountry(int cityId)
        {
            List<string> list = rp.GetStateCountry(cityId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Byte Convertion
        public byte[] ConvertToByte(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes((int)file.ContentLength);
            return imageByte;
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                rp.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}