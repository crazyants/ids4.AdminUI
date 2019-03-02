using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickstartIdentityServer.IdsAuthorization;

namespace QuickstartIdentityServer.Apis
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        ConfigurationDbContext context;
        public ClientsController(ConfigurationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public Task<List<Client>> List()
        {
           return context.Clients.Select(c=>new Client
            {
                ClientId = c.ClientId
            }).ToListAsync();
        }
    }
}