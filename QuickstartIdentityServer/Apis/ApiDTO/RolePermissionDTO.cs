using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    public class RolePermissionDTO
    {
       public int UseId { get; set; }
       public int SystemId { get; set; }
        /// <summary>
        /// 权限id
        /// </summary>
       public int[] PermisionIds { get; set; }
    }
}
