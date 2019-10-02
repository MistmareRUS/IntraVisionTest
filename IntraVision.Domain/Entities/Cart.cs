using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraVision.Domain.Entities
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; }
        public int CurrentSum { get; set; }
        public int PriceSum { get; set; }
        public bool ChangeAble { get; set; }

        public void AddProduct(Product p)
        {

        }
        public void DeleteProduct(Product p)
        {

        }
        //private List<CartLine> lineCollection = new List<CartLine>();
        //public void AddItem(Product product, int quantity)
        //{
        //    CartLine line = lineCollection
        //    .Where(p => p.Product.Id == product.Id)
        //    .FirstOrDefault();
        //    if (line == null)
        //    {
        //        lineCollection.Add(new CartLine
        //        {
        //            Product = product,
        //            Quantity = quantity
        //        });
        //    }
        //    else
        //    {
        //        line.Quantity += quantity;
        //    }
        //}
        //public void RemoveLine(Product product)
        //{
        //    lineCollection.RemoveAll(l => l.Product.Id == product.Id);
        //}
        //public int ComputeTotalValue()
        //{
        //    return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        //}
        //public void Clear()
        //{
        //    lineCollection.Clear();
        //}
        //public IEnumerable<CartLine> Lines
        //{
        //    get { return lineCollection; }
        //}
    }
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
