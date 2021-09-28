using System;
using System.Threading.Tasks;
using LogsVaivoa.Interface;
using LogsVaivoa.Models;
using Nest;

namespace LogsVaivoa.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private ElasticClient _elasticClient;

        public ElasticsearchService(string elasticsearchUrl)
        {
            var uriElastic = new Uri(elasticsearchUrl);
            var elasticsearchSettings = new ConnectionSettings(uriElastic);
            _elasticClient = new ElasticClient(elasticsearchSettings);
        }

        public async Task<bool> SendToElastic(Log log, string index)
        {
            var resultElastic = await _elasticClient
                .IndexAsync(log, idx => idx.Index(index));
            
            return resultElastic.IsValid;
        }
        
        public async Task<bool> SendToElastic(LogApplicationInsight log, string index)
        {
            var resultElastic = await _elasticClient
                .IndexAsync(log, idx => idx.Index(index));

            return resultElastic.IsValid;
        }
        
    }
    
}