using SmartInventoryTracker.Interfaces;
using SmartInventoryTracker.Models;
using SmartInventoryTracker.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly SQLiteSerializable<Log> _serializer;
        public LogRepository(string connectionString)
        {
            _serializer = new SQLiteSerializable<Log>(connectionString);
        }

        public void AddLog(Log log)
        {
            _serializer.Insert(new List<Log> { log });
        }

        public Log? GetLogById(int id)
        {
            return _serializer.GetById(id);
        }

        public List<Log> GetAllLogs()
        {
            return _serializer.GetAll();
        }

        public void UpdateLog(Log log)
        {
            _serializer.Update(log);
        }

        public void DeleteLog(int id)
        {
            _serializer.Delete(id);
        }
    }
}
