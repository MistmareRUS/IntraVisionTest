using IntraVisionTest.Models;
using IntraVisionTest.ViewModels;
using System.Linq;
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
            var uvm = new UserViewModel { Coins = db.Coins.ToList(), Purchase = purchase, Drinks=db.Drinks.ToList()};
            return View(uvm);
        }
        [HttpPost]
        public ActionResult InsertCoin(int value)
        {
            purchase = (Purchase)Session["PurchaseSession"];
            var coin= db.Coins.FirstOrDefault(c => c.Cost == value);
            if (coin!=null && coin.Able)
            {
                purchase.CurrentSum += value;
                coin.Count++;
                db.SaveChanges();
                Session["PurchaseSession"] = purchase;
                var vm = new UserViewModel { Purchase = purchase, Drinks = db.Drinks.ToList() };
                return PartialView("_Drinks_User", vm);
            }
            return new  HttpStatusCodeResult(400, "Error! Coin was returned.");
        }
        [HttpPost]
        public ActionResult ShowInfo()
        {
            purchase = (Purchase)Session["PurchaseSession"];
            TagBuilder answer = new TagBuilder("h3");
            answer.AddCssClass("InfoBlock");
            if (purchase.ChangeAble && purchase.PurchaseSum > 0)
            {
                TagBuilder link = new TagBuilder("a");
                link.Attributes.Add("href", "/Home/CompletePurchase");
                link.SetInnerText("Завершить");
                answer.InnerHtml = link.ToString();
            }
            else if (purchase.CurrentSum == 0)
            {
                answer.InnerHtml = "Внесите деньги";
            }
            else if (!purchase.ChangeAble && purchase.PurchaseSum > 0)
            {
                answer.InnerHtml = "В аппарате нет монет для сдачи.Обратитесь к менеджеру или измените заказ";
            }
            else
            {
                answer.InnerHtml = "Вы можете внести еще деньги или выбрать напики, стоимость которых не превышает свободный остаток";
            }
            return Content(answer.ToString());
        }
        [HttpPost]
        public ActionResult ShowSums()
        {
            purchase = (Purchase)Session["PurchaseSession"];
            TagBuilder r1 = new TagBuilder("h4");
            r1.AddCssClass("InfoBlock");
            r1.InnerHtml = "Внесенная сумма";

            TagBuilder r3 = new TagBuilder("h4");
            r3.AddCssClass("InfoBlock");
            r3.InnerHtml = "Сумма покупки";

            TagBuilder r2 = new TagBuilder("h3");
            r2.AddCssClass("InfoBlock");
            r2.InnerHtml = purchase.CurrentSum.ToString("c");

            TagBuilder r4 = new TagBuilder("h3");
            r4.AddCssClass("InfoBlock");
            r4.InnerHtml = purchase.PurchaseSum.ToString("c");            

            return Content(r1.ToString()+ r2.ToString() + r3.ToString() + r4.ToString());
        }
        [HttpPost]
        public ActionResult AddDrink(string drink)
        {
            purchase = (Purchase)Session["PurchaseSession"];
            var drinkInPurchase = purchase.Drinks.FirstOrDefault(d => d.Drink == drink);
            var drinkInDB = db.Drinks.FirstOrDefault(d => d.Name == drinkInPurchase.Drink);
            if (drinkInPurchase!=null && drinkInDB!=null)
            {
                if (drinkInPurchase.Count<drinkInDB.Count&&(purchase.CurrentSum-purchase.PurchaseSum)>=drinkInDB.Price)//перепроверка, что напитка для заказа хватает
                {
                    drinkInPurchase.Count++;
                    SumCalculate();
                    purchase.ChangeAble = Changeable();
                    Session["PurchaseSession"] = purchase;
                    var vm = new UserViewModel { Purchase = purchase, Drinks = db.Drinks.ToList() };
                    return PartialView("_Drinks_User", vm);
                }
            }            
            return new HttpStatusCodeResult(400, "Error! Something was wrong. Try Again.");
        }
        public ActionResult RemoveDrink(string drink)
        {
            purchase = (Purchase)Session["PurchaseSession"];
            var drinkInPurchase = purchase.Drinks.FirstOrDefault(d => d.Drink == drink);
            if (drinkInPurchase?.Count > 0)
            {
                purchase.Drinks.FirstOrDefault(d => d.Drink == drink).Count--;
                SumCalculate();
                purchase.ChangeAble = Changeable();
                Session["PurchaseSession"] = purchase;
                var vm = new UserViewModel { Purchase = purchase, Drinks = db.Drinks.ToList() };
                return PartialView("_Drinks_User", vm);
            }
            return new HttpStatusCodeResult(400, "Error! Something was wrong. Try Again.");
        }
        public ActionResult CompletePurchase()
        {
            purchase = (Purchase)Session["PurchaseSession"];
            if (purchase != null && DrinksValidate())
            {
                string complete = "";                
                complete += GiveDrinks();
                complete += GiveChange();
                ViewBag.Message = complete;
                purchase = new Purchase();
                foreach (var item in db.Drinks)
                {
                    purchase.Drinks.Add(new OrderedDrinks { Drink = item.Name });
                }
                Session["PurchaseSession"] = purchase;
            }
            else
            {
                purchase.ChangeAble = Changeable();
                ViewBag.Message = "Напитки в хранилище были изменены, необходимо перепроверить заказ.";
            }
            var uvm = new UserViewModel { Coins = db.Coins.ToList(), Purchase = purchase, Drinks = db.Drinks.ToList() };
            return View("Index", uvm);
        }
        private bool DrinksValidate()//проверка, на случай, если напитки выкупили с другого окна и количество в БД меньше, чем в текущей покупке.
        {
            bool result = true;
            foreach (var item in purchase.Drinks)
            {
                if (item.Count > 0)
                {
                    var tempDrink = db.Drinks.FirstOrDefault(d => d.Name == item.Drink);
                    if (tempDrink==null)//имя напитка былоизменено
                    {
                        item.Count = 0;
                        continue;
                    }
                    if (tempDrink.Count < item.Count)
                    {
                        item.Count = tempDrink.Count;//если не хватает-уменьшает кол-во до имеющегося
                        result = false;
                    }
                }
            }
            SumCalculate();
            if(purchase.PurchaseSum>purchase.CurrentSum)//если цены изменились и внесенной суммы не хватает, то не ясно, какой напиток лишний. ВЫбранные напитки сбрасываются.
            {
                purchase.Drinks = new System.Collections.Generic.List<OrderedDrinks>();
                foreach (var item in db.Drinks)
                {
                    purchase.Drinks.Add(new OrderedDrinks { Drink = item.Name });
                }
                purchase.PurchaseSum = 0;
                result = false;
            }
            return result;
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
                    outText += $"\n{item.Count} монет(а) по {item.Cost}.";
                    var coin = db.Coins.FirstOrDefault(c => c.Cost == item.Cost);
                    coin.Count -= item.Count;
                    db.SaveChanges();
                }
                if (needToChangeSum == 0)
                {
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
                    var dr = db.Drinks.FirstOrDefault(d => d.Name == item.Drink);
                    if (dr == null)//имя напитка былоизменено
                    {
                        item.Count = 0;
                        continue;
                    }
                    int price = dr.Price;
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