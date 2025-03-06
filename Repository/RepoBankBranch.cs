using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoBankBranch
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for Bank/Branch
        public Tuple<List<BANK>, List<BANK_BRANCHES>> GetList()
        {
            List<BANK> bankList = new List<BANK>();
            List<BANK_BRANCHES> bankBranchList = new List<BANK_BRANCHES>();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                bankList = db.BANKS.Where(w => w.BANK_BE == BE).ToList();
                bankBranchList = db.BANK_BRANCHES.Where(w => w.BB_BE == BE && w.BB_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBankBranch - GetList : " + ex.Message);
            }
            return new Tuple<List<BANK>, List<BANK_BRANCHES>>(bankList, bankBranchList);
        }
        #endregion

        #region Get Bank Details
        public BANK GetBankDetails(string bankCode)
        {
            BANK bankDet = new BANK();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            try
            {
                if (bankCode != "")
                {
                    bankDet = db.BANKS.Where(w => w.BANK_BE == BE && w.BANK_CODE == bankCode).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBankBranch - GetBankDetails : " + ex.Message);
            }
            return bankDet;
        }
        #endregion

        #region Get Bank Branch Details
        public BANK_BRANCHES GetBankBranchDetails(int bankBranchId)
        {
            BANK_BRANCHES bankBranchDet = new BANK_BRANCHES();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (bankBranchId > 0)
                {
                    bankBranchDet = db.BANK_BRANCHES.Where(w => w.BB_BE == BE && w.BB_BU == BU && w.BB_ID == bankBranchId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBankBranch - GetBankBranchDetails : " + ex.Message);
            }
            return bankBranchDet;
        }
        #endregion

        #region Save Bank
        public string SaveBank(BANK bank)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            try
            {
                if (bank.BANK_CRE_BY == null)
                {
                    bank.BANK_BE = BE;
                    bank.BANK_CRE_BY = User;
                    bank.BANK_CRE_DATE = DateTime.Now;
                    db.BANKS.Add(bank);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    BANK bankDet = db.BANKS.Where(w => w.BANK_BE == BE && w.BANK_CODE == bank.BANK_CODE).SingleOrDefault();
                    bankDet.BANK_NAME = bank.BANK_NAME;
                    bankDet.BANK_UPD_BY = User;
                    bankDet.BANK_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBankBranch - SaveBank : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Save Bank Branch
        public string SaveBankBranch(BANK_BRANCHES branch)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            try
            {
                if (branch.BB_ID == 0)
                {
                    branch.BB_BE = BE;
                    branch.BB_BU = BU;
                    branch.BB_ADDRESS = branch.BB_ADDRESS.Trim();
                    branch.BB_CRE_BY = User;
                    branch.BB_CRE_DATE = DateTime.Now;
                    db.BANK_BRANCHES.Add(branch);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    BANK_BRANCHES bankBranchDet = db.BANK_BRANCHES.Where(w => w.BB_BE == BE && w.BB_BU == BU && w.BB_ID == branch.BB_ID).SingleOrDefault();
                    bankBranchDet.BB_NAME = branch.BB_NAME;
                    bankBranchDet.BB_IFSC_CODE = branch.BB_IFSC_CODE;
                    bankBranchDet.BB_MOB_NO = branch.BB_MOB_NO;
                    bankBranchDet.BB_EMAIL = branch.BB_EMAIL;
                    bankBranchDet.BB_FAX = branch.BB_FAX;
                    bankBranchDet.BB_ADDRESS = branch.BB_ADDRESS.Trim();
                    bankBranchDet.BB_POST_BOX_NO = branch.BB_POST_BOX_NO;
                    bankBranchDet.BB_CITY_ID = branch.BB_CITY_ID;
                    bankBranchDet.BB_STATE_CODE = branch.BB_STATE_CODE;
                    bankBranchDet.BB_CNTRY_CODE = branch.BB_CNTRY_CODE;
                    bankBranchDet.BB_UPD_BY = User;
                    bankBranchDet.BB_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBankBranch - SaveBankBranch : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Delete Bank
        public string DeleteBank(string bankCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (CheckBankDependency(bankCode) == false)
                {
                    BANK bankDet = db.BANKS.Where(w => w.BANK_BE == BE && w.BANK_CODE == bankCode).SingleOrDefault();
                    if (bankDet != null)
                    {
                        db.BANKS.Remove(bankDet);
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
                log.Debug("Exception in RepoBankBranch - DeleteBank : " + ex.Message);
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

        #region Delete Bank Branch
        public string DeleteBankBranch(int bankBranchId)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (CheckBankBranchDependency(bankBranchId) == false)
                {
                    BANK_BRANCHES bankBranchDet = db.BANK_BRANCHES.Where(w => w.BB_BE == BE && w.BB_BU == BU && w.BB_ID == bankBranchId).SingleOrDefault();
                    db.BANK_BRANCHES.Remove(bankBranchDet);
                    db.SaveChanges();
                    message = "Delete";
                }
                else
                {
                    message = "Child";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBankBranch - DeleteBankBranch : " + ex.Message);
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

        #region Check Bank Dependency
        public bool CheckBankDependency(string bankCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_BANK_CODE == bankCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - CheckBankDependency : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Bank Branch Dependency
        public bool CheckBankBranchDependency(int bankBranchId)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.EMPLOYEES.Where(w => w.EMP_BE == BE && w.EMP_BU == BU && w.EMP_BANK_BRANCH_ID == bankBranchId).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - CheckBankBranchDependency : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Bank Code Exists
        public bool CheckBankCodeExists(string bankCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.BANKS.Where(w => w.BANK_BE == BE && w.BANK_CODE == bankCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBankBranch - CheckBankCodeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Bank Name Exists
        public bool CheckBankNameExists(string bankName, string bankCode, string creBy)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (creBy != "")
                {
                    result = db.BANKS.Where(w => w.BANK_BE == BE && w.BANK_CODE != bankCode && w.BANK_NAME == bankName).Count() > 0;
                }
                else
                {
                    result = db.BANKS.Where(w => w.BANK_BE == BE && w.BANK_NAME == bankName).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBankBranch - CheckBankNameExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Bank Branch Name Exists
        public bool CheckBankBranchNameExists(string bankCode,string bankBranchName, int bankBranchId, string creBy)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (creBy != "")
                {
                    result = db.BANK_BRANCHES.Where(w => w.BB_BE == BE && w.BB_BU == BU && w.BB_ID != bankBranchId && w.BB_BANK_CODE == bankCode && w.BB_NAME == bankBranchName).Count() > 0;
                }
                else
                {
                    result = db.BANK_BRANCHES.Where(w => w.BB_BE == BE && w.BB_BU == BU && w.BB_BANK_CODE == bankCode && w.BB_NAME == bankBranchName).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBankBranch - CheckBankBranchNameExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check IFSC Code Exists
        public bool CheckIFSCCodeExists(string IFSCCode, int bankBranchId, string creBy)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (creBy != "")
                {
                    result = db.BANK_BRANCHES.Where(w => w.BB_BE == BE && w.BB_BU == BU && w.BB_ID != bankBranchId && w.BB_IFSC_CODE == IFSCCode).Count() > 0;
                }
                else
                {
                    result = db.BANK_BRANCHES.Where(w => w.BB_BE == BE && w.BB_BU == BU && w.BB_IFSC_CODE == IFSCCode).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBankBranch - CheckIFSCCodeExists : " + ex.Message);
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