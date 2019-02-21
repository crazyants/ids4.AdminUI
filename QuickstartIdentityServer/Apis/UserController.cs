using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class UserController : ControllerBase
    {
        PermissionConext pcontext;
        public UserController(PermissionConext pcontext)
        {
            this.pcontext = pcontext;
        }
        /// <summary>
        /// 获取当前登入用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserNameDTO> Current()
        {
           var subid =  User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
           int.TryParse(subid, out int userid);
           var user = await pcontext.User.Where(u => u.Id == userid).Select(u=>new UserNameDTO {
               Name = u.Name
           }).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>返回用户列表</returns>
        [HttpPost]
        public async Task<PageResult<UserResponseDTO>> Query([FromBody]UserRequestDTO input)
        {
            string likevalue = $"%{input.Name}%";
            string likeaccount = $"%{input.Account}%";
            var query = pcontext.User.Where(u=>!u.IsSystemAdmin).Where(u => EF.Functions.Like(u.Name, likevalue)).Where(u => EF.Functions.Like(u.Account, likeaccount))
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    Account = u.Account,
                    Name = u.Name,
                    CreateTime = u.CreateTime
                });
            var count = await query.CountAsync();
            var data = await query.Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize)
                 .ToListAsync();
            return new PageResult<UserResponseDTO>(count, data);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="input">用户信息</param>
        [HttpPost]
        public async Task Create([FromBody]UserDTO input)
        {
            if(await pcontext.User.AnyAsync(u => u.Account == input.Account))
            {
                throw new Exception("已有相同账号存在");
            }
            pcontext.User.Add(new UserEntity
            {
                Account = input.Account,
                Name = input.Name,
                Pwd = EncryptUtil.GetMd5(input.Pwd)
            });
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 修改用户名称
        /// </summary>
        /// <param name="input">用户信息</param>
        [HttpPost]
        public async Task UpdateName([FromBody]UserNameDTO input)
        {
            var user = await pcontext.User.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == input.Id);
            if(user==null) throw new Exception("用户信息不存在");
            user.Name = input.Name;
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="input">用户信息</param>
        [HttpPost]
        public async Task UpdatePwd([FromBody]UserPwdDTO input)
        {
            var user = await pcontext.User.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == input.Id);
            if (user == null) throw new Exception("用户信息不存在");
            user.Pwd = EncryptUtil.GetMd5(input.Pwd);
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids">用户id集合</param>
        [HttpPost]
        public async Task Delete([FromBody]int[] ids)
        {
            var users = await pcontext.User.Where(u => ids.Contains(u.Id)).ToListAsync();
            users.ForEach(r => r.IsDeleted = true);
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="roleids">角色id集合</param>
        /// <returns></returns>
        [HttpPost]
        public async Task SetRole([FromQuery]int id,[FromBody] int[] roleids)
        {
            var maps = await pcontext.UserRoleMap.Where(map => map.UserId==id).ToListAsync();
            maps.ForEach(map =>
            {
                if (!roleids.Contains(map.RoleId)) pcontext.Entry(map).State = EntityState.Deleted;
            });
            foreach(var roleid in roleids)
            {
                if (!maps.Any(m => m.RoleId == roleid)) pcontext.UserRoleMap.Add(new UserRoleMap
                {
                    UserId = id,
                    RoleId = roleid
                });
            }
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="input">验证权限的信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> Validate([FromBody]ValidatePermissionDTO input)
        {
            bool isappadmin = await (from u in pcontext.User.Where(u => u.Id == input.UserId)
                            join urm in pcontext.UserRoleMap on u.Id equals urm.UserId
                            join ra in pcontext.RoleAppAdmin.Where(m => m.Code == input.Code) on urm.RoleId equals ra.RoleId select 1).AnyAsync();
            if (isappadmin) return true;
            bool haspermission = await (from u in pcontext.User.Where(u => u.Id == input.UserId)
                                  join urm in pcontext.UserRoleMap on u.Id equals urm.UserId
                                  join rmp in pcontext.RolePermissionMap.Where(m => m.Code == input.Code) on urm.RoleId equals rmp.RoleId
                                  join p in pcontext.Permission.Where(per=>per.ControllerName==input.Controller&&per.ActionName==input.Action) on rmp.PermissionId equals p.Id
                                        select 1).AnyAsync();
            return haspermission;
        }

        /// <summary>
        /// 角色已有权限
        /// </summary>
        /// <param name="roleid">角色id</param>
        /// <returns></returns>
        [HttpPost]
        public Task<int[]> Permission([FromQuery]int roleid)
        {
            return  pcontext.RolePermissionMap.Where(m => m.RoleId == roleid).Select(m => m.PermissionId).ToArrayAsync();
        }
    }
}