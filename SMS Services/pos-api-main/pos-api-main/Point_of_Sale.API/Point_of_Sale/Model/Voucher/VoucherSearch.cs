namespace Point_of_Sale.Model
{
    public class VoucherSearch
    {
       
        public byte? status { get; set; }
        public string? code { get; set; }
        public DateTime? start_date { set; get; }
        public DateTime? end_date { set; get; }
        public int page_number { set; get; } = 1;
        public int page_size { set; get; } = 20;
        public long? userAdded { set; get; }
    }
}
