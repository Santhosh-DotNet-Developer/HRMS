using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Repository;
using HRMS.Models;

namespace HRMS.Controllers
{
    [CheckSessionTimeOut]
    public class CityStateCountryController : Controller
    {
        #region Declaration
        RepoCityStateCountry rp = new RepoCityStateCountry();
        #endregion

        #region Index
        public ActionResult Index()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Show = TempData["message"];
            }
            if (TempData["content"] != null)
            {
                ViewBag.Content = TempData["content"];
            }
            if (TempData["tab"] != null)
            {
                ViewBag.CurTab = TempData["tab"];
            }
            return View(rp.GetList());
        }
        #endregion

        #region Add/Edit for City
        public ActionResult AddEditCity(int cityId)
        {
            return PartialView(rp.GetCityDetails(cityId));
        }
        #endregion

        #region Add/Edit for State
        public ActionResult AddEditState(string stateCode)
        {
            return PartialView(rp.GetStateDetails(stateCode));
        }
        #endregion

        #region Add/Edit for Country
        public ActionResult AddEditCountry(string countryCode)
        {
            return PartialView(rp.GetCountryDetails(countryCode));
        }
        #endregion

        #region Save for City
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCity(CITy city)
        {
            string result = rp.SaveCity(city);
            if (result == "Insert")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Insert;
            }
            else if (result == "Update")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Update;
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "City";
            return RedirectToAction("Index");
        }
        #endregion

        #region Save for State
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveState(STATE state)
        {
            string result = rp.SaveState(state);
            if (result == "Insert")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Insert;
            }
            else if (result == "Update")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Update;
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "State";
            return RedirectToAction("Index");
        }
        #endregion

        #region Save for Country
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCountry(COUNTRy country)
        {
            string result = rp.SaveCountry(country);
            if (result == "Insert")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Insert;
            }
            else if (result == "Update")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Update;
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            TempData["tab"] = "Country";
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete for City
        public ActionResult DeleteCity(int cityId)
        {
            string result = rp.DeleteCity(cityId);
            if (result == "Delete")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Delete;
            }
            else if (result == "Child")
            {
                TempData["message"] = result;
                TempData["content"] = "Cannot delete this city, This city has dependencies in the application.";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete for State
        public ActionResult DeleteState(string stateCode)
        {
            string result = rp.DeleteState(stateCode);
            if (result == "Delete")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Delete;
            }
            else if (result == "Child")
            {
                TempData["message"] = result;
                TempData["content"] = "Cannot delete this State, as it has dependencies in the application.";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete for Country
        public ActionResult DeleteCountry(string countryCode)
        {
            string result = rp.DeleteCountry(countryCode);
            if (result == "Delete")
            {
                TempData["message"] = result;
                TempData["content"] = Constant.Delete;
            }
            else if (result == "Child")
            {
                TempData["message"] = result;
                TempData["content"] = "Cannot delete this Country, as it has dependencies in the application.";
            }
            else
            {
                TempData["message"] = "Error";
                TempData["content"] = result;
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region JSON
        #region Check City Name Exists
        public JsonResult CheckCityNameExists(string cityName, int cityId, string creBy)
        {
            bool result = rp.CheckCityNameExists(cityName, cityId, creBy);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check State Code Exists
        public JsonResult CheckStateCodeExists(string stateCode)
        {
            bool result = rp.CheckStateCodeExists(stateCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check State Name Exists
        public JsonResult CheckStateNameExists(string stateName, string stateCode, string creBy)
        {
            bool result = rp.CheckStateNameExists(stateName, stateCode, creBy);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Country Code Exists
        public JsonResult CheckCountryCodeExists(string countryCode)
        {
            bool result = rp.CheckCountryCodeExists(countryCode);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check Country Name Exists
        public JsonResult CheckCountryNameExists(string countryName, string countryCode, string creBy)
        {
            bool result = rp.CheckCountryNameExists(countryName, countryCode, creBy);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rp.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}