using APIHelpers.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using YachtMerchant.Core.Models.PortLocation;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtPortService : ServiceBase, IYachtPortService
    {
        #region Fields
        private readonly IMapper _mapper;
        #endregion

        #region Ctor
        public YachtPortService(
            IMapper mapper,
            YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            this._mapper = mapper;
        }
        #endregion

        #region Methods
        public BaseResponse<List<PortLocationViewModel>> GetListPortLocation()
        {
            try
            {
                var listPortLocation = _context.PortLocations.AsNoTracking().Where(x => x.Deleted == false).Select(x => _mapper.Map<PortLocationViewModel>(x)).ToList();
                return listPortLocation.Count > 0 ? BaseResponse <List<PortLocationViewModel>>.Success(listPortLocation) : BaseResponse<List<PortLocationViewModel>>.Success( new List<PortLocationViewModel>());
            }
            catch(Exception ex)
            {
                return BaseResponse<List<PortLocationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<PortLocationViewModel>> GetPortLocationByCityName(string cityName)
        {
            try
            {
                var entities = _context.PortLocations
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.City.ToUpper() == cityName.ToUpper())
                    .Select(k => _mapper.Map<PortLocations, PortLocationViewModel>(k))
                    .ToList();
                return BaseResponse <List<PortLocationViewModel>>.Success(entities);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortLocationViewModel>>.InternalServerError(message:ex.Message, fullMsg:ex.StackTrace);
            }

        }
        public BaseResponse<List<PortLocationViewModel>> GetPortLocationByCountry(string name)
        {
            try
            {
                var entities = _context.PortLocations
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.Country.ToUpper() == name.ToUpper())
                    .Select(k => _mapper.Map<PortLocations, PortLocationViewModel>(k))
                    .ToList();
                return BaseResponse < List < PortLocationViewModel >>.Success( entities);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortLocationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }

        }

        #endregion
    }
}
