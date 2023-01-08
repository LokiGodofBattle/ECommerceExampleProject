
    public class Offering
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float TotalPrice { get; set; }
        public DateTime EffectiveDate  { get; set; }
        public string Status { get; set; }
        public Product Product { get; set; }
}

