using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
  public class VirtualCurrency
    {   [Key]
        public int VirCurID { get; set; }
        [Required(ErrorMessage = "必填")]
        [StringLength(6, MinimumLength = 2, ErrorMessage = "类型名应在2-6个字符范围内")]
        public string  Name { get; set; }
        [Required]
        public int TypeID { get; set; }
       
        [StringLength(maximumLength:1000)]
        public string  Description { get; set; }
        public virtual  Type Type { get; set; }
     

    }
}
