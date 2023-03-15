using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("refund")]
    public class Refund : IAuditableEntity
    {
       // public string order_code { set; get; }
        public string code { set; get; }
        public string? note { set; get; }
        public double product_total_cost { get; set; }  //tổng tiền hàng 
        public double voucher_cost { get; set; }  //tổng tiền voucher
        public double product_sale_cost { get; set; }  //tổng tiền giảm qua sp
        public int member_point { get; set; }  //Số point áp dụng vào đơn
        public double total_refund_money { set; get; } // số tiền phải trả khách = tổng tiền hàng - tiền voucher - tiền giảm qua sp - tiền giảm bằng số member point
        public long warehouse_id { get; set; }
        public long customer_id { set; get; }
        public long order_id { set; get; }// đây là Id của hóa đơn âm được sinh ra
                                          // - hóa đơn âm tiền để tránh kiểm kê giao ca thu nhân giải trình

    }
}
