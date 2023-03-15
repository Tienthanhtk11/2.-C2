using Point_of_Sale.Model.Order;

namespace Point_of_Sale.Model.Refund
{
    public class RefundSearchOrderModel
    {
        public long id { get; set; }
        public string code { get; set; }   // mã đơn hàng
        public long pos_id { get; set; } = 0; // id máy pos
        public long warehouse_id { get; set; }   // id điểm bán
        public long sales_session_id { get; set; } // id phiên bán hàng
        public long customer_id { get; set; }
        public string customer_name { get; set; }
        public string? staff_name { get; set; }
        public int member_point { get; set; }
        public string customer_phone { get; set; }
        public long? voucher_id { get; set; }  //mã voucher
        public double voucher_cost { get; set; }  //tổng tiền giảm qua voucher
        public double sale_cost { get; set; }  //tổng tiền giảm qua sản phẩm 
        public double product_total_cost { get; set; }  //tổng tiền hàng
        public byte status_id { get; set; }
        public byte payment_type { get; set; }  //0 cash, 1 card, 2 internet banking
        public double total_amount { get; set; }  //tổng tiền cần thanh toán = Tiền hàng + tiền ship + tiền lưu kho + tiền bảo hiểm - tiền discount voucher - tiền flashsale 
        public DateTime dateAdded { get; set; }
        public List<OrderProduct>? products { get; set; }
       
    } 
    public class OrderCheckData {
        public int code { get; set; }// 1: chưa Được hoàn, 2: đã hoàn 1 phần,3: đã hoàn hết, 4: quá hạn,
        public RefundSearchOrderModel data { get; set; }
    }
}
