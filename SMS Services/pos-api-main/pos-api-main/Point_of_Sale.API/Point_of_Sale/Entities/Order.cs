using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Entities
{
    [Table("order")]
    public class Order : IAuditableEntity
    {
        public string code { get; set; }   // mã đơn hàng
        public string? note { get; set; }   // mã đơn hàng
        public long pos_id { get; set; } = 0; // id máy pos
        public long warehouse_id { get; set; }   // id điểm bán
        public long sales_session_id { get; set; } // id phiên bán hàng
        public long customer_id { get; set; }
        public long? voucher_id { get; set; }  //mã voucher
        public double voucher_cost { get; set; }  //tổng tiền giảm qua voucher
        public double sale_cost { get; set; }  //tổng tiền giảm qua sản phẩm
        public double product_total_cost { get; set; }  //tổng tiền hàng
        public byte status_id { get; set; }// 0 đã thanh toán, 2 đơn đã hoàn
        public byte payment_type { get; set; }  //0 cash, 1 card, 2 internet banking
        public double total_amount { get; set; }  //tổng tiền cần thanh toán = Tiền hàng - tiền discount voucher - tien giam gia sp - tien giam qua member_point
        public string? bank_account { get; set; }
        public string? transaction_code { get; set; }
        public int type { get; set; } = 0;// 0 mua hàng ,1 hoàn hàng
        public int member_point { get; set; } = 0; // so diem quy doi tu member_point cua khach hang
        public double member_point_value {get;set;} = 0; // so tien quy doi tu member_point
    }
}
