using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TTG.Web.Areas.Member.Models
{
    public class RegisterViewModel
    {

        /// <summary>
        /// 角色ID
        /// </summary>
        [Required(ErrorMessage = "必须选择{0}")]
        [Display(Name = "角色ID")]
        public int RoleID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        //[Remote("CanUsername", "User", HttpMethod = "Post", ErrorMessage = "用户名已存在")]
        //[StringLength(50, MinimumLength = 4, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        //[Required(ErrorMessage = "必须输入{0}")]
        //[Display(Name = "用户名")]
        //public string UserName { get; set; }
        [Remote("CanUsername", "User", HttpMethod = "Post", ErrorMessage = "注册手机号已存在")]
        [Required(ErrorMessage = "必填")]
        [StringLength(16, ErrorMessage = "长度1-16位")]
        [Display(Name = "手机号")]
        [RegularExpression(@"^1[3458][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        public string PhoneNumber { get; set; }


        /// <summary>
        /// 姓名【可做昵称、真实姓名等】
        /// </summary>
        [StringLength(20, ErrorMessage = "{0}必须少于{1}个字符")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 性别【0-女，1-男，2-保密】
        /// </summary>
        [Required(ErrorMessage = "必须选择{0}")]
        [Range(0, 2, ErrorMessage = "{0}范围{1}-{2}")]
        [Display(Name = "性别")]
        public int Sex { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "必须输入{0}")]
        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "必须输入{0}")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的Email地址")]
        [Remote("CanEmail", "User", HttpMethod = "Post", ErrorMessage = "Email已存在")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "必填")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "验证码不正确")]
        [Display(Name = "验证码")]
        public string VerificationCode { get; set; }

    }
}