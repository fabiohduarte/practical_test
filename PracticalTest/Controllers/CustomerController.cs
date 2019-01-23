using PracticalTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PracticalTest.Controllers
{
    public class CustomerController : Controller
    {
        private PracticalTest.DAL.PracticalTest db = PracticalTest.DAL.PracticalTest.GetInstance();


        #region properties
        private bool IsUserAdmin
        {
            get {
                if (Session["IsUserAdmin"] != null)
                    return Session["IsUserAdmin"].ToString().Equals("1");
                return false;
            }
        }

        private int? UserId
        {
            get
            {
                if (Session["UserId"] != null)
                    return Convert.ToInt32(Session["UserId"]);
                return null;
            }
        }
        #endregion

        [Authorize]
        public ActionResult Index()
        {           
            return View();
        }

        [Authorize]
        public ViewResult List(string name, int? genderId, int? cityId, int? regionId, DateTime? lastPurchase, DateTime? lastPurchaseUntil, int? classificationId, int? sellerId,
                               string sortOrder, int? page, int? clearFields)
        {

            #region Clear Filters
            // clear filters
            if (clearFields.HasValue && clearFields.GetValueOrDefault() == 1)
            {
                name = String.Empty;
                genderId = null;
                cityId = null;
                regionId = null;
                lastPurchase = null;
                lastPurchaseUntil = null;
                classificationId = null;
                sellerId = null;              
                page = 1;
                ViewBag.Name = null;
               
            }

            #endregion


            ViewBag.NameParam = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.PhoneParam = sortOrder == "Phone" ? "Phone_desc" : "Phone";
            ViewBag.GenderParam = sortOrder == "Gender" ? "Gender_desc" : "Gender";
            ViewBag.CityParam = sortOrder == "City" ? "City_desc" : "City";
            ViewBag.RegionParam = sortOrder == "Region" ? "Region_desc" : "Region";
            ViewBag.LastPurchaseParam = sortOrder == "LastPurchase" ? "LastPurchase_desc" : "LastPurchase";
            ViewBag.ClassificationParam = sortOrder == "Classification" ? "Classification_desc" : "Classification";
            ViewBag.SellerParam = sortOrder == "Seller" ? "Seller_desc" : "Seller";

            ViewBag.city = getCities();
            ViewBag.classification = getClassifications();
            ViewBag.gender = getGenders();
            ViewBag.region = getRegions(cityId);
            ViewBag.seller = getSellers();

            var customers = from c in db.Customer select c;

            #region apply filters

            if (!String.IsNullOrEmpty(name))
            {
                customers = customers.Where(c => c.Name.ToUpper().Contains(name.ToUpper()));
            }

            if (genderId.HasValue)
            {
                customers = customers.Where(c => c.GenderId == genderId.Value);
            }

            if (cityId.HasValue)
            {
                customers = customers.Where(c => c.CityId == cityId.Value);
                int t = customers.Count();
            }

            if (regionId.HasValue)
            {
                customers = customers.Where(c => c.RegionId == regionId.Value);
            }

            if (lastPurchase.HasValue)
            {
                if (lastPurchaseUntil.HasValue)
                    customers = customers.Where(c => c.LastPurchase >= lastPurchase && c.LastPurchase <= lastPurchaseUntil);
                else
                    customers = customers.Where(c => c.LastPurchase == lastPurchase);
            }

            if (classificationId.HasValue)
            {
                customers = customers.Where(c => c.ClassificationId == classificationId.Value);
            }

            if (!IsUserAdmin)
            {
                sellerId = UserId;
            }
            
            if (sellerId.HasValue)
            {
                customers = customers.Where(c => c.UserId == sellerId.Value);
            }
           
            #endregion

            IEnumerable<CustomerViewModel> result = SelectDataToView(customers);

            #region Sorting columns

            switch (sortOrder)
            {
                case "Classification":
                    result = result.OrderBy(c => c.Classification);
                    break;
                case "Classification_desc":
                    result = result.OrderByDescending(c => c.Classification);
                    break;
                case "Name_desc":
                    result = result.OrderByDescending(c => c.Name);
                    break;
                case "Phone":
                    result = result.OrderBy(c => c.Phone);
                    break;
                case "Phone_desc":
                    result = result.OrderByDescending(c => c.Phone);
                    break;
                case "Gender":
                    result = result.OrderBy(c => c.Gender);
                    break;
                case "Gender_desc":
                    result = result.OrderByDescending(c => c.Gender);
                    break;
                case "City":
                    result = result.OrderBy(c => c.City);
                    break;
                case "City_desc":
                    result = result.OrderByDescending(c => c.City);
                    break;
                case "Region":
                    result = result.OrderBy(c => c.Region);
                    break;
                case "Region_desc":
                    result = result.OrderByDescending(c => c.Region);
                    break;
                case "LastPurchase":
                    result = result.OrderBy(c => c.LastPurchase);
                    break;
                case "LastPurchase_desc":
                    result = result.OrderByDescending(c => c.LastPurchase);
                    break;
                case "Seller":
                    result = result.OrderBy(c => c.Seller);
                    break;
                case "Seller_desc":
                    result = result.OrderByDescending(c => c.Seller);
                    break;

                default:
                    result = result.OrderBy(c => c.Name);
                    break;
            }

            #endregion

             if (clearFields.HasValue)
            {
                ModelState.Clear();
                clearFields = null;
            }

             return View(result);
        }

        private List<CustomerViewModel> SelectDataToView(IQueryable<DAL.Customer> customers)
        {
            return (from c in customers
                              join classif in db.Classification on c.ClassificationId equals classif.Id
                              join city in db.City on c.CityId equals city.Id
                              join gender in db.Gender on c.GenderId equals gender.Id
                              join region in db.Region on c.RegionId equals region.Id
                              join seller in db.UserSys on c.UserId equals seller.Id
                              select new CustomerViewModel
                              {
                                  Id = c.Id,
                                  Name = c.Name,
                                  Phone = c.Phone,
                                  Gender = gender.Name,
                                  City = city.Name,
                                  Region = region.Name,
                                  LastPurchase = c.LastPurchase,
                                  Classification = classif.Name,
                                  Seller = seller.Login
                              }).ToList();
        }

        #region Auxiliar Methods
        public List<SelectListItem> getCities()
        {
            var cities = new List<SelectListItem>();

            cities.Add(new SelectListItem() { Text = "", Value = "" });

            foreach (var obj in db.City.ToList())
            {
                cities.Add(new SelectListItem() { Text = obj.Name, Value = obj.Id.ToString() });
            }
            return cities;
        }

        public List<SelectListItem> getClassifications()
        {
            var classifications = new List<SelectListItem>();

            classifications.Add(new SelectListItem() { Text = "", Value = "" });

            foreach (var obj in db.Classification.ToList())
            {
                classifications.Add(new SelectListItem() { Text = obj.Name, Value = obj.Id.ToString() });
            }
            return classifications;
        }


        public List<SelectListItem> getGenders()
        {
            var genders = new List<SelectListItem>();

            genders.Add(new SelectListItem() { Text = "", Value = "" });

            foreach (var obj in db.Gender.ToList())
            {
                genders.Add(new SelectListItem() { Text = obj.Name, Value = obj.Id.ToString() });
            }
            return genders;
        }

        public List<SelectListItem> getRegions(int? cityId)
        {
          
            var regions = new List<SelectListItem>();

            regions.Add(new SelectListItem() { Text = "", Value = "" });

            foreach (var obj in db.Region.Where(r => r.City.Any(c => c.Id == cityId || cityId == null)).ToList())
            {
                regions.Add(new SelectListItem() { Text = obj.Name, Value = obj.Id.ToString() });
            }
            return regions;
        }

        public List<SelectListItem> getSellers()
        {
            var sellers = new List<SelectListItem>();

            sellers.Add(new SelectListItem() { Text = "", Value = "" });

            foreach (var obj in db.UserSys.Where(p => p.UserRoleId != 1).ToList())
            {
                sellers.Add(new SelectListItem() { Text = obj.Login, Value = obj.Id.ToString() });
            }
            return sellers;
        }
        #endregion

    }
}