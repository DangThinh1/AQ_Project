using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtAttributeValues;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AQEncrypts;
using ExtendedUtility;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtAttributeValueService : IYachtAttributevalueService
    {
        private DateTime _NOW_DATE { get => DateTime.Now; }
        private readonly AQYachtContext _AQYachtContext;
        public YachtAttributeValueService(AQYachtContext AQYachtContext)
        {
            _AQYachtContext = AQYachtContext;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtAttributeValueViewModel>> GetAttributesCharterPrivate(string yachtFId, int categoryFId, bool isInclude, List<string> attributeName)
        {
            try
            {
                var yachtFIdde = Terminator.Decrypt(yachtFId).ToInt32();
                var result = (from k in _AQYachtContext.YachtAttributeValues
                              join c in _AQYachtContext.YachtAttributes on k.AttributeFid equals c.Id
                              where k.YachtFid == yachtFIdde
                                  && (k.EffectiveDate == null || (k.EffectiveDate != null && k.EffectiveDate >= DateTime.Now.Date))
                                  && (categoryFId == 0 || k.AttributeCategoryFid == categoryFId)
                                  && (attributeName.Count == 0 || isInclude == false || attributeName.Contains(c.AttributeName))
                                  && (attributeName.Count == 0 || isInclude == true || !attributeName.Contains(c.AttributeName))
                              select new YachtAttributeValueViewModel
                              {
                                  AttributeFid = c.Id,
                                  AttributeCategoryFid = c.AttributeCategoryFid,
                                  AttributeName = c.AttributeName,
                                  IconCssClass = c.IconCssClass,
                                  Remarks = c.Remarks,
                                  ResourceKey = c.ResourceKey,
                                  AttributeValue = k.AttributeValue
                              });
                if (result != null)
                    return BaseResponse<List<YachtAttributeValueViewModel>>.Success(result.ToList());
                else
                    return BaseResponse<List<YachtAttributeValueViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtAttributeValueViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel> GetAttributesCharterPrivateGeneral(string yachtFId, int categoryFId, bool isInclude, List<string> attributeName)
        {
            try
            {
                var yachtFIdde = Terminator.Decrypt(yachtFId).ToInt32();
                var result = (from x in _AQYachtContext.Yachts
                              let attribute = (from v in _AQYachtContext.YachtAttributeValues
                                               join a in _AQYachtContext.YachtAttributes
                                               on v.AttributeFid equals a.Id
                                               where
                                                (v.EffectiveDate == null || (v.EffectiveDate != null && v.EffectiveDate >= DateTime.Now.Date))
                                               && (categoryFId == 0 || v.AttributeCategoryFid == categoryFId)
                                               && (attributeName.Count == 0 || isInclude == false || attributeName.Contains(a.AttributeName))// isInclude==false -->in AttributeName
                                               && (attributeName.Count == 0 || isInclude == true || !attributeName.Contains(a.AttributeName)) // isInclude==true --> not in AttributeName
                                               && v.YachtFid == x.Id
                                               && a.Deleted == false
                                               select new YachtAttributeValueViewModel
                                               {
                                                   AttributeFid = a.Id,
                                                   AttributeCategoryFid = v.AttributeCategoryFid,
                                                   AttributeName = a.AttributeName,
                                                   IconCssClass = a.IconCssClass,
                                                   Remarks = a.Remarks,
                                                   ResourceKey = a.ResourceKey,
                                                   AttributeValue = v.AttributeValue
                                               }
                                  )
                              where x.Deleted == false
                                    && x.ActiveForOperation == true
                                    && x.Id == yachtFIdde
                              select new YachtAttributeValueCharterPrivateGeneralViewModel
                              {
                                  ID = x.Id,
                                  Name = x.Name,
                                  City = x.City,
                                  Country = x.Country,
                                  LengthMeters = x.LengthMeters.GetValueOrDefault(),
                                  Cabins = x.Cabins,
                                  MaxPassenger = x.MaxPassengers,
                                  OvernightPassengers = x.OvernightPassengers,
                                  MaxSpeed = x.MaxSpeed.GetValueOrDefault(),
                                  CharterTypeFid = x.CharterTypeFid,
                                  CharterTypeReskey = x.CharterCategoryResKey,
                                  EngineGenerators = x.EngineGenerators,
                                  BeamMeters = x.BeamMeters.Value,
                                  AttributeValues = attribute.Select(x => x).ToList()
                              });
                if (result != null)
                    return BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel>.Success(result.FirstOrDefault());
                else
                    return BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }


        //*****modified by tuan tran 02-12-2019
        //*****next modified by 
        public BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel> GetAttributesCharterPrivateGeneral2(string yachtFId, int categoryFId, bool isInclude, List<string> attributeName)
        {
            try
            {
                var yachtFIdde = Terminator.Decrypt(yachtFId).ToInt32();
                var result = (from x in _AQYachtContext.Yachts
                              let attribute = (from v in _AQYachtContext.YachtAttributeValues
                                               join a in _AQYachtContext.YachtAttributes
                                               on v.AttributeFid equals a.Id
                                               where
                                                (v.EffectiveDate == null || (v.EffectiveDate != null && v.EffectiveDate >= DateTime.Now.Date))
                                               && (categoryFId == 0 || v.AttributeCategoryFid == categoryFId)
                                               && (attributeName.Count == 0 || isInclude == false || attributeName.Contains(a.AttributeName))// isInclude==false -->in AttributeName
                                               && (attributeName.Count == 0 || isInclude == true || !attributeName.Contains(a.AttributeName)) // isInclude==true --> not in AttributeName
                                               && v.YachtFid == x.Id
                                               && a.Deleted == false
                                               select new YachtAttributeValueViewModel
                                               {
                                                   AttributeFid = a.Id,
                                                   AttributeCategoryFid = v.AttributeCategoryFid,
                                                   AttributeName = a.AttributeName,
                                                   IconCssClass = a.IconCssClass,
                                                   Remarks = a.Remarks,
                                                   ResourceKey = a.ResourceKey,
                                                   AttributeValue = v.AttributeValue
                                               }
                                  )
                              where x.Deleted == false
                                    && x.Id == yachtFIdde
                              select new YachtAttributeValueCharterPrivateGeneralViewModel
                              {
                                  ID = x.Id,
                                  Name = x.Name,
                                  City = x.City,
                                  Country = x.Country,
                                  LengthMeters = x.LengthMeters.GetValueOrDefault(),
                                  Cabins = x.Cabins,
                                  MaxPassenger = x.MaxPassengers,
                                  OvernightPassengers = x.OvernightPassengers,
                                  MaxSpeed = x.MaxSpeed.GetValueOrDefault(),
                                  CharterTypeFid = x.CharterTypeFid,
                                  CharterTypeReskey = x.CharterCategoryResKey,
                                  EngineGenerators = x.EngineGenerators,
                                  BeamMeters = x.BeamMeters.Value,
                                  AttributeValues = attribute.Select(x => x).ToList()
                              });
                if (result != null)
                    return BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel>.Success(result.FirstOrDefault());
                else
                    return BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
