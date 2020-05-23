using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Core.Models.MembershipPrivileges;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class MembershipPrivilegeService : ServiceBase, IMembershipPrivilegeService
    {
        public MembershipPrivilegeService(AQConfigContext dbConfigContext, 
            IWorkContext workContext,
            IMapper mapper) : base(dbConfigContext, workContext, mapper)
        {
        }

        public async Task<BasicResponse> CreateOrUpdateMembershipPrivileges(MembershipPrivilegesCreateModel model)
        {
            try
            {
                var check = CheckValidMembershipPrivileges(model);
                if (check)
                {
                    var entity = _dbConfigContext.AqmembershipDiscountPrivileges.FirstOrDefault(x => x.Id == model.ID);
                    if (entity != null)
                    {
                        entity.Name = model.Name;
                        entity.Remark = model.Remark;
                        entity.EffectiveDate = model.EffectiveDate;
                        entity.EffectiveEndDate = model.EffectiveEndDate;
                        entity.LastModifiedBy = GetCurrentUserId();
                        entity.LastModifiedDate = DateTime.Now;
                        entity.RoleFid = model.RoleFID;
                        var details = _dbConfigContext.AqmembershipDiscountPrivilegeDetails.Where(x => x.PrivilegeFid == model.ID).ToList();
                        foreach (var i in details)
                        {
                            i.LastModifiedBy = GetCurrentUserId();
                            i.LastModifiedDate = DateTime.Now;
                            i.DiscountPercent = model.DataDetail.Find(x => x.ID == i.Id).DiscountPercent.GetValueOrDefault();
                        }
                        _dbConfigContext.AqmembershipDiscountPrivileges.Update(entity);
                        _dbConfigContext.AqmembershipDiscountPrivilegeDetails.UpdateRange(details);

                        UpdateOldMembershipPrivileges(entity);
                        _dbConfigContext.AqmembershipDiscountPrivileges.UpdateRange(entity);
                    }
                    else
                    {
                        var newMF = _mapper.Map<MembershipPrivilegesCreateModel, AqmembershipDiscountPrivileges>(model);
                        newMF.CreatedBy = GetCurrentUserId();
                        newMF.CreatedDate = DateTime.Now;
                        newMF.LastModifiedBy = GetCurrentUserId();
                        newMF.LastModifiedDate = DateTime.Now;
                        _dbConfigContext.AqmembershipDiscountPrivileges.Add(newMF);
                        await _dbConfigContext.SaveChangesAsync();
                        var newDetails = _mapper.Map<List<MembershipPrivilegesDetailCreateModel>, List<AqmembershipDiscountPrivilegeDetails>>(model.DataDetail);
                        foreach (var i in newDetails)
                        {
                            i.PrivilegeFid = newMF.Id;
                            i.LastModifiedBy = GetCurrentUserId();
                            i.LastModifiedDate = DateTime.Now;
                        }
                        _dbConfigContext.AqmembershipDiscountPrivilegeDetails.AddRange(newDetails);
                        UpdateOldMembershipPrivileges(newMF);
                    }
                    await _dbConfigContext.SaveChangesAsync();
                    return BasicResponse.Succeed("Success");
                }
                else
                {
                   return BasicResponse.Failed("Effective Date Invalid!"); 
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task<BasicResponse> DeleteMembershipPrivileges(int id)
        {
            using (var transaction = _dbConfigContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = _dbConfigContext.AqmembershipDiscountPrivileges.FirstOrDefault(x => x.Id == id);
                    if (entity != null)
                        entity.Deleted = true;
                    var result = _dbConfigContext.AqmembershipDiscountPrivileges.Update(entity);
                    await _dbConfigContext.SaveChangesAsync();
                    transaction.Commit();
                    transaction.Dispose();
                    return BasicResponse.Succeed("Success");
                }
                catch
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw;
                }
            }
        }

        public MembershipPrivilegesCreateModel GetMembershipPrivilegesById(int id)
        {
            var data = new MembershipPrivilegesCreateModel();
            try
            {
                var privilege = _dbConfigContext.AqmembershipDiscountPrivileges.SingleOrDefault(x => x.Id == id);
                if (id != 0 && privilege == null)
                    return null;
                else
                {
                    if (id == 0)
                    {
                        data = new MembershipPrivilegesCreateModel
                        {
                            EffectiveDate = DateTime.Now,
                            EffectiveEndDate = DateTime.Now.AddYears(1)
                        };
                    }
                    else data = _mapper.Map<AqmembershipDiscountPrivileges, MembershipPrivilegesCreateModel>(privilege);
                    var listBooking = _dbConfigContext.CommonValues.Where(x=>x.ValueGroup == "BOOKINGPORTALDOMAIN").ToList();
                    var privilegeDetails = _dbConfigContext.AqmembershipDiscountPrivilegeDetails.Where(x => x.PrivilegeFid == id).ToList();
                    data.DataDetail = listBooking.GroupJoin(privilegeDetails.Where(c => c.PrivilegeFid == id), x => x.UniqueId, y => y.BookingDomainUniqueId,
                        (x, y) => new { Booking = x, Privilege = y }).SelectMany(x=>x.Privilege.DefaultIfEmpty(),(x,y)=>new {b=x.Booking,p=y }).Select(a => new MembershipPrivilegesDetailCreateModel()
                        {
                            ID = a.p?.Id,
                            BookingDomainFID = a.b.Id,
                            BookingDomainName = a.b.Text,
                            BookingDomainUniqueID = a.b.UniqueId,
                            DiscountPercent = a.p?.DiscountPercent,
                            PrivilegeFID = a.p?.PrivilegeFid,
                            Remark = a.p?.Remark
                        }).ToList();
                    return data;
                }
            }
            catch
            {
                throw;
            }
        }

        public IPagedList<MembershipPrivilegesViewModel> SearchMembershipPrivileges(MembershipPrivilegesSearchModel searchModel)
        {
            try
            {
                //var sortString = !string.IsNullOrEmpty(searchModel.SortString) ? model.SortString : "CreatedDate DESC";
                var query = _dbConfigContext.AqmembershipDiscountPrivileges.Where(x =>
                !x.Deleted
                && (string.IsNullOrEmpty(searchModel.Name) || x.Name.ToLower().Contains(searchModel.Name.ToLower()))
                && (searchModel.EffectiveDate == null || (x.EffectiveDate >= searchModel.EffectiveDate && (x.EffectiveEndDate <= searchModel.EffectiveDate || x.EffectiveEndDate == null)))
                && (searchModel.Role == null || x.RoleFid == searchModel.Role)).OrderByDescending(a => a.EffectiveDate)
                .Select(c => new MembershipPrivilegesViewModel
                {
                    ID = c.Id,
                    Name = c.Name,
                    EffectiveDate = c.EffectiveDate,
                    EffectiveEndDate = c.EffectiveEndDate
                });
                return new PagedList<MembershipPrivilegesViewModel>(query, searchModel.PageIndex, searchModel.PageSize);
            }
            catch
            {
                throw;
            }
        }

        void UpdateOldMembershipPrivileges(AqmembershipDiscountPrivileges model)
        {          
            var oldEntities = _dbConfigContext.AqmembershipDiscountPrivileges.Where(x => x.Id != model.Id && x.RoleFid == model.RoleFid && x.EffectiveEndDate == null);
            foreach (var i in oldEntities)
                i.EffectiveEndDate = model.EffectiveDate.AddDays(-1);
            _dbConfigContext.AqmembershipDiscountPrivileges.UpdateRange(oldEntities);
        }
        bool CheckValidMembershipPrivileges(MembershipPrivilegesCreateModel model)
        {
            IQueryable<AqmembershipDiscountPrivileges> entities;
            if (model.ID > 0)
                entities = _dbConfigContext.AqmembershipDiscountPrivileges.Where(x =>!x.Deleted && x.Id != model.ID && x.EffectiveEndDate != null
               && x.EffectiveDate <= model.EffectiveDate && x.EffectiveEndDate >= model.EffectiveDate);
            else
                entities = _dbConfigContext.AqmembershipDiscountPrivileges.Where(x =>!x.Deleted && x.EffectiveEndDate != null
            && x.EffectiveDate <= model.EffectiveDate && x.EffectiveEndDate >= model.EffectiveDate);
            if (entities.Count() > 0)
                return false;
            return true;
        }
    }
}
