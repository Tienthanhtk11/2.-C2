using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Refund
{
    public class RefundOrderModel : IAuditableEntity
    {
        public long refund_id { get; set; }
        public long order_id { get; set; }// đơn hàng trả
        public string order_code { get; set; }//mã đơn hàng trả
        public double total_refund_money { set; get; }
        public List<RefundProduct>? products { get; set; }
    }
}
