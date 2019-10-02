using IntraVisionTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace IntraVisionTest.Controllers
{
    public class AdminController : Controller
    {
        VendingContext db;
        public AdminController()
        {
            this.db = new VendingContext();
        }
        public ActionResult Index()
        {
            ViewBag.Drinks = db.Drinks.ToList();
            ViewBag.Coins = db.Coins.ToList();
            ViewBag.Title = "Адинистрирование";
            return View();
        }
        [HttpPost]
        public ActionResult SaveCoin(Coin coin)
        {
            var tempCoin = db.Coins.FirstOrDefault(d => d.Id == coin.Id);
            tempCoin.Count = coin.Count;
            tempCoin.Able = coin.Able;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult SaveDrink(Drink drink, HttpPostedFileBase image)
        {
            var tempDrink = db.Drinks.FirstOrDefault(d => d.Id == drink.Id);
            tempDrink.Count = drink.Count;
            tempDrink.Name = drink.Name;
            tempDrink.Price = drink.Price;
            if (image != null && (image.ContentType == "image/png" || image.ContentType == "image/bmp"))
            {
                tempDrink.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(tempDrink.ImageData, 0, image.ContentLength);
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult DeleteDrink(string Name)
        {
            var drink = db.Drinks.FirstOrDefault(d => d.Name == Name);
            db.Drinks.Remove(drink);
            db.SaveChanges();
            var purchase = (Purchase)Session["PurchaseSession"];
            if (purchase != null)
            {
                purchase.Drinks.Remove(purchase.Drinks.FirstOrDefault(d => d.Drink == Name));
            }
            return RedirectToAction("Index");
        }
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
                    ViewBag.ErrorMessage = "Такой напиток уже есть в списке!";
                    ViewBag.Drinks = db.Drinks.ToList();
                    ViewBag.Coins = db.Coins.ToList();
                    ViewBag.Title = "Адинистрирование";
                    return View("Index");
                }
            }
           
            return RedirectToAction("Index");
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