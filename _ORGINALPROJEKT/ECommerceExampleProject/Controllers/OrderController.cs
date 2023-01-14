using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerceExampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // GET: api/<CustomerController>
        [HttpGet("getAllOrders")]
        public string Get()
        {
            return JsonConvert.SerializeObject(OrderData.orders);
        }


        [HttpDelete("deleteOrder/{id}")]
        public void Delete(int id)
        {
            Order order = OrderData.orders.First(o => o.id == id);
            OrderData.orders.Remove(order);
            OrderData.writeJson();
        }

    }
}
