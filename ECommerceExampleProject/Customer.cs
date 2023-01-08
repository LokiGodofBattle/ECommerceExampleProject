namespace ECommerceExampleProject
{
    public class Customer
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public List<Offering> shoppingBasket { get; set; }
    }
}
