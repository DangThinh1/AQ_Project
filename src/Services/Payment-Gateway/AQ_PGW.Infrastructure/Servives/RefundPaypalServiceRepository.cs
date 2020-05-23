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
    public class RefundPaypalServiceRepository : IRefundPaypalServiceRepository
    {
        private IUnitOfWork _unitOfWork;
        public RefundPaypalServiceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IQueryable<RefundPaypal> GetRefundPaypal(string transId)
        {
            return _unitOfWork.Repository<RefundPaypal>().Where(x => x.TransID.ToString() == transId).AsQueryable();
        }


        public void InsertRefundPaypal(RefundPaypal refund)
        {
            _unitOfWork.Repository<RefundPaypal>().Add(refund);
        }


        public bool UpdateRefundPaypal(RefundPaypal links)
        {
            _unitOfWork.Repository<RefundPaypal>().Update(links);
            return _unitOfWork.Save();
        }
    }
}
