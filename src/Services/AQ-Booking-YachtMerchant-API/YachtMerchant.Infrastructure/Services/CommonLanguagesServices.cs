using YachtMerchant.Core.Models.CommonLanguagues;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YachtMerchant.Infrastructure.Interfaces;
using YachtMerchant.Infrastructure.Database;

namespace YachtMerchant.Infrastructure.Services
{
    public class CommonLanguagesServices : ServiceBase, ICommonLanguagesServices
    {
        public CommonLanguagesServices(YachtOperatorDbContext dbcontext) : base(dbcontext)
        {

        }

        public async Task<List<CommonLanguagesViewModel>> GetAllAsync()
        {
            var rs = _context.CommonLanguages.AsNoTracking().Where(k => k.Deleted == false).Select(k => new CommonLanguagesViewModel()
            {
                Id = k.Id,
                LanguageName = k.LanguageName
            });
            if (rs.Count() > 0)
                return await rs.ToListAsync();
            else
            {
                return new List<CommonLanguagesViewModel>();
            }
        }

        public string GetLanguageCommonValue(int languageId)
        {
            try
            {
                var query = from l in _context.CommonLanguages.AsNoTracking().Where(l => l.Deleted == false && l.Id == languageId)
                            join c in _context.CommonValues.AsNoTracking() on l.ResourceKey equals c.ResourceKey
                            select c.Text;
                return query.FirstOrDefault();
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
