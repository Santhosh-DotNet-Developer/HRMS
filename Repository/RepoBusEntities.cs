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
        bool result = false;
        string message = "";
        private HRMSEntities db;
        Logger log = new Logger();
        #endregion

        #region Constructor
        public RepoBusEntities(HRMSEntities db)
        {
            this.db = db; 
        }
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
            BUS_BRANCH branchDet = new BUS_BRANCH();
            try
            {
                if (branchId > 0)
                {
                    branchDet = db.BUS_BRANCH.Where(w => w.BR_ID == branchId).Single();
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
        public string SaveEntity(BUS_ENTITIES inData)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                string user = HttpContext.Current.Request.Cookies[Constant.Username].Value;
                BUS_ENTITIES inUpdateData = db.BUS_ENTITIES.Where(w => w.BE_ID == inData.BE_ID).SingleOrDefault();
                inUpdateData.BE_NAME = inData.BE_NAME;
                inUpdateData.BE_ADDRESS = inData.BE_ADDRESS;
                inUpdateData.BE_CITY_ID = inData.BE_CITY_ID;
                inUpdateData.BE_STATE_CODE = inData.BE_STATE_CODE;
                inUpdateData.BE_CNTRY_CODE = inData.BE_CNTRY_CODE;
                inUpdateData.BE_ALT_MOB_NO = inData.BE_ALT_MOB_NO;
                inUpdateData.BE_MOB_NO = inData.BE_MOB_NO;
                inUpdateData.BE_EMAIL = inData.BE_EMAIL;
                inUpdateData.BE_WEBSITE = inData.BE_WEBSITE;
                inUpdateData.BE_GSTIN = inData.BE_GSTIN;
                inUpdateData.BE_PINCODE = inData.BE_PINCODE;
                inUpdateData.BE_LOGO = inData.BE_LOGO;
                inUpdateData.BE_LOGO_URL = inData.BE_LOGO_URL;
                db.SaveChanges();
                message = "Update";
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - Save : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Save Unit
        public string SaveUnit(BUS_UNITS inData)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (inData.BU_ID == 0)
                {
                    inData.BU_BE = BE;
                    db.BUS_UNITS.Add(inData);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    BUS_UNITS inUpdateData = db.BUS_UNITS.Where(w => w.BU_ID == inData.BU_ID).SingleOrDefault();
                    inUpdateData.BU_NAME = inData.BU_NAME;
                    inUpdateData.BU_ADDRESS = inData.BU_ADDRESS;
                    inUpdateData.BU_CITY_ID = inData.BU_CITY_ID;
                    inUpdateData.BU_STATE_CODE = inData.BU_STATE_CODE;
                    inUpdateData.BU_CNTRY_CODE = inData.BU_CNTRY_CODE;
                    inUpdateData.BU_POST_BOX_NO = inData.BU_POST_BOX_NO;
                    inUpdateData.BU_ALT_MOB_NO = inData.BU_ALT_MOB_NO;
                    inUpdateData.BU_MOB_NO = inData.BU_MOB_NO;
                    inUpdateData.BU_EMAIL = inData.BU_EMAIL;
                    inUpdateData.BU_WEBSITE = inData.BU_WEBSITE;
                    inUpdateData.BU_VAT_DOR = inData.BU_VAT_DOR;
                    inUpdateData.BU_FAX = inData.BU_FAX;
                    inUpdateData.BU_TIN_NO = inData.BU_TIN_NO;
                    inUpdateData.BU_VAT_NO = inData.BU_VAT_NO;
                    inUpdateData.BU_LOGO = inData.BU_LOGO;
                    inUpdateData.BU_LOGO_URL = inData.BU_LOGO_URL;
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
        public string SaveBranch(BUS_BRANCH inData)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (inData.BR_ID == 0)
                {
                    inData.BR_BE = BE;
                    db.BUS_BRANCH.Add(inData);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    BUS_BRANCH inUpdateData = db.BUS_BRANCH.Where(w => w.BR_ID == inData.BR_ID).SingleOrDefault();
                    inUpdateData.BR_NAME = inData.BR_NAME;
                    inUpdateData.BR_MOB_NO = inData.BR_MOB_NO;
                    inUpdateData.BR_VAT_DOR = inData.BR_VAT_DOR;
                    inUpdateData.BR_EMAIL = inData.BR_EMAIL;
                    inUpdateData.BR_WEBSITE = inData.BR_WEBSITE;
                    inUpdateData.BR_FAX = inData.BR_FAX;
                    inUpdateData.BR_TIN_NO = inData.BR_TIN_NO;
                    inUpdateData.BR_VAT_NO = inData.BR_VAT_NO;
                    inUpdateData.BR_CITY_ID = inData.BR_CITY_ID;
                    inUpdateData.BR_STATE_CODE = inData.BR_STATE_CODE;
                    inUpdateData.BR_CNTRY_CODE = inData.BR_CNTRY_CODE;
                    inUpdateData.BR_ADDR = inData.BR_ADDR;
                    inUpdateData.BR_POST_BOX_NO = inData.BR_POST_BOX_NO;
                    inUpdateData.BR_LOGO = inData.BR_LOGO;
                    inUpdateData.BR_LOGO_URL = inData.BR_LOGO_URL;
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
        public string DeleteUnit(Int16 inUnitId)
        {
            try
            {
                if (CheckUnitDependency(inUnitId) == false)
                {
                    BUS_UNITS inUnitData = db.BUS_UNITS.Where(w => w.BU_ID == inUnitId).SingleOrDefault();
                    db.BUS_UNITS.Remove(inUnitData);
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
        public bool CheckUnitDependency(Int16 inUnitId)
        {
            int count = 0;
            try
            {
                count = db.BUS_BRANCH.Where(w => w.BR_BU == inUnitId).Count();
                count = count + db.USER_CREATION.Where(w => w.USER_BU == inUnitId).Count();
                count = count + db.EMPLOYEES.Where(w => w.EMP_BU == inUnitId).Count();
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
        public string DeleteBranch(Int16 inBranchId)
        {
            try
            {
                if (CheckUnitDependency(inBranchId) == false)
                {
                    BUS_BRANCH inBranchData = db.BUS_BRANCH.Where(w => w.BR_ID == inBranchId).SingleOrDefault();
                    db.BUS_BRANCH.Remove(inBranchData);
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
                log.Debug("Exception in RepoBusEntities - DeleteBranch : " + ex.Message);
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

        #region Check Unit Name Exists
        public bool CheckUnitNameExists(string inUnitName,Int16 inUnitId)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (inUnitId == 0)
                {
                    result = db.BUS_UNITS.Where(w => w.BU_BE == BE && w.BU_ID == BU && w.BU_NAME == inUnitName).Count() > 0;
                }
                else
                {
                    result = db.BUS_UNITS.Where(w => w.BU_BE == BE && w.BU_ID != inUnitId && w.BU_NAME == inUnitName).Count() > 0;
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
        public bool CheckBranchNameExists(Int16 inUnitId, string inBranchName, int inBranchId)
        {
            try
            {
                Int16 BE = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BE].Value);
                Int16 BU = Convert.ToInt16(HttpContext.Current.Request.Cookies[Constant.BU].Value);
                if (inBranchId == 0)
                {
                    result = db.BUS_BRANCH.Where(w => w.BR_BE == BE && w.BR_BU == inUnitId && w.BR_NAME == inBranchName).Count() > 0;
                }
                else
                {
                    result = db.BUS_BRANCH.Where(w => w.BR_BE == BE && w.BR_BU == inUnitId && w.BR_NAME == inBranchName && w.BR_ID != inBranchId).Count() > 0;
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
        public List<string> GetStateCountry(int inCityId)
        {
            List<string> inLst = new List<string>();
            try
            {
                var inData = db.VIEW_CITY_STATE_COUNTRY.Where(w => w.CITY_ID == inCityId).Select(s => new { s.STATE_CODE, s.CNTRY_CODE }).SingleOrDefault();
                inLst.Add(inData.STATE_CODE);
                inLst.Add(inData.CNTRY_CODE);
            }
            catch(Exception ex)
            {
                log.Debug("Exception in RepoBusEntities - GetStateCountry : " + ex.Message);
            }
            return inLst;
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