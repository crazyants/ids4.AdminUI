using QuickstartIdentityServer.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRequestDTO: PageRequest
    {
        /// <summary>
        /// 用户账号(支持模糊匹配)
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 用户名(支持模糊匹配)
        /// </summary>
        public string Name { get; set; }
    }
}
