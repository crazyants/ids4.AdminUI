using QuickstartIdentityServer.DBManager.BaseData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.DBManager
{
    /// <summary>
    /// 角色对应系统管理员
    /// </summary>
    public class RoleAppAdmin : BaseKey<int>
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public int AppId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("AppId")]
        public AppEntity App { get; set; }
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
