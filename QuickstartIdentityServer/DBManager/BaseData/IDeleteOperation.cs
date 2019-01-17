using System;
using System.Collections.Generic;
using System.Text;

namespace QuickstartIdentityServer.DBManager.BaseData
{
    public interface IDeleteOperation : ITimeStamp
    {
        bool IsDeleted { get; set; }
    }
}
