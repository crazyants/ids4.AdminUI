using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickstartIdentityServer.Apis.ApiDTO;
using QuickstartIdentityServer.CommonDTO;
using QuickstartIdentityServer.IdsAuthorization;

namespace QuickstartIdentityServer.Apis
{
    /// <summary>
    /// Clients controller.
    /// </summary>
    [Authorize]
    [Route("base/api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        ConfigurationDbContext context;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuickstartIdentityServer.Apis.ClientsController"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public ClientController(ConfigurationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Query the specified input.
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="input">Input.</param>
        [HttpPost]
        public async Task<PageResult<Client>> Query([FromBody]AppRequestDTO input)
        {
            string likevalue = $"%{input.Name}%";
            var query = context.Clients.AsNoTracking().Where(u => EF.Functions.Like(u.ClientId, likevalue)|| EF.Functions.Like(u.ClientName, likevalue));
            var count = await query.CountAsync();
            var clients = await query
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties)
                .OrderBy(o => o.ClientId).Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize)
                .ToListAsync();
            var model = clients.Select(c=>c.ToModel()).ToList();

            return new PageResult<Client>(count,model);
        }

        /// <summary>
        /// Query the specified clientid.
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="clientid">Clientid.</param>
        [HttpGet]
        public async Task<Client> Query(string clientid)
        {
            var client = await context.Clients.AsNoTracking()
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties)
                .FirstOrDefaultAsync(x => x.ClientId == clientid);
            return client.ToModel();
        }

        /// <summary>
        /// Create the specified input.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="input">Input.</param>
        [HttpPost]
        public async Task Create([FromBody]Client input)
        {
            if (await context.Clients.AnyAsync(x => x.ClientId == input.ClientId))
                throw new Exception("已存在相同id的客户端");
            var add = input.ToEntity();
            context.Clients.Add(add);
            await  context.SaveChangesAsync();
        }

        /// <summary>
        /// Create the specified input.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="input">Input.</param>
        [HttpPost]
        [IdsAuth]
        public async Task Update([FromBody]Client input)
        {
            var client = await context.Clients.FirstOrDefaultAsync(x => x.ClientId == input.ClientId);
            if(client==null) throw new Exception("不存在此客户端");
            context.Entry(client).State = EntityState.Deleted;
            var add = input.ToEntity();
            context.Clients.Add(add);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete the specified clientId.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="clientId">Client identifier.</param>
        [HttpPost]
        [IdsAuth]
        public async Task Enabled(string clientId)
        {
            var client = await context.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            client.Enabled = !client.Enabled;
            await context.SaveChangesAsync();
        }
    }
}