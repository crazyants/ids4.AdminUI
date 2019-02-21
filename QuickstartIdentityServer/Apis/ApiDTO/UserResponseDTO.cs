using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    public class UserResponseDTO 
    {
        public int Id { set; get; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
