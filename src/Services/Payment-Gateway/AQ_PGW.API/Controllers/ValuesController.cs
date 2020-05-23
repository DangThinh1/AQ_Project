using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using Microsoft.AspNetCore.Mvc;

namespace AQ_PGW.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        ITransactionsServiceRepository _TransactionsService;
        public ValuesController(ITransactionsServiceRepository TransactionsService)
        {
            _TransactionsService = TransactionsService;
        }
        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            var Trans = _TransactionsService.GetTransactions();
            return Ok(Trans);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            StripeHelpers.GetByIdCharge(id);
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("PostData")]
        public IActionResult PostData([FromBody]Transactions value)
        {
            if (value != null)
            {
                if (value.ReferenceId != null)//Kiểm tra có tồn tại trên api Striper thì dùng lệnh update
                {

                }
                var rescoure = StripeHelpers.RequestCharge(value);

                Transactions trans = new Transactions()
                {
                    ID = Guid.NewGuid(),
                    Currency = rescoure.Currency,
                    OrderId = rescoure.OrderId,
                    ReferenceId = rescoure.Id,
                    CreatedDate = DateTime.Now,
                    OrderAmount = rescoure.Amount,
                    Description = rescoure.Description,
                    PaymentMethod = rescoure.PaymentMethodId,
                    Status = rescoure.Status,
                    BackUrl = rescoure.ReceiptUrl
                };
                _TransactionsService.InsertTransactions(trans);
                return Ok(rescoure);
            }
            return Ok("Fail");
        }

        // POST api/values
        [HttpPost]
        public IActionResult UpdateById(string value)
        {
            StripeHelpers.UpdateWithKeyCharge(value);
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            StripeHelpers.GetListCharge(new Transactions());
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
