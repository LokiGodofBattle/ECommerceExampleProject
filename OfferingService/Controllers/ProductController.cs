using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ECommerceExampleProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpPost("createOffering")]
        public void PostCO([FromBody] Offering offering)
        {
            OfferingsData.offerings.Add(offering);
            OfferingsData.writeJson();
        }

        [HttpPost("createProduct")]
        public void Post([FromBody] Product product)
        {
            OfferingsData.products.Add(product);
            OfferingsData.writeJson();
        }
    
    
        // GET: api/<CustomerController>
        [HttpGet("getAllProducts")]
        public string Get()
        {
            return JsonConvert.SerializeObject(OfferingsData.products);
        }


        [HttpDelete("deleteProduct/{id}")]
        public void Delete(int id)
        {
            Product product = OfferingsData.products.First(p => p.id == id);
            OfferingsData.products.Remove(product);
            OfferingsData.writeJson();
        }

        [HttpDelete("deleteOffering/{id}")]
        public void DeleteO(int id)
        {
            Offering offering = OfferingsData.offerings.First(p => p.id == id);
            OfferingsData.offerings.Remove(offering);
            OfferingsData.writeJson();
        }

    }
    
    
}
