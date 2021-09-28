using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using LogsVaivoa.Interface;
using LogsVaivoa.Models;
using Microsoft.Extensions.Logging;
using Nest;

namespace LogsVaivoa.Services
{
    public class LogService : ILogService
    {
        private static readonly string IndexLog = Environment.GetEnvironmentVariable("IndexLog");
        private readonly IElasticsearchService _elasticService;
        private readonly ILogger<LogService> _logger;
        private readonly SqlConnection _db;
        public LogService(IElasticsearchService elasticService, ILogger<LogService> logger, IDbContext dbContext)
        {
            _elasticService = elasticService;
            _logger = logger;
            _db = dbContext.GetDbConnection();
        }
        
        public async Task<(bool, object)> PostLog(Log log)
        {
            if (!log.IsValid) return (false, log.GetErrors());

            await _elasticService.SendToElastic(log, IndexLog);
          
            var resultDb= await InsertLogDb(log);

            return (resultDb, log);
        }

        private async Task<bool> InsertLogDb(Log log)
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