using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;

namespace AQ_PGW.API.MappingModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transactions, TransactionModel>();

            CreateMap<Customer, CustomerModel>();

            CreateMap<Card, CardStripeModel>();
        }
    }
}
