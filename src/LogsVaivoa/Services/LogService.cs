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
        private readonly ILogRepository _logRepository;
        public LogService(IElasticsearchService elasticService, ILogger<LogService> logger, IDbContext dbContext, ILogRepository logRepository)
        {
            _elasticService = elasticService;
            _logger = logger;
            _logRepository = logRepository;
        }

        public async Task<(bool, object)> PostLog(Log log)
        {
            if (!log.IsValid()) return (false, log.GetErrors());

            await _elasticService.SendToElastic(log, IndexLog);

            await _logRepository.InsertLogDb(log);

            return (true, log);
        }
    }
}