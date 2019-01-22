using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuickstartIdentityServer.Apis
{
    [Route("base/api/[controller]/[action]")]
    [ApiController]
    public class IdsController : ControllerBase
    {
        PersistedGrantDbContext persistedGrantDbContext;
        public IdsController(PersistedGrantDbContext persistedGrantDbContext)
        {
            this.persistedGrantDbContext = persistedGrantDbContext;
        }
        [HttpGet]
        public Task AdddPersistedGrant([FromQuery]string subid, [FromQuery]string clientid)
        {
            persistedGrantDbContext.PersistedGrants.Add(new IdentityServer4.EntityFramework.Entities.PersistedGrant
            {
                Key = Guid.NewGuid().ToString(),
                Type = "sub",
                SubjectId = subid,
                ClientId = clientid,
                CreationTime = DateTime.Now,
                Expiration = DateTime.Now,
                Data ="haha"
            });
            return persistedGrantDbContext.SaveChangesAsync();
        }
    }
}