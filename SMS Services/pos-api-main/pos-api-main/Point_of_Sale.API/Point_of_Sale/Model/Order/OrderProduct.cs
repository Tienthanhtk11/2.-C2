namespace Point_of_Sale.Model.Order
{
    public class OrderProduct
    {
        public long id { get; set; }
        public long order_id { get; set; }
        public long product_id { get; set; }
        public string? barcode { get; set; }
        public string? name { get; set; }
        public string? unit_code { get; set; } // mã đon tính
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public long warehouse_id { get; set; }
        public double price { get; set; }
        public double sale_price { get; set; } 
        public double quantity { get; set; }
        public long product_warehouse_id { get; set; } 
        
    }
}
