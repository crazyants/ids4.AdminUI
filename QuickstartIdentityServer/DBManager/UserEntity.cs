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
    /// 用户
    /// </summary>
    public class UserEntity: BaseEnity<int>
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { set; get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(u=>u.Account).HasMaxLength(20);
            builder.Property(u=>u.Name).HasMaxLength(20);
            builder.Property(u=>u.Pwd).HasMaxLength(32);
        }
    }
}
