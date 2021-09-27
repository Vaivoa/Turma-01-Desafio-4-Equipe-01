
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Startup : IWebJobsStartup
{   

    public void Configure(IWebJobsBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = "test";
            options.Configuration = "localhost:6379";
        });
    }
}
