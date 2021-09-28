using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LogsVaivoa.Interface;
using LogsVaivoa.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LogsVaivoa.Services
{
    public class ApplicationInsightService : IApplicationInsightService
    {
        private static readonly string UrlApplicationInsights = Environment.GetEnvironmentVariable("UrlApplicationInsights");
        private static readonly string IndexAI = Environment.GetEnvironmentVariable("IndexAI");
        private readonly IElasticsearchService _elasticService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApplicationInsightService> _log;

        public ApplicationInsightService(IElasticsearchService elasticService, ILogger<ApplicationInsightService> log)
        {
            _httpClient = new HttpClient();
            _elasticService = elasticService;
            _log = log;
        }
        private ApplicationInsightResponse GetLogApp()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Add("x-api-key", Environment.GetEnvironmentVariable("ApiKey")!);
            
            var response = _httpClient.GetAsync(UrlApplicationInsights).Result;

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

            if (log == null)
                _log.LogError("Falha ao realizar conversão das métricas do aplication insight");
            else
            {
                var resultElastic = await _elasticService.SendToElastic(log, IndexAI);

                if (!resultElastic)
                    _log.LogError("Falha ao salvar metricas do aplication insight no elasticsearch");
            }
        }

    }
}