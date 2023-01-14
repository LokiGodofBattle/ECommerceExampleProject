public class Order
{
    public int id { get; set; }
    public int cid { get; set; }
    public List<Offering> basketItems { get; set; }
    public DateTime orderDate { get; set; }
    public string status { get; set; }
}
