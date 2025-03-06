using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoRoleAccess : IDisposable
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        Logger log = new Logger();
        HRMSEntities db = new HRMSEntities();
        #endregion

        #region Get List
        public List<ROLE> GetList()
        {
            List<ROLE> roleList = new List<ROLE>();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                roleList = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU).ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoRoleAccess - GetList : " + ex.Message);
            }
            return roleList;
        }
        #endregion

        #region Get Details for Role/Role Access
        public RoleAccessModel GetDetails(int roleId)
        {
            RoleAccessModel roleAccessModel = new RoleAccessModel();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if(roleId != 0)
                {
                    roleAccessModel.RoleData = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU && w.ROLE_ID == roleId).Select(s => new Role { ROLE_ID = s.ROLE_ID, ROLE_DESC = s.ROLE_DESC }).SingleOrDefault();
                    roleAccessModel.RoleAccessList = db.APPL_FORMS.Where(w => w.AF_STATUS == "A" && w.AF_URL != "#").Select(s => new RoleAccess { RA_FORM_ID = s.AF_FORM_ID, RA_FORM_NAME = s.AF_FORM_NAME, RA_FLAG = (db.ROLE_ACCESS.Where(w => w.RA_BE == BE && w.RA_BU == BU && w.RA_ROLE_ID == roleId && w.RA_FORM_ID == s.AF_FORM_ID).Count() > 0) }).ToList();
                }
                else
                {
                    roleAccessModel.RoleAccessList = db.APPL_FORMS.Where(w => w.AF_STATUS == "A" && w.AF_URL != "#").Select(s => new RoleAccess { RA_FORM_ID = s.AF_FORM_ID, RA_FORM_NAME = s.AF_FORM_NAME, RA_FLAG = false }).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoRoleAccess - GetDetails : " + ex.Message);
            }
            return roleAccessModel;
        }
        #endregion

        #region Save for Role/Role Access
        public string Save(RoleAccessModel roleAccess)
        {
            ROLE role = new ROLE();
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            string User = HttpContext.Current.Request.Cookies[Constant.Username].Value;
            try
            {
                if (roleAccess.RoleData.ROLE_ID == 0)
                {
                    role.ROLE_BE = BE;
                    role.ROLE_BU = BU;
                    role.ROLE_DESC = roleAccess.RoleData.ROLE_DESC;
                    role.ROLE_CRE_BY = User;
                    role.ROLE_CRE_DATE = DateTime.Now;
                    db.ROLEs.Add(role);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    #region Delete Role Access Form Exists
                    List<ROLE_ACCESS> roleAccessLst = db.ROLE_ACCESS.Where(w => w.RA_BE == BE && w.RA_BU == BU && w.RA_ROLE_ID == roleAccess.RoleData.ROLE_ID).ToList();
                    if (roleAccessLst.Count() > 0)
                    {
                        db.ROLE_ACCESS.RemoveRange(roleAccessLst);
                        db.SaveChanges();
                    }
                    #endregion
                    message = "Update";
                }

                #region Insert/Update Role Access Form
                try
                {
                    var checkedList = roleAccess.RoleAccessList.Where(w => w.RA_FLAG == true).ToList();
                    for (int i = 0; i < checkedList.Count(); i++)
                    {
                        ROLE_ACCESS updateRoleAccess = new ROLE_ACCESS();
                        updateRoleAccess.RA_BE = BE;
                        updateRoleAccess.RA_BU = BU;
                        if (roleAccess.RoleData.ROLE_ID == 0)
                        {
                            updateRoleAccess.RA_ROLE_ID = role.ROLE_ID;
                        }
                        else
                        {
                            updateRoleAccess.RA_ROLE_ID = roleAccess.RoleData.ROLE_ID;
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
        public bool CheckRoleExists(string role)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU && w.ROLE_DESC == role).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoRoleAccess - CheckRoleExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Delete
        public string Delete(int roleId)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (CheckRoleDependency(roleId) == false)
                {
                    #region Delete Role Access
                    List<ROLE_ACCESS> roleAccessLst = db.ROLE_ACCESS.Where(w => w.RA_BE == BE && w.RA_BU == BU && w.RA_ROLE_ID == roleId).ToList();
                    db.ROLE_ACCESS.RemoveRange(roleAccessLst);
                    db.SaveChanges();
                    #endregion

                    #region Delete Role
                    ROLE roleDet = db.ROLEs.Where(w => w.ROLE_BE == BE && w.ROLE_BU == BU && w.ROLE_ID == roleId).SingleOrDefault();
                    if (roleDet != null)
                    {
                        db.ROLEs.Remove(roleDet);
                        db.SaveChanges();
                        message = "Delete";
                    }
                    #endregion
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

        #region Check Role Dependency
        public bool CheckRoleDependency(int roleId)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                result = db.USER_CREATION.Where(w => w.USER_BE == BE && w.USER_BU == BU && w.USER_ROLE_ID == roleId).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoRoleAccess - CheckRoleDependency : " + ex.Message);
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