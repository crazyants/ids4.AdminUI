using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickstartIdentityServer.DBManager.BaseData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.DBManager
{
    /// <summary>
    /// 角色对应权限
    /// </summary>
    public class RolePermissionMap : BaseKey<int>
    {
        /// <summary>
        /// 系统Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 权限Id
        /// </summary>
        public int PermissionId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RolePermissionMapConfiguration : IEntityTypeConfiguration<RolePermissionMap>
    {
        public void Configure(EntityTypeBuilder<RolePermissionMap> builder)
        {
            builder.HasIndex(u => u.Code);
            builder.HasIndex(u => u.PermissionId);
            builder.HasIndex(u => u.RoleId);
        }
    }
}
