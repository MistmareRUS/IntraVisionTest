using IntraVisionTest.Extentions;
using IntraVisionTest.Models;
using IntraVisionTest.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraVisionTest.Controllers
{
    public class AdminController : Controller
    {
        VendingContext db;
        public AdminController()
        {
            this.db = new VendingContext();
        }
        [AdminAuthorize]
        public ActionResult Index()
        {
            var uvm = new UserViewModel { Coins = db.Coins.ToList(), Drinks = db.Drinks.ToList() };
            return View(uvm);
        }
        [HttpPost]
        public ActionResult SaveCoins(List<Coin> coins)
        {
            if (coins != null)
            {
                foreach (var item in coins)
                {
                    var tempCoin = db.Coins.Find(item.Id);
                    if (tempCoin != null)
                    {
                        tempCoin.Able = item.Able;
                        tempCoin.Count = item.Count;
                    }
                    db.SaveChanges();
                }
                return PartialView("_Money_Admin",db.Coins.ToList());
            }
            else
            {
                return new HttpStatusCodeResult(400, "Error!");
            }
        }
        [HttpPost]
        public ActionResult SaveDrinks(List<DrinkWithImageFileViewModel> drinks)
        {
            if (drinks != null)
            {
                foreach (var item in drinks)
                {
                    var tempDrink = db.Drinks.Find(item.drink.Id);
                    tempDrink.Count = item.drink.Count;
                    tempDrink.Name = item.drink.Name;
                    tempDrink.Price = item.drink.Price;
                    if (item.ImageFile != null && (item.ImageFile.ContentType == "image/png" || item.ImageFile.ContentType == "image/bmp"))
                    {
                        tempDrink.ImageData = new byte[item.ImageFile.ContentLength];
                        item.ImageFile.InputStream.Read(tempDrink.ImageData, 0, item.ImageFile.ContentLength);
                    }
                }
                db.SaveChanges();

                return PartialView("_Drinks_Admin", db.Drinks.ToList());
            }
            else
            {
                return new HttpStatusCodeResult(400, "Error!");
            }
        }
        [AdminAuthorize]
        public ActionResult DeleteDrink(string Name)
        {            
            var drink = db.Drinks.FirstOrDefault(d => d.Name == Name);
            if (drink != null)
            {
                db.Drinks.Remove(drink);
                db.SaveChanges();
                var purchase = (Purchase)Session["PurchaseSession"];
                if (purchase != null)
                {
                    purchase.Drinks.Remove(purchase.Drinks.FirstOrDefault(d => d.Drink == Name));
                }
                return PartialView("_Drinks_Admin", db.Drinks.ToList());
            }
            else
            {
                return new HttpStatusCodeResult(400, "Error!");
            }
        }
        [HttpPost]
        public ActionResult AddDrink(Drink drink, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (CheckName(drink.Name))
                {
                    if (image != null && (image.ContentType == "image/png" || image.ContentType == "image/bmp"))
                    {
                        drink.ImageData = new byte[image.ContentLength];
                        image.InputStream.Read(drink.ImageData, 0, image.ContentLength);
                    }
                    db.Drinks.Add(drink);
                    db.SaveChanges();
                    var purchase = (Purchase)Session["PurchaseSession"];
                    if (purchase != null)
                    {
                        purchase.Drinks.Add(new OrderedDrinks { Drink = drink.Name });
                    }                    
                }
                else
                {
                    return new HttpStatusCodeResult(400, "Error! There is the drink with the same title.");
                }
            }
            return PartialView("_Drinks_Admin", db.Drinks.ToList());
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [HttpGet]
        public bool CheckName(string name)
        {
            return !(db.Drinks.Any(d => d.Name.ToLower() == name.ToLower()));
        }
    }
}