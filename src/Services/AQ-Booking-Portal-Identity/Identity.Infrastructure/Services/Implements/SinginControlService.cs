using System;
using AutoMapper;
using System.Linq;
using APIHelpers.Response;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Database.Entities;
using Identity.Core.Portal.Models.SigninControls;
using Identity.Infrastructure.Services.Interfaces;

namespace Identity.Infrastructure.Services.Implements
{
    public class SinginControlService: IdentityServiceBase, ISinginControlService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public SinginControlService(IAccountService accountService, IMapper mapper, IdentityDbContext db, UserManager<Users> userManager) : base(db, userManager)
        {
            _mapper = mapper;
            _accountService = accountService;
        }

        public BaseResponse<List<SigninControlViewModel>> FindByDomainUid(string domainUid)
        {
            try
            {
                if (string.IsNullOrEmpty(domainUid))
                    return BaseResponse<List<SigninControlViewModel>>.BadRequest();
                var result = _db.SigninControls
                    .AsNoTracking()
                    .Where(k=> k.CurrentDomainUid == domainUid)
                    .Select(k=> _mapper.Map<SigninControls, SigninControlViewModel>(k))
                    .ToList();
                if(result != null)
                    return BaseResponse<List<SigninControlViewModel>>.Success(result);
                return BaseResponse<List<SigninControlViewModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<SigninControlViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}