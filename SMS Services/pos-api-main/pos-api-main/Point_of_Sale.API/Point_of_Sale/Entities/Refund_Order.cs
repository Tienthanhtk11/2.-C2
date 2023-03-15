using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("refund_order")]
    public class Refund_Order : IAuditableEntity
    {
        public long refund_id { get; set; }
        public long order_id { get; set; }// đơn hàng trả
        public string order_code { get; set; }//mã đơn hàng trả
        public double total_refund_money { set; get; }

    }
}
