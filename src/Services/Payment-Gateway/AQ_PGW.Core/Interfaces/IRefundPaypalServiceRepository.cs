using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQ_PGW.Core.Interfaces
{
    public interface IRefundPaypalServiceRepository
    {
        IQueryable<RefundPaypal> GetRefundPaypal(string Id);
        void InsertRefundPaypal(RefundPaypal links);
        bool UpdateRefundPaypal(RefundPaypal links);
    }
}
