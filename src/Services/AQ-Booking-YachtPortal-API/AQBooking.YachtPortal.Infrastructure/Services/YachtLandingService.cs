using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtLanding;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AQEncrypts;
using AutoMapper;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using AQBooking.YachtPortal.Core.Models.YachtMerchants;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtLandingService : IYachtLandingService
    {
        private readonly AQYachtContext _aQYachtContext;
        private readonly IMapper _mapper;
        public YachtLandingService(AQYachtContext aQYachtContext, IMapper mapper)
        {
            _aQYachtContext = aQYachtContext;
            _mapper = mapper;
        }

        public BaseResponse<PagedList<YachtLandingViewModel>> GetYachtByMerchantIDForLanding(SearchYachtWithMerchantIdModel searchModel)
        {
            try
            {
                var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                : "Name DESC";

                searchModel.PageIndex = searchModel.PageIndex > 0 ? searchModel.PageIndex : 1;
                searchModel.PageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 10;
                //var q = _aQYachtContext.Yachts.AsNoTracking()
                //        .Join(_aQYachtContext.YachtMerchants, x => x.MerchantFid, y => y.Id, (x, y) => new { x.Id, y.MerchantName })
                //        //.Select(xy => _mapper.Map<YachtLandingViewModel>(xy))
                //        //.Select(y => _mapper.Map<YachtLandingViewModel>(y))
                //        .ToList();
                _aQYachtContext.Yachts.AsNoTracking().Where(x => x.MerchantFid == searchModel.MerchantId.ToInt32()).Load();

                var yachtLst = _aQYachtContext.Yachts.AsNoTracking().Where(x => x.Deleted == false).ToList();

                var query = (from a in yachtLst
                             join b in _aQYachtContext.YachtMerchants.AsNoTracking().Where(x => x.Deleted == false) on a.MerchantFid equals b.Id
                             where a.MerchantFid == searchModel.MerchantId.ToInt32()
                             select _mapper.Map<(Yachts, YachtMerchants), YachtLandingViewModel>((a, b))).ToList();
                if (query != null)
                {
                    if (_aQYachtContext.YachtMerchantFileStreams.AsNoTracking().Any(x => x.MerchantFid == searchModel.MerchantId.ToInt32()))
                        query.ForEach(k => k.MerchantFileStreamId = _aQYachtContext.YachtMerchantFileStreams.AsNoTracking().Where(x => x.MerchantFid == k.MerchantFid && x.Deleted == false && x.ActivatedDate <= DateTime.Now).OrderByDescending(x => x.ActivatedDate).FirstOrDefault().FileStreamFid);

                    query.ForEach(k => k.YachtFileStreamId = _aQYachtContext.YachtFileStreams.AsNoTracking().Where(x => x.YachtFid == x.Id && (x.FileTypeFid == 4 || x.FileTypeFid == 5) && x.Deleted == false && x.ActivatedDate <= DateTime.Now).FirstOrDefault().FileStreamFid);
                }
                return BaseResponse<PagedList<YachtLandingViewModel>>.Success(new PagedList<YachtLandingViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtLandingViewModel>>.InternalServerError(new PagedList<YachtLandingViewModel>(Enumerable.Empty<YachtLandingViewModel>().AsQueryable(), searchModel.PageIndex, searchModel.PageSize), message: ex.Message, fullMsg: ex.StackTrace);
            }

        }
    }
}
