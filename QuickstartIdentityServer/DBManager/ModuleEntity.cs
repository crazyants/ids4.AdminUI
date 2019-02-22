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
    /// 模块
    /// </summary>
    public class ModuleEntity : BaseEnity<int>
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 模块编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 系统Code
        /// </summary>
        public string AppCode { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ModuleConfiguration : IEntityTypeConfiguration<ModuleEntity>
    {
        public void Configure(EntityTypeBuilder<ModuleEntity> builder)
        {
            builder.Property(u => u.Name).HasMaxLength(30);
            builder.HasIndex(u => u.AppCode);
            builder.Property(u => u.Code).HasMaxLength(8);
            builder.Property(u => u.AppCode).HasMaxLength(8);
        }
    }
}
