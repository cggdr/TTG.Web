using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TTG.Core
{
  public  class Role
    {
        [Key]
        public int RoleID { get; set; }
        [Required(ErrorMessage ="必填")]
        [StringLength(20,MinimumLength =2,ErrorMessage ="{0}长度为{2}-{1}个字符")]
        [Display(Name ="名称")]
        public string Name { get; set; }
        [StringLength(1000,ErrorMessage ="{0}必须少于{1}个字符")]
        [Display(Name ="说明")]
        public string Description { get; set; }

    }
}
