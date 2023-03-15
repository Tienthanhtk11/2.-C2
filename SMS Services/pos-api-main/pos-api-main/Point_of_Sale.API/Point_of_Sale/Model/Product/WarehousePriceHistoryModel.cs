using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Product
{
    public class WarehousePriceHistoryModel : IAuditableEntity
    {
        public long product_id { get; set; }
        public string product_name { get; set; }
        public string barcode { get; set; }
        public long product_warehouse_id { get; set; }
        public long warehouse_id { get; set; }
        public string warehouse_name { get; set; }
        public string unit_code { get; set; } 
        public string user_change { get; set; }
        public double import_price_old { get; set; } // giá nhập
        public double price_old { get; set; }// giá bán
        public double sale_price_old { get; set; }// giá khuyến mại
        public double import_price { get; set; } // giá nhập mới
        public double price { get; set; }// giá bán mới
        public double sale_price { get; set; }// giá khuyến mại mới
    }
}
