using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQ_PGW.Core.Interfaces
{
    public interface IRelatedTransactionDetailsServiceRepository
    {
        IQueryable<RelatedTransactionDetails> GetRelatedTransactionDetails(string Id);
        bool InsertRelatedTransactionDetails(RelatedTransactionDetails RelatedTransactionDetails);
        bool UpdateRelatedTransactionDetails(RelatedTransactionDetails RelatedTransactionDetails);
    }
}
