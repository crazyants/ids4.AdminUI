using System.Collections.Generic;

namespace QuickstartIdentityServer.CommonDTO
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T>  where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="data"></param>
        public PageResult(int count, IEnumerable<T> data)
        {
            TotalCount = count;
            List = data;
        }
        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<T> List { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }
    }
}
