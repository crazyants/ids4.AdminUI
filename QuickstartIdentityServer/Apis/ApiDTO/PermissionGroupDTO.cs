using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    /// <summary>
    /// 
    /// </summary>
    public class PermissionGroupDTO
    {
        /// <summary>
        /// 系统code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 模块集合
        /// </summary>
        public List<ModuleGroupDTO> Modules { get; set; }
    }

    public class ModuleGroupDTO
    {
        /// <summary>
        /// 模块id
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// 权限集合
        /// </summary>
        public int[] PermissionIds { get; set; }
    }
}
