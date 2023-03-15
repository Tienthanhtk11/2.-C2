namespace Point_of_Sale.Model
{
    public class OrderSearch
    {
        public long? customer_id { get; set; }
        public long? pos_id { get; set; }
        public long? staff_id { get; set; }
        public long warehouse_id { get; set; }
        public long? sales_session_id { get; set; }
        public string? keyword { get; set; }
        public string? staff_name { get; set; } 
        public DateTime? start_date { set; get; }
        public DateTime? end_date { set; get; }
        public int page_number { set; get; } = 1;
        public int page_size { set; get; } = 30;
   
    }
}
