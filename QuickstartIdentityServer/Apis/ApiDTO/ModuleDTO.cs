using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    /// <summary>
    /// 
    /// </summary>
    public class ModuleDTO
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
        /// 模块Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 系统Code
        /// </summary>
        public string AppCode { get; set; }
    }
}
