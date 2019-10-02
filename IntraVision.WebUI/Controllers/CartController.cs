using IntraVision.Domain.Concrete;
using IntraVision.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraVision.WebUI.Controllers
{
    public class CartController : Controller
    {
        private VendingContext repository;
        public CartController(VendingContext repo)
        {
            repository = new VendingContext();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}