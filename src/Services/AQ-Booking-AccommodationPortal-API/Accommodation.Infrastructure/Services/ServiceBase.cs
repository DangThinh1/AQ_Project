using Accommodation.Infrastructure.Databases;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accommodation.Infrastructure.Services
{
    public class ServiceBase
    {
        protected readonly IMapper _mapper;
        protected readonly AccommodationContext _dbContext;

        public ServiceBase() { }

        public ServiceBase(AccommodationContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

    }
}
