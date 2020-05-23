using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Infrastructure.Helpers
{
    public interface IWorkContext
    {
        string GetToken { get; }
        string GetUserId { get; }
        int GetUserRoleId { get; }
        string GetUserRoleName { get; }
    }
}
