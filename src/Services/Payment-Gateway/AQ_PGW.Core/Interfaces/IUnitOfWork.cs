using System;
using System.Collections.Generic;
using System.Text;

namespace AQ_PGW.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        bool Save();
    }
}
