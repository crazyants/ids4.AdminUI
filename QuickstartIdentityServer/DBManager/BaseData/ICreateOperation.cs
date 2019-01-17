using System;
using System.Collections.Generic;
using System.Text;

namespace QuickstartIdentityServer.DBManager.BaseData
{
	
    public interface ICreateOperation: ITimeStamp
    {
        int CreateId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}
