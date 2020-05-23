using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQ_PGW.Core.Interfaces
{
    public interface IPaymentLogsServiceRepository
    {
        IQueryable<PaymentLogs> GetByTransID(string Id);
        bool InsertPaymentLogs(PaymentLogs payment);
        bool UpdatePaymentLogs(PaymentLogs payment);
    }
}
