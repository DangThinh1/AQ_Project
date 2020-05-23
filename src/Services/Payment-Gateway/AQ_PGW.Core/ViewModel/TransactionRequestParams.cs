using System;
using System.Collections.Generic;
using System.Text;

namespace AQ_PGW.Core.ViewModel
{
    public class TransactionRequestParams
    {
        public Guid ID { get; set; }
        public string OrderId { get; set; }
        public decimal? OrderAmount { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string BackUrl { get; set; }
        public string Currency { get; set; }
    }

    public class TransactionProcessParams
    {
        public Guid ID { get; set; }
        public string OrderId { get; set; }
        public decimal? OrderAmount { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public string BackUrl { get; set; }
        public string CancelUrl { get; set; }
        public string Currency { get; set; }
        public string PaymentCardToken { get; set; }
    }

    public class ItemsPayment
    {
        public string ItemName { get; set; }
        public decimal Amount { get; set; }
    }
    public class TransactionProcessParamsItems
    {
        public TransactionProcessParamsItems()
        {
            ItemsPayment = new List<ItemsPayment>();
        }
        public TransactionProcessParams TransactionProcess { get; set; }
        public List<ItemsPayment> ItemsPayment { get; set; }
    }
    public class InfoPayment
    {
        public string TranId { get; set; }
        public string PaymentId { get; set; }
        public string PayerId { get; set; }
        public string PaymentToken { get; set; }
    }
}
