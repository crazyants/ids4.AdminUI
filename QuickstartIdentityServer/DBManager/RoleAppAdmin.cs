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
    /// 角色对应系统管理员
    /// </summary>
    public class RoleAppAdmin : BaseKey<int>
    {
        /// <summary>
        /// 系统Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RoleAppConfiguration : IEntityTypeConfiguration<RoleAppAdmin>
    {
        public void Configure(EntityTypeBuilder<RoleAppAdmin> builder)
        {
            builder.HasIndex(u => u.Code);
            builder.HasIndex(u => u.RoleId);
        }
    }
}
