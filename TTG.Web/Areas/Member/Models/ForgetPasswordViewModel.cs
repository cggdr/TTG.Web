using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTG.Web.Areas.Member.Models
{
    public class ForgetPasswordViewModel
    {
        [Remote("ExistPhoneNumber", "User", HttpMethod = "Post", ErrorMessage = "该手机号未注册")]
        [Required(ErrorMessage = "必填")]
        [StringLength(16, ErrorMessage = "长度1-16位")]
        [Display(Name = "手机号")]
        [RegularExpression(@"^1[3458][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "必须输入{0}")]
        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }
    
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "必填")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "验证码不正确")]
        [Display(Name = "验证码")]
        public string VerificationCode { get; set; }
    }
}