using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(Startup))]
namespace KafkaFunction
{
    public class KafkaFunction
    {
        private readonly IDistributedCache _cache;
        public KafkaFunction(IDistributedCache cache)
        {
            _cache = cache;
        }

        [FunctionName("KafkaFunction")]
        public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            string bootstrapServers = "localhost:9092/";
            string nomeTopic = "CADASTRO_CONTA_CORRENTE_ATUALIZADO";
            log.LogInformation($"BootstrapServers = {bootstrapServers}");
            log.LogInformation($"Topic = {nomeTopic}");
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = $"{nomeTopic}-group-0",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(10));



            var messages = new List<(string, string)>();


            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(nomeTopic);

            while (!cts.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(cts.Token);

                    messages.Add((cr.Message.Key, cr.Message.Value));

                    log.LogInformation($"Offset: OffsetPartition: {cts.IsCancellationRequested}");

                    log.LogInformation(
                        $"Mensagem lida: {cr.Message.Value}");

                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                    log.LogWarning("Cancelada a execução do Consumer...");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                cts.CancelAfter(TimeSpan.FromSeconds(2));
            }




            foreach (var message in messages)
            {
                var (key, value) = message;
                _cache.SetString($"{key}:{DateTime.Now:dd/MM/yyyy HH:mm:ss}", value);
            }


        }
    }
}
