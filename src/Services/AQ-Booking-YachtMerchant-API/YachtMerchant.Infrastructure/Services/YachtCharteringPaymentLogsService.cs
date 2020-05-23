using APIHelpers.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YachtMerchant.Core.Models.YachtCharteringPaymentLogs;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtCharteringPaymentLogsService:ServiceBase, IYachtCharteringPaymentLogsService
    {
        private readonly IMapper _mapper;
        public YachtCharteringPaymentLogsService(IMapper mapper, YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _mapper = mapper;
        }

        public BaseResponse<List<YachtCharteringPaymentLogsViewModel>> GetYachtCharteringPaymentLogsByCharteringId(long id)
        {
            try
            {
                var entity = _context.YachtCharteringPaymentLogs.AsNoTracking().Where(x => x.CharteringFid == id).Select(s=> _mapper.Map<YachtCharteringPaymentLogs, YachtCharteringPaymentLogsViewModel>(s));
                if (entity.Count() > 0)
                    return BaseResponse<List<YachtCharteringPaymentLogsViewModel>>.Success(entity.ToList());
                else
                    return BaseResponse<List<YachtCharteringPaymentLogsViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtCharteringPaymentLogsViewModel>>.InternalServerError(message:ex.Message);
            }
        }
    }
}
