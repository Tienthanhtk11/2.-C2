using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{ 
    [Table("product_warehouse_price_history")]
    public class Product_Warehouse_Price_History : IAuditableEntity// lịch sử thay đổi giá
    { 
        public long product_id { get; set; }
        public long product_warehouse_id { get; set; } 
        public double import_price_old { get; set; } // giá nhập
        public double price_old { get; set; }// giá bán
        public double sale_price_old { get; set; }// giá khuyến mại
        public double import_price { get; set; } // giá nhập mới
        public double price { get; set; }// giá bán mới
        public double sale_price { get; set; }// giá khuyến mại mới

    }
}
