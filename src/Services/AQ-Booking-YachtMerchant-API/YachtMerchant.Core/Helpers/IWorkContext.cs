
using System;

namespace YachtMerchant.Core.Helpers
{
    public interface IWorkContext
    {
        string UserToken { get; }
        string UserRole { get; }
        Guid UserGuid { get; }
    }
}
