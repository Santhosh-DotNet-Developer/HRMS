using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoDepartment : IRepository<DEPARTMENT>
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for Department
        public List<DEPARTMENT> GetList()
        {
            List<DEPARTMENT> deptList = new List<DEPARTMENT>();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                deptList = db.DEPARTMENTs.Where(w => w.DEPT_BE == BE && w.DEPT_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDepartment - GetList : " + ex.Message);
            }
            return deptList;
        }
        #endregion

        #region Get Details for Department
        public DEPARTMENT GetDetails(string deptCode)
        {
            DEPARTMENT deptDet = new DEPARTMENT();
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (deptCode != "")
                {
                    deptDet = db.DEPARTMENTs.Where(w => w.DEPT_BE == BE && w.DEPT_BU == BU && w.DEPT_CODE == deptCode).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDepartment - GetDetails : " + ex.Message);
            }
            return deptDet;
        }
        #endregion

        #region Save for Department
        public string Save(DEPARTMENT department)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            try
            {
                if (department.DEPT_CRE_BY == null)
                {
                    department.DEPT_BE = BE;
                    department.DEPT_BU = BU;
                    department.DEPT_CRE_BY = User;
                    department.DEPT_CRE_DATE = DateTime.Now;
                    db.DEPARTMENTs.Add(department);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    DEPARTMENT deptDet = db.DEPARTMENTs.Where(w => w.DEPT_BE == BE && w.DEPT_BU == BU && w.DEPT_CODE == department.DEPT_CODE).SingleOrDefault();
                    deptDet.DEPT_NAME = department.DEPT_NAME;
                    deptDet.DEPT_UPD_BY = User;
                    deptDet.DEPT_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDepartment - Save : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Delete for Department
        public string Delete(string deptCode)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (CheckDepartmentDependency(deptCode) == false)
                {
                    DEPARTMENT deptDet = db.DEPARTMENTs.Where(w => w.DEPT_BE == BE && w.DEPT_BU == BU && w.DEPT_CODE == deptCode).SingleOrDefault();
                    if (deptDet != null)
                    {
                        db.DEPARTMENTs.Remove(deptDet);
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
                log.Debug("Exception in RepoDepartment - Delete : " + ex.Message);
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

        #region Check Dependency for Department
        public bool CheckDepartmentDependency(string deptCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_DEPT_CODE == deptCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDepartment - CheckDepartmentDependency : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Department Code Exists
        public bool CheckDeptCodeExists(string deptCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.DEPARTMENTs.Where(w => w.DEPT_BE == BE && w.DEPT_BU == BU && w.DEPT_CODE == deptCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDepartment - CheckDeptCodeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Department Name Exists
        public bool CheckDeptNameExists(string inDeptName, string inDeptCode, string inCreBy)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (inCreBy != "")
                {
                    result = db.DEPARTMENTs.Where(w => w.DEPT_BE == BE && w.DEPT_BU == BU && w.DEPT_CODE != inDeptCode && w.DEPT_NAME == inDeptName).Count() > 0;
                }
                else
                {
                    result = db.DEPARTMENTs.Where(w => w.DEPT_BE == BE && w.DEPT_BU == BU && w.DEPT_NAME == inDeptName).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoDepartment - CheckDeptNameExists : " + ex.Message);
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