using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickstartIdentityServer.Apis.ApiDTO;
using QuickstartIdentityServer.DBManager;
using QuickstartIdentityServer.Filters;

namespace QuickstartIdentityServer.Apis
{
    //[Authorize]
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
        public async Task<List<RoleResponseDTO>> Query([FromBody]RoleRequestDTO input)
        {
            string likevalue = $"%{input.Name}%";
            var query = pcontext.Role.Where(u => EF.Functions.Like(u.Name, likevalue))
                .Select(u => new RoleResponseDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    CreateTime = u.CreateTime
                });
            return await query.Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize)
                 .ToListAsync();
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
        /// <param name="id">用户id</param>
        [HttpGet]
        public async Task Delete([FromQuery]int id)
        {
            var role = await pcontext.Role.FirstOrDefaultAsync(u => u.Id == id);
            if (role == null) throw new Exception("用户信息不存在");
            role.IsDeleted = true;
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
        /// 分配权限
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="pids">系统id集合</param>
        /// <returns></returns>
        [HttpPost]
        public async Task SetPermission([FromQuery]int id, [FromBody] int[] pids)
        {
            var maps = await pcontext.RolePermissionMap.Where(map => map.RoleId == id).ToListAsync();
            maps.ForEach(map =>
            {
                if (!pids.Contains(map.PermissionId)) pcontext.Entry(map).State = EntityState.Deleted;
            });
            var adds =  pids.Where(pid => !maps.Any(m => m.PermissionId == pid)).ToList();
            var dic = await (from pm in pcontext.Permission.Where(p => adds.Contains(p.Id))
             join m in pcontext.Module on pm.ModuleId equals m.Id
             select new { pm.Id, m.Code }
            ).ToDictionaryAsync(a => a.Id, a => a.Code);
            adds.ForEach(pid =>
            {
                pcontext.RolePermissionMap.Add(new RolePermissionMap
                {
                    RoleId = id,
                    PermissionId = pid,
                    Code = dic[pid]
                });
            });
            await pcontext.SaveChangesAsync();
        }
    }
}