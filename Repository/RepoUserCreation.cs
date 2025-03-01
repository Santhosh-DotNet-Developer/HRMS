using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoUserCreation
    {
        #region Constructor
        private HRMSEntities db;

        public RepoUserCreation(HRMSEntities db)
        {
            this.db = db;
        }
        #endregion

        #region Declaration
        string message = "";
        bool result = false;
        Logger log = new Logger();
        #endregion

        #region GetList
        public List<USER_CREATION> GetList()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            List<USER_CREATION> inUserList = new List<USER_CREATION>();
            try
            {
                inUserList = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - GetList : " + ex.Message);
            }
            return inUserList;
        }
        #endregion

        #region GetSingle
        public USER_CREATION GetSingle(int inUserId)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            USER_CREATION inUserData = new USER_CREATION();
            EncryptDecrypt inDecrypt = new EncryptDecrypt();
            try
            {
                if (inUserId != 0)
                {
                    inUserData = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_ID == inUserId).SingleOrDefault();
                    inUserData.USER_PSWD = inDecrypt.Decrypt(inUserData.USER_PSWD);
                }
                else
                {
                    inUserData.USER_STATUS = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - GetSingle : " + ex.Message);
            }
            return inUserData;
        }
        #endregion

        #region Save
        public string Save(USER_CREATION inData)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            EncryptDecrypt inEncrypt = new EncryptDecrypt();
            try
            {
                if (inData.USER_ID == 0)
                {
                    inData.USER_BE = BE;
                    inData.USER_BU = BU;
                    inData.USER_CRE_BY = User;
                    inData.USER_CRE_DATE = DateTime.Now;
                    inData.USER_PSWD = inEncrypt.Encrypt(inData.USER_PSWD);
                    db.USER_CREATION.Add(inData);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    USER_CREATION inUserData = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_ID == inData.USER_ID).SingleOrDefault();
                    inUserData.USER_ROLE_ID = inData.USER_ROLE_ID;
                    inUserData.USER_UPD_BY = User;
                    inUserData.USER_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - Save : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Check Username Exists
        public bool CheckUsernameExists(string inUsername)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_NAME == inUsername).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - CheckUsernameExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Employee Exists
        public bool CheckEmployeeExists(string inEmpCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_EMP_CODE == inEmpCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - CheckEmployeeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Update Password
        public bool UpdatePassword(int inUserId, string inNewPassword)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            EncryptDecrypt inEncrypt = new EncryptDecrypt();
            try
            {
                string encryptPassword = inEncrypt.Encrypt(inNewPassword);
                USER_CREATION inUserData = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_ID == inUserId).SingleOrDefault();
                if(inUserData != null)
                {
                    inUserData.USER_PSWD = encryptPassword;
                    inUserData.USER_UPD_BY = User;
                    inUserData.USER_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - UpdatePassword : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Destructor
        ~RepoUserCreation()
        {
            Dispose(false);
        }
        #endregion

        #region Dispose
        private bool disposed = false;
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