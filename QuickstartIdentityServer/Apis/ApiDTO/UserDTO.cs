using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 账号
        /// </summary>
        [StringLength(20,ErrorMessage = "账号最大长度为20位")]
        [Required(ErrorMessage = "账号不允许为null")]
        public string Account { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(20,ErrorMessage = "用户名最大长度为20位")]
        [Required(ErrorMessage = "用户名不允许为null")]
        public string Name { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(32,ErrorMessage ="密码最大长度为32位")]
        [Required(ErrorMessage ="密码不允许为null")]
        public string Pwd { set; get; }
    }
    public class UserNameDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(20, ErrorMessage = "用户名最大长度为20位")]
        [Required(ErrorMessage = "用户名不允许为null")]
        public string Name { set; get; }
    }
    public class UserPwdDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(32, ErrorMessage = "密码最大长度为32位")]
        [Required(ErrorMessage = "密码不允许为null")]
        public string Pwd { set; get; }
    }
}
