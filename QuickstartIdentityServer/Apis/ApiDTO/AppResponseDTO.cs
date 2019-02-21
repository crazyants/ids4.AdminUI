using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.Apis.ApiDTO
{
    public class AppResponseDTO
    {
        public int Id { get;  set; }
        public string Name { get;  set; }
        public string Code { get;  set; }
        public DateTime CreateTime { get;  set; }
    }
}
