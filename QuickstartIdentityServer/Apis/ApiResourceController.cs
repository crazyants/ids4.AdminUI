using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickstartIdentityServer.Apis.ApiDTO;
using QuickstartIdentityServer.CommonDTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuickstartIdentityServer.Apis
{
    [Authorize]
    [Route("base/api/[controller]/[action]")]
    [ApiController]
    public class ApiResourceController : Controller
    {
        ConfigurationDbContext context;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:QuickstartIdentityServer.Apis.ClientsController"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public ApiResourceController(ConfigurationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Query the specified input.
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="input">Input.</param>
        [HttpPost]
        public async Task<PageResult<ApiResource>> Query([FromBody]AppRequestDTO input)
        {
            string likevalue = $"%{input.Name}%";
            var query = context.ApiResources.AsNoTracking().Where(u => EF.Functions.Like(u.Name, likevalue)|| EF.Functions.Like(u.DisplayName, likevalue));
            var count = await query.CountAsync();
            var clients = await query
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims)
                .Include(x => x.Properties)
                .OrderBy(o => o.Name).Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize)
                .ToListAsync();
            var model = clients.Select(c => c.ToModel()).ToList();

            return new PageResult<ApiResource>(count, model);
        }

        /// <summary>
        /// Create the specified input.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="input">Input.</param>
        [HttpPost]
        public async Task Create([FromBody]ApiResourceDTO input)
        {
            if (await context.ApiResources.AnyAsync(x => x.Name == input.Name))
                throw new Exception("已存在相同名称的资端");
            var add = (new ApiResource(input.Name, input.DisplayName)).ToEntity();
            context.ApiResources.Add(add);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Update the specified input.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="input">Input.</param>
        [HttpPost]
        public async Task Update([FromBody]ApiResourceDTO input)
        {
            var apiresource = await context.ApiResources.FirstOrDefaultAsync(x => x.Name == input.Name);
            if (apiresource == null) throw new Exception("不存在该资源");
            context.Entry(apiresource).State = EntityState.Deleted;
            var add = (new ApiResource(input.Name,input.DisplayName)).ToEntity();
            context.ApiResources.Add(add);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete the specified name.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="name">Name.</param>
        [HttpPost]
        public async Task Enabled(string name)
        {
            var apiresource = await context.ApiResources.FirstOrDefaultAsync(c => c.Name == name);
            apiresource.Enabled = !apiresource.Enabled;
            await context.SaveChangesAsync();
        }
    }
}
