using AQBooking.Admin.Core.Models.AuthModel;
using AQBooking.Admin.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Helpers
{
    public interface IWorkContext
    {
        string UserToken { get; }
        string UserRoleName { get; }
        Guid UserGuid { get; }
        int UserRoleId { get; }
        string UserRoleGuidId { get; }
    }
}
