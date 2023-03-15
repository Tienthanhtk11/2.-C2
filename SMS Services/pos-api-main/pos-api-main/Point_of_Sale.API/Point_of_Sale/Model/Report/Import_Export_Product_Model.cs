namespace Point_of_Sale.Model.Report
{
    public class Import_Export_Product_Model
    {
        public DateTime export_date { get; set; }
        public string parent_code { get; set; }
        public string date { get; set; }
        public string partner_code { get; set; }
        public string warehouse_code { get; set; }
        public string partner_name { get; set; }
        public string warehouse_name { get; set; }
        public long? customer_id { get; set; }
        public long? partner_id { get; set; }
        public long product_id { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string product_unit { get; set; }
        public double quantity { get; set; }
        public double price { get; set; }
        public double total_cost { get; set; } = 0;
        public long warehouse_id { get; set; }
        public long? warehouse_destination_id { get; set; }
        public long? order_id { get; set; }
        public string warehouse_destination_name { get; set; }
        public byte type { get; set; }
    }
}
