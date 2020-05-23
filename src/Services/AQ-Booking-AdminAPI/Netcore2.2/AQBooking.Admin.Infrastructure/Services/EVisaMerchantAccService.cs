using AQBooking.Admin.Core.Models.EVisaMerchantAccount;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.EvisaEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class EVisaMerchantAccService : ServiceBase, IEVisaMerchantAccService
    { 
        #region Fields
        #endregion

        #region Ctor
        public EVisaMerchantAccService(
            AQEvisaContext dbEVisaContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbEVisaContext, workContext, mapper)
        { }
        #endregion

        #region Methods
        public IPagedList<EVisaMerchantAccViewModel> SearchEVisaMerchantAcc(EVisaMerchantAccSearchModel model)
        {
            var searchString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : $"{nameof(VisaMerchantUsers.CreatedDate)} Desc";
            var lstEvisaMerchant = new PagedList<EVisaMerchantAccViewModel>();
            var query = _dbEvisaContext.VisaMerchantUsers.AsNoTracking().Where(x => x.Deleted == false
            && (string.IsNullOrEmpty(model.UserFid) || x.UserFid.ToString() == model.UserFid)
            && (model.MerchantFid == 0 || x.MerchantFid == model.MerchantFid)
            && (model.EffectiveStartDate == null || x.EffectiveStartDate >= DateTime.Now)
            && (model.EffectiveEndDate == null || x.EffectiveEndDate <= DateTime.Now)).Select(x => _mapper.Map<EVisaMerchantAccViewModel>(x)).OrderBy(searchString);

            if (query.Count() > 0)
                lstEvisaMerchant = new PagedList<EVisaMerchantAccViewModel>(query, model.PageIndex, model.PageSize);

            return lstEvisaMerchant;
        }

        public EVisaMerchantAccViewModel GetEvisaMerchantAccById(int id)
        {
            var evisaMerchant = new EVisaMerchantAccViewModel();
            var query = _dbEvisaContext.VisaMerchantUsers.AsNoTracking().Where(x => x.Deleted == false && x.Id == id).Select(x => _mapper.Map<EVisaMerchantAccViewModel>(x)).AsQueryable();
            if (query.Count() > 0)
                evisaMerchant = query.FirstOrDefault();

            return evisaMerchant;
        }

        public bool CreateEvisaMerchantAcc(EVisaMerchantAccCreateUpdateModel model)
        {
            var result = false;
            var entity = new VisaMerchantUsers();
            entity = _mapper.Map<EVisaMerchantAccCreateUpdateModel, VisaMerchantUsers>(model, entity);
            entity.CreatedBy = _workContext.UserGuid;
            entity.CreatedDate = DateTime.Now;
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbEvisaContext.VisaMerchantUsers.Add(entity);
            _dbEvisaContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool UpdateEvisaMerchantAcc(EVisaMerchantAccCreateUpdateModel model)
        {
            var result = false;
            var entity = new VisaMerchantUsers();
            var query = _dbEvisaContext.VisaMerchantUsers.Where(x => x.Deleted == false && x.Id == model.Id);
            if (query.Count() > 0)
                entity = query.FirstOrDefault();
            else
                return result;
            entity = _mapper.Map<EVisaMerchantAccCreateUpdateModel, VisaMerchantUsers>(model, entity);
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbEvisaContext.VisaMerchantUsers.Update(entity);
            _dbEvisaContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool DeleteEvisaMerchantAcc(int id)
        {
            var result = false;
            var entity = new VisaMerchantUsers();
            var query = _dbEvisaContext.VisaMerchantUsers.Where(x => x.Deleted == false && x.Id == id);
            if (query.Count() > 0)
                entity = query.FirstOrDefault();
            else
                return result;
            entity.Deleted = true;
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbEvisaContext.VisaMerchantUsers.Update(entity);
            _dbEvisaContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }
        #endregion
    }
}
