using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuickstartIdentityServer.DBManager
{
    /// <summary>
    /// 用户对应角色
    /// </summary>
    public class UserRoleMap
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("RoleId")]
        public RoleEntity Role { get; set; }
    }
}
