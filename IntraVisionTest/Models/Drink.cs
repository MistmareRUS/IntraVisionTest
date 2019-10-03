using System.ComponentModel.DataAnnotations;

namespace IntraVisionTest.Models
{
    public class Drink
    {
        public int Id { get; set; }
        [Required]
        [MinLength(1,ErrorMessage ="Слишком короткое название")]
        public string Name { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int Price { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
        public byte[] ImageData { get; set; }
    }
}