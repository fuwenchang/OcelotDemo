using System.ComponentModel.DataAnnotations;

namespace Api_Auth
{
    public class LoginInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        /// <example>superAdmin</example>
        [Required(ErrorMessage = "用户名不能为空"), MinLength(5, ErrorMessage = "用户名不能少于5位字符")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        /// <example>123456</example>
        [Required(ErrorMessage = "密码不能为空"), MinLength(5, ErrorMessage = "密码不能少于5位字符")]
        public string Password { get; set; }
        public List<string> Socpes { get; set; }
    }
}
