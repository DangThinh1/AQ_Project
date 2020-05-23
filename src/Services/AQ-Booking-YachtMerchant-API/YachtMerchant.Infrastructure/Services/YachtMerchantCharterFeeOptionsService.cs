using APIHelpers.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YachtMerchant.Core.Models.YachtMerchantCharterFeeOptions;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtMerchantCharterFeeOptionsService: ServiceBase , IYachtMerchantCharterFeeOptionsService
    {
        private readonly IMapper _mapper;
        public YachtMerchantCharterFeeOptionsService(IMapper mapper, YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _mapper = mapper;
        }

        public BaseResponse<List<YachtMerchantCharterFeeOptionsViewModel>> GetAllCharterFeeOptionOfMerchantByMerchantId(int merchantId)
        {
            try
            {
                var entity = _context.YachtMerchantCharterFeeOptions.AsNoTracking().Where(x => x.MerchantFid == merchantId  && x.Deleted==false).AsEnumerable().Select(s => _mapper.Map<YachtMerchantCharterFeeOptions, YachtMerchantCharterFeeOptionsViewModel>(s));
                if (entity.Count() > 0)
                    return BaseResponse<List<YachtMerchantCharterFeeOptionsViewModel>>.Success(entity.ToList());
                else
                    return BaseResponse<List<YachtMerchantCharterFeeOptionsViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtMerchantCharterFeeOptionsViewModel>>.InternalServerError(message: ex.Message);
            }
        }
    }
}
