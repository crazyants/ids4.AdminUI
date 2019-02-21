using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [StringLength(50, ErrorMessage = "系统名最大长度为50位")]
        [Required(ErrorMessage = "系统名不允许为null")]
        public string Name { get; set; }
        /// <summary>
        /// 系统编号
        /// </summary>
        [StringLength(8, ErrorMessage = "系统编号最大长度为8位")]
        [Required(ErrorMessage = "系统编号不允许为null")]
        public string Code { set; get; }
    }
    /// <summary>
    /// 系统详细信息
    /// </summary>

    public class AppDetaiDTO:AppDTO
    {
        /// <summary>
        /// 模块
        /// </summary>
        public List<ModuleDetailDTO> Modules { get; set; }
    }
    /// <summary>
    /// 模块详细信息
    /// </summary>
    public class ModuleDetailDTO
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
