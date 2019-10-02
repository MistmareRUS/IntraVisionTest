using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using IntraVision.Domain.Entities;

namespace IntraVision.Domain.Concrete
{
    public class VendingContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Coin> Coins { get; set; }
    }
    //public class VendingInitializer : CreateDatabaseIfNotExists<VendingContext>
    public class VendingInitializer : DropCreateDatabaseAlways<VendingContext>
    {
        protected override void Seed(VendingContext context)
        {
            context.Products.Add(new Product { Name = "Caka-Cola", Price = 15 });
            context.Products.Add(new Product { Name = "Pepsi", Price = 1, });
            context.Products.Add(new Product { Name = "Fanta", Price = 14 });

            context.Coins.Add(new Coin { Count = 10, Able = true, Cost = 1 });
            context.Coins.Add(new Coin { Count = 10, Able = true, Cost = 2 });
            context.Coins.Add(new Coin { Count = 10, Able = false, Cost = 5 });
            context.Coins.Add(new Coin { Count = 10, Able = true, Cost = 10 });
            base.Seed(context);
        }
    }
}
