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
    public class LinksRelatedServiceRepository : ILinksRelatedServiceRepository
    {
        private IUnitOfWork _unitOfWork;
        public LinksRelatedServiceRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<LinksRelated> GetLinksRelated(string Id)
        {
            return _unitOfWork.Repository<LinksRelated>().Where(x => x.ID.ToString() == Id).AsQueryable();
        }

        public void InsertLinksRelated(LinksRelated links)
        {
            _unitOfWork.Repository<LinksRelated>().Add(links);
            //return _unitOfWork.Save();
        }

        public bool UpdateLinksRelated(LinksRelated links)
        {

            _unitOfWork.Repository<LinksRelated>().Update(links);
            return _unitOfWork.Save();
        }

    }
}
