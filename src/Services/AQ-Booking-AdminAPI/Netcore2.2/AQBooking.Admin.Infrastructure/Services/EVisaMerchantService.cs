using AQBooking.Admin.Core.Models.EVisaMerchant;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.EvisaEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AQBooking.Admin.Infrastructure.Interfaces;
using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class EVisaMerchantService : ServiceBase, IEVisaMerchantService
    {
        #region Fields
        #endregion
        
        #region Ctor
        public EVisaMerchantService(
            AQEvisaContext dbAQEvisaContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbAQEvisaContext, workContext, mapper)
        { }
        #endregion

        #region Methods
        public IPagedList<EVisaMerchantViewModel> SearchEVisaMerchant(EVisaMerchantSearchModel model)
        {
            var searchString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : $"{nameof(VisaMerchants.CreatedDate)} Desc";
            var lstEvisaMerchant = new PagedList<EVisaMerchantViewModel>();
            var query = _dbEvisaContext.VisaMerchants.AsNoTracking().Where(x => x.Deleted == false
            && string.IsNullOrEmpty(model.Country) || x.Country.Contains(model.Country)
            && string.IsNullOrEmpty(model.City) || x.City.Contains(model.City)
            && string.IsNullOrEmpty(model.State) || x.State.Contains(model.State)).Select(x => _mapper.Map<EVisaMerchantViewModel>(x)).OrderBy(searchString);

            if (query.Count() > 0)
                lstEvisaMerchant = new PagedList<EVisaMerchantViewModel>(query, model.PageIndex, model.PageSize);

            return lstEvisaMerchant;
        }

        public EVisaMerchantViewModel GetEvisaMerchantById(int id)
        {
            var evisaMerchant = new EVisaMerchantViewModel();
            var query = _dbEvisaContext.VisaMerchants.AsNoTracking().Where(x => x.Deleted == false && x.Id == id).Select(x => _mapper.Map<EVisaMerchantViewModel>(x)).AsQueryable();
            if (query.Count() > 0)
                evisaMerchant = query.FirstOrDefault();

            return evisaMerchant;
        }

        public bool CreateEvisaMerchant(EVisaMerchantCreateUpdateModel model)
        {
            var result = false;
            var entity = new VisaMerchants();
            entity = _mapper.Map<EVisaMerchantCreateUpdateModel, VisaMerchants>(model, entity);
            entity.UniqueId = UniqueIDHelper.GenerateRandomString(12);
            entity.CountryVisa = model.Country;
            entity.CreatedBy = _workContext.UserGuid;
            entity.CreatedDate = DateTime.Now;
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbEvisaContext.VisaMerchants.Add(entity);
            _dbEvisaContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool UpdateEvisaMerchant(EVisaMerchantCreateUpdateModel model)
        {
            var result = false;
            var entity = new VisaMerchants();
            var query = _dbEvisaContext.VisaMerchants.Where(x => x.Deleted == false && x.Id == model.Id);
            if (query.Count() > 0)
                entity = query.FirstOrDefault();
            else
                return result;
            entity = _mapper.Map<EVisaMerchantCreateUpdateModel, VisaMerchants>(model, entity);
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbEvisaContext.VisaMerchants.Update(entity);
            _dbEvisaContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool DeleteEvisaMerchant(int id)
        {
            var result = false;
            var entity = new VisaMerchants();
            var query = _dbEvisaContext.VisaMerchants.Where(x => x.Deleted == false && x.Id == id);
            if (query.Count() > 0)
                entity = query.FirstOrDefault();
            else
                return result;
            entity.Deleted = true;
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbEvisaContext.VisaMerchants.Update(entity);
            _dbEvisaContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        /// <summary>
        /// Get list evisa merchant selectlist don't have user account
        /// </summary>
        /// <returns></returns>
        public List<SelectListModel> GetEVisaMerchantNoUserSll()
        {
            var userId = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            var listMerchantSll = new List<SelectListModel>();
            var listMerchantHasUser = _dbEvisaContext.VisaMerchantUsers.Where(x => x.Deleted == false).ToList();
            if (userRoleId != (int)UserRoleEnum.EVisaMerchantManager)
            {
                listMerchantSll = _dbEvisaContext.VisaMerchants.Where(x => !listMerchantHasUser.Any(y => y.MerchantFid == x.Id)).Select(r => new SelectListModel(r.MerchantName, r.Id.ToString())).ToList();
            }
            else
            {
                //Pending because table visamerchantmanager has not created    
            }

            return listMerchantSll;
        }

        /// <summary>
        /// Get all evisa merchant selectlist
        /// </summary>
        /// <returns></returns>
        public List<SelectListModel> GetAllEVisaMerchantSll()
        {
            var lstMerchant = new List<SelectListModel>();
            var query = _dbEvisaContext.VisaMerchants.Where(x => x.Deleted == false).Select(x => new SelectListModel(x.MerchantName, x.Id.ToString()));
            if (query.Count() > 0)
                lstMerchant = query.ToList();
            return lstMerchant;
        }
        #endregion
    }
}
