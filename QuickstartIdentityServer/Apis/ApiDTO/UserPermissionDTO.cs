using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    public class UserPermissionDTO
    {
        /// <summary>
        /// 权限中心管理员
        /// </summary>
        public bool Admin { get; set; }
        /// <summary>
        /// 系统app
        /// </summary>
        public List<App> Apps { get; set; }
    }
    public class App
    {
        /// <summary>
        /// App管理员
        /// </summary>
        public bool Admin { get; set; }
        /// <summary>
        /// 权限 Control Action
        /// </summary>
        public Dictionary<string,string[]> Pms { get; set; }
    }
}
