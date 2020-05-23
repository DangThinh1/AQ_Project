using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.YachtMerchantFileStream;
using AQBooking.Admin.Infrastructure.Databases.YachtEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class YachtMerchantFileStreamService : ServiceBase, IYachtMerchantFileStreamService
    {
        #region Fields
        #endregion

        #region Ctor
        public YachtMerchantFileStreamService(
            AQYachtContext dbContext,
            IWorkContext workContext,
            IMapper mapper
        ) : base(dbContext, workContext, mapper)
        {

        }
        #endregion

        #region Methods
        public List<YachtMerchantFileStreamViewModel> GetAllYachtMerchantFileStream()
        {
            try
            {
                var lstMerchantFileStream = _dbYachtContext.YachtMerchantFileStreams.AsNoTracking().Where(x => x.Deleted == false).Select(x => _mapper.Map<YachtMerchantFileStreamViewModel>(x)).ToList();
                return lstMerchantFileStream ?? new List<YachtMerchantFileStreamViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public YachtMerchantFileStreamViewModel GetYachtMerchantFileStreamById(int id)
        {
            try
            {
                var result = _dbYachtContext.YachtMerchantFileStreams.AsNoTracking().Where(x => x.Deleted == false && x.Id == id).Select(x => _mapper.Map<YachtMerchantFileStreamViewModel>(x)).FirstOrDefault();
                return result; 
            }
            catch
            {
                throw;
            }
        }

        public List<YachtMerchantFileStreamViewModel> GetYachtMerchantFileStreamByType(int fileType)
        {
            try
            {
                var result = _dbYachtContext.YachtMerchantFileStreams.AsNoTracking().Where(x => x.Deleted == false && x.FileTypeFid == fileType).Select(x => _mapper.Map<YachtMerchantFileStreamViewModel>(x)).ToList();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public BasicResponse CreateYachtMerchantFileStream(YachtMerchantFileStreamCreateModel model)
        {
            try
            {
                var userId = _workContext.UserGuid;
                var entity = new YachtMerchantFileStreams();
                entity = _mapper.Map<YachtMerchantFileStreams>(model);
                entity.Deleted = false;
                entity.LastModifiedBy = userId;
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.YachtMerchantFileStreams.Add(entity);
                _dbYachtContext.SaveChanges();
                return BasicResponse.Succeed("Create successfully!");
            }
            catch
            {
                throw;
            }
        }

        public BasicResponse UpdateYachtMerchantFileStream(YachtMerchantFileStreamUpdateModel model)
        {
            try
            {
                var userId = _workContext.UserGuid;
                var entity = _dbYachtContext.YachtMerchantFileStreams.AsNoTracking().Where(x => x.Id == model.Id && x.Deleted == false).FirstOrDefault();
                entity = _mapper.Map<YachtMerchantFileStreamUpdateModel, YachtMerchantFileStreams>(model, entity);
                entity.LastModifiedBy = userId;
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.YachtMerchantFileStreams.Update(entity);
                _dbYachtContext.SaveChanges();

                return BasicResponse.Succeed("Update successfully!");
            }
            catch
            {
                throw;
            }
        }

        public BasicResponse DeleteYachtMerchantFileStream(int id)
        {
            try
            {
                var userId = _workContext.UserGuid;
                var entity = _dbYachtContext.YachtMerchantFileStreams.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).FirstOrDefault();
                entity.Deleted = true;
                entity.LastModifiedBy = userId;
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.YachtMerchantFileStreams.Update(entity);
                _dbYachtContext.SaveChanges();

                return BasicResponse.Succeed("Delete successfully!");
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
