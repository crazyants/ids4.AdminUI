using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickstartIdentityServer.DBManager.BaseData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.DBManager
{
    /// <summary>
    /// 系统
    /// </summary>
    public class AppEntity: BaseEnity<int>
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public string Code { set; get; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppConfiguration : IEntityTypeConfiguration<AppEntity>
    {
        public void Configure(EntityTypeBuilder<AppEntity> builder)
        {
            builder.Property(u => u.Code).HasMaxLength(8);
            builder.Property(u => u.Name).HasMaxLength(50);
        }
    }
}
