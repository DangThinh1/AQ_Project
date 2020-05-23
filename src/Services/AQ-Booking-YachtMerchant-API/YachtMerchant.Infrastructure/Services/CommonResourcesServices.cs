using YachtMerchant.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using YachtMerchant.Infrastructure.Interfaces;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;

namespace YachtMerchant.Infrastructure.Services
{
    public class CommonResourcesServices : ServiceBase, ICommonResourcesPortalServices
    {
        private static Dictionary<int, Dictionary<string, string>> _resourceList;
        private string RESOURCE_CACHE_KEY = "LANGUAGE_RESOURCE_{0}";
        private int langIdPublic;
        private static string resourceType;
        public CommonResourcesServices(YachtOperatorDbContext dbcontext) : base(dbcontext)
        {

        }

        public async Task<List<CommonResources>> GetAllResource(int languageID, List<string> type = null)
        {
            List<string> typesearch = type.Count() > 0 ? type : new List<string>() { "COMMON" };
            //var res = await Task.Run(() => (from r in _context.CommonResources.AsNoTracking()
            //                                where (typesearch.Contains(r.TypeOfResource))
            //                                && r.LanguageFid == languageID
            //                                select r).ToList());
            return null;
        }

        public void LoadResource()
        {
            try
            {
                int languageID = langIdPublic != 0 ? langIdPublic : 1;
                if (_resourceList == null)
                    _resourceList = new Dictionary<int, Dictionary<string, string>>();
                if (!_resourceList.ContainsKey(languageID))
                    _resourceList[languageID] = new Dictionary<string, string>();
                if (_resourceList[languageID].Count == 0)
                {
                    _resourceList[languageID] = WebHelperCustomize.GetCache(string.Format(RESOURCE_CACHE_KEY, languageID), 10, () =>
                    {
                        return GetAllResource(languageID).Result.ToDictionary(x => x.ResourceKey, x => x.ResourceValue);
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetResource(string key)
        {
            LoadResource();
            if (_resourceList.ContainsKey(langIdPublic))
                if (_resourceList[langIdPublic].ContainsKey(key))
                    return _resourceList[langIdPublic][key];
            return key;
        }

        public void GetLangId(int langid, string type = null)
        {
            resourceType = type;
            langIdPublic = langid;
        }
    }
}
