using LogsVaivoa.Infra;
using LogsVaivoa.Interface;
using LogsVaivoa.repository;
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

            builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();

            builder.Services.AddScoped<ILogRepository, LogRepository>();
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IApplicationInsightService, ApplicationInsightService>();
            builder.Services.AddSingleton<IDbContext, DbContext>();
            builder.Services.AddLogging();


        }
    }
}