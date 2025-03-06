using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoUserCreation
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List for User Creations
        public List<USER_CREATION> GetList()
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            List<USER_CREATION> userList = new List<USER_CREATION>();
            try
            {
                userList = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - GetList : " + ex.Message);
            }
            return userList;
        }
        #endregion

        #region Get Details for User Creations
        public USER_CREATION GetDetails(string empCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            USER_CREATION userCreDet = new USER_CREATION();
            EncryptDecrypt decrypt = new EncryptDecrypt();
            try
            {
                if (empCode != "")
                {
                    userCreDet = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_EMP_CODE == empCode).SingleOrDefault();
                    userCreDet.USER_PSWD = decrypt.Decrypt(userCreDet.USER_PSWD);
                }
                else
                {
                    userCreDet.USER_STATUS = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - GetDetails : " + ex.Message);
            }
            return userCreDet;
        }
        #endregion

        #region Save for User Creation
        public string Save(USER_CREATION userCreation)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            EncryptDecrypt encrypt = new EncryptDecrypt();
            try
            {
                if (userCreation.USER_CRE_BY == null)
                {
                    userCreation.USER_BE = BE;
                    userCreation.USER_BU = BU;
                    userCreation.USER_CRE_BY = User;
                    userCreation.USER_CRE_DATE = DateTime.Now;
                    userCreation.USER_PSWD = encrypt.Encrypt(userCreation.USER_PSWD);
                    db.USER_CREATION.Add(userCreation);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    USER_CREATION userCreDet = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_EMP_CODE == userCreation.USER_EMP_CODE).SingleOrDefault();
                    userCreDet.USER_ROLE_ID = userCreation.USER_ROLE_ID;
                    userCreDet.USER_UPD_BY = User;
                    userCreDet.USER_UPD_DATE = DateTime.Now;
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
        public bool CheckUsernameExists(string username)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_NAME == username).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - CheckUsernameExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Employee Exists
        public bool CheckEmployeeExists(string empCode)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_EMP_CODE == empCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - CheckEmployeeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Update Password
        public string UpdatePassword(string empCode, string newPassword)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            EncryptDecrypt encrypt = new EncryptDecrypt();
            try
            {
                string encryptPassword = encrypt.Encrypt(newPassword);
                USER_CREATION userCreDet = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_EMP_CODE == empCode).SingleOrDefault();
                if(userCreDet != null)
                {
                    userCreDet.USER_PSWD = encryptPassword;
                    userCreDet.USER_UPD_BY = User;
                    userCreDet.USER_UPD_DATE = DateTime.Now;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoUserCreation - UpdatePassword : " + ex.Message);
            }
            return message;
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