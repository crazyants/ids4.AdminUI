using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using QuickstartIdentityServer.Apis.ApiDTO;
using QuickstartIdentityServer.DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.IdsAuthorization
{
    public class IdsAuthPermissionHandler : AuthorizationHandler<IdsAuthPermissionHandler> ,IAuthorizationRequirement
    {
        private readonly string SystemTag;
        private const string userinfo = "userinfo";
        public IdsAuthPermissionHandler(string sysTag)
        {
            this.SystemTag = sysTag;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IdsAuthPermissionHandler requirement)
        {
            var subid = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentityName.Role).Value;
            if(role== ClaimsIdentityName.Admin) context.Succeed(requirement);
            else
            {
                var filterContext = context.Resource as AuthorizationFilterContext;
                var controllerContext = (filterContext.ActionDescriptor as ControllerActionDescriptor);

                int.TryParse(subid, out int userid);
                ValidatePermissionDTO param = new ValidatePermissionDTO {
                    UserId = userid,
                    Code = SystemTag,
                    Controller = controllerContext.ControllerName.ToLower(),
                    Action = controllerContext.ActionName.ToLower()
                };
                var db = filterContext.HttpContext.RequestServices.GetService(typeof(PermissionConext)) as PermissionConext;
                var isvalidate = await Validate(db,param);
                if(isvalidate) context.Succeed(requirement);
            }
        }

        private async Task<bool> Validate(PermissionConext pcontext, ValidatePermissionDTO input)
        {
            bool isappadmin = await (from u in pcontext.User.Where(u => u.Id == input.UserId)
                                     join urm in pcontext.UserRoleMap on u.Id equals urm.UserId
                                     join ra in pcontext.RoleAppAdmin.Where(m => m.Code == input.Code) on urm.RoleId equals ra.RoleId
                                     select 1).AnyAsync();
            if (isappadmin) return true;
            bool haspermission = await (from u in pcontext.User.Where(u => u.Id == input.UserId)
                                        join urm in pcontext.UserRoleMap on u.Id equals urm.UserId
                                        join rmp in pcontext.RolePermissionMap.Where(m => m.Code == input.Code) on urm.RoleId equals rmp.RoleId
                                        join p in pcontext.Permission.Where(per => per.ControllerName == input.Controller && per.ActionName == input.Action) on rmp.PermissionId equals p.Id
                                        select 1).AnyAsync();
            return haspermission;
        }

    }
}
