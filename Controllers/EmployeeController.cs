using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Repository;
using HRMS.Models;
using System.IO;

namespace HRMS.Controllers
{
    [CheckSessionTimeOut]
    public class EmployeeController : Controller
    {
        #region Declaration
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        RepoEmployee rp = new RepoEmployee();
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
        public ActionResult AddEdit(string empCode)
        {
            ViewBag.Show = TempData["message"];
            ViewBag.Content = TempData["content"];
            return View(rp.GetDetails(empCode));
        }
        #endregion

        #region Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(EMPLOYEE employee, string[] languages, HttpPostedFileBase photo, HttpPostedFileBase aadhar, HttpPostedFileBase pan)
        {
            string fileUrl = "";
            string filePath = "";
            string fileName = "";
            string folder = "";
            EMPLOYEE empDet = db.EMPLOYEES.Where(m => m.EMP_CODE == employee.EMP_CODE).SingleOrDefault();

            #region Delete Previous Photo
            if (empDet != null)
            {
                fileUrl = empDet.EMP_IMAGE_URL;
                if ((fileUrl != null && photo != null) || (employee.EMP_IMAGE_URL == null && photo == null))
                {
                    try
                    {
                        folder = fileUrl.Substring(fileUrl.LastIndexOf("Upload/"));
                        filePath = Path.Combine(Server.MapPath("~\\"), folder);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        else
                        {
                            log.Debug("File not found in EmployeeController - Save : Delete Previous Photo");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception in EmployeeController - Save(Delete Previous Photo) : " + ex.Message);
                    }
                }
            }
            #endregion

            #region Save Photo
            if (photo != null)
            {
                try
                {
                    folder = "~/Upload/Employee/Photo/" + employee.EMP_CODE + "/";
                    fileName = Path.GetFileName(photo.FileName);
                    filePath = Path.Combine(Server.MapPath(folder), fileName);
                    if (!Directory.Exists(Server.MapPath(folder)))
                    {
                        Directory.CreateDirectory(Server.MapPath(folder));
                    }
                    employee.EMP_IMAGE_URL = "../Upload/Employee/Photo/" + employee.EMP_CODE + "/" + fileName;
                    photo.SaveAs(filePath);
                }
                catch (Exception ex)
                {
                    log.Debug("Exception in EmployeeController - Save(Save Photo) : " + ex.Message);
                }
            }
            #endregion

            #region Delete Previous Aadhar File
            if (empDet != null)
            {
                string aadharFileUrl = empDet.EMP_AADHAR_FILE_URL;
                if (aadharFileUrl != null && empDet != null)
                {
                    try
                    {
                        folder = aadharFileUrl.Substring(aadharFileUrl.LastIndexOf("Upload/"));
                        filePath = Path.Combine(Server.MapPath("~\\"), folder);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        else
                        {
                            log.Debug("File not found in EmployeeController - Save : Delete Previous Aadhar File");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception in EmployeeController - Save(Delete Previous Aadhar File) : " + ex.Message);
                    }
                }
            }
            #endregion

            #region Save Aadhar File
            if (aadhar != null)
            {
                try
                {
                    folder = "~/Upload/Employee/Aadhar/" + employee.EMP_CODE + "/";
                    fileName = Path.GetFileName(aadhar.FileName);
                    filePath = Path.Combine(Server.MapPath(folder), fileName);
                    if (!Directory.Exists(Server.MapPath(folder)))
                    {
                        Directory.CreateDirectory(Server.MapPath(folder));
                    }
                    employee.EMP_AADHAR_FILE_URL = "../Upload/Employee/Aadhar/" + employee.EMP_CODE + "/" + fileName;
                    aadhar.SaveAs(filePath);
                }
                catch (Exception ex)
                {
                    log.Debug("Exception in EmployeeController - Save(Save Aadhar File) : " + ex.Message);
                }
            }
            #endregion

            #region Delete Previous PAN File
            if (empDet != null)
            {
                string panFileUrl = empDet.EMP_PAN_FILE_URL;
                if (panFileUrl != null && pan != null)
                {
                    try
                    {
                        folder = panFileUrl.Substring(panFileUrl.LastIndexOf("Upload/"));
                        filePath = Path.Combine(Server.MapPath("~\\"), folder);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        else
                        {
                            log.Debug("File not found in EmployeeController - Save : Delete Previous PAN File");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception in EmployeeController - Save(Delete Previous PAN File) : " + ex.Message);
                    }
                }
            }
            #endregion

            #region Save PAN File
            if (pan != null)
            {
                try
                {
                    folder = "~/Upload/Employee/PAN/" + employee.EMP_CODE + "/";
                    fileName = Path.GetFileName(pan.FileName);
                    filePath = Path.Combine(Server.MapPath(folder), fileName);
                    if (!Directory.Exists(Server.MapPath(folder)))
                    {
                        Directory.CreateDirectory(Server.MapPath(folder));
                    }
                    employee.EMP_PAN_FILE_URL = "../Upload/Employee/PAN/" + employee.EMP_CODE + "/" + fileName;
                    pan.SaveAs(filePath);
                }
                catch (Exception ex)
                {
                    log.Debug("Exception in EmployeeController - Save(Save PAN File) : " + ex.Message);
                }
            }
            #endregion

            string result = rp.Save(employee, languages);
            if(result == "Insert")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Insert;
            }
            else if(result == "Update")
            {
                TempData["message"] = result;
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
        public ActionResult Delete(string empCode)
        {
            string result = rp.Delete(empCode);
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
            return RedirectToAction("Index");
        }
        #endregion

        #region Download File
        public ActionResult DownloadFile(string empCode, string type)
        {
            string fileUrl = "";
            string filepath = "";
            string filename = "";
            try
            {
                if (type == "Aadhar")
                {
                    fileUrl = db.EMPLOYEES.Where(w => w.EMP_CODE == empCode).Select(s => s.EMP_AADHAR_FILE_URL).SingleOrDefault();
                }
                else if (type == "Pan")
                {
                    fileUrl = db.EMPLOYEES.Where(w => w.EMP_CODE == empCode).Select(s => s.EMP_PAN_FILE_URL).SingleOrDefault();
                }

                if (fileUrl != null)
                {
                    filename = Path.GetFileName(fileUrl);
                    filepath = Server.MapPath(fileUrl);
                }
                FileStream stream = System.IO.File.OpenRead(filepath);
                return new FileStreamResult(stream, "application/octet-stream") { FileDownloadName = filename };
            }
            catch(Exception ex)
            {
                log.Debug("Exception in EmployeeController - DownloadFile : " + ex.Message);
                if (ex.Message != null && ex.Message.StartsWith("Could not find file"))
                {
                    TempData["message"] = "Error";
                    TempData["content"] = "File does not exists";
                }
                else if (ex.Message != null && ex.Message.StartsWith("Could not find a part of the path"))
                {
                    TempData["message"] = "Error";
                    TempData["content"] = "File path does not exists";
                }
                else
                {
                    TempData["message"] = "Error";
                    TempData["content"] = ex.Message;
                }
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Delete File
        public ActionResult DeleteFile(string empCode, string type)
        {
            string fileUrl = "";
            string filepath = "";
            string result = "";
            try
            {
                string user = HttpContext.Request.Cookies[Constant.Username].Value;
                EMPLOYEE empDet = db.EMPLOYEES.Where(w => w.EMP_CODE == empCode).SingleOrDefault();
                if (empDet != null)
                {
                    #region Delete File in Directory
                    if (type == "Aadhar")
                    {
                        fileUrl = empDet.EMP_AADHAR_FILE_URL;
                    }
                    else if (type == "Pan")
                    {
                        fileUrl = empDet.EMP_PAN_FILE_URL;
                    }
                    filepath = Server.MapPath(fileUrl);
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    else
                    {
                        log.Debug("File not found in EmployeeController - DeleteFile");
                    }
                    #endregion

                    #region Update File URL
                    if (type == "Aadhar")
                    {
                        empDet.EMP_AADHAR_FILE_URL = null;
                    }
                    else if (type == "Pan")
                    {
                        empDet.EMP_PAN_FILE_URL = null;
                    }
                    empDet.EMP_UPD_BY = user;
                    empDet.EMP_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    #endregion

                    result = "Delete";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in EmployeeController - DeleteFile : " + ex.Message);
                result = ex.Message;
            }

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
            return RedirectToAction("AddEdit", "Employee", new { empCode = empCode });
        }
        #endregion

        #region JSON
        #region Get State Country
        public JsonResult GetStateCountry(int cityId)
        {
            List<string> list = rp.GetStateCountry(cityId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Bank Branches
        public JsonResult GetBankBranches(int cityId, string bankCode)
        {
            List<dynamic> branchList = rp.GetBankBranches(cityId, bankCode);
            return Json(branchList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get IFSC Code
        public JsonResult GetIFSCCode(int bankBranchId)
        {
            string IFSCCode = rp.GetIFSCCode(bankBranchId);
            return Json(IFSCCode, JsonRequestBehavior.AllowGet);
        }
        #endregion
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