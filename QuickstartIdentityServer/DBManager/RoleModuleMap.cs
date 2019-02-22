using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickstartIdentityServer.DBManager.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.DBManager
{
    public class RoleModuleMap : BaseKey<int>
    {
        /// <summary>
        /// 系统Code
        /// </summary>
        public string AppCode { get; set; }
        /// <summary>
        /// 模块Id
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RoleModuleMapConfiguration : IEntityTypeConfiguration<RoleModuleMap>
    {
        public void Configure(EntityTypeBuilder<RoleModuleMap> builder)
        {
            builder.HasIndex(u => u.AppCode);
            builder.HasIndex(u => u.RoleId);
            builder.HasIndex(u => u.ModuleId);
            builder.Property(u => u.AppCode).HasMaxLength(8);
        }
    }
}
