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
    public class RelatedTransactionDetailsServiceRepository : IRelatedTransactionDetailsServiceRepository
    {
        private IUnitOfWork _unitOfWork;
        public RelatedTransactionDetailsServiceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IQueryable<RelatedTransactionDetails> GetRelatedTransactionDetails(string Id)
        {
            return _unitOfWork.Repository<RelatedTransactionDetails>().Where(x => x.TransID.ToString().ToUpper() == Id.ToUpper()).AsQueryable();
        }


        public bool InsertRelatedTransactionDetails(RelatedTransactionDetails RelatedTransactionDetails)
        {
            _unitOfWork.Repository<RelatedTransactionDetails>().Add(RelatedTransactionDetails);
            return _unitOfWork.Save();
        }

        public bool UpdateRelatedTransactionDetails(RelatedTransactionDetails RelatedTransactionDetails)
        {

            _unitOfWork.Repository<RelatedTransactionDetails>().Update(RelatedTransactionDetails);
            return _unitOfWork.Save();
        }
    }
}
