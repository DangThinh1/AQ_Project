using APIHelpers.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using YachtMerchant.Core.Models.Calendar;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtCalendarService : IYachtCalendarService
    {
        private readonly YachtOperatorDbContext _db;
        private readonly IMapper _mapper;

        public YachtCalendarService(YachtOperatorDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public BaseResponse<List<YachtCalendar>> Calendar(int merchantId, DateTime start, DateTime end)
        {
            try
            {
                var listYachtCharter = (from ch in _db.YachtCharterings
                                        join y in _db.Yachts on ch.YachtFid equals y.Id
                                        where y.MerchantFid == merchantId && !y.Deleted && ch.StatusFid == 4 &&
                                            ch.CharterDateFrom.Date >= start.AddMonths(-1).Date && ch.CharterDateTo.Date <= end.AddMonths(1).Date
                                        select new YachtCalendar()
                                        {
                                            YachtId = ch.YachtFid,
                                            YachtName = _db.Yachts.FirstOrDefault(x => x.Id == ch.YachtFid).Name,
                                            Start = ch.CharterDateFrom.Date,
                                            End = ch.CharterDateTo.Date,
                                            Id = ch.Id,
                                            Status = 1
                                        }).ToList();

                var listYachtNonOperator = (from n in _db.YachtNonOperationDays
                                            join y in _db.Yachts on n.YachtFid equals y.Id
                                            where y.MerchantFid == merchantId && !y.Deleted && !n.Deleted &&
                                                n.StartDate.Date >= start.AddMonths(-1).Date && n.EndDate.Date <= end.AddMonths(1).Date
                                            select new YachtCalendar()
                                            {
                                                YachtId = n.YachtFid,
                                                YachtName = _db.Yachts.FirstOrDefault(x => x.Id == n.YachtFid).Name,
                                                Start = n.StartDate.Date,
                                                End = n.EndDate.Date,
                                                Id = n.Id,
                                                Status = 2,
                                                Recurring = n.Recurring
                                            }).ToList();

                var merList = listYachtCharter.Concat(listYachtNonOperator).OrderBy(x => x.Start).ToList();
                return BaseResponse<List<YachtCalendar>>.Success(merList);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtCalendar>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}