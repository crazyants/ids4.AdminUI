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
    /// 权限
    /// </summary>
    public class PermissionEntity : BaseEnity<int>
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// MVC  Action
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// api url
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.Property(u => u.Name).HasMaxLength(30);
            builder.Property(u => u.ControllerName).HasMaxLength(20);
            builder.Property(u => u.ActionName).HasMaxLength(20);
            builder.Property(u => u.Url).HasMaxLength(50);
        }
    }
}
