using YachtMerchant.Core.Models.YachtTourCategory;
using YachtMerchant.Infrastructure.Database;
using APIHelpers.Response;
using AutoMapper;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using YachtMerchant.Infrastructure.Database.Entities;
using System.Collections.Generic;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;

namespace YachtMerchant.Infrastructure.Services.YachtTour
{
    public class YachtTourCategoryService : IYachtTourCategoryService
    {
        private readonly IMapper _mapper;
        private readonly YachtOperatorDbContext _db;
        public YachtTourCategoryService(YachtOperatorDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public BaseResponse<List<YachtTourCategoryViewModel>> GetAll()
        {
            try
            {
                var result = _db.YachtTourCategories
                    .AsNoTracking()
                    .Where(k => !k.Deleted && k.IsActivated)
                    .Select(k => _mapper.Map<YachtTourCategories, YachtTourCategoryViewModel>(k))
                    .ToList();
                if(result != null)
                    return BaseResponse<List<YachtTourCategoryViewModel>>.Success(result);
                return BaseResponse<List<YachtTourCategoryViewModel>>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<List<YachtTourCategoryViewModel>>.InternalServerError(message:ex.Message);
            }
        }
    }
}
