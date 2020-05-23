using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQ_PGW.Core.Interfaces
{
    public interface IRefundStripeServiceRepository
    {
        IQueryable<RefundStripe> GetRefundStripe(string transId);
        void InsertRefundStripe(RefundStripe RefundStripe);
        bool UpdateRefundStripe(RefundStripe RefundStripe);
    }
}
