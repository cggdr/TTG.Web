using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTG.Web.Areas.Member.Models
{
    public class AddSale
    {
        [Required]
        [Range(100,100000,ErrorMessage ="数量需在100以上")]
        [Display(Name ="数量")]
        public double  Amount { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="交易密码")]
        [StringLength(16,MinimumLength =6,ErrorMessage ="交易密码需在6位以上")]
        public string Password { get; set; }
        //以下隐藏表单 作为提交值 便于处理
        public string PayCoin { get; set; }
        public string  SellCoin { get; set; }
        public int ID { get; set; }
        public double price { get; set; }

    }
}