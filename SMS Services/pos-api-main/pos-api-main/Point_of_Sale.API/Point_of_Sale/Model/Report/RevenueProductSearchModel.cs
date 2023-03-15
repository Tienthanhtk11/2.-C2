namespace Point_of_Sale.Model.Report
{
    public class RevenueProductSearchModel
    { 
        public long warehouse_id { get; set; }
        public string? category_code { get; set; }
        public string? keyword { get; set; } 
        public DateTime? start_date { set; get; }
        public DateTime? end_date { set; get; } 
    }
}
