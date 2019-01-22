using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuickstartIdentityServer.DBManager;
using QuickstartIdentityServer.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.IdsAuthorization
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        PermissionConext db;
        ILogger<PasswordValidator> logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        public PasswordValidator(PermissionConext db, ILogger<PasswordValidator> logger)
        {
            this.db = db;
            this.logger = logger;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(context.UserName) || string.IsNullOrWhiteSpace(context.Password))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
                return;
            }
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            //var code = context.Request.Raw[validatecode];
            //context.Result = new GrantValidationResult(
            // subject: context.UserName,
            // authenticationMethod: "custom",
            // claims: GetUserClaims());
            try
            {
                var pwd = EncryptUtil.GetMd5(context.Password);
                var user = await db.User.FirstOrDefaultAsync(u => u.Account == context.UserName && u.Pwd == pwd);
                if (user == null)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "用户名或密码错误");
                    return;
                }
                context.Result = new GrantValidationResult(subject: user.Id.ToString(),
                    authenticationMethod: ClaimsIdentityName.Custom,
                    claims: new Claim[] { new Claim(JwtClaimTypes.Role, user.IsSystemAdmin ? ClaimsIdentityName.Admin : ClaimsIdentityName.Custom) });
            }
            catch (Exception e)
            {
                this.logger.LogError("登入异常:", e);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, $"登入异常：{e.Message}");
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class ClaimsIdentityName
    {
        /// <summary>
        /// 
        /// </summary>
        public const string Admin = "admin";
        /// <summary>
        /// 
        /// </summary>
        public const string Custom = "custom";
    }
}
