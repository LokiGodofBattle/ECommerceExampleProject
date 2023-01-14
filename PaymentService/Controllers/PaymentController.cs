using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


[Route("api/[controller]")]
[ApiController]
public class PlaymentController : ControllerBase
{
    [HttpPost("payOrder/{id}")]
    public void Post(int id)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = GlobalData.ip
        };

        // Create a new producer
        using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
        {
            var result = producer.ProduceAsync("payment", new Message<Null, string> { Value = ""+id }).Result;
            producer.Flush();
        }

        PaymentData.payments.Add(new Payment { orderId = id, paymentDate = DateTime.Now });
        PaymentData.writeJson();
    }
}

