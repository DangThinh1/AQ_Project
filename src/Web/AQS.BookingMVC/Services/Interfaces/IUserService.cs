using APIHelpers.Response;
using Identity.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces
{
    public interface IUserService
    {
        bool Resigster(UserCreateModel registerModel);
    }
}
