using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AQ_PGW.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Repository<TEntity> Repository<TEntity>() where TEntity : class;
        bool Save();
        Task<bool> SaveAsync();
    }
}
