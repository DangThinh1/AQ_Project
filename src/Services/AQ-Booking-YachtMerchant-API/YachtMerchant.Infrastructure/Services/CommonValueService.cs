using System.Collections.Generic;
using System.Linq;
using YachtMerchant.Core.Enum;
using ExtendedUtility;
using System.Linq.Dynamic.Core;
using YachtMerchant.Infrastructure.Interfaces;
using YachtMerchant.Core.Models.CommonValues;
using System;
using AutoMapper;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;

namespace YachtMerchant.Infrastructure.Services
{
    public class CommonValueService : ServiceBase, ICommonValueService
    {
        private readonly IMapper _mapper;

        public CommonValueService(IMapper mapper, YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
            _mapper = mapper;
        }

        public List<CommonValueViewModel> GetListCommonValueByGroup(string valueGroup, SortTypeEnum sortType = SortTypeEnum.Ascending)
        {
            try
            {
                return _context.CommonValues
                    .Where(e => valueGroup.Equals(e.ValueGroup))
                    .Select(k=>_mapper.Map<CommonValues, CommonValueViewModel>(k))
                    .OrderBy("OrderBy " + sortType.ToString())                    
                    .ToList();
            }
            catch(Exception ex)
            {

                return new List<CommonValueViewModel>();
            }
        }

        public CommonValueViewModel GetCommonValueByGroupDouble(string valueGroup, double valueDouble)
        {
            try
            {
                if (!valueGroup.HaveValue())
                    return new CommonValueViewModel();

                var entity = _context.CommonValues
                    .FirstOrDefault(e => valueGroup.ToUpper().Equals(e.ValueGroup.ToUpper()) && e.ValueDouble == valueDouble);
                 var viewmodel = _mapper.Map<CommonValues, CommonValueViewModel>(entity);
                return viewmodel;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public CommonValueViewModel GetCommonValueByGroupInt(string valueGroup, int valueInt)
        {
            try
            {
                if (!valueGroup.HaveValue())
                    return new CommonValueViewModel();

                var entity = _context.CommonValues
                    .FirstOrDefault(e=> valueGroup.ToUpper().Equals(e.ValueGroup.ToUpper()) && e.ValueInt == valueInt);

                var viewmodel = _mapper.Map<CommonValues, CommonValueViewModel>(entity);
                return viewmodel;
            }
            catch(Exception ex)
            {
                return new CommonValueViewModel();
            }
        }

        public CommonValueViewModel GetCommonValueByGroupString(string valueGroup, string valueString)
        {
            try
            {
                if (!valueGroup.HaveValue() || !valueString.HaveValue())
                    return new CommonValueViewModel();

                var entity = _context.CommonValues
                    .FirstOrDefault(e => valueGroup.ToUpper().Equals(e.ValueGroup.ToUpper()) && e.ValueString == valueString);

                var viewmodel = _mapper.Map<CommonValues, CommonValueViewModel>(entity);
                return viewmodel;
            }
            catch(Exception ex)
            {
                return new CommonValueViewModel();
            }
        }

        public List<CommonValueViewModel> GetAllCommonValue()
        {
            try
            {
                return _context.CommonValues.Select(k=> _mapper.Map<CommonValues, CommonValueViewModel>(k)).ToList();
            }
            catch(Exception ex)
            {
                return new List<CommonValueViewModel>();
            }
        }

        #region Async Method

        public CommonValueViewModel GetCommonValueByGroupStringAsync(string valueGroup, string valueString)
        {
            return GetCommonValueByGroupString(valueGroup, valueString);
        }

        public CommonValueViewModel GetCommonValueByGroupIntAsync(string valueGroup, int valueInt)
        {
            return  GetCommonValueByGroupInt(valueGroup, valueInt);
        }

        public CommonValueViewModel GetCommonValueByGroupDoubleAsync(string valueGroup, double valueDouble)
        {
            return GetCommonValueByGroupDouble(valueGroup, valueDouble);
        }

        public List<CommonValueViewModel> GetListCommonValueByGroupAsync(string valueGroup, SortTypeEnum sortType= SortTypeEnum.Descending)
        {
            return GetListCommonValueByGroup(valueGroup, sortType);
        }

        public List<CommonValueViewModel> GetAllCommonValueAsync()
        {
            return  GetAllCommonValue();
        }

        #endregion
    }
}
