using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
   public class Type
    {
        [Key]
        public int ID  { get; set; }
        [Required(ErrorMessage = "必填")]
        [StringLength(6, MinimumLength = 2, ErrorMessage = "类型名应在2-6个字符范围内")]
        public string TypeName { get; set; }
    }
}
