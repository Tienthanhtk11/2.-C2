using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("order_detail")]
    public class Order_Detail : IAuditableEntity
    {
        public long order_id { get; set; }
        public long product_id { get; set; }
        public long product_warehouse_id { get; set; }
        public string? unit_code { get; set; } // mã đon tính
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public long warehouse_id { get; set; }
        public double price { get; set; }
        public double sale_price { get; set; } = 0; // giá khuyến mại
        public double quantity { get; set; }
        
    }
}
