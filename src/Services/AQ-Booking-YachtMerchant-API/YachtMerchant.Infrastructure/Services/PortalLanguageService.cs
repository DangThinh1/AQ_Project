using System;
using System.Collections.Generic;
using System.Linq;
using ExtendedUtility;
using YachtMerchant.Core.DTO;
using Microsoft.EntityFrameworkCore;
using YachtMerchant.Infrastructure.Interfaces;
using YachtMerchant.Infrastructure.Database;

namespace YachtMerchant.Infrastructure.Services
{
    public class PortalLanguageService: ServiceBase, IPortalLanguageService
    {
        public PortalLanguageService(YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
        }

        public List<DTODropdownItem> GetAllLanguageSupportByPortalId(string Id)
        {
            try
            {
                if (String.IsNullOrEmpty(Id) || Id.Length == 0)
                    return new List<DTODropdownItem>();
                
                // Case Id input Int ( domain portal Fid )
                if (Id.Length < 2)
                {
                    var query = _context.PortalLanguageControls.AsNoTracking().Where(p => p.DomainPortalFid == Id.ToInt32()).Select(m => new DTODropdownItem { Value = m.LanguageFid.ToString(), Text = _context.CommonLanguages.FirstOrDefault(l => l.Id == m.LanguageFid).LanguageName }); ;

                    if (query.Count() > 0)
                        return query.ToList();
                    else
                        return new List<DTODropdownItem>();
                }
                // Case Id input string Domain portalUnique Fid
                else
                {
                    var query =  _context.PortalLanguageControls.AsNoTracking().Where(p => p.PortalUniqueId == Id).Select(m => new DTODropdownItem { Value = m.LanguageFid.ToString(), Text = _context.CommonLanguages.FirstOrDefault(l => l.Id == m.LanguageFid).LanguageName });
                    if (query.Count() > 0)
                        return query.ToList();
                    else
                        return new List<DTODropdownItem>();
                }
            }
            catch(Exception  ex)
            {
                return new List<DTODropdownItem>();
            }
            
        }
    }
}