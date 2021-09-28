using System;
using LogsVaivoa.Interface;
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

            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<ApplicationInsightService>();
            builder.Services.AddSingleton<IDbContext, DbContext>();
            builder.Services.AddLogging();

        }
    }
}