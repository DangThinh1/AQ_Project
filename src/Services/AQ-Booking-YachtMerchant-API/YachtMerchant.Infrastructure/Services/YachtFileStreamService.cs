using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQBooking.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using YachtMerchant.Core.Enum;
using YachtMerchant.Core.Models.YachtFileStreams;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtFileStreamService : ServiceBase, IYachtFileStreamService
    {
        #region Fields
        private readonly IMapper _mapper;
        #endregion

        #region Ctor
        public YachtFileStreamService(IMapper mapper, YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public BaseResponse<PagedList<YachtFileStreamViewModel>> SearchYachtGallery(YachtFileStreamSearchModel model)
        {
            try
            {
                string sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "LastModifiedDate DESC";
                var listFileType = Enum.GetValues(typeof(YachtImageTypeEnum)).Cast<YachtImageTypeEnum>().Select(x => new { Value = (int)x, Name = x.ToString() }).ToList();
                var result = (from f in _context.YachtFileStreams.AsNoTracking()
                              join t in listFileType on f.FileTypeFid equals t.Value
                              where f.YachtFid == model.YachtId
                              && f.Deleted == false
                              && (model.FileTypeFid == 0 || f.FileTypeFid == model.FileTypeFid)
                              && (model.FileCategoryFid == 0 || f.FileCategoryFid == model.FileCategoryFid)
                              select _mapper.Map<YachtFileStreams, YachtFileStreamViewModel>(f)).OrderBy(sortString).AsQueryable();

                return BaseResponse<PagedList<YachtFileStreamViewModel>>.Success( new PagedList<YachtFileStreamViewModel>(result, model.PageIndex, model.PageSize));
            }
            catch(Exception ex)
            {
                return BaseResponse<PagedList<YachtFileStreamViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<PagedList<YachtFileStreamViewModel>> SearchYachtFileStream(YachtFileStreamSearchModel model)
        {
            try
            {
                string sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "LastModifiedDate DESC";
                var result = (from f in _context.YachtFileStreams.AsNoTracking()
                              where f.YachtFid == model.YachtId
                              && f.Deleted == false
                              && (model.FileTypeFid == 0 || f.FileTypeFid == model.FileTypeFid)
                              && (model.FileCategoryFid == 0 || f.FileCategoryFid == model.FileCategoryFid)
                              select _mapper.Map<YachtFileStreams, YachtFileStreamViewModel>(f)).OrderBy(sortString).AsQueryable();

                return BaseResponse<PagedList<YachtFileStreamViewModel>>.Success( new PagedList<YachtFileStreamViewModel>(result, model.PageIndex, model.PageSize));
            }
            catch(Exception ex)
            {
                return BaseResponse<PagedList<YachtFileStreamViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<YachtFileStreamViewModel> GetYachtFileStreamById(int id)
        {
            try
            {
                var result = _context.YachtFileStreams.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).Select(x => _mapper.Map<YachtFileStreams, YachtFileStreamViewModel>(x)).FirstOrDefault();
                return BaseResponse<YachtFileStreamViewModel>.Success(result);
            }
            catch(Exception ex)
            {
                return BaseResponse<YachtFileStreamViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> CreateYachtFileStream(YachtFileStreamCreateModel model)
        {
            try
            {
                var entity = new YachtFileStreams();
                entity = _mapper.Map<YachtFileStreamCreateModel, YachtFileStreams>(model, entity);
                entity.Deleted = false;
                entity.ActivatedDate = model.ActivatedDate;
                entity.LastModifiedBy = GetUserGuidId();
                entity.LastModifiedDate = DateTime.Now;

                await _context.YachtFileStreams.AddAsync(entity);
                var result = await _context.SaveChangesAsync();
                if (result == 1)
                    return BaseResponse<bool>.Success(true);
                return BaseResponse<bool>.NoContent(false);
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> UpdateYachtFileStream(YachtFileStreamUpdateModel model)
        {
            try
            {
                var entity = _context.YachtFileStreams.AsNoTracking().Where(x => x.Id == model.id && x.Deleted == false).FirstOrDefault();
                if (entity != null)
                {
                    entity = _mapper.Map<YachtFileStreamUpdateModel, YachtFileStreams>(model, entity);
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _context.YachtFileStreams.Update(entity);
                    var result = await _context.SaveChangesAsync();
                    if (result == 1)
                        return BaseResponse<bool>.Success(true);
                    return BaseResponse<bool>.NoContent(false);
                }
                return BaseResponse<bool>.NotFound(false);
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message:ex.Message, fullMsg:ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> DeleteYachtFileStream(int id)
        {
            try
            {
                var entity = _context.YachtFileStreams.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).FirstOrDefault();
                if (entity != null)
                {
                    entity.Deleted = true;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;

                    _context.YachtFileStreams.Update(entity);
                    var result = await _context.SaveChangesAsync();
                    if (result == 1)
                        return BaseResponse<bool>.Success(true);
                    return BaseResponse<bool>.NoContent(false);
                }
                return BaseResponse<bool>.NotFound(false);
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion
    }
}
