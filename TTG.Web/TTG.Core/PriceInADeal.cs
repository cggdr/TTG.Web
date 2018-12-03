using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
   public  class PriceInADeal
    {   [Key]
        public int ID { get; set; }
        [ForeignKey("PriceInDay")]
        public int PriceInDayID { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public DateTime DealTime { get; set; }
        [Required]
        [Range(0,100000)]
        public double Price { get; set; }
        [Required]
        [Range(100,100000)]
        public double Amount { get; set; }
        public virtual PriceInDay PriceInDay { get; set; }
    }
}
