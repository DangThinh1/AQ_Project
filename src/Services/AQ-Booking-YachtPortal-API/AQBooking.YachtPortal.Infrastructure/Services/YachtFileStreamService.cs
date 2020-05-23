using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AQEncrypts;
using AutoMapper;
using ExtendedUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtFileStreamService : IYachtFileStreamService
    {
        private readonly AQYachtContext _aqYachtContext;
        private readonly IMapper _mapper;
        public YachtFileStreamService(AQYachtContext aqYachtContext, IMapper mapper)
        {
            _aqYachtContext = aqYachtContext;
            _mapper = mapper;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtFileStreamViewModel>> GetFileStream(string yachtFId, int categoryFId)
        {
            try
            {
                var yachtIdde = Terminator.Decrypt(yachtFId).ToInt32();
                var result = (_aqYachtContext.YachtFileStreams
                    .Where(k => k.Deleted == false
                    && k.YachtFid == yachtIdde
                    && k.FileCategoryFid == categoryFId
                   && k.ActivatedDate <= DateTime.Now
                    )
                    .Select(i => _mapper.Map<YachtFileStreams, YachtFileStreamViewModel>(i))
                    );

                if (result != null)
                    return BaseResponse<List<YachtFileStreamViewModel>>.Success(result.ToList());
                else
                    return BaseResponse<List<YachtFileStreamViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtFileStreamViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<PagedList<YachtFileStreamViewModel>> GetFileStreamPaging(YachtFileStreamSearchModel searchModel)
        {
            try
            {
                
                var yachtIdde = Terminator.Decrypt(searchModel.YachtFId).ToInt32();
                var result = (_aqYachtContext.YachtFileStreams
                    .Where(k => k.Deleted == false
                    && k.YachtFid == yachtIdde
                    && k.FileTypeFid == searchModel.FileTypeFId
                    && k.ActivatedDate <= DateTime.Now
                    )
                    .Select(i => _mapper.Map<YachtFileStreams, YachtFileStreamViewModel>(i))
                    );

                if (result != null)
                    return BaseResponse<PagedList<YachtFileStreamViewModel>>.Success(new PagedList<YachtFileStreamViewModel>(result, searchModel.PageIndex, searchModel.PageSize));
                else
                    return BaseResponse<PagedList<YachtFileStreamViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtFileStreamViewModel>>.InternalServerError(new PagedList<YachtFileStreamViewModel>(Enumerable.Empty<YachtFileStreamViewModel>().AsQueryable(), searchModel.PageIndex, searchModel.PageSize), message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<string> Encrypt(int yachtFId)
        {
            try
            {
                var result = Terminator.Encrypt(yachtFId.ToString());

                if (result != null)
                    return BaseResponse<string>.Success(result);
                else
                    return BaseResponse<string>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
