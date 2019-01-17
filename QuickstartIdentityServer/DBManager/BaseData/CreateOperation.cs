using System;
using System.Collections.Generic;
using System.Text;

namespace QuickstartIdentityServer.DBManager.BaseData
{
    public class CreateOperation : ICreateOperation
    {
        public int CreateId { get ; set ; }
        public DateTime CreateTime { get; set; }
        public long Ts { get; set; }
    }
}
