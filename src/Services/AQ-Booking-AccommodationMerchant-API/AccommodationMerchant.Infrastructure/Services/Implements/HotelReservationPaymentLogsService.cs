using AccommodationMerchant.Core.Models.HotelReservationPaymentLogs;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelReservationPaymentLogsService:ServiceBase, IHotelReservationPaymentLogsService
    {
        public HotelReservationPaymentLogsService(AccommodationContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public BaseResponse<List<HotelReservationPaymentLogViewModel>> GetReservationPaymentLogsByReservationId(long id)
        {
            try
            {
                var entity = _db.HotelReservationPaymentLogs.AsNoTracking().Where(x => x.ReservationsFid == id).Select(s => _mapper.Map<HotelReservationPaymentLogs, HotelReservationPaymentLogViewModel>(s));
                if (entity.Count() > 0)
                    return BaseResponse<List<HotelReservationPaymentLogViewModel>>.Success(entity.ToList());
                return BaseResponse<List<HotelReservationPaymentLogViewModel>>.NoContent(new List<HotelReservationPaymentLogViewModel>());

            }
            catch (Exception ex)
            {
                return BaseResponse<List<HotelReservationPaymentLogViewModel>>.InternalServerError(message: ex.Message);
            }

        }



    }
}
