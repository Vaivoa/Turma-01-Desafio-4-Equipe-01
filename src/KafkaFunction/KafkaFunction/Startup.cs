
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

public class Startup : IWebJobsStartup
{   

    public void Configure(IWebJobsBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = Environment.GetEnvironmentVariable("InstanceName");
            options.Configuration = Environment.GetEnvironmentVariable("RedisConnection");
        });
    }
}
