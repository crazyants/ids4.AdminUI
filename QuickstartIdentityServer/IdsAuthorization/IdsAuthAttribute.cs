using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.IdsAuthorization
{
    public class IdsAuthAttribute : AuthorizeAttribute
    {
        public const string policy = "OwenAuth";
        public IdsAuthAttribute() :base(policy: policy)
        {
        }
    }
}
