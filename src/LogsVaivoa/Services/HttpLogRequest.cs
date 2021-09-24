using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public static Root GetLogApp()
        {
            var URL = Environment.GetEnvironmentVariable("UrlApplicationInsights")!;
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("x-api-key", Environment.GetEnvironmentVariable("ApiKey")!);
            
            var response = client.GetAsync(URL).Result;

            return response.IsSuccessStatusCode switch
            {
                true => JsonConvert.DeserializeObject<Root>(response.Content.ReadAsStringAsync().Result),
                _ => null
            };
        }

        public void SendMetricFunctionToElk()
        {
            var result = GetLogApp();

            if (result != null)
            {
                var log = result.MapToLog();
            }
        }

        

        public class Root
        {
            public class Table
            {
                public string name { get; set; }
                public List<List<string>> rows { get; set; }
            }
            public List<Table> tables { get; set; }


            public LogApplicationInsight MapToLog()
            {
                return tables[0].rows.Select(i => 
                    new LogApplicationInsight(i[0], i[1], i[2], i[3], i[4], i[5])).First();
            }
        }


        public class LogApplicationInsight
        {
            public LogApplicationInsight(string timestamps, string id, string operationName, string success, string resultCode, string duration)
            {
                Timestamps = timestamps;
                Id = id;
                OperationName = operationName;
                Success = success;
                ResultCode = resultCode;
                Duration = duration;
            }
            public string Timestamps { get; set; }
            public string Id { get; set; }
            public string OperationName { get; set; }
            public string Success { get; set; }
            public string ResultCode { get; set; }
            public string Duration { get; set; }
        }


    }
}