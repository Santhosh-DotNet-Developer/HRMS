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
        RepoBusEntities rp;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Constructor
        public BusEntitiesController()
        {
            this.rp = new RepoBusEntities(new HRMSEntities());
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
            if (TempData["tab"] != null)
            {
                ViewBag.CurTab = TempData["tab"];
            }
            var test = TempData["message"];
            return View(rp.GetList());
        }
        #endregion

        #region Add/Edit for Unit
        public ActionResult AddUnit(Int16 unitId)
        {
            return PartialView(rp.GetUnitDetails(unitId));
        }
        #endregion

        #region Add/Edit for Branch
        public ActionResult AddBranch(Int16 branchId)
        {
            return PartialView(rp.GetBranchDetails(branchId));
        }
        #endregion

        #region Save Entity
        [ValidateAntiForgeryToken]
        public ActionResult SaveEntity([Bind(Prefix = "Item1")]BUS_ENTITIES inData, HttpPostedFileBase file)
        {
            #region Image
            try
            {
                #region Delete Previous Image
                if (inData.BE_ID > 0)
                {
                    try
                    {
                        BUS_ENTITIES inSingleData = db.BUS_ENTITIES.Where(m => m.BE_ID == inData.BE_ID).SingleOrDefault();
                        if (inSingleData != null)
                        {
                            string imgPath = inSingleData.BE_LOGO_URL;
                            if (string.IsNullOrEmpty(imgPath) == false && (file != null || string.IsNullOrEmpty(inData.BE_LOGO_URL) == true))
                            {
                                string subFolder = imgPath.Substring(imgPath.LastIndexOf("Upload/"));
                                string path = Path.Combine(Server.MapPath("~\\"), subFolder);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                else
                                {
                                    log.Debug("File not found in BusEntitiesController - SaveEntity");
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

                if (file != null)
                {
                    string subFolder = "~/Upload/Business Entities/" + inData.BE_ID + "/";
                    string fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath(subFolder), fileName);
                    bool checkFileExist = Directory.Exists(Server.MapPath(subFolder));
                    if (checkFileExist == false)
                    {
                        Directory.CreateDirectory(Server.MapPath(subFolder));
                    }
                    inData.BE_LOGO_URL = "../Upload/Business Entities/" + inData.BE_ID + "/" + fileName;
                    file.SaveAs(path);
                    inData.BE_LOGO = ConvertToByte(file);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in BusEntitiesController - SaveEntity : " + ex.Message);
            }
            #endregion

            string result = rp.SaveEntity(inData);
            if (result == "Update")
            {
                TempData["message"] = result;
                TempData["content"] = "Updated Successfully";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Entity";
            return RedirectToAction("Index", "BusEntities");
        }
        #endregion

        #region Save Unit
        [ValidateAntiForgeryToken]
        public ActionResult SaveUnit(BUS_UNITS inData, HttpPostedFileBase inUnitImg)
        {
            #region Image
            try
            {
                #region Delete Previous Image
                if (inData.BU_ID > 0)
                {
                    try
                    {
                        BUS_UNITS inUnitData = db.BUS_UNITS.Where(m => m.BU_ID == inData.BU_ID).SingleOrDefault();
                        if (inUnitData != null)
                        {
                            string imgPath = inUnitData.BU_LOGO_URL;
                            if (string.IsNullOrEmpty(imgPath) == false && (inUnitImg != null || string.IsNullOrEmpty(inData.BU_LOGO_URL) == true))
                            {
                                string subFolder = imgPath.Substring(imgPath.LastIndexOf("Upload/"));
                                string path = Path.Combine(Server.MapPath("~\\"), subFolder);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                else
                                {
                                    log.Debug("File not found in BusEntitiesController - SaveUnit");
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

                if (inUnitImg != null)
                {
                    string subFolder = "~/Upload/Business Units/" + inData.BU_ID + "/";
                    string fileName = Path.GetFileName(inUnitImg.FileName);
                    var path = Path.Combine(Server.MapPath(subFolder), fileName);
                    bool checkFileExist = Directory.Exists(Server.MapPath(subFolder));
                    if (checkFileExist == false)
                    {
                        Directory.CreateDirectory(Server.MapPath(subFolder));
                    }
                    inData.BU_LOGO_URL = "../Upload/Business Units/" + inData.BU_ID + "/" + fileName;
                    inUnitImg.SaveAs(path);
                    inData.BU_LOGO = ConvertToByte(inUnitImg);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in BusEntitiesController - SaveUnit : " + ex.Message);
            }
            #endregion

            string result = rp.SaveUnit(inData);
            if (result == "Insert")
            {
                TempData["message"] = result;
                TempData["content"] = "Inserted Successfully";
            }
            else if (result == "Update")
            {
                TempData["message"] = result;
                TempData["content"] = "Updated Successfully";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Unit";
            return RedirectToAction("Index", "BusEntities");
        }
        #endregion

        #region Save Branch
        [ValidateAntiForgeryToken]
        public ActionResult SaveBranch(BUS_BRANCH inData, HttpPostedFileBase inBranchImg)
        {
            #region Image
            try
            {
                #region Delete Previous Image
                if (inData.BR_ID > 0)
                {
                    try
                    {
                        BUS_BRANCH inBranchData = db.BUS_BRANCH.Where(m => m.BR_ID == inData.BR_ID).SingleOrDefault();
                        if (inBranchData != null)
                        {
                            string imgPath = inBranchData.BR_LOGO_URL;
                            if (string.IsNullOrEmpty(imgPath) == false && (inBranchImg != null || string.IsNullOrEmpty(inData.BR_LOGO_URL) == true))
                            {
                                string subFolder = imgPath.Substring(imgPath.LastIndexOf("Upload/"));
                                string path = Path.Combine(Server.MapPath("~\\"), subFolder);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                else
                                {
                                    log.Debug("File not found in BusEntitiesController - SaveBranch");
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

                if (inBranchImg != null)
                {
                    string subFolder = "~/Upload/Business Branches/" + inData.BR_ID + "/";
                    string fileName = Path.GetFileName(inBranchImg.FileName);
                    var path = Path.Combine(Server.MapPath(subFolder), fileName);
                    bool checkFileExist = Directory.Exists(Server.MapPath(subFolder));
                    if (checkFileExist == false)
                    {
                        Directory.CreateDirectory(Server.MapPath(subFolder));
                    }
                    inData.BR_LOGO_URL = "../Upload/Business Branches/" + inData.BR_ID + "/" + fileName;
                    inBranchImg.SaveAs(path);
                    inData.BR_LOGO = ConvertToByte(inBranchImg);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in BusEntitiesController - SaveBranch : " + ex.Message);
            }
            #endregion

            string result = rp.SaveBranch(inData);
            if (result == "Insert")
            {
                TempData["message"] = result;
                TempData["content"] = "Inserted Successfully";
            }
            else if (result == "Update")
            {
                TempData["message"] = result;
                TempData["content"] = "Updated Successfully";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Branch";
            return RedirectToAction("Index", "BusEntities");
        }
        #endregion

        #region Delete Unit
        public ActionResult DeleteUnit(Int16 unitId)
        {
            string result = rp.DeleteUnit(unitId);
            if (result == "Delete")
            {
                TempData["message"] = result;
                TempData["content"] = "Deleted Successfully";
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
            return RedirectToAction("Index", "BusEntities");
        }
        #endregion

        #region Delete Branch
        public ActionResult DeleteBranch(Int16 branchId)
        {
            string result = rp.DeleteBranch(branchId);
            if (result == "Delete")
            {
                TempData["message"] = result;
                TempData["content"] = "Deleted Successfully";
            }
            else if (result == "Child")
            {
                TempData["message"] = result;
                TempData["content"] = "Cannot delete this branch, as it has dependencies in this application";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Branch";
            return RedirectToAction("Index", "BusEntities");
        }
        #endregion

        #region JSON
        #region Check Unit Name Exists
        public JsonResult CheckUnitNameExists(string inUnitName, Int16 unitId)
        {
            bool isExist = rp.CheckUnitNameExists(inUnitName, unitId);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Branch Name Exists
        public JsonResult CheckBranchNameExists(Int16 unitId, string inBranchName, int branchId)
        {
            bool isExist = rp.CheckBranchNameExists(unitId, inBranchName, branchId);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get State Country
        public JsonResult GetStateCountry(int inCityId)
        {
            List<string> inLst = new List<string>();
            inLst = rp.GetStateCountry(inCityId);
            return Json(inLst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Image file convert
        public byte[] ConvertToByte(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes((int)file.ContentLength);
            return imageByte;
        }
        #endregion
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}