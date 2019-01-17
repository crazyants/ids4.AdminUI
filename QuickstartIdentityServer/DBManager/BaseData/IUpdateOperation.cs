using System;
using System.Collections.Generic;
using System.Text;

namespace QuickstartIdentityServer.DBManager.BaseData
{
    public interface IUpdateOperation : ITimeStamp
    {
        int UpdateId { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime UpdateTime { get; set; }
    }
}
