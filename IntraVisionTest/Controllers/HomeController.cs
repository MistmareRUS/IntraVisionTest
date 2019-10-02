using IntraVisionTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraVisionTest.Controllers
{
    public class HomeController : Controller
    {
        VendingContext db;
        Purchase purchase;
        public HomeController()
        {
            this.db = new VendingContext();                  
        }
        public ActionResult Index()
        {
            if (Request["SecretKey"]!=null && Request["SecretKey"].ToLower() == "magickey")
            {
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.Drinks = db.Drinks.ToList();
            ViewBag.Coins = db.Coins.ToList();
            ViewBag.Title = "Покупка напитков";
            {
                purchase = (Purchase)Session["PurchaseSession"];
                if (purchase == null)
                {
                    purchase = new Purchase();
                    foreach (var item in db.Drinks)
                    {
                        purchase.Drinks.Add(new OrderedDrinks { Drink = item.Name });
                    }
                    Session["PurchaseSession"] = purchase;
                }
                else
                {
                    SumCalculate();
                    purchase.ChangeAble = Changeable();
                    Session["PurchaseSession"] = purchase;
                }
            }
            return View(purchase);
        }
        public ActionResult InsertCoin(int value)
        {
            purchase = (Purchase)Session["PurchaseSession"];
            purchase.CurrentSum += value;
            var coin= db.Coins.FirstOrDefault(c => c.Cost == value);
            coin.Count++;
            db.SaveChanges();
            Session["PurchaseSession"] = purchase;
            return RedirectToAction("Index");
        }
        public ActionResult AddDrink(string drink)
        {
            purchase = (Purchase)Session["PurchaseSession"];
            purchase.Drinks.FirstOrDefault(d => d.Drink == drink).Count++;

            SumCalculate();
            purchase.ChangeAble = Changeable();
            Session["PurchaseSession"] = purchase;
            return RedirectToAction("Index");
        }
        public ActionResult RemoveDrink(string drink)
        {
            purchase = (Purchase)Session["PurchaseSession"];
            purchase.Drinks.FirstOrDefault(d => d.Drink == drink).Count--;
            SumCalculate();
            purchase.ChangeAble = Changeable();
            Session["PurchaseSession"] = purchase;
            return RedirectToAction("Index");
        }
        public ActionResult CompletePurchase()
        {
            purchase = (Purchase)Session["PurchaseSession"];
            string complete = "";
            complete += GiveDrinks();
            complete += GiveChange();
            ViewBag.Complete = complete;
            purchase = new Purchase();
            foreach (var item in db.Drinks)
            {
                purchase.Drinks.Add(new OrderedDrinks { Drink = item.Name });
            }
            return RedirectToAction("Index");
        }
        private bool Changeable()
        {
            int needToChangeSum = purchase.CurrentSum - purchase.PurchaseSum;
            var coins = db.Coins.ToList().OrderByDescending(c => c.Cost);
            foreach (var item in coins)
            {
                int needToGive = needToChangeSum / item.Cost;
                if (item.Count >= needToGive)
                {
                    needToChangeSum -= (item.Cost * needToGive);
                }
                else
                {
                    needToChangeSum -= (item.Cost * item.Count);
                }
                if (needToChangeSum == 0)
                {
                    return true;
                }
            }
            return false;
        }
        private string GiveChange()
        {
            int needToChangeSum = purchase.CurrentSum - purchase.PurchaseSum;
            if (needToChangeSum == 0)
            {
                return "\nБез сдачи";
            }
            var coins = db.Coins.ToList().OrderByDescending(c => c.Cost);
            string outText = "\nВыдано сдачи: ";
            foreach (var item in coins)
            {
                int needToGive = needToChangeSum / item.Cost;
                if (item.Count >= needToGive)
                {
                    needToChangeSum -= (item.Cost * needToGive);
                    outText += $"\n{needToGive} монет(а) по {item.Cost}";
                    var coin = db.Coins.FirstOrDefault(c => c.Cost == item.Cost);
                    coin.Count -= needToGive;
                    db.SaveChanges();
                }
                else
                {
                    needToChangeSum -= (item.Cost * item.Count);
                    outText += $"\n{item.Count} монет(а) по {item.Cost}";
                    var coin = db.Coins.FirstOrDefault(c => c.Cost == item.Cost);
                    coin.Count -= item.Count;
                    db.SaveChanges();
                }
                if (needToChangeSum == 0)
                {
                    outText += ".";
                    return outText;
                }
            }
            return "";
        }
        private string GiveDrinks()
        {
            string outText = "\nВыданы напитки: ";
            foreach (var item in purchase.Drinks)
            {
                if (item.Count==0)
                {
                    continue;
                }
                var drink = db.Drinks.FirstOrDefault(d => d.Name == item.Drink);
                outText += $"\n{drink.Name}: {item.Count} шт";
                drink.Count -= item.Count;
                db.SaveChanges();
            }
            return outText;
        }
        private void SumCalculate()
        {
            int sum=0;
            foreach (var item in purchase.Drinks)
            {
                if (item.Count > 0)
                {
                    int price = db.Drinks.FirstOrDefault(d => d.Name == item.Drink).Price;
                    sum += (item.Count * price);
                }
            }
            purchase.PurchaseSum = sum;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}