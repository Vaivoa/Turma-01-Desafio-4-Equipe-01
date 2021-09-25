using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using LogsVaivoa.Models;
using Microsoft.Extensions.Logging;
using Nest;

namespace LogsVaivoa.Services
{
    public class LogService
    {
        private readonly string _sqlConnection;
        private readonly ElasticsearchService _elasticService;
        private readonly ILogger<LogService> _logger;
        public LogService(ElasticsearchService elasticService, ILogger<LogService> logger)
        {
            _sqlConnection = Environment.GetEnvironmentVariable("SqlConnection");
            _elasticService = elasticService;
            _logger = logger;
        }
        
        public async Task<(bool, Object)> PostLog(Log log)
        {
            var errors = log.GetErrors();
            if (errors.Any()) return (false, errors);

            await SendLogElastic(log);
          
            var resultDb= await InsertLogDb(log);

            return (resultDb, log);
        }

        private async Task<bool> InsertLogDb(Log log)
        {
            await using var db = DbConnection(_sqlConnection);
            try
            {
                await db.InsertAsync(log);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        private async Task SendLogElastic(Log log)
        {
            var resultElastic = await _elasticService.ElasticClient
                .IndexAsync(log, idx => idx.Index(Environment.GetEnvironmentVariable("IndexAI")));

            if(resultElastic.Result == Result.Error)
                _logger.LogError("Falha ao enviar log para o elasticsearch");
        }
        
        
        private static SqlConnection DbConnection(string connString) => 
            new SqlConnection(connString);
    }
}