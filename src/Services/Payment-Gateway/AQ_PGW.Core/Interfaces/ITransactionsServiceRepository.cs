using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQ_PGW.Core.Interfaces
{
    public interface ITransactionsServiceRepository
    {
        Transactions InsertTransactions(Transactions model);
        Transactions UpdateTransactions(Transactions model);
        Transactions GetTransactionById(string id);
        PageModel<Transactions> GetTransactions(int? page, string sortBy, string cusID = null);

        IQueryable<TransactionItems> GetTransactionItems(Guid transactionId, string status = null);
        IQueryable<Transactions> GetSearchTransaction(DateTime Today, DateTime FromDate);
        SearchTransactionsModel SearchAllTransaction(DateTime Today, DateTime FromDate, string cusID = null);

        PageModel<Transactions> SearchAllTransaction(int? page, DateTime Today, DateTime FromDate, string sortBy, string cusID = null);
        PageModel<Transactions> SearchUnPaidTransaction(int? page, DateTime Today, DateTime FromDate, string sortBy, string cusID = null);
        PageModel<Transactions> SearchPedingTransaction(int? page, DateTime Today, DateTime FromDate, string sortBy, string cusID = null);
        PageModel<Transactions> SearchPaidTransaction(int? page, DateTime Today, DateTime FromDate, string sortBy, string cusID = null);
        PageModel<Transactions> SearchOrderCompleteTransaction(int? page, DateTime Today, DateTime FromDate, string sortBy, string cusID = null);
        IQueryable<TransactionItems> GetTransactionItemsToday();
        TransactionItems GetTransactionItem(Guid id);
        bool InsertTransactionItems(List<TransactionItems> items);
        bool UpdateTransactionItem(TransactionItems model);
    }
}
