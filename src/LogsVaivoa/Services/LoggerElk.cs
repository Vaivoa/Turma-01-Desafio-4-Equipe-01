using System;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using ILoggerSerilog = Serilog.ILogger;

namespace LogsVaivoa.Services
{
    public class LoggerElk
    {
        private readonly ILoggerSerilog _configLogger;
        public LoggerElk(ILoggerFactory loggerFactory)
        {
            _configLogger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo
                .Elasticsearch(new ElasticsearchSinkOptions(new Uri(Environment.GetEnvironmentVariable("ElkConnection")!))
                {
                    AutoRegisterTemplate = true
                }).CreateLogger();
        }

        public ILoggerSerilog CreateLogger()
        {
            return _configLogger;
        }
    }
}