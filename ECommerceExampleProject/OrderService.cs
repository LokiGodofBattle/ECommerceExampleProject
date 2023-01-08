using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using ECommerceExampleProject;

public static class OrderData{
    public static List<Order> orders = loadOrders();

    public static List<Order> loadOrders()
    {
        string filePath = "orders.txt";

        string jsonString = File.ReadAllText(filePath);

        if (jsonString != null && jsonString != "") return JsonConvert.DeserializeObject<List<Order>>(jsonString);
        else return new List<Order>();
    }

    public static void writeJson()
    {
        File.WriteAllText("orders.txt", JsonConvert.SerializeObject(orders));
    }
}

class OrderService
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
                BootstrapServers = "192.168.178.141:9092",
                GroupId = "order",
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
                        OrderData.orders.Add(JsonConvert.DeserializeObject<Order>(message.Value));
                        OrderData.writeJson();
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