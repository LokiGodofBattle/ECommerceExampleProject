using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerceExampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // GET: api/<CustomerController>
        [HttpGet]
        public string Get()
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "192.168.178.141:9092"
            };

            // Create a new producer
            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                var result = producer.ProduceAsync("offerings", new Message<Null, string> { Value = "cno" }).Result;
                producer.Flush();
            }

            while (GlobalData.offerings == "")
            {
                // Wait for 500 milliseconds
                Thread.Sleep(500);
            }


            return GlobalData.offerings;
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
