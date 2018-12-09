using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
   public class Wallet
    {
        [Key]
        public int WalletID { get; set; }
        [Required]
        public int UserID { get; set; }
        [ForeignKey("VirtualCurrency")]
        public int VirCurID { get; set; }
        public double Amount { get; set; }
        public virtual User User { get; set; }
        public virtual VirtualCurrency VirtualCurrency { get; set; }
        public virtual PriceTable PriceTable { get; set; }
        public virtual ICollection<Interaction> Interaction { get; set; }

    }
}
