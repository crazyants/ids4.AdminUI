using System;
using System.Collections.Generic;
using System.Text;

namespace QuickstartIdentityServer.DBManager.BaseData
{
    public class DeleteOperation : IDeleteOperation
    {
        public bool IsDeleted { get; set; }
        public long Ts { get; set; }
    }
}
