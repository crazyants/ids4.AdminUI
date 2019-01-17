using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    public class PermissionDTO
    {
        /// <summary>
        /// 权限 id
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 模块Id
        /// </summary>
        public int ModuleId { get; set; }
    }
}
