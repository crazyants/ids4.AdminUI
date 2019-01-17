using System;
using System.Collections.Generic;
using System.Text;

namespace QuickstartIdentityServer.DBManager.BaseData
{
	/// <summary>
    /// 功能描述    ：时间戳  
    /// 创 建 者    ：shenl
    /// 创建日期    ：2017-11-15 13:11:56 
    /// 最后修改者  ：shenl
    /// 最后修改日期：2017-11-15 13:11:56 
    /// </summary>
    public interface ITimeStamp
    {
        /// <summary>
        /// 时间戳 datediff(second,'1970-01-01',GETUTCDATE())
        /// </summary>
        long Ts { get; set; }
    }
}
