using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTG.Core
{
    public class UserIdenty
    {
        [Key]
        public int ID { get; set; }
      
        [StringLength(256)]
        [Display(Name = "交易密码")]
        public string Password { get; set; }
      

    }
}
