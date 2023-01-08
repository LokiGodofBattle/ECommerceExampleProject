using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using System.Net.Http;




public static class GlobalData{
    public static string offerings = "";
}

class CustomerService
{
    public static void StartService()
    {
        // Create a new consumer configuration
            
            // Create a new thread to run the consumer
            Thread consumerThread = new Thread(() =>
            {
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = "192.168.178.141:9092",
                    GroupId = "customer",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                // Create a cancellation token to stop the consumer when needed
                CancellationTokenSource cts = new CancellationTokenSource();

                // Create a new consumer
                using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
                {
                    // Subscribe to the topic
                    consumer.Subscribe("offerings_data");

                    try
                    {
                        // Run the consumer loop
                        while (true)
                        {
                            // Poll for new messages
                            var message = consumer.Consume(cts.Token);
                            GlobalData.offerings = message.Message.Value;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Stop the consumer when the cancellation token is canceled
                        consumer.Close();
                    }
                }
            });

            // Start the consumer thread
            consumerThread.Start();
            
    }
}

