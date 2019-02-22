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
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// MVC  Action
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// api url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 模块Id
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }
}
