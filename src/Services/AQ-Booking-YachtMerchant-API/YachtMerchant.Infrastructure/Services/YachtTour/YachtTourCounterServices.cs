using System;
using AutoMapper;
using System.Linq;
using Omu.ValueInjecter;
using APIHelpers.Response;
using AQBooking.Core.Helpers;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Core.Models.YachtTourCounters;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourCounterServices : IYachtTourCounterServices
    {
        private readonly IMapper _mapper;
        private readonly YachtOperatorDbContext _db;
        public YachtTourCounterServices(YachtOperatorDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public BaseResponse<bool> Create(YachtTourCounterCreateModel createModel)
        {
            try
            {
                if (createModel == null)
                    return BaseResponse<bool>.BadRequest();
                var entity = _mapper.Map<YachtTourCounterCreateModel, YachtTourCounters>(createModel);
                entity.YachtTourUniqueId = UniqueIDHelper.GenarateRandomString(12);
                _db.YachtTourCounters.Add(entity);
                var result = _db.SaveChanges();
                if(result > 0)
                    return BaseResponse<bool>.Success(true);
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtTourCounterViewModel> FindById(int id)
        {
            try
            {
                var result = _db.YachtTourCounters.Find(id);
                if (result != null)
                {
                    var model = _mapper.Map<YachtTourCounters, YachtTourCounterViewModel>(result);
                    return BaseResponse<YachtTourCounterViewModel>.Success(model);
                }
                return BaseResponse<YachtTourCounterViewModel>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtTourCounterViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Update(YachtTourCounterViewModel createModel)
        {
            try
            {
                if (createModel == null)
                    return BaseResponse<bool>.BadRequest();

                var entity = _db.YachtTourCounters.FirstOrDefault(x => x.YachtTourId.Equals(createModel.YachtTourId));
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();
                entity.InjectFrom(createModel);
                _db.YachtTourCounters.Update(entity);
                _db.SaveChangesAsync().Wait();
                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}