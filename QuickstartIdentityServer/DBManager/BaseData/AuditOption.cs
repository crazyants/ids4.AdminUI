using System;
using System.Collections.Generic;
using System.Text;

namespace QuickstartIdentityServer.DBManager.BaseData
{
    public class AuditOption :  ICreateOperation, IDeleteOperation, IUpdateOperation
    {
        public int CreateId { get ; set ; }
        public DateTime CreateTime { get ; set ; }
        public int UpdateId { get ; set ; }
        public DateTime UpdateTime { get ; set ; }
        public long Ts { get; set; }
        public bool IsDeleted { get; set; }
    }
}
