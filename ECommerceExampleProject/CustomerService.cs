using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using System.Net.Http;
using ECommerceExampleProject;
using Newtonsoft.Json;

public static class GlobalData{

    public static string offeringsString = "";
    public static List<Offering> offerings = new List<Offering>();
    public static string ip = "192.168.178.141:9092";

    public static List<Customer> customers = loadCustomers();

    public static List<Customer> loadCustomers()
    {
        string filePath = "customers.txt";

        string jsonString = File.ReadAllText(filePath);

        if(jsonString != null && jsonString != "") return JsonConvert.DeserializeObject<List<Customer>>(jsonString);
        else return new List<Customer>();
    }

    public static void readJson()
    {
        customers = loadCustomers();
    }

    public static void writeJson()
    {
        File.WriteAllText("customers.txt", JsonConvert.SerializeObject(customers));
    }

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
                    BootstrapServers = GlobalData.ip,
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
                            GlobalData.offeringsString = message.Value;
                            GlobalData.offerings = JsonConvert.DeserializeObject<List<Offering>>(message.Value);
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

