using System.ComponentModel.DataAnnotations;

namespace IntraVisionTest.Models
{
    public class Coin
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
        public bool Able { get; set; }
        public byte[] ImageData { get; set; }
    }
}
