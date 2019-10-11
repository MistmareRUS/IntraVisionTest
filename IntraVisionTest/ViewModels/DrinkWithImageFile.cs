using IntraVisionTest.Models;
using System.Web;

namespace IntraVisionTest.ViewModels
{
    public class DrinkWithImageFileViewModel
    {
        public Drink drink { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public DrinkWithImageFileViewModel()
        {
            drink = new Drink();
        }
    }
}