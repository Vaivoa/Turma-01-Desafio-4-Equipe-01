using System;
using LogsVaivoa.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace LogsVaivoa
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {

            builder.Services.AddScoped(elasticsearchService =>
                new ElasticsearchService(Environment.GetEnvironmentVariable("ElkConnection")));

            builder.Services.AddScoped<LogService>();
            builder.Services.AddScoped<ApplicationInsightService>();
            builder.Services.AddLogging();

        }
    }
}