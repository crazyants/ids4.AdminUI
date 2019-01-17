using QuickstartIdentityServer.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    public class RoleRequestDTO : PageRequest
    {
        public string Name { get;  set; }
    }
}
