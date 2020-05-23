using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AQ_PGW.Common.Important;
using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using AQ_PGW.Core.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using PayPal.Core;
using PaypalExpressCheckout.BusinessLogic.Interfaces;
using Stripe;
using static AQ_PGW.Common.DTO;

namespace AQ_PGW.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        ITransactionsServiceRepository _TransactionsService;
        IPaymentLogsServiceRepository _PaymentLogsService;
        IRelatedTransactionDetailsServiceRepository _IRelatedTransaction;
        ILinksRelatedServiceRepository linksRelatedService;
        IRefundPaypalServiceRepository RefundserviceRepository;
        IRefundStripeServiceRepository refundStripeService;
        private IPaypalServices _PaypalServices;

        private readonly IMapper _mapper;
        public TransactionController(ITransactionsServiceRepository TransactionsService,
            IPaypalServices Paypal,
            IMapper mapper,
            IPaymentLogsServiceRepository Paymentlogs,
            IRelatedTransactionDetailsServiceRepository relatedTransactionDetailsServiceRepository,
            ILinksRelatedServiceRepository linksRelated,
            IRefundPaypalServiceRepository refundPaypal,
            IRefundStripeServiceRepository refundStripe)
        {
            _TransactionsService = TransactionsService;
            _PaypalServices = Paypal;
            _mapper = mapper;
            _PaymentLogsService = Paymentlogs;
            _IRelatedTransaction = relatedTransactionDetailsServiceRepository;
            this.linksRelatedService = linksRelated;
            this.RefundserviceRepository = refundPaypal;
            this.refundStripeService = refundStripe;
        }
        // GET api/values
        [HttpGet]
        [Authorize]
        [Route("GetTransactions")]
        public ActionResult GetTransactions(string page, string sortBy)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";

            try
            {
                int pager = 0;
                if (!string.IsNullOrEmpty(page))
                {
                    pager = Convert.ToInt32(page);
                }
                var results = _TransactionsService.GetTransactions(pager, sortBy);
                var resultModel = new PageModel<TransactionModel>()
                {
                    Items = _mapper.Map<IEnumerable<Transactions>, IEnumerable<TransactionModel>>(results.Items),
                    Pager = results.Pager
                };
                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = resultModel;

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = "AQ GetTransactions",
                    Exception = ex
                });
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }


        [HttpGet]
        [Authorize]
        [Route("SearchPayment")]
        public ActionResult SearchPayment(string Today, string FromDate)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            try
            {
                var tday = Convert.ToDateTime(Today);
                var fday = Convert.ToDateTime(FromDate);
                var results = _TransactionsService.GetSearchTransaction(tday, fday).ToList();

                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = results;

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = "AQ SearchPayment",
                    Exception = ex
                });
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<string> GetPayment(string id)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";

            if (string.IsNullOrEmpty(id) == true)
            {
                responseData.StatusCode = 2;
                responseData.Message = "Id is required.";
                goto skipToReturn;
            }

            try
            {
                var result = _TransactionsService.GetTransactionById(id);

                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = result;

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = $"AQ GetPayment, TransactionID : {id}",
                    Exception = ex
                });
            }

        skipToReturn://label use to force return
            return Ok(responseData);

            //return "value";
        }

        [Authorize]
        [HttpPost("GetTokenCard")]
        public IActionResult GetTokenCard(CardModel card)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            responseData.Result.Data = "";
            try
            {
                var result = StripeHelpers.GetTokenCard(card);
                if (!string.IsNullOrEmpty(result))
                {
                    responseData.Message = "Successfully.";
                    responseData.StatusCode = 1;
                    responseData.Result.Data = result;
                }
            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;
                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = "AQ GetTokenCard",
                    Exception = ex
                });
            }

            return Ok(responseData);

            //return "value";
        }

        [Authorize]
        [HttpPost]
        [Route("RequestPayment")]
        public ActionResult RequestPayment([FromBody]TransactionRequestParams requestParams)
        {
            Transactions model = new Transactions();
            model.BackUrl = requestParams.BackUrl;
            model.Currency = requestParams.Currency;
            model.Description = requestParams.Description;
            model.ID = requestParams.ID;
            model.OrderAmount = requestParams.OrderAmount;
            model.OrderId = requestParams.OrderId;
            model.PaymentMethod = requestParams.PaymentMethod.ToUpper();

            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";

            if (model == null)
            {
                responseData.Message = "Value is required.";
                goto skipToReturn;
            }

            try
            {
                if (model != null)
                {
                    if (model.ReferenceId != null)//Kiểm tra có tồn tại trên api Striper thì dùng lệnh update
                    {

                    }
                }

                model.CreatedDate = DateTime.Now;
                model.OrderAmountRemaining = model.OrderAmount;
                _TransactionsService.InsertTransactions(model);

                if (model.PaymentMethod == Common.DTO.PaymentMethod.Partial.ToUpper())
                {
                    DateTime startDate = new DateTime(model.CreatedDate.Value.Year, model.CreatedDate.Value.Month, model.CreatedDate.Value.Day);

                    var transactionItems = new List<TransactionItems> { };
                    var item0 = new TransactionItems { OrderNo = 0, PayDate = startDate, PayPercent = 30, PayAmount = Convert.ToDecimal(((30 * model.OrderAmount.Value) / 100).ToString("#.##")) };
                    var item1 = new TransactionItems { OrderNo = 1, PayDate = startDate.AddDays(2), PayPercent = 35, PayAmount = Convert.ToDecimal(((35 * model.OrderAmount.Value) / 100).ToString("#.##")) };
                    var item2 = new TransactionItems { OrderNo = 2, PayDate = startDate.AddDays(4), PayPercent = 35, PayAmount = Convert.ToDecimal(((35 * model.OrderAmount.Value) / 100).ToString("#.##")) };

                    transactionItems.Add(item0);
                    transactionItems.Add(item1);
                    transactionItems.Add(item2);

                    transactionItems.ForEach(x => { x.CreatedDate = DateTime.Now; x.TransactionId = model.ID; });

                    _TransactionsService.InsertTransactionItems(transactionItems);
                }

                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = new
                {
                    TransactionId = model.ID,
                    PaymentMethod = model.PaymentMethod
                };

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = $"AQ RequestPayment",
                    Exception = ex
                });
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }

        [Authorize]
        [HttpPost]
        [Route("ProcessPayment")]
        public IActionResult ProcessPayment([FromBody]TransactionProcessParamsItems request, string PaymentType)
        {
            var requestParams = request.TransactionProcess;
            Transactions model = new Transactions();
            model.BackUrl = requestParams.BackUrl;
            model.Currency = requestParams.Currency;
            model.Description = requestParams.Description;
            model.ID = requestParams.ID;
            model.OrderAmount = requestParams.OrderAmount;
            model.OrderId = requestParams.OrderId;
            model.PaymentMethod = requestParams.PaymentMethod;
            model.PaymentCardToken = requestParams.PaymentCardToken;


            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";

            if (model == null)
            {
                responseData.StatusCode = 2;
                responseData.Message = "Value is required.";
                goto skipToReturn;
            }

            try
            {
                var trans = _TransactionsService.GetTransactionById(model.ID.ToString());
                if (trans == null)
                {
                    responseData.Message = "Notfound.";
                    responseData.StatusCode = 2;
                    goto skipToReturn;
                }

                #region PAYPAL
                if (PaymentType.ToUpper() == "PAYPAL") // Paypal
                {
                    var paypal = Payment_PayPal(request);
                    trans.ModifiedDate = DateTime.Now;
                    trans.PaymentTypeAPI = PaymentType.ToUpper();

                    _TransactionsService.UpdateTransactions(trans);

                    responseData.Message = "Successfully.";
                    responseData.StatusCode = 1;
                    responseData.Result.Data = new
                    {
                        TransactionId = model.ID,
                        PaymenID = paypal.id,
                        PaymentStatus = paypal.state,
                        FailureMessage = paypal.failed_transactions
                    };
                    return Ok(responseData);
                    //return new JsonResult(paypal);
                }
                #endregion
                #region Stripe
                else
                {
                    Stripe.Charge charge = null;
                    //var amount = charge.Amount.ToString();
                    decimal payAmount = 0;
                    if (trans.PaymentMethod == Common.DTO.PaymentMethod.Full.ToUpper())
                    {
                        payAmount = model.OrderAmount.Value;
                        charge = StripeHelpers.RequestCharge(model, payAmount, model.Description);

                        trans.Status = charge.Status;
                        trans.ReferenceId = charge.Id;
                    }
                    else
                    {
                        var transactionItems = _TransactionsService.GetTransactionItems(trans.ID).ToList()
                        .Where(x => x.Status == null).OrderBy(x => x.OrderNo).ToList();
                        var nextPayItem = transactionItems.FirstOrDefault();
                        payAmount = nextPayItem.PayAmount.Value;

                        string description = string.Format("Charge {0}% of OrderId: {1}. {2}", nextPayItem.PayPercent, model.OrderId, model.Description);
                        charge = StripeHelpers.RequestCharge(model, payAmount, description);

                        nextPayItem.Status = charge.Status;
                        nextPayItem.ReferenceId = charge.Id;
                        _TransactionsService.UpdateTransactionItem(nextPayItem);

                        trans.Status = PaymentStatus.Processing;
                    }

                    trans.ModifiedDate = DateTime.Now;
                    trans.PaymentCardToken = model.PaymentCardToken;
                    trans.StripeCustomerId = charge.CustomerId;
                    trans.OrderAmountRemaining = trans.OrderAmountRemaining.Value - payAmount;
                    trans.PaymentTypeAPI = PaymentType.ToUpper();

                    _TransactionsService.UpdateTransactions(trans);

                    responseData.Message = "Successfully.";
                    responseData.StatusCode = 1;
                    responseData.Result.Data = new
                    {
                        TransactionId = model.ID,
                        PaymenID = charge.Id,
                        PaymentStatus = charge.Status,
                        FailureMessage = charge.FailureMessage
                    };

                    var PaymentLogs = new PaymentLogs()
                    {
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(charge),
                        Error = "Success",
                        PaymentType = PaymentType,
                        FunctionName = "ProcessPayment",
                        TransID = requestParams.ID.ToString()
                    };
                    _PaymentLogsService.InsertPaymentLogs(PaymentLogs);
                }
                #endregion

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;
                var typeEx = ex.GetType();

                if (typeEx.FullName == "PayPal.PayPalException")
                {
                    var excep = ex as PayPal.PaymentsException;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Error:    " + excep.Details.name);
                    sb.AppendLine("Message:  " + excep.Details.message);
                    sb.AppendLine("URI:      " + excep.Details.information_link);
                    sb.AppendLine("Debug ID: " + excep.Details.debug_id);

                    foreach (var errorDetails in excep.Details.details)
                    {
                        sb.AppendLine("Details:  " + errorDetails.field + " -> " + errorDetails.issue);
                    }
                    var PaymentLogs = new PaymentLogs()
                    {
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(request),
                        Error = sb.ToString(),
                        PaymentType = PaymentType,
                        FunctionName = "ProcessPayment",
                        TransID = requestParams.ID.ToString()
                    };
                    _PaymentLogsService.InsertPaymentLogs(PaymentLogs);
                }
                else
                {
                    var PaymentLogs = new PaymentLogs()
                    {
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(request),
                        Error = ex.InnerException != null ? Newtonsoft.Json.JsonConvert.SerializeObject(ex.InnerException.Message) :  Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message),
                        PaymentType = PaymentType,
                        FunctionName = "ProcessPayment",
                        TransID = requestParams.ID.ToString()
                    };
                    _PaymentLogsService.InsertPaymentLogs(PaymentLogs);
                }



                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = $"AQ ProcessPayment PaymentType: {PaymentType} <br /> TransactionID : {requestParams.ID}",
                    Exception = ex
                });
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }

        [Authorize]
        [HttpPost]
        [Route("ProcessSchedulePayment")]
        public IActionResult ProcessSchedulePayment(string transactionId)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";

            if (string.IsNullOrEmpty(transactionId) == true)
            {
                responseData.StatusCode = 2;
                responseData.Message = "Id is required.";
                goto skipToReturn;
            }

            try
            {
                var trans = _TransactionsService.GetTransactionById(transactionId);
                if (trans == null)
                {
                    responseData.Message = "Notfound.";
                    responseData.StatusCode = 2;
                    goto skipToReturn;
                }

                var transactionItems = _TransactionsService.GetTransactionItems(trans.ID).ToList()
                    .Where(x => x.Status == null).OrderBy(x => x.OrderNo).ToList();
                var nextPayItem = transactionItems.FirstOrDefault();

                string description = string.Format("Charge {0}% of OrderId: {1}. {2}", nextPayItem.PayPercent, trans.OrderId, trans.Description);
                var charge = StripeHelpers.RequestChargeWithCustomer(trans, trans.StripeCustomerId, nextPayItem.PayAmount.Value, description);

                if (charge.Status == "succeeded")
                {
                    nextPayItem.Status = charge.Status;
                    nextPayItem.ReferenceId = charge.Id;
                    _TransactionsService.UpdateTransactionItem(nextPayItem);

                    trans.ModifiedDate = DateTime.Now;
                    trans.OrderAmountRemaining = trans.OrderAmountRemaining.Value - nextPayItem.PayAmount.Value;
                    if (trans.OrderAmountRemaining == 0 || trans.OrderAmountRemaining.Value.ToString("#") == "0")
                    {
                        trans.Status = PaymentStatus.Completed;
                    }

                    _TransactionsService.UpdateTransactions(trans);
                }

                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = new
                {
                    TransactionId = transactionId,
                    ProcessItemNo = nextPayItem.OrderNo,
                    ProcessItemAmount = nextPayItem.PayAmount,
                    ProcessItemPercent = nextPayItem.PayPercent,
                    ProcessStatus = charge.Status,
                };

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }

        [Authorize]
        [HttpPost]
        [Route("CreateCard")]
        public IActionResult CreateCard(string tokenCard, string cusID)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            try
            {
                Card trans = StripeHelpers.CreateCard(cusID, tokenCard);
                var CardModel = _mapper.Map<Card, CardStripeModel>(trans);

                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = trans;

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = "AQ Request CreateCard",
                    Exception = ex
                });
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }

        [Authorize]
        [HttpPost]
        [Route("DefaultCard")]
        public IActionResult DefaultCard(string cusID, string cardID)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            try
            {
                var trans = StripeHelpers.UpdateCardDefault(cusID, cardID);
                var customerModel = _mapper.Map<Customer, CustomerModel>(trans);

                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = trans;

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = "AQ Request DefaultCard",
                    Exception = ex
                });
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }

        [Authorize]
        [HttpPost]
        [Route("GetCardByCustomer")]
        public IActionResult GetCardByCustomer(string cusID)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            try
            {
                Customer trans = StripeHelpers.GetCustomerById(cusID);

                var customerModel = _mapper.Map<Customer, CustomerModel>(trans);

                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = trans;

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;

                //EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                //{
                //    Section = "AQ Request GetCardByCustomer",
                //    Exception = ex
                //});
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }

        [Authorize]
        [HttpPost]
        [Route("DeleteCard")]
        public IActionResult DeleteCard(string cusID, string cardID)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            try
            {
                var trans = StripeHelpers.RemoveCard(cusID, cardID);

                var CardModel = _mapper.Map<Card, CardStripeModel>(trans);


                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = trans;

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = "AQ Request DeleteCard",
                    Exception = ex
                });
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }

        [Authorize]
        [HttpGet]
        [Route("GetPageTrans")]
        public IActionResult GetPageTrans(string page, string Today, string FromDate, string sortBy, string type = "ALL", string cusID = null)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            try
            {
                int pager = 0;
                if (!string.IsNullOrEmpty(page))
                {
                    pager = Convert.ToInt32(page);
                }
                var tday = Convert.ToDateTime(Today);
                var fday = Convert.ToDateTime(FromDate);

                var trans = new PageModel<Transactions>();
                type = type.ToUpper();
                switch (type)
                {
                    case "UNPAID":
                        trans = _TransactionsService.SearchUnPaidTransaction(pager, tday, fday, sortBy, cusID);
                        break;
                    case "PAID":
                        trans = _TransactionsService.SearchPaidTransaction(pager, tday, fday, sortBy, cusID);
                        break;
                    case "PEDDING":
                        trans = _TransactionsService.SearchPedingTransaction(pager, tday, fday, sortBy, cusID);
                        break;
                    case "COMPLETE":
                        trans = _TransactionsService.SearchOrderCompleteTransaction(pager, tday, fday, sortBy, cusID);
                        break;
                    default:
                        trans = _TransactionsService.SearchAllTransaction(pager, tday, fday, sortBy, cusID);
                        break;
                }
                var transModel = new PageModel<TransactionModel>()
                {
                    Items = _mapper.Map<IEnumerable<Transactions>, IEnumerable<TransactionModel>>(trans.Items),
                    Pager = trans.Pager
                };

                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = transModel;

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = "AQ Request GetPageTrans",
                    Exception = ex
                });
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }

        [Authorize]
        [HttpGet]
        [Route("GetSchedulePaymentToday")]
        public ActionResult GetSchedulePaymentToday()
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";

            try
            {
                var results = _TransactionsService.GetTransactionItemsToday();

                responseData.Message = "Successfully.";
                responseData.StatusCode = 1;
                responseData.Result.Data = results;

            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;
            }

        skipToReturn://label use to force return
            return Ok(responseData);
        }

        private Payment Payment_PayPal(TransactionProcessParamsItems requestParams)
        {
            var result = _TransactionsService.GetTransactionById(requestParams.TransactionProcess.ID.ToString());
            var payment = new Payment();
            if (result == null)
            {
                return payment;
            }

            payment = _PaypalServices.CreatePayment(requestParams);
            result.Status = payment.state;
            result.ReferenceId = payment.id;
            _TransactionsService.UpdateTransactions(result);

            var PaymentLogs = new PaymentLogs()
            {
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(payment),
                Error = "Success",
                PaymentType = "PAYPAL",
                FunctionName = "ProcessPayment",
                TransID = requestParams.TransactionProcess.ID.ToString()
            };
            _PaymentLogsService.InsertPaymentLogs(PaymentLogs);


            return payment;
        }

        [Authorize]
        [Route("ExecutePaymentPayPal")]
        [HttpPost]
        public IActionResult ExecutePaymentPayPal(InfoPayment infoPayment)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            try
            {
                var payment = _PaypalServices.ExecutePayment(infoPayment.PaymentId, infoPayment.PayerId);

                var result = _TransactionsService.GetTransactionById(infoPayment.TranId);
                result.PaymentCardToken = infoPayment.PaymentToken;
                result.StripeCustomerId = infoPayment.PayerId;
                result.Status = payment.state;
                result.ModifiedDate = DateTime.Now;

                _TransactionsService.UpdateTransactions(result);


                foreach (var tran in payment.transactions)
                {
                    foreach (var item in tran.related_resources)
                    {
                        var RelatedTrans = new RelatedTransactionDetails()
                        {
                            totalAmount = Convert.ToDecimal(item.sale.amount.total),
                            TransID = result.ID.ToString(),
                            currency = item.sale.amount.currency,
                            IdRelatedSale = item.sale.id,
                            payment_mode = item.sale.payment_mode,
                            state = item.sale.state,
                            protection_eligibility = item.sale.protection_eligibility,
                            protection_eligibility_type = item.sale.protection_eligibility_type,
                            parent_payment = item.sale.parent_payment,
                        };
                        _IRelatedTransaction.InsertRelatedTransactionDetails(RelatedTrans);
                        foreach (var link in item.sale.links)
                        {
                            var LinksRelated = new LinksRelated()
                            {
                                href = link.href,
                                method = link.method,
                                rel = link.rel,
                                IdRelatedTransaction = RelatedTrans.ID,
                            };
                            linksRelatedService.InsertLinksRelated(LinksRelated);
                        }
                    }
                }


                // Hint: You can save the transaction details to your database using payment/buyer info
                responseData.StatusCode = 1;
                responseData.Message = "Success";
                responseData.Result.Data = new
                {
                    TransactionId = infoPayment.TranId,
                    Status = payment.state
                };

                var PaymentLogs = new PaymentLogs()
                {
                    Data = Newtonsoft.Json.JsonConvert.SerializeObject(payment),
                    Error = "Success",
                    PaymentType = "PAYPAL",
                    FunctionName = "ExecutePaymentPayPal",
                    TransID = infoPayment.TranId
                };
                _PaymentLogsService.InsertPaymentLogs(PaymentLogs);
            }
            catch (Exception ex)
            {
                responseData.StatusCode = 0;
                responseData.Message = "Something went wrong, please try again.";
                var typeEx = ex.GetType();
                if (typeEx.FullName == "PayPal.PayPalException")
                {
                    dynamic excep = ex.InnerException;
                    dynamic sourceEx = excep == null ? "" : excep?.InnerExceptions[0]?.Response;
                    var PaymentLogs = new PaymentLogs()
                    {
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(infoPayment),
                        Error = sourceEx,
                        PaymentType = "PAYPAL",
                        FunctionName = "ExecutePaymentPayPal",
                        TransID = infoPayment.TranId
                    };
                    _PaymentLogsService.InsertPaymentLogs(PaymentLogs);
                }
                else
                {

                    var PaymentLogs = new PaymentLogs()
                    {
                        //Data = Newtonsoft.Json.JsonConvert.SerializeObject(infoPayment),
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(infoPayment),
                        Error = ex.InnerException.Message,
                        PaymentType = "PAYPAL",
                        FunctionName = "ExecutePaymentPayPal",
                        TransID = infoPayment.TranId
                    };
                    _PaymentLogsService.InsertPaymentLogs(PaymentLogs);
                }


                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = $"ExecutePaymentPayPal <br /> TransactionID : {infoPayment.TranId}",
                    Exception = ex
                });
            }

            return Ok(responseData);
        }

        [HttpGet]
        [Route("RefundPaypal")]
        public IActionResult RefundPaypal(string TransId)
        {
            var trans = _TransactionsService.GetTransactionById(TransId);
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            responseData.Result.Data = new
            {
                TransactionId = TransId,
                Status = "Fail"
            };
            try
            {
                var RelatedTrans = _IRelatedTransaction.GetRelatedTransactionDetails(TransId).OrderByDescending(x => x.create_time).FirstOrDefault();
                if (RelatedTrans != null)
                {
                    var refund = _PaypalServices.RefundPayment(RelatedTrans.IdRelatedSale, RelatedTrans.parent_payment);
                    responseData.StatusCode = 1;
                    responseData.Message = "Success";
                    responseData.Result.Data = new
                    {
                        TransactionId = TransId,
                        Status = refund.state
                    };

                    var RefundPaypal = new RefundPaypal()
                    {
                        reason = refund.reason,
                        description = refund.description,
                        state = refund.state,
                        parent_payment = refund.parent_payment,
                        RefundID = refund.id,
                        sale_id = refund.sale_id,
                        TransID = TransId
                    };
                    RefundserviceRepository.InsertRefundPaypal(RefundPaypal);

                    var PaymentLogs = new PaymentLogs()
                    {
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(refund),
                        Error = "Success",
                        PaymentType = "PAYPAL",
                        FunctionName = "RefundPaypal",
                        TransID = TransId
                    };
                    _PaymentLogsService.InsertPaymentLogs(PaymentLogs);
                }
            }
            catch (Exception ex)
            {
                responseData.StatusCode = 0;
                responseData.Message = "Something went wrong, please try again.";
                var typeEx = ex.GetType();
                if (typeEx.FullName == "PayPal.PayPalException")
                {
                    dynamic excep = ex.InnerException;
                    dynamic sourceEx = excep.InnerExceptions[0].Response;
                    var PaymentLogs = new PaymentLogs()
                    {
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(trans),
                        Error = sourceEx,
                        PaymentType = "PAYPAL",
                        FunctionName = "RefundPaypal",
                        TransID = TransId
                    };
                    _PaymentLogsService.InsertPaymentLogs(PaymentLogs);
                }
                else
                {
                    var PaymentLogs = new PaymentLogs()
                    {
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(trans),
                        Error = ex.InnerException.Message,
                        PaymentType = "PAYPAL",
                        FunctionName = "RefundPaypal",
                        TransID = TransId
                    };
                    _PaymentLogsService.InsertPaymentLogs(PaymentLogs);
                }

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = $"AQ RefundPaypal PaymentType: PAYPAL <br /> TransactionID : {TransId}",
                    Exception = ex
                });
            }
            return Ok(responseData);
        }

        [HttpGet]
        [Route("RefundStripe")]
        public IActionResult RefundStripe(string TransId)
        {
            var trans = _TransactionsService.GetTransactionById(TransId);
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            responseData.Result.Data = new
            {
                TransactionId = TransId,
                Status = "Fail"
            };
            try
            {
                if (trans != null)
                {
                    var refund = StripeHelpers.RefundPayment(trans.ReferenceId);
                    responseData.StatusCode = 1;
                    responseData.Message = "Success";
                    responseData.Result.Data = new
                    {
                        TransactionId = TransId,
                        Status = refund.Status
                    };

                    var RefundStripe = new RefundStripe()
                    {
                        Reason = refund.Reason,
                        Description = refund.Description,
                        Status = refund.Status,
                        ChargeId = trans.ReferenceId,
                        Amount = refund.Amount,
                        RefundID = refund.Id,
                        FailureReason = refund.FailureReason,
                        Currency = refund.Currency,
                        TransID = TransId
                    };
                    refundStripeService.InsertRefundStripe(RefundStripe);

                    var PaymentLogs = new PaymentLogs()
                    {
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(refund),
                        Error = "Success",
                        PaymentType = "STRIPE",
                        FunctionName = "RefundStripe",
                        TransID = TransId
                    };
                    _PaymentLogsService.InsertPaymentLogs(PaymentLogs);
                }
            }
            catch (Exception ex)
            {
                responseData.StatusCode = 0;
                responseData.Message = "Something went wrong, please try again.";
                var typeEx = ex.GetType();
                var PaymentLogs = new PaymentLogs()
                {
                    Data = Newtonsoft.Json.JsonConvert.SerializeObject(trans),
                    Error = ex.Message,
                    PaymentType = "STRIPE",
                    FunctionName = "RefundStripe",
                    TransID = TransId
                };
                _PaymentLogsService.InsertPaymentLogs(PaymentLogs);

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = $"AQ RefundStripe PaymentType: STRIPE <br /> TransactionID : {TransId}",
                    Exception = ex
                });
            }
            return Ok(responseData);
        }

    }

}