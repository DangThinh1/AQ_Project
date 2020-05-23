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
    public class PaymentLogsServiceRepository : IPaymentLogsServiceRepository
    {
        private IUnitOfWork _unitOfWork;
        public PaymentLogsServiceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<PaymentLogs> GetByTransID(string Id)
        {
            return _unitOfWork.Repository<PaymentLogs>().Where(x => x.TransID == Id).AsQueryable();
        }

        public bool InsertPaymentLogs(PaymentLogs payment)
        {
            _unitOfWork.Repository<PaymentLogs>().Add(payment);
            return _unitOfWork.Save();
        }

        public bool UpdatePaymentLogs(PaymentLogs payment)
        {
            _unitOfWork.Repository<PaymentLogs>().Update(payment);
            return _unitOfWork.Save();
        }
    }
}
