using System;
using System.Collections.Generic;
using System.Text;

namespace QuickstartIdentityServer.DBManager.BaseData
{
    /// <summary>
    ///可持久到数据库的领域模型的基类。
    /// </summary>
    [Serializable]
    public abstract class BaseKey<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TKey Id { get; set; }
    }

    /// <summary>
    /// 对象基类
    /// </summary>
    public class BaseEnity<TKey> : BaseKey<TKey>, ITimeStamp, ICreateOperation, IUpdateOperation, IDeleteOperation
    {
        /// <summary>
        /// 时间戳 datediff(second,'1970-01-01',GETUTCDATE())
        /// </summary>
        //[ConcurrencyCheck]
        public long Ts { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public int UpdateId { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
