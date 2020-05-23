using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AQ_PGW.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        //private IRepository<Transactions> _transactionsRepository;

        private DbContext _context;
        private Dictionary<string, dynamic> _repositories;

        public UnitOfWork(AQ_PaymentGatewayDBContext context)
        {
            _context = context;
        }

        //public IRepository<Transactions> TransactionsRepository
        //{
        //    get
        //    {
        //        if (_transactionsRepository == null)
        //            _transactionsRepository = new Repository<Transactions>(_context);
        //        return _transactionsRepository;
        //    }
        //}
       

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Save()
        {
            bool saved = false;
            try
            {
                _context.SaveChanges();
                saved = true;
            }
            catch (Exception ex)
            {
                throw;

            }
            return saved;
        }

        public async Task<bool> SaveAsync()
        {
            bool saved = false;
            try
            {
                await _context.SaveChangesAsync();
                saved = true;
            }
            catch (Exception ex)
            {
                throw;

            }
            return saved;
        }

        public Repository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (Repository<TEntity>)_repositories[type];
            }

            _repositories.Add(type, new Repository<TEntity>(_context));


            return _repositories[type];
        }
    }
}
