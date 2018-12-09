using System.ComponentModel.DataAnnotations;

namespace TTG.Web.Areas.Member.Models
{
    public class ChangePWViewModel
    {
        [Required(ErrorMessage = "必须输入{0}")]
        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "原来的密码")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "必须输入{0}")]
        [StringLength(256, ErrorMessage = "{0}长度少于{1}个字符")]
        [DataType(DataType.Password)]
        [Display(Name = "修改后密码")]
        public string ConfirmPassword { get; set; }
    }
}