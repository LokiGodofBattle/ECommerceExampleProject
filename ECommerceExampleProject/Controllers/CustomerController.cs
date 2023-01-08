using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerceExampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // GET: api/<CustomerController>
        [HttpGet("getAllOfferings")]
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

            while (GlobalData.offeringsString == "")
            {
                // Wait for 500 milliseconds
                Thread.Sleep(500);
            }


            return GlobalData.offeringsString;
        }

        // GET api/<CustomerController>/5
        [HttpGet("getShoppingBasketItems/{cid}")]
        public string Get(int cid)
        {
            Customer customer = GlobalData.customers.First(value => value.id == cid);

            return JsonConvert.SerializeObject(customer.shoppingBasket);
        }

        [HttpPost("addToCart/{cid}/{oid}")]
        public void Post(int cid, int oid)
        {
            Customer customer = GlobalData.customers.First(value => value.id == cid);
            Offering offering = GlobalData.offerings.First(value => value.id == oid);
            customer.shoppingBasket.Add(offering);
            GlobalData.writeJson();
            GlobalData.readJson();
        }

        [HttpDelete("removeFromCart/{cid}/{oid}")]
        public void Delete(int cid, int oid)
        {
            Customer customer = GlobalData.customers.First(value => value.id == cid);
            Offering offering = customer.shoppingBasket.First(value => value.id == oid);
            customer.shoppingBasket.Remove(offering);
            GlobalData.writeJson();
            GlobalData.readJson();
        }

        // PUT api/<CustomerController>/5
        [HttpPost("createCustomer/{id}/{name}")]
        public void Post(int id, string name)
        {
            GlobalData.customers.Add(new Customer { id = id, name = name, shoppingBasket = new List<Offering>() });
            GlobalData.writeJson();
            GlobalData.readJson();
        }
    }
}
