using AQBooking.YachtPortal.Core.Models.YachtTourCategory;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtTourCategoryService : ServiceBase, IYachtTourCategoryService
    {
        #region Fields

        #endregion

        #region Ctor
        public YachtTourCategoryService(
            AQYachtContext yachtDbContext) : base(yachtDbContext)
        { }
        #endregion

        #region Methods
        public List<YachtTourCategoryViewModel> GetAllYachtTourCategory(int langId)
        {
            var query = from ytc in _yachtDbContext.YachtTourCategories.AsNoTracking().Where(x => x.Deleted == false)
                        join ytci in _yachtDbContext.YachtTourCategoryInfomations.AsNoTracking() on ytc.Id equals ytci.TourCategoryFid
                        where ytc.Deleted == false && ytci.LanguageFid == langId
                        select new YachtTourCategoryViewModel
                        {
                            DefaultName = ytc.DefaultName,
                            TourCategoryResourceKey = ytci.TourCategoryResourceKey,
                            ShortDescription = ytci.ShortDescription,
                            FullDescription = ytci.FullDescription,
                            FileStreamFid = ytc.FileStreamFid,
                            FileTypeFid = ytc.FileTypeFid
                        };
            if (query.Count() > 0)
                return query.ToList();
            return new List<YachtTourCategoryViewModel>();
        }
        #endregion
    }
}
