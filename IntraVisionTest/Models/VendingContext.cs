using System;
using System.Data.Entity;
using System.Drawing;
using System.IO;

namespace IntraVisionTest.Models
{
    public class VendingContext:DbContext
    {
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Coin> Coins { get; set; }
    }
    public class VendingInitializer : CreateDatabaseIfNotExists<VendingContext>
    //public class VendingInitializer : DropCreateDatabaseAlways<VendingContext>//TODO: переключать инициализатор тут.
    {
        protected override void Seed(VendingContext context)
        {
            ImageConverter ic = new ImageConverter();
            Image imCola = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\cocacola.png"));
            Bitmap bmpCola = new Bitmap(imCola);
            byte[] cola = (byte[])ic.ConvertTo(bmpCola, typeof(byte[]));

            Image imPepci = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\pepsi.png"));
            Bitmap bmpPepci = new Bitmap(imPepci);
            byte[] pepci = (byte[])ic.ConvertTo(bmpPepci, typeof(byte[]));

            Image imFanta = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\fanta.png"));
            Bitmap bmpFanta = new Bitmap(imFanta);
            byte[] fanta = (byte[])ic.ConvertTo(bmpFanta, typeof(byte[]));

            context.Drinks.Add(new Drink { Name = "Coca-Cola", Price = 15,Count=2,ImageData=cola});            
            context.Drinks.Add(new Drink { Name = "Pepsi", Price = 12,Count=2, ImageData = pepci });
            context.Drinks.Add(new Drink { Name = "Fanta", Price = 14,Count=2, ImageData = fanta });

            Image im1R = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\1R.png"));
            Bitmap bmp1R = new Bitmap(im1R);
            byte[] array1R = (byte[])ic.ConvertTo(bmp1R, typeof(byte[]));

            Image im2R = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\2R.png"));
            Bitmap bmp2R = new Bitmap(im2R);
            byte[] array2R = (byte[])ic.ConvertTo(bmp2R, typeof(byte[]));

            Image im5R = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\5R.png"));
            Bitmap bmp5R = new Bitmap(im5R);
            byte[] array5R = (byte[])ic.ConvertTo(bmp5R, typeof(byte[]));

            Image im10R = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Content\10R.png"));
            Bitmap bmp10R = new Bitmap(im10R);
            byte[] array10R = (byte[])ic.ConvertTo(bmp10R, typeof(byte[]));

            context.Coins.Add(new Coin { Count = 1, Able = true,Cost=1,ImageData=array1R });
            context.Coins.Add(new Coin { Count = 1, Able = true,Cost=2, ImageData = array2R });
            context.Coins.Add(new Coin { Count = 1, Able = false,Cost= 5, ImageData = array5R });
            context.Coins.Add(new Coin { Count = 1, Able = true,Cost= 10, ImageData = array10R });
            base.Seed(context);
        }
    }
}