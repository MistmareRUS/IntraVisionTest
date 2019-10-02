using IntraVision.Domain.Concrete;
using IntraVision.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraVision.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private VendingContext repository;
        public ProductController()
        {
            this.repository = new VendingContext();
        }
        public PartialViewResult List()
        {
            return PartialView("ProductList",repository.Products);
        }
    }
}