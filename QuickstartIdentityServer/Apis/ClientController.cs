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
        /// Create the specified input.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="input">Input.</param>
        [HttpPost]
        public async Task Create([FromBody]Client input)
        {
            var add = input.ToEntity();
            context.Clients.Add(add);
            await  context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete the specified clientId.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="clientId">Client identifier.</param>
        [HttpPost]
        public async Task Delete(string clientId)
        {
            var client = context.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            context.Entry(client).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }
    }
}