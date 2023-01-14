using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using ECommerceExampleProject;

public static class OfferingsData
{
    public static List<Offering> offerings = loadOfferings();
    public static List<Product> products = loadProducts();

    public static List<Offering> loadOfferings()
    {
        string filePath = "offerings.txt";

        string jsonString = File.ReadAllText(filePath);

        if (jsonString != null && jsonString != "") return JsonConvert.DeserializeObject<List<Offering>>(jsonString);
        else return new List<Offering>();
    }

    public static List<Product> loadProducts()
    {
        string filePath = "products.txt";

        string jsonString = File.ReadAllText(filePath);

        if (jsonString != null && jsonString != "") return JsonConvert.DeserializeObject<List<Product>>(jsonString);
        else return new List<Product>();
    }
    public static void writeJson()
    {
        File.WriteAllText("offerings.txt", JsonConvert.SerializeObject(offerings));
        File.WriteAllText("products.txt", JsonConvert.SerializeObject(products));
    }

    public static void readJson()
    {
        offerings = loadOfferings();
       products = loadProducts();
    }
}

class OfferingsService
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
                    GroupId = "offerings",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                // Create a cancellation token to stop the consumer when needed
                CancellationTokenSource cts = new CancellationTokenSource();


                using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
                {
                    // Subscribe to the topic
                    consumer.Subscribe("offerings");

                    try
                    {
                        // Run the consumer loop
                        while (true)
                        {
                            // Poll for new messages
                            var message = consumer.Consume(cts.Token);
                            if(message.Message.Value == "cno"){
                                var producerConfig = new ProducerConfig
                                {
                                    BootstrapServers = GlobalData.ip
                                };

                                // Create a new producer
                                using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build()){
                                    OfferingsData.readJson();
                                    var result = producer.ProduceAsync("offerings_data", new Message<Null, string> { Value = JsonConvert.SerializeObject(OfferingsData.offerings) }).Result;
                                    producer.Flush();
                                }
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