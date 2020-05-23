using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.Subscriber;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.CMSEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using APIHelpers.Response;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class SubscriberService : ServiceBase, ISubscriberService
    {
        #region Ctor
        public SubscriberService(AQCMSContext dbCMSContext, IWorkContext workContext, IMapper mapper) : base(dbCMSContext, workContext, mapper)
        {

        }
        #endregion
        #region Methods
        public async Task<BaseResponse<bool>> CreateNewSubscriber(SubscriberCreateModel model)
        {
            try
            {
                var currentModel = _dbCMSContext.Subscribers.FirstOrDefault(item => (item.Email == model.Email));
                if (currentModel != null) return BaseResponse<bool>.BadRequest();

                Subscribers obj = new Subscribers();
                obj = _mapper.Map<Subscribers>(model);
                var res = await _dbCMSContext.Subscribers.AddAsync(obj);
                _dbCMSContext.SaveChanges();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public IPagedList<SubscriberViewModel> SearchSubscriber(SubscriberSearchModel searchModel)
        {
            var query = _dbCMSContext.Subscribers.AsNoTracking();
            if (!string.IsNullOrEmpty(searchModel.Email))
                query = query.Where(x => x.Email.Contains(searchModel.Email));
            if (searchModel.IsActivated.HasValue)
                query = query.Where(x => x.IsActivated == searchModel.IsActivated);
            if (searchModel.CreatedDateFrom.HasValue)
                query = query.Where(x => x.CreatedDate >= searchModel.CreatedDateFrom);
            if (searchModel.CreatedDateTo.HasValue)
                query = query.Where(x => x.CreatedDate <= searchModel.CreatedDateTo);
            query = query.OrderByDescending(x => x.CreatedDate);

            return new PagedList<SubscriberViewModel>(_mapper.ProjectTo<SubscriberViewModel>(query), searchModel.PageIndex, searchModel.PageSize);

        }
        public List<SubscriberViewModel> GetListSubToExport(SubscriberSearchModel searchModel)
        {
            var query = _dbCMSContext.Subscribers.AsNoTracking();
            if (searchModel.CreatedDateFrom.HasValue)
                query = query.Where(x => x.CreatedDate >= searchModel.CreatedDateFrom);
            if (searchModel.CreatedDateTo.HasValue)
                query = query.Where(x => x.CreatedDate <= searchModel.CreatedDateTo);
            query = query.OrderByDescending(x => x.CreatedDate);
            return _mapper.ProjectTo<SubscriberViewModel>(query).ToList();
        }
        #endregion
    }
}
