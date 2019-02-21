using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickstartIdentityServer.Apis.ApiDTO;
using QuickstartIdentityServer.CommonDTO;
using QuickstartIdentityServer.DBManager;
using QuickstartIdentityServer.Filters;

namespace QuickstartIdentityServer.Apis
{
    [Authorize]
    [Route("base/api/[controller]/[action]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        PermissionConext pcontext;
        public AppController(PermissionConext pcontext)
        {
            this.pcontext = pcontext;
        }

        /// <summary>
        /// 查询系统
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>返回用户列表</returns>
        [HttpPost]
        public async Task<PageResult<AppResponseDTO>> Query([FromBody]AppRequestDTO input)
        {
            string likevalue = $"%{input.Name}%";
            var query = pcontext.App.Where(u => EF.Functions.Like(u.Name, likevalue))
                .Select(u => new AppResponseDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    CreateTime = u.CreateTime
                });
            var count = await query.CountAsync();
            var data = await query.Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize)
                 .ToListAsync();
            return new PageResult<AppResponseDTO>(count, data);
        }

        /// <summary>
        /// 创建系统
        /// </summary>
        /// <param name="input">用户信息</param>
        [HttpPost]
        public async Task Create([FromBody]AppDTO input)
        {
            if (await pcontext.App.AnyAsync(u => u.Code == input.Code))
            {
                throw new Exception("已有相同系统编号存在");
            }
            pcontext.App.Add(new AppEntity
            {
                Code = input.Code,
                Name = input.Name
            });
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 修改系统
        /// </summary>
        /// <param name="input">用户信息</param>
        [HttpPost]
        public async Task Update([FromBody]AppDTO input)
        {
            var app = await pcontext.App.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Code == input.Code);
            if (app == null) throw new Exception("系统信息不存在");
            app.Name = input.Name;
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除系统
        /// </summary>
        /// <param name="id">用户id</param>
        [HttpGet]
        public async Task Delete([FromQuery]int id)
        {
            var app = await pcontext.App.FirstOrDefaultAsync(u => u.Id == id);
            if (app == null) throw new Exception("系统信息不存在");
            app.IsDeleted = true;
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 新建模块
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddModule([FromBody]ModuleDTO input)
        {
            pcontext.Module.Add(new ModuleEntity
            {
                Name = input.Name,
                Code = input.Code
            });
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id">模块id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task DelModule([FromQuery]int id)
        {
            var del = new ModuleEntity
            {
                Id = id
            };
            pcontext.Entry(del).State = EntityState.Deleted;
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 新建权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddPermission([FromBody]PermissionDTO input)
        {
            pcontext.Permission.Add(new PermissionEntity
            {
                Name = input.Name,
                ModuleId = input.ModuleId,
                ControllerName = input.ControllerName.ToLower(),
                ActionName = input.ActionName.ToLower(),
                Url =input.Url.ToLower()
            });
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id">权限id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task DelPermission([FromQuery]int id)
        {
            var del = new PermissionEntity
            {
                Id = id
            };
            pcontext.Entry(del).State = EntityState.Deleted;
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 所有App模块权限信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AppDetaiDTO>> AppDeatil()
        {
            var apps = await (from a in pcontext.App
                       join b in pcontext.Module on a.Code equals b.Code
                       join c in pcontext.Permission on b.Id equals c.ModuleId
                       select new
                       {
                           a.Code,
                           a.Name,
                           Mid = b.Id,
                           MName = b.Name,
                           Pid = c.Id,
                           PName = c.Name
                       }).ToListAsync();
            return apps.GroupBy(a => new { a.Code, a.Name }).Select(kv => new AppDetaiDTO
            {
                Code = kv.Key.Code,
                Name = kv.Key.Name,
                Modules = kv.GroupBy(m=>new {m.Mid,m.MName }).Select(mkv => new ModuleDetailDTO
                {
                    Id = mkv.Key.Mid,
                    Name = mkv.Key.MName,
                    Permissions = mkv.Select(pkv=>new PermissionDetaiDTO {
                        Id = pkv.Pid,
                        Name = pkv.PName
                    }).ToList()
                }).ToList()
            }).ToList();
        }
    }
}