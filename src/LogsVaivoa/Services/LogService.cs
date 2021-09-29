using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using LogsVaivoa.Interface;
using LogsVaivoa.Models;
using Microsoft.Extensions.Logging;

namespace LogsVaivoa.Services
{
    public class LogService : ILogService
    {
        private static readonly string IndexLog = Environment.GetEnvironmentVariable("IndexLog");
        private readonly IElasticsearchService _elasticService;
        private readonly ILogger<LogService> _logger;
        private readonly IDbContext _dbContext;
        public LogService(IElasticsearchService elasticService, ILogger<LogService> logger, IDbContext dbContext)
        {
            _elasticService = elasticService;
            _logger = logger;
            _dbContext = dbContext;
        }
        
        public async Task<(bool, object)> PostLog(Log log)
        {
            if (!log.IsValid()) return (false, log.GetErrors());

            await _elasticService.SendToElastic(log, IndexLog);
          
            InsertLogDb(log);

            return (true, log);
        }

        private void InsertLogDb(Log log)
        {
            using var db = _dbContext.GetDbConnection();
            try
            { 
                db.Insert(log);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

    }
}