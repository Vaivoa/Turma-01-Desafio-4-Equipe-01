using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using LogsVaivoa.Interface;
using LogsVaivoa.Models;
using Microsoft.Extensions.Logging;

namespace LogsVaivoa.repository
{
    public class LogRepository : ILogRepository
    {
        private readonly ILogger<LogRepository> _logger;
        private readonly SqlConnection _db;

        public LogRepository(ILogger<LogRepository> logger, IDbContext db)
        {
            _logger = logger;
            _db = db.GetDbConnection();
        }

        public async Task<bool> InsertLogDb(Log log)
        {
            try
            {
                await _db.InsertAsync(log);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}