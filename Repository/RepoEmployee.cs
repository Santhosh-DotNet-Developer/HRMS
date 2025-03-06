using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;
using System.Data.Entity.Validation;

namespace HRMS.Repository
{
    public class RepoEmployee
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for Employees
        public List<EMPLOYEE> GetList()
        {
            List<EMPLOYEE> empList = new List<EMPLOYEE>();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                empList = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployee - GetList : " + ex.Message);
            }
            return empList;
        }
        #endregion

        #region Get Details for Employees
        public EMPLOYEE GetDetails(string empCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            EMPLOYEE empDet = new EMPLOYEE();
            try
            {
                if (empCode != "")
                {
                    empDet = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_CODE == empCode).SingleOrDefault();
                }
                else
                {
                    #region Get Employee Code
                    DOCUMENT_NUMBER docNumDet = db.DOCUMENT_NUMBER.Where(w => w.DN_BE == BE && w.DN_BU == BU && w.DN_DOC_TYPE == "EC").SingleOrDefault();
                    int nextDocNo = Convert.ToInt32(docNumDet.DN_DOC_NEXT_NO) + 1;
                    string nextEmpCode = docNumDet.DN_DOC_PREFIX + nextDocNo.ToString("000000");
                    #endregion

                    empDet.EMP_CODE = nextEmpCode;
                    empDet.EMP_STATUS = true;
                    empDet.EMP_PER_ADDR_FLAG = false;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployee - GetDetails : " + ex.Message);
            }
            return empDet;
        }
        #endregion

        #region Save for Employees
        public string Save(EMPLOYEE employee, string[] languages)
        {
            string language = "";
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string user = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            try
            {
                if (languages != null)
                {
                    language = string.Join(",", languages);
                }
                EMPLOYEE empDet = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_CODE == employee.EMP_CODE).SingleOrDefault();
                if (empDet == null)
                {
                    employee.EMP_BE = BE;
                    employee.EMP_BU = BU;
                    employee.EMP_LANGUAGE = language;
                    employee.EMP_CRE_BY = user;
                    employee.EMP_CRE_DATE = DateTime.Now;
                    db.EMPLOYEES.Add(employee);
                    db.SaveChanges();
                    message = "Insert";

                    #region Update Document Number
                    DOCUMENT_NUMBER docNumDet = db.DOCUMENT_NUMBER.Where(w => w.DN_BE == BE && w.DN_BU == BU && w.DN_DOC_TYPE == "EC").SingleOrDefault();
                    int nextDocNo = Convert.ToInt32(docNumDet.DN_DOC_NEXT_NO) + 1;
                    string nextDoc = nextDocNo.ToString("000000");
                    docNumDet.DN_DOC_NEXT_NO = nextDoc;
                    db.SaveChanges();
                    #endregion
                }
                else
                {
                    empDet.EMP_FIRST_NAME = employee.EMP_FIRST_NAME;
                    empDet.EMP_LAST_NAME = employee.EMP_LAST_NAME;
                    empDet.EMP_FATHER_NAME = employee.EMP_FATHER_NAME;
                    empDet.EMP_GENDER = employee.EMP_GENDER;
                    empDet.EMP_MRT_STATUS = employee.EMP_MRT_STATUS;
                    empDet.EMP_DOB = employee.EMP_DOB;
                    empDet.EMP_DOJ = employee.EMP_DOJ;
                    empDet.EMP_MOB_NO = employee.EMP_MOB_NO;
                    empDet.EMP_ALT_MOB_NO = employee.EMP_ALT_MOB_NO;
                    empDet.EMP_EMAIL_ID = employee.EMP_EMAIL_ID;
                    empDet.EMP_DEPT_CODE = employee.EMP_DEPT_CODE;
                    empDet.EMP_DESG_CODE = employee.EMP_DESG_CODE;
                    empDet.EMP_GRADE = employee.EMP_GRADE;
                    empDet.EMP_EDU_DET = employee.EMP_EDU_DET;
                    empDet.EMP_EDU_DOM = employee.EMP_EDU_DOM;
                    empDet.EMP_BLOOD_GRP = employee.EMP_BLOOD_GRP;
                    empDet.EMP_LANGUAGE = language;
                    empDet.EMP_CUR_ADDR = employee.EMP_CUR_ADDR;
                    empDet.EMP_CUR_ADDR_PINCODE = employee.EMP_CUR_ADDR_PINCODE;
                    empDet.EMP_CUR_CITY = employee.EMP_CUR_CITY;
                    empDet.EMP_CUR_STATE = employee.EMP_CUR_STATE;
                    empDet.EMP_CUR_COUNTRY = employee.EMP_CUR_COUNTRY;
                    empDet.EMP_PER_ADDR_FLAG = employee.EMP_PER_ADDR_FLAG;
                    empDet.EMP_PER_ADDR = employee.EMP_PER_ADDR;
                    empDet.EMP_PER_ADDR_PINCODE = employee.EMP_PER_ADDR_PINCODE;
                    empDet.EMP_PER_CITY = employee.EMP_PER_CITY;
                    empDet.EMP_PER_STATE = employee.EMP_PER_STATE;
                    empDet.EMP_PER_COUNTRY = employee.EMP_PER_COUNTRY;
                    empDet.EMP_BANK_CODE = employee.EMP_BANK_CODE;
                    empDet.EMP_BANK_BRANCH_ID = employee.EMP_BANK_BRANCH_ID;
                    empDet.EMP_IFSC_CODE = employee.EMP_IFSC_CODE;
                    empDet.EMP_ACC_NO = employee.EMP_ACC_NO;
                    empDet.EMP_BASIC = employee.EMP_BASIC;
                    empDet.EMP_DA = employee.EMP_DA;
                    empDet.EMP_HRA = employee.EMP_HRA;
                    empDet.EMP_CA = employee.EMP_CA;
                    empDet.EMP_MA = employee.EMP_MA;
                    empDet.EMP_OTHERS = employee.EMP_OTHERS;
                    empDet.EMP_SALARY = employee.EMP_SALARY;
                    empDet.EMP_PF_NO = employee.EMP_PF_NO;
                    empDet.EMP_ESI_NO = employee.EMP_ESI_NO;
                    empDet.EMP_UAN_NO = employee.EMP_UAN_NO;
                    empDet.EMP_AADHAR_NO = employee.EMP_AADHAR_NO;
                    empDet.EMP_PAN_NO = employee.EMP_PAN_NO;
                    empDet.EMP_IMAGE_URL = employee.EMP_IMAGE_URL;
                    empDet.EMP_AADHAR_FILE_URL = employee.EMP_AADHAR_FILE_URL;
                    empDet.EMP_PAN_FILE_URL = employee.EMP_PAN_FILE_URL;
                    empDet.EMP_STATUS = employee.EMP_STATUS;
                    empDet.EMP_UPD_BY = user;
                    empDet.EMP_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployee - Save : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Delete for Employees
        public string Delete(string empCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                EMPLOYEE empDet = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_CODE == empCode).SingleOrDefault();
                if (empDet != null)
                {
                    db.EMPLOYEES.Remove(empDet);
                    db.SaveChanges();
                    message = "Delete";
                }
            }
            catch(Exception ex)
            {
                log.Debug("Exception in RepoEmployee - Delete : " + ex.Message);
            }
            return message;
        }
        #endregion 

        #region Get State Country
        public List<string> GetStateCountry(int cityId)
        {
            List<string> list = new List<string>();
            try
            {
                var stateCntryDet = db.VIEW_CITY_STATE_COUNTRY.Where(w => w.CITY_ID == cityId).Select(s => new { s.STATE_CODE, s.CNTRY_CODE }).SingleOrDefault();
                list.Add(stateCntryDet.STATE_CODE);
                list.Add(stateCntryDet.CNTRY_CODE);
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployee - GetStateCountry : " + ex.Message);
            }
            return list;
        }
        #endregion

        #region Get Bank Branches
        public List<dynamic> GetBankBranches(int cityId, string bankCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            List<dynamic> branchList = new List<dynamic>();
            try
            {
                var lst = db.BANK_BRANCHES.AsEnumerable().Where(w => w.BB_BE == BE && w.BB_BU == BU && w.BB_CITY_ID == cityId && w.BB_BANK_CODE == bankCode).Select(s => new { id = s.BB_ID.ToString(), text = s.BB_NAME }).ToList();
                lst.Insert(0, new { id = "", text = "" });
                branchList.AddRange(lst);
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployee - GetBankBranches : " + ex.Message);
            }
            return branchList;
        }
        #endregion

        #region Get IFSC Code
        public string GetIFSCCode(int bankBranchId)
        {
            string IFSCCode = "";
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                IFSCCode = db.BANK_BRANCHES.Where(w => w.BB_BE == BE && w.BB_BU == BU && w.BB_ID == bankBranchId).Select(s => s.BB_IFSC_CODE).SingleOrDefault();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoEmployee - GetIFSCCode : " + ex.Message);
            }
            return IFSCCode;
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