namespace ECommerceExampleProject
{
    public class Customer
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Offering> shoppingBasket { get; set; }
    }
}
