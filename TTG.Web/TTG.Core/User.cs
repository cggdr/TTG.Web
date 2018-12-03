using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTG.Core
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage ="必填")]
        [Display(Name ="角色ID")]
        public int RoleID { get; set; }
        //[StringLength(50,MinimumLength =4,ErrorMessage ="{0}长度为{2}-{1}个字符")]
        //[Display(Name ="用户名")]
        //public string  UserName { get; set; }

        [Required(ErrorMessage = "必填")]
        [StringLength(16, ErrorMessage = "长度1-16位")]
        [Display(Name = "请输入手机号")]
        [RegularExpression(@"^1[3458][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        public string PhoneNumber { get; set; }


        [StringLength(20,ErrorMessage ="{0}必须少于{1}个字符")]
        [Display(Name ="名称")]
        public string Name { get; set; }

        [Required(ErrorMessage ="必填")]
        [Range(0,2,ErrorMessage ="{0}范围{1}-{2}")]
        [Display(Name ="性别")]
        public int Sex { get; set; }

        [DataType(DataType.Password)]
        [StringLength(256,ErrorMessage ="{0}长度少于{1}个字符")]
        [Display(Name ="密码")]
        public string  Password{ get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(50,MinimumLength =4,ErrorMessage ="{0}长度为{2}-{1}个字符")]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="必填")]
        [Display(Name ="注册时间")]
        public DateTime RegTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name ="最后登录时间")]
        public Nullable<DateTime> LastLoginTime { get; set; }

        [Display(Name ="最后登录IP")]
        public string LastLoginIP { get; set; }
        public virtual Role Role { get; set; }
        [ForeignKey("UserIdenty")]
        public int FKIdentyID { get; set; }

        public virtual ICollection<Wallet> Wallet { get; set; }
        public virtual ICollection<Entrust> Entrust { get; set; }

        public virtual UserIdenty UserIdenty { get; set; }
    }
}
