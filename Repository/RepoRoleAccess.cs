using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoRoleAccess : IDisposable
    {
        #region Constructor
        private HRMSEntities db;

        public RepoRoleAccess(HRMSEntities db)
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
        public List<ROLE> GetList()
        {
            List<ROLE> inRoleList = new List<ROLE>();
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                inRoleList = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoRoleAccess - GetList : " + ex.Message);
            }
            return inRoleList;
        }
        #endregion

        #region GetSingle
        public RoleAccessModel GetSingle(int inRoleId)
        {
            RoleAccessModel roleAccessModel = new RoleAccessModel();
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if(inRoleId != 0)
                {
                    roleAccessModel.RoleData = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU && w.ROLE_ID == inRoleId).Select(s => new Role { ROLE_ID = s.ROLE_ID, ROLE_DESC = s.ROLE_DESC }).SingleOrDefault();
                    roleAccessModel.RoleAccessList = db.APPL_FORMS.Where(w => w.AF_STATUS == "A" && w.AF_URL != "#").Select(s => new RoleAccess { RA_FORM_ID = s.AF_FORM_ID, RA_FORM_NAME = s.AF_FORM_NAME, RA_FLAG = (db.ROLE_ACCESS.Where(w => w.RA_BE == BE && w.RA_BU == BU && w.RA_ROLE_ID == inRoleId && w.RA_FORM_ID == s.AF_FORM_ID).Count() > 0) }).ToList();
                }
                else
                {
                    roleAccessModel.RoleAccessList = db.APPL_FORMS.Where(w => w.AF_STATUS == "A" && w.AF_URL != "#").Select(s => new RoleAccess { RA_FORM_ID = s.AF_FORM_ID, RA_FORM_NAME = s.AF_FORM_NAME, RA_FLAG = false }).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoRoleAccess - GetSingle : " + ex.Message);
            }
            return roleAccessModel;
        }
        #endregion

        #region Save
        public string Save(RoleAccessModel inData)
        {
            ROLE inRoleInsert = new ROLE();
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                if (inData.RoleData.ROLE_ID == 0)
                {
                    inRoleInsert.ROLE_BE = BE;
                    inRoleInsert.ROLE_BU = BU;
                    inRoleInsert.ROLE_DESC = inData.RoleData.ROLE_DESC;
                    inRoleInsert.ROLE_CRE_BY = User;
                    inRoleInsert.ROLE_CRE_DATE = DateTime.Now;
                    db.ROLEs.Add(inRoleInsert);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    //ROLE inRoleData = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU && w.ROLE_ID == inData.RoleData.ROLE_ID).SingleOrDefault();
                    //inRoleData.ROLE_DESC = inData.RoleData.ROLE_DESC;
                    //inRoleData.ROLE_UPD_BY = User;
                    //inRoleData.ROLE_UPD_DATE = DateTime.Now;
                    //db.SaveChanges();

                    #region Delete Role Access Form Exists
                    List<ROLE_ACCESS> inRoleAccessLst = db.ROLE_ACCESS.Where(w => w.RA_BE == BE && w.RA_BU == BU && w.RA_ROLE_ID == inData.RoleData.ROLE_ID).ToList();
                    if (inRoleAccessLst.Count() > 0)
                    {
                        db.ROLE_ACCESS.RemoveRange(inRoleAccessLst);
                        db.SaveChanges();
                    }
                    #endregion
                    message = "Update";
                }

                #region Insert/Update Role Access Form
                try
                {
                    var checkedList = inData.RoleAccessList.Where(w => w.RA_FLAG == true).ToList();
                    for (int i = 0; i < checkedList.Count(); i++)
                    {
                        ROLE_ACCESS updateRoleAccess = new ROLE_ACCESS();
                        updateRoleAccess.RA_BE = BE;
                        updateRoleAccess.RA_BU = BU;
                        if (inData.RoleData.ROLE_ID == 0)
                        {
                            updateRoleAccess.RA_ROLE_ID = inRoleInsert.ROLE_ID;
                        }
                        else
                        {
                            updateRoleAccess.RA_ROLE_ID = inData.RoleData.ROLE_ID;
                        }
                        updateRoleAccess.RA_FORM_ID = checkedList[i].RA_FORM_ID;
                        updateRoleAccess.RA_CRE_BY = User;
                        updateRoleAccess.RA_CRE_DATE = DateTime.Now;
                        db.ROLE_ACCESS.Add(updateRoleAccess);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("Error occured while updating RoleAccess, RepoRoleAccess - Save : " + ex.Message);
                    message = ex.Message;
                }
                #endregion
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoRoleAccess - Save : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Check Role Exists
        public bool CheckRoleExists(string inRoleDesc)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                result = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU && w.ROLE_DESC == inRoleDesc).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoRoleAccess - CheckRoleExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Role Dependency
        public bool CheckRoleDependency(int inRoleId)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                result = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_ROLE_ID == inRoleId).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoRoleAccess - CheckRoleDependency : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Delete
        public string Delete(int inRoleId)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (CheckRoleDependency(inRoleId) == false)
                {
                    #region Delete Role Access
                    List<ROLE_ACCESS> inRoleAccessLst = db.ROLE_ACCESS.Where(w => w.RA_BE == BE && w.RA_BU == BU && w.RA_ROLE_ID == inRoleId).ToList();
                    db.ROLE_ACCESS.RemoveRange(inRoleAccessLst);
                    db.SaveChanges();
                    #endregion

                    #region Delete Role
                    ROLE inRoleData = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU && w.ROLE_ID == inRoleId).Single();
                    db.ROLEs.Remove(inRoleData);
                    db.SaveChanges();
                    #endregion

                    message = "Delete";
                }
                else
                {
                    message = "Child";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoRoleAccess - Delete : " + ex.Message);
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

        #region Destructor
        ~RepoRoleAccess()
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