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
    public class RoleController : ControllerBase
    {
        PermissionConext pcontext;
        public RoleController(PermissionConext pcontext)
        {
            this.pcontext = pcontext;
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>返回用户列表</returns>
        [HttpPost]
        public async Task<PageResult<RoleResponseDTO>> Query([FromBody]RoleRequestDTO input)
        {
            string likevalue = $"%{input.Name}%";
            var query = pcontext.Role.Where(u => EF.Functions.Like(u.Name, likevalue))
                .Select(u => new RoleResponseDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    CreateTime = u.CreateTime
                });
            var count = await query.CountAsync();
            var data = await query.Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize)
                 .ToListAsync();
            return new PageResult<RoleResponseDTO>(count, data);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="input">角色信息</param>
        [HttpPost]
        public async Task Create([FromBody]RoleDTO input)
        {
            if (await pcontext.Role.AnyAsync(u => u.Name == input.Name))
            {
                throw new Exception("已有相同角色已存在");
            }
            pcontext.Role.Add(new RoleEntity
            {
                Name = input.Name
            });
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="input">角色信息</param>
        [HttpPost]
        public async Task Update([FromBody]RoleDTO input)
        {
            var role = await pcontext.Role.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == input.Id);
            if (role == null) throw new Exception("角色信息不存在");
            role.Name = input.Name;
            role.IsDeleted = false;
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids">角色id集合</param>
        [HttpPost]
        public async Task Delete([FromBody]int[] ids)
        {
            var roles = await pcontext.Role.Where(u => ids.Contains(u.Id)).ToListAsync();
            roles.ForEach(r => r.IsDeleted = true);
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 分配系统管理员
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="appcodes">系统id集合</param>
        /// <returns></returns>
        [HttpPost]
        public async Task SetAdmin([FromQuery]int id, [FromBody] string[] appcodes)
        {
            var maps = await pcontext.RoleAppAdmin.Where(map => map.RoleId == id).ToListAsync();
            maps.ForEach(map =>
            {
                if (!appcodes.Contains(map.Code)) pcontext.Entry(map).State = EntityState.Deleted;
            });
            foreach (var code in appcodes)
            {
                if (!maps.Any(m => m.Code == code)) pcontext.RoleAppAdmin.Add(new RoleAppAdmin
                {
                    RoleId = id,
                    Code = code
                });
            }
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AppGroupDTO>> GetPermission([FromQuery]int id)
        {
            var result = (await (from a in pcontext.RoleAppAdmin.Where(map => map.RoleId == id)
                         join m1 in pcontext.RoleModuleMap.Where(map => map.RoleId == id) on a.Code equals m1.AppCode into t1
                         from m in t1.DefaultIfEmpty()
                         select new
                         {
                             a.Id,
                             a.Code,
                             m = m==null?null:new ModuleGroupDTO { MapId = m.Id,ModuleId = m.ModuleId}
                         }).ToListAsync()).GroupBy(a=> new { a.Id, a.Code }).Select(a=>new AppGroupDTO {
                            MapId = a.Key.Id,
                            AppCode = a.Key.Code,
                             Modules = a.Where(b => b.m!=null).Select(b => b.m).ToList()
                         }).ToList();
            var pids = (await (from a in pcontext.RoleAppAdmin.Where(map => map.RoleId == id)
                         join p in pcontext.RolePermissionMap.Where(map => map.RoleId == id) on a.Code equals p.Code 
                         join m  in pcontext.Permission on p.PermissionId equals m.Id 
                         select new
                         {
                             mid = m.ModuleId,
                             p = new PermissionGroupDTO { MapId = p.Id,PermissionId = p.PermissionId}
                         }).ToListAsync()).GroupBy(a => a.mid).ToDictionary(a => a.Key, a => a.Select(b => b.p).ToList());
            result.ForEach(m => m.Modules.ForEach(i =>
            {
               if(pids.ContainsKey(i.ModuleId))  i.Permissions = pids[i.ModuleId];
            }));
            return result;
        }

        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="apps">系统权限集合</param>
        /// <returns></returns>
        [HttpPost]
        public async Task SetPermission([FromQuery]int id, [FromBody]List<AppGroupDTO> apps)
        {
            var olds = await GetPermission(id);
            olds.ForEach(app => //找出删除的AppMap
            {
                var nowapp = apps.FirstOrDefault(a => a.AppCode == app.AppCode);
                if (nowapp != null)
                {
                    app.Modules.ForEach(m =>//找出删除的ModlueMap
                    {
                        var nowm = nowapp.Modules.FirstOrDefault(a => a.ModuleId == m.ModuleId);
                        if (nowm != null)
                        {
                            m.Permissions.ForEach(p =>//找出删除的Permission
                            {
                                var nowp = nowm.Permissions.FirstOrDefault(a => a.PermissionId == p.PermissionId);
                                if (nowp == null) pcontext.Entry(new RolePermissionMap { Id = p.MapId }).State = EntityState.Deleted;
                            });
                        }
                        else
                        {
                            pcontext.Entry(new RoleModuleMap { Id = m.MapId }).State = EntityState.Deleted;
                            m.Permissions.ForEach(p =>
                            {
                                pcontext.Entry(new RolePermissionMap { Id = p.MapId }).State = EntityState.Deleted;
                            });
                        }
                    });
                }
                else {
                    pcontext.Entry(new RoleAppAdmin {Id = app.MapId  }).State = EntityState.Deleted;
                    app.Modules.ForEach(m =>
                    {
                        pcontext.Entry(new RoleModuleMap { Id = m.MapId}).State = EntityState.Deleted;
                        m.Permissions.ForEach(p =>
                        {
                            pcontext.Entry(new RolePermissionMap { Id = p.MapId }).State = EntityState.Deleted;
                        });
                    });
                } 
            });
            apps.ForEach(app => //找出新增的AppMap
            {
                var oldapp = olds.FirstOrDefault(a => a.AppCode == app.AppCode);
                if (oldapp != null)
                {
                    app.Modules.ForEach(m => //找出新增的ModlueMap
                    {
                        var oldm = oldapp.Modules.FirstOrDefault(a => a.ModuleId == m.ModuleId);
                        if (oldm != null)
                        {
                            m.Permissions.ForEach(p =>//找出新增的Permission
                            {
                                var oldp = oldm.Permissions.FirstOrDefault(a => a.PermissionId == p.PermissionId);
                                if (oldp == null) pcontext.RolePermissionMap.Add(new RolePermissionMap { PermissionId = p.PermissionId, RoleId = id ,Code = app.AppCode});
                            });
                        }
                        else
                        {
                            pcontext.RoleModuleMap.Add(new RoleModuleMap { ModuleId = m.ModuleId, RoleId = id, AppCode = app.AppCode });
                            m.Permissions.ForEach(p =>
                            {
                                pcontext.RolePermissionMap.Add(new RolePermissionMap { PermissionId = p.PermissionId, RoleId = id, Code = app.AppCode });
                            });
                        }
                    });
                }
                else
                {
                    pcontext.RoleAppAdmin.Add(new RoleAppAdmin { Code = app.AppCode ,RoleId = id });
                    app.Modules.ForEach(m =>
                    {
                        pcontext.RoleModuleMap.Add(new RoleModuleMap { ModuleId = m.ModuleId, RoleId = id, AppCode = app.AppCode });
                        m.Permissions.ForEach(p =>
                        {
                            pcontext.RolePermissionMap.Add(new RolePermissionMap { PermissionId = p.PermissionId, RoleId = id, Code = app.AppCode });
                        });
                    });
                }
            });
            await pcontext.SaveChangesAsync();
        }
    }
}