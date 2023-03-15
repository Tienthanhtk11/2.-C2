namespace Point_of_Sale.Model.Receipt
{
    public class SearchReceipt : SearchBase
    {
        public long warehouse_id { set; get; } 
        public long partner_id { set; get; }
        public byte type { set; get; }
        public byte? status_id { set; get; }
        public DateTime? start_date { set; get; }
        public DateTime? end_date { set; get; }

    }
}
