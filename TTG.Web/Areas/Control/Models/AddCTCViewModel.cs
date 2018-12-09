using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTG.Web.Areas.Control.Models
{
    public class AddCTCViewModel
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        [Required(ErrorMessage = "必须选择{0}")]
        [Display(Name = "结算货币")]
        public int ID { get; set; }

        /// <summary>
        /// 货币名称(英文代称)
        /// </summary>
        [StringLength(20, ErrorMessage = "{0}必须少于{1}个字符")]
        [Display(Name = "货币名称(英文代称)")]
        public string Name { get; set; }

    }
}