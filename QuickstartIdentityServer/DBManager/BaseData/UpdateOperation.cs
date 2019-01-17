using System;
using System.Collections.Generic;
using System.Text;

namespace QuickstartIdentityServer.DBManager.BaseData
{
    public class UpdateOperation : IUpdateOperation
    {
        public int UpdateId { get ; set ; }
        public DateTime UpdateTime { get ; set ; }
        public long Ts { get; set; }
    }
}
