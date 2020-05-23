using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.YachtMerchantCharterFee;
using AQBooking.Admin.Infrastructure.Databases.YachtEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class YachtMerchantCharterFeeService : ServiceBase, IYachtMerchantCharterFeeService
    {
        #region Fields

        #endregion

        #region Ctor
        public YachtMerchantCharterFeeService(
            AQYachtContext dbContext,
            IWorkContext workContext,
            IMapper mapper
            ) : base(dbContext, workContext, mapper)
        {

        }
        #endregion

        #region Methods
        public List<YachtMerchantCharterFeeViewModel> GetAllYachtMerchantCharterFee()
        {
            try
            {
                var lstYachtMerchantCharterFee = _dbYachtContext.YachtMerchantCharterFeeOptions.AsNoTracking().Where(x => x.Deleted == false).Select(x => _mapper.Map<YachtMerchantCharterFeeViewModel>(x)).ToList();
                return lstYachtMerchantCharterFee ?? new List<YachtMerchantCharterFeeViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public List<YachtMerchantCharterFeeViewModel> GetYachtMerchantCharterFeeByMerchantId(int merchantId)
        {
            try
            {
                var listYachtMerchantCharterFee = _dbYachtContext.YachtMerchantCharterFeeOptions.AsNoTracking().Where(x => x.Deleted == false && x.MerchantFid == merchantId).Select(x => _mapper.Map<YachtMerchantCharterFeeViewModel>(x)).ToList();
                return listYachtMerchantCharterFee ?? new List<YachtMerchantCharterFeeViewModel>();
            }
            catch
            {
                throw;
            }
        }

        public YachtMerchantCharterFeeViewModel GetYachtMerchantCharterFeeById(int id)
        {
            try
            {
                var yachtMerchantCharterFee = _dbYachtContext.YachtMerchantCharterFeeOptions.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).Select(x => _mapper.Map<YachtMerchantCharterFeeViewModel>(x)).FirstOrDefault();
                return yachtMerchantCharterFee ?? new YachtMerchantCharterFeeViewModel();
            }
            catch
            {
                throw;
            }
        }

        public BasicResponse CreateYachtMerchantCharterFee(YahctMerchantCharterFeeCreateModel model)
        {
            try
            {
                var userId = _workContext.UserGuid;
                var entity = new YachtMerchantCharterFeeOptions();
                entity = _mapper.Map<YachtMerchantCharterFeeOptions>(model);
                entity.Deleted = false;
                entity.CreatedBy = userId;
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy = userId;
                entity.LastModifiedDate = DateTime.Now;

                _dbYachtContext.YachtMerchantCharterFeeOptions.Add(entity);
                _dbYachtContext.SaveChanges();

                return BasicResponse.Succeed("Create successfully!");
            }
            catch
            {
                throw;
            }
        }

        public BasicResponse UpdateYachtMerchantCharterFee(YachtMerchantCharterFeeUpdateModel model)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchantCharterFeeOptions.AsNoTracking().Where(x => x.Deleted == false && x.Id == model.Id).FirstOrDefault();
                if(entity != null)
                {
                    var userId = _workContext.UserGuid;   
                    entity = _mapper.Map<YachtMerchantCharterFeeUpdateModel, YachtMerchantCharterFeeOptions>(model, entity);
                    entity.LastModifiedBy = userId;
                    entity.LastModifiedDate = DateTime.Now;

                    _dbYachtContext.YachtMerchantCharterFeeOptions.Update(entity);
                    _dbYachtContext.SaveChanges();

                    return BasicResponse.Succeed("Update successfully!");
                }

                return BasicResponse.Failed("Update fail");
            }
            catch
            {
                throw;
            }
        }

        public BasicResponse DeleteYachtMerchantCharterFee(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchantCharterFeeOptions.AsNoTracking().Where(x => x.Deleted == false && x.Id == id).FirstOrDefault();
                if (entity != null)
                {
                    var userId = _workContext.UserGuid;
                    entity.Deleted = true;
                    entity.LastModifiedBy = userId;
                    entity.LastModifiedDate = DateTime.Now;
                    _dbYachtContext.YachtMerchantCharterFeeOptions.Update(entity);
                    _dbYachtContext.SaveChanges();

                    return BasicResponse.Succeed("Delete successfully!");
                }

                return BasicResponse.Failed("Delete fail");
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
