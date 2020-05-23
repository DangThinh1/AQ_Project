using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.PortalLocation;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class PortalLocationService : ServiceBase, IPortalLocationService
    {
        #region Ctor
        public PortalLocationService(AQConfigContext dbContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbContext, workContext, mapper)
        {
        }
        #endregion Ctor

        #region Methods

        public IPagedList<PortalLocationViewModel> SearchPortalLocation(PortalLocationSearchModel model)
        {
            try
            {
                var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "CreatedDate DESC";
                var query = (from l in _dbConfigContext.PortalLocationControls
                             where l.Deleted == false && l.IsActive == true
                             && (string.IsNullOrEmpty(model.PortalUniqueId) || l.PortalUniqueId == model.PortalUniqueId)
                             && (model.CityCode == 0 || l.CityCode == model.CityCode)
                             && (model.CountryCode == 0 || l.CountryCode == model.CountryCode)
                             select new PortalLocationViewModel
                             {
                                 Id = l.Id,
                                 PortalUniqueId = l.PortalUniqueId,
                                 DomainPortalFID = l.DomainPortalFid,
                                 CityName = l.CityName,
                                 CityCode = l.CityCode,
                                 CountryName = l.CountryName,
                                 CountryCode = l.CountryCode,
                                 FileStreamFID = l.FileStreamFid,
                                 CreatedBy = l.CreatedBy,
                                 CreatedDate = l.CreatedDate,
                                 LastModifiedBy = l.LastModifiedBy,
                                 LastModifiedDate = l.LastModifiedDate,
                                 Deleted = l.Deleted,
                                 IsActive = l.IsActive
                             }).OrderBy(sortString);
                return new PagedList<PortalLocationViewModel>(query, model.PageIndex, model.PageSize);
            }
            catch
            {
                throw;
            }
        }

        public PortalLocationViewModel GetPortalLocationById(int id)
        {
            try
            {
                var entity = _dbConfigContext.PortalLocationControls.Where(x => x.Deleted == false && x.IsActive == true && x.Id == id).FirstOrDefault();
                var result = _mapper.Map<PortalLocationViewModel>(entity);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<BasicResponse> CreatePortalLocation(PortalLocationCreateModel model)
        {
            using (var transaction = _dbConfigContext.Database.BeginTransaction())
            {
                try
                {
                    var userId = _workContext.UserGuid;
                    var entity = new PortalLocationControls();
                    entity = _mapper.Map<PortalLocationControls>(model);
                    entity.CreatedBy = userId;
                    entity.CreatedDate = DateTime.Now;
                    var result = await _dbConfigContext.PortalLocationControls.AddAsync(entity);
                    await _dbConfigContext.SaveChangesAsync();
                    transaction.Commit();
                    transaction.Dispose();
                    return BasicResponse.Succeed("Success");
                }
                catch
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw;
                }
            }
        }

        public async Task<BasicResponse> UpdatePortalLocation(PortalLocationCreateModel model)
        {
            using (var transaction = _dbConfigContext.Database.BeginTransaction())
            {
                try
                {
                    var userId = _workContext.UserGuid;
                    var entity = _dbConfigContext.PortalLocationControls.Where(x => x.Deleted == false && x.IsActive == true && x.Id == model.Id).FirstOrDefault();
                    if (entity == null)
                    {
                        transaction.Dispose();
                        return BasicResponse.Failed("Fail");
                    }

                    entity = _mapper.Map<PortalLocationCreateModel, PortalLocationControls>(model, entity);
                    entity.LastModifiedBy = userId;
                    entity.LastModifiedDate = DateTime.Now;
                    var result = _dbConfigContext.PortalLocationControls.Update(entity);
                    await _dbConfigContext.SaveChangesAsync();
                    transaction.Commit();
                    transaction.Dispose();
                    return BasicResponse.Succeed("Success");
                }
                catch
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw;
                }
            }
        }

        public async Task<BasicResponse> DeletePortalLocation(int id)
        {
            using (var transaction = await _dbConfigContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var entity = _dbConfigContext.PortalLocationControls.Where(x => x.Deleted == false && x.IsActive == true && x.Id == id).FirstOrDefault();
                    entity.Deleted = true;
                    entity.LastModifiedBy = GetCurrentUserId();
                    entity.LastModifiedDate = DateTime.Now;
                    _dbConfigContext.PortalLocationControls.Update(entity);
                    await _dbConfigContext.SaveChangesAsync();
                    transaction.Commit();
                    transaction.Dispose();
                    return BasicResponse.Succeed("Success");
                }
                catch
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw;
                }
            }
        }

        public async Task<BasicResponse> DisablePortalLocation(int id)
        {
            using (var transaction = await _dbConfigContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var entity = _dbConfigContext.PortalLocationControls.Where(x => x.Deleted == false && x.IsActive == true && x.Id == id).FirstOrDefault();
                    if (entity == null)
                    {
                        transaction.Dispose();
                        return BasicResponse.Failed("Fail");
                    }

                    entity.IsActive = false;
                    entity.LastModifiedBy = GetCurrentUserId();
                    entity.LastModifiedDate = DateTime.Now;
                    _dbConfigContext.PortalLocationControls.Update(entity);
                    await _dbConfigContext.SaveChangesAsync();
                    transaction.Commit();
                    transaction.Dispose();
                    return BasicResponse.Succeed("Success");
                }
                catch
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw;
                }
            }
        }

        #endregion Methods
    }
}