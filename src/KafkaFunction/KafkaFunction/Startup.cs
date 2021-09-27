
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
            options.InstanceName = "KafkaMessage";
            options.Configuration = "kafkaredisvaivoa.redis.cache.windows.net:6380,password=1vwDwc3owbVeMgGyg5jSD2f6kB1xe2Wb1Ai1ovyPnLY=,ssl=True,abortConnect=False";
        });
    }
}
