namespace Point_of_Sale.Model.SaleSession
{
    public class SaleSessionSearch
    {
        public byte? status { get; set; } // 0 la dang hoat dong, 1 la da ket thuc
        public string? keyword { get; set; }
        public DateTime? start_date { set; get; }
        public DateTime? end_date { set; get; }
        public int page_number { set; get; } = 1;
        public int page_size { set; get; } = 20;
        public long? userAdded { set; get; }
        public long? warehouse_id { set; get; }
    }
}
