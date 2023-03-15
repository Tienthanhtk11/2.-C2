using Point_of_Sale.Entities;

namespace Point_of_Sale.Model.Report
{
    public class Sale_Session_Report : IAuditableEntity
    {
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public long staff_id { get; set; }
        public long warehouse_id { get; set; }
        public string? note { get; set; }
        public string? warehouse_name { get; set; }
        public double opening_cash { get; set; } //tiền mặt lúc bắt đầu ca
        public double session_cash { get; set; } // tiền mặt thu trong ca
        public double closing_total_proceeds { get; set; }// Tổng tiền thu trong ca (bằng tổng tiền theo đơn)
        public string staff_name { get; set; }
        public string code { get; set; } = "";
        public double closing_card { get; set; } = 0; // tiền nhận qua hình thức thanh toán bằng thẻ
        public double closing_online_transfer { get; set; } = 0;   // tiền nhận qua hình thức chuyển khoản ngân hàng
        public byte? status { get; set; } // 0 la dang hoat dong, 1 la da ket thuc
        public int member_point { get; set; }  //số điểm member_point sử dụng
        public double member_point_value { get; set; }  //tổng tiền giảm qua member_point
    }
}
