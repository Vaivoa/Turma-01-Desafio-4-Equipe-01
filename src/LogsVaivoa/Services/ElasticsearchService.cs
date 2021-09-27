using System;
using Nest;

namespace LogsVaivoa.Services
{
    public class ElasticsearchService
    {
        private readonly ConnectionSettings _elasticsearchSettings; 
        public ElasticClient ElasticClient => new ElasticClient(_elasticsearchSettings);

        public ElasticsearchService(string elasticsearchUrl)
        {
            _elasticsearchSettings = new ConnectionSettings(new Uri(elasticsearchUrl));
        }
        
    }
}