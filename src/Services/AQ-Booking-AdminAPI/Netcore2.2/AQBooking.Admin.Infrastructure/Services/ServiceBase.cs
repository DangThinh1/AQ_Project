using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Databases.YachtEntities;
using AQBooking.Admin.Infrastructure.Databases.DiningEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Databases.EvisaEntities;
using AQBooking.Admin.Infrastructure.Databases.HotelEntities;
using AutoMapper;
using System;
using AQBooking.Admin.Infrastructure.Databases.CMSEntities;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class ServiceBase
    {
        #region Fields
        protected readonly AQConfigContext _dbConfigContext;
        protected readonly AQYachtContext _dbYachtContext;
        protected readonly AQDiningContext _dbDiningContext;
        protected readonly AQEvisaContext _dbEvisaContext;
        protected readonly AQHotelContext _dbHotelContext;
        protected readonly AQCMSContext _dbCMSContext;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;
        #endregion Fields

        #region Ctor
        public ServiceBase(AQConfigContext dbConfigContext,
            IWorkContext workContext,
            IMapper mapper)
        {
            _dbConfigContext = dbConfigContext;
            _workContext = workContext;
            _mapper = mapper;
        }

        public ServiceBase(AQConfigContext dbConfigContext,
            AQYachtContext dbYachtContext,
            IMapper mapper)
        {
            _dbConfigContext = dbConfigContext;
            _dbYachtContext = dbYachtContext;
            _mapper = mapper;
        }

        public ServiceBase(
            IWorkContext workContext,
            IMapper mapper)
        {
            _workContext = workContext;
            _mapper = mapper;
        }

        public ServiceBase(
            AQYachtContext dbYachtContext,
            IWorkContext workContext,
            IMapper mapper)
        {
            _dbYachtContext = dbYachtContext;
            _workContext = workContext;
            _mapper = mapper;
        }

        public ServiceBase(
            AQDiningContext dbDiningContext,
            IWorkContext workContext,
            IMapper mapper)
        {
            _dbDiningContext = dbDiningContext;
            _workContext = workContext;
            _mapper = mapper;
        }

        public ServiceBase(
            AQEvisaContext dbEvisaContext,
            IWorkContext workContext,
            IMapper mapper)
        {
            _dbEvisaContext = dbEvisaContext;
            _workContext = workContext;
            _mapper = mapper;
        }

        public ServiceBase(
            AQHotelContext dbHotelContext,
            IWorkContext workContext,
            IMapper mapper)
        {
            _dbHotelContext = dbHotelContext;
            _workContext = workContext;
            _mapper = mapper;
        }
        public ServiceBase(AQCMSContext dbCMSContext, IWorkContext workContext, IMapper mapper)
        {
            _dbCMSContext = dbCMSContext;
            _workContext = workContext;
            _mapper = mapper;
        }
        #endregion Ctor

        #region Methods
        public Guid GetCurrentUserId()
        {
            return _workContext.UserGuid;
        }

        #endregion Methods
    }
}