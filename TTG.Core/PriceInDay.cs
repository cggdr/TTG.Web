using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
   public class PriceInDay
    {   [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name ="交易对")]
        public string CoinToCoin { get; set; }
        [Display(Name ="价格")]
        public double Price { get; set; }
        [Display(Name ="24h最高")]
        public Double MaxInDay { get; set; }
        [Display(Name = "24h最低")]
        public Double MinInDay { get; set; }
        [Display(Name = "涨幅")]
        public Double Up { get; set; }
       

        public double VolumeInDay { get; set; }
        public double AmountInDay { get; set; }
        
        public virtual ICollection<PriceInADeal> PriceInADeal { get; set; }
    }
}
