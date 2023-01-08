using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

class OfferingsService
    {
        public static Offering[] loadOfferings(){
            string filePath = "offerings.txt";

            string jsonString = File.ReadAllText(filePath);

            // Deserialize the JSON string into an object
            Offering[] jsonObject = JsonConvert.DeserializeObject<Offering[]>(jsonString);

            return jsonObject;
        }

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
                                    BootstrapServers = "192.168.178.141:9092"
                                };

                                // Create a new producer
                                using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build()){
                                    var result = producer.ProduceAsync("offerings_data", new Message<Null, string> { Value = JsonConvert.SerializeObject(loadOfferings()) }).Result;
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