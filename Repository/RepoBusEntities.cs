using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoBusEntities
    {
        #region Declaration
        string message = "";
        bool result = false, disposed = false;
        HRMSEntities db = new HRMSEntities();
        Logger log = new Logger();
        #endregion

        #region Get List
        public Tuple<BUS_ENTITIES,List<BUS_UNITS>,List<BUS_BRANCH>> GetList()
        {
            BUS_ENTITIES entityDet = new BUS_ENTITIES();
            List<BUS_UNITS> unitList = new List<BUS_UNITS>();
            List<BUS_BRANCH> branchList = new List<BUS_BRANCH>();
            try
            {
                entityDet = db.BUS_ENTITIES.SingleOrDefault();
                unitList = db.BUS_UNITS.ToList();
                branchList = db.BUS_BRANCH.ToList();
            }
            catch(Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - GetList : " + ex.Message);
            }
            return new Tuple<BUS_ENTITIES, List<BUS_UNITS>, List<BUS_BRANCH>>(entityDet, unitList, branchList);
        }
        #endregion

        #region Get Unit Details
        public BUS_UNITS GetUnitDetails(Int16 unitId)
        {
            BUS_UNITS unitDet = new BUS_UNITS();
            try
            {
                if (unitId > 0)
                {
                    unitDet = db.BUS_UNITS.Where(w => w.BU_ID == unitId).Single();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - GetUnitDetails : " + ex.Message);
            }
            return unitDet;
        }
        #endregion

        #region Get Branch Details
        public BUS_BRANCH GetBranchDetails(int branchId)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            BUS_BRANCH branchDet = new BUS_BRANCH();
            try
            {
                if (branchId > 0)
                {
                    branchDet = db.BUS_BRANCH.Where(w => w.BR_BE == BE && w.BR_BU == BU && w.BR_ID == branchId).Single();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - GetBranchDetails : " + ex.Message);
            }
            return branchDet;
        }
        #endregion

        #region Save Entity
        public string SaveEntity(BUS_ENTITIES entity)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            try
            {
                BUS_ENTITIES entityDet = db.BUS_ENTITIES.Where(w => w.BE_ID == entity.BE_ID).SingleOrDefault();
                entityDet.BE_NAME = entity.BE_NAME;
                entityDet.BE_ADDRESS = entity.BE_ADDRESS;
                entityDet.BE_CITY_ID = entity.BE_CITY_ID;
                entityDet.BE_STATE_CODE = entity.BE_STATE_CODE;
                entityDet.BE_CNTRY_CODE = entity.BE_CNTRY_CODE;
                entityDet.BE_ALT_MOB_NO = entity.BE_ALT_MOB_NO;
                entityDet.BE_MOB_NO = entity.BE_MOB_NO;
                entityDet.BE_EMAIL = entity.BE_EMAIL;
                entityDet.BE_WEBSITE = entity.BE_WEBSITE;
                entityDet.BE_GSTIN = entity.BE_GSTIN;
                entityDet.BE_PINCODE = entity.BE_PINCODE;
                entityDet.BE_LOGO = entity.BE_LOGO;
                entityDet.BE_LOGO_URL = entity.BE_LOGO_URL;
                db.SaveChanges();
                message = "Update";
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - SaveEntity : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Save Unit
        public string SaveUnit(BUS_UNITS unit)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (unit.BU_ID == 0)
                {
                    unit.BU_BE = BE;
                    db.BUS_UNITS.Add(unit);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    BUS_UNITS unitDet = db.BUS_UNITS.Where(w => w.BU_ID == unit.BU_ID).SingleOrDefault();
                    unitDet.BU_NAME = unit.BU_NAME;
                    unitDet.BU_ADDRESS = unit.BU_ADDRESS;
                    unitDet.BU_CITY_ID = unit.BU_CITY_ID;
                    unitDet.BU_STATE_CODE = unit.BU_STATE_CODE;
                    unitDet.BU_CNTRY_CODE = unit.BU_CNTRY_CODE;
                    unitDet.BU_POST_BOX_NO = unit.BU_POST_BOX_NO;
                    unitDet.BU_ALT_MOB_NO = unit.BU_ALT_MOB_NO;
                    unitDet.BU_MOB_NO = unit.BU_MOB_NO;
                    unitDet.BU_EMAIL = unit.BU_EMAIL;
                    unitDet.BU_WEBSITE = unit.BU_WEBSITE;
                    unitDet.BU_VAT_DOR = unit.BU_VAT_DOR;
                    unitDet.BU_FAX = unit.BU_FAX;
                    unitDet.BU_TIN_NO = unit.BU_TIN_NO;
                    unitDet.BU_VAT_NO = unit.BU_VAT_NO;
                    unitDet.BU_LOGO = unit.BU_LOGO;
                    unitDet.BU_LOGO_URL = unit.BU_LOGO_URL;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - SaveUnit : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Save Branch
        public string SaveBranch(BUS_BRANCH branch)
        {
            Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
            Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
            try
            {
                if (branch.BR_ID == 0)
                {
                    branch.BR_BE = BE;
                    db.BUS_BRANCH.Add(branch);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    BUS_BRANCH branchDet = db.BUS_BRANCH.Where(w => w.BR_ID == branch.BR_ID).SingleOrDefault();
                    branchDet.BR_NAME = branch.BR_NAME;
                    branchDet.BR_MOB_NO = branch.BR_MOB_NO;
                    branchDet.BR_VAT_DOR = branch.BR_VAT_DOR;
                    branchDet.BR_EMAIL = branch.BR_EMAIL;
                    branchDet.BR_WEBSITE = branch.BR_WEBSITE;
                    branchDet.BR_FAX = branch.BR_FAX;
                    branchDet.BR_TIN_NO = branch.BR_TIN_NO;
                    branchDet.BR_VAT_NO = branch.BR_VAT_NO;
                    branchDet.BR_CITY_ID = branch.BR_CITY_ID;
                    branchDet.BR_STATE_CODE = branch.BR_STATE_CODE;
                    branchDet.BR_CNTRY_CODE = branch.BR_CNTRY_CODE;
                    branchDet.BR_ADDR = branch.BR_ADDR;
                    branchDet.BR_POST_BOX_NO = branch.BR_POST_BOX_NO;
                    branchDet.BR_LOGO = branch.BR_LOGO;
                    branchDet.BR_LOGO_URL = branch.BR_LOGO_URL;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - SaveBranch : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Delete Unit
        public string DeleteUnit(Int16 unitId)
        {
            try
            {
                if (CheckUnitDependency(unitId) == false)
                {
                    BUS_UNITS unitDet = db.BUS_UNITS.Where(w => w.BU_ID == unitId).SingleOrDefault();
                    if (unitDet != null)
                    {
                        db.BUS_UNITS.Remove(unitDet);
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
                log.Debug("Exception in RepoBusEntities - DeleteUnit : " + ex.Message);
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

        #region Check Unit Dependency
        public bool CheckUnitDependency(Int16 unitId)
        {
            int count = 0;
            try
            {
                count = db.BUS_BRANCH.Where(w => w.BR_BU == unitId).Count();
                count = count + db.USER_CREATION.Where(w => w.USER_BU == unitId).Count();
                count = count + db.EMPLOYEES.Where(w => w.EMP_BU == unitId).Count();
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - CheckUnitDependency : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Delete Branch
        public string DeleteBranch(Int16 branchId)
        {
            try
            {
                BUS_BRANCH branchDet = db.BUS_BRANCH.Where(w => w.BR_ID == branchId).SingleOrDefault();
                if (branchDet != null)
                {
                    db.BUS_BRANCH.Remove(branchDet);
                    db.SaveChanges();
                    message = "Delete";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - DeleteBranch : " + ex.Message);
                message = ex.Message;
            }
            return message;
        }
        #endregion

        #region Check Unit Name Exists
        public bool CheckUnitNameExists(string inUnitName,Int16 unitId)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (unitId == 0)
                {
                    result = db.BUS_UNITS.Where(w => w.BU_BE == BE && w.BU_ID == BU && w.BU_NAME == inUnitName).Count() > 0;
                }
                else
                {
                    result = db.BUS_UNITS.Where(w => w.BU_BE == BE && w.BU_ID != unitId && w.BU_NAME == inUnitName).Count() > 0;
                }
            }
            catch(Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - CheckUnitNameExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Branch Name Exists
        public bool CheckBranchNameExists(Int16 unitId, string branchName, int branchId)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (branchId == 0)
                {
                    result = db.BUS_BRANCH.Where(w => w.BR_BE == BE && w.BR_BU == unitId && w.BR_NAME == branchName).Count() > 0;
                }
                else
                {
                    result = db.BUS_BRANCH.Where(w => w.BR_BE == BE && w.BR_BU == unitId && w.BR_NAME == branchName && w.BR_ID != branchId).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - CheckBranchNameExists : " + ex.Message);
            }
            return result;
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
            catch(Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - GetStateCountry : " + ex.Message);
            }
            return list;
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