using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    /// <summary>
    /// 系统信息
    /// </summary>
    public class AppDTO
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 系统Id
        /// </summary>
        public int Id { get; set; }
    }
    /// <summary>
    /// 系统详细信息
    /// </summary>

    public class AppDetaiDTO:AppDTO
    {
        /// <summary>
        /// 模块
        /// </summary>
        public List<ModuleDetaiDTO> Modules { get; set; }
    }
    /// <summary>
    /// 模块详细信息
    /// </summary>
    public class ModuleDetaiDTO
    {
        /// <summary>
        /// 模块 id
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 权限
        /// </summary>
        public List<PermissionDetaiDTO> Permissions { get; set; }
    }
    /// <summary>
    /// 权限详细信息
    /// </summary>
    public class PermissionDetaiDTO
    {
        /// <summary>
        /// 权限 id
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { set; get; }
    }
}
