using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntraVisionTest.Models
{
    public class Drink
    {
        public int Id { get; set; }
        //[Remote("Name","")]
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