namespace Point_of_Sale.Model.Report
{
    public class RevenueProductModel
    {

        public double total_amount { get; set; }// tổng tiền hàng
        public double total_revenue { get; set; }// tổng doanh thu = tổng tiền hàng - (tổng khuyến mại +  tổng giảm giá)
        public double total_voucher { get; set; }// tổng khuyến mại
        public double total_sale { get; set; }// tổng giảm giá
        public List<RevenueProducts> products { get; set; }
    }

    public class RevenueProducts
    {
        public long product_warehouse_id { get; set; }
        public long product_id { get; set; }
        public long warehouse_id { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
        public string? category_name { get; set; }
        public string? category_code { get; set; }
        public string? unit_code { get; set; }
        public double quantity { get; set; }
        public double price { get; set; } // giá trung bình
        public double sale_price { get; set; } // giảm giá trung bình
        public double amount { get; set; } //tổng tiền hàng
        public double sale_amount { get; set; } //tổng tiền hàng giảm giá
        public double revenue { get; set; } // doanh thu
        
    }
}
