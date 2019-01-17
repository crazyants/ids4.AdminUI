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
        /// 查询用户
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>返回用户列表</returns>
        [HttpPost]
        public async Task<List<UserResponseDTO>> Query([FromBody]UserRequestDTO input)
        {
            string likevalue = $"%{input.Name}%";
            var query = pcontext.User.Where(u => EF.Functions.Like(u.Name, likevalue))
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    CreateTime = u.CreateTime
                });
           return await query.Skip((input.PageIndex-1)* input.PageSize).Take(input.PageSize)
                .ToListAsync();
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
        /// 修改用户
        /// </summary>
        /// <param name="input">用户信息</param>
        [HttpPost]
        public async Task Update([FromBody]UserDTO input)
        {
            var user = await pcontext.User.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == input.Id);
            if(user==null) throw new Exception("用户信息不存在");
            user.Name = input.Name;
            user.Pwd = EncryptUtil.GetMd5(input.Pwd);
            user.IsDeleted = false;
            await pcontext.SaveChangesAsync();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户id</param>
        [HttpGet]
        public async Task Delete([FromQuery]int id)
        {
            var user = await pcontext.User.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) throw new Exception("用户信息不存在");
            user.IsDeleted = true;
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
    }
}