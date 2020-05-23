using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using AQ_PGW.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using IUnitOfWork = AQ_PGW.Infrastructure.Repositories.IUnitOfWork;
using System.Linq.Dynamic.Core;

namespace AQ_PGW.Infrastructure.Servives
{
    public class TransactionsServiceRepository : ITransactionsServiceRepository
    {
        private IUnitOfWork _unitOfWork;

        public TransactionsServiceRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public PageModel<Transactions> GetTransactions(int? page, string sortBy, string cusId = null)
        {
            var getTrans = this._unitOfWork.Repository<Transactions>().Where(x => x.StripeCustomerId == cusId);
            if (!string.IsNullOrEmpty(sortBy))
            {
                getTrans = getTrans.OrderBy(sortBy);                
            }
            var ListTrans = getTrans.ToList();
            var pager = new Pager(ListTrans.Count(), page);

            var ViewTrans = new PageModel<Transactions>()
            {
                Items = ListTrans.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return ViewTrans;
        }
        public Transactions GetTransactionById(string id)
        {
            var getTran = this._unitOfWork.Repository<Transactions>().FirstOrDefault(x => x.ID.ToString() == id);

            return getTran;
        }

        public Transactions InsertTransactions(Transactions model)
        {
            _unitOfWork.Repository<Transactions>().Add(model);
            _unitOfWork.Save();

            return model;
        }
        public Transactions UpdateTransactions(Transactions model)
        {
            _unitOfWork.Repository<Transactions>().Update(model);
            _unitOfWork.Save();

            return model;
        }

        public IQueryable<TransactionItems> GetTransactionItems(Guid transactionId, string status = null)
        {
            var result = _unitOfWork.Repository<TransactionItems>().Where(x => x.TransactionId == transactionId);
            if (string.IsNullOrEmpty(status) == false)
            {
                result = result.Where(x => x.Status == status);
            }
            return result;
        }

        public IQueryable<TransactionItems> GetTransactionItemsToday()
        {
            DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var result = _unitOfWork.Repository<TransactionItems>().Where(x => x.PayDate == today);
            return result;
        }

        public TransactionItems GetTransactionItem(Guid id)
        {
            var result = _unitOfWork.Repository<TransactionItems>().FirstOrDefault(x => x.ID == id);
            return result;
        }

        public bool UpdateTransactionItem(TransactionItems model)
        {
            _unitOfWork.Repository<TransactionItems>().Update(model);
            var flag = _unitOfWork.Save();
            return flag;
        }

        public bool InsertTransactionItems(List<TransactionItems> items)
        {
            _unitOfWork.Repository<TransactionItems>().AddRange(items);
            return _unitOfWork.Save();
        }

        public IQueryable<Transactions> GetSearchTransaction(DateTime Today, DateTime FromDate)
        {
            var GetTrans = _unitOfWork.Repository<Transactions>().Where(x => x.CreatedDate >= Today && x.CreatedDate <= FromDate);
            return GetTrans;
        }

        public PageModel<Transactions> SearchAllTransaction(int? page, DateTime Today, DateTime FromDate, string sortBy, string cusID = null)
        {
            var list = GetALL(Today, FromDate, cusID);
            if (!string.IsNullOrEmpty(sortBy))
            {
                list = list.OrderBy(sortBy);
            }
            var getTrans = list.ToList();
            var ViewTrans = Pagination(page, getTrans);

            return ViewTrans;
        }

        public PageModel<Transactions> SearchUnPaidTransaction(int? page, DateTime Today, DateTime FromDate,string sortBy, string cusID = null)
        {
            var list = GetALL(Today, FromDate, cusID).Where(x => x.Status == null);
            if (!string.IsNullOrEmpty(sortBy))
            {
                list = list.OrderBy(sortBy);
            }
            var getTrans = list.ToList();
            var ViewTrans = Pagination(page, getTrans);

            return ViewTrans;
        }

        public PageModel<Transactions> SearchPedingTransaction(int? page, DateTime Today, DateTime FromDate, string sortBy, string cusID = null)
        {
            var list = GetALL(Today, FromDate, cusID).Where(x => x.Status.ToLower() == "pending");
            if (!string.IsNullOrEmpty(sortBy))
            {
                list = list.OrderBy(sortBy);
            }
            var getTrans = list.ToList();
            var ViewTrans = Pagination(page, getTrans);

            return ViewTrans;
        }

        public PageModel<Transactions> SearchPaidTransaction(int? page, DateTime Today, DateTime FromDate, string sortBy, string cusID = null)
        {
            var list = GetALL(Today, FromDate, cusID).Where(x => x.Status.ToLower() == "succeeded");
            if (!string.IsNullOrEmpty(sortBy))
            {
                list = list.OrderBy(sortBy);
            }
            var getTrans = list.ToList();
            var ViewTrans = Pagination(page, getTrans);

            return ViewTrans;
        }

        public PageModel<Transactions> SearchOrderCompleteTransaction(int? page, DateTime Today, DateTime FromDate, string sortBy, string cusID = null)
        {
            var list = GetALL(Today, FromDate, cusID).Where(x => x.Status.ToLower() == "completed");
            if (!string.IsNullOrEmpty(sortBy))
            {
                list = list.OrderBy(sortBy);
            }
            var getTrans = list.ToList();
            var ViewTrans = Pagination(page, getTrans);

            return ViewTrans;
        }
        public IQueryable<Transactions> GetALL(DateTime Today, DateTime FromDate, string cusID = null)
        {
            var getTrans = this._unitOfWork.Repository<Transactions>().Where(x => x.StripeCustomerId == cusID && x.CreatedDate >= Today && x.CreatedDate <= FromDate);
            return getTrans;
        }

        public PageModel<T> Pagination<T>(int? page, List<T> trans)
        {
            var pager = new Pager(trans.Count(), page);

            var ViewTrans = new PageModel<T>()
            {
                Items = trans.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return ViewTrans;
        }

        public SearchTransactionsModel SearchAllTransaction(DateTime Today, DateTime FromDate, string cusID = null)
        {
            var getTrans = GetALL(Today, FromDate, cusID);

            var ViewTransAll = Pagination(1, getTrans.ToList());

            var ViewTransUnPaid = Pagination(1, getTrans.Where(x => x.Status == null).ToList());

            var ViewTransPeding = Pagination(1, getTrans.Where(x => x.Status == "pending").ToList());

            var ViewTransPaid = Pagination(1, getTrans.Where(x => x.Status == "succeeded").ToList());

            var ViewTransOrderComplete = Pagination(1, getTrans.Where(x => x.Status == "completed").ToList());

            var search = new SearchTransactionsModel()
            {
                ViewAll = ViewTransAll,
                ViewNoPaid = ViewTransUnPaid,
                ViewCompleted = ViewTransOrderComplete,
                ViewPaid = ViewTransPaid,
                ViewPending = ViewTransPeding,
            };
            return search;
        }
    }
}
