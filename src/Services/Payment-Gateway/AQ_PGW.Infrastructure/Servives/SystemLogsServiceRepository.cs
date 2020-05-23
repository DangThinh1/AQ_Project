using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUnitOfWork = AQ_PGW.Infrastructure.Repositories.IUnitOfWork;

namespace AQ_PGW.Infrastructure.Servives
{
    public class SystemLogsServiceRepository : ISystemLogsServiceRepository
    {
        private IUnitOfWork _unitOfWork;

        public SystemLogsServiceRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<SystemLogs> GetSystemLogs()
        {
            var getTrans=  this._unitOfWork.Repository<SystemLogs>().GetAll();

            return getTrans;
        }
        public SystemLogs GetSystemLogsById(decimal id)
        {
            var getTran = this._unitOfWork.Repository<SystemLogs>().FirstOrDefault(x=> x.ID == id);

            return getTran;
        }

        public SystemLogs InsertSystemLogs(SystemLogs model)
        {
            _unitOfWork.Repository<SystemLogs>().Add(model);
            _unitOfWork.Save();

            return model;
        }

        public async Task<SystemLogs> InsertSystemLogsAsync(SystemLogs model)
        {
            await _unitOfWork.Repository<SystemLogs>().AddAsync(model);
            _unitOfWork.Save();

            return model;
        }

        public SystemLogs UpdateSystemLogs(SystemLogs model)
        {
            _unitOfWork.Repository<SystemLogs>().Update(model);
            _unitOfWork.Save();

            return model;
        }
    }
}
