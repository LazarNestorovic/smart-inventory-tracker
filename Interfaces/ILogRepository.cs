using SmartInventoryTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Interfaces
{
    public interface ILogRepository
    {
        public void AddLog(Log log);

        public Log? GetLogById(int id);

        public List<Log> GetAllLogs();

        public void UpdateLog(Log log);

        public void DeleteLog(int id);
    }
}
