using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models;

namespace HRMS.Repository
{
    public class RepoCityStateCountry
    {
        #region Declaration
        bool result = false, disposed = false;
        string message = "";
        HRMSEntities db = new HRMSEntities();
        Logger log = new Logger();
        #endregion

        #region Get List for City, State, Country
        public Tuple<List<CITy>, List<STATE>, List<COUNTRy>> GetList()
        {
            List<CITy> cityList = new List<CITy>();
            List<STATE> stateList = new List<STATE>();
            List<COUNTRy> cntryList = new List<COUNTRy>();
            try
            {
                cityList = db.CITIES.ToList();
                stateList = db.STATES.ToList();
                cntryList = db.COUNTRIES.ToList();
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - GetList : " + ex.Message);
            }
            return new Tuple<List<CITy>, List<STATE>, List<COUNTRy>>(cityList, stateList, cntryList);
        }
        #endregion

        #region Add/Edit for City
        public CITy GetCityDetails(int cityId)
        {
            CITy cityDet = new CITy();
            try
            {
                if (cityId > 0)
                {
                    cityDet = db.CITIES.Where(w => w.CITY_ID == cityId).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - GetCityDetails : " + ex.Message);
            }
            return cityDet;
        }
        #endregion

        #region Add/Edit for State
        public STATE GetStateDetails(string stateCode)
        {
            STATE stateDet = new STATE();
            try
            {
                if (stateCode != "")
                {
                    stateDet = db.STATES.Where(w => w.STATE_CODE == stateCode).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - GetStateDetails : " + ex.Message);
            }
            return stateDet;
        }
        #endregion

        #region Add/Edit for Country
        public COUNTRy GetCountryDetails(string countryCode)
        {
            COUNTRy countryDet = new COUNTRy();
            try
            {
                if (countryCode != "")
                {
                    countryDet = db.COUNTRIES.Where(w => w.CNTRY_CODE == countryCode).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - GetCountryDetails : " + ex.Message);
            }
            return countryDet;
        }
        #endregion

        #region Save for City
        public string SaveCity(CITy city)
        {
            try
            {
                if (city.CITY_ID == 0)
                {
                    db.CITIES.Add(city);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    CITy cityDet = db.CITIES.Where(w => w.CITY_ID == city.CITY_ID).SingleOrDefault();
                    cityDet.CITY_NAME = city.CITY_NAME;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - SaveCity : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Save for State
        public string SaveState(STATE state)
        {
            try
            {
                STATE stateDet = db.STATES.Where(w => w.STATE_CODE == state.STATE_CODE).SingleOrDefault();
                if (stateDet == null)
                {
                    db.STATES.Add(state);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    stateDet.STATE_NAME = state.STATE_NAME;
                    stateDet.STATE_CNTRY_CODE = state.STATE_CNTRY_CODE;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - SaveState : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Save for Country
        public string SaveCountry(COUNTRy country)
        {
            try
            {
                COUNTRy countryDet = db.COUNTRIES.Where(w => w.CNTRY_CODE == country.CNTRY_CODE).SingleOrDefault();
                if (countryDet == null)
                {
                    db.COUNTRIES.Add(country);
                    db.SaveChanges();
                    message = "Insert";
                }
                else
                {
                    countryDet.CNTRY_NAME = country.CNTRY_NAME;
                    db.SaveChanges();
                    message = "Update";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - SaveCountry : " + ex.Message);
            }
            return message;
        }
        #endregion

        #region Delete for City
        public string DeleteCity(int cityId)
        {
            try
            {
                if (CheckCityDependency(cityId) == false)
                {
                    CITy cityDet = db.CITIES.Where(w => w.CITY_ID == cityId).SingleOrDefault();
                    if (cityDet != null)
                    {
                        db.CITIES.Remove(cityDet);
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
                log.Debug("Exception in RepoCityStateCountry - DeleteCity : " + ex.Message);
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

        #region Delete for State
        public string DeleteState(string stateCode)
        {
            try
            {
                if (CheckStateDependency(stateCode) == false)
                {
                    STATE stateDet = db.STATES.Where(w => w.STATE_CODE == stateCode).SingleOrDefault();
                    if (stateDet != null)
                    {
                        db.STATES.Remove(stateDet);
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
                log.Debug("Exception in RepoCityStateCountry - DeleteState : " + ex.Message);
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

        #region Delete for Country
        public string DeleteCountry(string countryCode)
        {
            try
            {
                if (CheckCountryDependency(countryCode) == false)
                {
                    COUNTRy countryDet = db.COUNTRIES.Where(w => w.CNTRY_CODE == countryCode).SingleOrDefault();
                    if (countryDet != null)
                    {
                        db.COUNTRIES.Remove(countryDet);
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
                log.Debug("Exception in RepoCityStateCountry - DeleteCountry : " + ex.Message);
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

        #region Dependency for City
        public bool CheckCityDependency(int cityId)
        {
            int count = 0;
            try
            {
                count = db.BUS_ENTITIES.Where(w => w.BE_CITY_ID == cityId).Count();
                count = count + db.BUS_UNITS.Where(w => w.BU_CITY_ID == cityId).Count();
                count = count + db.EMPLOYEES.Where(w => w.EMP_CUR_CITY == cityId).Count();
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - CheckCityDependency : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Dependency for State
        public bool CheckStateDependency(string stateCode)
        {
            int count = 0;
            try
            {
                count = db.BUS_ENTITIES.Where(w => w.BE_STATE_CODE == stateCode).Count();
                count = count + db.BUS_UNITS.Where(w => w.BU_STATE_CODE == stateCode).Count();
                count = count + db.EMPLOYEES.Where(w => w.EMP_CUR_STATE == stateCode).Count();
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - CheckStateDependency : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Dependency for Country
        public bool CheckCountryDependency(string countryCode)
        {
            int count = 0;
            try
            {
                count = db.BUS_ENTITIES.Where(w => w.BE_CNTRY_CODE == countryCode).Count();
                count = count + db.BUS_UNITS.Where(w => w.BU_CNTRY_CODE == countryCode).Count();
                count = count + db.EMPLOYEES.Where(w => w.EMP_CUR_COUNTRY == countryCode).Count();
                if (count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - CheckCountryDependency : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check City Name Exists
        public bool CheckCityNameExists(string cityName, int cityId, string creBy)
        {
            bool result = false;
            try
            {
                using (HRMSEntities db = new HRMSEntities())
                {
                    if (creBy == null)
                    {
                        result = db.CITIES.Where(w => w.CITY_NAME == cityName).Count() > 0;
                    }
                    else
                    {
                        result = db.CITIES.Where(w => w.CITY_NAME == cityName && w.CITY_ID != cityId).Count() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - CheckCityNameExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check State Code Exists
        public bool CheckStateCodeExists(string stateCode)
        {
            try
            {
                result = db.STATES.Where(w => w.STATE_CODE == stateCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - CheckStateCodeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check State Name Exists
        public bool CheckStateNameExists(string stateName, string stateCode, string creBy)
        {
            try
            {
                if (creBy != "")
                {
                    result = db.STATES.Where(w => w.STATE_CODE != stateCode && w.STATE_NAME == stateName).Count() > 0;
                }
                else
                {
                    result = db.STATES.Where(w => w.STATE_NAME == stateName).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - CheckStateNameExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Country Code Exists
        public bool CheckCountryCodeExists(string countryCode)
        {
            try
            {
                result = db.COUNTRIES.Where(w => w.CNTRY_CODE == countryCode).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - CheckCountryCodeExists : " + ex.Message);
            }
            return result;
        }
        #endregion

        #region Check Country Name Exists
        public bool CheckCountryNameExists(string countryName, string countryCode, string creBy)
        {
            try
            {
                if (creBy != "")
                {
                    result = db.COUNTRIES.Where(w => w.CNTRY_CODE != countryCode && w.CNTRY_NAME == countryName).Count() > 0;
                }
                else
                {
                    result = db.COUNTRIES.Where(w => w.CNTRY_NAME == countryName).Count() > 0;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception in RepoCityStateCountry - CheckCountryNameExists : " + ex.Message);
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