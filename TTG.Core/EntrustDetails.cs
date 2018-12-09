using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
     public class EntrustDetails
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Entrust")]
        public int EntrustID { get; set; }
        [Required]
        public int BuyerID { get; set; }
        [Required]
        public int SellID { get; set; }
        [Range(100,100000)]
        public double Amount { get; set; }
        public virtual Entrust Entrust { get; set; }


    }
}
