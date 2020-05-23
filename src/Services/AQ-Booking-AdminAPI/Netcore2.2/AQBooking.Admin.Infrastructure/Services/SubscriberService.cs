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
        public ApiActionResult CreateNewSubscriber(SubscriberCreateModel model)
        {
            try
            {
                Subscribers obj = new Subscribers();
                obj = _mapper.Map<Subscribers>(model);
                var res = _dbCMSContext.Subscribers.Add(obj);
                _dbCMSContext.SaveChanges();
                return ApiActionResult.Success();
            }
            catch (Exception ex)
            {
                return ApiActionResult.Failed(ex.Message);
                throw ex;
            }
        }

        public IPagedList<SubscriberViewModel> SearchSubscriber(SubscriberSearchModel searchModel)
        {
            var query = (from p in _dbCMSContext.Subscribers.AsNoTracking()
                         where (p.Email.Contains(searchModel.Email) || string.IsNullOrEmpty(searchModel.Email))
                               || (p.IsActivated == searchModel.IsActivated || searchModel.IsActivated == null)
                               || (p.CreatedDate >= searchModel.CreatedDateFrom || searchModel.CreatedDateFrom == null)
                               || (p.CreatedDate <= searchModel.CreatedDateTo || searchModel.CreatedDateTo == null)
                         select _mapper.Map<Subscribers, SubscriberViewModel>(p)).ToList();
            throw new NotImplementedException();
        }
        #endregion
    }
}
