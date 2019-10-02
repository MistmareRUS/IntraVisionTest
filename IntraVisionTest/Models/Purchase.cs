using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntraVisionTest.Models
{
    public class Purchase
    {
        public Purchase()
        {
            Drinks = new List<OrderedDrinks>();
        }
        public List<OrderedDrinks> Drinks { get; set; }
        public int CurrentSum { get; set; }
        public int PurchaseSum { get; set; }
        public bool ChangeAble { get; set; }
        
    }
    public class OrderedDrinks
    {
        public string Drink { get; set; }
        public int Count { get; set; }
    }
}