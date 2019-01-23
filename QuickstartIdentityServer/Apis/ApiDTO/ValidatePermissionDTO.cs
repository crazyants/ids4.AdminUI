using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    public class ValidatePermissionDTO
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 系统code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// 操作
        /// </summary>
        public string Action { get; set; }
    }
}
