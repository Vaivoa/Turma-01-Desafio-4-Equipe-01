using System;
using System.Threading;
using Confluent.Kafka;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace KafkaFunction
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
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
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };



            try
            {
                using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumer.Subscribe(nomeTopic);

                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {

                            var cr = consumer.Consume(cts.Token);


                            log.LogInformation($"Offset: {cr.Offset}  OffsetPartition: {cr.TopicPartitionOffset}");

                            log.LogInformation(
                                $"Mensagem lida: {cr.Message.Value}");

                        }
                        catch (OperationCanceledException)
                        {
                            consumer.Close();
                            log.LogWarning("Cancelada a execução do Consumer...");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError($"Exceção: {ex.GetType().FullName} | " +
                             $"Mensagem: {ex.Message}");
            }

        }
    }
}
