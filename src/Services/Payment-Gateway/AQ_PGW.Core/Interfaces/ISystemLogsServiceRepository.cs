using AQ_PGW.Core.Models.DBTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ_PGW.Core.Interfaces
{
    public interface ISystemLogsServiceRepository
    {
        SystemLogs InsertSystemLogs(SystemLogs model);
        Task<SystemLogs> InsertSystemLogsAsync(SystemLogs model);
        SystemLogs UpdateSystemLogs(SystemLogs model);
        SystemLogs GetSystemLogsById(decimal id);
        IEnumerable<SystemLogs> GetSystemLogs();
    }
}
