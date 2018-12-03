using System.ComponentModel.DataAnnotations;

namespace TTG.Web.Areas.Member.Models
{
    public class AddInViewModel
    {   [Required(ErrorMessage ="请输入{0}")]
        [Range(100,100000,ErrorMessage ="操作数量应在100以上")]
        [Display(Name ="数量")]
        public double Amount { get; set; }
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(256,MinimumLength =20,ErrorMessage ="请输入合适的钱包地址")]
        [Display(Name ="钱包地址")]
        public string WalletAddrress { get; set; }
    }
}