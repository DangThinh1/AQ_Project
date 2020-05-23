using AQBooking.YachtPortal.Infrastructure.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class ServiceBase
    {
        #region Fields
        protected readonly IMapper _mapper;
        protected readonly AQYachtContext _yachtDbContext;
        #endregion

        #region Ctor
        public ServiceBase() { }

        public ServiceBase(AQYachtContext yachtDbContext)
        {
            this._yachtDbContext = yachtDbContext;
        }

        public ServiceBase(AQYachtContext yachtDbContext, IMapper mapper)
        {
            this._yachtDbContext = yachtDbContext;
            this._mapper = mapper;
        }

        #endregion

        #region Methods
        #endregion
    }
}
