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
        /// 系统Id
        /// </summary>
        public int AppId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("AppId")]
        public AppEntity App { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ModuleConfiguration : IEntityTypeConfiguration<ModuleEntity>
    {
        public void Configure(EntityTypeBuilder<ModuleEntity> builder)
        {
            builder.Property(u => u.Name).HasMaxLength(30);
        }
    }
}
