using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace LogsVaivoa.Services
{
    public class LogService
    {
        private readonly string _sqlConnection;
        private readonly ElasticsearchService _elasticService; 
        public LogService(ElasticsearchService elasticService)
        {
            _sqlConnection = Environment.GetEnvironmentVariable("SqlConnection");
            _elasticService = elasticService;
        }
        
        public async Task<(bool, object)> InsertLog(Log log)
        {
            var errors = log.GetErrors();
            if (errors.Any()) return (false, errors);

           await _elasticService.ElasticClient
                .IndexAsync(log, idx => idx.Index(Environment.GetEnvironmentVariable("IndexAI")));

          
            await using var db = DbConnection(_sqlConnection);
            await db.InsertAsync(log);

            return (true, log);
        }
        
        
        private static SqlConnection DbConnection(string connString) => 
            new SqlConnection(connString);
    }
}