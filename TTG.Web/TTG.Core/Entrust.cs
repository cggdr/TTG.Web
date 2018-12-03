using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
  public class Entrust
    {
        #region
        /// <summary>
        /// </summary>
        #endregion
        [Key]
        public int ID { get; set; }
        
       [ForeignKey("User")]
        public int FUserID { get; set; }
        [Required]
        public string CoinName { get; set; }
        [Required]
        public string PayCoinName { get; set; }
        [Required]
        [Range(100,100000)]
        public Double Amount { get; set; }
        [Required]
        [Range(0,100000)]
        public double Price { get; set; }
       
        //-1撤回 0正在交易 1交易完成
        [Required]
        public int IsSuccess { get; set; }
        [Required]
        public DateTime EnstructTime { get; set; }

        public DateTime SucOrDefTime { get; set; }
        public virtual User User { get; set; }
 
        public virtual ICollection<EntrustDetails> EntrustDetails { get; set; }

    }
}
