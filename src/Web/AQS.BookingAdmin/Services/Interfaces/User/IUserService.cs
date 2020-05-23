using APIHelpers.Response;
using AQBooking.Admin.Core.Models.AuthModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Interfaces.User
{
    public interface IUserService
    {
        Task<BaseResponse<List<UserInfoModel>>> GetAllUser();
    }
}
