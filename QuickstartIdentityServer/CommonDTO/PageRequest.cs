using System;
using System.ComponentModel.DataAnnotations;

namespace QuickstartIdentityServer.CommonDTO
{
    /// <summary>
    /// 分页查询参数
    /// </summary>
    public class PageRequest
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Range(1,Int32.MaxValue,ErrorMessage ="页码应该大于0")]
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 每页数量
        /// </summary>
        [Range(1, Int32.MaxValue, ErrorMessage = "每页数量应该大于0")]
        public int PageSize { get; set; } = 10;
    }
}
