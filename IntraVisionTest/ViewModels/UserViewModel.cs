using IntraVisionTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntraVisionTest.ViewModels
{
    public class UserViewModel
    {
        public Purchase Purchase { get; set; }
        public List<Coin> Coins { get; set; }
        public List<Drink> Drinks { get; set; }
    }
}