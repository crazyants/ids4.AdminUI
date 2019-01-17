using QuickstartIdentityServer.DBManager.BaseData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.DBManager
{
    /// <summary>
    /// 角色对应权限
    /// </summary>
    public class RolePermissionMap : BaseKey<int>
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public int PermissionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("PermissionId")]
        public PermissionEntity Permission { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("RoleId")]
        public RoleEntity Role { get; set; }
    }
}
