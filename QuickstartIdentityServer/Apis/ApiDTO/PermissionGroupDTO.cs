using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    /// <summary>
    /// 
    /// </summary>
    public class AppGroupDTO
    {
        /// <summary>
        /// RoleAppMapId
        /// </summary>
        public int MapId { get; set; }
        /// <summary>
        /// 系统code
        /// </summary>
        public string AppCode { get; set; }
        /// <summary>
        /// 模块集合
        /// </summary>
        public List<ModuleGroupDTO> Modules { get; set; }
    }

    public class ModuleGroupDTO
    {
        /// <summary>
        /// RoleModuleMapId
        /// </summary>
        public int MapId { get; set; }
        /// <summary>
        /// 模块id
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// 权限集合
        /// </summary>
        public List<PermissionGroupDTO> Permission { get; set; }
    }

    public class PermissionGroupDTO
    {
        /// <summary>
        /// RolePermissionMapId
        /// </summary>
        public int MapId { get; set; }
        /// <summary>
        /// 权限id
        /// </summary>
        public int PermissionId { get; set; }
    }
}
