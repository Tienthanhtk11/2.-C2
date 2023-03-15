using Point_of_Sale.Entities;
using Point_of_Sale.Model.Order;

namespace Point_of_Sale.Model.Refund
{
    public class RefundModel : IAuditableEntity
    {
        public string code { set; get; }
        public string? note { set; get; }
        public double product_total_cost { get; set; }  //tổng tiền hàng 
        public double voucher_cost { get; set; }  //tổng tiền voucher
        public double product_sale_cost { get; set; }  //tổng tiền giảm qua sp
        public int member_point { get; set; }  //Số point áp dụng vào đơn
        public double total_refund_money { set; get; } // số tiền phải trả khách = tổng tiền hàng - tiền voucher - tiền giảm qua sp - tiền giảm bằng số member point
        public double sale_cost { set; get; }
        public long order_id { set; get; }// đơn hàng âm
        public long customer_id { get; set; }
        public long warehouse_id { get; set; }
        public string customer_name { get; set; }
        public string? staff_name { get; set; }
        public string customer_phone { get; set; }
        public long sales_session_id { get; set; } 
        public List<RefundOrderModel>? refund_order { get; set; } 
    }
}
