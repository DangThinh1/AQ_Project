using AQ_PGW.Common.Important;
using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using IUnitOfWork = AQ_PGW.Infrastructure.Repositories.IUnitOfWork;

namespace AQ_PGW.Infrastructure.Servives
{
    public class RefundStripeServiceRepository : IRefundStripeServiceRepository
    {
        private IUnitOfWork _unitOfWork;
        public RefundStripeServiceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public IQueryable<RefundStripe> GetRefundStripe(string transId)
        {
                return _unitOfWork.Repository<RefundStripe>().Where(x => x.TransID.ToString().ToUpper() == transId.ToUpper()).AsQueryable();
            
        }

        public void InsertRefundStripe(RefundStripe RefundStripe)
        {
            _unitOfWork.Repository<RefundStripe>().Add(RefundStripe);
        }


        public bool UpdateRefundStripe(RefundStripe RefundStripe)
        {
            _unitOfWork.Repository<RefundStripe>().Update(RefundStripe);
            return _unitOfWork.Save();
        }
    }
}
