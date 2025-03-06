using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoPermissionRequest
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for PermissionRequest/History
        public PermissionRequestModel GetList()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string role = HttpContext.Current.Request.Cookies[Constant.Role].Value.ToLower();
            string empCode = HttpContext.Current.Request.Cookies[Constant.EmpCode].Value;
            PermissionRequestModel perReq = new PermissionRequestModel();
            try
            {
                if (role == "employee")
                {
                    perReq.PermissionRequest = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_STATUS == "N" && w.PR_EMP_CODE == empCode).ToList();
                    perReq.PermissionRequestHist = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_STATUS != "N" && w.PR_EMP_CODE == empCode).OrderByDescending(o => o.PR_CRE_DATE).ToList();
                }
                else if (role == "manager")
                {
                    string managerDept = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_CODE == empCode).Select(s => s.EMP_DEPT_CODE).Single();
                    perReq.PermissionRequest = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_STATUS == "N" && w.PR_EMP_DEPT_CODE == managerDept).ToList();
                    perReq.PermissionRequestHist = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_STATUS != "N" && w.PR_EMP_DEPT_CODE == managerDept).OrderByDescending(o => o.PR_CRE_DATE).ToList();
                }
                else if (role == "hr manager")
                {
                    perReq.PermissionRequest = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_STATUS == "MA").ToList();
                    perReq.PermissionRequestHist = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_STATUS != "N").OrderByDescending(o => o.PR_CRE_DATE).ToList();
                }
                else
                {
                    perReq.PermissionRequest = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_STATUS == "N").ToList();
                    perReq.PermissionRequestHist = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_STATUS != "N").OrderByDescending(o => o.PR_CRE_DATE).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoPermissionRequest - GetList : " + ex.Message);
            }
            return perReq;
        }
        #endregion

        #region Get Details for Permission Request
        public PERMISSION_REQUEST GetDetails(int perReqId)
        {
            PERMISSION_REQUEST perReqDet = new PERMISSION_REQUEST();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (perReqId > 0)
                {
                    perReqDet = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_ID == perReqId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoPermissionRequest - GetDetails : " + ex.Message);
            }
            return perReqDet;
        }
        #endregion

        #region Save for Permission Request
        public string Save(PERMISSION_REQUEST perReq)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string user = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            string empCode = HttpContext.Current.Request.Cookies[Constant.EmpCode].Value;
            string empDept = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_CODE == empCode).Select(s => s.EMP_DEPT_CODE).Single();
            try
            {
                perReq.PR_BE = BE;
                perReq.PR_BU = BU;
                perReq.PR_EMP_CODE = empCode;
                perReq.PR_EMP_DEPT_CODE = empDept;
                perReq.PR_STATUS = "N";
                perReq.PR_CRE_BY = user;
                perReq.PR_CRE_DATE = DateTime.Now;
                db.PERMISSION_REQUEST.Add(perReq);
                db.SaveChanges();
                message = "Success";
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoPermissionRequest - Save : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Cancel/Reject Permission Request
        public string CancelReject(PERMISSION_REQUEST perReq, string type)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string user = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                string empCode = HttpContext.Current.Request.Cookies[Constant.EmpCode].Value;
                PERMISSION_REQUEST perReqDet = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_ID == perReq.PR_ID).SingleOrDefault();
                if (perReqDet != null)
                {
                    if (type == "Cancel")
                    {
                        perReqDet.PR_STATUS = "C";
                        perReqDet.PR_REJ_REASON = perReq.PR_REJ_REASON;
                        perReqDet.PR_REJ_BY = empCode;
                    }
                    else if (type == "Reject")
                    {
                        perReqDet.PR_STATUS = "R";
                        perReqDet.PR_REJ_REASON = perReq.PR_REJ_REASON;
                        perReqDet.PR_REJ_BY = empCode;
                    }
                    perReqDet.PR_UPD_BY = user;
                    perReqDet.PR_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    if (type == "Cancel")
                    {
                        message = "Cancel";
                    }
                    else if (type == "Reject")
                    {
                        message = "Reject";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoPermissionRequest - Cancel : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Approve Permission Request
        public string Approve(int perReqId)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string user = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                string empCode = HttpContext.Current.Request.Cookies[Constant.EmpCode].Value;
                PERMISSION_REQUEST perReqDet = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_ID == perReqId).SingleOrDefault();
                if (perReqDet != null)
                {
                    perReqDet.PR_STATUS = "A";
                    perReqDet.PR_APR_BY = empCode;
                    perReqDet.PR_UPD_BY = user;
                    perReqDet.PR_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Approve";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoPermissionRequest - Approve : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Check Permission Exists
        public bool CheckPermissionExists(string empCode, string perTime)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.PERMISSION_REQUEST.Where(w => w.PR_BE == BE && w.PR_BU == BU && w.PR_EMP_CODE == empCode && w.PR_START_TIME == perTime).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoPermissionRequest - CheckPermissionExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}