using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ILoggerSerilog = Serilog.ILogger;

namespace LogsVaivoa.Services
{
    public class HttpLogRequest
    {
        private readonly ILoggerSerilog _log;

        public HttpLogRequest(ILoggerSerilog log)
        {
            _log = log;
        }
        public static string GetLogApp()
        {
            var URL = Environment.GetEnvironmentVariable("UrlApplicationInsights")!;
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("x-api-key", Environment.GetEnvironmentVariable("ApiKey")!);
            
            var response = client.GetAsync(URL).Result;

            return response.IsSuccessStatusCode switch
            {
                true => response.Content.ReadAsStringAsync().Result,
                _ => null
            };
        }

        public void SendMetricFunctionToElk()
        {
            var result = GetLogApp();

            if (result != null)
            {
                _log.Information(result, "result");
            }
        }

        

        public class Root
        {
            public class Table
            {
                public string name { get; set; }
                public List<List<object>> rows { get; set; }
            }
            public List<Table> tables { get; set; }
        }


    }
}