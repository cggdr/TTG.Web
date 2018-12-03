using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
  public  class Interaction
    {   [Key]
        public int ID { get; set; }
        [ForeignKey("Wallet")]
        [Required(ErrorMessage = "必填")]
        public int FWalletID { get; set; }
        [StringLength(256,MinimumLength =20,ErrorMessage ="请输入合适的钱包地址")]
        public string WalletAddress { get; set; }
        //0审核中 1成功 -1 失败
        [Required(ErrorMessage = "必填")]
        public int Status { get; set; }
        [Required(ErrorMessage ="必填")]
        [Range(100,100000,ErrorMessage ="操作数量应在100以上")]
        public double Amount { get; set; }
        [DataType(DataType.Time)]
        public DateTime OperatingTime { get; set; }
        //1 充值 -1提现
        [Required]
        public int InOrOut { get; set; }
        public virtual  Wallet Wallet { get; set; }
    }
}
