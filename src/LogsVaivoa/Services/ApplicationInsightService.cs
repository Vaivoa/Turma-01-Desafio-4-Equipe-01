using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LogsVaivoa.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LogsVaivoa.Services
{
    public class ApplicationInsightService
    {
        private readonly ElasticsearchService _elasticService;
        private readonly string _urlApplicationInsights;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApplicationInsightService> _log;

        public ApplicationInsightService(ElasticsearchService elasticService, ILogger<ApplicationInsightService> log)
        {
            _urlApplicationInsights = Environment.GetEnvironmentVariable("UrlApplicationInsights");
            _elasticService = elasticService;
            _log = log;
            _httpClient = new HttpClient();
        }
        private ApplicationInsightResponse GetLogApp()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Add("x-api-key", Environment.GetEnvironmentVariable("ApiKey")!);
            
            var response = _httpClient.GetAsync(_urlApplicationInsights).Result;

            if (!response.IsSuccessStatusCode)
            {
                _log.LogError("Falha ao tentar realizar uma requisição no application insights");
            }

            return response.IsSuccessStatusCode switch
            {
                true => JsonConvert.DeserializeObject<ApplicationInsightResponse>(response.Content.ReadAsStringAsync().Result),
                _ => null
            };
        }

        public async Task SendMetricToElastic()
        {
            var result = GetLogApp();
            
            var log = result?.MapToLog();

            if (log != null)
            {
                var resultElastic = await _elasticService
                    .SendToElastic(log, Environment.GetEnvironmentVariable("IndexAI"));

                if (!resultElastic)
                    _log.LogError("Falha ao salvar metricas do aplication insight no elasticsearch");
            }
            else
                _log.LogError("Falha ao realizar conversão das métricas do aplication insight");
        }

    }
    
}