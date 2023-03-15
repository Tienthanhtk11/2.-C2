namespace Point_of_Sale.Model.Request
{
    public class RequestModel
    {
        public long id { get; set; }
        public long userAdded { set; get; }
        public long? userUpdated { set; get; }
        public long partner_id { get; set; } 
        public string? code { get; set; } 
        public string? shipper  { get; set; }  // nguoi giao
        public long warehouse_id { get; set; } 
        public string? note { get; set; }
        public string? content { get; set; }
        public DateTime transfer_date { get; set; }
        public string? delivery_address { get; set; }
        public byte status_id { get; set; }// 0 là chưa xử lý 1 đã xử lý
        public byte type { get; set; }
        public List<Request_ProductModel>? Products { get; set; }
    }

    public class RequestViewModel
    {
        public long id { get; set; } 
        public string? code { get; set; } 
        public long warehouse_id { get; set; }
        public string? warehouse_name { get; set; }
        public string? note { get; set; }
        public string? content { get; set; }
        public DateTime transfer_date { get; set; }
        public string? delivery_address { get; set; }
        public string? shipper { get; set; }  // nguoi giao

        public byte status_id { get; set; }// 0 là chưa xử lý 1 đã xử lý
        public byte type { get; set; }

    }
}
