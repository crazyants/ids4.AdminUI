using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickstartIdentityServer.DBManager.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.DBManager
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleEntity : BaseEnity<int>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.Property(u => u.Name).HasMaxLength(50);
        }
    }
}
