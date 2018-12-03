using System;
using System.ComponentModel.DataAnnotations;

namespace TTG.Core
{
    public class Application
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage ="请输入需上市的货币名称")]
        [StringLength(10,MinimumLength =3)]
        [Display(Name ="货币名称(英文代称)")]
        
        public string CoinName { get; set; }

        [Required(ErrorMessage ="请输入您的真实姓名")]
        [StringLength(8,MinimumLength =4)]
        [Display(Name = "真实姓名")]
        public string ApplicantName{ get; set; }
        [Required(ErrorMessage ="请输入您的联系电话")]
        [StringLength(12)]
        [Display(Name = "联系电话")]
        public string  PhoneNum { get; set; }

        [Required(ErrorMessage = "请输入您的联系邮箱")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的Email地址")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="请输入贵公司的名称")]
        [StringLength(36)]
        [Display(Name = "公司名称")]
        public string  CompanyName { get; set; }
        [Required]
        [Display(Name ="创建时间")]
        public DateTime CreateTime { get; set; }
        [Required]
        [Display(Name ="审查时间")]
        public DateTime ReviewTime { get; set; }
        [Required]
        [Display(Name ="状态")]
        public int Status { get; set; }
        public string FileAddress { get; set; }


    }
}
