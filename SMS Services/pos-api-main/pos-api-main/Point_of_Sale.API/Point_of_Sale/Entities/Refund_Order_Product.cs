using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("refund_order_product")]
    public class Refund_Order_Product:IAuditableEntity
    {
        public long refund_id { get; set; }
        public long refund_order_id { get; set; }
        public long product_id { get; set; }
        public long product_warehouse_id { get; set; }
        public string? unit_code { get; set; } // mã đon tính
        public string? packing_code { get; set; }// mã quy cách đóng gói
        public long warehouse_id { get; set; }
        public double price { get; set; }
        public double quantity { get; set; }
        public string product_batch_number { get; set; }

    }
}
