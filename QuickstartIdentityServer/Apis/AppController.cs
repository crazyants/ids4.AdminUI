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
                    Code = u.Code,
                    CreateTime = u.CreateTime
                });
            var count = await query.CountAsync();
            var data = await query.OrderBy(o => o.Name).Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize)
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
            var app = await pcontext.App.FirstOrDefaultAsync(u => u.Code == input.Code);
            if (app == null) throw new Exception("系统信息不存在");
            app.Name = input.Name;
            await pcontext.SaveChangesAsync();
        }
       
        /// <summary>
        /// 删除系统
        /// </summary>
        /// <param name="codes">系统编码</param>
        [HttpPost]
        public async Task Delete([FromBody]string[] codes)
        {
            var apps = await pcontext.App.Where(u => codes.Contains(u.Code)).ToListAsync();
            apps.ForEach(r => r.IsDeleted = true);
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 查询模块
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<ModuleDetailDTO>> QueryModule([FromBody]ModuleRequestDTO input)
        {
            var tempquery = pcontext.Module.AsNoTracking();
            if (!string.IsNullOrEmpty(input.Name))
            {
                string likevalue = $"%{input.Name}%";
                tempquery = tempquery.Where(u => EF.Functions.Like(u.Name, likevalue));
            }
            if (!string.IsNullOrEmpty(input.Code)) tempquery = tempquery.Where(p => p.AppCode == input.Code);
            var query = from u in tempquery
                        join a in pcontext.App on u.AppCode equals a.Code
                        select new ModuleDetailDTO
                        {
                            Id = u.Id,
                            Name = u.Name,
                            Code = u.Code,
                            AppName = a.Name,
                            CreateTime = u.CreateTime
                        };
            var count = await query.CountAsync();
            query = query.OrderBy(o=>o.AppName).ThenBy(o=>o.Name).Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize);
            var resultquery = from m in query
                              join pt in pcontext.Permission on m.Id equals pt.ModuleId into l1
                              from p in l1.DefaultIfEmpty()
                              select new
                              {
                                  mid = m.Id,
                                  p = p == null ? null : new PermissionDetaiDTO
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      ControllerName = p.ControllerName,
                                      ActionName = p.ActionName,
                                      Url = p.Url
                                  }
                              };
            var temp1 = await query.ToListAsync();
            var temp2 = await resultquery.ToListAsync();
            var temp3 = temp2.GroupBy(t => t.mid).Select(t => new
            {
                Id = t.Key,
                children = t.Where(kv => kv.p != null).Select(kv => kv.p).ToList()
            }).ToDictionary(t => t.Id, t => t.children);
            temp1.ForEach(t =>
            {
                t.Permissions = temp3[t.Id];
            });
            return new PageResult<ModuleDetailDTO>(count, temp1);
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
                Code = input.Code,
                AppCode = input.AppCode
            });
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 修改模块
        /// </summary>
        /// <param name="input">模块信息</param>
        [HttpPost]
        public async Task UpdateModule([FromBody]ModuleUpdateDTO input)
        {
            var module = await pcontext.Module.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (module == null) throw new Exception("模块信息不存在");
            module.Name = input.Name;
            module.Code = input.Code;
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="ids">模块id数组</param>
        /// <returns></returns>
        [HttpPost]
        public async Task DelModule([FromBody]int[] ids)
        {
            var modules = await pcontext.Module.Where(u => ids.Contains(u.Id)).ToListAsync();
            modules.ForEach(r => r.IsDeleted = true);
            await pcontext.SaveChangesAsync();;
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
                ControllerName = input.ControllerName,
                ActionName = input.ActionName,
                Url =input.Url,
                Order = input.Order
            });
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task UpdatePermission([FromBody]PermissionDTO input)
        {
            var p = await pcontext.Permission.FirstOrDefaultAsync(u => u.Id == input.Id);
            if (p == null) throw new Exception("权限信息不存在");
            p.Name = input.Name;
            p.ControllerName = input.ControllerName;
            p.ActionName = input.ActionName;
            p.Url = input.Url;
            p.Order = input.Order;
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除权限
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
                       join b in pcontext.Module on a.Code equals b.AppCode
                       join c in pcontext.Permission on b.Id equals c.ModuleId
                       select new
                       {
                           a.Code,
                           a.Name,
                           Id = b.Id,
                           Mid = b.Code,
                           MName = b.Name,
                           Pid = c.Id,
                           PName = c.Name
                       }).ToListAsync();
            return apps.GroupBy(a => new { a.Code, a.Name }).Select(kv => new AppDetaiDTO
            {
                Code = kv.Key.Code,
                Name = kv.Key.Name,
                Modules = kv.GroupBy(m=>new {m.Id, m.Mid,m.MName }).Select(mkv => new ModuleDetailDTO
                {
                    Id = mkv.Key.Id,
                    Code = mkv.Key.Mid,
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