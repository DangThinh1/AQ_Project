using Accommodation.Core.Models.Hotels;
using Accommodation.Infrastructure.Databases;
using Accommodation.Infrastructure.Interfaces;
using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ExtendedUtility;
using Accommodation.Infrastructure.Databases.Entities;
using System.Linq.Dynamic.Core;
using AQEncrypts;
using Accommodation.Core.Helpers;
using Accommodation.Core.Models.HotelReservations;
using Accommodation.Core.Models.HotelInformations;
using Accommodation.Core.Models.HotelInformationDetails;
using Accommodation.Core.Models.HotelReservationDetails;

namespace Accommodation.Infrastructure.Services
{
    public class HotelService : ServiceBase, IHotelService
    {
        public HotelService(AccommodationContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                var query = _dbContext.Hotels.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
                if (query != null)
                {
                    query.Deleted = true;
                    _dbContext.Hotels.Update(query);
                    _dbContext.SaveChanges();
                    return BaseResponse<bool>.Success();
                }
                else
                    return BaseResponse<bool>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<HotelViewModel> FindByID(int id)
        {
            var query = _dbContext.Hotels.AsNoTracking().Where(x => x.Id == id).Select(x => _mapper.Map<HotelViewModel>(x)).FirstOrDefault();
            if (query != null)
                return BaseResponse<HotelViewModel>.Success(query);
            return BaseResponse<HotelViewModel>.NotFound(query);
        }

        public BaseResponse<PagedList<HotelViewModel>> Search(HotelSearchModel searchModel)
        {
            try
            {
                var merchantFid = Decrypt.ToInt32(searchModel.MerchantFid);
                if (searchModel != null)
                {
                    var pageIndex = searchModel.PageIndex > 0 ? searchModel.PageIndex : 1;
                    var pageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 10;
                    var sortColumn = !string.IsNullOrEmpty(searchModel.SortColumn) ? searchModel.SortColumn : "HotelName";
                    var sortType = !string.IsNullOrEmpty(searchModel.SortType) ? searchModel.SortType : "ASC";
                    var sortString = !string.IsNullOrEmpty(searchModel.SortString) ? searchModel.SortString : "HotelName DESC";
                    var query = _dbContext.Hotels.AsNoTracking()
                                .Where(x => x.Deleted != true
                                       && (x.HotelName.Contains(searchModel.HotelName) || string.IsNullOrEmpty(searchModel.HotelName))
                                       && (x.MerchantFid == int.Parse(searchModel.MerchantFid) || x.MerchantFid == 0)
                                       && (x.ActiveForOperation == searchModel.ActiveForOperation))
                                .Select(x => _mapper.Map<Hotels, HotelViewModel>(x)).OrderBy(sortString).ToList();

                    query.ForEach(x => x.hotelInforLst = _dbContext.HotelInformations.AsNoTracking().Where(y => y.Deleted != true && y.HotelFid == Int32.Parse(x.Id)).Select(y => _mapper.Map<HotelInformations, HotelInformationViewModel>(y)).ToList());
                    query.ForEach(x => x.hotelReservationLst = _dbContext.HotelReservations.AsNoTracking().Where(y => y.HotelFid == Int32.Parse(x.Id)).Select(y => _mapper.Map<HotelReservations, HotelReservationViewModel>(y)).ToList());
                    if (query.Select(x => x.hotelInforLst).Count() > 0 && query.Select(x => x.hotelReservationLst).Count() > 0)
                    {
                        query.ForEach(x => x.hotelInforLst.ForEach(z => z.hotelInforDetailLst = _dbContext.HotelInformationDetails.AsNoTracking().Where(y => y.Deleted != true && y.InformationFid == z.Id).Select(y => _mapper.Map<HotelInformationDetails, HotelInformationDetailViewModel>(y)).ToList()));
                        query.ForEach(x => x.hotelReservationLst.ForEach(z => z.hotelReserDetailLst = _dbContext.HotelReservationDetails.AsNoTracking().Where(y => y.ReservationsFid == z.Id).Select(y => _mapper.Map<HotelReservationDetails, HotelReservationDetailViewModel>(y)).ToList()));
                    }
                    return BaseResponse<PagedList<HotelViewModel>>.Success(new PagedList<HotelViewModel>(query.AsQueryable(), searchModel.PageIndex, searchModel.PageSize));
                }
                return BaseResponse<PagedList<HotelViewModel>>.NotFound();
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<HotelViewModel>>.InternalServerError(ex);
            }
        }
    }
}
