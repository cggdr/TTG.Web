using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTG.Web.Areas.Member.Models
{
    public class AddEntrust
    {
        [Required]
        [Range(100, 100000, ErrorMessage = "数量需在100以上")]
        [Display(Name = "数量")]
        public double EAmount { get; set; }
        
        [Range(0,100000)]
        [Display(Name ="价格")]
        public double Eprice { get; set; }
        //以下隐藏表单 作为提交值 便于处理
        public string EPayCoin { get; set; }
        public string ESellCoin { get; set; }


    }
}