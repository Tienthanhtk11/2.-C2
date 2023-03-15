namespace Point_of_Sale.Model.Receipt
{
    public class ReceiptModel
    {
        public long id { get; set; }
        public long partner_id { get; set; }
        public long userAdded { set; get; }
        public long? userUpdated { set; get; }
        public string? code { get; set; }
        public long request_id { get; set; } 
        public long warehouse_id { get; set; } 
        public string? note { get; set; }
        public string? content { get; set; }
        public DateTime transfer_date { get; set; }
        public string? delivery_address { get; set; }
        public byte status_id { get; set; }
        public byte type { get; set; } 
        public List<Receipt_ProductModel>? Products { set; get; }
    }
    public class ReceiptViewModel
    {
        public long id { get; set; }
        public long partner_id { get; set; }
        public string partner_name {set; get; }
        public string? code { get; set; }
        public long request_id { get; set; }
        public string? user_name { set; get; }
        public DateTime? dateUpdate { set; get; }
        public long warehouse_id { get; set; }
        public string warehouse_name { set; get; }
        public string? note { get; set; }
        public string? content { get; set; }
        public DateTime transfer_date { get; set; }
        public string? delivery_address { get; set; }
        public byte status_id { get; set; }
        public byte type { get; set; }
        public double total_amount { set; get; }// tổng tiền phiếu

    }
}
