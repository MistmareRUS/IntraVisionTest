using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraVision.Domain.Entities
{
    public class Coin
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public int Count { get; set; }
        public bool Able { get; set; }
    }
}
