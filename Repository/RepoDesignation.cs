using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoDesignation
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for Designation
        public List<DESIGNATION> GetList()
        {
            List<DESIGNATION> desgList = new List<DESIGNATION>();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                desgList = db.DESIGNATIONs.Where(w => w.DESG_BE == BE && w.DESG_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDesignation - GetList : " + ex.Message);
            }
            return desgList;
        }
        #endregion

        #region Get Details for Designation
        public DESIGNATION GetDetails(string desgCode)
        {
            DESIGNATION desgDet = new DESIGNATION();
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (desgCode != "")
                {
                    desgDet = db.DESIGNATIONs.Where(w => w.DESG_BE == BE && w.DESG_BU == BU && w.DESG_CODE == desgCode).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDesignation - GetDetails : " + ex.Message);
            }
            return desgDet;
        }
        #endregion

        #region Save for Designation
        public string Save(DESIGNATION designation)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                if (designation.DESG_CRE_BY == null)
                {
                    designation.DESG_BE = BE;
                    designation.DESG_BU = BU;
                    designation.DESG_CRE_BY = User;
                    designation.DESG_CRE_DATE = DateTime.Now;
                    db.DESIGNATIONs.Add(designation);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    DESIGNATION desgDet = db.DESIGNATIONs.Where(w => w.DESG_BE == BE && w.DESG_BU == BU && w.DESG_CODE == designation.DESG_CODE).SingleOrDefault();
                    desgDet.DESG_NAME = designation.DESG_NAME;
                    desgDet.DESG_UPD_BY = User;
                    desgDet.DESG_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDesignation - Save : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Delete for Designation
        public string Delete(string desgCode)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (CheckDesignationDependency(desgCode) == false)
                {
                    DESIGNATION desgDet = db.DESIGNATIONs.Where(w => w.DESG_BE == BE && w.DESG_BU == BU && w.DESG_CODE == desgCode).SingleOrDefault();
                    if (desgDet != null)
                    {
                        db.DESIGNATIONs.Remove(desgDet);
                        db.SaveChanges();
                        message = "Delete";
                    }
                }
                else
                {
                    message = "Child";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDesignation - Delete : " + ex.Message);
                if (ex.InnerException != null)
                {
                    string innerExcepMsg = ex.InnerException.Message.ToString();
                    if (innerExcepMsg.StartsWith("The DELETE statement conflicted with the REFERENCE constraint "))
                    {
                        message = "Child";
                    }
                    else
                    {
                        message = innerExcepMsg;
                    }
                }
                else
                {
                    message = ex.Message;
                }
            }
            return message;
        }
        #endregion

        #region Check Dependency for Designation
        public bool CheckDesignationDependency(string desgCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_DESG_CODE == desgCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDepartment - CheckDesignationDependency : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Designation Code Exists
        public bool CheckDesgCodeExists(string desgCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.DESIGNATIONs.Where(w => w.DESG_BE == BE && w.DESG_BU == BU && w.DESG_CODE == desgCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDesignation - CheckDesgCodeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Designation Name Exists
        public bool CheckDesgNameExists(string desgName, string desgCode, string creBy)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (creBy != "")
                {
                    result = db.DESIGNATIONs.Where(w => w.DESG_BE == BE && w.DESG_BU == BU && w.DESG_CODE != desgCode && w.DESG_NAME == desgName).Count() > 0;
                }
                else
                {
                    result = db.DESIGNATIONs.Where(w => w.DESG_BE == BE && w.DESG_BU == BU && w.DESG_NAME == desgName).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDesignation - CheckDesgNameExists : " + ex.Message);
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