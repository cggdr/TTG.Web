using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTG.Web.Areas.Member.Models
{
    public class C2CViewModel
    {
        [Required(ErrorMessage ="请输入数量")]
        [Range(100, 100000, ErrorMessage = "数量需在100以上")]
        [Display(Name = "卖出量(CNYX)")]
        public double Amount { get; set; }
      
        
        [Display(Name ="卖出价(￥)")]
        public double? C2CPrice { get; set; }

        [Required(ErrorMessage ="请输入交易密码")]
        [DataType(DataType.Password)]
        [Display(Name = "交易密码")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "交易密码需在6位以上")]
        public string C2CPassword { get; set; }
       
        
    }
}