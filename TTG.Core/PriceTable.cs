using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
   public class PriceTable
    {
        [Key]
        public int ID { get; set; }
       [Required]
        public string CoinToCoin { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime KDateTime { get; set; }
        [Required]
        [Range(0,100000)]
        public double OpeningPrice { get; set; }
        [Required]
        [Range(0, 100000)]
        public double ClosingPrice { get; set; }
        [Required]
        [Range(0, 100000)]
        public double MaxPrice { get; set; }
         [Required]
        [Range(0,100000)]
        public double MinPrice { get; set; }
    
        public double Amount { get; set; }
        public virtual PriceInDay PriceInDay { get; set; }

    }
}
