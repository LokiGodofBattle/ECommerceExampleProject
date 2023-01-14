using Confluent.Kafka;
using Newtonsoft.Json;
using System.IO;

class OrderArchivingService
{

    public static void StartService()
    {
        // Create a new consumer configuration

        // Create a new consumer


        // Create a new thread to run the consumer
        Thread consumerThread = new Thread(() =>
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = GlobalData.ip,
                GroupId = "archive",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            // Create a cancellation token to stop the consumer when needed
            CancellationTokenSource cts = new CancellationTokenSource();


            using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
            {
                // Subscribe to the topic
                consumer.Subscribe("order_data");

                try
                {
                    // Run the consumer loop
                    while (true)
                    {
                        // Poll for new messages
                        var message = consumer.Consume(cts.Token);
                        Order order = JsonConvert.DeserializeObject<Order>(message.Value);

                        using (StreamWriter sw = File.AppendText("orderArchive.txt"))
                        {
                            sw.WriteLine(order.id + ", " + order.cid + ", " + order.orderDate);
                        }
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