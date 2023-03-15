namespace Point_of_Sale.Model
{
    public class SearchBase
    {
        public int page_number { get; set; }
        public int page_size { get; set; }
        public int? province_code { get; set; }
        public int? type { get; set; }
        public string? keyword { get; set; }
        public DateTime? start_date { set; get; }
        public DateTime? end_date { set; get; }
        public int? is_active { get; set; }
    }
}
