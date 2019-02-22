using QuickstartIdentityServer.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    public class AppRequestDTO : PageRequest
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string Name { get; set; }
    }

    public class ModuleRequestDTO : PageRequest
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string Name { get; set; }
    }
}
