using APIHelpers.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YachtMerchant.Core.Models.YachtTourCharterPaymentLogs;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourCharterPaymentLogsService: ServiceBase, IYachtTourCharterPaymentLogsService
    {
        private readonly IMapper _mapper;
        public YachtTourCharterPaymentLogsService(IMapper mapper, YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _mapper = mapper;
        }
        public BaseResponse<List<YachtTourCharterPaymentLogsViewModel>> GetYachtTourCharterPaymentLogsByTourCharterId(long id)
        {
            try
            {
                var entity = _context.YachtTourCharterPaymentLogs.Where(x => x.TourCharterFid == id).AsNoTracking().Select(s => _mapper.Map<YachtTourCharterPaymentLogs, YachtTourCharterPaymentLogsViewModel>(s));
                if (entity.Count() > 0)
                    return BaseResponse<List<YachtTourCharterPaymentLogsViewModel>>.Success(entity.ToList());
                else
                    return BaseResponse<List<YachtTourCharterPaymentLogsViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtTourCharterPaymentLogsViewModel>>.InternalServerError(message: ex.Message);
            }
        }

    }
}
