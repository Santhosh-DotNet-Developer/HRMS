using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoEmployeeGradeSalaryGroup
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for EmployeeGrade,SalaryGroup
        public Tuple<List<EMPLOYEE_GRADE>, List<SALARY_GROUP>> GetList()
        {
            List<EMPLOYEE_GRADE> empGradeList = new List<EMPLOYEE_GRADE>();
            List<SALARY_GROUP> salaryGrpList = new List<SALARY_GROUP>();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                empGradeList = db.EMPLOYEE_GRADE.Where(w => w.EG_BE == BE && w.EG_BU == BU).ToList();
                salaryGrpList = db.SALARY_GROUP.Where(w => w.SG_BE == BE && w.SG_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - GetList : " + ex.Message);
            }
            return new Tuple<List<EMPLOYEE_GRADE>, List<SALARY_GROUP>>(empGradeList, salaryGrpList);
        }
        #endregion

        #region Get Employee Grade Details
        public EMPLOYEE_GRADE GetEmpGradeDetails(string empGradeCode)
        {
            EMPLOYEE_GRADE empGradeDet = new EMPLOYEE_GRADE();
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                using (HRMSEntities db = new HRMSEntities())
                {
                    if (empGradeCode != "")
                    {
                        empGradeDet = db.EMPLOYEE_GRADE.Where(w => w.EG_BE == BE && w.EG_BU == BU && w.EG_CODE == empGradeCode).SingleOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - GetEmpGradeDetails : " + ex.Message);
            }
            return empGradeDet;
        }
        #endregion

        #region Get Salary Group Details
        public SALARY_GROUP GetSalaryGrpDetails(string salaryGrpCode)
        {
            SALARY_GROUP salaryGrpData = new SALARY_GROUP();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (salaryGrpCode != "")
                {
                    salaryGrpData = db.SALARY_GROUP.Where(w => w.SG_BE == BE && w.SG_BU == BU && w.SG_CODE == salaryGrpCode).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - GetSalaryGrpDetails : " + ex.Message);
            }
            return salaryGrpData;
        }
        #endregion

        #region Save for Employee Grade
        public string SaveEmployeeGrade(EMPLOYEE_GRADE empGrade)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                if (empGrade.EG_CRE_BY == null)
                {
                    empGrade.EG_BE = BE;
                    empGrade.EG_BU = BU;
                    empGrade.EG_CRE_BY = User;
                    empGrade.EG_CRE_DATE = DateTime.Now;
                    db.EMPLOYEE_GRADE.Add(empGrade);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    EMPLOYEE_GRADE empGradeDet = db.EMPLOYEE_GRADE.Where(w => w.EG_BE == BE && w.EG_BU == BU && w.EG_CODE == empGrade.EG_CODE).SingleOrDefault();
                    empGradeDet.EG_NAME = empGrade.EG_NAME;
                    empGradeDet.EG_SAL_GRP_CODE = empGrade.EG_SAL_GRP_CODE;
                    empGradeDet.EG_UPD_BY = User;
                    empGradeDet.EG_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - SaveEmployeeGrade : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Save for Salary Group
        public string SaveSalaryGroup(SALARY_GROUP salaryGrp)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                if (salaryGrp.SG_CRE_BY == null)
                {
                    salaryGrp.SG_BE = BE;
                    salaryGrp.SG_BU = BU;
                    salaryGrp.SG_CRE_BY = User;
                    salaryGrp.SG_CRE_DATE = DateTime.Now;
                    db.SALARY_GROUP.Add(salaryGrp);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    SALARY_GROUP salaryGrpData = db.SALARY_GROUP.Where(w => w.SG_BE == BE && w.SG_BU == BU && w.SG_CODE == salaryGrp.SG_CODE).SingleOrDefault();
                    salaryGrpData.SG_NAME = salaryGrp.SG_NAME;
                    salaryGrpData.SG_UPD_BY = User;
                    salaryGrpData.SG_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - SaveSalaryGroup : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Delete for Employee Grade
        public string DeleteEmployeeGrade(string empGradeCode)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                EMPLOYEE_GRADE empGradeDet = db.EMPLOYEE_GRADE.Where(w => w.EG_BE == BE && w.EG_BU == BU && w.EG_CODE == empGradeCode).SingleOrDefault();
                if (empGradeDet != null)
                {
                    db.EMPLOYEE_GRADE.Remove(empGradeDet);
                    db.SaveChanges();
                    message = "Delete";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - DeleteEmployeeGrade : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Delete for Salary Group
        public string DeleteSalaryGroup(string salaryGrpCode)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                SALARY_GROUP salaryGrpData = db.SALARY_GROUP.Where(w => w.SG_BE == BE && w.SG_BU == BU && w.SG_CODE == salaryGrpCode).SingleOrDefault();
                if (salaryGrpData != null)
                {
                    db.SALARY_GROUP.Remove(salaryGrpData);
                    db.SaveChanges();
                    message = "Delete";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - DeleteSalaryGroup : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Check Employee Grade Code Exists
        public bool CheckEmployeeGradeCodeExists(string empGradeCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.EMPLOYEE_GRADE.Where(w => w.EG_BE == BE && w.EG_BU == w.EG_BU && w.EG_CODE == empGradeCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - CheckEmployeeGradeCodeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Salary Group Code Exists
        public bool CheckSalaryGroupCodeExists(string salaryGrpCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.SALARY_GROUP.Where(w => w.SG_BE == BE && w.SG_BU == BU && w.SG_CODE == salaryGrpCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - CheckSalaryGroupCodeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Employee Grade Exists
        public bool CheckEmployeeGradeExists(string empGrade, string empGradeCode, string creBy)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (creBy != "")
                {
                    result = db.EMPLOYEE_GRADE.Where(w => w.EG_BE == BE && w.EG_BU == BU && w.EG_CODE != empGradeCode && w.EG_NAME == empGrade).Count() > 0;
                }
                else
                {
                    result = db.EMPLOYEE_GRADE.Where(w => w.EG_BE == BE && w.EG_BU == BU && w.EG_NAME == empGrade).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - CheckEmployeeGradeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Salary Group Exists
        public bool CheckSalaryGroupExists(string salaryGrp, string salaryGrpCode, string creBy)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (creBy != "")
                {
                    result = db.SALARY_GROUP.Where(w => w.SG_BE == BE && w.SG_BU == BU && w.SG_CODE != salaryGrpCode && w.SG_NAME == salaryGrp).Count() > 0;
                }
                else
                {
                    result = db.SALARY_GROUP.Where(w => w.SG_BE == BE && w.SG_BU == BU && w.SG_NAME == salaryGrp).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployeeGradeSalaryGroup - CheckSalaryGroupExists : " + ex.Message);
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