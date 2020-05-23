using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.CommonValue;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class CommonValueService : ICommonValueService
    {
        private readonly IWorkContext _workContext;
        private readonly IMapper _mapper;
        private readonly AQConfigContext _dbConfigContext;
        public CommonValueService(
            IWorkContext workContext, 
            IMapper mapper, 
            AQConfigContext aqcontext)
        {
            _workContext = workContext;
            _mapper = mapper;
            _dbConfigContext = aqcontext;
        }

        public Task<List<CommonValueViewModel>> GetAllByValueDoubleAsync(string valueGroup, double valueDouble)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<CommonValueViewModel>> GetAllByValueIntAsync(string valueGroup, int valueInt)
        {
            try
            {
                return null;
            }
            catch
            {
                throw;
            }
        }

        public Task<List<CommonValueViewModel>> GetAllByValueStringAsync(string valueGroup, string valueString)
        {
            throw new System.NotImplementedException();
        }

        public ApiActionResult CreateNewCommonValues(CommonValueCreateModel model)
        {
            try
            {
                CommonValues obj = new CommonValues();
                obj = _mapper.Map<CommonValueCreateModel, CommonValues>(model);
                obj.UniqueId = UniqueIDHelper.GenerateRandomString(12);
                _dbConfigContext.CommonValues.Add(obj);
                var flag = _dbConfigContext.SaveChanges();

                if (flag != 0)
                    return ApiActionResult.Success();
                return ApiActionResult.Failed("Created Failed");
            }
            catch (Exception ex)
            {
                return ApiActionResult.Failed(ex.Message);
            }
        }

        public Task<bool> DeleteCommonValues(int Id)
        {
            throw new NotImplementedException();
        }

        public List<CommonValues> GetAllCommonValues()
        {
            var res = _dbConfigContext.CommonValues.AsNoTracking().ToList();
            if (res != null)
                return res;
            else
                return new List<CommonValues>();
        }

        public IPagedList<CommonValueViewModel> GetAllCommonValuesPaging(CommonValueSearchModel model)
        {
            string sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "ValueGroup ASC";

            var result = (from value in _dbConfigContext.CommonValues.AsNoTracking()
                          where (value.ValueGroup.Contains(model.ValueGroup) || string.IsNullOrEmpty(model.ValueGroup))
                          && (string.IsNullOrEmpty(model.Text) || value.Text.Contains(model.Text))
                          && (value.ValueString.Contains(model.ValueString) || string.IsNullOrEmpty(model.ValueString))
                          select _mapper.Map<CommonValues, CommonValueViewModel>(value)
                          ).OrderBy(sortString).AsQueryable();



            return new PagedList<CommonValueViewModel>(result, model.PageIndex, model.PageSize);
        }

        public CommonValues GetById(int Id) => _dbConfigContext.CommonValues.AsNoTracking().FirstOrDefault(x => x.Id == Id);

        public ApiActionResult UpdateCommonValues(CommonValueUpdateModel model)
        {
            try
            {
                CommonValues obj = new CommonValues();
                obj = _dbConfigContext.CommonValues.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);
                if (obj != null)
                {
                    obj.ValueDouble = model.ValueDouble;
                    obj.ValueGroup = model.ValueGroup;
                    obj.ValueInt = model.ValueInt;
                    obj.ValueString = model.ValueString;
                    obj.Text = model.Text;
                    obj.OrderBy = model.OrderBy;
                    var resUpdate = _dbConfigContext.CommonValues.Update(obj);
                    int flag = _dbConfigContext.SaveChanges();
                    if (flag != 0)
                        return ApiActionResult.Success();
                    return ApiActionResult.Failed("Updated failed");
                }
                return ApiActionResult.Failed("Can not updated because entity doest not existed!!");
            }
            catch (Exception ex)
            {
                return ApiActionResult.Failed(ex.Message);
            }
        }

        public List<string> GetValueGroupDDL()
        {
            var result = _dbConfigContext.CommonValues.AsNoTracking().GroupBy(x => x.ValueGroup).Select(x => x.Key).ToList();
            if (result != null)
                return result;
            return new List<string>();
        }

        public List<CommonValues> GetAllYachtAttributeCategory()
        {
            var res = _dbConfigContext.CommonValues.Where(x=>x.ValueGroup == "YACHTATT").ToList();
            if (res != null)
                return res;
            else
                return new List<CommonValues>();
        }
    }
}