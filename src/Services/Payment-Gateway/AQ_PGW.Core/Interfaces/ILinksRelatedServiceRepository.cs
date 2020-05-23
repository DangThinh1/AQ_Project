using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQ_PGW.Core.Interfaces
{
    public interface ILinksRelatedServiceRepository
    {
        IQueryable<LinksRelated> GetLinksRelated(string Id);
        void InsertLinksRelated(LinksRelated links);
        bool UpdateLinksRelated(LinksRelated links);
    }
}
