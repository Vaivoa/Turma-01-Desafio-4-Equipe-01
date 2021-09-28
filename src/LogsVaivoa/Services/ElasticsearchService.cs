using System;
using System.Threading.Tasks;
using LogsVaivoa.Interface;
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

        public async Task<bool> SendToElastic<T>(T log, string index) where T : class
        {
            var resultElastic = await _elasticClient
                .IndexAsync(log, idx => idx.Index(index));
            
            return resultElastic.IsValid;
        }
        
        
    }
    
}